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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Assistant.Core;
using Assistant.HotKeys;
using Assistant.Scripts.Engine;
using Assistant.Scripts.Helpers;
using Ultima;

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

            

            Interpreter.RegisterCommandHandler("overhead", OverheadMessage); //OverheadMessageAction
            Interpreter.RegisterCommandHandler("headmsg", OverheadMessage); //OverheadMessageAction
            Interpreter.RegisterCommandHandler("sysmsg", SystemMessage); //SystemMessageAction
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

            Interpreter.RegisterCommandHandler("random", Random);

            Interpreter.RegisterCommandHandler("cleardragdrop", ClearDragDrop);
            Interpreter.RegisterCommandHandler("interrupt", Interrupt);

            Interpreter.RegisterCommandHandler("sound", Sound);
            Interpreter.RegisterCommandHandler("music", Music);

            Interpreter.RegisterCommandHandler("classicuo", ClassicUOProfile);
            Interpreter.RegisterCommandHandler("cuo", ClassicUOProfile);
            
            Interpreter.RegisterCommandHandler("rename", Rename);
            
            Interpreter.RegisterCommandHandler("getlabel", GetLabel);
            
            Interpreter.RegisterCommandHandler("ignore", AddIgnore);
            Interpreter.RegisterCommandHandler("unignore", RemoveIgnore);
            Interpreter.RegisterCommandHandler("clearignore", ClearIgnore);
            
            Interpreter.RegisterCommandHandler("cooldown", Cooldown);
            
            Interpreter.RegisterCommandHandler("poplist", PopList);
            Interpreter.RegisterCommandHandler("pushlist", PushList);
            Interpreter.RegisterCommandHandler("removelist", RemoveList);
            Interpreter.RegisterCommandHandler("createlist", CreateList);
            Interpreter.RegisterCommandHandler("clearlist", ClearList);
            
            Interpreter.RegisterCommandHandler("settimer", SetTimer);
            Interpreter.RegisterCommandHandler("removetimer", RemoveTimer);
            Interpreter.RegisterCommandHandler("createtimer", CreateTimer);
        }
        
        private static bool PopList(string command, Variable[] args, bool quiet, bool force)
        {
            if (args.Length != 2)
                throw new RunTimeError("Usage: poplist ('list name') ('element value'/'front'/'back')");

            if (args[1].AsString() == "front")
            {
                if (force)
                    while (Interpreter.PopList(args[0].AsString(), true, out _)) { }
                else
                    Interpreter.PopList(args[0].AsString(), true, out _);
            }
            else if (args[1].AsString() == "back")
            {
                if (force)
                    while (Interpreter.PopList(args[0].AsString(), false, out _)) { }
                else
                    Interpreter.PopList(args[0].AsString(), false, out _);
            }
            else
            {
                var evaluatedVar = new Variable(args[1].AsString());
                if (force)
                {
                    while (Interpreter.PopList(args[0].AsString(), evaluatedVar)) { }
                }
                else
                    Interpreter.PopList(args[0].AsString(), evaluatedVar);
            }

            return true;
        }

        private static bool PushList(string command, Variable[] args, bool quiet, bool force)
        {
            if (args.Length < 2 || args.Length > 3)
                throw new RunTimeError("Usage: pushlist ('list name') ('element value') ['front'/'back']");

            bool front = false;
            if (args.Length == 3)
            {
                if (args[2].AsString() == "front")
                    front = true;
            }

            Interpreter.PushList(args[0].AsString(), new Variable(args[1].AsString()), front, force);

            return true;
        }

        private static bool RemoveList(string command, Variable[] args, bool quiet, bool force)
        {
            if (args.Length != 1)
                throw new RunTimeError("Usage: removelist ('list name')");

            Interpreter.DestroyList(args[0].AsString());

            return true;
        }

        private static bool CreateList(string command, Variable[] args, bool quiet, bool force)
        {
            if (args.Length != 1)
                throw new RunTimeError("Usage: createlist ('list name')");

            Interpreter.CreateList(args[0].AsString());

            return true;
        }

        private static bool ClearList(string command, Variable[] args, bool quiet, bool force)
        {
            if (args.Length != 1)
                throw new RunTimeError("Usage: clearlist ('list name')");

            Interpreter.ClearList(args[0].AsString());

            return true;
        }

        private static bool SetTimer(string command, Variable[] args, bool quiet, bool force)
        {
            if (args.Length != 2)
                throw new RunTimeError("Usage: settimer (timer name) (value)");


            Interpreter.SetTimer(args[0].AsString(), args[1].AsInt());
            return true;
        }

        private static bool RemoveTimer(string command, Variable[] args, bool quiet, bool force)
        {
            if (args.Length != 1)
                throw new RunTimeError("Usage: removetimer (timer name)");

            Interpreter.RemoveTimer(args[0].AsString());
            return true;
        }

        private static bool CreateTimer(string command, Variable[] args, bool quiet, bool force)
        {
            if (args.Length != 1)
                throw new RunTimeError("Usage: createtimer (timer name)");

            Interpreter.CreateTimer(args[0].AsString());
            return true;
        }

        private static bool Cooldown(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 2)
            {
                throw new RunTimeError("Usage: cooldown ('name') ('seconds') ['hue'] ['icon'] ['sound'] ['stay visible'] ['foreground color'] ['background color']");
            }

            string name = vars[0].AsString();
            int seconds = vars[1].AsInt();
            
            int hue = 0, sound = 0;
            string icon = "none";
            bool stay = false;
            
            Color foreColor = Color.Empty;
            Color backColor = Color.Empty;
            
            switch (vars.Length)
            {
                case 3:
                    hue = vars[2].AsInt();

                    break;
                case 4:
                    hue = vars[2].AsInt();
                    icon = vars[3].AsString();

                    break;
                case 5:
                    hue = vars[2].AsInt();
                    icon = vars[3].AsString();
                    sound = vars[4].AsInt();

                    break;
                case 6:
                    hue = vars[2].AsInt();
                    icon = vars[3].AsString();
                    sound = vars[4].AsInt();
                    stay = vars[5].AsBool();

                    break;
                case 7:
                    hue = vars[2].AsInt();
                    icon = vars[3].AsString();
                    sound = vars[4].AsInt();
                    stay = vars[5].AsBool();

                    foreColor = Color.FromName(vars[6].AsString());

                    break;
                case 8:
                    hue = vars[2].AsInt();
                    icon = vars[3].AsString();
                    sound = vars[4].AsInt();
                    stay = vars[5].AsBool();

                    foreColor = Color.FromName(vars[6].AsString());
                    backColor = Color.FromName(vars[7].AsString());

                    break;
            }
            
            CooldownManager.AddCooldown(new Cooldown
            {
                Name = name,
                EndTime = DateTime.UtcNow.AddSeconds(seconds),
                Hue = hue,
                Icon = BuffDebuffManager.GetGraphicId(icon),
                Seconds = seconds,
                SoundId = sound,
                StayVisible = stay,
                ForegroundColor = foreColor,
                BackgroundColor = backColor
            });

            return true;
        }
        
        private enum GetLabelState
        {
            None,
            WaitingForFirstLabel,
            WaitingForRemainingLabels
        };
        
        private static GetLabelState _getLabelState = GetLabelState.None;
        private static Action<Packet, PacketHandlerEventArgs, Serial, ushort, MessageType, ushort, ushort, string, string, string> _onLabelMessage;
        private static Action _onStop;
        
        private static bool GetLabel(string command, Variable[] args, bool quiet, bool force)
        {
            if (args.Length != 2)
                throw new RunTimeError("Usage: getlabel (serial) (name)");

            var serial = args[0].AsSerial();
            var name = args[1].AsString(false);

            var mobile = World.FindMobile(serial);
            if (mobile != null)
            {
                if (mobile.IsHuman)
                {
                    return false;
                }
            }

            switch (_getLabelState)
            {
                case GetLabelState.None:
                    _getLabelState = GetLabelState.WaitingForFirstLabel;
                    Interpreter.Timeout(2000, () =>
                    {
                        MessageManager.OnLabelMessage -= _onLabelMessage;
                        _onLabelMessage = null;
                        Interpreter.OnStop -= _onStop;
                        _getLabelState = GetLabelState.None;
                        MessageManager.GetLabelCommand = false;
                        return true;
                    });

                    // Single click the object
                    Client.Instance.SendToServer(new SingleClick((Serial)args[0].AsSerial()));

                    // Capture all message responses
                    StringBuilder label = new StringBuilder();
                    
                    // Some messages from Outlands server are send in sequence of LabelType and RegularType
                    // so we want to invoke that _onLabelMessage in both cases with delays
                    MessageManager.GetLabelCommand = true;

                    // Reset the state when script is stopped
                    _onStop = () =>
                    {
                        if (_onLabelMessage != null)
                        {
                            MessageManager.OnLabelMessage -= _onLabelMessage;
                            _onLabelMessage = null;
                        }
                        _getLabelState = GetLabelState.None;
                        
                        Interpreter.OnStop -= _onStop;
                        MessageManager.GetLabelCommand = false;
                    };

                    _onLabelMessage = (p, a, source, graphic, type, hue, font, lang, sourceName, text) =>
                    {
                        if (source != serial)
                            return;

                        a.Block = true;

                        if (_getLabelState == GetLabelState.WaitingForFirstLabel)
                        {
                            // After the first message, switch to a pause instead of a timeout.
                            _getLabelState = GetLabelState.WaitingForRemainingLabels;
                            Interpreter.Pause(500);
                        }

                        label.Append(" " + text);

                        Interpreter.SetVariable(name, label.ToString().Trim());
                    };

                    Interpreter.OnStop += _onStop;
                    MessageManager.OnLabelMessage += _onLabelMessage;
                    
                    break;
                case GetLabelState.WaitingForFirstLabel:
                    break;
                case GetLabelState.WaitingForRemainingLabels:
                    // We get here after the pause has expired.
                    Interpreter.OnStop -= _onStop;
                    MessageManager.OnLabelMessage -= _onLabelMessage;
                    
                    _onLabelMessage = null;
                    _getLabelState = GetLabelState.None;
                    
                    MessageManager.GetLabelCommand = false;
                    
                    return true;
            }

            return false;
        }

        private static bool Rename(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 2)
            {
                throw new RunTimeError("Usage: rename (serial) (new_name)");
            }

            string newName = vars[1].AsString();
            
            if (newName.Length < 1)
            {
                throw new RunTimeError("Mobile name must be longer than one character");
            }

            if (World.Mobiles.TryGetValue(vars[0].AsSerial(), out var follower))
            {
                if (follower.CanRename)
                {
                    PlayerData.RenameMobile(follower.Serial, newName);
                }
                else
                {
                    CommandHelper.SendMessage("Unable to rename mobile", quiet);
                }
            }
            
            return true;
        }

        private static bool ClassicUOProfile(string commands, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length != 2)
            {
                throw new RunTimeError("Usage: cuo (setting) (value)");
            }

            string property = ClassicUOManager.IsValidProperty(vars[0].AsString());

            if (string.IsNullOrEmpty(property))
            {
                throw new RunTimeError("Unknown ClassicUO setting/property. Type `>cuo list` for a list of valid settings.");
            }

            bool isNumeric = int.TryParse(vars[0].AsString(), out var value);

            if (isNumeric)
            {
                ClassicUOManager.ProfilePropertySet(property, value);
            }
            else
            {
                switch (vars[1].AsString())
                {
                    case "true":
                        ClassicUOManager.ProfilePropertySet(property, true);
                        break;
                    case "false":
                        ClassicUOManager.ProfilePropertySet(property, false);
                        break;
                    default:
                        ClassicUOManager.ProfilePropertySet(property, vars[1].AsString());
                        break;
                }
            }

            CommandHelper.SendMessage($"ClassicUO Setting: '{property}' set to '{vars[1].AsString()}'", quiet);

            return true;
        }

        private static bool Sound(string commands, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length != 1)
            {
                throw new RunTimeError("Usage: sound (serial)");
            }

            Client.Instance.SendToClient(new PlaySound(vars[0].AsInt()));

            return true;
        }

        private static bool Music(string commands, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length != 1)
            {
                throw new RunTimeError("Usage: music (id)");
            }

            Client.Instance.SendToClient(new PlayMusic(vars[0].AsUShort()));

            return true;
        }

        private static bool AddIgnore(string commands, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length != 1)
            {
                throw new RunTimeError("Usage: ignore ('serial')");
            }

            var serial = vars[0].AsSerial();
            Interpreter.AddIgnore(serial);

            CommandHelper.SendMessage($"Added '{serial}' to ignore list", quiet);

            return true;
        }
        
        private static bool RemoveIgnore(string commands, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length != 1)
            {
                throw new RunTimeError("Usage: unignore ('serial')");
            }

            var serial = vars[0].AsSerial();
            Interpreter.AddIgnore(serial);

            CommandHelper.SendMessage($"Removed '{serial}' from ignore list", quiet);

            return true;
        }

        private static bool ClearIgnore(string commands, Variable[] vars, bool quiet, bool force)
        {
            Interpreter.ClearIgnore();

            CommandHelper.SendMessage("Ignore List cleared", quiet);

            return true;
        }

        private static readonly string[] Virtues = { "honor", "sacrifice", "valor" };

        private static bool Virtue(string command, Variable[] vars, bool quiet, bool force)
        {

            if (vars.Length == 0 || !Virtues.Contains(vars[0].AsString()))
            {
                throw new RunTimeError("Usage: virtue ('honor'/'sacrifice'/'valor')");
            }

            switch (vars[0].AsString())
            {
                case "honor":
                    PlayerData.InvokeVirtue(PlayerData.InvokeVirtues.Honor);
                    break;
                case "sacrifice":
                    PlayerData.InvokeVirtue(PlayerData.InvokeVirtues.Sacrifice);
                    break;
                case "valor":
                    PlayerData.InvokeVirtue(PlayerData.InvokeVirtues.Valor);
                    break;
            }

            return true;
        }

        private static bool ClearAll(string command, Variable[] vars, bool quiet, bool force)
        {

            DragDropManager.GracefulStop(); // clear drag/drop queue
            Targeting.CancelTarget(); // clear target queue & cancel current target
            DragDropManager.DropCurrent(); // drop what you are currently holding

            return true;
        }

        private static bool SetLastTarget(string command, Variable[] vars, bool quiet, bool force)
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

        private static bool SetVar(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1)
            {
                throw new RunTimeError("Usage: setvar ('variable') ['serial'] [timeout]");
            }

            string name = vars[0].AsString();
            Serial serial = Serial.Zero;

            if (vars.Length == 2)
            {
                serial = vars[1].AsSerial();

                if (force)
                {
                    Interpreter.SetVariable(name, serial.ToString());
                    return true;
                }
            }

            ScriptVariables.ScriptVariable variable = ScriptVariables.GetVariable(name);
            
            if (variable == null) // new variable
            {
                World.Player.SendMessage(MsgLevel.Info, $"'{name}' not found, creating new variable", quiet);

                variable = new ScriptVariables.ScriptVariable(name, new TargetInfo());

                ScriptVariables.ScriptVariableList.Add(variable);
                ScriptVariables.RegisterVariable(name);
            }


            if (serial != Serial.Zero) // they supplied a serial
            {
                variable.TargetInfo.Serial = serial;
                World.Player.SendMessage(MsgLevel.Info, $"'{name}' script variable updated to '{variable.TargetInfo.Serial}'", quiet);

                ScriptManager.RedrawScriptVariables();

                return true;
            }

            Interpreter.Timeout(vars.Length == 3 ? vars[2].AsUInt() : 30000, () => { return true; });

            if (!ScriptManager.SetVariableActive)
            {
                variable.SetTarget();
                ScriptManager.SetVariableActive = true;

                return false;
            }

            if (variable.TargetWasSet)
            {
                Interpreter.ClearTimeout();
                ScriptManager.SetVariableActive = false;

                World.Player.SendMessage(MsgLevel.Info, $"'{name}' script variable updated to '{variable.TargetInfo.Serial}'", quiet);

                ScriptManager.RedrawScriptVariables();

                return true;
            }

            return false;
        }

        private static bool Stop(string command, Variable[] vars, bool quiet, bool force)
        {
            ScriptManager.StopScript();

            return true;
        }

        private static bool Hotkey(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1)
            {
                throw new RunTimeError("Usage: hotkey ('name of hotkey') OR (hotkeyId)");
            }

            string query = vars[0].AsString();

            KeyData hk = HotKey.GetByNameOrId(query);

            if (hk == null)
            {
                throw new RunTimeError($"{command} - Hotkey '{query}' not found");
            }

            hk.Callback();

            return true;
        }

        private static bool WaitForGump(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1)
            {
                throw new RunTimeError("Usage: waitforgump (gumpId/'any') [timeout]");
            }

            uint gumpId = 0;
            bool strict = false;

            if (vars[0].AsString().IndexOf("any", StringComparison.OrdinalIgnoreCase) != -1)
            {
                strict = false;
            }
            else
            {
                gumpId = Utility.ToUInt32(vars[0].AsString(), 0);

                if (gumpId > 0)
                {
                    strict = true;
                }
            }

            Interpreter.Timeout(vars.Length == 2 ? vars[1].AsUInt() : 30000, () => { return true; });

            if ((World.Player.HasGump || World.Player.HasCompressedGump) &&
                (World.Player.CurrentGumpI == gumpId || !strict || gumpId == 0))
            {
                Interpreter.ClearTimeout();
                return true;
            }

            return false;
        }

        private static bool WaitForMenu(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1)
            {
                throw new RunTimeError("Usage: waitformenu (menuId/'any') [timeout]");
            }

            uint menuId = 0;

            // Look for a specific menu
            menuId = vars[0].AsString().IndexOf("any", StringComparison.OrdinalIgnoreCase) != -1
                ? 0
                : Utility.ToUInt32(vars[0].AsString(), 0);

            Interpreter.Timeout(vars.Length == 2 ? vars[1].AsUInt() : 30000, () => { return true; });

            if (World.Player.HasMenu && (World.Player.CurrentGumpI == menuId || menuId == 0))
            {
                Interpreter.ClearTimeout();
                return true;
            }

            return false;
        }

        private static bool WaitForPrompt(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1)
            {
                throw new RunTimeError("Usage: waitforprompt (promptId/'any') [timeout]");
            }

            uint promptId = 0;
            bool strict = false;

            // Look for a specific prompt
            if (vars[0].AsString().IndexOf("any", StringComparison.OrdinalIgnoreCase) != -1)
            {
                strict = false;
            }
            else
            {
                promptId = Utility.ToUInt32(vars[0].AsString(), 0);

                if (promptId > 0)
                {
                    strict = true;
                }
            }

            Interpreter.Timeout(vars.Length == 2 ? vars[1].AsUInt() : 30000, () => { return true; });

            if (World.Player.HasPrompt && (World.Player.PromptID == promptId || !strict || promptId == 0))
            {
                Interpreter.ClearTimeout();
                return true;
            }

            return false;
        }

        private static readonly string[] Abilities = {"primary", "secondary", "stun", "disarm"};

        private static bool SetAbility(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1 || !Abilities.Contains(vars[0].AsString()))
            {
                throw new RunTimeError("Usage: setability ('primary'/'secondary'/'stun'/'disarm') ['on'/'off']");
            }

            if (vars.Length == 2 && vars[1].AsString() == "on" || vars.Length == 1)
            {
                switch (vars[0].AsString())
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
                }
            }
            else if (vars.Length == 2 && vars[1].AsString() == "off")
            {
                Client.Instance.SendToServer(new UseAbility(AOSAbility.Clear));
                Client.Instance.SendToClient(ClearAbility.Instance);
            }

            return true;
        }

        private static readonly string[] Hands = {"left", "right", "both", "hands"};

        private static bool ClearHands(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length == 0 || !Hands.Contains(vars[0].AsString()))
            {
                throw new RunTimeError("Usage: clearhands ('left'/'right'/'both')");
            }

            switch (vars[0].AsString())
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

        private static bool DClickType(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length == 0)
            {
                throw new RunTimeError("Usage: dclicktype ('name of item'/'graphicID') [inrangecheck (true/false)/backpack] [hue]");
            }

            string gfxStr = vars[0].AsString();
            Serial gfx = Utility.ToUInt16(gfxStr, 0);
            List<Item> items;
            List<Mobile> mobiles = new List<Mobile>();

            bool inRangeCheck = false;
            bool backpack = false;
            int hue = -1;

            if (vars.Length > 1)
            {
                if (vars.Length == 3)
                {
                    hue = vars[2].AsInt();
                }

                if (vars[1].AsString().IndexOf("pack", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    backpack = true;
                }
                else
                {
                    inRangeCheck = vars[1].AsBool();
                }
            }

            // No graphic id, maybe searching by name?
            if (gfx == 0)
            {
                items = CommandHelper.GetItemsByName(gfxStr, backpack, inRangeCheck, hue);

                if (items.Count == 0) // no item found, search mobile by name
                {
                    mobiles = CommandHelper.GetMobilesByName(gfxStr, inRangeCheck);
                }
            }
            else // Provided graphic id for type, check backpack first (same behavior as DoubleClickAction in macros
            {
                ushort id = Utility.ToUInt16(gfxStr, 0);

                items = CommandHelper.GetItemsById(id, backpack, inRangeCheck, hue);
                
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

        private static bool DClick(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length == 0)
            {
                throw new RunTimeError("Usage: dclick (serial) or dclick ('left'/'right'/'hands')");
            }

            if (Hands.Contains(vars[0].AsString()))
            {
                Item item;

                switch (vars[0].AsString())
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
                    CommandHelper.SendWarning(command, $"Item not found in '{vars[0].AsString()}'", quiet);
                }
            }
            else
            {
                Serial serial = vars[0].AsSerial();

                if (!serial.IsValid)
                {
                    throw new RunTimeError("dclick - invalid serial");
                }

                PlayerData.DoubleClick(serial);
            }

            return true;
        }

        private static bool DropItem(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1)
            {
                throw new RunTimeError("Usage: drop (serial) (x y z/layername)");
            }

            Serial serial = vars[0].AsString().IndexOf("ground", StringComparison.OrdinalIgnoreCase) > 0
                ? uint.MaxValue
                : vars[0].AsSerial();

            Point3D to = new Point3D(0, 0, 0);
            Layer layer = Layer.Invalid;

            switch (vars.Length)
            {
                case 1: // drop at feet if only serial is provided
                    to = new Point3D(World.Player.Position.X, World.Player.Position.Y, World.Player.Position.Z);
                    break;
                case 2: // dropping on a layer
                    layer = (Layer) Enum.Parse(typeof(Layer), vars[1].AsString(), true);
                    break;
                case 3: // x y
                    to = new Point3D(Utility.ToInt32(vars[1].AsString(), 0), Utility.ToInt32(vars[2].AsString(), 0), 0);
                    break;
                case 4: // x y z
                    to = new Point3D(Utility.ToInt32(vars[1].AsString(), 0), Utility.ToInt32(vars[2].AsString(), 0),
                        Utility.ToInt32(vars[3].AsString(), 0));
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

        private static bool DropRelLoc(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 2)
            {
                throw new RunTimeError("Usage: droprelloc (x) (y)");
            }

            int x = vars[0].AsInt();
            int y = vars[1].AsInt();

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

        private static bool LiftItem(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1)
            {
                throw new RunTimeError("Usage: lift (serial) [amount]");
            }

            Serial serial = vars[0].AsSerial();

            if (!serial.IsValid)
            {
                throw new RunTimeError($"{command} - Invalid serial");
            }

            ushort amount = 1;

            if (vars.Length == 2)
            {
                amount = Utility.ToUInt16(vars[1].AsString(), 1);
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

        private static bool LiftType(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1)
            {
                throw new RunTimeError("Usage: lifttype (gfx/'name of item') [amount] [hue]");
            }

            string gfxStr = vars[0].AsString();
            ushort gfx = Utility.ToUInt16(gfxStr, 0);
            ushort amount = 1;
            int hue = -1;

            if (vars.Length > 1)
            {
                if (vars.Length >= 2)
                {
                    amount = Utility.ToUInt16(vars[1].AsString(), 1);
                }

                if (vars.Length == 3)
                {
                    hue = Utility.ToUInt16(vars[2].AsString(), 0);
                }
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
                List<Item> items = new List<Item>();

                // No graphic id, maybe searching by name?
                if (gfx == 0)
                {
                    items = World.Player.Backpack.FindItemsByName(gfxStr, true);

                    if (items.Count == 0)
                    {
                        CommandHelper.SendWarning(command, $"Item '{gfxStr}' not found", quiet);
                        return true;
                    }
                }
                else
                {
                    items = World.Player.Backpack.FindItemsById(gfx);
                }

                if (hue > -1)
                {
                    items.RemoveAll(item => item.Hue != hue);
                }

                if (items.Count > 0)
                {
                    Item item = items[Utility.Random(items.Count)];

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

        private static bool Walk(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1)
            {
                throw new RunTimeError("Usage: walk ('direction')");
            }

            if (ScriptManager.LastWalk + TimeSpan.FromSeconds(0.4) >= DateTime.UtcNow)
            {
                return false;
            }

            ScriptManager.LastWalk = DateTime.UtcNow;

            Direction dir = (Direction) Enum.Parse(typeof(Direction), vars[0].AsString(), true);
            Client.Instance.RequestMove(dir);

            return true;
        }

        private static bool UseSkill(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length == 0)
            {
                throw new RunTimeError("Usage: skill ('skill name'/'last')");
            }

            int skillId = 0;

            if (World.Player.LastSkill != -1)
            {
                skillId = World.Player.LastSkill;
            }

            if (vars[0].AsString() == "last")
            {
                Client.Instance.SendToServer(new UseSkill(World.Player.LastSkill));
            }
            else if (Skills.SkillsByName.TryGetValue(vars[0].AsString(), out SkillInfo skill))
            {
                if (skill.IsAction)
                {
                    Client.Instance.SendToServer(new UseSkill(skill.Index));

                    World.Player.LastSkill = skill.Index;
                }
                else
                {
                    CommandHelper.SendWarning(command, $"Skill '{vars[0].AsString()}' is not usable. Available usable skills: {string.Join(", ", Skills.GetUsableSkillNames())}", quiet);
                }
            }
            else
            {
                CommandHelper.SendWarning(command, $"Skill '{vars[0].AsString()}' not found. Available usable skills: {string.Join(", ", Skills.GetUsableSkillNames())}", quiet);
            }

            if (skillId == Skills.StealthIndex && !World.Player.Visible)
            {
                StealthSteps.Hide();
            }

            return true;
        }
        
        private static bool Pause(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length == 0)
                throw new RunTimeError("Usage: wait (timeout) [shorthand]");

            uint timeout = vars[0].AsUInt();

            if (vars.Length == 2)
            {
                switch (vars[1].AsString())
                {
                    case "s":
                    case "sec":
                    case "secs":
                    case "second":
                    case "seconds":
                        timeout *= 1000;
                        break;
                    case "m":
                    case "min":
                    case "mins":
                    case "minute":
                    case "minutes":
                        timeout *= 60000;
                        break;
                }
            }

            Interpreter.Pause(timeout);

            return true;
        }

        private static bool Attack(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length == 0)
            {
                throw new RunTimeError("Usage: attack (serial)");
            }

            Serial serial = vars[0].AsSerial();

            if (!serial.IsValid)
            {
                throw new RunTimeError($"{command} - Invalid serial");
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

        private static bool Cast(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1)
            {
                throw new RunTimeError("Usage: cast 'name of spell'");
            }

            Spell spell = int.TryParse(vars[0].AsString(), out int spellnum)
                ? Spell.Get(spellnum)
                : Spell.GetByName(vars[0].AsString());

            if (spell != null)
            {
                spell.OnCast(new CastSpellFromMacro((ushort) spell.GetID()));
            }
            else
            {
                throw new RunTimeError($"{command} - Spell name or number not valid");
            }

            return true;
        }

        private static bool OverheadMessage(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length == 0)
            {
                throw new RunTimeError("Usage: overhead ('text') [color] [serial]");
            }
            
            string overheadMessage = vars[0].AsString();
            overheadMessage = CommandHelper.ReplaceStringInterpolations(overheadMessage);

            if (vars.Length == 1)
            {
                World.Player.OverheadMessage(Config.GetInt("SysColor"), overheadMessage);
            }
            else if (vars.Length == 2)
            {
                int hue = Utility.ToInt32(vars[1].AsString(), 0);

                if (vars.Length == 3)
                {
                    uint serial = vars[2].AsSerial();

                    Mobile m = World.FindMobile(serial);
                    m?.OverheadMessage(hue, overheadMessage);
                }
                else
                {
                    World.Player.OverheadMessage(hue, overheadMessage);
                }
            }

            return true;
        }

        private static bool SystemMessage(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length == 0)
            {
                throw new RunTimeError("Usage: sysmsg ('text') [color]");
            }
            
            var sysMessage = vars[0].AsString();
            sysMessage = CommandHelper.ReplaceStringInterpolations(sysMessage);

            if (vars.Length == 1)
            {
                World.Player.SendMessage(Config.GetInt("SysColor"), sysMessage);
            }
            else if (vars.Length == 2)
            {
                World.Player.SendMessage(Utility.ToInt32(vars[1].AsString(), 0), sysMessage);
            }

            return true;
        }

        private static bool ClearSysMsg(string command, Variable[] vars, bool quiet, bool force)
        {
            SystemMessages.Messages.Clear();

            return true;
        }

        private static DressList _lastDressList;

        private static bool DressCommand(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length == 0)
            {
                throw new RunTimeError("Usage: dress ('name of dress list')");
            }

            if (_lastDressList == null)
            {
                _lastDressList = DressList.Find(vars[0].AsString());
                
                if (_lastDressList != null)
                {
                    _lastDressList.Dress();
                }
                else
                {
                    Serial serial = vars[0].AsSerial();
                    Item item = World.FindItem(serial);

                    if (item != null)
                    {
                        DressList dressList = new DressList("temp");
                        dressList.Items.Add(serial);
                        dressList.Dress();

                        _lastDressList = dressList;
                    }
                    else
                    {
                        CommandHelper.SendWarning(command, $"'{vars[0].AsString()}' not found", quiet);
                        return true;
                    }
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
        private static bool _undressLayer;

        private static bool UnDressCommand(string command, Variable[] vars, bool quiet, bool force)
        {

            if (vars.Length == 0 && !_undressAll) // full naked!
            {
                _undressAll = true;
                UndressHotKeys.OnUndressAll();
            }
            else if (vars.Length == 1 && _lastUndressList == null && !_undressLayer) // either a dress list item or a layer
            {
                _lastUndressList = DressList.Find(vars[0].AsString());

                if (_lastUndressList != null)
                {
                    _lastUndressList.Undress();
                }
                else // lets find the layer
                {
                    if (Enum.TryParse(vars[0].AsString(), true, out Layer layer))
                    {
                        Dress.Unequip(layer);
                        _undressLayer = true;
                    }
                    else
                    {
                        Serial serial = vars[0].AsSerial();
                        Item item = World.FindItem(serial);

                        if (item != null)
                        {
                            DressList undressList = new DressList("temp");
                            undressList.Items.Add(serial);
                            undressList.Undress();

                            _lastUndressList = undressList;
                        }
                        else
                        {
                            CommandHelper.SendWarning(command, $"'{vars[0].AsString()}' not found", quiet);
                            return true;
                        }
                    }
                }
            }
            else if (ActionQueue.Empty)
            {
                _undressAll = false;
                _undressLayer = false;
                _lastUndressList = null;
                return true;
            }

            return false;
        }

        private static bool GumpResponse(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1)
            {
                throw new RunTimeError("Usage: gumpresponse (buttondId)");
                //throw new RunTimeError("Usage: gumpresponse (buttondId) [option] ['text1'|fieldId] ['text2'|fieldId]");
            }

            int buttonId = vars[0].AsInt();

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

        private static bool GumpClose(string command, Variable[] vars, bool quiet, bool force)
        {
            uint gumpI = World.Player.CurrentGumpI;

            if (vars.Length > 0)
            {
                gumpI = vars[0].AsUInt();
            }

            if (!World.Player.GumpList.ContainsKey(gumpI))
            {
                CommandHelper.SendWarning(command, $"'{gumpI}' unknown gump id", quiet);
                return true;
            }

            uint gumpS = World.Player.GumpList[gumpI].GumpSerial;

            Client.Instance.SendToClient(new CloseGump(gumpI));
            Client.Instance.SendToServer(new GumpResponse(gumpS, gumpI, 0, new int[] { }, new GumpTextEntry[] { }));

            World.Player.HasGump = false;
            World.Player.HasCompressedGump = false;

            return true;
        }

        private static bool ContextMenu(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 2)
            {
                throw new RunTimeError("Usage: menu (serial) (index)");
            }

            Serial s = vars[0].AsSerial();
            ushort index = vars[1].AsUShort();
            bool blockPopup = true;

            if (vars.Length > 2)
            {
                blockPopup = vars[2].AsBool();
            }

            if (s == Serial.Zero && World.Player != null)
                s = World.Player.Serial;
            
            ScriptManager.BlockPopupMenu = blockPopup;

            Client.Instance.SendToServer(new ContextMenuRequest(s));
            Client.Instance.SendToServer(new ContextMenuResponse(s, index));
            return true;
        }

        private static bool MenuResponse(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 2)
            {
                throw new RunTimeError("Usage: menuresponse (index) (menuId) [hue]");
            }

            ushort index = vars[0].AsUShort();
            ushort menuId = vars[1].AsUShort();
            ushort hue = 0;

            if (vars.Length == 3)
                hue = vars[2].AsUShort();

            Client.Instance.SendToServer(new MenuResponse(World.Player.CurrentMenuS, World.Player.CurrentMenuI, index,
                menuId, hue));
            World.Player.HasMenu = false;
            return true;
        }

        private static bool PromptResponse(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1)
            {
                throw new RunTimeError("Usage: promptresponse ('response to the prompt')");
            }

            World.Player.ResponsePrompt(vars[0].AsString());
            return true;
        }

        private static bool LastTarget(string command, Variable[] vars, bool quiet, bool force)
        {
            if (!Targeting.DoLastTarget())
                Targeting.ResendTarget();

            return true;
        }

        private static bool PlayScript(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1)
            {
                throw new RunTimeError("Usage: script 'name of script'");
            }

            ScriptManager.PlayScript(vars[0].AsString());

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

        private static bool Potion(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length == 0)
            {
                throw new RunTimeError("Usage: potion ('type')");
            }

            Item pack = World.Player.Backpack;
            if (pack == null)
                return true;

            if (PotionList.TryGetValue(vars[0].AsString().ToLower(), out ushort potionId))
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
                throw new RunTimeError($"{command} - Unknown potion type");
            }

            return true;
        }

        private static bool WaitForSysMsg(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1)
            {
                throw new RunTimeError("Usage: waitforsysmsg 'message to wait for' [timeout]");
            }
            
            if (SystemMessages.Exists(vars[0].AsString()))
            {
                Interpreter.ClearTimeout();
                return true;
            }

            Interpreter.Timeout(vars.Length > 1 ? vars[1].AsUInt() : 30000, () => { return true; });

            return false;
        }

        private static bool Random(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1)
            {
                throw new RunTimeError("Usage: random 'max value'");
            }

            int max = vars[0].AsInt();

            World.Player.SendMessage(MsgLevel.Info, $"Random: {Utility.Random(1, max)}");

            return true;
        }

        private static bool ClearDragDrop(string command, Variable[] vars, bool quiet, bool force)
        {
            DragDropManager.GracefulStop();

            return true;
        }
        
        private static bool Interrupt(string command, Variable[] vars, bool quiet, bool force)
        {
            Spell.Interrupt();

            return true;
        }
    }
}