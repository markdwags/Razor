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

using System.Windows.Forms.VisualStyles;
using Assistant.Agents;

namespace Assistant.Gumps.Internal
{
    public sealed class AgentsGump : Gump
    {
        private Agent Agent { get; set; }

        public AgentsGump(Agent agent) : base(500, 250, 5000)
        {
            Closable = true;
            Disposable = true;
            Movable = true;
            Resizable = false;
            Resend = false;

            int pageCount = 1;
            Agent = agent;

            AddPage(pageCount);

            AddPage(0);

            if (agent is RestockAgent restockAgent)
            {
                AddBackground(1, 0, 453, 397, 3600);

                AddButton(21, 355, 4029, 4031, (int)Buttons.btnAddTarget, GumpButtonType.Reply, 0);
                AddLabel(58, 357, 990, Language.GetString(LocString.AddTarg));
                AddLabel(300, 25, 990, "Set Amount  Remove");
                
                AddButton(152, 355, 4014, 4016, (int)Buttons.btnAgentAction, GumpButtonType.Reply, 0);
                AddLabel(191, 357, 990, "Restock Now");

                string title = !string.IsNullOrEmpty(agent.Alias)
                    ? $"Restock Agent {agent.Number} ({agent.Alias})"
                    : $"Restock Agent {agent.Number}";

                AddLabel(27, 23, 252, title);

                int count = 0;
                int labelY = 66;

                if (restockAgent.Items.Count > 0)
                {
                    foreach (RestockAgent.RestockItem item in restockAgent.Items)
                    {
                        //AddItem(5, labelY, item.ItemID.Value);
                        AddImage(25, labelY + 4, 2224);
                        AddLabel(50, labelY, 342, $"{item.ItemID}");
                        AddLabel(250, labelY, 372, $"{item.Amount:N0}");

                        AddButton(390, labelY - 2, 4017, 4019, GetButtonId(0, item.ItemID.Value), GumpButtonType.Reply, 0); //remove
                        AddButton(324, labelY - 2, 4008, 4010, GetButtonId(1, item.ItemID.Value), GumpButtonType.Reply, 0); //set amount

                        count++;
                        labelY += 20;

                        if (count == 14)
                        {
                            AddButton(410, 356, 9904, 9905, pageCount + 2, GumpButtonType.Page, pageCount + 1);

                            pageCount++;
                            count = 0;
                            labelY = 66;

                            AddPage(pageCount);

                            AddBackground(1, 0, 453, 397, 3600);

                            AddLabel(27, 23, 252, title);

                            AddButton(21, 355, 4029, 4031, (int)Buttons.btnAddTarget, GumpButtonType.Reply, 0);
                            AddLabel(58, 357, 990, Language.GetString(LocString.AddTarg));
                            AddLabel(300, 25, 990, "Set Amount  Remove");

                            AddButton(152, 355, 4014, 4016, (int)Buttons.btnAgentAction, GumpButtonType.Reply, 0);
                            AddLabel(191, 357, 990, "Restock Now");

                            AddButton(388, 356, 9910, 9911, pageCount + 1, GumpButtonType.Page, pageCount - 1);

                            AddLabel(312, 357, 900, $"Page {pageCount}");
                        }
                    }
                }
                else
                {
                    AddImage(25, labelY + 4, 2224);
                    AddLabel(50, labelY, 342, "No items on this agent");
                }
            }
            else if (agent is OrganizerAgent orgAgent)
            {
                AddBackground(1, 0, 453, 397, 3600);

                AddButton(21, 355, 4029, 4031, (int)Buttons.btnAddTarget, GumpButtonType.Reply, 0);
                AddLabel(58, 357, 990, Language.GetString(LocString.AddTarg));
                AddLabel(420, 25, 990, "Remove");

                AddButton(152, 355, 4014, 4016, (int)Buttons.btnAgentAction, GumpButtonType.Reply, 0);
                AddLabel(191, 357, 990, "Organize Now");

                string title = !string.IsNullOrEmpty(agent.Alias)
                    ? $"Organizer Agent {agent.Number} ({agent.Alias})"
                    : $"Organizer Agent {agent.Number}";

                AddLabel(27, 23, 252, title);

                int count = 0;
                int labelY = 66;

                if (orgAgent.Items.Count > 0)
                {
                    foreach (ItemID item in orgAgent.Items)
                    {
                        //AddItem(5, labelY, item.Value);
                        AddImage(25, labelY + 4, 2224);
                        AddLabel(50, labelY, 342, $"{item}");

                        AddButton(390, labelY - 2, 4017, 4019, GetButtonId(0, item.Value), GumpButtonType.Reply, 0); //remove

                        count++;
                        labelY += 20;

                        if (count == 14)
                        {
                            AddButton(410, 356, 9904, 9905, pageCount + 2, GumpButtonType.Page, pageCount + 1);

                            pageCount++;
                            count = 0;
                            labelY = 66;

                            AddPage(pageCount);

                            AddBackground(1, 0, 453, 397, 3600);

                            AddLabel(27, 23, 252, title);

                            AddButton(21, 355, 4029, 4031, (int)Buttons.btnAddTarget, GumpButtonType.Reply, 0);
                            AddLabel(58, 357, 990, Language.GetString(LocString.AddTarg));
                            AddLabel(420, 25, 990, "Remove");

                            AddButton(152, 355, 4014, 4016, (int)Buttons.btnAgentAction, GumpButtonType.Reply, 0);
                            AddLabel(191, 357, 990, "Organize Now");

                            AddButton(388, 356, 9910, 9911, pageCount + 1, GumpButtonType.Page, pageCount - 1);

                            AddLabel(312, 357, 900, $"Page {pageCount}");
                        }
                    }
                }
                else
                {
                    AddImage(25, labelY + 4, 2224);
                    AddLabel(50, labelY, 342, "No items on this agent");
                }
            }
            else if (agent is BuyAgent buyAgent)
            {
                AddBackground(1, 0, 453, 397, 3600);

                AddButton(21, 355, 4029, 4031, (int)Buttons.btnAddTarget, GumpButtonType.Reply, 0);
                AddLabel(58, 357, 990, Language.GetString(LocString.AddTarg));
                AddLabel(300, 25, 990, "Set Amount  Remove");

                string title = !string.IsNullOrEmpty(agent.Alias)
                    ? $"Buy Agent {agent.Number} ({agent.Alias})"
                    : $"Buy Agent {agent.Number}";

                AddLabel(27, 23, 252, title);

                int count = 0;
                int labelY = 66;

                if (buyAgent.Items.Count > 0)
                {
                    foreach (BuyAgent.BuyEntry item in buyAgent.Items)
                    {
                        //AddItem(5, labelY, item.Id);
                        AddImage(25, labelY + 4, 2224);
                        AddLabel(50, labelY, 342, $"{item.Id}");
                        AddLabel(250, labelY, 372, $"{item.Amount}");

                        AddButton(390, labelY - 2, 4017, 4019, GetButtonId(0, item.Id), GumpButtonType.Reply, 0); //remove
                        AddButton(324, labelY - 2, 4008, 4010, GetButtonId(1, item.Id), GumpButtonType.Reply, 0); //set amount

                        count++;
                        labelY += 20;

                        if (count == 14)
                        {
                            AddButton(410, 356, 9904, 9905, pageCount + 2, GumpButtonType.Page, pageCount + 1);

                            pageCount++;
                            count = 0;
                            labelY = 66;

                            AddPage(pageCount);

                            AddBackground(1, 0, 453, 397, 3600);

                            AddLabel(27, 23, 252, title);

                            AddButton(21, 355, 4029, 4031, (int)Buttons.btnAddTarget, GumpButtonType.Reply, 0);
                            AddLabel(58, 357, 990, Language.GetString(LocString.AddTarg));
                            AddLabel(300, 25, 990, "Set Amount  Remove");

                            AddButton(388, 356, 9910, 9911, pageCount + 1, GumpButtonType.Page, pageCount - 1);

                            AddLabel(312, 357, 900, $"Page {pageCount}");
                        }
                    }
                }
                else
                {
                    AddImage(25, labelY + 4, 2224);
                    AddLabel(50, labelY, 342, "No items on this agent");
                }
            }
        }

        public enum Buttons
        {
            btnAddTarget = 1,
            btnRemove,
            btnSetAmount,
            btnNextPage,
            btnPrevPage,
            btnAgentAction
        }

        public override void OnResponse(int buttonId, int[] switches, GumpTextEntry[] textEntries = null)
        {
            if (buttonId > 0)
            {
                var type = buttonId % 11;
                var index = buttonId / 11;

                if (Agent is RestockAgent restockAgent)
                {
                    switch (buttonId)
                    {
                        case (int)Buttons.btnAddTarget:
                            restockAgent.AddItemTarget(true);
                            break;
                        case (int) Buttons.btnAgentAction:
                            restockAgent.OnHotKey();
                            break;
                        default:
                        {
                            switch (type)
                            {
                                // remove
                                case 1:
                                    restockAgent.RemoveItem(index);
                                    break;
                                // set amount
                                case 2:
                                    restockAgent.SetItemAmount(index);
                                    break;
                            }

                            break;
                        }
                    }

                    AgentsGump gump = new AgentsGump(Agent);
                    gump.SendGump();
                }
                else if (Agent is OrganizerAgent orgAgent)
                {
                    switch (buttonId)
                    {
                        case (int)Buttons.btnAddTarget:
                            orgAgent.AddItemTarget(true);
                            break;
                        case (int)Buttons.btnAgentAction:
                            orgAgent.Organize();
                            break;
                        default:
                        {
                            if (type == 1) // remove
                            {
                                orgAgent.RemoveItem(index);
                            }

                            break;
                        }
                    }

                    AgentsGump gump = new AgentsGump(Agent);
                    gump.SendGump();
                }
                else if (Agent is BuyAgent buyAgent)
                {
                    switch (buttonId)
                    {
                        case (int)Buttons.btnAddTarget:
                            buyAgent.AddItemTarget(true);
                            break;
                        default:
                            switch (type)
                            {
                                // remove
                                case 1:
                                    buyAgent.RemoveItem(index);
                                    break;
                                // set amount
                                case 2:
                                    buyAgent.SetItemAmount(index, true);
                                    break;
                            }

                            break;
                    }

                    AgentsGump gump = new AgentsGump(Agent);
                    gump.SendGump();
                }
            }

            base.OnResponse(buttonId, switches, textEntries);
        }
    }
}