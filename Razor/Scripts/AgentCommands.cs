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
using Assistant.Agents;
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
            Interpreter.RegisterCommandHandler("organize", OrganizerAgentCommand);
            Interpreter.RegisterCommandHandler("org", OrganizerAgentCommand);
            Interpreter.RegisterCommandHandler("restock", RestockAgentCommand);
            Interpreter.RegisterCommandHandler("scav", ScavAgentCommand);
            Interpreter.RegisterCommandHandler("scavenger", ScavAgentCommand);
            Interpreter.RegisterCommandHandler("sell", SellAgentCommand);
        }

        private static bool RestockAgentCommand(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1)
            {
                throw new RunTimeError( "Usage: restock (number) ['set']");
            }
            
            bool setBag = false;

            if (vars.Length == 2)
            {
                if (vars[1].AsString().IndexOf("set", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    setBag = true;
                }
            }

            if (!int.TryParse(vars[0].AsString(), out var agentNum))
            {
                string alias = vars[0].AsString();

                foreach (RestockAgent agent in RestockAgent.Agents)
                {
                    if (agent.Alias.IndexOf(alias, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        agentNum = agent.Number;
                        break;
                    }
                }
            }

            if (setBag)
            {
                RestockAgent.Agents[agentNum - 1].SetHB();
            }
            else
            {
                RestockAgent.Agents[agentNum - 1].OnHotKey();
            }

            return true;
        }

        private static bool UseOnceCommand(string command, Variable[] vars, bool quiet, bool force)
        {
            bool add = false;
            bool container = false;

            if (vars.Length == 1)
            {
                if (vars[0].AsString().IndexOf("add", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    add = true;
                }
                else if (vars[0].AsString().IndexOf("addcontainer", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    container = true;
                }
            }

            if (add)
            {
                UseOnceAgent.Instance.OnAdd();
            }
            else if (container)
            {
                UseOnceAgent.Instance.OnAddContainer();
            }
            else
            {
                UseOnceAgent.Instance.OnHotKey();
            }

            return true;
        }

        private static bool OrganizerAgentCommand(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1)
            {
                throw new RunTimeError("Usage: organizer (number) ['set']");
            }

            bool setBag = false;

            if (vars.Length == 2)
            {
                if (vars[1].AsString().IndexOf("set", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    setBag = true;
                }
            }

            if (!int.TryParse(vars[0].AsString(), out var agentNum))
            {
                string alias = vars[0].AsString();

                foreach (OrganizerAgent agent in OrganizerAgent.Agents)
                {
                    if (agent.Alias.IndexOf(alias, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        agentNum = agent.Number;
                        break;
                    }
                }
            }

            if (setBag)
            {
                OrganizerAgent.Agents[agentNum - 1].SetHotBag();
            }
            else
            {
                OrganizerAgent.Agents[agentNum - 1].Organize();
            }

            return true;
        }

        private static bool ScavAgentCommand(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1)
            {
                throw new RunTimeError("Usage: scavenger ['clear'/'add'/'on'/'off'/'set']");
            }

            bool clear = false;
            bool add = false;
            bool set = false;

            bool status = false;
            bool enabled = true;

            if (vars.Length == 1)
            {
                if (vars[0].AsString().IndexOf("clear", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    clear = true;
                }
                else if (vars[0].AsString().IndexOf("add", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    add = true;
                }
                else if (vars[0].AsString().IndexOf("on", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    status = true;
                }
                else if (vars[0].AsString().IndexOf("off", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    status = true;
                    enabled = false;
                }
                else if (vars[0].AsString().IndexOf("set", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    set = true;
                }
            }

            if (clear)
            {
                ScavengerAgent.Instance.ClearCache();
            }
            else if (add)
            {
                ScavengerAgent.Instance.OnAddToHotBag();
            }
            else if (status)
            {
                if (enabled)
                {
                    ScavengerAgent.Instance.Enable();
                }
                else
                {
                    ScavengerAgent.Instance.Disable();
                }
            }
            else if (set)
            {
                ScavengerAgent.Instance.OnSetHotBag();
            }

            return true;
        }

        private static bool SellAgentCommand(string command, Variable[] vars, bool quiet, bool force)
        {
            SellAgent.Instance.SetHotBag();

            return true;
        }
    }
}