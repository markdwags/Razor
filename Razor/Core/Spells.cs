#region license

// Razor: An Ultima Online Assistant
// Copyright (C) 2020 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Assistant
{
    public class Spell
    {
        public enum SpellFlag
        {
            None = '?',
            Beneficial = 'B',
            Harmful = 'H',
            Neutral = 'N'
        }

        readonly public SpellFlag Flag;
        readonly public int Circle;
        readonly public int Number;
        readonly public string WordsOfPower;
        readonly public string[] Reagents;

        private static Timer m_UnflagTimer;

        public Spell(char flag, int n, int c, string power, string[] reags)
        {
            Flag = (SpellFlag) flag;
            Number = n;
            Circle = c;
            WordsOfPower = power;
            Reagents = reags;
        }

        public int Name
        {
            get
            {
                if (Circle <= 8) // Mage
                    return 3002011 + ((Circle - 1) * 8) + (Number - 1);
                else if (Circle == 10) // Necr
                    return 1060509 + Number - 1;
                else if (Circle == 20) // Chiv
                    return 1060585 + Number - 1;
                else if (Circle == 40) // Bush
                    return 1060595 + Number - 1;
                else if (Circle == 50) // Ninj
                    return 1060610 + Number - 1;
                else if (Circle == 60) // Elfs
                    return 1071026 + Number - 1;
                else
                    return -1;
            }
        }

        public override string ToString()
        {
            return String.Format("{0} (#{1})", Language.GetString(this.Name), GetID());
        }

        public int GetID()
        {
            return ToID(Circle, Number);
        }

        public int GetHue(int def)
        {
            if (Config.GetBool("ForceSpellHue"))
            {
                switch (Flag)
                {
                    case SpellFlag.Beneficial:
                        return Config.GetInt("BeneficialSpellHue");
                    case SpellFlag.Harmful:
                        return Config.GetInt("HarmfulSpellHue");
                    case SpellFlag.Neutral:
                        return Config.GetInt("NeutralSpellHue");
                    default:
                        return def;
                }
            }
            else
            {
                return def;
            }
        }

        public void OnCast(PacketReader p)
        {
            Cast();
            Client.Instance.SendToServer(p);
        }

        public void OnCast(Packet p)
        {
            Cast();
            Client.Instance.SendToServer(p);
        }

        private void Cast()
        {
            if (Config.GetBool("SpellUnequip") && Client.Instance.AllowBit(FeatureBit.UnequipBeforeCast))
            {
                Item pack = World.Player.Backpack;
                if (pack != null)
                {
                    // dont worry about uneqipping RuneBooks or SpellBooks
                    Item item = World.Player.GetItemOnLayer(Layer.RightHand);
#if DEBUG
                    if (item != null && item.ItemID != 0x22C5 && item.ItemID != 0xE3B && item.ItemID != 0xEFA &&
                        !item.IsVirtueShield)
#else
					if ( item != null && item.ItemID != 0x22C5 && item.ItemID != 0xE3B && item.ItemID != 0xEFA )
#endif
                    {
                        DragDropManager.Drag(item, item.Amount);
                        DragDropManager.Drop(item, pack);
                    }

                    item = World.Player.GetItemOnLayer(Layer.LeftHand);
#if DEBUG
                    if (item != null && item.ItemID != 0x22C5 && item.ItemID != 0xE3B && item.ItemID != 0xEFA &&
                        !item.IsVirtueShield)
#else
					if ( item != null && item.ItemID != 0x22C5 && item.ItemID != 0xE3B && item.ItemID != 0xEFA )
#endif
                    {
                        DragDropManager.Drag(item, item.Amount);
                        DragDropManager.Drop(item, pack);
                    }
                }
            }

            for (int i = 0; i < Counter.List.Count; i++)
                ((Counter) Counter.List[i]).Flag = false;

            if (Config.GetBool("HighlightReagents"))
            {
                for (int r = 0; r < Reagents.Length; r++)
                {
                    for (int i = 0; i < Counter.List.Count; i++)
                    {
                        Counter c = (Counter) Counter.List[i];
                        if (c.Enabled && c.Format.ToLower() == Reagents[r])
                        {
                            c.Flag = true;
                            break;
                        }
                    }
                }

                if (m_UnflagTimer != null)
                    m_UnflagTimer.Stop();
                else
                    m_UnflagTimer = new UnflagTimer();
                m_UnflagTimer.Start();
            }

            Client.Instance.RequestTitlebarUpdate();
            UOAssist.PostSpellCast(this.Number);

            if (World.Player != null)
            {
                World.Player.LastSpell = GetID();
                LastCastTime = DateTime.UtcNow;
                Targeting.SpellTargetID = 0;
            }
        }

        public static DateTime LastCastTime = DateTime.MinValue;

        private class UnflagTimer : Timer
        {
            public UnflagTimer() : base(TimeSpan.FromSeconds(30.0))
            {
            }

            protected override void OnTick()
            {
                for (int i = 0; i < Counter.List.Count; i++)
                    ((Counter) Counter.List[i]).Flag = false;
                Client.Instance.RequestTitlebarUpdate();
            }
        }

        private static Dictionary<string, Spell> m_SpellsByPower;
        private static Dictionary<int, Spell> m_SpellsByID;
        private static Dictionary<string, Spell> m_SpellsByName;
        private static HotKeyCallbackState HotKeyCallback;

        static Spell()
        {
            string filename = Path.Combine(Config.GetInstallDirectory(), "spells.def");
            m_SpellsByPower = new Dictionary<string, Spell>(64 + 10 + 16);
            m_SpellsByID = new Dictionary<int, Spell>(64 + 10 + 16);
            m_SpellsByName = new Dictionary<string, Spell>(64 + 10 + 16);

            if (!File.Exists(filename))
            {
                MessageBox.Show(Engine.ActiveWindow, Language.GetString(LocString.NoSpells), "Spells.def",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.Length <= 0 || line[0] == '#')
                        continue;
                    string[] split = line.Split('|');

                    try
                    {
                        if (split.Length >= 5)
                        {
                            string[] reags = new string[split.Length - 5];
                            for (int i = 5; i < split.Length; i++)
                                reags[i - 5] = split[i].ToLower().Trim();
                            Spell s = new Spell(split[0].Trim()[0], Convert.ToInt32(split[1].Trim()),
                                Convert.ToInt32(split[2].Trim()), /*split[3].Trim(),*/ split[4].Trim(), reags);

                            m_SpellsByID[s.GetID()] = s;

                            line = Language.GetString(s.Name);
                            if (string.IsNullOrEmpty(line))
                                line = split[3].Trim();
                            if (!string.IsNullOrEmpty(line))
                                m_SpellsByName[line.ToLower()] = s;

                            if (s.WordsOfPower != null && s.WordsOfPower.Trim().Length > 0)
                                m_SpellsByPower[s.WordsOfPower] = s;
                        }
                    }
                    catch
                    {
                    }
                }
            }

            HotKeyCallback = new HotKeyCallbackState(OnHotKey);
            foreach (Spell s in m_SpellsByID.Values)
                HotKey.Add(HKCategory.Spells, HKSubCat.SpellOffset + s.Circle, s.Name, HotKeyCallback,
                    (ushort) s.GetID());
            HotKey.Add(HKCategory.Spells, LocString.HealOrCureSelf, new HotKeyCallback(HealOrCureSelf));
            HotKey.Add(HKCategory.Spells, LocString.MiniHealOrCureSelf, new HotKeyCallback(MiniHealOrCureSelf));
            HotKey.Add(HKCategory.Spells, LocString.GHealOrCureSelf, new HotKeyCallback(GHealOrCureSelf));
            HotKey.Add(HKCategory.Spells, LocString.Interrupt, new HotKeyCallback(Interrupt));
        }

        public static void HealOrCureSelf()
        {
            Spell s = null;

            if (!Client.Instance.AllowBit(FeatureBit.BlockHealPoisoned))
            {
                if (World.Player.Hits + 30 < World.Player.HitsMax && World.Player.Mana >= 12)
                    s = Get(4, 5); // greater heal
                else
                    s = Get(1, 4); // mini heal
            }
            else
            {
                if (World.Player.Poisoned && Client.Instance.AllowBit(FeatureBit.BlockHealPoisoned))
                {
                    s = Get(2, 3); // cure 
                }
                else if (World.Player.Hits + 2 < World.Player.HitsMax)
                {
                    if (World.Player.Hits + 30 < World.Player.HitsMax && World.Player.Mana >= 12)
                        s = Get(4, 5); // greater heal
                    else
                        s = Get(1, 4); // mini heal
                }
                else
                {
                    if (World.Player.Mana >= 12)
                        s = Get(4, 5); // greater heal
                    else
                        s = Get(1, 4); // mini heal
                }
            }

            if (s != null)
            {
                if (World.Player.Poisoned || World.Player.Hits < World.Player.HitsMax)
                    Targeting.TargetSelf(true);
                Client.Instance.SendToServer(new CastSpellFromMacro((ushort) s.GetID()));
                s.Cast();
            }
        }

        public static void MiniHealOrCureSelf()
        {
            Spell s = null;

            if (!Client.Instance.AllowBit(FeatureBit.BlockHealPoisoned))
            {
                s = Get(1, 4); // mini heal
            }
            else
            {
                if (World.Player.Poisoned)
                    s = Get(2, 3); // cure
                else
                    s = Get(1, 4); // mini heal
            }

            if (s != null)
            {
                if (World.Player.Poisoned || World.Player.Hits < World.Player.HitsMax)
                    Targeting.TargetSelf(true);
                Client.Instance.SendToServer(new CastSpellFromMacro((ushort) s.GetID()));
                s.Cast();
            }
        }

        public static void GHealOrCureSelf()
        {
            Spell s = null;

            if (!Client.Instance.AllowBit(FeatureBit.BlockHealPoisoned))
            {
                s = Get(4, 5); // gheal
            }
            else
            {
                if (World.Player.Poisoned)
                    s = Get(2, 3); // cure
                else
                    s = Get(4, 5); // gheal
            }

            if (s != null)
            {
                if (World.Player.Poisoned || World.Player.Hits < World.Player.HitsMax)
                    Targeting.TargetSelf(true);
                Client.Instance.SendToServer(new CastSpellFromMacro((ushort) s.GetID()));
                s.Cast();
            }
        }

        public static void Interrupt()
        {
            Item item = FindUsedLayer();

            if (item != null)
            {
                Client.Instance.SendToServer(new LiftRequest(item, 1)); // unequip
                Client.Instance.SendToServer(new EquipRequest(item.Serial, World.Player, item.Layer)); // Equip
            }
        }

        private static Item FindUsedLayer()
        {
            Item layeredItem = World.Player.GetItemOnLayer(Layer.Shirt);
            if (layeredItem != null)
                return layeredItem;

            layeredItem = World.Player.GetItemOnLayer(Layer.Shoes);
            if (layeredItem != null)
                return layeredItem;

            layeredItem = World.Player.GetItemOnLayer(Layer.Pants);
            if (layeredItem != null)
                return layeredItem;

            layeredItem = World.Player.GetItemOnLayer(Layer.Head);
            if (layeredItem != null)
                return layeredItem;

            layeredItem = World.Player.GetItemOnLayer(Layer.Gloves);
            if (layeredItem != null)
                return layeredItem;

            layeredItem = World.Player.GetItemOnLayer(Layer.Ring);
            if (layeredItem != null)
                return layeredItem;

            layeredItem = World.Player.GetItemOnLayer(Layer.Neck);
            if (layeredItem != null)
                return layeredItem;

            layeredItem = World.Player.GetItemOnLayer(Layer.Waist);
            if (layeredItem != null)
                return layeredItem;

            layeredItem = World.Player.GetItemOnLayer(Layer.InnerTorso);
            if (layeredItem != null)
                return layeredItem;

            layeredItem = World.Player.GetItemOnLayer(Layer.Bracelet);
            if (layeredItem != null)
                return layeredItem;

            layeredItem = World.Player.GetItemOnLayer(Layer.MiddleTorso);
            if (layeredItem != null)
                return layeredItem;

            layeredItem = World.Player.GetItemOnLayer(Layer.Earrings);
            if (layeredItem != null)
                return layeredItem;

            layeredItem = World.Player.GetItemOnLayer(Layer.Arms);
            if (layeredItem != null)
                return layeredItem;

            layeredItem = World.Player.GetItemOnLayer(Layer.Cloak);
            if (layeredItem != null)
                return layeredItem;

            layeredItem = World.Player.GetItemOnLayer(Layer.OuterTorso);
            if (layeredItem != null)
                return layeredItem;

            layeredItem = World.Player.GetItemOnLayer(Layer.OuterLegs);
            if (layeredItem != null)
                return layeredItem;

            layeredItem = World.Player.GetItemOnLayer(Layer.InnerLegs);
            if (layeredItem != null)
                return layeredItem;

            layeredItem = World.Player.GetItemOnLayer(Layer.RightHand);
            if (layeredItem != null)
                return layeredItem;

            layeredItem = World.Player.GetItemOnLayer(Layer.LeftHand);
            if (layeredItem != null)
                return layeredItem;

            return null;
        }

        public static void Initialize()
        {
            // no code, this is here to make sure out static ctor is init'd by the core
        }

        public static void OnHotKey(ref object state)
        {
            ushort id = (ushort) state;
            Spell s = Spell.Get(id);
            if (s != null)
            {
                s.OnCast(new CastSpellFromMacro(id));
                //if ( Macros.MacroManager.AcceptActions )
                //	Macros.MacroManager.Action( new Macros.MacroCastSpellAction( s ) );
            }
        }

        public static int ToID(int circle, int num)
        {
            if (circle < 10)
                return ((circle - 1) * 8) + num;
            else
                return (circle * 10) + num;
        }

        public static Spell Get(string power)
        {
            Spell s;
            m_SpellsByPower.TryGetValue(power, out s);
            return s;
        }

        public static Spell Get(int num)
        {
            Spell s;
            m_SpellsByID.TryGetValue(num, out s);
            return s;
        }

        public static Spell GetByName(string name)
        {
            m_SpellsByName.TryGetValue(name.ToLower(), out Spell s);
            return s;
        }

        public static string GetName(int num)
        {
            if (num > m_SpellsByName.Count - 1)
                return string.Empty;

            return m_SpellsByName.ElementAt(num).Key;
        }

        public static Spell Get(int circle, int num)
        {
            return Get(Spell.ToID(circle, num));
        }
    }
}