using System;
using System.IO;
using System.Collections;

namespace Assistant
{
	public class Item : UOEntity
	{
		private ItemID m_ItemID;
		private ushort m_Amount;
		private byte m_Direction;

		private bool m_Visible;
		private bool m_Movable;
		
		private byte m_Layer;
		private string m_Name;
		private object m_Parent;
		private int m_Price;
		private string m_BuyDesc;
		private ArrayList m_Items;

		private bool m_IsNew;
		private bool m_AutoStack;

		private byte[] m_HousePacket;
		private int m_HouseRev;

		public override void SaveState( BinaryWriter writer )
		{
			base.SaveState (writer);

			writer.Write( (ushort)m_ItemID );
			writer.Write( m_Amount );
			writer.Write( m_Direction );
			writer.Write( (byte)GetPacketFlags() );
			writer.Write( (byte)m_Layer );
			writer.Write( m_Name == null ? "" : m_Name );
			if ( m_Parent is UOEntity )
				writer.Write( (uint)((UOEntity)m_Parent).Serial );
			else if ( m_Parent is Serial )
				writer.Write( (uint)((Serial)m_Parent) );
			else
				writer.Write( (uint) 0 );

			//writer.Write( m_Items.Count );
			//for(int i=0;i<m_Items.Count;i++)
			//	writer.Write( (uint)((Item)m_Items[i]).Serial );
			writer.Write( (int) 0 );

			//if ( m_HouseRev != 0 && m_HousePacket == null )
			//	MakeHousePacket();

			if ( m_HouseRev != 0 && m_HousePacket != null )
			{
				writer.Write( (int)m_HouseRev );
					
				writer.Write( (ushort)m_HousePacket.Length );
				writer.Write( m_HousePacket );
			}
			else
			{
				writer.Write( (int) 0 );
			}
		}

		public Item( BinaryReader reader, byte version ) : base( reader, version )
		{
			m_ItemID = reader.ReadUInt16();
			m_Amount = reader.ReadUInt16();
			m_Direction = reader.ReadByte();
			ProcessPacketFlags( reader.ReadByte() );
			m_Layer = (byte)reader.ReadByte();
			m_Name = reader.ReadString();
			m_Parent = (Serial)reader.ReadUInt32();
			if ( ((Serial)m_Parent) == Serial.Zero )
				m_Parent = null;

			int count = reader.ReadInt32();
			m_Items = new ArrayList( count );
			for(int i=0;i<count;i++)
				m_Items.Add( (Serial)reader.ReadUInt32() );

			if ( version > 2 )
			{
				m_HouseRev = reader.ReadInt32();
				if ( m_HouseRev != 0 )
				{
					int len = reader.ReadUInt16();
					m_HousePacket = reader.ReadBytes( len );
				}
			}
			else
			{
				m_HouseRev = 0;
				m_HousePacket = null;
			}
		}

		public override void AfterLoad()
		{
			for(int i=0;i<m_Items.Count;i++)
			{
				if ( m_Items[i] is Serial )
				{
					m_Items[i] = World.FindItem( (Serial)m_Items[i] );

					if ( m_Items[i] == null )
					{
						m_Items.RemoveAt( i );
						i--;
					}
				}
			}

			UpdateContainer();
		}

		public Item( Serial serial ) : base( serial )
		{
			m_Items = new ArrayList();

			m_Visible = true;
			m_Movable = true;
		}

		public ItemID ItemID
		{
			get{ return m_ItemID; }
			set{ m_ItemID = value; }
		}

		public ushort Amount
		{
			get{ return m_Amount; }
			set{ m_Amount = value; }
		}

		public byte Direction
		{
			get{ return m_Direction; }
			set{ m_Direction = value; }
		}

		public bool Visible
		{
			get{ return m_Visible; }
			set{ m_Visible = value; }
		}

		public bool Movable
		{
			get{ return m_Movable; }
			set{ m_Movable = value; }
		}

		public string Name
		{
			get
			{
				if ( m_Name != null && m_Name != "" )
				{
					return  m_Name;
				}
				else
				{
					return m_ItemID.ToString();
				}
			}
			set
			{
				if ( value != null )
					m_Name = value.Trim();
				else
					m_Name = null;
			}
		}

		public byte Layer
		{
			get
			{ 
				if ( ( m_Layer <= 0 || m_Layer > 0x1D ) && 
					( (this.ItemID.ItemData.Flags&Ultima.TileFlag.Wearable) != 0 || 
					(this.ItemID.ItemData.Flags&Ultima.TileFlag.Armor) != 0 ||
					(this.ItemID.ItemData.Flags&Ultima.TileFlag.Weapon) != 0
					) )
				{
					m_Layer = (byte)this.ItemID.ItemData.Quality;
				}
				return m_Layer; 
			}
			set
			{
				m_Layer = value; 
			}
		}

		public Item FindItemByID( ItemID id )
		{
			return FindItemByID( id, true );
		}

		public Item FindItemByID( ItemID id, bool recurse )
		{
			for (int i=0;i<m_Items.Count;i++)
			{
				Item item = (Item)m_Items[i];
				if ( item.ItemID == id )
				{
					return item;
				}
				else if ( recurse )
				{
					item = item.FindItemByID( id, true );
					if ( item != null )
						return item;
				}
			}
			return null;
		}

		public int GetCount( ushort iid )
		{
			int count = 0;
			for (int i=0;i<m_Items.Count;i++)
			{
				Item item = (Item)m_Items[i];
				if ( item.ItemID == iid )
					count += item.Amount;
				count += item.GetCount( iid );
			}

			return count;
		}
		
		public object Container
		{
			get
			{
				if ( m_Parent is Serial && UpdateContainer() )
					m_NeedContUpdate.Remove( this );
				return m_Parent;
			}
			set
			{
				if ( ( m_Parent != null && m_Parent.Equals( value ) ) 
					|| ( value is Serial && m_Parent is UOEntity && ((UOEntity)m_Parent).Serial == (Serial)value ) 
					|| ( m_Parent is Serial && value is UOEntity && ((UOEntity)value).Serial == (Serial)m_Parent ) )
				{
					return;
				}

				if ( m_Parent is Mobile )
					((Mobile)m_Parent).RemoveItem( this );
				else if ( m_Parent is Item )
					((Item)m_Parent).RemoveItem( this );

				if ( value is Mobile )
					m_Parent = ((Mobile)value).Serial;
				else if ( value is Item )
					m_Parent = ((Item)value).Serial;
				else
					m_Parent = value;

				if ( !UpdateContainer() )
					m_NeedContUpdate.Add( this );
			}
		}

		public bool UpdateContainer()
		{
			if ( !(m_Parent is Serial) || Deleted )
				return true;

			object o = null;
			Serial contSer = (Serial)m_Parent;
			if ( contSer.IsItem )
				o = World.FindItem( contSer );
			else if ( contSer.IsMobile )
				o = World.FindMobile( contSer );
			
			if ( o == null )
				return false;
			
			m_Parent = o;

			if ( m_Parent is Item )
				((Item)m_Parent).AddItem( this );
			else if ( m_Parent is Mobile )
				((Mobile)m_Parent).AddItem( this );

			if ( IsChildOf( World.Player.Backpack ) )
			{
			}
			m_AutoStack = m_IsNew = false;

			return true;
		}

		private static ArrayList m_NeedContUpdate = new ArrayList();
		public static void UpdateContainers()
		{
			for(int i=0;i<m_NeedContUpdate.Count;i++)
			{
				if ( ((Item)m_NeedContUpdate[i]).UpdateContainer() )
				{
					m_NeedContUpdate.RemoveAt( i );
					i--;
				}
			}
		}

		public object RootContainer
		{
			get
			{
				object cont = this.Container;
				while ( cont != null && cont is Item && ((Item)cont).Container != null )
					cont = ((Item)cont).Container;

				return cont;
			}
		}

		public bool IsChildOf( object parent )
		{
			Serial parentSerial = 0;
			if ( parent is Mobile )
				return parent == RootContainer;
			else if ( parent is Item )
				parentSerial = ((Item)parent).Serial;
			else
				return false;

			object check = this;
			while ( check != null && check is Item )
			{
				if ( ((Item)check).Serial == parentSerial )
					return true;
				else
					check = ((Item)check).Container;
			}

			return false;
		}

		public Point3D GetWorldPosition()
		{
			object root = this.RootContainer;

			if ( root is Item )
				return ((Item)root).Position;
			else if ( root is Mobile )
				return ((Mobile)root).Position;
			else
				return this.Position;
		}

		private void AddItem( Item item )
		{
			for (int i=0;i<m_Items.Count;i++)
			{
				if ( m_Items[i] == item )
					return;
			}
			m_Items.Add( item );
		}

		private void RemoveItem( Item item )
		{
			m_Items.Remove( item );
		}

		public byte GetPacketFlags()
		{
			byte flags = 0;

			if ( !m_Visible )
			{
				flags |= 0x80;
			}

			if ( m_Movable )
			{
				flags |= 0x20;
			}

			return flags;
		}

		public int DistanceTo( Mobile m )
		{
			int x = Math.Abs( this.Position.X - m.Position.X );
			int y = Math.Abs( this.Position.Y - m.Position.Y );

			return x > y ? x : y;
		}

		public void ProcessPacketFlags( byte flags )
		{
			m_Visible = ( (flags&0x80) == 0 );
			m_Movable = ( (flags&0x20) != 0 );
		}

		public override void Remove()
		{
			ArrayList rem = new ArrayList( m_Items );
			m_Items.Clear();
			for (int i=0;i<rem.Count;i++)
				((Item)rem[i]).Remove();
			
			if ( m_Parent is Mobile )
				((Mobile)m_Parent).RemoveItem( this );
			else if ( m_Parent is Item )
				((Item)m_Parent).RemoveItem( this );

			World.RemoveItem( this );
			base.Remove();
		}

		public ArrayList Contains{ get{ return m_Items; } }
		
		public bool OnGround{ get{ return Container == null; } }
		public bool IsContainer
		{
			get
			{
				ushort iid = m_ItemID.Value;
				return iid == 0x9B2 || iid == 0x9AB || iid == 0xE3C || iid == 0xE3E ||
					( iid >= 0xE75 && iid <= 0xE80 && iid != 0xE7B ) || ( iid >= 0xE40 && iid <= 0xE42 );
			}
		}

		public bool IsNew
		{
			get{ return m_IsNew; }
			set{ m_IsNew = value; }
		}

		public bool AutoStack
		{
			get{ return m_AutoStack; }
			set{ m_AutoStack = value; }
		}

		public bool IsMulti
		{
			get { return m_ItemID.Value >= 0x4000; }
		}

		public bool IsPouch
		{
			get { return m_ItemID.Value == 0x0E79; }
		}

		public bool IsCorpse
		{
			get	{ return m_ItemID.Value == 0x2006 || ( m_ItemID.Value >= 0x0ECA && m_ItemID.Value <= 0x0ED2 ); }
		}

		public bool IsResource
		{
			get
			{
				ushort iid = m_ItemID.Value;
				return ( iid >= 0x19B7 && iid <= 0x19BA ) || // ore
					( iid >= 0x09CC && iid <= 0x09CF ) || // fishes
					( iid >= 0x1BDD && iid <= 0x1BE2 ) || // logs
					iid == 0x1779 || // granite / stone
					iid == 0x11EA || iid == 0x11EB // sand
				;
			}
		}

		public bool IsTwoHanded
		{
			get
			{
				ushort iid = m_ItemID.Value;
				return ( 
					// everything in layer 2 except shields is 2handed
						this.Layer == 0x02 && 
						!( ( iid >= 0x1b72 && iid <= 0x1b7b ) || // shields
						( iid >= 0x1bc3 && iid <= 0x1bc5 ) ) // virtue shields
					) || 

					// and all of these layer 1 weapons:
					( iid == 0x13fc || iid == 0x13fd ) || // hxbow
					( iid == 0x13AF || iid == 0x13b2 ) || // war axe & bow
					( iid >= 0x0F43 && iid <= 0x0F50 ) || // axes & xbow
					( iid == 0x1438 || iid == 0x1439 ) || // war hammer
					( iid == 0x1442 || iid == 0x1443 ) || // 2handed axe
					( iid == 0x1402 || iid == 0x1403 ) || // short spear
					( iid == 0x26c1 || iid == 0x26cb ) || // aos gay blade
					( iid == 0x26c2 || iid == 0x26cc ) || // aos gay bow
					( iid == 0x26c3 || iid == 0x26cd ) // aos gay xbow
				;
			}
		}

		public override string ToString()
		{
			return String.Format( "{0} ({1})", this.Name, this.Serial );
		}

		public int Price
		{
			get { return m_Price; }
			set { m_Price = value; }
		}

		public string BuyDesc
		{
			get { return m_BuyDesc; }
			set { m_BuyDesc = value; }
		}

		public int HouseRevision 
		{
			get { return m_HouseRev; }
			set { m_HouseRev = value; }
		}

		public byte[] HousePacket
		{
			get { return m_HousePacket; }
			set { m_HousePacket = value; }
		}
	}
}
