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

using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using Assistant.UI;

namespace Assistant.Agents
{
    public abstract class Agent
    {
        public static List<Agent> List { get; } = new List<Agent>();

        public delegate void ItemCreatedEventHandler(Item item);

        public delegate void MobileCreatedEventHandler(Mobile m);

        public static event ItemCreatedEventHandler OnItemCreated;
        public static event MobileCreatedEventHandler OnMobileCreated;

        public static void InvokeMobileCreated(Mobile m)
        {
            if (OnMobileCreated != null)
            {
                OnMobileCreated(m);
            }
        }

        public static void InvokeItemCreated(Item i)
        {
            if (OnItemCreated != null)
            {
                OnItemCreated(i);
            }
        }

        public static void Add(Agent a)
        {
            List.Add(a);
        }

        public static void ClearAll()
        {
            for (int i = 0; i < List.Count; i++)
            {
                ((Agent) List[i]).Clear();
            }
        }

        public static void SaveProfile(XmlTextWriter xml)
        {
            foreach (Agent a in List)
            {
                xml.WriteStartElement(a.Name);
                a.Save(xml);
                xml.WriteEndElement();
            }
        }

        public static void LoadProfile(XmlElement xml)
        {
            ClearAll();

            if (xml == null)
            {
                return;
            }

            for (int i = 0; i < List.Count; i++)
            {
                try
                {
                    Agent a = (Agent) List[i];
                    XmlElement el = xml[a.Name];
                    if (el != null)
                    {
                        a.Load(el);
                    }
                }
                catch
                {
                }
            }
        }

        public static void Redraw(ComboBox list, GroupBox gb, params Button[] buttons)
        {
            list.Visible = true;
            list.BeginUpdate();
            list.Items.Clear();
            list.SelectedIndex = -1;

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Visible = false;
            }

            for (int i = 0; i < List.Count; i++)
            {
                list.Items.Add(List[i]);
            }

            list.EndUpdate();

            gb.Visible = false;
        }

        public static void Select(int idx, ComboBox agents, ListBox subList, GroupBox grp, params Button[] buttons)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Visible = false;
                buttons[i].Text = "";
                Engine.MainWindow.SafeAction(s => s.UnlockControl(buttons[i]));
            }

            grp.Visible = false;
            subList.Visible = false;
            Engine.MainWindow.SafeAction(s => s.UnlockControl(subList));

            Agent a = null;
            if (idx >= 0 && idx < List.Count)
            {
                a = List[idx] as Agent;
            }

            if (a != null)
            {
                grp.Visible = true;
                subList.Visible = true;
                grp.Text = a.ToString();
                a.OnSelected(subList, buttons);
            }
        }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(Alias) ? $"{Name} ({Alias})" : Name;
        }

        public abstract string Name { get; }
        public abstract string Alias { get; set; }
        public abstract int Number { get; }
        public abstract void Save(XmlTextWriter xml);
        public abstract void Load(XmlElement node);
        public abstract void Clear();
        public abstract void OnSelected(ListBox subList, params Button[] buttons);
        public abstract void OnButtonPress(int num);
    }
}