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
    public class UseOnceAgent : Agent
    {
        public static UseOnceAgent Instance { get; private set; }

        public static void Initialize()
        {
            Agent.Add(Instance = new UseOnceAgent());
        }

        private ListBox m_SubList;
        private readonly ArrayList m_Items;

        public UseOnceAgent()
        {
            m_Items = new ArrayList();
            PacketHandler.RegisterClientToServerViewer(0x09, new PacketViewerCallback(OnSingleClick));
            HotKey.Add(HKCategory.Agents, LocString.UseOnceAgent, new HotKeyCallback(OnHotKey));
            HotKey.Add(HKCategory.Agents, LocString.AddUseOnce, new HotKeyCallback(OnAdd));
            HotKey.Add(HKCategory.Agents, LocString.AddUseOnceContainer, new HotKeyCallback(OnAddContainer));

            Agent.OnItemCreated += new ItemCreatedEventHandler(CheckItemOPL);
        }

        public override void Clear()
        {
            m_Items.Clear();
        }

        private void CheckItemOPL(Item newItem)
        {
            for (int i = 0; i < m_Items.Count; i++)
            {
                if (m_Items[i] is Serial)
                {
                    if (newItem.Serial == (Serial) m_Items[i])
                    {
                        m_Items[i] = newItem;
                        newItem.ObjPropList.Add(Language.GetString(LocString.UseOnce));
                        break;
                    }
                }
            }
        }

        private void OnSingleClick(PacketReader pvSrc, PacketHandlerEventArgs args)
        {
            Serial serial = pvSrc.ReadUInt32();
            for (int i = 0; i < m_Items.Count; i++)
            {
                Item item;
                if (m_Items[i] is Serial)
                {
                    item = World.FindItem((Serial) m_Items[i]);
                    if (item != null)
                    {
                        m_Items[i] = item;
                    }
                }

                item = m_Items[i] as Item;
                if (item == null)
                {
                    continue;
                }

                if (item.Serial == serial)
                {
                    Client.Instance.SendToClient(new UnicodeMessage(item.Serial, item.ItemID,
                        Assistant.MessageType.Label, 0x3B2, 3, Language.CliLocName, "",
                        Language.Format(LocString.UseOnceHBA1, i + 1)));
                    break;
                }
            }
        }

        public override string Name
        {
            get { return Language.GetString(LocString.UseOnce); }
        }

        public override void OnSelected(ListBox subList, params Button[] buttons)
        {
            m_SubList = subList;
            buttons[0].Text = Language.GetString(LocString.AddTarg);
            buttons[0].Visible = true;
            buttons[1].Text = Language.GetString(LocString.AddContTarg);
            buttons[1].Visible = true;
            buttons[2].Text = Language.GetString(LocString.RemoveTarg);
            buttons[2].Visible = true;
            buttons[3].Text = Language.GetString(LocString.ClearList);
            buttons[3].Visible = true;

            m_SubList.BeginUpdate();
            m_SubList.Items.Clear();

            for (int i = 0; i < m_Items.Count; i++)
            {
                if (m_Items[i] is Serial)
                {
                    Item item = World.FindItem((Serial) m_Items[i]);
                    if (item != null)
                    {
                        m_Items[i] = item;
                    }
                }

                m_SubList.Items.Add(m_Items[i]);
            }

            m_SubList.EndUpdate();

            if (!Client.Instance.AllowBit(FeatureBit.UseOnceAgent) && Engine.MainWindow != null)
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    Engine.MainWindow.SafeAction(s => s.LockControl(buttons[i]));
                }

                Engine.MainWindow.SafeAction(s => s.LockControl(subList));
            }
        }

        public override void OnButtonPress(int num)
        {
            switch (num)
            {
                case 1:
                    OnAdd();
                    break;
                case 2:
                    World.Player.SendMessage(MsgLevel.Force, LocString.TargCont);
                    Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnTargetBag));
                    break;
                case 3:
                    World.Player.SendMessage(MsgLevel.Force, LocString.TargItemRem);
                    Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnTargetRemove));
                    break;
                case 4:

                    if (MessageBox.Show(Language.GetString(LocString.Confirm), Language.GetString(LocString.ClearList),
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        for (int i = 0; i < m_Items.Count; i++)
                        {
                            if (m_Items[i] is Item)
                            {
                                Item item = (Item) m_Items[i];

                                item.ObjPropList.Remove(Language.GetString(LocString.UseOnce));
                                item.OPLChanged();
                            }
                        }

                        m_SubList.Items.Clear();
                        m_Items.Clear();
                    }

                    break;
            }
        }

        public void OnAdd()
        {
            World.Player.SendMessage(MsgLevel.Force, LocString.TargItemAdd);
            Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnTarget));
        }

        public void OnAddContainer()
        {
            World.Player.SendMessage(MsgLevel.Force, LocString.TargItemAdd);
            Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnTargetBag));
        }

        private void OnTarget(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            if (Config.GetBool("AlwaysOnTop"))
            {
                Engine.MainWindow.SafeAction(s => s.ShowMe());
            }

            if (!location && serial.IsItem)
            {
                Item item = World.FindItem(serial);
                if (item == null)
                {
                    World.Player.SendMessage(MsgLevel.Force, LocString.ItemNotFound);
                    return;
                }

                item.ObjPropList.Add(Language.GetString(LocString.UseOnce));
                item.OPLChanged();

                m_Items.Add(item);
                if (m_SubList != null)
                {
                    m_SubList.Items.Add(item);
                }

                World.Player.SendMessage(MsgLevel.Force, LocString.ItemAdded);
            }
        }

        private void OnTargetRemove(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            Engine.MainWindow.SafeAction(s => s.ShowMe());
            if (!location && serial.IsItem)
            {
                for (int i = 0; i < m_Items.Count; i++)
                {
                    bool rem = false;
                    if (m_Items[i] is Item)
                    {
                        if (((Item) m_Items[i]).Serial == serial)
                        {
                            ((Item) m_Items[i]).ObjPropList.Remove(Language.GetString(LocString.UseOnce));
                            ((Item) m_Items[i]).OPLChanged();

                            rem = true;
                        }
                    }
                    else if (m_Items[i] is Serial)
                    {
                        if (((Serial) m_Items[i]) == serial)
                        {
                            rem = true;
                        }
                    }

                    if (rem)
                    {
                        m_Items.RemoveAt(i);
                        m_SubList.Items.RemoveAt(i);
                        World.Player.SendMessage(MsgLevel.Force, LocString.ItemRemoved);
                        return;
                    }
                }

                World.Player.SendMessage(MsgLevel.Force, LocString.ItemNotFound);
            }
        }

        private void OnTargetBag(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            if (Config.GetBool("AlwaysOnTop"))
            {
                Engine.MainWindow.SafeAction(s => s.ShowMe());
            }

            Engine.MainWindow.SafeAction(s => s.ShowMe());
            if (!location && serial.IsItem)
            {
                Item i = World.FindItem(serial);
                if (i != null && i.Contains.Count > 0)
                {
                    for (int ci = 0; ci < i.Contains.Count; ci++)
                    {
                        Item toAdd = i.Contains[ci] as Item;

                        if (toAdd != null)
                        {
                            toAdd.ObjPropList.Add(Language.GetString(LocString.UseOnce));
                            toAdd.OPLChanged();
                            m_Items.Add(toAdd);

                            if (m_SubList != null)
                            {
                                m_SubList.Items.Add(toAdd);
                            }
                        }
                    }

                    World.Player.SendMessage(MsgLevel.Force, LocString.ItemsAdded, i.Contains.Count);
                }
            }
        }

        public override void Save(XmlTextWriter xml)
        {
            for (int i = 0; i < m_Items.Count; i++)
            {
                xml.WriteStartElement("item");
                if (m_Items[i] is Item)
                {
                    xml.WriteAttributeString("serial", ((Item) m_Items[i]).Serial.Value.ToString());
                }
                else
                {
                    xml.WriteAttributeString("serial", ((Serial) m_Items[i]).Value.ToString());
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
                    m_Items.Add((Serial) Convert.ToUInt32(ser));
                }
                catch
                {
                }
            }
        }

        public void OnHotKey()
        {
            if (World.Player == null || !Client.Instance.AllowBit(FeatureBit.UseOnceAgent))
            {
                return;
            }

            if (m_Items.Count <= 0)
            {
                World.Player.SendMessage(MsgLevel.Error, LocString.UseOnceEmpty);
            }
            else
            {
                Item item = null;
                if (m_Items[0] is Item)
                {
                    item = (Item) m_Items[0];
                }
                else if (m_Items[0] is Serial)
                {
                    item = World.FindItem((Serial) m_Items[0]);
                }

                try
                {
                    m_Items.RemoveAt(0);
                    if (m_SubList != null && m_SubList.Items.Count > 0)
                    {
                        m_SubList.Items.RemoveAt(0);
                    }
                }
                catch
                {
                }

                if (item != null)
                {
                    item.ObjPropList.Remove(Language.GetString(LocString.UseOnce));
                    item.OPLChanged();

                    World.Player.SendMessage(LocString.UseOnceStatus, item, m_Items.Count);
                    PlayerData.DoubleClick(item);
                }
                else
                {
                    World.Player.SendMessage(LocString.UseOnceError);
                    OnHotKey();
                }
            }
        }
    }
}