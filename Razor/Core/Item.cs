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
using System.IO;
using System.Collections.Generic;
using Assistant.Agents;
using Assistant.Core;

namespace Assistant
{
    public enum Layer : byte
    {
        Invalid = 0x00,

        FirstValid = 0x01,

        RightHand = 0x01,
        LeftHand = 0x02,
        Shoes = 0x03,
        Pants = 0x04,
        Shirt = 0x05,
        Head = 0x06,
        Gloves = 0x07,
        Ring = 0x08,
        Talisman = 0x09,
        Neck = 0x0A,
        Hair = 0x0B,
        Waist = 0x0C,
        InnerTorso = 0x0D,
        Bracelet = 0x0E,
        Face = 0x0F,
        FacialHair = 0x10,
        MiddleTorso = 0x11,
        Earrings = 0x12,
        Arms = 0x13,
        Cloak = 0x14,
        Backpack = 0x15,
        OuterTorso = 0x16,
        OuterLegs = 0x17,
        InnerLegs = 0x18,

        LastUserValid = 0x18,

        Mount = 0x19,
        ShopBuy = 0x1A,
        ShopResale = 0x1B,
        ShopSell = 0x1C,
        Bank = 0x1D,

        LastValid = 0x1D
    }

    public class Item : UOEntity
    {
        private ItemID _itemId;
        private ushort _amount;
        private byte _direction;

        private bool _visible;
        private bool _movable;

        private Layer _layer;
        private string _name;
        private object _parent;
        private int _price;
        private string _buyDesc;
        private List<Item> _items;

        private bool _isNew;
        private bool _autoStack;

        private byte[] _housePacket;
        private int _houseRev;

        private byte _gridNum;
        
        public override void AfterLoad()
        {
            _items = new List<Item>();

            for (int i = 0; i < Serial.Serials.Count; i++)
            {
                Serial s = Serial.Serials[i];
                if (s.IsItem)
                {
                    Item item = World.FindItem(s);

                    if (item != null)
                    {
                        _items[i] = item;
                    }

                    Serial.Serials.RemoveAt(i);
                    i--;
                }
            }

            UpdateContainer();
        }

        public Item(Serial serial) : base(serial)
        {
            _items = new List<Item>();

            _visible = true;
            _movable = true;

            Agent.InvokeItemCreated(this);
        }

        public ItemID ItemID
        {
            get { return _itemId; }
            set { _itemId = value; }
        }

        public ushort Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public byte Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public bool Movable
        {
            get { return _movable; }
            set { _movable = value; }
        }

        public string Name
        {
            get
            {
                if (!string.IsNullOrEmpty(_name))
                {
                    return _name;
                }
                else
                {
                    return _itemId.ToString();
                }
            }
            set
            {
                if (value != null)
                    _name = value.Trim();
                else
                    _name = null;
            }
        }

        public string DisplayName => _itemId.Value < Ultima.TileData.ItemTable.Length ? Ultima.TileData.ItemTable[_itemId.Value].Name : string.Empty;

        public Layer Layer
        {
            get
            {
                if ((_layer < Layer.FirstValid || _layer > Layer.LastValid) &&
                    ((this.ItemID.ItemData.Flags & Ultima.TileFlag.Wearable) != 0 ||
                     (this.ItemID.ItemData.Flags & Ultima.TileFlag.Armor) != 0 ||
                     (this.ItemID.ItemData.Flags & Ultima.TileFlag.Weapon) != 0
                    ))
                {
                    _layer = (Layer) this.ItemID.ItemData.Quality;
                }

                return _layer;
            }
            set { _layer = value; }
        }

        public Item FindItemById(ItemID id)
        {
            return FindItemById(id, true);
        }

        public List<Item> FindItemsById(ItemID id)
        {
            return FindItemsById(id, true);
        }

        public Item FindItemById(ItemID id, bool recurse)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                Item item = (Item) _items[i];
                if (item.ItemID == id)
                {
                    return item;
                }

                if (recurse)
                {
                    item = item.FindItemById(id, true);
                    if (item != null)
                        return item;
                }
            }

            return null;
        }

        public List<Item> FindItemsById(ItemID id, bool recurse)
        {
            List<Item> items = new List<Item>();

            for (int i = 0; i < _items.Count; i++)
            {
                Item item = _items[i];
                if (item.ItemID == id)
                {
                    items.Add(item);
                }
                
                if (recurse)
                {
                    List<Item> recurseItems = item.FindItemsById(id, true);

                    if (recurseItems.Count > 0)
                    {
                        items.AddRange(recurseItems);
                    }
                }
            }

            return items;
        }

        public Item FindItemByName(string name, bool recurse)
        {
            foreach (var i in _items)
            {
                Item item = i;
                
                if (item.ItemID.ItemData.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }

                if (recurse)
                {
                    item = item.FindItemByName(name, true);
                    if (item != null)
                        return item;
                }
            }

            return null;
        }

        public List<Item> FindItemsByName(string name, bool recurse)
        {
            List<Item> items = new List<Item>();

            foreach (Item i in _items)
            {
                if (i.ItemID.ItemData.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    items.Add(i);
                }
                
                if (recurse)
                {
                    List<Item> recurseItems = i.FindItemsByName(name, true);
                    if (recurseItems.Count > 0)
                    {
                        items.AddRange(recurseItems);
                    }
                }
            }

            return items;
        }

        public int GetCount(ushort iid)
        {
            int count = 0;
            for (int i = 0; i < _items.Count; i++)
            {
                Item item = (Item) _items[i];
                if (item.ItemID == iid)
                    count += item.Amount;
                // fucking osi blank scrolls
                else if ((item.ItemID == 0x0E34 && iid == 0x0EF3) || (item.ItemID == 0x0EF3 && iid == 0x0E34))
                    count += item.Amount;
                count += item.GetCount(iid);
            }

            return count;
        }
        
        public int GetTotalCount()
        {
            int count = 0;
            
            foreach (Item item in _items)
            {
                count++;

                if (item.IsContainer)
                {
                    count += item.GetTotalCount();
                }
            }

            return count;
        }

        public object Container
        {
            get
            {
                if (_parent is Serial && UpdateContainer())
                    m_NeedContUpdate.Remove(this);
                return _parent;
            }
            set
            {
                if ((_parent != null && _parent.Equals(value))
                    || (value is Serial && _parent is UOEntity && ((UOEntity) _parent).Serial == (Serial) value)
                    || (_parent is Serial && value is UOEntity && ((UOEntity) value).Serial == (Serial) _parent))
                {
                    return;
                }

                if (_parent is Mobile)
                    ((Mobile) _parent).RemoveItem(this);
                else if (_parent is Item)
                    ((Item) _parent).RemoveItem(this);

                if (World.Player != null && (IsChildOf(World.Player.Backpack) || IsChildOf(World.Player.Quiver)))
                    Counter.Uncount(this);

                if (value is Mobile)
                    _parent = ((Mobile) value).Serial;
                else if (value is Item)
                    _parent = ((Item) value).Serial;
                else
                    _parent = value;

                if (!UpdateContainer() && m_NeedContUpdate != null)
                    m_NeedContUpdate.Add(this);
            }
        }

        public bool UpdateContainer()
        {
            if (!(_parent is Serial) || Deleted)
                return true;

            object o = null;
            Serial contSer = (Serial) _parent;
            if (contSer.IsItem)
                o = World.FindItem(contSer);
            else if (contSer.IsMobile)
                o = World.FindMobile(contSer);

            if (o == null)
                return false;

            _parent = o;

            if (_parent is Item)
                ((Item) _parent).AddItem(this);
            else if (_parent is Mobile)
                ((Mobile) _parent).AddItem(this);

            if (World.Player != null && (IsChildOf(World.Player.Backpack) || IsChildOf(World.Player.Quiver)))
            {
                bool exempt = SearchExemptionAgent.IsExempt(this);
                if (!exempt)
                    Counter.Count(this);

                if (_isNew)
                {
                    if (_autoStack)
                        AutoStackResource();

                    if (IsContainer && !exempt && (!IsPouch || !Config.GetBool("NoSearchPouches")) &&
                        Config.GetBool("AutoSearch"))
                    {
                        PacketHandlers.IgnoreGumps.Add(this);
                        PlayerData.DoubleClick(this);

                        for (int c = 0; c < Contains.Count; c++)
                        {
                            Item icheck = (Item) Contains[c];
                            if (icheck.IsContainer && !SearchExemptionAgent.IsExempt(icheck) &&
                                (!icheck.IsPouch || !Config.GetBool("NoSearchPouches")))
                            {
                                PacketHandlers.IgnoreGumps.Add(icheck);
                                PlayerData.DoubleClick(icheck);
                            }
                        }
                    }
                }
            }

            _autoStack = _isNew = false;

            return true;
        }

        private static List<Item> m_NeedContUpdate = new List<Item>();

        public static void UpdateContainers()
        {
            int i = 0;
            while (i < m_NeedContUpdate.Count)
            {
                if (((Item) m_NeedContUpdate[i]).UpdateContainer())
                    m_NeedContUpdate.RemoveAt(i);
                else
                    i++;
            }
        }

        private static List<Serial> m_AutoStackCache = new List<Serial>();

        public void AutoStackResource()
        {
            if (!IsResource || !Config.GetBool("AutoStack") || m_AutoStackCache.Contains(Serial))
                return;

            foreach (Item check in World.Items.Values)
            {
                if (check.Container == null && check.ItemID == ItemID && check.Hue == Hue &&
                    Utility.InRange(World.Player.Position, check.Position, 2))
                {
                    DragDropManager.DragDrop(this, check);
                    m_AutoStackCache.Add(Serial);
                    return;
                }
            }

            DragDropManager.DragDrop(this, World.Player.Position);
            m_AutoStackCache.Add(Serial);
        }

        public object RootContainer
        {
            get
            {
                int die = 100;
                object cont = this.Container;
                while (cont != null && cont is Item && die-- > 0)
                    cont = ((Item) cont).Container;

                return cont;
            }
        }

        public bool IsChildOf(object parent)
        {
            Serial parentSerial = 0;
            if (parent is Mobile)
                return parent == RootContainer;
            else if (parent is Item)
                parentSerial = ((Item) parent).Serial;
            else
                return false;

            object check = this;
            int die = 100;
            while (check != null && check is Item && die-- > 0)
            {
                if (((Item) check).Serial == parentSerial)
                    return true;
                else
                    check = ((Item) check).Container;
            }

            return false;
        }

        public Point3D GetWorldPosition()
        {
            int die = 100;
            object root = this.Container;
            while (root != null && root is Item && ((Item) root).Container != null && die-- > 0)
                root = ((Item) root).Container;

            if (root is Item)
                return ((Item) root).Position;
            else if (root is Mobile)
                return ((Mobile) root).Position;
            else
                return this.Position;
        }

        private void AddItem(Item item)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i] == item)
                    return;
            }

            _items.Add(item);
        }

        private void RemoveItem(Item item)
        {
            _items.Remove(item);
        }

        public byte GetPacketFlags()
        {
            byte flags = 0;

            if (!_visible)
            {
                flags |= 0x80;
            }

            if (_movable)
            {
                flags |= 0x20;
            }

            return flags;
        }

        public int DistanceTo(Mobile m)
        {
            int x = Math.Abs(this.Position.X - m.Position.X);
            int y = Math.Abs(this.Position.Y - m.Position.Y);

            return x > y ? x : y;
        }

        public void ProcessPacketFlags(byte flags)
        {
            _visible = ((flags & 0x80) == 0);
            _movable = ((flags & 0x20) != 0);
        }

        private Timer m_RemoveTimer = null;

        public void RemoveRequest()
        {
            if (m_RemoveTimer == null)
                m_RemoveTimer = Timer.DelayedCallback(TimeSpan.FromSeconds(0.25), new TimerCallback(Remove));
            else if (m_RemoveTimer.Running)
                m_RemoveTimer.Stop();

            m_RemoveTimer.Start();
        }

        public bool CancelRemove()
        {
            if (m_RemoveTimer != null && m_RemoveTimer.Running)
            {
                m_RemoveTimer.Stop();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Remove()
        {
            /*if ( IsMulti )
                 UOAssist.PostRemoveMulti( this );*/

            List<Item> rem = new List<Item>(_items);
            _items.Clear();
            for (int i = 0; i < rem.Count; i++)
                (rem[i]).Remove();

            Counter.Uncount(this);

            if (_parent is Mobile)
                ((Mobile) _parent).RemoveItem(this);
            else if (_parent is Item)
                ((Item) _parent).RemoveItem(this);

            World.RemoveItem(this);
            base.Remove();
        }

        public List<Item> Contains
        {
            get { return _items; }
        }

        // possibly 4 bit x/y - 16x16?
        public byte GridNum
        {
            get { return _gridNum; }
            set { _gridNum = value; }
        }

        public bool OnGround
        {
            get { return Container == null; }
        }

        public bool IsContainer
        {
            get
            {
                ushort iid = _itemId.Value;
                return (_items.Count > 0 && !IsCorpse) || (iid >= 0x9A8 && iid <= 0x9AC) ||
                       (iid >= 0x9B0 && iid <= 0x9B2) ||
                       (iid >= 0xA2C && iid <= 0xA53) || (iid >= 0xA97 && iid <= 0xA9E) ||
                       (iid >= 0xE3C && iid <= 0xE43) ||
                       (iid >= 0xE75 && iid <= 0xE80 && iid != 0xE7B) || iid == 0x1E80 || iid == 0x1E81 ||
                       iid == 0x232A || iid == 0x232B ||
                       iid == 0x2B02 || iid == 0x2B03 || iid == 0x2FB7 || iid == 0x3171;
            }
        }

        public bool IsBagOfSending
        {
            get { return Hue >= 0x0400 && _itemId.Value == 0xE76; }
        }

        public bool IsInBank
        {
            get
            {
                if (_parent is Item)
                    return ((Item) _parent).IsInBank;
                else if (_parent is Mobile)
                    return this.Layer == Layer.Bank;
                else
                    return false;
            }
        }

        public bool IsNew
        {
            get { return _isNew; }
            set { _isNew = value; }
        }

        public bool AutoStack
        {
            get { return _autoStack; }
            set { _autoStack = value; }
        }

        public bool IsMulti
        {
            get { return _itemId.Value >= 0x4000; }
        }

        public bool IsPouch
        {
            get { return _itemId.Value == 0x0E79; }
        }

        public bool IsCorpse
        {
            get { return _itemId.Value == 0x2006 || (_itemId.Value >= 0x0ECA && _itemId.Value <= 0x0ED2); }
        }

        public bool IsDoor
        {
            get
            {
                ushort iid = _itemId.Value;
                return (iid >= 0x0675 && iid <= 0x06F6) || (iid >= 0x0821 && iid <= 0x0875) ||
                       (iid >= 0x1FED && iid <= 0x1FFC) ||
                       (iid >= 0x241F && iid <= 0x2424) || (iid >= 0x2A05 && iid <= 0x2A1C);
            }
        }

        public bool IsResource
        {
            get
            {
                ushort iid = _itemId.Value;
                return (iid >= 0x19B7 && iid <= 0x19BA) || // ore
                       (iid >= 0x09CC && iid <= 0x09CF) || // fishes
                       (iid >= 0x1BDD && iid <= 0x1BE2) || // logs
                       iid == 0x1779 || // granite / stone
                       iid == 0x11EA || iid == 0x11EB // sand
                    ;
            }
        }

        public bool IsPotion
        {
            get
            {
                return (_itemId.Value >= 0x0F06 && _itemId.Value <= 0x0F0D) ||
                       _itemId.Value == 0x2790 || _itemId.Value == 0x27DB; // Ninja belt (works like a potion)
            }
        }

        public bool IsVirtueShield
        {
            get
            {
                ushort iid = _itemId.Value;
                return (iid >= 0x1bc3 && iid <= 0x1bc5); // virtue shields
            }
        }

        public bool IsTwoHanded
        {
            get
            {
                ushort iid = _itemId.Value;

                if (Overrides.TwoHanded.TryGetValue(iid, out bool value))
                {
                    return value;
                }

                return (
                           // everything in layer 2 except shields is 2handed
                           Layer == Layer.LeftHand &&
                           !((iid >= 0x1b72 && iid <= 0x1b7b) || IsVirtueShield) // shields
                       ) ||

                       // and all of these layer 1 weapons:
                       (iid == 0x13fc || iid == 0x13fd) || // hxbow
                       (iid == 0x13AF || iid == 0x13b2) || // war axe & bow
                       (iid >= 0x0F43 && iid <= 0x0F50) || // axes & xbow
                       (iid == 0x1438 || iid == 0x1439) || // war hammer
                       (iid == 0x1442 || iid == 0x1443) || // 2handed axe
                       (iid == 0x1402 || iid == 0x1403) || // short spear
                       (iid == 0x26c1 || iid == 0x26cb) || // aos blade
                       (iid == 0x26c2 || iid == 0x26cc) || // aos bow
                       (iid == 0x26c3 || iid == 0x26cd) // aos xbow
                    ;
            }
        }

        public override string ToString()
        {
            return $"{this.Name} ({this.Serial})";
        }

        public int Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public string BuyDesc
        {
            get { return _buyDesc; }
            set { _buyDesc = value; }
        }

        public int HouseRevision
        {
            get { return _houseRev; }
            set { _houseRev = value; }
        }

        public byte[] HousePacket
        {
            get { return _housePacket; }
            set { _housePacket = value; }
        }

        public void MakeHousePacket()
        {
            _housePacket = null;

            try
            {
                // 3 locations... which is right? all of them? wtf?
                //"Desktop/{0}/{1}/{2}/Multicache.dat", World.AccountName, World.ShardName, World.OrigPlayerName
                //"Desktop/{0}/{1}/{2}/Multicache.dat", World.AccountName, World.ShardName, World.Player.Name );
                //"Desktop/{0}/Multicache.dat", World.AccountName );
                string path = Ultima.Files.GetFilePath(
                    $"Desktop/{World.AccountName}/{World.ShardName}/{World.OrigPlayerName}/Multicache.dat");
                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                    return;

                using (StreamReader reader =
                    new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                {
                    string line;
                    reader.ReadLine(); // ver
                    int skip = 0;
                    int count = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (count++ < skip || line == "" || line[0] == ';')
                            continue;

                        string[] split = line.Split(' ', '\t');
                        if (split.Length <= 0)
                            return;

                        skip = 0;
                        Serial ser = (uint) Utility.ToInt32(split[0], 0);
                        int rev = Utility.ToInt32(split[1], 0);
                        int lines = Utility.ToInt32(split[2], 0);

                        if (ser == this.Serial)
                        {
                            _houseRev = rev;
                            MultiTileEntry[] tiles = new MultiTileEntry[lines];
                            count = 0;

                            Ultima.MultiComponentList mcl = Ultima.Multis.GetComponents(_itemId);

                            while ((line = reader.ReadLine()) != null && count < lines)
                            {
                                split = line.Split(' ', '\t');

                                tiles[count] = new MultiTileEntry();
                                tiles[count].m_ItemID = (ushort) Utility.ToInt32(split[0], 0);
                                tiles[count].m_OffsetX = (short) (Utility.ToInt32(split[1], 0) + mcl.Center.X);
                                tiles[count].m_OffsetX = (short) (Utility.ToInt32(split[2], 0) + mcl.Center.Y);
                                tiles[count].m_OffsetX = (short) Utility.ToInt32(split[3], 0);

                                count++;
                            }

                            _housePacket = new DesignStateDetailed(Serial, _houseRev, mcl.Min.X, mcl.Min.Y, mcl.Max.X,
                                mcl.Max.Y, tiles).Compile();
                            break;
                        }
                        else
                        {
                            skip = lines;
                        }

                        count = 0;
                    }
                }
            }
            catch // ( Exception e )
            {
                //Engine.LogCrash( e );
            }
        }
    }
}