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
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Assistant.Gumps;
using Assistant.Gumps.Internal;
using Assistant.Scripts;
using Assistant.UI;

namespace Assistant.Agents
{
    public class RestockAgent : Agent
    {
        public static List<RestockAgent> Agents { get; set; }
        
        public static void Initialize()
        {
            int maxAgents = Config.GetAppSetting<int>("MaxRestockAgents") == 0
                ? 20
                : Config.GetAppSetting<int>("MaxRestockAgents");

            Agents = new List<RestockAgent>();

            for (int i = 1; i <= maxAgents; i++)
            {
                RestockAgent restockAgent = new RestockAgent(i);

                Agent.Add(restockAgent);
                Agents.Add(restockAgent);
            }
        }

        private ListBox m_SubList;
        public readonly List<RestockItem> Items;
        private Button m_HotBTN;
        private Serial m_HotBag;

        private bool _fromGump { get; set; }

        public RestockAgent(int num)
        {
            Number = num;

            Items = new List<RestockItem>();

            HotKey.Add(HKCategory.Agents, HKSubCat.None,
                $"{Language.GetString(LocString.RestockAgent)}-{Number:D2}",
                new HotKeyCallback(OnHotKey));
            HotKey.Add(HKCategory.Agents, HKSubCat.None,
                $"{Language.GetString(LocString.SetRestockHB)}-{Number:D2}",
                new HotKeyCallback(SetHB));
            PacketHandler.RegisterClientToServerViewer(0x09, new PacketViewerCallback(OnSingleClick));

            Agent.OnItemCreated += new ItemCreatedEventHandler(CheckHBOPL);
        }

        public void CheckHBOPL(Item item)
        {
            if (item.Serial == m_HotBag)
            {
                item.ObjPropList.Add(Language.Format(LocString.RestockHBA1, Number));
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
                    Language.CliLocName, "", Language.Format(LocString.RestockHBA1, Number)));
            }
        }

        public override void Clear()
        {
            Items.Clear();
        }
        public override string Name
        {

            get { return $"{Language.GetString(LocString.Restock)}-{Number}"; }
        }

        public override string Alias { get; set; }

        public override int Number { get; }

        public override void OnSelected(ListBox subList, params Button[] buttons)
        {
            buttons[0].Text = Language.GetString(LocString.AddTarg);
            buttons[0].Visible = true;
            buttons[1].Text = Language.GetString(LocString.Remove);
            buttons[1].Visible = true;
            buttons[2].Text = Language.GetString(LocString.SetAmt);
            buttons[2].Visible = true;
            buttons[3].Text = Language.GetString(LocString.ClearList);
            buttons[3].Visible = true;
            m_HotBTN = buttons[4];
            SetHBText();
            buttons[4].Visible = true;
            buttons[5].Text = Language.GetString(LocString.RestockNow);
            buttons[5].Visible = true;

            m_SubList = subList;
            subList.BeginUpdate();
            subList.Items.Clear();
            for (int i = 0; i < Items.Count; i++)
            {
                subList.Items.Add(Items[i]);
            }

            subList.EndUpdate();

            if (!Client.Instance.AllowBit(FeatureBit.RestockAgent) && Engine.MainWindow != null)
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
                {
                    AddItemTarget();
                    break;
                }

                case 2:
                {
                    if (m_SubList.SelectedIndex >= 0 && m_SubList.SelectedIndex < Items.Count)
                    {
                        Items.RemoveAt(m_SubList.SelectedIndex);
                        m_SubList.Items.RemoveAt(m_SubList.SelectedIndex);
                    }

                    break;
                }

                case 3:
                {
                    int i = m_SubList.SelectedIndex;
                    if (i < 0 || i > Items.Count)
                    {
                        return;
                    }

                    RestockItem ri = (RestockItem) Items[i];
                    
                    InputDialogGump inputGump = new InputDialogGump(OnItemTargetChangeResponse, m_SubList.SelectedIndex, Language.GetString(LocString.EnterAmount), ri.Amount.ToString());
                    inputGump.SendGump();

                    break;
                }

                case 4:
                {
                    if (MessageBox.Show(Language.GetString(LocString.Confirm), Language.GetString(LocString.ClearList),
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        m_SubList.Items.Clear();
                        Items.Clear();
                    }

                    break;
                }

                case 5:
                {
                    if (m_HotBag == Serial.Zero)
                    {
                        SetHB();
                    }
                    else
                    {
                        m_HotBag = Serial.Zero;
                        SetHBText();
                    }

                    break;
                }

                case 6:
                {
                    OnHotKey();
                    break;
                }
            }
        }

        private void SetHBText()
        {
            if (m_HotBTN != null)
            {
                m_HotBTN.Text = Language.GetString(m_HotBag == Serial.Zero ? LocString.SetHB : LocString.ClearHB);
            }
        }

        public void SetHB()
        {
            World.Player.SendMessage(MsgLevel.Force, LocString.TargCont);
            Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnHBTarget));
        }

        private void OnHBTarget(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            if (Engine.MainWindow != null && !ScriptManager.Running)
            {
                Engine.MainWindow.SafeAction(s => s.ShowMe());
            }

            Item hb = World.FindItem(m_HotBag);
            if (hb != null)
            {
                if (hb.ObjPropList.Remove(Language.Format(LocString.RestockHBA1, Number)))
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
                hb.ObjPropList.Add(Language.Format(LocString.RestockHBA1, Number));
                hb.OPLChanged();
            }

            SetHBText();
        }

        public void OnHotKey()
        {
            if (Client.Instance.AllowBit(FeatureBit.RestockAgent))
            {
                World.Player.SendMessage(MsgLevel.Force, LocString.RestockTarget);
                Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnRestockTarget));
            }
        }

        Item m_Cont = null;

        private void OnRestockTarget(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            if (serial == World.Player.Serial)
            {
                m_Cont = World.Player.GetItemOnLayer(Layer.Bank);
            }
            else if (serial.IsItem)
            {
                m_Cont = World.FindItem(serial);
                if (m_Cont != null)
                {
                    object root = m_Cont.RootContainer;
                    if (root is Mobile && root != World.Player)
                    {
                        m_Cont = null;
                    }
                }
            }

            if (m_Cont == null || m_Cont.IsCorpse)
            {
                World.Player.SendMessage(MsgLevel.Force, LocString.InvalidCont);
                return;
            }

            if (Utility.Distance(World.Player.Position, m_Cont.GetWorldPosition()) > 3)
            {
                World.Player.SendMessage(MsgLevel.Error, LocString.TooFar);
            }
            else
            {
                if (m_Cont.IsContainer && m_Cont.Layer != Layer.Bank)
                {
                    PlayerData.DoubleClick(m_Cont);

                    if (Config.GetBool("ObjectDelayEnabled"))
                    {
                        Timer.DelayedCallback(TimeSpan.FromMilliseconds(Config.GetInt("ObjectDelay") + 200),
                            new TimerCallback(DoRestock)).Start();
                    }
                    else
                    {
                        Timer.DelayedCallback(TimeSpan.FromMilliseconds(200), new TimerCallback(DoRestock)).Start();
                    }


                    World.Player.SendMessage(LocString.RestockQueued);
                }
                else
                {
                    DoRestock();
                }
            }
        }

        private void DoRestock()
        {
            Item bag = null;
            if (m_HotBag != Serial.Zero)
            {
                bag = World.FindItem(m_HotBag);
                if (bag != null && bag.RootContainer != World.Player)
                {
                    bag = null;
                }
            }

            if (bag == null)
            {
                bag = World.Player.Backpack;
                if (bag == null)
                {
                    World.Player.SendMessage(MsgLevel.Force, LocString.NoBackpack);
                    return;
                }
            }

            int num = 0;
            for (int i = 0; i < Items.Count; i++)
            {
                RestockItem ri = Items[i];
                int count = World.Player.Backpack.GetCount(ri.ItemID);

                num += Recurse(bag, m_Cont.Contains, ri, ref count);
            }

            World.Player.SendMessage(MsgLevel.Force, LocString.RestockDone, num, num != 1 ? "s" : "");
        }

        private int Recurse(Item pack, List<Item> items, RestockItem ri, ref int count)
        {
            int num = 0;
            for (int i = 0; count < ri.Amount && i < items.Count; i++)
            {
                Item item = (Item) items[i];

                if (item.ItemID == ri.ItemID)
                {
                    int amt = ri.Amount - count;
                    if (amt > item.Amount)
                    {
                        amt = item.Amount;
                    }

                    DragDropManager.DragDrop(item, amt, pack);
                    count += amt;
                    num++;
                }
                else if (item.Contains.Count > 0)
                {
                    num += Recurse(pack, item.Contains, ri, ref count);
                }
            }

            return num;
        }

        private void OnItemTarget(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            if (location || serial.IsMobile)
            {
                return;
            }

            Item item = World.FindItem(serial);
            if (item != null)
            {
                gfx = item.ItemID;
            }

            if (gfx == 0 || gfx >= 0x4000)
            {
                return;
            }
            
            InputDialogGump inputGump = new InputDialogGump(OnItemTargetAmountResponse, gfx, Language.GetString(LocString.EnterAmount),"1");
            inputGump.SendGump();
        }

        private bool OnItemTargetAmountResponse(int gfx, string input)
        {
            if (int.TryParse(input, out int amount))
            {
                RestockItem ri = new RestockItem((ushort) gfx, amount);
                AddItem(ri);

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

        private bool OnItemTargetChangeResponse(int restockId, string input)
        {
            if (int.TryParse(input, out int amount))
            {
                RestockItem ri = (RestockItem)Items[restockId];
                
                ri.Amount = amount;

                if (m_SubList != null)
                {
                    m_SubList.BeginUpdate();
                    m_SubList.Items.Clear();

                    for (int j = 0; j < Items.Count; j++)
                    {
                        m_SubList.Items.Add(Items[j]);
                    }

                    m_SubList.SelectedIndex = restockId;
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

            Targeting.OneTimeTarget(OnItemTarget);
            World.Player.SendMessage(MsgLevel.Force, LocString.TargItemAdd);
        }

        public void AddItem(RestockItem item)
        {
            foreach (RestockItem restockItem in Items)
            {
                if (restockItem.ItemID.Value == item.ItemID.Value)
                {
                    World.Player?.SendMessage(MsgLevel.Force, LocString.ItemExists);
                    return;
                }
            }

            Items.Add(item);
            m_SubList?.Items?.Add(item);

            World.Player?.SendMessage(MsgLevel.Force, LocString.ItemAdded);
        }

        public void RemoveItem(int itemId)
        {
            RestockItem item = Items.FirstOrDefault(a => a.ItemID == itemId);

            if (item != null)
            {
                Items.Remove(item);
                m_SubList?.Items?.Remove(item);

                World.Player?.SendMessage(MsgLevel.Force, LocString.ItemRemoved);
            }
        }

        public void SetItemAmount(int itemId, bool fromGump = false)
        {
            int itemIndex = Items.FindIndex(a => a.ItemID == itemId);
            RestockItem item = Items[itemIndex];

            if (item != null)
            {
                _fromGump = fromGump;

                InputDialogGump inputGump = new InputDialogGump(OnItemTargetChangeResponse, itemIndex, Language.GetString(LocString.EnterAmount), item.Amount.ToString());
                inputGump.SendGump();
            }
        }

        public override void Save(XmlTextWriter xml)
        {
            xml.WriteAttributeString("hotbag", m_HotBag.Value.ToString());
            xml.WriteAttributeString("alias", Alias);

            for (int i = 0; i < Items.Count; i++)
            {
                xml.WriteStartElement("item");
                RestockItem ri = (RestockItem) Items[i];
                xml.WriteAttributeString("id", ri.ItemID.Value.ToString());
                xml.WriteAttributeString("amount", ri.Amount.ToString());
                xml.WriteEndElement();
            }
        }

        public override void Load(XmlElement node)
        {
            try
            {
                m_HotBag = Convert.ToUInt32(node.GetAttribute("hotbag"));
            }
            catch
            {
                m_HotBag = Serial.Zero;
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
                    string iid = el.GetAttribute("id");
                    string amt = el.GetAttribute("amount");
                    Items.Add(new RestockItem((ItemID) Convert.ToInt32(iid), Convert.ToInt32(amt)));
                }
                catch
                {
                    // ignored
                }
            }
        }

        public class RestockItem
        {
            public ItemID ItemID;
            public int Amount;

            public RestockItem(ItemID id, int amount)
            {
                ItemID = id;
                Amount = amount;
            }

            public override string ToString()
            {
                return $"{ItemID}\t\t{Amount}";
            }
        }
    }
}