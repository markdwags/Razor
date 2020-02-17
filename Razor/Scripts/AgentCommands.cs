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
using Assistant.Agents;
using Assistant.Core;
using Assistant.Scripts.Engine;

namespace Assistant.Scripts
{
    public static class AgentCommands
    {
        public static void Register()
        {
            // "useonce", "organizer", "org", "restock", "scav", "scavenger"
            Interpreter.RegisterCommandHandler("useonce", UseOnceCommand);
            Interpreter.RegisterCommandHandler("organizer", OrganizerAgentCommand);
            Interpreter.RegisterCommandHandler("org", OrganizerAgentCommand);
            Interpreter.RegisterCommandHandler("restock", RestockAgentCommand);
            Interpreter.RegisterCommandHandler("scav", ScavAgentCommand);
            Interpreter.RegisterCommandHandler("scavenger", ScavAgentCommand);
            Interpreter.RegisterCommandHandler("sell", SellAgentCommand);
        }

        private static bool RestockAgentCommand(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                ScriptManager.Error("Usage: restock (number) ['set']");
                return true;
            }

            string agentNum = args[0].AsString();
            string agentName = $"{Language.GetString(LocString.Restock)}-{agentNum}";

            bool setBag = false;

            if (args.Length == 2)
            {
                if (args[1].AsString().IndexOf("set", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    setBag = true;
                }
            }

            foreach (Agent agent in Agent.List)
            {
                if (agent.Name.Equals(agentName))
                {
                    if (setBag)
                    {
                        ((RestockAgent) agent).SetHB();
                    }
                    else
                    {
                        ((RestockAgent)agent).OnHotKey();
                    }
                    break;
                }
            }

            return true;
        }

        private static bool UseOnceCommand(string command, Argument[] args, bool quiet, bool force)
        {
            string agentName = Language.GetString(LocString.UseOnce);

            bool add = false;
            bool container = false;

            if (args.Length == 1)
            {
                if (args[0].AsString().IndexOf("add", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    add = true;
                } 
                else if (args[0].AsString().IndexOf("addcontainer", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    container = true;
                }
            }

            foreach (Agent agent in Agent.List)
            {
                if (agent.Name.Equals(agentName))
                {
                    if (add)
                    {
                        ((UseOnceAgent)agent).OnAdd();
                    }
                    else if (container)
                    {
                        ((UseOnceAgent)agent).OnAddContainer();
                    }
                    else
                    {
                        ((UseOnceAgent)agent).OnHotKey();
                    }

                    break;
                }
            }

            return true;
        }

        private static bool OrganizerAgentCommand(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                ScriptManager.Error("Usage: organizer (number) ['set']");
                return true;
            }

            string agentNum = args[0].AsString();
            string agentName = $"{Language.GetString(LocString.Organizer)}-{agentNum}";

            bool setBag = false;

            if (args.Length == 2)
            {
                if (args[1].AsString().IndexOf("set", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    setBag = true;
                }
            }

            foreach (Agent agent in Agent.List)
            {
                if (agent.Name.Equals(agentName))
                {
                    if (setBag)
                    {
                        ((OrganizerAgent)agent).SetHotBag();
                    }
                    else
                    {
                        ((OrganizerAgent)agent).Organize();
                    }
                    break;
                }
            }

            return true;
        }

        private static bool ScavAgentCommand(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                ScriptManager.Error("Usage: scavenger ['clear'/'add'/'on'/'off'/'set']");
                return true;
            }

            string agentName = Language.GetString(LocString.Scavenger);

            bool clear = false;
            bool add = false;
            bool set = false;

            bool status = false;
            bool enabled = true;

            if (args.Length == 1)
            {
                if (args[0].AsString().IndexOf("clear", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    clear = true;
                }
                else if (args[0].AsString().IndexOf("add", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    add = true;
                }
                else if (args[0].AsString().IndexOf("on", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    status = true;
                }
                else if (args[0].AsString().IndexOf("off", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    status = true;
                    enabled = false;
                }
                else if (args[0].AsString().IndexOf("set", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    set = true;
                }
            }

            foreach (Agent agent in Agent.List)
            {
                if (agent.Name.Equals(agentName))
                {
                    if (clear)
                    {
                        ((ScavengerAgent)agent).ClearCache();
                    }
                    else if (add)
                    {
                        ((ScavengerAgent)agent).OnAddToHotBag();
                    }
                    else if (status)
                    {
                        if (enabled)
                        {
                            ((ScavengerAgent)agent).Enable();
                        }
                        else
                        {
                            ((ScavengerAgent)agent).Disable();
                        }
                    }
                    else if (set)
                    {
                        ((ScavengerAgent)agent).OnSetHotBag();
                    }

                    break;
                }
            }

            return true;
        }

        private static bool SellAgentCommand(string command, Argument[] args, bool quiet, bool force)
        {
            string agentName = Language.GetString(LocString.Sell);
            
            foreach (Agent agent in Agent.List)
            {
                if (agent.Name.Equals(agentName))
                {
                    ((SellAgent)agent).SetHotBag();

                    break;
                }
            }

            return true;
        }
    }
}