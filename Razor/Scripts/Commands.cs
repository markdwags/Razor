#region license

// Razor: An Ultima Online Assistant
// Copyright (C) 2021 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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
using System.Windows.Forms;
using Assistant.Core;
using Assistant.HotKeys;
using Assistant.Scripts.Engine;
using Assistant.Scripts.Helpers;

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

            // Using stuff
            Interpreter.RegisterCommandHandler("dclicktype", DClickType); // DoubleClickTypeAction
            Interpreter.RegisterCommandHandler("dclick", DClick); //DoubleClickAction

            Interpreter.RegisterCommandHandler("usetype", DClickType); // DoubleClickTypeAction
            Interpreter.RegisterCommandHandler("useobject", DClick); //DoubleClickAction

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

            

            Interpreter.RegisterCommandHandler("overhead", HeadMsg); //OverheadMessageAction
            Interpreter.RegisterCommandHandler("headmsg", HeadMsg); //OverheadMessageAction
            Interpreter.RegisterCommandHandler("sysmsg", SysMsg); //SystemMessageAction
            Interpreter.RegisterCommandHandler("clearsysmsg", ClearSysMsg); //SystemMessageAction
            Interpreter.RegisterCommandHandler("clearjournal", ClearSysMsg); //SystemMessageAction

            // General Waits/Pauses
            Interpreter.RegisterCommandHandler("wait", Pause); //PauseAction
            Interpreter.RegisterCommandHandler("pause", Pause); //PauseAction
            Interpreter.RegisterCommandHandler("waitforsysmsg", WaitForSysMsg);
            Interpreter.RegisterCommandHandler("wfsysmsg", WaitForSysMsg);

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

            Interpreter.RegisterCommandHandler("stop", Stop);

            Interpreter.RegisterCommandHandler("clearall", ClearAll);

            Interpreter.RegisterCommandHandler("clearhands", ClearHands);

            Interpreter.RegisterCommandHandler("virtue", Virtue);
        }

        private static string[] virtues = new string[3] { "honor", "sacrifice", "valor" };

        private static bool Virtue(string command, Argument[] args, bool quiet, bool force)
        {

            if (args.Length == 0 || !virtues.Contains(args[0].AsString()))
            {
                throw new RunTimeError(null, "Usage: virtue ('honor'/'sacrifice'/'valor')");
            }

            switch (args[0].AsString())
            {
                case "honor":
                    World.Player.InvokeVirtue(PlayerData.InvokeVirtues.Honor);
                    break;
                case "sacrifice":
                    World.Player.InvokeVirtue(PlayerData.InvokeVirtues.Sacrifice);
                    break;
                case "valor":
                    World.Player.InvokeVirtue(PlayerData.InvokeVirtues.Valor);
                    break;
            }

            return true;
        }

        private static bool ClearAll(string command, Argument[] args, bool quiet, bool force)
        {

            DragDropManager.GracefulStop(); // clear drag/drop queue
            Targeting.CancelTarget(); // clear target queue & cancel current target
            DragDropManager.DropCurrent(); // drop what you are currently holding

            return true;
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
                throw new RunTimeError(null, "Usage: setvar ('variable') [timeout]");
            }

            string varname = args[0].AsString();

            ScriptVariables.ScriptVariable var = ScriptVariables.GetVariable(varname);

            if (var == null)
            {
                throw new RunTimeError(null, $"{command} - Unknown variable '{varname}'");
            }

            Interpreter.Timeout(args.Length == 2 ? args[1].AsUInt() : 30000, () => { return true; });
            

            if (!ScriptManager.SetVariableActive)
            {
                var.SetTarget();
                ScriptManager.SetVariableActive = true;

                return false;
            }

            if (var.TargetWasSet)
            {
                Interpreter.ClearTimeout();
                ScriptManager.SetVariableActive = false;
                return true;
            }

            return false;
        }

        private static bool Stop(string command, Argument[] args, bool quiet, bool force)
        {
            ScriptManager.StopScript();

            return true;
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
                throw new RunTimeError(null, $"{command} - Hotkey '{query}' not found");
            }

            hk.Callback();

            return true;
        }

        private static bool WaitForGump(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: waitforgump (gumpId/'any') [timeout]");
            }

            uint gumpId = 0;
            bool strict = false;

            if (args[0].AsString().IndexOf("any", StringComparison.InvariantCultureIgnoreCase) != -1)
            {
                strict = false;
            }
            else
            {
                gumpId = Utility.ToUInt32(args[0].AsString(), 0);

                if (gumpId > 0)
                {
                    strict = true;
                }
            }

            Interpreter.Timeout(args.Length == 2 ? args[1].AsUInt() : 30000, () => { return true; });

            if ((World.Player.HasGump || World.Player.HasCompressedGump) &&
                (World.Player.CurrentGumpI == gumpId || !strict || gumpId == 0))
            {
                Interpreter.ClearTimeout();
                return true;
            }

            return false;
        }

        private static bool WaitForMenu(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: waitformenu (menuId/'any') [timeout]");
            }

            uint menuId = 0;

            // Look for a specific menu
            menuId = args[0].AsString().IndexOf("any", StringComparison.InvariantCultureIgnoreCase) != -1
                ? 0
                : Utility.ToUInt32(args[0].AsString(), 0);

            Interpreter.Timeout(args.Length == 2 ? args[1].AsUInt() : 30000, () => { return true; });

            if (World.Player.HasMenu && (World.Player.CurrentGumpI == menuId || menuId == 0))
            {
                Interpreter.ClearTimeout();
                return true;
            }

            return false;
        }

        private static bool WaitForPrompt(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: waitforprompt (promptId/'any') [timeout]");
            }

            uint promptId = 0;
            bool strict = false;

            // Look for a specific prompt
            if (args[0].AsString().IndexOf("any", StringComparison.InvariantCultureIgnoreCase) != -1)
            {
                strict = false;
            }
            else
            {
                promptId = Utility.ToUInt32(args[0].AsString(), 0);

                if (promptId > 0)
                {
                    strict = true;
                }
            }

            Interpreter.Timeout(args.Length == 2 ? args[1].AsUInt() : 30000, () => { return true; });

            if (World.Player.HasPrompt && (World.Player.PromptID == promptId || !strict || promptId == 0))
            {
                Interpreter.ClearTimeout();
                return true;
            }

            return false;
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

        private static string[] hands = new string[4] {"left", "right", "both", "hands"};

        private static bool ClearHands(string command, Argument[] args, bool quiet, bool force)
        {
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

        private static bool DClickType(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null,
                    "Usage: dclicktype ('name of item') OR (graphicID) [inrangecheck (true/false)/backpack]");
            }

            string gfxStr = args[0].AsString();
            Serial gfx = Utility.ToUInt16(gfxStr, 0);
            List<Item> items;
            List<Mobile> mobiles = new List<Mobile>();

            bool inRangeCheck = false;
            bool backpack = false;

            if (args.Length == 2)
            {
                if (args[1].AsString().IndexOf("pack", StringComparison.InvariantCultureIgnoreCase) > 0)
                {
                    backpack = true;
                }
                else
                {
                    inRangeCheck = args[1].AsBool();
                }
            }

            // No graphic id, maybe searching by name?
            if (gfx == 0)
            {
                items = CommandHelper.GetItemsByName(gfxStr, backpack, inRangeCheck);

                if (items.Count == 0) // no item found, search mobile by name
                {
                    mobiles = CommandHelper.GetMobilesByName(gfxStr, inRangeCheck);
                }
            }
            else // Provided graphic id for type, check backpack first (same behavior as DoubleClickAction in macros
            {
                ushort id = Utility.ToUInt16(gfxStr, 0);

                items = CommandHelper.GetItemsById(id, backpack, inRangeCheck);
                
                // Still no item? Mobile check!
                if (items.Count == 0)
                {
                    mobiles = CommandHelper.GetMobilesById(id, inRangeCheck);
                }
            }

            if (items.Count > 0)
            {
                PlayerData.DoubleClick(items[Utility.Random(items.Count)].Serial);
            }
            else if (mobiles.Count > 0)
            {
                PlayerData.DoubleClick(mobiles[Utility.Random(mobiles.Count)].Serial);
            }
            else
            {
                CommandHelper.SendWarning(command, $"Item or mobile type '{gfxStr}' not found", quiet);
            }

            return true;
        }

        private static bool DClick(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: dclick (serial) or dclick ('left'/'right'/'hands')");
            }

            if (hands.Contains(args[0].AsString()))
            {
                Item item;

                switch (args[0].AsString())
                {
                    case "left":
                        item = World.Player.GetItemOnLayer(Layer.LeftHand);
                        break;
                    case "right":
                        item = World.Player.GetItemOnLayer(Layer.RightHand);
                        break;
                    default:
                        item = World.Player.GetItemOnLayer(Layer.RightHand) ?? World.Player.GetItemOnLayer(Layer.LeftHand);
                        break;
                }

                if (item != null)
                {
                    PlayerData.DoubleClick(item);
                }
                else
                {
                    CommandHelper.SendWarning(command, $"Item not found in '{args[0].AsString()}'", quiet);
                }
            }
            else
            {
                Serial serial = args[0].AsSerial();

                if (!serial.IsValid)
                {
                    throw new RunTimeError(null, "dclick - invalid serial");
                }

                PlayerData.DoubleClick(serial);
            }

            return true;
        }

        private static bool DropItem(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: drop (serial) (x y z/layername)");
            }

            Serial serial = args[0].AsString().IndexOf("ground", StringComparison.InvariantCultureIgnoreCase) > 0
                ? uint.MaxValue
                : args[0].AsSerial();

            Point3D to = new Point3D(0, 0, 0);
            Layer layer = Layer.Invalid;

            switch (args.Length)
            {
                case 1: // drop at feet if only serial is provided
                    to = new Point3D(World.Player.Position.X, World.Player.Position.Y, World.Player.Position.Z);
                    break;
                case 2: // dropping on a layer
                    layer = (Layer) Enum.Parse(typeof(Layer), args[1].AsString(), true);
                    break;
                case 3: // x y
                    to = new Point3D(Utility.ToInt32(args[1].AsString(), 0), Utility.ToInt32(args[2].AsString(), 0), 0);
                    break;
                case 4: // x y z
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
                CommandHelper.SendWarning(command, "Not holding anything", quiet);
            }

            return true;
        }

        private static bool DropRelLoc(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 2)
            {
                throw new RunTimeError(null, "Usage: droprelloc (x) (y)");
            }

            int x = args[0].AsInt();
            int y = args[1].AsInt();

            if (DragDropManager.Holding != null)
            {
                DragDropManager.Drop(DragDropManager.Holding, null,
                    new Point3D((ushort) (World.Player.Position.X + x),
                        (ushort) (World.Player.Position.Y + y), World.Player.Position.Z));
            }
            else
            {
                CommandHelper.SendWarning(command, "Not holding anything", quiet);
            }

            return true;
        }

        private static int _lastLiftId;

        private static bool LiftItem(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: lift (serial) [amount]");
            }

            Serial serial = args[0].AsSerial();

            if (!serial.IsValid)
            {
                throw new RunTimeError(null, $"{command} - Invalid serial");
            }

            ushort amount = 1;

            if (args.Length == 2)
            {
                amount = Utility.ToUInt16(args[1].AsString(), 1);
            }

            if (_lastLiftId > 0)
            {
                if (DragDropManager.LastIDLifted == _lastLiftId)
                {
                    _lastLiftId = 0;
                    Interpreter.ClearTimeout();
                    return true;
                }

                Interpreter.Timeout(30000, () =>
                {
                    _lastLiftId = 0;
                    return true;
                });
            }
            else
            {
                Item item = World.FindItem(serial);

                if (item != null)
                {
                    _lastLiftId = DragDropManager.Drag(item, amount <= item.Amount ? amount : item.Amount);
                }
                else
                {
                    CommandHelper.SendWarning(command, "Item not found or out of range", quiet);
                    return true;
                }
            }

            return false;
        }

        private static int _lastLiftTypeId;

        private static bool LiftType(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: lifttype (gfx/'name of item') [amount]");
            }

            string gfxStr = args[0].AsString();
            ushort gfx = Utility.ToUInt16(gfxStr, 0);
            ushort amount = 1;

            if (args.Length == 2)
            {
                amount = Utility.ToUInt16(args[1].AsString(), 1);
            }

            if (_lastLiftTypeId > 0)
            {
                if (DragDropManager.LastIDLifted == _lastLiftTypeId)
                {
                    _lastLiftTypeId = 0;
                    Interpreter.ClearTimeout();
                    return true;
                }

                Interpreter.Timeout(30000, () =>
                {
                    _lastLiftTypeId = 0;
                    return true;
                });
            }
            else
            {
                Item item;

                // No graphic id, maybe searching by name?
                if (gfx == 0)
                {
                    item = World.Player.Backpack?.FindItemByName(gfxStr, true);

                    if (item == null)
                    {
                        CommandHelper.SendWarning(command, $"Item '{gfxStr}' not found", quiet);
                        return true;
                    }
                }
                else
                {
                    item = World.Player.Backpack?.FindItemByID(gfx);
                }

                if (item != null)
                {
                    if (item.Amount < amount)
                        amount = item.Amount;

                    _lastLiftTypeId = DragDropManager.Drag(item, amount);
                }
                else
                {
                    CommandHelper.SendWarning(command, Language.Format(LocString.NoItemOfType, (ItemID)gfx), quiet);
                    return true;
                }
            }

            return false;
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

        private static bool UseSkill(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: skill ('skill name'/'last')");
            }

            if (args[0].AsString() == "last")
            {
                Client.Instance.SendToServer(new UseSkill(World.Player.LastSkill));
            }
            else if (SkillHotKeys.UsableSkillsByName.TryGetValue(args[0].AsString().ToLower(), out int skillId))
            {
                Client.Instance.SendToServer(new UseSkill(skillId));
            }

            return true;
        }

        private static bool Pause(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
                throw new RunTimeError(null, "Usage: pause/wait (timeout)");

            Interpreter.Pause(args[0].AsUInt());

            return true;
        }

        private static bool Attack(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: attack (serial)");
            }

            Serial serial = args[0].AsSerial();

            if (!serial.IsValid)
            {
                throw new RunTimeError(null, $"{command} - Invalid serial");
            }

            if (serial == Targeting.LastTargetInfo.Serial)
            {
                Targeting.AttackLastTarg();
            }
            else
            {
                if (serial.IsMobile)
                    Client.Instance.SendToServer(new AttackReq(serial));
            }

            return true;
        }

        private static bool Cast(string command, Argument[] args, bool quiet, bool force)
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
            else
            {
                throw new RunTimeError(null, $"{command} - Spell name or number not valid");
            }

            return true;
        }

        private static bool HeadMsg(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: overhead ('text') [color] [serial]");
            }

            if (args.Length == 1)
            {
                World.Player.OverheadMessage(Config.GetInt("SysColor"), args[0].AsString());
            }
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
                {
                    World.Player.OverheadMessage(hue, args[0].AsString());
                }
            }

            return true;
        }

        private static bool SysMsg(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: sysmsg ('text') [color]");
            }

            if (args.Length == 1)
            {
                World.Player.SendMessage(Config.GetInt("SysColor"), args[0].AsString());
            }
            else if (args.Length == 2)
            {
                World.Player.SendMessage(Utility.ToInt32(args[1].AsString(), 0), args[0].AsString());
            }

            return true;
        }

        private static bool ClearSysMsg(string command, Argument[] args, bool quiet, bool force)
        {
            SystemMessages.Messages.Clear();

            return true;
        }

        private static DressList _lastDressList;

        private static bool DressCommand(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: dress ('name of dress list')");
            }

            if (_lastDressList == null)
            {
                _lastDressList = DressList.Find(args[0].AsString());

                if (_lastDressList != null)
                {
                    _lastDressList.Dress();
                }
                else if (!quiet)
                {
                    CommandHelper.SendWarning(command, $"'{args[0].AsString()}' not found", quiet);
                    return true;
                }
            }
            else if (ActionQueue.Empty)
            {
                _lastDressList = null;
                return true;
            }

            return false;
        }

        private static DressList _lastUndressList;
        private static bool _undressAll;

        private static bool UnDressCommand(string command, Argument[] args, bool quiet, bool force)
        {

            if (args.Length == 0 && !_undressAll) // full naked!
            {
                _undressAll = true;
                UndressHotKeys.OnUndressAll();
            }
            else if (args.Length == 1 && _lastUndressList == null) // either a dress list item or a layer
            {
                _lastUndressList = DressList.Find(args[0].AsString());

                if (_lastUndressList != null)
                {
                    _lastUndressList.Undress();
                }
                else // lets find the layer
                {
                    if (Enum.TryParse(args[0].AsString(), true, out Layer layer))
                    {
                        Dress.Unequip(layer);
                    }
                    else
                    {
                        throw new RunTimeError(null, $"'{args[0].AsString()}' not found");
                    }
                }
            }
            else if (ActionQueue.Empty)
            {
                _undressAll = false;
                _lastUndressList = null;
                return true;
            }

            return false;
        }

        private static bool GumpResponse(string command, Argument[] args, bool quiet, bool force)
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

        private static bool GumpClose(string command, Argument[] args, bool quiet, bool force)
        {
            Client.Instance.SendToClient(new CloseGump(World.Player.CurrentGumpI));
            Client.Instance.SendToServer(new GumpResponse(World.Player.CurrentGumpS, World.Player.CurrentGumpI, 0,
                new int[] { }, new GumpTextEntry[] { }));

            World.Player.HasGump = false;
            World.Player.HasCompressedGump = false;

            return true;
        }

        private static bool ContextMenu(string command, Argument[] args, bool quiet, bool force)
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

        private static bool MenuResponse(string command, Argument[] args, bool quiet, bool force)
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

        private static bool PromptResponse(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: promptresponse ('response to the prompt')");
            }

            World.Player.ResponsePrompt(args[0].AsString());
            return true;
        }

        private static bool LastTarget(string command, Argument[] args, bool quiet, bool force)
        {
            if (!Targeting.DoLastTarget())
                Targeting.ResendTarget();

            return true;
        }

        private static bool PlayScript(string command, Argument[] args, bool quiet, bool force)
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
                {
                    CommandHelper.SendWarning(command, Language.Format(LocString.NoItemOfType, (ItemID)potionId), quiet);
                }
            }
            else
            {
                throw new RunTimeError(null, $"{command} - Unknown potion type");
            }

            return true;
        }

        private static bool WaitForSysMsg(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: waitforsysmsg 'message to wait for' [timeout]");
            }
            
            if (SystemMessages.Exists(args[0].AsString()))
            {
                Interpreter.ClearTimeout();
                return true;
            }

            Interpreter.Timeout(args.Length > 1 ? args[1].AsUInt() : 30000, () => { return true; });

            return false;
        }
    }
}