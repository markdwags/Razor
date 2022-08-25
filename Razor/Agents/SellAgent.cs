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
using System.Windows.Forms;
using System.Xml;
using Assistant.Gumps.Internal;
using Assistant.UI;

namespace Assistant.Agents
{
    public class SellAgent : Agent
    {
        public static SellAgent Instance { get; private set; }

        public static void Initialize()
        {
            Agent.Add(Instance = new SellAgent());
        }

        private ListBox m_SubList;
        private Button m_EnableBTN;
        private Button m_HotBTN;
        private Button m_AmountButton;
        private readonly List<ushort> m_Items;
        private Serial m_HotBag;
        private bool m_Enabled;
        
        public SellAgent()
        {
            m_Items = new List<ushort>();
            PacketHandler.RegisterServerToClientViewer(0x9E, new PacketViewerCallback(OnVendorSell));
            PacketHandler.RegisterClientToServerViewer(0x09, new PacketViewerCallback(OnSingleClick));

            Number = 0;

            HotKey.Add(HKCategory.Agents, HKSubCat.None, Language.GetString(LocString.SetSellAgentHotBag),
                new HotKeyCallback(SetHotBag));

            Agent.OnItemCreated += new ItemCreatedEventHandler(CheckHBOPL);
        }

        private void CheckHBOPL(Item item)
        {
            if (item.Serial == m_HotBag)
            {
                item.ObjPropList.Add(Language.GetString(LocString.SellHB));
            }
        }

        private void OnSingleClick(PacketReader pvSrc, PacketHandlerEventArgs args)
        {
            Serial serial = pvSrc.ReadUInt32();
            if (m_HotBag == serial)
            {
                ushort gfx = 0;
                Item c = World.FindItem(m_HotBag);
                if (c != null)
                {
                    gfx = c.ItemID.Value;
                }

                Client.Instance.SendToClient(new UnicodeMessage(m_HotBag, gfx, Assistant.MessageType.Label, 0x3B2, 3,
                    Language.CliLocName, "", Language.GetString(LocString.SellHB)));
            }
        }

        public override void Clear()
        {
            m_Items.Clear();
        }

        private void OnVendorSell(PacketReader pvSrc, PacketHandlerEventArgs args)
        {
            if (!m_Enabled || !Client.Instance.AllowBit(FeatureBit.SellAgent) ||
                (m_Items.Count == 0 && m_HotBag == Serial.Zero))
            {
                return;
            }

            Item hb = null;
            if (m_HotBag != Serial.Zero)
            {
                hb = World.FindItem(m_HotBag);
                if (hb == null)
                {
                    //m_HotBag = Serial.Zero;
                    //SetHBText();
                    World.Player.SendMessage(MsgLevel.Warning, "Sell Agent HotBag could not be found.");

                    if (m_Items.Count == 0)
                    {
                        return;
                    }
                }
            }

            int total = 0;

            uint serial = pvSrc.ReadUInt32();
            Mobile vendor = World.FindMobile(serial);
            if (vendor == null)
            {
                World.AddMobile(vendor = new Mobile(serial));
            }

            int count = pvSrc.ReadUInt16();

            int maxSell = Config.GetInt("SellAgentMax");
            int sold = 0;
            List<SellListItem> list = new List<SellListItem>(count);
            for (int i = 0; i < count && (sold < maxSell || maxSell <= 0); i++)
            {
                uint ser = pvSrc.ReadUInt32();
                ushort gfx = pvSrc.ReadUInt16();
                ushort hue = pvSrc.ReadUInt16();
                ushort amount = pvSrc.ReadUInt16();
                ushort price = pvSrc.ReadUInt16();

                pvSrc.ReadString(pvSrc.ReadUInt16()); //name

                Item item = World.FindItem(ser);

                if (m_Items.Contains(gfx) || (item != null && item != hb && item.IsChildOf(hb)))
                {
                    if (sold + amount > maxSell && maxSell > 0)
                    {
                        amount = (ushort) (maxSell - sold);
                    }

                    list.Add(new SellListItem(ser, amount));
                    total += amount * price;
                    sold += amount;
                }

                //if ( sold >= maxSell && maxSell > 0 ) break;
            }

            if (list.Count > 0)
            {
                Client.Instance.SendToServer(new VendorSellResponse(vendor, list));
                World.Player.SendMessage(MsgLevel.Force, LocString.SellTotals, sold, total);
                args.Block = true;
            }
        }

        public override string Name
        {
            get { return Language.GetString(LocString.Sell); }
        }

        public override string Alias { get; set; }

        public override int Number { get; }

        public override void OnSelected(ListBox subList, params Button[] buttons)
        {
            m_SubList = subList;
            m_EnableBTN = buttons[5];
            m_HotBTN = buttons[2];
            m_AmountButton = buttons[4];

            buttons[0].Text = Language.GetString(LocString.AddTarg);
            buttons[0].Visible = true;
            buttons[1].Text = Language.GetString(LocString.Remove);
            buttons[1].Visible = true;
            //button[2] = hotbutton
            buttons[2].Visible = true;
            buttons[3].Text = Language.GetString(LocString.Clear);
            buttons[3].Visible = true;
            m_AmountButton.Text = Language.Format(LocString.SellAmount, Config.GetInt("SellAgentMax"));
            buttons[4].Visible = true;
            buttons[5].Text = Language.GetString(m_Enabled ? LocString.PushDisable : LocString.PushEnable);
            buttons[5].Visible = true;

            SetHBText();
            m_SubList.BeginUpdate();
            m_SubList.Items.Clear();
            for (int i = 0; i < m_Items.Count; i++)
            {
                m_SubList.Items.Add((ItemID) ((ushort) m_Items[i]));
            }

            m_SubList.EndUpdate();

            if (!Client.Instance.AllowBit(FeatureBit.SellAgent) && Engine.MainWindow != null)
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
                    World.Player.SendMessage(MsgLevel.Force, LocString.TargItemAdd);
                    Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnTarget));
                    break;
                case 2:
                    if (m_SubList.SelectedIndex >= 0)
                    {
                        m_Items.RemoveAt(m_SubList.SelectedIndex);
                        m_SubList.Items.RemoveAt(m_SubList.SelectedIndex);
                        m_SubList.SelectedIndex = -1;
                    }

                    break;
                case 3:
                    if (m_HotBag == Serial.Zero)
                    {
                        World.Player.SendMessage(MsgLevel.Force, LocString.TargCont);
                        Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnHBTarget));
                    }
                    else
                    {
                        Item hb = World.FindItem(m_HotBag);
                        if (hb != null)
                        {
                            if (hb.ObjPropList.Remove(Language.GetString(LocString.SellHB)))
                            {
                                hb.OPLChanged();
                            }
                        }

                        m_HotBag = Serial.Zero;
                        SetHBText();
                    }

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

                    InputDialogGump gump = new InputDialogGump(OnChangeSellMaxAmount, 0, Language.GetString(LocString.EnterAmount), Config.GetInt("SellAgentMax").ToString());
                    gump.SendGump();

                    break;
                case 6:
                    m_Enabled = !m_Enabled;
                    m_EnableBTN.Text = Language.GetString(m_Enabled ? LocString.PushDisable : LocString.PushEnable);
                    break;
            }
        }
        
        private bool OnChangeSellMaxAmount(int gfx, string amount)
        {
            if (int.TryParse(amount, out int parsedAmount))
            {
                Config.SetProperty("SellAgentMax", parsedAmount);
                m_AmountButton.Text = Language.Format(LocString.SellAmount, Config.GetInt("SellAgentMax"));

                return true;
            }

            return false;
        }

        private void SetHBText()
        {
            m_HotBTN?.SafeAction(s =>
            {
                s.Text = Language.GetString(m_HotBag == Serial.Zero ? LocString.SetHB : LocString.ClearHB);
            });
        }

        private void OnTarget(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            Engine.MainWindow.SafeAction(s => s.ShowMe());
            if (!location && serial.IsItem)
            {
                Add(gfx);
            }
        }

        public void Add(ItemID itemId)
        {
            m_Items?.Add(itemId);
            m_SubList?.Items.Add((ItemID)itemId);

            World.Player?.SendMessage(MsgLevel.Force, LocString.ItemAdded);
        }

        private void OnHBTarget(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            Engine.MainWindow.SafeAction(s => s.ShowMe());
            if (!location && serial.IsItem)
            {
                m_HotBag = serial;
                SetHBText();

                Item hb = World.FindItem(m_HotBag);
                if (hb != null)
                {
                    hb.ObjPropList.Add(Language.GetString(LocString.SellHB));
                    hb.OPLChanged();
                }
            }
        }

        public void SetHotBag()
        {
            World.Player.SendMessage(MsgLevel.Force, LocString.TargCont);
            Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnHBTarget));
        }

        /*private void OnHBTarget(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            Engine.MainWindow.SafeAction(s => s.ShowMe(););

            Item hb = World.FindItem(m_HotBag);
            if (hb != null)
            {
                if (hb.ObjPropList.Remove(Language.Format(LocString.RestockHBA1, m_Num)))
                {
                    hb.OPLChanged();
                }
            }

            if (!location && serial.IsItem)
            {
                m_HotBag = serial;
            }
            else
            {
                m_HotBag = Serial.Zero;
            }

            hb = World.FindItem(m_HotBag);
            if (hb != null)
            {
                hb.ObjPropList.Add(Language.Format(LocString.RestockHBA1, m_Num));
                hb.OPLChanged();
            }

            SetHBText();
        }*/

        public override void Save(XmlTextWriter xml)
        {
            if (m_Items == null)
            {
                return;
            }

            xml.WriteAttributeString("enabled", m_Enabled.ToString());

            if (m_HotBag != Serial.Zero)
            {
                xml.WriteStartElement("hotbag");
                xml.WriteString(m_HotBag.ToString());
                xml.WriteEndElement();
            }

            foreach (ushort iid in m_Items)
            {
                xml.WriteStartElement("item");
                xml.WriteAttributeString("id", iid.ToString());
                xml.WriteEndElement();
            }
        }


        public override void Load(XmlElement node)
        {
            try
            {
                m_Enabled = bool.Parse(node.GetAttribute("enabled"));
            }
            catch
            {
                m_Enabled = false;
            }

            try
            {
                m_HotBag = node["hotbag"] != null ? Serial.Parse(node["hotbag"].InnerText) : Serial.Zero;
            }
            catch
            {
                m_HotBag = Serial.Zero;
            }

            foreach (XmlElement el in node.GetElementsByTagName("item"))
            {
                try
                {
                    string str = el.GetAttribute("id");
                    m_Items.Add(Convert.ToUInt16(str));
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}