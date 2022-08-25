#region license
// Razor: An Ultima Online Assistant
// Copyright (c) 2022 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Assistant.Agents;
using Assistant.Core;
using Assistant.Gumps;
using Assistant.Macros;
using Assistant.UI;

namespace Assistant
{
    public enum LockType : byte
    {
        Up = 0,
        Down = 1,
        Locked = 2
    }

    public enum MsgLevel
    {
        Debug = 0,
        Info = 0,
        Warning = 1,
        Error = 2,
        Force = 3,
        Friend = 4
    }

    public class Skill
    {
        private LockType m_Lock;
        private ushort m_Value;
        private ushort m_Base;
        private ushort m_Cap;
        private short m_Delta;
        private int m_Idx;

        public Skill(int idx)
        {
            m_Idx = idx;
        }

        public int Index
        {
            get { return m_Idx; }
        }

        public LockType Lock
        {
            get { return m_Lock; }
            set { m_Lock = value; }
        }

        public ushort FixedValue
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public ushort FixedBase
        {
            get { return m_Base; }
            set
            {
                m_Delta += (short) (value - m_Base);
                m_Base = value;
            }
        }

        public ushort FixedCap
        {
            get { return m_Cap; }
            set { m_Cap = value; }
        }

        public double Value
        {
            get { return m_Value / 10.0; }
            set { m_Value = (ushort) (value * 10.0); }
        }

        public double Base
        {
            get { return m_Base / 10.0; }
            set { m_Base = (ushort) (value * 10.0); }
        }

        public double Cap
        {
            get { return m_Cap / 10.0; }
            set { m_Cap = (ushort) (value * 10.0); }
        }

        public double Delta
        {
            get { return m_Delta / 10.0; }
            set { m_Delta = (short) (value * 10); }
        }
    }

    public enum MaleSounds
    {
        Ah = 0x41A,
        Ahha = 0x41B,
        Applaud = 0x41C,
        BlowNose = 0x41D,
        Burp = 0x41E,
        Cheer = 0x41F,
        ClearThroat = 0x420,
        Cough = 0x421,
        CoughBS = 0x422,
        Cry = 0x423,
        Fart = 0x429,
        Gasp = 0x42A,
        Giggle = 0x42B,
        Groan = 0x42C,
        Growl = 0x42D,
        Hey = 0x42E,
        Hiccup = 0x42F,
        Huh = 0x430,
        Kiss = 0x431,
        Laugh = 0x432,
        No = 0x433,
        Oh = 0x434,
        Oomph1 = 0x435,
        Oomph2 = 0x436,
        Oomph3 = 0x437,
        Oomph4 = 0x438,
        Oomph5 = 0x439,
        Oomph6 = 0x43A,
        Oomph7 = 0x43B,
        Oomph8 = 0x43C,
        Oomph9 = 0x43D,
        Oooh = 0x43E,
        Oops = 0x43F,
        Puke = 0x440,
        Scream = 0x441,
        Shush = 0x442,
        Sigh = 0x443,
        Sneeze = 0x444,
        Sniff = 0x445,
        Snore = 0x446,
        Spit = 0x447,
        Whistle = 0x448,
        Yawn = 0x449,
        Yea = 0x44A,
        Yell = 0x44B,
    }

    public enum FemaleSounds
    {
        Ah = 0x30B,
        Ahha = 0x30C,
        Applaud = 0x30D,
        BlowNose = 0x30E,
        Burp = 0x30F,
        Cheer = 0x310,
        ClearThroat = 0x311,
        Cough = 0x312,
        CoughBS = 0x313,
        Cry = 0x314,
        Fart = 0x319,
        Gasp = 0x31A,
        Giggle = 0x31B,
        Groan = 0x31C,
        Growl = 0x31D,
        Hey = 0x31E,
        Hiccup = 0x31F,
        Huh = 0x320,
        Kiss = 0x321,
        Laugh = 0x322,
        No = 0x323,
        Oh = 0x324,
        Oomph1 = 0x325,
        Oomph2 = 0x326,
        Oomph3 = 0x327,
        Oomph4 = 0x328,
        Oomph5 = 0x329,
        Oomph6 = 0x32A,
        Oomph7 = 0x32B,
        Oooh = 0x32C,
        Oops = 0x32D,
        Puke = 0x32E,
        Scream = 0x32F,
        Shush = 0x330,
        Sigh = 0x331,
        Sneeze = 0x332,
        Sniff = 0x333,
        Snore = 0x334,
        Spit = 0x335,
        Whistle = 0x336,
        Yawn = 0x337,
        Yea = 0x338,
        Yell = 0x339,
    }

    public class PlayerData : Mobile
    {
        public int VisRange = 18;

        public int MultiVisRange
        {
            get { return VisRange + 5; }
        }

        private int m_MaxWeight = -1;

        private short m_FireResist, m_ColdResist, m_PoisonResist, m_EnergyResist, m_Luck;
        private ushort m_DamageMin, m_DamageMax;

        private ushort m_Str, m_Dex, m_Int;
        private LockType m_StrLock, m_DexLock, m_IntLock;
        private uint m_Gold;
        private ushort m_Weight;
        private Skill[] m_Skills;
        private ushort m_AR;
        private ushort m_StatCap;
        private byte m_Followers;
        private byte m_FollowersMax;
        private int m_Tithe;
        private sbyte m_LocalLight;
        private byte m_GlobalLight;
        private uint m_Features;
        private byte m_Season;
        private byte m_DefaultSeason;
        private int[] m_MapPatches = new int[10];


        private bool m_SkillsSent;
        private Timer m_CriminalTime;
        private DateTime m_CriminalStart = DateTime.MinValue;

        internal List<BuffDebuff> m_BuffsDebuffs = new List<BuffDebuff>();

        internal List<BuffDebuff> BuffsDebuffs
        {
            get { return m_BuffsDebuffs; }
        }

        private List<uint> m_OpenedCorpses = new List<uint>();

        public List<uint> OpenedCorpses
        {
            get { return m_OpenedCorpses; }
        }

        public PlayerData(Serial serial) : base(serial)
        {
            m_Skills = new Skill[Ultima.Skills.TotalSkills()];
            for (int i = 0; i < m_Skills.Length; i++)
                m_Skills[i] = new Skill(i);
        }

        public ushort Str
        {
            get { return m_Str; }
            set { m_Str = value; }
        }

        public ushort Dex
        {
            get { return m_Dex; }
            set { m_Dex = value; }
        }

        public ushort Int
        {
            get { return m_Int; }
            set { m_Int = value; }
        }

        public uint Gold
        {
            get { return m_Gold; }
            set { m_Gold = value; }
        }

        public ushort Weight
        {
            get { return m_Weight; }
            set { m_Weight = value; }
        }

        public ushort MaxWeight
        {
            get
            {
                if (m_MaxWeight == -1)
                    return (ushort) ((m_Str * 3.5) + 40);
                else
                    return (ushort) m_MaxWeight;
            }
            set { m_MaxWeight = value; }
        }

        public short FireResistance
        {
            get { return m_FireResist; }
            set { m_FireResist = value; }
        }

        public short ColdResistance
        {
            get { return m_ColdResist; }
            set { m_ColdResist = value; }
        }

        public short PoisonResistance
        {
            get { return m_PoisonResist; }
            set { m_PoisonResist = value; }
        }

        public short EnergyResistance
        {
            get { return m_EnergyResist; }
            set { m_EnergyResist = value; }
        }

        public short Luck
        {
            get { return m_Luck; }
            set { m_Luck = value; }
        }

        public ushort DamageMin
        {
            get { return m_DamageMin; }
            set { m_DamageMin = value; }
        }

        public ushort DamageMax
        {
            get { return m_DamageMax; }
            set { m_DamageMax = value; }
        }

        public LockType StrLock
        {
            get { return m_StrLock; }
            set { m_StrLock = value; }
        }

        public LockType DexLock
        {
            get { return m_DexLock; }
            set { m_DexLock = value; }
        }

        public LockType IntLock
        {
            get { return m_IntLock; }
            set { m_IntLock = value; }
        }

        public ushort StatCap
        {
            get { return m_StatCap; }
            set { m_StatCap = value; }
        }

        public ushort AR
        {
            get { return m_AR; }
            set { m_AR = value; }
        }

        public byte Followers
        {
            get { return m_Followers; }
            set { m_Followers = value; }
        }

        public byte FollowersMax
        {
            get { return m_FollowersMax; }
            set { m_FollowersMax = value; }
        }

        public int Tithe
        {
            get { return m_Tithe; }
            set { m_Tithe = value; }
        }

        public Skill[] Skills
        {
            get { return m_Skills; }
        }

        public bool SkillsSent
        {
            get { return m_SkillsSent; }
            set { m_SkillsSent = value; }
        }

        public int CriminalTime
        {
            get
            {
                if (m_CriminalStart != DateTime.MinValue)
                {
                    int sec = (int) (DateTime.UtcNow - m_CriminalStart).TotalSeconds;
                    if (sec > 300)
                    {
                        if (m_CriminalTime != null)
                            m_CriminalTime.Stop();
                        m_CriminalStart = DateTime.MinValue;
                        return 0;
                    }
                    else
                    {
                        return sec;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        private class DoorOpenTimer : Timer
        {
            // Fire off once in 5ms
            public DoorOpenTimer() : base(TimeSpan.FromMilliseconds(5), 1)
            {
            }

            protected override void OnTick()
            {
                Client.Instance.SendToServer(new OpenDoorMacro());
            }
        }

        private readonly DoorOpenTimer _doorTimer = new DoorOpenTimer();

        private void AutoOpenDoors(bool onDirChange)
        {
            if (!Visible && !Config.GetBool("AutoOpenDoorWhenHidden"))
                return;

            if (Body != 0x03DB && !IsGhost && ((int) (Direction & Direction.Mask)) % 2 == 0 &&
                Config.GetBool("AutoOpenDoors") &&
                Client.Instance.AllowBit(FeatureBit.AutoOpenDoors))
            {
                int x = Position.X, y = Position.Y, z = Position.Z;

                /* Check if one more tile in the direction we just moved is a door */
                Utility.Offset(Direction, ref x, ref y);

                if (World.Items.Values.Any(s =>
                    s.IsDoor && s.Position.X == x && s.Position.Y == y && s.Position.Z - 15 <= z &&
                    s.Position.Z + 15 >= z))
                {
                    if (Client.IsOSI)
                    {
                        Client.Instance.SendToServer(new OpenDoorMacro());
                    }
                    else
                    {
                        // ClassicUO requires a slight pause before attempting to
                        // open a door after a direction change
                        if (onDirChange)
                        {
                            _doorTimer.Start();
                        }
                        else
                        {
                            Client.Instance.SendToServer(new OpenDoorMacro());
                        }
                    }
                }
            }
        }

        public override void OnPositionChanging(Point3D oldPos)
        {
            if (!IsGhost)
                StealthSteps.OnMove();

            AutoOpenDoors(false);

            List<Mobile> mlist = new List<Mobile>(World.Mobiles.Values);
            for (int i = 0; i < mlist.Count; i++)
            {
                Mobile m = mlist[i];
                if (m != this)
                {
                    if (!Utility.InRange(m.Position, Position, VisRange))
                        m.Remove();
                    else
                        Targeting.CheckLastTargetRange(m);
                }
            }

            mlist = null;


            List<Item> ilist = new List<Item>(World.Items.Values);
            ScavengerAgent s = ScavengerAgent.Instance;
            for (int i = 0; i < ilist.Count; i++)
            {
                Item item = ilist[i];
                if (item.Deleted || item.Container != null)
                    continue;

                int dist = Utility.Distance(item.GetWorldPosition(), Position);
                if (item != DragDropManager.Holding && (dist > MultiVisRange || (!item.IsMulti && dist > VisRange)))
                    item.Remove();
                else if (!IsGhost && Visible && dist <= 2 && s.Enabled && item.Movable)
                    s.Scavenge(item);
            }

            ilist = null;

            if (Engine.MainWindow != null && Engine.MainWindow.MapWindow != null)
                Engine.MainWindow.SafeAction(f => f.MapWindow.PlayerMoved());

            base.OnPositionChanging(oldPos);
        }

        public override void OnDirectionChanging(Direction oldDir)
        {
            AutoOpenDoors(true);
        }

        public override void OnMapChange(byte old, byte cur)
        {
            List<Mobile> list = new List<Mobile>(World.Mobiles.Values);
            for (int i = 0; i < list.Count; i++)
            {
                Mobile m = list[i];
                if (m != this && m.Map != cur)
                    m.Remove();
            }

            list = null;

            World.Items.Clear();
            Counter.Reset();
            for (int i = 0; i < Contains.Count; i++)
            {
                Item item = (Item) Contains[i];
                World.AddItem(item);
                item.Contains.Clear();
            }

            if (Config.GetBool("AutoSearch") && Backpack != null)
                PlayerData.DoubleClick(Backpack);

            UOAssist.PostMapChange(cur);

            if (Engine.MainWindow != null && Engine.MainWindow.MapWindow != null)
                Engine.MainWindow.SafeAction(s => s.MapWindow.PlayerMoved());
        }

        /*public override void OnMapChange( byte old, byte cur )
        {
             World.Mobiles.Clear();
             World.Items.Clear();
             Counter.Reset();

             Contains.Clear();

             World.AddMobile( this );

             UOAssist.PostMapChange( cur );
        }*/

        protected override void OnNotoChange(byte old, byte cur)
        {
            if ((old == 3 || old == 4) && (cur != 3 && cur != 4))
            {
                // grey is turning off
                // SendMessage( "You are no longer a criminal." );
                if (m_CriminalTime != null)
                    m_CriminalTime.Stop();
                m_CriminalStart = DateTime.MinValue;
                Client.Instance.RequestTitlebarUpdate();
            }
            else if ((cur == 3 || cur == 4) && (old != 3 && old != 4 && old != 0))
            {
                // grey is turning on
                ResetCriminalTimer();
            }
        }

        public void ResetCriminalTimer()
        {
            if (m_CriminalStart == DateTime.MinValue || DateTime.UtcNow - m_CriminalStart >= TimeSpan.FromSeconds(1))
            {
                m_CriminalStart = DateTime.UtcNow;
                if (m_CriminalTime == null)
                    m_CriminalTime = new CriminalTimer(this);
                m_CriminalTime.Start();
                Client.Instance.RequestTitlebarUpdate();
            }
        }

        private class CriminalTimer : Timer
        {
            private PlayerData m_Player;

            public CriminalTimer(PlayerData player) : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
            {
                m_Player = player;
            }

            protected override void OnTick()
            {
                Client.Instance.RequestTitlebarUpdate();
            }
        }

        internal void SendMessage(MsgLevel lvl, LocString loc, params object[] args)
        {
            SendMessage(lvl, Language.Format(loc, args));
        }

        internal void SendMessage(MsgLevel lvl, LocString loc)
        {
            SendMessage(lvl, Language.GetString(loc));
        }

        internal void SendMessage(LocString loc, params object[] args)
        {
            SendMessage(MsgLevel.Info, Language.Format(loc, args));
        }

        internal void SendMessage(LocString loc)
        {
            SendMessage(MsgLevel.Info, Language.GetString(loc));
        }

        internal void SendMessage(int hue, string text)
        {
            Client.Instance.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, hue, 3,
                Language.CliLocName, "System", text));
        }

        internal void SendMessage(MsgLevel lvl, string format, params object[] args)
        {
            SendMessage(lvl, String.Format(format, args));
        }

        internal void SendMessage(string format, params object[] args)
        {
            SendMessage(MsgLevel.Info, String.Format(format, args));
        }

        internal void SendMessage(string text)
        {
            SendMessage(MsgLevel.Info, text);
        }

        internal void SendMessage(MsgLevel lvl, string text)
        {
            if (lvl >= (MsgLevel) Config.GetInt("MessageLevel") && text.Length > 0)
            {
                int hue;
                switch (lvl)
                {
                    case MsgLevel.Error:
                    case MsgLevel.Warning:
                        hue = Config.GetInt("WarningColor");
                        break;
                    case MsgLevel.Friend:
                        hue = 63;
                        break;
                    default:
                        hue = Config.GetInt("SysColor");
                        break;
                }

                SystemMessages.Add(text);

                if (Config.GetBool("FilterRazorMessages"))
                {
                    if (!MessageQueue.Enqueue(0xFFFFFFFF, null, 0, MessageType.Regular, (ushort) hue, 3,
                        Language.CliLocName, "System", text))
                    {
                        return;
                    }
                }

                Client.Instance.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, hue, 3,
                    Language.CliLocName, "System", text));
            }
        }

        internal void Say(int hue, string msg)
        {
            List<ushort> keywords = EncodedSpeech.GetKeywords(msg);

            if (keywords.Count == 1 && (int) keywords[0] == 0)
            {
                Client.Instance.SendToServer(new ClientUniEncodedCommandMessage(MessageType.Regular, hue, 3,
                    Language.CliLocName, keywords, msg));
            }
            else
            {
                Client.Instance.SendToServer(new ClientUniEncodedCommandMessage(MessageType.Encoded, hue, 3,
                    Language.CliLocName, keywords, msg));
            }
        }

        internal void Say(string msg)
        {
            Say(Config.GetInt("SpeechHue"), msg);
        }

        internal void Whisper(string msg, int hue)
        {
            Client.Instance.SendToServer(new ClientUniMessage(MessageType.Whisper, hue, 3,
                Language.CliLocName, new ArrayList(), msg));
        }

        internal void Yell(string msg, int hue)
        {
            Client.Instance.SendToServer(new ClientUniMessage(MessageType.Yell, hue, 3,
                Language.CliLocName, new ArrayList(), msg));
        }

        internal void Emote(string msg, int hue)
        {
            msg = $"*{msg}*";

            Client.Instance.SendToServer(new ClientUniMessage(MessageType.Emote, hue, 3,
                Language.CliLocName, new ArrayList(), msg));
        }

        internal void Guild(string msg, int hue)
        {
            Client.Instance.SendToServer(new ClientUniMessage(MessageType.Guild, hue, 3,
                Language.CliLocName, new ArrayList(), msg));
        }

        internal void Alliance(string msg, int hue)
        {
            Client.Instance.SendToServer(new ClientUniMessage(MessageType.Alliance, hue, 3,
                Language.CliLocName, new ArrayList(), msg));
        }

        public uint CurrentGumpS, CurrentGumpI;
        public GumpResponseAction LastGumpResponseAction;
        public bool HasGump;
        public bool HasCompressedGump;
        public List<string> CurrentGumpStrings = new List<string>();
        public string CurrentGumpRawData;
        public uint CurrentMenuS;
        public ushort CurrentMenuI;
        public bool HasMenu;

        public bool HasPrompt;
        public uint PromptSenderSerial;
        public uint PromptID;
        public uint PromptType;
        public string PromptInputText;

        public GumpCollection InternalGumps { get; set; } = new GumpCollection();

        public void CancelPrompt()
        {
            Client.Instance.SendToServer(new PromptResponse(World.Player.PromptSenderSerial, World.Player.PromptID, 0,
                Language.CliLocName, string.Empty));
            World.Player.HasPrompt = false;
        }

        public void ResponsePrompt(string text)
        {
            Client.Instance.SendToServer(new PromptResponse(World.Player.PromptSenderSerial, World.Player.PromptID, 1,
                Language.CliLocName, text));

            PromptInputText = text;
            World.Player.HasPrompt = false;
        }

        private ushort m_SpeechHue;

        public ushort SpeechHue
        {
            get { return m_SpeechHue; }
            set { m_SpeechHue = value; }
        }

        public sbyte LocalLightLevel
        {
            get { return m_LocalLight; }
            set { m_LocalLight = value; }
        }

        public byte GlobalLightLevel
        {
            get { return m_GlobalLight; }
            set { m_GlobalLight = value; }
        }

        public enum SeasonFlag
        {
            Spring,
            Summer,
            Fall,
            Winter,
            Desolation
        }

        public byte Season
        {
            get { return m_Season; }
            set { m_Season = value; }
        }

        public byte DefaultSeason
        {
            get { return m_DefaultSeason; }
            set { m_DefaultSeason = value; }
        }

        /// <summary>
        /// Sets the player's season, set a default to revert back if required
        /// </summary>
        /// <param name="defaultSeason"></param>
        public void SetSeason(byte defaultSeason = 0)
        {
            if (Config.GetInt("Season") < 5)
            {
                byte season = (byte) Config.GetInt("Season");

                World.Player.Season = season;
                World.Player.DefaultSeason = defaultSeason;

                if (!m_SeasonTimer.Running)
                    m_SeasonTimer.Start();
            }
            else
            {
                World.Player.Season = defaultSeason;
                World.Player.DefaultSeason = defaultSeason;
            }
        }

        public static Timer m_SeasonTimer = new SeasonTimer();

        private class SeasonTimer : Timer
        {
            public SeasonTimer() : base(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3))
            {
            }

            protected override void OnTick()
            {
                if (World.Player != null && Client.Instance != null)
                {
                    Client.Instance.ForceSendToClient(new SeasonChange(World.Player.Season, true));
                    m_SeasonTimer.Stop();
                }
            }
        }

        public uint Features
        {
            get { return m_Features; }
            set { m_Features = value; }
        }

        public int[] MapPatches
        {
            get { return m_MapPatches; }
            set { m_MapPatches = value; }
        }

        private int m_LastSkill = -1;

        public int LastSkill
        {
            get { return m_LastSkill; }
            set { m_LastSkill = value; }
        }

        private Serial m_LastObj = Serial.Zero;

        public Serial LastObject
        {
            get { return m_LastObj; }
            set { m_LastObj = value; }
        }

        private int m_LastSpell = -1;

        public int LastSpell
        {
            get { return m_LastSpell; }
            set { m_LastSpell = value; }
        }

        //private UOEntity m_LastCtxM = null;
        //public UOEntity LastContextMenu { get { return m_LastCtxM; } set { m_LastCtxM = value; } }

        public bool UseItem(Item cont, ushort find)
        {
            if (!Client.Instance.AllowBit(FeatureBit.PotionHotkeys))
                return false;

            for (int i = 0; i < cont.Contains.Count; i++)
            {
                Item item = (Item)cont.Contains[i];

                if (item.ItemID == find)
                {
                    PlayerData.DoubleClick(item);
                    return true;
                }
                else if (item.Contains != null && item.Contains.Count > 0)
                {
                    if (UseItem(item, find))
                        return true;
                }
            }

            return false;
        }

        public static bool DoubleClick(object clicked)
        {
            return DoubleClick(clicked, true);
        }

        public static bool DoubleClick(object clicked, bool silent)
        {
            Serial s;
            if (clicked is Mobile)
                s = ((Mobile) clicked).Serial.Value;
            else if (clicked is Item)
                s = ((Item) clicked).Serial.Value;
            else if (clicked is Serial)
                s = ((Serial) clicked).Value;
            else
                s = Serial.Zero;

            if (s != Serial.Zero)
            {
                Item free = null, pack = World.Player.Backpack;
                if (s.IsItem && pack != null && Config.GetBool("PotionEquip") &&
                    Client.Instance.AllowBit(FeatureBit.AutoPotionEquip))
                {
                    Item i = World.FindItem(s);
                    if (i != null && i.IsPotion && i.ItemID != 3853) // dont unequip for exploison potions
                    {
                        // dont worry about uneqipping RuneBooks or SpellBooks
                        Item left = World.Player.GetItemOnLayer(Layer.LeftHand);
                        Item right = World.Player.GetItemOnLayer(Layer.RightHand);

                        if (left != null && (right != null || left.IsTwoHanded))
                            free = left;
                        else if (right != null && right.IsTwoHanded)
                            free = right;

                        if (free != null)
                        {
                            if (DragDropManager.HasDragFor(free.Serial))
                                free = null;
                            else
                                DragDropManager.DragDrop(free, pack);
                        }
                    }
                }

                ActionQueue.DoubleClick(silent, s);

                if (free != null && Config.GetBool("PotionReequip"))
                    DragDropManager.DragDrop(free, World.Player, free.Layer, true);

                if (s.IsItem)
                    World.Player.m_LastObj = s;
            }

            return false;
        }

        public enum InvokeVirtues
        {
            Honor = 0x01,
            Sacrifice = 0x02,
            Valor = 0x03
        }

        public void InvokeVirtue(InvokeVirtues virtue)
        {
            Client.Instance.SendToServer(new VirtueRequest((byte) virtue));
        }
    }
}