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
using System.Windows.Forms;
using System.Xml;
using Assistant.UI;

namespace Assistant.Agents
{
    public class SearchExemptionAgent : Agent
    {
        private static SearchExemptionAgent m_Instance;

        public static int Count
        {
            get { return m_Instance.m_Items.Count; }
        }

        public static SearchExemptionAgent Instance
        {
            get { return m_Instance; }
        }

        public static void Initialize()
        {
            Agent.Add(m_Instance = new SearchExemptionAgent());
        }

        public static bool IsExempt(Item item)
        {
            if (item == null || item.IsBagOfSending)
            {
                return true;
            }

            return m_Instance == null ? false : m_Instance.CheckExempt(item);
        }

        public static bool Contains(Item item)
        {
            return m_Instance == null
                ? false
                : m_Instance.m_Items.Contains(item.Serial) || m_Instance.m_Items.Contains(item.ItemID);
        }

        private ListBox m_SubList;
        private readonly ArrayList m_Items;

        public SearchExemptionAgent()
        {
            m_Items = new ArrayList();
        }

        public override void Clear()
        {
            m_Items.Clear();
        }

        private bool CheckExempt(Item item)
        {
            if (m_Items.Count > 0)
            {
                if (m_Items.Contains(item.Serial))
                {
                    return true;
                }
                else if (m_Items.Contains(item.ItemID))
                {
                    return true;
                }
                else if (item.Container != null && item.Container is Item)
                {
                    return CheckExempt((Item) item.Container);
                }
            }

            return false;
        }

        public override string Name
        {
            get { return Language.GetString(LocString.AutoSearchEx); }
        }

        public override void OnSelected(ListBox subList, params Button[] buttons)
        {
            m_SubList = subList;

            buttons[0].Text = Language.GetString(LocString.AddTarg);
            buttons[0].Visible = true;
            buttons[1].Text = Language.GetString(LocString.AddTargType);
            buttons[1].Visible = true;
            buttons[2].Text = Language.GetString(LocString.Remove);
            buttons[2].Visible = true;
            buttons[3].Text = Language.GetString(LocString.RemoveTarg);
            buttons[3].Visible = true;
            buttons[4].Text = Language.GetString(LocString.ClearList);
            buttons[4].Visible = true;

            m_SubList.BeginUpdate();
            m_SubList.Items.Clear();

            for (int i = 0; i < m_Items.Count; i++)
            {
                Item item = null;
                if (m_Items[i] is Serial)
                {
                    item = World.FindItem((Serial) m_Items[i]);
                }

                if (item != null)
                {
                    m_SubList.Items.Add(item.ToString());
                }
                else
                {
                    m_SubList.Items.Add(m_Items[i].ToString());
                }
            }

            m_SubList.EndUpdate();
        }

        public override void OnButtonPress(int num)
        {
            switch (num)
            {
                case 1:
                    World.Player.SendMessage(MsgLevel.Force, LocString.TargItemAdd);
                    Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnTarget));
                    break;
                case 2:
                    World.Player.SendMessage(MsgLevel.Force, LocString.TargItemAdd);
                    Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnTargetType));
                    break;
                case 3:
                    if (m_SubList.SelectedIndex >= 0 && m_SubList.SelectedIndex < m_Items.Count)
                    {
                        m_Items.RemoveAt(m_SubList.SelectedIndex);
                        m_SubList.Items.RemoveAt(m_SubList.SelectedIndex);
                    }

                    break;
                case 4:
                    World.Player.SendMessage(MsgLevel.Force, LocString.TargItemRem);
                    Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnTargetRemove));
                    break;
                case 5:
                    if (MessageBox.Show(Language.GetString(LocString.Confirm), Language.GetString(LocString.ClearList),
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        m_SubList.Items.Clear();
                        m_Items.Clear();
                    }

                    break;
            }
        }

        private void OnTarget(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            Engine.MainWindow.SafeAction(s => s.ShowMe());
            if (!location && serial.IsItem)
            {
                m_Items.Add(serial);

                Item item = World.FindItem(serial);
                if (item != null)
                {
                    Client.Instance.SendToClient(new ContainerItem(item));
                    m_SubList.Items.Add(item.ToString());
                }
                else
                {
                    m_SubList.Items.Add(serial.ToString());
                }

                World.Player.SendMessage(MsgLevel.Force, LocString.ItemAdded);
            }
        }

        private void OnTargetType(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            Engine.MainWindow.SafeAction(s => s.ShowMe());

            if (!serial.IsItem)
            {
                return;
            }

            m_Items.Add((ItemID) gfx);
            m_SubList.Items.Add(((ItemID) gfx).ToString());
            World.Player.SendMessage(MsgLevel.Force, LocString.ItemAdded);
        }

        private void OnTargetRemove(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            Engine.MainWindow.SafeAction(s => s.ShowMe());
            if (!location && serial.IsItem)
            {
                for (int i = 0; i < m_Items.Count; i++)
                {
                    if (m_Items[i] is Serial && (Serial) m_Items[i] == serial)
                    {
                        m_Items.RemoveAt(i);
                        m_SubList.Items.RemoveAt(i);
                        World.Player.SendMessage(MsgLevel.Force, LocString.ItemRemoved);

                        Item item = World.FindItem(serial);
                        if (item != null)
                        {
                            Client.Instance.SendToClient(new ContainerItem(item));
                        }

                        return;
                    }
                }

                World.Player.SendMessage(MsgLevel.Force, LocString.ItemNotFound);
            }
        }

        public override void Save(XmlTextWriter xml)
        {
            for (int i = 0; i < m_Items.Count; i++)
            {
                xml.WriteStartElement("item");
                if (m_Items[i] is Serial)
                {
                    xml.WriteAttributeString("serial", ((Serial) m_Items[i]).Value.ToString());
                }
                else
                {
                    xml.WriteAttributeString("id", ((ItemID) m_Items[i]).Value.ToString());
                }

                xml.WriteEndElement();
            }
        }

        public override void Load(XmlElement node)
        {
            foreach (XmlElement el in node.GetElementsByTagName("item"))
            {
                try
                {
                    string ser = el.GetAttribute("serial");
                    string iid = el.GetAttribute("id");
                    if (ser != null)
                    {
                        m_Items.Add((Serial) Convert.ToUInt32(ser));
                    }
                    else if (iid != null)
                    {
                        m_Items.Add((ItemID) Convert.ToUInt16(iid));
                    }
                }
                catch
                {
                }
            }
        }
    }
}