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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assistant.Core;
using Assistant.Scripts.Engine;

namespace Assistant.Scripts
{
    public static class Commands
    {
        public static void Register()
        {
            // Commands based on Actions.cs
            Interpreter.RegisterCommandHandler("attack", Attack); //Attack by serial
            Interpreter.RegisterCommandHandler("cast", Cast); //BookcastAction, etc

            // Dress
            Interpreter.RegisterCommandHandler("dress", DressCommand); //DressAction
            Interpreter.RegisterCommandHandler("undress", UnDressCommand); //UndressAction

            // Targets
            Interpreter.RegisterCommandHandler("target", Target); //Absolute Target
            Interpreter.RegisterCommandHandler("targettype", TargetType); //TargetTypeAction
            Interpreter.RegisterCommandHandler("targetrelloc", TargetRelLoc); //TargetRelLocAction

            Interpreter.RegisterCommandHandler("waitfortarget", WaitForTarget); //WaitForTargetAction
            Interpreter.RegisterCommandHandler("wft", WaitForTarget); //WaitForTargetAction

            // Using stuff
            Interpreter.RegisterCommandHandler("dclicktype", UseType); // DoubleClickTypeAction
            Interpreter.RegisterCommandHandler("dclick", UseObject); //DoubleClickAction

            Interpreter.RegisterCommandHandler("usetype", UseType); // DoubleClickTypeAction
            Interpreter.RegisterCommandHandler("useobject", UseObject); //DoubleClickAction

            // Moving stuff
            Interpreter.RegisterCommandHandler("drop", DropItem); //DropAction
            Interpreter.RegisterCommandHandler("droprelloc", DropRelLoc); //DropAction
            Interpreter.RegisterCommandHandler("lift", LiftItem); //LiftAction
            Interpreter.RegisterCommandHandler("lifttype", LiftType); //LiftTypeAction

            // Gump
            Interpreter.RegisterCommandHandler("waitforgump", WaitForGump); // WaitForGumpAction
            Interpreter.RegisterCommandHandler("gumpresponse", GumpResponse); // GumpResponseAction
            Interpreter.RegisterCommandHandler("gumpclose", GumpClose); // GumpResponseAction

            // Menu
            Interpreter.RegisterCommandHandler("menu", ContextMenu); //ContextMenuAction
            Interpreter.RegisterCommandHandler("menuresponse", MenuResponse); //MenuResponseAction
            Interpreter.RegisterCommandHandler("waitformenu", WaitForMenu); //WaitForMenuAction

            // Prompt
            Interpreter.RegisterCommandHandler("promptresponse", PromptResponse); //PromptAction
            Interpreter.RegisterCommandHandler("waitforprompt", WaitForPrompt); //WaitForPromptAction

            // Hotkey execution
            Interpreter.RegisterCommandHandler("hotkey", Hotkey); //HotKeyAction

            // Messages
            Interpreter.RegisterCommandHandler("say", Msg); //SpeechAction
            Interpreter.RegisterCommandHandler("msg", Msg); //SpeechAction
            Interpreter.RegisterCommandHandler("overhead", HeadMsg); //OverheadMessageAction
            Interpreter.RegisterCommandHandler("headmsg", HeadMsg); //OverheadMessageAction
            Interpreter.RegisterCommandHandler("sysmsg", SysMsg); //SystemMessageAction

            // General Waits/Pauses
            Interpreter.RegisterCommandHandler("wait", Pause); //PauseAction
            Interpreter.RegisterCommandHandler("pause", Pause); //PauseAction

            // Misc
            Interpreter.RegisterCommandHandler("setability", SetAbility); //SetAbilityAction
            Interpreter.RegisterCommandHandler("setlasttarget", SetLastTarget); //SetLastTargetAction
            Interpreter.RegisterCommandHandler("lasttarget", LastTarget); //LastTargetAction
            Interpreter.RegisterCommandHandler("skill", UseSkill); //SkillAction
            Interpreter.RegisterCommandHandler("useskill", UseSkill); //SkillAction
            Interpreter.RegisterCommandHandler("walk", Walk); //Move/WalkAction
            Interpreter.RegisterCommandHandler("potion", Potion);

            // Script related
            Interpreter.RegisterCommandHandler("script", PlayScript);
            Interpreter.RegisterCommandHandler("setvar", SetVar);
            Interpreter.RegisterCommandHandler("setvariable", SetVar);
        }

        private static bool SetLastTarget(string command, Argument[] args, bool quiet, bool force)
        {
            if (!ScriptManager.SetLastTargetActive)
            {
                Targeting.TargetSetLastTarget();
                ScriptManager.SetLastTargetActive = true;

                return false;
            }

            if (Targeting.LTWasSet)
            {
                ScriptManager.SetLastTargetActive = false;
                return true;
            }

            return false;
        }

        private static bool SetVar(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: setvar ('variable')");
            }

            string varname = args[0].AsString();

            ScriptVariables.ScriptVariable var = ScriptVariables.GetVariable(varname);

            if (var == null)
            {
                throw new RunTimeError(null, $"Unknown variable '{varname}'");
            }

            if (!ScriptManager.SetVariableActive)
            {
                var.SetTarget();
                ScriptManager.SetVariableActive = true;

                return false;
            }

            if (var.TargetWasSet)
            {
                ScriptManager.SetVariableActive = false;
                return true;
            }

            return false;
        }

        private static bool Target(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: target (serial) OR (x) (y) (z)");
            }

            if (args.Length == 1)
            {
                Serial serial = args[0].AsSerial();

                if (serial != Serial.Zero) // Target a specific item or mobile
                {
                    Item item = World.FindItem(serial);

                    if (item != null)
                    {
                        Targeting.Target(item);
                        return true;
                    }

                    Mobile mobile = World.FindMobile(serial);

                    if (mobile != null)
                    {
                        Targeting.Target(mobile);
                    }
                }
            }
            else if (args.Length == 3) // target ground at specific x/y/z
            {
                Targeting.Target(new TargetInfo
                {
                    Type = 1,
                    Flags = 0,
                    Serial = Serial.Zero,
                    X = args[0].AsInt(),
                    Y = args[1].AsInt(),
                    Z = args[2].AsInt(),
                    Gfx = 0
                });
            }

            return true;
        }

        private static bool TargetType(string command, Argument[] args, bool quiet, bool force)
        {
            if (Targeting.FromGrabHotKey)
                return false;

            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: targettype (graphic) OR ('name of item or mobile type') [inrangecheck]");
            }

            string gfxStr = args[0].AsString();
            Serial gfx = Utility.ToUInt16(gfxStr, 0);

            bool inRangeCheck = false;

            if (args.Length == 2)
            {
                inRangeCheck = args[1].AsBool();
            }

            ArrayList list = new ArrayList();

            // No graphic id, maybe searching by name?
            if (gfx == 0)
            {
                List<Item> items = World.FindItemsByName(gfxStr);

                Item backItem = World.Player.Backpack.FindItemByName(gfxStr, true);

                if (backItem != null)
                    items.Add(backItem);

                if (items.Count > 0)
                {
                    list.AddRange(inRangeCheck
                        ? items.Where(i => Utility.InRange(World.Player.Position, i.Position, 2)).ToList()
                        : items);
                }
                else // try to find a mobile
                {
                    List<Mobile> mobiles = World.FindMobilesByName(gfxStr);

                    if (mobiles.Count > 0)
                    {
                        list.AddRange(inRangeCheck
                            ? mobiles.Where(m => Utility.InRange(World.Player.Position, m.Position, 2)).ToList()
                            : mobiles);
                    }
                }
            }
            else // check if they are mobile or an item
            {
                foreach (Mobile find in World.MobilesInRange())
                {
                    if (find.Body == gfx)
                    {
                        list.Add(find);
                    }
                }

                if (list.Count == 0)
                {
                    foreach (Item i in World.Items.Values)
                    {
                        if (i.ItemID == gfx && !i.IsInBank)
                        {
                            list.Add(i);
                        }
                    }
                }
            }

            if (list.Count > 0)
            {
                Targeting.Target(list[Utility.Random(list.Count)]);
            }
            else
            {
                World.Player.SendMessage(MsgLevel.Warning, LocString.NoItemOfType,
                    gfx.IsMobile ? $"Character [{gfx}]" : ((ItemID) gfx.Value).ToString());
            }

            return true;
        }

        private static bool TargetRelLoc(string command, Argument[] args, bool quiet, bool force)
        {
            if (Targeting.FromGrabHotKey)
                return false;

            if (args.Length < 2)
            {
                throw new RunTimeError(null, "Usage: targetrelloc (x-offset) (y-offset)");
            }

            int xoffset = Utility.ToInt32(args[0].AsString(), 0);
            int yoffset = Utility.ToInt32(args[1].AsString(), 0);

            ushort x = (ushort) (World.Player.Position.X + xoffset);
            ushort y = (ushort) (World.Player.Position.Y + yoffset);
            short z = (short) World.Player.Position.Z;

            try
            {
                Ultima.HuedTile tile = Map.GetTileNear(World.Player.Map, x, y, z);
                Targeting.Target(new Point3D(x, y, tile.Z), tile.ID);
            }
            catch (Exception e)
            {
                throw new RunTimeError(null, $"Error Executing TargetRelLoc: {e.Message}");
            }

            return true;
        }

        private static bool WaitForTarget(string command, Argument[] args, bool quiet, bool force)
        {
            if (Targeting.HasTarget)
                return true;

            Interpreter.Timeout(args.Length > 0 ? args[0].AsUInt() : 30000, () => { return true; });

            return false;
        }

        private static bool Hotkey(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: hotkey ('name of hotkey') OR (hotkeyId)");
            }

            string query = args[0].AsString();

            KeyData hk = HotKey.GetByNameOrId(query);

            if (hk == null)
            {
                throw new RunTimeError(null, $"Hotkey '{query}' not found");
            }

            hk.Callback();

            return true;
        }

        private static bool WaitForGump(string command, Argument[] args, bool quiet, bool force)
        {
            uint gumpId = 0;
            bool strict = false;

            // Look for a specific gump
            if (args.Length == 1)
            {
                gumpId = Utility.ToUInt32(args[0].AsString(), 0);

                if (gumpId > 0)
                    strict = true;
            }

            return ((World.Player.HasGump || World.Player.HasCompressedGump) &&
                    (World.Player.CurrentGumpI == gumpId || !strict || gumpId == 0));
        }

        private static bool WaitForMenu(string command, Argument[] args, bool quiet, bool force)
        {
            uint menuId = 0;

            // Look for a specific menu
            if (args.Length == 1)
            {
                menuId = Utility.ToUInt32(args[0].AsString(), 0);
            }

            return (World.Player.HasMenu && (World.Player.CurrentGumpI == menuId || menuId == 0));
        }

        private static bool WaitForPrompt(string command, Argument[] args, bool quiet, bool force)
        {
            uint promptId = 0;
            bool strict = false;

            // Look for a specific gump
            if (args.Length == 1)
            {
                promptId = Utility.ToUInt32(args[0].AsString(), 0);

                if (promptId > 0)
                    strict = true;
            }

            return (World.Player.HasPrompt && (World.Player.PromptID == promptId || !strict || promptId == 0));
        }

        private static string[] abilities = new string[4] {"primary", "secondary", "stun", "disarm"};

        private static bool SetAbility(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1 || !abilities.Contains(args[0].AsString()))
            {
                throw new RunTimeError(null, "Usage: setability ('primary'/'secondary'/'stun'/'disarm') ['on'/'off']");
            }

            if (args.Length == 2 && args[1].AsString() == "on" || args.Length == 1)
            {
                switch (args[0].AsString())
                {
                    case "primary":
                        SpecialMoves.SetPrimaryAbility();
                        break;
                    case "secondary":
                        SpecialMoves.SetSecondaryAbility();
                        break;
                    case "stun":
                        Client.Instance.SendToServer(new StunRequest());
                        break;
                    case "disarm":
                        Client.Instance.SendToServer(new DisarmRequest());
                        break;
                    default:
                        break;
                }
            }
            else if (args.Length == 2 && args[1].AsString() == "off")
            {
                Client.Instance.SendToServer(new UseAbility(AOSAbility.Clear));
                Client.Instance.SendToClient(ClearAbility.Instance);
            }

            return true;
        }

        private static string[] hands = new string[3] {"left", "right", "both"};

        private static bool ClearHands(string command, Argument[] args, bool quiet, bool force)
        {
            // expect one STRING node

            if (args.Length == 0 || !hands.Contains(args[0].AsString()))
            {
                throw new RunTimeError(null, "Usage: clearhands ('left'/'right'/'both')");
            }

            switch (args[0].AsString())
            {
                case "left":
                    Dress.Unequip(Layer.LeftHand);
                    break;
                case "right":
                    Dress.Unequip(Layer.RightHand);
                    break;
                default:
                    Dress.Unequip(Layer.LeftHand);
                    Dress.Unequip(Layer.RightHand);
                    break;
            }

            return true;
        }

        private static bool UseType(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, 
                    "Usage: dclicktype|usetype ('name of item') OR (graphicID) [inrangecheck (true/false)]");
            }

            string gfxStr = args[0].AsString();
            Serial gfx = Utility.ToUInt16(gfxStr, 0);
            Serial click = Serial.Zero;
            List<Item> items = new List<Item>();

            bool inRangeCheck = false;

            if (args.Length == 2)
            {
                inRangeCheck = args[1].AsBool();
            }

            // No graphic id, maybe searching by name?
            if (gfx == 0)
            {
                foreach (Item item in World.FindItemsByName(gfxStr))
                {
                    if (inRangeCheck)
                    {
                        if (Utility.InRange(World.Player.Position, item.Position, 2))
                        {
                            items.Add(item);
                        }
                    }
                    else
                    {
                        items.Add(item);
                    }
                }

                Item i = World.Player.Backpack.FindItemByName(gfxStr, true);

                if (i != null)
                    items.Add(i);

                if (items.Count == 0)
                {
                    throw new RunTimeError(null, $"Script Error: Couldn't find '{gfxStr}'");
                    return true;
                }

                click = items[Utility.Random(items.Count)].Serial;
            }
            else // Check backpack first
            {
                if (World.Player.Backpack != null)
                {
                    Item i = World.Player.Backpack.FindItemByID(Utility.ToUInt16(gfxStr, 0));

                    if (i != null)
                        items.Add(i);

                    if (items.Count > 0)
                        click = items[Utility.Random(items.Count)].Serial;
                }
            }

            // Not in backpack? Lets check the world
            if (items.Count == 0)
            {
                foreach (Item i in World.Items.Values)
                {
                    if (i.ItemID == gfx && i.RootContainer == null)
                    {
                        if (inRangeCheck)
                        {
                            if (Utility.InRange(World.Player.Position, i.Position, 2))
                                items.Add(i);
                        }
                        else
                        {
                            items.Add(i);
                        }
                    }
                }

                if (items.Count == 0)
                {
                    foreach (Item i in World.Items.Values)
                    {
                        if (i.ItemID == gfx && !i.IsInBank)
                        {
                            if (inRangeCheck)
                            {
                                if (Utility.InRange(World.Player.Position, i.Position, 2))
                                    items.Add(i);
                            }
                            else
                            {
                                items.Add(i);
                            }
                        }
                    }
                }

                if (items.Count > 0)
                    click = items[Utility.Random(items.Count)].Serial;
            }

            // Still no item? Mobile check!
            if (items.Count == 0)
            {
                List<Mobile> mobiles = new List<Mobile>();
                foreach (Mobile m in World.MobilesInRange())
                {
                    if (m.Body == gfx)
                    {
                        if (inRangeCheck)
                        {
                            if (Utility.InRange(World.Player.Position, m.Position, 2))
                                mobiles.Add(m);
                        }
                        else
                        {
                            mobiles.Add(m);
                        }
                    }
                }

                if (mobiles.Count > 0)
                    click = mobiles[Utility.Random(mobiles.Count)].Serial;
            }

            if (click != Serial.Zero)
                PlayerData.DoubleClick(click);
            else
                World.Player.SendMessage(MsgLevel.Force, LocString.NoItemOfType,
                    gfx.IsItem ? ((ItemID) gfx.Value).ToString() : $"(Character) 0x{gfx:X}");

            return true;
        }

        private static bool UseObject(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: dclick/useobject (serial)");
            }

            Serial serial = args[0].AsSerial();

            if (!serial.IsValid)
            {
                throw new RunTimeError(null, "useobject - invalid serial");
            }

            PlayerData.DoubleClick(serial);

            return true;
        }

        private static bool DropItem(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 2)
            {
                throw new RunTimeError(null, "Usage: drop (serial) (x y z/layername)");
            }

            Serial serial = args[0].AsSerial();
            Point3D to = new Point3D(0, 0, 0);
            Layer layer = Layer.Invalid;

            switch (args.Length)
            {
                case 1: // drop at feet
                    to = new Point3D(World.Player.Position.X, World.Player.Position.Y, World.Player.Position.Z);
                    break;
                case 2: // dropping on a layer
                    layer = (Layer) Enum.Parse(typeof(Layer), args[1].AsString(), true);
                    break;

                default: // dropping at x/y/z
                    to = new Point3D(Utility.ToInt32(args[1].AsString(), 0), Utility.ToInt32(args[2].AsString(), 0),
                        Utility.ToInt32(args[3].AsString(), 0));
                    break;
            }

            if (DragDropManager.Holding != null)
            {
                if (layer > Layer.Invalid && layer <= Layer.LastUserValid)
                {
                    Mobile m = World.FindMobile(serial);
                    if (m != null)
                        DragDropManager.Drop(DragDropManager.Holding, m, layer);
                }
                else
                {
                    DragDropManager.Drop(DragDropManager.Holding, serial, to);
                }
            }
            else
            {
                World.Player.SendMessage(MsgLevel.Warning, LocString.MacroNoHold);
            }

            return true;
        }

        private static bool DropRelLoc(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 3)
            {
                throw new RunTimeError(null, "Usage: droprelloc (x) (y)");
            }

            int x = args[0].AsInt();
            int y = args[1].AsInt();

            if (DragDropManager.Holding != null)
                DragDropManager.Drop(DragDropManager.Holding, null,
                    new Point3D((ushort) (World.Player.Position.X + x),
                        (ushort) (World.Player.Position.Y + y), World.Player.Position.Z));
            else
                World.Player.SendMessage(LocString.MacroNoHold);

            return true;
        }

        private static bool LiftItem(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: lift (serial) [amount]");
            }

            Serial serial = args[0].AsSerial();

            if (!serial.IsValid)
            {
                throw new RunTimeError(null, "lift - invalid serial");
            }

            ushort amount = Utility.ToUInt16(args[1].AsString(), 1);

            Item item = World.FindItem(serial);
            if (item != null)
            {
                DragDropManager.Drag(item, amount <= item.Amount ? amount : item.Amount);
            }
            else
            {
                World.Player.SendMessage(MsgLevel.Warning, LocString.MacroItemOutRange);
            }

            return true;
        }

        private static bool LiftType(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: lifttype (gfx/'name of item') [amount]");
            }

            string gfxStr = args[0].AsString();
            ushort gfx = Utility.ToUInt16(gfxStr, 0);
            ushort amount = Utility.ToUInt16(args[1].AsString(), 1);

            Item item;

            // No graphic id, maybe searching by name?
            if (gfx == 0)
            {
                item = World.Player.Backpack != null ? World.Player.Backpack.FindItemByName(gfxStr, true) : null;

                if (item == null)
                {
                    throw new RunTimeError(null, $"Script Error: Couldn't find '{gfxStr}'");
                }
            }
            else
            {
                item = World.Player.Backpack != null ? World.Player.Backpack.FindItemByID(gfx) : null;
            }

            if (item != null)
            {
                if (item.Amount < amount)
                    amount = item.Amount;

                DragDropManager.Drag(item, amount);
            }
            else
            {
                World.Player.SendMessage(MsgLevel.Warning, LocString.NoItemOfType, (ItemID) gfx);
            }

            return true;
        }

        private static bool Walk(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: walk ('direction')");
            }

            if (ScriptManager.LastWalk + TimeSpan.FromSeconds(0.4) >= DateTime.UtcNow)
            {
                return false;
            }

            ScriptManager.LastWalk = DateTime.UtcNow;

            Direction dir = (Direction) Enum.Parse(typeof(Direction), args[0].AsString(), true);
            Client.Instance.RequestMove(dir);

            return true;
        }

        private static Dictionary<string, int> UsableSkills = new Dictionary<string, int>()
        {
            {"anatomy", 1}, // anatomy
            {"animallore", 2}, // animal lore
            {"itemidentification", 3}, // item identification
            {"itemid", 3}, // item identification
            {"armslore", 4}, // arms lore
            {"begging", 6}, // begging
            {"peacemaking", 9}, // peacemaking
            {"peace", 9}, // peacemaking
            {"cartography", 12}, // cartography
            {"detectinghidden", 14}, // detect hidden
            {"discord", 15}, // Discordance
            {"discordance", 15}, // Discordance
            {"evaluatingintelligence", 16}, // evaluate intelligence
            {"evalint", 16}, // evaluate intelligence
            {"forensicevaluation", 19}, // forensic evaluation
            {"forensiceval", 19}, // forensic evaluation
            {"hiding", 21}, // hiding
            {"provocation", 22}, // provocation
            {"provo", 22}, // provocation
            {"inscription", 23}, // inscription
            {"poisoning", 30}, // poisoning
            {"spiritspeak", 32}, // spirit speak
            {"stealing", 33}, // stealing
            {"taming", 35}, // taming
            {"tasteidentification", 36}, // taste id
            {"tasteid", 36}, // taste id
            {"tracking", 38}, // tracking
            {"meditation", 46}, // Meditation
            {"stealth", 47}, // Stealth
            {"removetrap", 48} // RemoveTrap
        };

        private static bool UseSkill(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: useskill ('skill name'/'last')");
            }

            if (args[0].AsString() == "last")
                Client.Instance.SendToServer(new UseSkill(World.Player.LastSkill));
            else if (UsableSkills.TryGetValue(args[0].AsString().ToLower(), out int skillId))
                Client.Instance.SendToServer(new UseSkill(skillId));

            return true;
        }

        private static bool Pause(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
                throw new RunTimeError(null, "Usage: pause (timeout)");

            Interpreter.Pause(args[0].AsUInt());

            return true;
        }

        public static bool Msg(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: msg ('text') [color]");
            }

            if (args.Length == 1)
                World.Player.Say(Config.GetInt("SysColor"), args[0].AsString());
            else
                World.Player.Say(Utility.ToInt32(args[1].AsString(), 0), args[0].AsString());

            return true;
        }

        public static bool Attack(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: attack (serial)");
            }

            Serial serial = args[0].AsSerial();

            if (!serial.IsValid)
            {
                throw new RunTimeError(null, "attack - invalid serial");
            }

            if (serial != null && serial.IsMobile)
                Client.Instance.SendToServer(new AttackReq(serial));

            return true;
        }

        public static bool Cast(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: cast 'name of spell'");
            }

            Spell spell = int.TryParse(args[0].AsString(), out int spellnum)
                ? Spell.Get(spellnum)
                : Spell.GetByName(args[0].AsString());

            if (spell != null)
            {
                spell.OnCast(new CastSpellFromMacro((ushort) spell.GetID()));
            }
            else if (!quiet)
            {
                throw new RunTimeError(null, "cast - spell name or number not valid");
            }

            return true;
        }

        public static bool HeadMsg(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: overhead ('text') [color] [serial]");
            }
            
            if (args.Length == 1)
                World.Player.OverheadMessage(Config.GetInt("SysColor"), args[0].AsString());
            else
            {
                int hue = Utility.ToInt32(args[1].AsString(), 0);

                if (args.Length == 3)
                {
                    uint serial = args[2].AsSerial();
                    Mobile m = World.FindMobile(serial);
                    m?.OverheadMessage(hue, args[0].AsString());
                }
                else
                    World.Player.OverheadMessage(hue, args[0].AsString());
            }

            return true;
        }

        public static bool SysMsg(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: sysmsg ('text') [color]");
            }

            if (args.Length == 1)
                World.Player.SendMessage(Config.GetInt("SysColor"), args[0].AsString());
            else if (args.Length == 2)
                World.Player.SendMessage(Utility.ToInt32(args[1].AsString(), 0), args[0].AsString());

            return true;
        }

        public static bool DressCommand(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: dress ('name of dress list')");
            }

            DressList d = DressList.Find(args[0].AsString());

            if (d != null)
                d.Dress();
            else if (!quiet)
                throw new RunTimeError(null, $"'{args[0].AsString()}' not found");

            return true;
        }

        public static bool UnDressCommand(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0) // full naked!
            {
                HotKeys.UndressHotKeys.OnUndressAll();
            }
            else if (args.Length == 1) // either a dress list item or a layer
            {
                DressList d = DressList.Find(args[0].AsString());

                if (d != null)
                {
                    d.Undress();
                }
                else // lets find the layer
                {
                    if (Enum.TryParse(args[0].AsString(), true, out Layer layer))
                    {
                        Dress.Unequip(layer);
                    }
                }
            }

            return true;
        }

        public static bool GumpResponse(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: gumpresponse (buttondId)");
                //throw new RunTimeError(null, "Usage: gumpresponse (buttondId) [option] ['text1'|fieldId] ['text2'|fieldId]");
            }

            int buttonId = args[0].AsInt();

            /*private int m_ButtonID;
                    private int[] m_Switches;
                    private GumpTextEntry[] m_TextEntries;*/

            //Assistant.Macros.GumpResponseAction|9|0|0
            //Assistant.Macros.GumpResponseAction|1|0|1|0&Hello How are you?
            //Assistant.Macros.GumpResponseAction|501|0|2|1&box2|0&box1

            Client.Instance.SendToClient(new CloseGump(World.Player.CurrentGumpI));
            Client.Instance.SendToServer(new GumpResponse(World.Player.CurrentGumpS, World.Player.CurrentGumpI,
                buttonId, new int[] { }, new GumpTextEntry[] { }));

            World.Player.HasGump = false;
            World.Player.HasCompressedGump = false;

            return true;
        }

        public static bool GumpClose(string command, Argument[] args, bool quiet, bool force)
        {
            Client.Instance.SendToClient(new CloseGump(World.Player.CurrentGumpI));
            Client.Instance.SendToServer(new GumpResponse(World.Player.CurrentGumpS, World.Player.CurrentGumpI, 0,
                new int[] { }, new GumpTextEntry[] { }));

            World.Player.HasGump = false;
            World.Player.HasCompressedGump = false;

            return true;
        }

        public static bool ContextMenu(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 2)
            {
                throw new RunTimeError(null, "Usage: menu (serial) (index)");
            }

            Serial s = args[0].AsSerial();
            ushort index = args[1].AsUShort();

            if (s == Serial.Zero && World.Player != null)
                s = World.Player.Serial;

            Client.Instance.SendToServer(new ContextMenuRequest(s));
            Client.Instance.SendToServer(new ContextMenuResponse(s, index));
            return true;
        }

        public static bool MenuResponse(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 2)
            {
                throw new RunTimeError(null, "Usage: menuresponse (index) (menuId) [hue]");
            }

            ushort index = args[0].AsUShort();
            ushort menuId = args[1].AsUShort();
            ushort hue = 0;

            if (args.Length == 3)
                hue = args[2].AsUShort();

            Client.Instance.SendToServer(new MenuResponse(World.Player.CurrentMenuS, World.Player.CurrentMenuI, index,
                menuId, hue));
            World.Player.HasMenu = false;
            return true;
        }

        public static bool PromptResponse(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: promptresponse ('response to the prompt')");
            }

            World.Player.ResponsePrompt(args[0].AsString());
            return true;
        }

        public static bool LastTarget(string command, Argument[] args, bool quiet, bool force)
        {
            if (!Targeting.DoLastTarget())
                Targeting.ResendTarget();

            return true;
        }

        public static bool PlayScript(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: script 'name of script'");
            }

            ScriptManager.PlayScript(args[0].AsString());

            return true;
        }

        private static readonly Dictionary<string, ushort> PotionList = new Dictionary<string, ushort>()
        {
            {"heal", 3852},
            {"cure", 3847},
            {"refresh", 3851},
            {"nightsight", 3846},
            {"ns", 3846},
            {"explosion", 3853},
            {"strength", 3849},
            {"str", 3849},
            {"agility", 3848}
        };

        private static bool Potion(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: potion ('type')");
            }
            
            Item pack = World.Player.Backpack;
            if (pack == null)
                return true;

            if (PotionList.TryGetValue(args[0].AsString().ToLower(), out ushort potionId))
            {
                if (potionId == 3852 && World.Player.Poisoned && Config.GetBool("BlockHealPoison") &&
                    Client.Instance.AllowBit(FeatureBit.BlockHealPoisoned))
                {
                    World.Player.SendMessage(MsgLevel.Force, LocString.HealPoisonBlocked);
                    return true;
                }

                if (!World.Player.UseItem(pack, potionId))
                    World.Player.SendMessage(LocString.NoItemOfType, (ItemID)potionId);
            }
            else
            {
                throw new RunTimeError(null, "Unknown potion type");
            }

            return true;
        }
    }
}