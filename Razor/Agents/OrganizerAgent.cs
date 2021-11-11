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
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using Assistant.UI;

namespace Assistant.Agents
{
    public class OrganizerAgent : Agent
    {
        public static List<OrganizerAgent> Agents { get; set; }

        public static void Initialize()
        {
            int maxAgents = Config.GetAppSetting<int>("MaxOrganizerAgents") == 0
                ? 20
                : Config.GetAppSetting<int>("MaxOrganizerAgents");

            Agents = new List<OrganizerAgent>();

            for (int i = 1; i <= maxAgents; i++)
            {
                OrganizerAgent organizerAgent = new OrganizerAgent(i);

                Agent.Add(organizerAgent);

                Agents.Add(organizerAgent);
            }
        }

        private ListBox m_SubList;
        private Button m_BagBTN;
        private Button m_ArrBTN;
        private readonly List<ItemID> m_Items;
        private uint m_Cont;

        public OrganizerAgent(int num)
        {
            m_Items = new List<ItemID>();
            Number = num;
            HotKey.Add(HKCategory.Agents, HKSubCat.None,
                $"{Language.GetString(LocString.OrganizerAgent)}-{Number:D2}",
                new HotKeyCallback(Organize));
            HotKey.Add(HKCategory.Agents, HKSubCat.None,
                $"{Language.GetString(LocString.SetOrganizerHB)}-{Number:D2}",
                new HotKeyCallback(SetHotBag));
            PacketHandler.RegisterClientToServerViewer(0x09, new PacketViewerCallback(OnSingleClick));

            Agent.OnItemCreated += new ItemCreatedEventHandler(CheckContOPL);
        }

        public void CheckContOPL(Item item)
        {
            if (item.Serial == m_Cont)
            {
                item.ObjPropList.Add(Language.Format(LocString.OrganizerHBA1, Number));
            }
        }

        private void OnSingleClick(PacketReader pvSrc, PacketHandlerEventArgs args)
        {
            uint serial = pvSrc.ReadUInt32();
            if (m_Cont == serial)
            {
                ushort gfx = 0;
                Item c = World.FindItem(m_Cont);
                if (c != null)
                {
                    gfx = c.ItemID.Value;
                }

                Client.Instance.SendToClient(new UnicodeMessage(m_Cont, gfx, Assistant.MessageType.Label, 0x3B2, 3,
                    Language.CliLocName, "", Language.Format(LocString.OrganizerHBA1, Number)));
            }
        }

        public override string Name
        {
            get { return $"{Language.GetString(LocString.Organizer)}-{Number}"; }
        }

        public override string Alias { get; set; }

        public override int Number { get; }

        public override void OnSelected(ListBox subList, params Button[] buttons)
        {
            m_SubList = subList;
            buttons[0].Text = Language.GetString(LocString.AddTarg);
            buttons[0].Visible = true;
            m_BagBTN = buttons[1];
            m_ArrBTN = buttons[2];
            if (m_Cont != 0)
            {
                buttons[1].Text = Language.GetString(LocString.ClearHB);
            }
            else
            {
                buttons[1].Text = Language.GetString(LocString.SetHB);
            }

            buttons[1].Visible = true;
            buttons[2].Text = Language.GetString(LocString.OrganizeNow);
            buttons[2].Visible = true;
            buttons[3].Text = Language.GetString(LocString.Remove);
            buttons[3].Visible = true;
            buttons[4].Text = Language.GetString(LocString.Clear);
            buttons[4].Visible = true;
            buttons[5].Text = Language.GetString(LocString.StopNow);
            buttons[5].Visible = true;

            m_SubList.BeginUpdate();
            m_SubList.Items.Clear();
            for (int i = 0; i < m_Items.Count; i++)
            {
                m_SubList.Items.Add((ItemID) (m_Items[i]));
            }

            m_SubList.EndUpdate();
        }

        public void SetHotBag()
        {
            World.Player.SendMessage(MsgLevel.Force, LocString.TargCont);
            Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnTargetBag));
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
                    SetHotBag();
                    break;
                case 3:
                    Organize();
                    break;
                case 4:
                    if (m_SubList.SelectedIndex >= 0 && m_SubList.SelectedIndex < m_Items.Count)
                    {
                        m_Items.RemoveAt(m_SubList.SelectedIndex);
                        m_SubList.Items.RemoveAt(m_SubList.SelectedIndex);
                    }

                    break;
                case 5:
                {
                    if (MessageBox.Show(Language.GetString(LocString.Confirm), Language.GetString(LocString.ClearList),
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Item bag = World.FindItem(m_Cont);
                        if (bag != null)
                        {
                            bag.ObjPropList.Remove(Language.Format(LocString.OrganizerHBA1, Number));
                            bag.OPLChanged();
                        }

                        m_SubList.Items.Clear();
                        m_Items.Clear();
                        m_Cont = 0;
                        m_BagBTN.Text = Language.GetString(LocString.SetHB);
                    }

                    break;
                }

                case 6:
                    DragDropManager.GracefulStop();
                    break;
            }
        }

        public void Organize()
        {
            if (m_Cont == 0 || m_Cont > 0x7FFFFF00)
            {
                World.Player.SendMessage(MsgLevel.Force, LocString.ContNotSet);
                return;
            }

            Item pack = World.Player.Backpack;
            if (pack == null)
            {
                World.Player.SendMessage(MsgLevel.Warning, LocString.NoBackpack);
                return;
            }

            int count = OrganizeChildren(pack);

            if (count > 0)
            {
                World.Player.SendMessage(LocString.OrgQueued, count);
            }
            else
            {
                World.Player.SendMessage(LocString.OrgNoItems);
            }
        }

        private int OrganizeChildren(Item container)
        {
            object dest = World.FindItem(m_Cont);
            if (dest == null)
            {
                dest = World.FindMobile(m_Cont);
                if (dest == null)
                {
                    return 0;
                }
            }

            /*else if ( World.Player.Backpack != null && ((Item)dest).IsChildOf( World.Player ) && !((Item)dest).IsChildOf( World.Player.Backpack ) )
            {
                 return 0;
            }*/

            return OrganizeChildren(container, dest);
        }

        private int OrganizeChildren(Item container, object dest)
        {
            int count = 0;
            for (int i = 0; i < container.Contains.Count; i++)
            {
                Item item = (Item) container.Contains[i];
                if (item.Serial != m_Cont && !item.IsChildOf(dest))
                {
                    count += OrganizeChildren(item, dest);
                    if (m_Items.Contains(item.ItemID.Value))
                    {
                        if (dest is Item)
                        {
                            DragDropManager.DragDrop(item, (Item) dest);
                        }
                        else if (dest is Mobile)
                        {
                            DragDropManager.DragDrop(item, ((Mobile) dest).Serial);
                        }

                        count++;
                    }
                }
            }

            return count;
        }

        private void OnTarget(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            if (Engine.MainWindow != null)
            {
                Engine.MainWindow.SafeAction(s => s.ShowMe());
            }

            if (!location && serial.IsItem && World.Player != null)
            {
                Add(gfx);
            }
        }

        public void Add(ushort gfx)
        {
            if (m_Items != null && m_Items.Contains(gfx))
            {
                World.Player?.SendMessage(MsgLevel.Force, LocString.ItemExists);
            }
            else
            {
                m_Items?.Add(gfx);

                m_SubList?.Items.Add((ItemID)gfx);

                World.Player?.SendMessage(MsgLevel.Force, LocString.ItemAdded);
            }
        }

        private void OnTargetBag(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            if (Engine.MainWindow != null)
            {
                Engine.MainWindow.SafeAction(s => s.ShowMe());
            }

            if (!location && serial > 0 && serial <= 0x7FFFFF00)
            {
                Item bag = World.FindItem(m_Cont);
                if (bag != null && bag.ObjPropList != null)
                {
                    bag.ObjPropList.Remove(Language.Format(LocString.OrganizerHBA1, Number));
                    bag.OPLChanged();
                }

                m_Cont = serial;
                if (m_BagBTN != null)
                {
                    m_BagBTN.Text = Language.GetString(LocString.ClearHB);
                }

                if (World.Player != null)
                {
                    World.Player.SendMessage(MsgLevel.Force, LocString.ContSet);
                }

                bag = World.FindItem(m_Cont);
                if (bag != null && bag.ObjPropList != null)
                {
                    bag.ObjPropList.Add(Language.Format(LocString.OrganizerHBA1, Number));
                    bag.OPLChanged();
                }
            }
        }

        public override void Clear()
        {
            m_Items.Clear();
            m_Cont = 0;
            if (m_BagBTN != null)
            {
                m_BagBTN.Text = Language.GetString(LocString.SetHB);
            }
        }

        public override void Save(XmlTextWriter xml)
        {
            xml.WriteAttributeString("hotbag", m_Cont.ToString());
            xml.WriteAttributeString("alias", Alias);

            for (int i = 0; i < m_Items.Count; i++)
            {
                xml.WriteStartElement("item");
                xml.WriteAttributeString("id", m_Items[i].Value.ToString());
                xml.WriteEndElement();
            }
        }

        public override void Load(XmlElement node)
        {
            try
            {
                m_Cont = Convert.ToUInt32(node.GetAttribute("hotbag"));
            }
            catch
            {
                // ignored
            }

            try
            {
                Alias = node.GetAttribute("alias");
            }
            catch
            {
                Alias = string.Empty;
            }

            if (m_BagBTN != null)
            {
                m_BagBTN.Text = Language.GetString(m_Cont != 0 ? LocString.ClearHB : LocString.SetHB);
            }

            foreach (XmlElement el in node.GetElementsByTagName("item"))
            {
                try
                {
                    string gfx = el.GetAttribute("id");
                    m_Items.Add(Convert.ToUInt16(gfx));
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}