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
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using Assistant.Agents;
using Assistant.UI;

namespace Assistant.Agents
{
    public class ScavengerAgent : Agent
    {
        private static readonly ScavengerAgent m_Instance = new ScavengerAgent();

        public static ScavengerAgent Instance
        {
            get { return m_Instance; }
        }

        public static bool Debug = false;

        public static void Initialize()
        {
            Agent.Add(m_Instance);
        }

        private bool m_Enabled;
        private Serial m_Bag;
        private ListBox m_SubList;
        private Button m_EnButton;
        private readonly List<ItemID> m_Items;

        private List<Serial> m_Cache;
        private Item m_BagRef;

        public ScavengerAgent()
        {
            m_Items = new List<ItemID>();

            HotKey.Add(HKCategory.Agents, LocString.ClearScavCache, new HotKeyCallback(ClearCache));
            HotKey.Add(HKCategory.Agents, LocString.ScavengerEnableDisable, new HotKeyCallback(OnEnableDisable));
            HotKey.Add(HKCategory.Agents, LocString.ScavengerSetHotBag, new HotKeyCallback(OnSetHotBag));
            HotKey.Add(HKCategory.Agents, LocString.ScavengerAddTarget, new HotKeyCallback(OnAddToHotBag));

            PacketHandler.RegisterClientToServerViewer(0x09, new PacketViewerCallback(OnSingleClick));

            Agent.OnItemCreated += new ItemCreatedEventHandler(CheckBagOPL);
        }

        public void ToggleEnabled()
        {
            OnEnableDisable();
        }

        public void Disable()
        {
            m_Enabled = false;
            UpdateEnableButton();
            World.Player.SendMessage(MsgLevel.Force, "Scavenger Agent Disabled");
        }

        public void Enable()
        {
            m_Enabled = true;
            UpdateEnableButton();
            World.Player.SendMessage(MsgLevel.Force, "Scavenger Agent Enabled");
        }

        private void OnEnableDisable()
        {
            m_Enabled = !m_Enabled;
            UpdateEnableButton();

            if (m_Enabled)
            {
                World.Player.SendMessage(MsgLevel.Force, "Scavenger Agent Enabled");
            }
            else
            {
                World.Player.SendMessage(MsgLevel.Force, "Scavenger Agent Disabled");
            }
        }

        public void OnAddToHotBag()
        {
            World.Player.SendMessage(MsgLevel.Force, LocString.TargItemAdd);
            Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnTarget));
        }

        public void OnSetHotBag()
        {
            World.Player.SendMessage(LocString.TargCont);
            Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnTargetBag));
        }

        private void CheckBagOPL(Item item)
        {
            if (item.Serial == m_Bag)
            {
                item.ObjPropList.Add(Language.GetString(LocString.ScavengerHB));
            }
        }

        private void OnSingleClick(PacketReader pvSrc, PacketHandlerEventArgs args)
        {
            Serial serial = pvSrc.ReadUInt32();
            if (m_Bag == serial)
            {
                ushort gfx = 0;
                Item c = World.FindItem(m_Bag);
                if (c != null)
                {
                    gfx = c.ItemID.Value;
                }

                Client.Instance.SendToClient(new UnicodeMessage(m_Bag, gfx, Assistant.MessageType.Label, 0x3B2, 3,
                    Language.CliLocName, "", Language.GetString(LocString.ScavengerHB)));
            }
        }

        public override void Clear()
        {
            m_Items.Clear();
            m_BagRef = null;
        }

        public bool Enabled
        {
            get { return m_Enabled; }
        }

        public override string Name
        {
            get { return Language.GetString(LocString.Scavenger); }
        }

        public override void OnSelected(ListBox subList, params Button[] buttons)
        {
            buttons[0].Text = Language.GetString(LocString.AddTarg);
            buttons[0].Visible = true;
            buttons[1].Text = Language.GetString(LocString.Remove);
            buttons[1].Visible = true;
            buttons[2].Text = Language.GetString(LocString.SetHB);
            buttons[2].Visible = true;
            buttons[3].Text = Language.GetString(LocString.ClearList);
            buttons[3].Visible = true;
            buttons[4].Text = Language.GetString(LocString.ClearScavCache);
            buttons[4].Visible = true;
            m_EnButton = buttons[5];
            m_EnButton.Visible = true;
            UpdateEnableButton();

            m_SubList = subList;
            subList.BeginUpdate();
            subList.Items.Clear();

            for (int i = 0; i < m_Items.Count; i++)
            {
                subList.Items.Add(m_Items[i]);
            }

            subList.EndUpdate();
        }

        private void UpdateEnableButton()
        {
            if (m_EnButton != null)
            {
                m_EnButton.Text = Language.GetString(m_Enabled ? LocString.PushDisable : LocString.PushEnable);
            }
        }

        public override void OnButtonPress(int num)
        {
            DebugLog("User pressed button {0}", num);
            switch (num)
            {
                case 1:
                    World.Player.SendMessage(MsgLevel.Force, LocString.TargItemAdd);
                    Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnTarget));
                    break;
                case 2:
                    if (m_SubList.SelectedIndex >= 0 && m_SubList.SelectedIndex < m_Items.Count)
                    {
                        m_Items.RemoveAt(m_SubList.SelectedIndex);
                        m_SubList.Items.RemoveAt(m_SubList.SelectedIndex);
                    }

                    break;
                case 3:
                    World.Player.SendMessage(LocString.TargCont);
                    Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnTargetBag));
                    break;
                case 4:
                    if (MessageBox.Show(Language.GetString(LocString.Confirm), Language.GetString(LocString.ClearList),
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        m_SubList.Items.Clear();
                        m_Items.Clear();
                    }

                    break;
                case 5:
                    ClearCache();
                    break;
                case 6:
                    m_Enabled = !m_Enabled;
                    UpdateEnableButton();
                    break;
            }
        }

        public void ClearCache()
        {
            DebugLog("Clearing Cache of {0} items", m_Cache == null ? -1 : m_Cache.Count);
            if (m_Cache != null)
            {
                m_Cache.Clear();
            }

            if (World.Player != null)
            {
                World.Player.SendMessage(MsgLevel.Force, "Scavenger agent item cache cleared.");
            }
        }

        private void OnTarget(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            Engine.MainWindow.SafeAction(s => s.ShowMe());

            if (location || !serial.IsItem)
            {
                return;
            }

            Item item = World.FindItem(serial);
            if (item == null)
            {
                return;
            }

            m_Items.Add(item.ItemID);

            if (m_SubList != null)
            {
                m_SubList.Items.Add(item.ItemID);
            }


            DebugLog("Added item {0}", item);

            World.Player.SendMessage(MsgLevel.Force, LocString.ItemAdded);
        }

        private void OnTargetBag(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            Engine.MainWindow.SafeAction(s => s.ShowMe());

            if (location || !serial.IsItem)
            {
                return;
            }

            if (m_BagRef == null)
            {
                m_BagRef = World.FindItem(m_Bag);
            }

            if (m_BagRef != null)
            {
                m_BagRef.ObjPropList.Remove(Language.GetString(LocString.ScavengerHB));
                m_BagRef.OPLChanged();
            }

            DebugLog("Set bag to {0}", serial);
            m_Bag = serial;
            m_BagRef = World.FindItem(m_Bag);
            if (m_BagRef != null)
            {
                m_BagRef.ObjPropList.Add(Language.GetString(LocString.ScavengerHB));
                m_BagRef.OPLChanged();
            }

            World.Player.SendMessage(MsgLevel.Force, LocString.ContSet, m_Bag);
        }

        public void Uncache(Serial s)
        {
            if (m_Cache != null)
            {
                m_Cache.Remove(s);
            }
        }

        public void Scavenge(Item item)
        {
            DebugLog("Checking WorldItem {0} ...", item);
            if (!m_Enabled || !m_Items.Contains(item.ItemID) || World.Player.Backpack == null || World.Player.IsGhost ||
                World.Player.Weight >= World.Player.MaxWeight)
            {
                DebugLog("... skipped.");
                return;
            }

            if (m_Cache == null)
            {
                m_Cache = new List<Serial>();
            }
            else if (m_Cache.Count >= 190)
            {
                m_Cache.RemoveRange(0, 50);
            }

            if (m_Cache.Contains(item.Serial))
            {
                DebugLog("Item was cached.");
                return;
            }

            Item bag = m_BagRef;
            if (bag == null || bag.Deleted)
            {
                bag = m_BagRef = World.FindItem(m_Bag);
            }

            if (bag == null || bag.Deleted || !bag.IsChildOf(World.Player.Backpack))
            {
                bag = World.Player.Backpack;
            }

            m_Cache.Add(item.Serial);
            DragDropManager.DragDrop(item, bag);
            DebugLog("Dragging to {0}!", bag);
        }

        private static void DebugLog(string str, params object[] args)
        {
            if (Debug)
            {
                using (System.IO.StreamWriter w = new System.IO.StreamWriter("Scavenger.log", true))
                {
                    w.Write(Engine.MistedDateTime.ToString("HH:mm:ss.fff"));
                    w.Write(":: ");
                    w.WriteLine(str, args);
                    w.Flush();
                }
            }
        }

        public override void Save(XmlTextWriter xml)
        {
            xml.WriteAttributeString("enabled", m_Enabled.ToString());

            if (m_Bag != Serial.Zero)
            {
                xml.WriteStartElement("bag");
                xml.WriteAttributeString("serial", m_Bag.ToString());
                xml.WriteEndElement();
            }

            for (int i = 0; i < m_Items.Count; i++)
            {
                xml.WriteStartElement("item");
                xml.WriteAttributeString("id", ((ItemID) m_Items[i]).Value.ToString());
                xml.WriteEndElement();
            }
        }

        public override void Load(XmlElement node)
        {
            try
            {
                m_Enabled = Boolean.Parse(node.GetAttribute("enabled"));
            }
            catch
            {
                m_Enabled = false;
            }

            try
            {
                m_Bag = Serial.Parse(node["bag"].GetAttribute("serial"));
            }
            catch
            {
                m_Bag = Serial.Zero;
            }

            foreach (XmlElement el in node.GetElementsByTagName("item"))
            {
                try
                {
                    string iid = el.GetAttribute("id");
                    m_Items.Add((ItemID) Convert.ToUInt16(iid));
                }
                catch
                {
                }
            }
        }
    }
}