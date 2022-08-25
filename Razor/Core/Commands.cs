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
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assistant.Agents;
using Assistant.Core;
using Assistant.Gumps.Internal;
using Assistant.Macros;
using Assistant.Scripts;
using Ultima;

namespace Assistant
{
    public class Commands
    {
        public static void Initialize()
        {
            Command.Register("AddUseOnce", AddUseOnce);
            Command.Register("Time", Time);
            Command.Register("Where", Where);
            Command.Register("Ping", Ping);
            Command.Register("Help", Command.ListCommands);
            Command.Register("Echo", Echo);
            Command.Register("Macro", MacroCmd);
            Command.Register("Script", ScriptCmd);
            Command.Register("Hue", GetItemHue);
            Command.Register("Item", GetItemHue);
            Command.Register("Resync", Resync);
            Command.Register("Mobile", GetMobile);
            Command.Register("Weather", SetWeather);
            Command.Register("Season", SetSeason);
            Command.Register("Damage", DamageTrackerReport);
            Command.Register("Set", SetMacroVariable);
            Command.Register("Track", Track);
            Command.Register("Waypoint", Track);

            Command.Register("TestGump", CreateGump);
            Command.Register("Info", GetGumpInfo);

            Command.Register("SysMsgs", GetSystemMessages);
            Command.Register("SysMessages", GetSystemMessages);

            Command.Register("HotKeys", ShowHotKeyGump);

            Command.Register("Boat", ShowBoatControlGump);

            Command.Register("Skills", ShowUsableSkills);

            Command.Register("Restock", ShowRestockGump);
            Command.Register("Org", ShowOrgGump);
            Command.Register("Buy", ShowBuyGump);

            Command.Register("CUO", ClassicUoProfile);
            
            Command.Register("Sound", PlaySound);
        }
        
        private static void PlaySound(string[] param)
        {
            if (param.Length <= 0)
            {
                World.Player.SendMessage(MsgLevel.Error, "You must enter sound id.");
                return;
            }

            Client.Instance.SendToClient(new PlaySound(Utility.ToInt32(param[0], 0)));
        }

        private static void ClassicUoProfile(string[] param)
        {
            if (string.IsNullOrEmpty(param[0]))
            {
                World.Player.SendMessage(MsgLevel.Error, "You must include a valid ClassicUO profile setting. Type `>cuo list` for a list of valid settings.");
                return;
            }

            if (param[0].Equals("list"))
            {
                string[] props = ClassicUOManager.GetAllProperties().ToArray();

                for (var i = 0; i < (float)props.Length / 40; i++)
                {
                    World.Player.SendMessage(MsgLevel.Info, string.Join(", ", props.Skip(i * 40).Take(40)));
                }

                return;
            }

            string property = ClassicUOManager.IsValidProperty(param[0]);

            if (string.IsNullOrEmpty(property))
            {
                World.Player.SendMessage("Unknown ClassicUO setting/property. Type `>cuo list` for a list of valid settings.");
                return;
            }

            bool isNumeric = int.TryParse(param[1], out var value);

            if (isNumeric)
            {
                ClassicUOManager.ProfilePropertySet(property, value);
            }
            else
            {
                switch (param[1])
                {
                    case "true":
                        ClassicUOManager.ProfilePropertySet(property, true);
                        break;
                    case "false":
                        ClassicUOManager.ProfilePropertySet(property, false);
                        break;
                    default:
                        ClassicUOManager.ProfilePropertySet(property, param[1]);
                        break;
                }
            }

            World.Player.SendMessage($"ClassicUO Setting: '{property}' set to '{param[1]}'");
        }

        private static void ShowBuyGump(string[] param)
        {
            if (string.IsNullOrEmpty(param[0]))
            {
                World.Player.SendMessage(MsgLevel.Error, "You must include an id or alias");
                return;
            }

            if (int.TryParse(param[0], out int agentIndex))
            {
                BuyAgent agent = BuyAgent.Agents[agentIndex - 1];

                AgentsGump gump = new AgentsGump(agent);
                gump.SendGump();
            }
            else
            {
                foreach (BuyAgent agent in BuyAgent.Agents)
                {
                    if (agent.Alias.Equals(param[0], StringComparison.OrdinalIgnoreCase))
                    {
                        AgentsGump gump = new AgentsGump(agent);
                        gump.SendGump();

                        break;
                    }
                }
            }
        }

        private static void ShowOrgGump(string[] param)
        {
            if (string.IsNullOrEmpty(param[0]))
            {
                World.Player.SendMessage(MsgLevel.Error, "You must include an id or alias");
                return;
            }

            if (int.TryParse(param[0], out int agentIndex))
            {
                OrganizerAgent agent = OrganizerAgent.Agents[agentIndex - 1];

                AgentsGump gump = new AgentsGump(agent);
                gump.SendGump();
            }
            else
            {
                foreach (OrganizerAgent agent in OrganizerAgent.Agents)
                {
                    if (agent.Alias.Equals(param[0], StringComparison.OrdinalIgnoreCase))
                    {
                        AgentsGump gump = new AgentsGump(agent);
                        gump.SendGump();

                        break;
                    }
                }
            }
        }

        private static void ShowRestockGump(string[] param)
        {
            if (string.IsNullOrEmpty(param[0]))
            {
                World.Player.SendMessage(MsgLevel.Error, "You must include an id or alias");
                return;
            }

            if (int.TryParse(param[0], out int agentIndex))
            {
                RestockAgent agent = RestockAgent.Agents[agentIndex - 1];

                AgentsGump gump = new AgentsGump(agent);
                gump.SendGump();
            }
            else
            {
                foreach (RestockAgent agent in RestockAgent.Agents)
                {
                    if (agent.Alias.Equals(param[0], StringComparison.OrdinalIgnoreCase))
                    {
                        AgentsGump gump = new AgentsGump(agent);
                        gump.SendGump();

                        break;
                    }
                }
            }
        }

        private static void ShowUsableSkills(string[] param)
        {
            World.Player.SendMessage(MsgLevel.Info, $"Usable skills: {string.Join(", ", Skills.GetUsableSkillNames())}");
        }

        private static void GetSystemMessages(string[] param)
        {
            SystemMessagesGump gump = new SystemMessagesGump();
            gump.SendGump();            
        }

        private static void ShowHotKeyGump(string[] param)
        {
            HotKeyGump gump = new HotKeyGump(true, true, true);
            gump.SendGump();
        }

        private static void ShowBoatControlGump(string[] param)
        {
            BoatControlGump gump = new BoatControlGump(0);
            gump.SendGump();
        }

        private static void CreateGump(string[] param)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("Razor");
            message.AppendLine();
            message.AppendLine("Another message");

            TestMessageGump gump =
                new TestMessageGump(message.ToString());
            gump.SendGump();
        }

        private static DateTime m_LastSync;
        private static string _lastMacroVariable;

        private static void Resync(string[] param)
        {
            if (DateTime.UtcNow - m_LastSync > TimeSpan.FromSeconds(1.0))
            {
                m_LastSync = DateTime.UtcNow;

                Client.Instance.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x3B2, 3,
                    Language.CliLocName, "System", "Updating and resyncing with server"));

                Client.Instance.SendToServer(new ResyncReq());
            }
        }

        private static void DamageTrackerReport(string[] param)
        {
            if (DamageTracker.Running)
                DamageTracker.SendReport();
        }

        private static void Track(string[] param)
        {
            try
            {
                if (param[0].Equals("off"))
                {
                    WaypointManager.ClearWaypoint();
                    return;
                }

                WaypointManager.Waypoint waypoint = new WaypointManager.Waypoint
                {
                    Name = "N/A",
                    X = Convert.ToUInt16(param[0]),
                    Y = Convert.ToUInt16(param[1])
                };

                WaypointManager.ShowWaypoint(waypoint);
            }
            catch
            {
                WaypointManager.ClearWaypoint();
            }
        }

        private static void SetMacroVariable(string[] param)
        {
            if (MacroManager.Playing || MacroManager.Recording || World.Player == null)
                return;

            if (string.IsNullOrEmpty(param[0]))
            {
                World.Player.SendMessage(MsgLevel.Error, "You must pass in a macro variable name.");
                return;
            }

            _lastMacroVariable = param[0];

            Targeting.OneTimeTarget(OnMacroVariableAddTarget);
            World.Player.SendMessage(MsgLevel.Force, $"Select new target for '{_lastMacroVariable}'");
        }

        private static void OnMacroVariableAddTarget(bool ground, Serial serial, Point3D pt, ushort gfx)
        {
            TargetInfo t = new TargetInfo
            {
                Gfx = gfx,
                Serial = serial,
                Type = (byte) (ground ? 1 : 0),
                X = pt.X,
                Y = pt.Y,
                Z = pt.Z
            };

            bool foundVar = false;

            foreach (MacroVariables.MacroVariable mV in MacroVariables.MacroVariableList
            )
            {
                if (mV.Name.ToLower().Equals(_lastMacroVariable.ToLower()))
                {
                    foundVar = true;
                    // macro exists, update
                    mV.TargetInfo = t;

                    World.Player.SendMessage(MsgLevel.Force, $"'{mV.Name}' macro variable updated to '{t.Serial}'");

                    break;
                }
            }

            if (!foundVar)
            {
                MacroVariables.MacroVariableList.Add(new MacroVariables.MacroVariable(_lastMacroVariable, t));
                World.Player.SendMessage(MsgLevel.Force,
                    $"'{_lastMacroVariable}' not found, created variable and set to '{t.Serial}'");
            }

            // Save and reload the macros and vars
            Engine.MainWindow.SaveMacroVariables();
        }

        private static void SetWeather(string[] param)
        {
            Client.Instance.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x3B2, 3,
                Language.CliLocName, "System", "Setting weather.."));

            Client.Instance.SendToClient(new SetWeather(Convert.ToInt32(param[0]), Convert.ToInt32(param[1])));
        }

        private static void SetSeason(string[] param)
        {
            Client.Instance.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x3B2, 3,
                Language.CliLocName, "System", "Setting season.."));

            Client.Instance.ForceSendToClient(new SeasonChange(Convert.ToInt32(param[0]), true));
        }

        private static void GetGumpInfo(string[] param)
        {
            ScriptManager.GetGumpInfo(param);
        }

        private static void GetItemHue(string[] param)
        {
            Targeting.OneTimeTarget(OnGetItemHueTarget);
            Client.Instance.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x3B2, 3,
                Language.CliLocName, "System", "Select an item to get the hue value"));
        }

        private static void OnGetItemHueTarget(bool ground, Serial serial, Point3D pt, ushort gfx)
        {
            Item item = World.FindItem(serial);
            if (item != null)
            {
                Client.Instance.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x3B2, 3,
                    Language.CliLocName, "System",
                    $"Item Name: '{item.ItemID.ItemData.Name}' Serial: '{item.Serial}' Id: '{item.ItemID.Value}' Hue: '{item.Hue}'"));
            }
        }

        private static void GetMobile(string[] param)
        {
            ScriptManager.GetGumpInfo(param);
        }

        private static void Echo(string[] param)
        {
            StringBuilder sb = new StringBuilder("Note To Self: ");
            for (int i = 0; i < param.Length; i++)
                sb.Append(param[i]);
            Client.Instance.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x3B2, 3,
                Language.CliLocName, "System", sb.ToString()));
        }

        private static void AddUseOnce(string[] param)
        {
            string use = Language.GetString(LocString.UseOnce);
            for (int i = 0; i < Agent.List.Count; i++)
            {
                Agent a = Agent.List[i];
                if (a.Name == use)
                {
                    a.OnButtonPress(1);
                    break;
                }
            }
        }

        private static void Time(string[] param)
        {
            World.Player.SendMessage(MsgLevel.Force, LocString.CurTime,
                Engine.MistedDateTime.ToString("MM/dd/yy HH:mm:ss.f"));
        }

        private static void Where(string[] param)
        {
            string mapStr;
            switch (World.Player.Map)
            {
                case 0:
                    mapStr = "Felucca";
                    break;
                case 1:
                    mapStr = "Trammel";
                    break;
                case 2:
                    mapStr = "Ilshenar";
                    break;
                case 3:
                    mapStr = "Malas";
                    break;
                case 4:
                    mapStr = "Tokuno";
                    break;
                case 0x7F:
                    mapStr = "Internal";
                    break;
                default:
                    mapStr = $"Unknown (#{World.Player.Map})";
                    break;
            }

            World.Player.SendMessage(MsgLevel.Force, LocString.CurLoc, World.Player.Position, mapStr);
        }

        private static void Ping(string[] param)
        {
            int count = 5;
            if (param.Length > 0)
                count = Utility.ToInt32(param[0], 5);

            if (count > 10)
                count = 10;

            Assistant.Ping.StartPing(count);
        }

        private static void MacroCmd(string[] param)
        {
            if (param.Length <= 0)
            {
                World.Player.SendMessage("You must enter a macro name.");
                return;
            }

            string macroName = string.Join(" ", param);

            foreach (Macro m in MacroManager.List)
            {
                if (m.ToString().ToLower().Equals(macroName.ToLower()))
                {
                    MacroManager.HotKeyPlay(m);
                    break;
                }
            }
        }

        private static void ScriptCmd(string[] param)
        {
            if (param.Length <= 0)
            {
                World.Player.SendMessage("You must enter a script name.");
                return;
            }

            string name = string.Join(" ", param);

            ScriptManager.PlayScript(name);
        }
    }

    public delegate void CommandCallback(string[] param);

    public class Command
    {
        private static Dictionary<string, CommandCallback> _commandList;

        public static List<string> GetCommands => _commandList.Keys.ToList();

        static Command()
        {
            _commandList = new Dictionary<string, CommandCallback>(16, StringComparer.OrdinalIgnoreCase);
            PacketHandler.RegisterClientToServerFilter(0xAD, OnSpeech);
        }

        public static void ListCommands(string[] param)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (string cmd in _commandList.Keys)
            {
                sb.Append(cmd);
                sb.Append(" ");
            }

            World.Player.SendMessage(MsgLevel.Force, LocString.CommandList);
            World.Player.SendMessage(MsgLevel.Force, sb.ToString());
        }

        public static void Register(string cmd, CommandCallback callback)
        {
            _commandList[cmd] = callback;
        }

        public static CommandCallback FindCommand(string cmd)
        {
            return _commandList[cmd] as CommandCallback;
        }

        public static void RemoveCommand(string cmd)
        {
            _commandList.Remove(cmd);
        }

        public static Dictionary<string, CommandCallback> List
        {
            get { return _commandList; }
        }

        public static void OnSpeech(Packet pvSrc, PacketHandlerEventArgs args)
        {
            MessageType type = (MessageType) pvSrc.ReadByte();
            ushort hue = pvSrc.ReadUInt16();
            ushort font = pvSrc.ReadUInt16();
            string lang = pvSrc.ReadString(4);
            string text = "";
            ArrayList keys = null;
            long txtOffset = 0;

            World.Player.SpeechHue = hue;

            if ((type & MessageType.Encoded) != 0)
            {
                int value = pvSrc.ReadInt16();
                int count = (value & 0xFFF0) >> 4;
                keys = new ArrayList();
                keys.Add((ushort) value);

                for (int i = 0; i < count; ++i)
                {
                    if ((i & 1) == 0)
                    {
                        keys.Add(pvSrc.ReadByte());
                    }
                    else
                    {
                        keys.Add(pvSrc.ReadByte());
                        keys.Add(pvSrc.ReadByte());
                    }
                }

                txtOffset = pvSrc.Position;
                text = pvSrc.ReadUTF8StringSafe();
                type &= ~MessageType.Encoded;
            }
            else
            {
                txtOffset = pvSrc.Position;
                text = pvSrc.ReadUnicodeStringSafe();
            }

            text = text.Trim();

            char commandToggle = Client.IsOSI ? '-' : '>';

            if (text.Length > 0)
            {
                if (text[0] != commandToggle)
                {
                    Macros.MacroManager.Action(new Macros.SpeechAction(type, hue, font, lang, keys, text));
                    ScriptManager.AddToScript($"say \'{text}\'");
                }
                else
                {
                    text = text.Substring(1);
                    string[] split = text.Split(' ', '\t');

                    if (_commandList.ContainsKey(split[0]))
                    {
                        CommandCallback call = (CommandCallback) _commandList[split[0]];

                        if (call != null)
                        {
                            string[] param = new string[split.Length - 1];
                            for (int i = 0; i < param.Length; i++)
                                param[i] = split[i + 1];
                            call(param);

                            args.Block = true;
                        }
                    }
                    else if (HotKey.ExecuteCommand(split[0]))
                    {
                        args.Block = true;
                    }
                    else
                    {
                        World.Player.SendMessage(MsgLevel.Force, "Unknown command");
                    }
                }
            }
        }
    }
}