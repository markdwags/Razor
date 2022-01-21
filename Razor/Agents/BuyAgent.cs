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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Assistant.Gumps.Internal;
using Assistant.UI;

namespace Assistant.Agents
{
    public class BuyAgent : Agent
    {
        public class BuyEntry
        {
            public BuyEntry(ItemID id, ushort amount)
            {
                Id = id;
                Amount = amount;
            }

            public readonly ItemID Id;
            public ushort Amount;

            public override string ToString()
            {
                return $"{Id}\t{Amount}";
            }
        }

        private class ItemXYComparer : IComparer<Item>
        {
            public static readonly ItemXYComparer Instance = new ItemXYComparer();

            private ItemXYComparer()
            {
            }

            public int Compare(Item x, Item y)
            {
                if (!(x is Item))
                {
                    return 1;
                }
                else if (!(y is Item))
                {
                    return -1;
                }

                int xsum = x.Position.X + x.Position.Y * 200;
                int ysum = y.Position.X + y.Position.Y * 200;

                return xsum.CompareTo(ysum);
            }
        }

        private static readonly ArrayList m_Instances = new ArrayList();

        public static List<BuyAgent> Agents { get; set; }

        public static void Initialize()
        {
            PacketHandler.RegisterServerToClientViewer(0x74, new PacketViewerCallback(ExtBuyInfo));
            PacketHandler.RegisterServerToClientViewer(0x24, new PacketViewerCallback(DisplayBuy));
            PacketHandler.RegisterServerToClientViewer(0x3B, new PacketViewerCallback(EndVendorBuy));

            int maxAgents = Config.GetAppSetting<int>("MaxBuyAgents") == 0
                ? 20
                : Config.GetAppSetting<int>("MaxBuyAgents");

            Agents = new List<BuyAgent>();

            for (int i = 1; i <= maxAgents; i++)
            {
                BuyAgent b = new BuyAgent(i);
                m_Instances.Add(b);
                Agent.Add(b);

                Agents.Add(b);
            }
        }

        private ListBox m_SubList;
        private Button m_EnableBTN;
        public readonly List<BuyEntry> Items;
        private bool m_Enabled;
        private bool _fromGump;

        public BuyAgent(int num)
        {
            Number = num;
            Items = new List<BuyEntry>();
        }

        private static void DisplayBuy(PacketReader p, PacketHandlerEventArgs args)
        {
            Serial serial = p.ReadUInt32();
            ushort gump = p.ReadUInt16();

            if (gump != 0x30 || !serial.IsMobile || !Client.Instance.AllowBit(FeatureBit.BuyAgent) ||
                World.Player == null)
            {
                return;
            }

            Mobile vendor = World.FindMobile(serial);
            if (vendor == null)
            {
                return;
            }

            Item pack = vendor.GetItemOnLayer(Layer.ShopBuy);
            if (pack == null || pack.Contains == null || pack.Contains.Count <= 0)
            {
                return;
            }

            pack.Contains.Sort(ItemXYComparer.Instance);

            int total = 0;
            int cost = 0;
            List<VendorBuyItem> buyList = new List<VendorBuyItem>();
            Dictionary<ushort, int> found = new Dictionary<ushort, int>();
            bool lowGoldWarn = false;
            for (int i = 0; i < pack.Contains.Count; i++)
            {
                Item item = (Item) pack.Contains[i];
                if (item == null)
                {
                    continue;
                }

                foreach (BuyAgent ba in m_Instances)
                {
                    if (ba == null || ba.Items == null || !ba.m_Enabled)
                    {
                        continue;
                    }

                    for (int a = 0; a < ba.Items.Count; a++)
                    {
                        BuyEntry b = (BuyEntry) ba.Items[a];
                        if (b == null)
                        {
                            continue;
                        }

                        bool dupe = false;
                        foreach (VendorBuyItem vbi in buyList)
                        {
                            if (vbi.Serial == item.Serial)
                            {
                                dupe = true;
                            }
                        }

                        if (dupe)
                        {
                            continue;
                        }

                        // fucking osi and their blank scrolls
                        if (b.Id == item.ItemID.Value || (b.Id == 0x0E34 && item.ItemID.Value == 0x0EF3) ||
                            (b.Id == 0x0EF3 && item.ItemID.Value == 0x0E34))
                        {
                            int count = World.Player.Backpack.GetCount(b.Id);
                            if (found.ContainsKey(b.Id))
                            {
                                count += (int) found[b.Id];
                            }

                            if (count < b.Amount && b.Amount > 0)
                            {
                                count = b.Amount - count;
                                if (count >= item.Amount)
                                {
                                    count = item.Amount;
                                }
                                else if (count <= 0)
                                {
                                    continue;
                                }

                                if (!found.ContainsKey(b.Id))
                                {
                                    found.Add(b.Id, (int) count);
                                }
                                else
                                {
                                    found[b.Id] = (int) found[b.Id] + (int) count;
                                }

                                buyList.Add(new VendorBuyItem(item.Serial, count, item.Price));
                                total += count;
                                cost += item.Price * count;
                            }
                        }
                    }
                }
            }

            if (!Config.GetBool("BuyAgentsIgnoreGold"))
            {
                if (cost > World.Player.Gold && cost < 2000 && buyList.Count > 0)
                {
                    lowGoldWarn = true;
                    do
                    {
                        VendorBuyItem vbi = (VendorBuyItem)buyList[0];
                        if (cost - vbi.TotalCost <= World.Player.Gold)
                        {
                            while (cost > World.Player.Gold && vbi.Amount > 0)
                            {
                                cost -= vbi.Price;
                                --vbi.Amount;
                                --total;
                            }

                            if (vbi.Amount <= 0)
                            {
                                buyList.RemoveAt(0);
                            }
                        }
                        else
                        {
                            cost -= vbi.TotalCost;
                            total -= vbi.Amount;
                            buyList.RemoveAt(0);
                        }
                    } while (cost > World.Player.Gold && buyList.Count > 0);
                }
            }            

            if (buyList.Count > 0)
            {
                args.Block = true;
                BuyLists[serial] = buyList;
                Client.Instance.SendToServer(new VendorBuyResponse(serial, buyList));

                if (Config.GetBool("BuyAgentsIgnoreGold"))
                {
                    World.Player.SendMessage(MsgLevel.Force, LocString.BuyAgentAttempt, total, cost);
                }
                else
                {
                    World.Player.SendMessage(MsgLevel.Force, LocString.BuyTotals, total, cost);
                }
            }

            if (lowGoldWarn)
            {
                World.Player.SendMessage(MsgLevel.Force, LocString.BuyLowGold);
            }
        }

        private static readonly Dictionary<uint, List<VendorBuyItem>> BuyLists = new Dictionary<uint, List<VendorBuyItem>>();
        private static void ExtBuyInfo(PacketReader p, PacketHandlerEventArgs args)
        {
            Serial ser = p.ReadUInt32();
            Item pack = World.FindItem(ser);
            if (pack == null)
            {
                return;
            }

            byte count = p.ReadByte();
            if (count < pack.Contains.Count)
            {
                World.Player.SendMessage(MsgLevel.Debug,
                    "Buy Agent Warning: Contains Count {0} does not match ExtInfo {1}.", pack.Contains.Count, count);
            }

            pack.Contains.Sort(ItemXYComparer.Instance);

            for (int i = count - 1; i >= 0; i--)
            {
                if (i < pack.Contains.Count)
                {
                    Item item = (Item) pack.Contains[i];
                    item.Price = p.ReadInt32();
                    byte len = p.ReadByte();
                    item.BuyDesc = p.ReadStringSafe(len);
                }
                else
                {
                    p.ReadInt32();
                    p.Position += p.ReadByte() + 1;
                }
            }
        }

        private static void EndVendorBuy(PacketReader p, PacketHandlerEventArgs args)
        {
            if (!Client.Instance.AllowBit(FeatureBit.BuyAgent) || World.Player == null)
                return;
            uint serial = p.ReadUInt32();
            if (BuyLists.TryGetValue(serial, out var list))
            {
                BuyLists.Remove(serial);
                Mobile vendor = World.FindMobile(serial);
                if (vendor == null)
                    return;

                Item pack = vendor.GetItemOnLayer(Layer.ShopBuy);
                if (pack == null || pack.Contains == null || pack.Contains.Count <= 0)
                    return;

                for (int i = list.Count - 1; i >= 0; --i)
                {
                    VendorBuyItem vbi = list[i];
                    Item item = World.FindItem(vbi.Serial);
                    if (item == null || !pack.Contains.Contains(item))
                        continue;
                    item.Amount -= (ushort)vbi.Amount;
                    if (item.Amount <= 0)
                        item.Remove();
                }
            }
        }

        public override void Clear()
        {
            Items.Clear();
        }

        internal static void OnDisconnected()
        {
            BuyLists.Clear();
        }

        public override string Name => $"{Language.GetString(LocString.Buy)}-{Number}";

        public override string Alias { get; set; }

        public override int Number { get; }

        public override void OnSelected(ListBox subList, params Button[] buttons)
        {
            m_SubList = subList;
            m_EnableBTN = buttons[4];

            buttons[0].Text = Language.GetString(LocString.AddTarg);
            buttons[0].Visible = true;
            buttons[1].Text = Language.GetString(LocString.Edit);
            buttons[1].Visible = true;
            buttons[2].Text = Language.GetString(LocString.Remove);
            buttons[2].Visible = true;
            buttons[3].Text = Language.GetString(LocString.ClearList);
            buttons[3].Visible = true;
            buttons[4].Text = Language.GetString(m_Enabled ? LocString.PushDisable : LocString.PushEnable);
            buttons[4].Visible = true;

            m_SubList.BeginUpdate();
            m_SubList.Items.Clear();
            for (int i = 0; i < Items.Count; i++)
            {
                m_SubList.Items.Add(Items[i]);
            }

            m_SubList.EndUpdate();

            if (!Client.Instance.AllowBit(FeatureBit.BuyAgent) && Engine.MainWindow != null)
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
                    if (m_SubList == null)
                    {
                        break;
                    }

                    if (m_SubList.SelectedIndex >= 0)
                    {
                        BuyEntry e = (BuyEntry)Items[m_SubList.SelectedIndex];

                        InputDialogGump inputGump = new InputDialogGump(OnItemChangeAmountResponse, m_SubList.SelectedIndex, Language.GetString(LocString.EnterAmount), e.Amount.ToString());
                        inputGump.SendGump();
                    }

                    break;
                case 3:

                    if (MessageBox.Show(Language.GetString(LocString.Confirm), Language.GetString(LocString.ClearList),
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (m_SubList.SelectedIndex >= 0)
                        {
                            Items.RemoveAt(m_SubList.SelectedIndex);
                            m_SubList.Items.RemoveAt(m_SubList.SelectedIndex);
                            m_SubList.SelectedIndex = -1;
                        }
                    }

                    break;
                case 4:

                    if (MessageBox.Show(Language.GetString(LocString.Confirm), Language.GetString(LocString.ClearList),
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        m_SubList.Items.Clear();
                        Items.Clear();
                    }

                    break;
                case 5:
                    m_Enabled = !m_Enabled;
                    m_EnableBTN.Text = Language.GetString(m_Enabled ? LocString.PushDisable : LocString.PushEnable);
                    break;
            }
        }

        private void OnTarget(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            if (!location && !serial.IsMobile)
            {
                InputDialogGump inputGump = new InputDialogGump(OnItemTargetAmountResponse, gfx, Language.GetString(LocString.EnterAmount), "0");
                inputGump.SendGump();
            }

            if (!_fromGump)
            {
                Engine.MainWindow.SafeAction(s => s.ShowMe());
            }
        }

        private bool OnItemTargetAmountResponse(int gfx, string input)
        {
            if (ushort.TryParse(input, out ushort count))
            {
                if (count <= 0)
                {
                    return false;
                }

                AddItem(new BuyEntry((ushort) gfx, count));
                
                if (_fromGump)
                {
                    _fromGump = false;

                    AgentsGump agent = new AgentsGump(this);
                    agent.SendGump();
                }
                else
                {
                    Engine.MainWindow.SafeAction(s => s.ShowMe());
                }

                return true;
            }

            if (_fromGump)
            {
                _fromGump = false;

                AgentsGump agent = new AgentsGump(this);
                agent.SendGump();
            }
            else
            {
                Engine.MainWindow.SafeAction(s => s.ShowMe());
            }

            return false;
        }

        private bool OnItemChangeAmountResponse(int entryId, string input)
        {
            if (ushort.TryParse(input, out ushort amount))
            {
                if (amount <= 0)
                {
                    return false;
                }

                BuyEntry e = (BuyEntry)Items[entryId];

                e.Amount = amount;

                if (m_SubList != null)
                {
                    m_SubList.BeginUpdate();
                    m_SubList.Items.Clear();

                    for (int i = 0; i < Items.Count; i++)
                    {
                        m_SubList.Items.Add(Items[i]);
                    }

                    m_SubList.EndUpdate();
                }

                if (_fromGump)
                {
                    _fromGump = false;

                    AgentsGump agent = new AgentsGump(this);
                    agent.SendGump();
                }
                else
                {
                    Engine.MainWindow.SafeAction(s => s.ShowMe());
                }

                return true;
            }

            if (_fromGump)
            {
                _fromGump = false;

                AgentsGump agent = new AgentsGump(this);
                agent.SendGump();
            }
            else
            {
                Engine.MainWindow.SafeAction(s => s.ShowMe());
            }

            return false;
        }

        public void AddItemTarget(bool fromGump = false)
        {
            _fromGump = fromGump;

            World.Player.SendMessage(MsgLevel.Force, LocString.TargItemAdd);
            Targeting.OneTimeTarget(OnTarget);
        }

        public void AddItem(BuyEntry entry)
        {
            Items?.Add(entry);

            m_SubList?.Items.Add(entry);

            World.Player?.SendMessage(MsgLevel.Force, LocString.ItemAdded);
        }

        public void RemoveItem(int itemId)
        {
            BuyEntry item = Items.FirstOrDefault(a => a.Id == itemId);

            if (item != null)
            {
                Items.Remove(item);
                m_SubList?.Items?.Remove(item);

                World.Player?.SendMessage(MsgLevel.Force, LocString.ItemRemoved);
            }
        }

        public void SetItemAmount(int itemId, bool fromGump = false)
        {
            int itemIndex = Items.FindIndex(a => a.Id == itemId);
            BuyEntry item = Items[itemIndex];

            if (item != null)
            {
                _fromGump = fromGump;

                InputDialogGump inputGump = new InputDialogGump(OnItemChangeAmountResponse, itemIndex, Language.GetString(LocString.EnterAmount), item.Amount.ToString());
                inputGump.SendGump();
            }
        }

        public override void Save(XmlTextWriter xml)
        {
            if (Items == null)
            {
                return;
            }

            xml.WriteAttributeString("enabled", m_Enabled.ToString());
            xml.WriteAttributeString("alias", Alias);

            foreach (BuyEntry b in Items)
            {
                xml.WriteStartElement("item");
                xml.WriteAttributeString("id", b.Id.ToString());
                xml.WriteAttributeString("amount", b.Amount.ToString());
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
                Alias = node.GetAttribute("alias");
            }
            catch
            {
                Alias = string.Empty;
            }

            foreach (XmlElement el in node.GetElementsByTagName("item"))
            {
                try
                {
                    ushort id = Convert.ToUInt16(el.GetAttribute("id"));
                    ushort amount = Convert.ToUInt16(el.GetAttribute("amount"));

                    Items.Add(new BuyEntry(id, amount));
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
