using System;

namespace Assistant
{
	public struct Serial : IComparable
	{
		private uint m_Serial;

		public static readonly Serial MinusOne = new Serial( 0xFFFFFFFF );
		public static readonly Serial Zero = new Serial( 0 );

		private Serial( uint serial )
		{
			m_Serial = serial;
		}

		public uint Value
		{
			get
			{
				return m_Serial;
			}
		}

		public bool IsMobile
		{
			get
			{
				return ( m_Serial > 0 && m_Serial < 0x40000000 );
			}
		}

		public bool IsItem
		{
			get
			{
				return ( m_Serial >= 0x40000000 && m_Serial <= 0x7FFFFF00 );
			}
		}

		public bool IsValid
		{
			get
			{
				return ( m_Serial > 0 && m_Serial <= 0x7FFFFF00 );
			}
		}

		public override int GetHashCode()
		{
			return (int)m_Serial;
		}

		public int CompareTo( object o )
		{
			if ( o == null ) return 1;
			else if ( !(o is Serial) ) throw new ArgumentException();

			uint ser = ((Serial)o).m_Serial;

			if ( m_Serial > ser ) return 1;
			else if ( m_Serial < ser ) return -1;
			else return 0;
		}

		public override bool Equals( object o )
		{
			if ( o == null || !(o is Serial) ) return false;

			return ((Serial)o).m_Serial == m_Serial;
		}

		public static bool operator == ( Serial l, Serial r )
		{
			return l.m_Serial == r.m_Serial;
		}

		public static bool operator != ( Serial l, Serial r )
		{
			return l.m_Serial != r.m_Serial;
		}

		public static bool operator > ( Serial l, Serial r )
		{
			return l.m_Serial > r.m_Serial;
		}

		public static bool operator < ( Serial l, Serial r )
		{
			return l.m_Serial < r.m_Serial;
		}

		public static bool operator >= ( Serial l, Serial r )
		{
			return l.m_Serial >= r.m_Serial;
		}

		public static bool operator <= ( Serial l, Serial r )
		{
			return l.m_Serial <= r.m_Serial;
		}

		public override string ToString()
		{
			return String.Format( "0x{0:X}", m_Serial );
		}

		public static Serial Parse( string s )
		{
			if ( s.StartsWith( "0x" ) )
				return (Serial)Convert.ToUInt32( s.Substring( 2 ), 16 );
			else
				return (Serial)Convert.ToUInt32( s );
		}

		public static implicit operator uint( Serial a )
		{
			return a.m_Serial;
		}

		public static implicit operator int( Serial a )
		{
			return (int)a.m_Serial;
		}

		public static implicit operator Serial( uint a )
		{
			return new Serial( a );
		}
	}
}

