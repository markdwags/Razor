using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Assistant
{
    [Flags]
    public enum Direction : byte
    {
        North = 0x0,
        Right = 0x1,
        East = 0x2,
        Down = 0x3,
        South = 0x4,
        Left = 0x5,
        West = 0x6,
        Up = 0x7,
        Mask = 0x7,
        Running = 0x80,
        ValueMask = 0x87
    }

    //public enum BodyType : byte
    //{
    //    Empty,
    //    Monster,
    //    Sea_Monster,
    //    Animal,
    //    Human,
    //    Equipment
    //}

    public class Mobile : UOEntity
    {
        private ushort m_Body;
        private Direction m_Direction;
        private string m_Name;

        private byte m_Notoriety;

        private bool m_Visible;
        private bool m_Female;
        private bool m_Poisoned;
        private bool m_Blessed;
        private bool m_Warmode;

        //new
        private bool m_Unknown;
        private bool m_Unknown2;
        private bool m_Unknown3;

        private bool m_CanRename;
        //end new

        private ushort m_HitsMax, m_Hits;
        protected ushort m_StamMax, m_Stam, m_ManaMax, m_Mana;

        private List<Serial> m_LoadSerials;
        private List<Item> m_Items = new List<Item>();

        private byte m_Map;

        //private static BodyType[] m_Types;

        //public static void Initialize()
        //{
        //    using (StreamReader ip = new StreamReader(Path.Combine(Ultima.Files.RootDir, "mobtypes.txt")))
        //    {
        //        m_Types = new BodyType[0x1000];

        //        string line;

        //        while ((line = ip.ReadLine()) != null)
        //        {
        //            if (line.Length == 0 || line.StartsWith("#"))
        //                continue;

        //            string[] split = line.Split('\t');

        //            BodyType type;
        //            int bodyID;

        //            if (int.TryParse(split[0], out bodyID) && Enum.TryParse(split[1], true, out type) && bodyID >= 0 &&
        //                bodyID < m_Types.Length)
        //            {
        //                m_Types[bodyID] = type;
        //            }
        //        }
        //    }
        //}

        public override void SaveState(BinaryWriter writer)
        {
            base.SaveState(writer);

            writer.Write(m_Body);
            writer.Write((byte)m_Direction);
            writer.Write(m_Name == null ? "" : m_Name);
            writer.Write(m_Notoriety);
            writer.Write((byte)GetPacketFlags());
            writer.Write(m_HitsMax);
            writer.Write(m_Hits);
            writer.Write(m_Map);

            writer.Write((int)m_Items.Count);
            for (int i = 0; i < m_Items.Count; i++)
                writer.Write((uint)(((Item)m_Items[i]).Serial));
            //writer.Write(	(int)0 );
        }

        public Mobile(BinaryReader reader, int version) : base(reader, version)
        {
            m_Body = reader.ReadUInt16();
            m_Direction = (Direction)reader.ReadByte();
            m_Name = reader.ReadString();
            m_Notoriety = reader.ReadByte();
            ProcessPacketFlags(reader.ReadByte());
            m_HitsMax = reader.ReadUInt16();
            m_Hits = reader.ReadUInt16();
            m_Map = reader.ReadByte();

            int count = reader.ReadInt32();
            m_LoadSerials = new List<Serial>();

            for (int i = count - 1; i >= 0; --i)
                m_LoadSerials.Add(reader.ReadUInt32());
        }

        public override void AfterLoad()
        {
            int count = m_LoadSerials.Count;

            for (int i = count - 1; i >= 0; --i)
            {
                Item it = World.FindItem(m_LoadSerials[i]);
                if (it != null)
                    m_Items.Add(it);
            }
            m_LoadSerials = null;//per il GC e per liberare RAM
        }

        public Mobile(Serial serial) : base(serial)
        {
            m_Map = World.Player == null ? (byte)0 : World.Player.Map;
            m_Visible = true;

            Agent.InvokeMobileCreated(this);
        }

        public string Name
        {
            get
            {
                if (m_Name == null)
                    return "";
                else
                    return m_Name;
            }
            set
            {
                if (value != null)
                {
                    string trim = value.Trim();
                    if (trim.Length > 0)
                        m_Name = trim;
                }
            }
        }

        public ushort Body
        {
            get { return m_Body; }
            set { m_Body = value; }
        }

        public Direction Direction
        {
            get { return m_Direction; }
            set { m_Direction = value; }
        }

        public bool Visible
        {
            get { return m_Visible; }
            set { m_Visible = value; }
        }

        public bool Poisoned
        {
            get { return m_Poisoned; }
            set { m_Poisoned = value; }
        }

        public bool Blessed
        {
            get { return m_Blessed; }
            set { m_Blessed = value; }
        }

        public bool IsGhost
        {
            get
            {
                return m_Body == 402
                     || m_Body == 403
                     || m_Body == 607
                     || m_Body == 608
                     || m_Body == 970;
            }
        }

        public bool IsHuman
        {
            get
            {
                return m_Body >= 0
                    && (m_Body == 400
                    || m_Body == 401
                    || m_Body == 402
                    || m_Body == 403
                    || m_Body == 605
                    || m_Body == 606
                    || m_Body == 607
                    || m_Body == 608
                    || m_Body == 970); //player ghost
            }
        }

        public bool IsMonster
        {
            get
            {
                return !IsHuman;
            }
        }
        
        //new
        public bool Unknown
        {
            get { return m_Unknown; }
            set { m_Unknown = value; }
        }
        public bool Unknown2
        {
            get { return m_Unknown2; }
            set { m_Unknown2 = value; }
        }
        public bool Unknown3
        {
            get { return m_Unknown3; }
            set { m_Unknown3 = value; }
        }
        public bool CanRename       //A pet! (where the health bar is open, we can add this to an arraylist of mobiles...
        {
            get { return m_CanRename; }
            set { m_CanRename = value; }
        }
        //end new

        public bool Warmode
        {
            get { return m_Warmode; }
            set { m_Warmode = value; }
        }

        public bool Female
        {
            get { return m_Female; }
            set { m_Female = value; }
        }

        public byte Notoriety
        {
            get { return m_Notoriety; }
            set
            {
                if (value != Notoriety)
                {
                    OnNotoChange(m_Notoriety, value);
                    m_Notoriety = value;
                }
            }
        }

        protected virtual void OnNotoChange(byte old, byte cur)
        {
        }

        // grey, blue, green, 'canbeattacked'
        private static uint[] m_NotoHues = new uint[8]
        { 
			// hue color #30
			0x000000, // black		unused 0
			0x30d0e0, // blue		0x0059 1 
			0x60e000, // green		0x003F 2
			0x9090b2, // greyish	0x03b2 3
			0x909090, // grey		   "   4
			0xd88038, // orange		0x0090 5
			0xb01000, // red		0x0022 6
			0xe0e000 // yellow		0x0035 7
        };

        private static int[] m_NotoHuesInt = new int[8]
          {
               1, // black		unused 0
			0x059, // blue		0x0059 1
			0x03F, // green		0x003F 2
			0x3B2, // greyish	0x03b2 3
			0x3B2, // grey		   "   4
			0x090, // orange		0x0090 5
			0x022, // red		0x0022 6
			0x035, // yellow		0x0035 7
		};

        public uint GetNotorietyColor()
        {
            if (m_Notoriety < 0 || m_Notoriety >= m_NotoHues.Length)
                return m_NotoHues[0];
            else
                return m_NotoHues[m_Notoriety];
        }

        public int GetNotorietyColorInt()
        {
            if (m_Notoriety < 0 || m_Notoriety >= m_NotoHues.Length)
                return m_NotoHuesInt[0];
            else
                return m_NotoHuesInt[m_Notoriety];
        }

        public byte GetStatusCode()
        {
            if (m_Poisoned)
                return 1;
            else
                return 0;
        }

        public ushort HitsMax
        {
            get { return m_HitsMax; }
            set { m_HitsMax = value; }
        }

        public ushort Hits
        {
            get { return m_Hits; }
            set { m_Hits = value; }
        }

        public ushort Stam
        {
            get { return m_Stam; }
            set { m_Stam = value; }
        }

        public ushort StamMax
        {
            get { return m_StamMax; }
            set { m_StamMax = value; }
        }

        public ushort Mana
        {
            get { return m_Mana; }
            set { m_Mana = value; }
        }

        public ushort ManaMax
        {
            get { return m_ManaMax; }
            set { m_ManaMax = value; }
        }


        public byte Map
        {
            get { return m_Map; }
            set
            {
                if (m_Map != value)
                {
                    OnMapChange(m_Map, value);
                    m_Map = value;
                }
            }
        }

        public virtual void OnMapChange(byte old, byte cur)
        {
        }

        public void AddItem(Item item)
        {
            m_Items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            m_Items.Remove(item);
        }

        public override void Remove()
        {
            List<Item> rem = new List<Item>(m_Items);
            m_Items.Clear();

            for (int i = 0; i < rem.Count; i++)
                rem[i].Remove();

            if (!InParty)
            {
                base.Remove();
                World.RemoveMobile(this);
            }
            else
            {
                Visible = false;
            }
        }

        public bool InParty
        {
            get
            {
                return PacketHandlers.Party.Contains(this.Serial);
            }
        }

        public Item GetItemOnLayer(Layer layer)
        {
            for (int i = 0; i < m_Items.Count; i++)
            {
                Item item = (Item)m_Items[i];
                if (item.Layer == layer)
                    return item;
            }
            return null;
        }

        public Item Backpack
        {
            get
            {
                return GetItemOnLayer(Layer.Backpack);
            }
        }

        public Item Quiver
        {
            get
            {
                Item item = GetItemOnLayer(Layer.Cloak);

                if (item != null && item.IsContainer)
                    return item;
                else
                    return null;
            }
        }

        public Item FindItemByID(ItemID id)
        {
            for (int i = 0; i < Contains.Count; i++)
            {
                Item item = (Item)Contains[i];
                if (item.ItemID == id)
                    return item;
            }
            return null;
        }

        public override void OnPositionChanging(Point3D newPos)
        {
            if (this != World.Player && Engine.MainWindow.MapWindow != null)
                Engine.MainWindow.MapWindow.CheckLocalUpdate(this);

            base.OnPositionChanging(newPos);
        }

        public int GetPacketFlags()
        {
            int flags = 0x0;

            if (m_Female)
                flags |= 0x02;

            if (m_Poisoned)
                flags |= 0x04;

            if (m_Blessed)
                flags |= 0x08;

            if (m_Warmode)
                flags |= 0x40;

            if (!m_Visible)
                flags |= 0x80;

            if (m_Unknown)
                flags |= 0x01;

            if (m_Unknown2)
                flags |= 0x10;

            if (m_Unknown3)
                flags |= 0x20;

            return flags;
        }

        public void ProcessPacketFlags(byte flags)
        {
            if (!PacketHandlers.UseNewStatus)
                m_Poisoned = (flags & 0x04) != 0;

            m_Unknown = (flags & 0x01) != 0;  //new
            m_Female = (flags & 0x02) != 0;
            m_Blessed = (flags & 0x08) != 0;
            m_Unknown2 = (flags & 0x10) != 0;  //new
            m_Unknown3 = (flags & 0x10) != 0;  //new
            m_Warmode = (flags & 0x40) != 0;
            m_Visible = (flags & 0x80) == 0;


        }

        public List<Item> Contains { get { return m_Items; } }

        internal void OverheadMessageFrom(int hue, string from, string format, params object[] args)
        {
            OverheadMessageFrom(hue, from, String.Format(format, args));
        }

        internal void OverheadMessageFrom(int hue, string from, string text)
        {
            if (Config.GetInt("OverheadStyle") == 0)
            {
                ClientCommunication.SendToClient(new AsciiMessage(Serial, m_Body, MessageType.Regular, hue, 3, Language.CliLocName, text));
            }
            else
            {
                ClientCommunication.SendToClient(new UnicodeMessage(Serial, m_Body, MessageType.Regular, hue, 3, Language.CliLocName, from, text));
            }
        }

        internal void OverheadMessage(string text)
        {
            OverheadMessage(Config.GetInt("SysColor"), text);
        }

        internal void OverheadMessage(string format, params object[] args)
        {
            OverheadMessage(Config.GetInt("SysColor"), String.Format(format, args));
        }

        internal void OverheadMessage(int hue, string format, params object[] args)
        {
            OverheadMessage(hue, String.Format(format, args));
        }

        internal void OverheadMessage(int hue, string text)
        {
            OverheadMessageFrom(hue, "Razor", text);
        }

        internal void OverheadMessage(LocString str)
        {
            OverheadMessage(Language.GetString(str));
        }

        internal void OverheadMessage(LocString str, params object[] args)
        {
            OverheadMessage(Language.Format(str, args));
        }

        private Point2D m_ButtonPoint = Point2D.Zero;
        internal Point2D ButtonPoint
        {
            get { return m_ButtonPoint; }
            set { m_ButtonPoint = value; }
        }
    }
}


