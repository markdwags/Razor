using System;

namespace Assistant
{
	public interface IPoint2D
	{
		int X{ get; }
		int Y{ get; }
	}

	public interface IPoint3D : IPoint2D
	{
		int Z{ get; }
	}

	public struct Point2D : IPoint2D
	{
		internal int m_X;
		internal int m_Y;

		public static readonly Point2D Zero = new Point2D( 0, 0 );
		public static readonly Point2D MinusOne = new Point2D( -1, -1 );

		public Point2D( int x, int y )
		{
			m_X = x;
			m_Y = y;
		}

		public Point2D( IPoint2D p ) : this( p.X, p.Y )
		{
		}

		public int X
		{
			get
			{
				return m_X;
			}
			set
			{
				m_X = value;
			}
		}

		public int Y
		{
			get
			{
				return m_Y;
			}
			set
			{
				m_Y = value;
			}
		}

		public override string ToString()
		{
			return String.Format( "({0}, {1})", m_X, m_Y );
		}

		public override bool Equals( object o )
		{
			if ( o == null || !(o is IPoint2D) ) return false;

			IPoint2D p = (IPoint2D)o;

			return m_X == p.X && m_Y == p.Y;
		}

		public override int GetHashCode()
		{
			return m_X ^ m_Y;
		}

		public static bool operator == ( Point2D l, Point2D r )
		{
			return l.m_X == r.m_X && l.m_Y == r.m_Y;
		}

		public static bool operator != ( Point2D l, Point2D r )
		{
			return l.m_X != r.m_X || l.m_Y != r.m_Y;
		}

		public static bool operator == ( Point2D l, IPoint2D r )
		{
			return l.m_X == r.X && l.m_Y == r.Y;
		}

		public static bool operator != ( Point2D l, IPoint2D r )
		{
			return l.m_X !=r.X || l.m_Y != r.Y;
		}

		public static bool operator > ( Point2D l, Point2D r )
		{
			return l.m_X > r.m_X && l.m_Y > r.m_Y;
		}

		public static bool operator > ( Point2D l, Point3D r )
		{
			return l.m_X > r.m_X && l.m_Y > r.m_Y;
		}

		public static bool operator > ( Point2D l, IPoint2D r )
		{
			return l.m_X > r.X && l.m_Y > r.Y;
		}

		public static bool operator < ( Point2D l, Point2D r )
		{
			return l.m_X < r.m_X && l.m_Y < r.m_Y;
		}

		public static bool operator < ( Point2D l, Point3D r )
		{
			return l.m_X < r.m_X && l.m_Y < r.m_Y;
		}

		public static bool operator < ( Point2D l, IPoint2D r )
		{
			return l.m_X < r.X && l.m_Y < r.Y;
		}

		public static bool operator >= ( Point2D l, Point2D r )
		{
			return l.m_X >= r.m_X && l.m_Y >= r.m_Y;
		}

		public static bool operator >= ( Point2D l, Point3D r )
		{
			return l.m_X >= r.m_X && l.m_Y >= r.m_Y;
		}

		public static bool operator >= ( Point2D l, IPoint2D r )
		{
			return l.m_X >= r.X && l.m_Y >= r.Y;
		}

		public static bool operator <= ( Point2D l, Point2D r )
		{
			return l.m_X <= r.m_X && l.m_Y <= r.m_Y;
		}

		public static bool operator <= ( Point2D l, Point3D r )
		{
			return l.m_X <= r.m_X && l.m_Y <= r.m_Y;
		}

		public static bool operator <= ( Point2D l, IPoint2D r )
		{
			return l.m_X <= r.X && l.m_Y <= r.Y;
		}
	}

	public struct Point3D : IPoint3D
	{
		internal int m_X;
		internal int m_Y;
		internal int m_Z;

		public static readonly Point3D Zero = new Point3D( 0, 0, 0 );
		public static readonly Point3D MinusOne = new Point3D( -1, -1, 0 );

		public Point3D( int x, int y, int z )
		{
			m_X = x;
			m_Y = y;
			m_Z = z;
		}

		public Point3D( IPoint3D p ) : this( p.X, p.Y, p.Z )
		{
		}

		public Point3D( IPoint2D p, int z ) : this( p.X, p.Y, z )
		{
		}

		public int X
		{
			get
			{
				return m_X;
			}
			set
			{
				m_X = value;
			}
		}

		public int Y
		{
			get
			{
				return m_Y;
			}
			set
			{
				m_Y = value;
			}
		}

		public int Z
		{
			get
			{
				return m_Z;
			}
			set
			{
				m_Z = value;
			}
		}

		public override string ToString()
		{
			return String.Format( "({0}, {1}, {2})", m_X, m_Y, m_Z );
		}

		public override bool Equals( object o )
		{
			if ( o == null || !(o is IPoint3D) ) return false;

			IPoint3D p = (IPoint3D)o;

			return m_X == p.X && m_Y == p.Y && m_Z == p.Z;
		}

		public override int GetHashCode()
		{
			return m_X ^ m_Y ^ m_Z;
		}

		public static Point3D Parse( string value )
		{
			int start = value.IndexOf( '(' );
			int end = value.IndexOf( ',', start + 1 );

			string param1 = value.Substring( start + 1, end - (start + 1) ).Trim();

			start = end;
			end = value.IndexOf( ',', start + 1 );

			string param2 = value.Substring( start + 1, end - (start + 1) ).Trim();

			start = end;
			end = value.IndexOf( ')', start + 1 );

			string param3 = value.Substring( start + 1, end - (start + 1) ).Trim();

			return new Point3D( Convert.ToInt32( param1 ), Convert.ToInt32( param2 ), Convert.ToInt16( param3 ) );
		}

		public static bool operator == ( Point3D l, Point3D r )
		{
			return l.m_X == r.m_X && l.m_Y == r.m_Y && l.m_Z == r.m_Z;
		}

		public static bool operator != ( Point3D l, Point3D r )
		{
			return l.m_X != r.m_X || l.m_Y != r.m_Y || l.m_Z != r.m_Z;
		}

		public static bool operator == ( Point3D l, IPoint3D r )
		{
			return l.m_X == r.X && l.m_Y == r.Y && l.m_Z == r.Z;
		}

		public static bool operator != ( Point3D l, IPoint3D r )
		{
			return l.m_X != r.X || l.m_Y != r.Y || l.m_Z != r.Z;
		}

		public static Point3D operator + ( Point3D l, Point3D r )
		{
			return new Point3D( l.m_X+r.m_X, l.m_Y+r.m_Y, l.m_Z+r.m_Z );
		}

		public static Point3D operator - ( Point3D l, Point3D r )
		{
			return new Point3D( l.m_X-r.m_X, l.m_Y-r.m_Y, l.m_Z-r.m_Z );
		}
	}
}
