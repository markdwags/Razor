using System;

namespace Assistant
{
	public struct ItemID 
	{
		private ushort m_ID;

		public ItemID( ushort id )
		{
			m_ID = id;
		}

		public ushort Value
		{
			get
			{
				return m_ID;
			}
		}
		public static implicit operator ushort( ItemID a )
		{
			return a.m_ID;
		}

		public static implicit operator ItemID( ushort a )
		{
			return new ItemID( a );
		}

		public override string ToString()
		{
			try
			{
				return string.Format( "{0} ({1:X4})", Ultima.TileData.ItemTable[m_ID].Name, m_ID );
			}
			catch
			{
				return String.Format( " ({0:X4})", m_ID );
			}
		}

		public Ultima.ItemData ItemData
		{
			get
			{
				try
				{
					return Ultima.TileData.ItemTable[m_ID];
				}
				catch
				{
					return new Ultima.ItemData("", Ultima.TileFlag.None, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
				}
			}
		}

		public override int GetHashCode()
		{
			return m_ID;
		}

		public override bool Equals( object o )
		{
			if ( o == null || !(o is ItemID) ) return false;

			return ((ItemID)o).m_ID == m_ID;
		}

		public static bool operator == ( ItemID l, ItemID r )
		{
			return l.m_ID == r.m_ID;
		}

		public static bool operator != ( ItemID l, ItemID r )
		{
			return l.m_ID != r.m_ID;
		}

		public static bool operator > ( ItemID l, ItemID r )
		{
			return l.m_ID > r.m_ID;
		}

		public static bool operator >= ( ItemID l, ItemID r )
		{
			return l.m_ID >= r.m_ID;
		}

		public static bool operator < ( ItemID l, ItemID r )
		{
			return l.m_ID < r.m_ID;
		}

		public static bool operator <= ( ItemID l, ItemID r )
		{
			return l.m_ID <= r.m_ID;
		}
	}
}
