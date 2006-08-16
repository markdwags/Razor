using System;
using System.IO;
using System.Text;
using System.Collections;

namespace Assistant
{
	public class Utility
	{	
		private static Random m_Random = new Random();

		public static int Random( int min, int max )
		{
			return m_Random.Next( max - min + 1 ) + min;
		}

		public static int Random( int num )
		{
			return m_Random.Next( num );
		}

		public static bool InRange( IPoint2D from, IPoint2D to, int range )
		{
			return ( to.X >= (from.X - range) )
				&& ( to.X <= (from.X + range) )
				&& ( to.Y >= (from.Y - range) )
				&& ( to.Y <= (from.Y + range) );
		}

		public static int Distance( int fx, int fy, int tx, int ty )
		{
			int xDelta = Math.Abs( fx - tx );
			int yDelta = Math.Abs( fy - ty );

			return ( xDelta > yDelta ? xDelta : yDelta );
		}

		public static int Distance( IPoint2D from, IPoint2D to )
		{
			int xDelta = Math.Abs( from.X - to.X );
			int yDelta = Math.Abs( from.Y - to.Y );

			return ( xDelta > yDelta ? xDelta : yDelta );
		}

		public static double DistanceSqrt( IPoint2D from, IPoint2D to )
		{
			float xDelta = Math.Abs( from.X - to.X );
			float yDelta = Math.Abs( from.Y - to.Y );

			return Math.Sqrt( xDelta*xDelta + yDelta*yDelta );
		}

		public static void Offset( Direction d, ref int x, ref int y )
		{
			switch ( d & Direction.Mask )
			{
				case Direction.North:		--y; break;
				case Direction.South:		++y; break;
				case Direction.West:  --x;		 break;
				case Direction.East:  ++x;		 break;
				case Direction.Right: ++x; --y;  break;
				case Direction.Left:  --x; ++y;  break;
				case Direction.Down:  ++x; ++y;  break;
				case Direction.Up:    --x; --y;  break;
			}
		}

		public static void FormatBuffer( TextWriter output, Stream input, int length )
		{
			output.WriteLine( "        0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F" );
			output.WriteLine( "       -- -- -- -- -- -- -- --  -- -- -- -- -- -- -- --" );

			int byteIndex = 0;

			int whole = length >> 4;
			int rem = length & 0xF;

			for ( int i = 0; i < whole; ++i, byteIndex += 16 )
			{
				StringBuilder bytes = new StringBuilder( 49 );
				StringBuilder chars = new StringBuilder( 16 );

				for ( int j = 0; j < 16; ++j )
				{
					int c = input.ReadByte();

					bytes.Append( c.ToString( "X2" ) );

					if ( j != 7 )
					{
						bytes.Append( ' ' );
					}
					else
					{
						bytes.Append( "  " );
					}

					if ( c >= 0x20 && c < 0x80 )
					{
						chars.Append( (char)c );
					}
					else
					{
						chars.Append( '.' );
					}
				}

				output.Write( byteIndex.ToString( "X4" ) );
				output.Write( "   " );
				output.Write( bytes.ToString() );
				output.Write( "  " );
				output.WriteLine( chars.ToString() );
			}

			if ( rem != 0 )
			{
				StringBuilder bytes = new StringBuilder( 49 );
				StringBuilder chars = new StringBuilder( rem );

				for ( int j = 0; j < 16; ++j )
				{
					if ( j < rem )
					{
						int c = input.ReadByte();

						bytes.Append( c.ToString( "X2" ) );

						if ( j != 7 )
						{
							bytes.Append( ' ' );
						}
						else
						{
							bytes.Append( "  " );
						}

						if ( c >= 0x20 && c < 0x80 )
						{
							chars.Append( (char)c );
						}
						else
						{
							chars.Append( '.' );
						}
					}
					else
					{
						bytes.Append( "   " );
					}
				}

				output.Write( byteIndex.ToString( "X4" ) );
				output.Write( "   " );
				output.Write( bytes.ToString() );
				if ( rem <= 8 )
					output.Write( "   " );
				else
					output.Write( "  " );
				output.WriteLine( chars.ToString() );
			}
		}

		public static unsafe void FormatBuffer( TextWriter output, byte *buff, int length )
		{
			output.WriteLine( "        0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F" );
			output.WriteLine( "       -- -- -- -- -- -- -- --  -- -- -- -- -- -- -- --" );

			int byteIndex = 0;

			int whole = length >> 4;
			int rem = length & 0xF;

			for ( int i = 0; i < whole; ++i, byteIndex += 16 )
			{
				StringBuilder bytes = new StringBuilder( 49 );
				StringBuilder chars = new StringBuilder( 16 );

				for ( int j = 0; j < 16; ++j )
				{
					int c = *buff++;

					bytes.Append( c.ToString( "X2" ) );

					if ( j != 7 )
					{
						bytes.Append( ' ' );
					}
					else
					{
						bytes.Append( "  " );
					}

					if ( c >= 0x20 && c < 0x80 )
					{
						chars.Append( (char)c );
					}
					else
					{
						chars.Append( '.' );
					}
				}

				output.Write( byteIndex.ToString( "X4" ) );
				output.Write( "   " );
				output.Write( bytes.ToString() );
				output.Write( "  " );
				output.WriteLine( chars.ToString() );
			}

			if ( rem != 0 )
			{
				StringBuilder bytes = new StringBuilder( 49 );
				StringBuilder chars = new StringBuilder( rem );

				for ( int j = 0; j < 16; ++j )
				{
					if ( j < rem )
					{
						int c = *buff++;

						bytes.Append( c.ToString( "X2" ) );

						if ( j != 7 )
						{
							bytes.Append( ' ' );
						}
						else
						{
							bytes.Append( "  " );
						}

						if ( c >= 0x20 && c < 0x80 )
						{
							chars.Append( (char)c );
						}
						else
						{
							chars.Append( '.' );
						}
					}
					else
					{
						bytes.Append( "   " );
					}
				}

				output.Write( byteIndex.ToString( "X4" ) );
				output.Write( "   " );
				output.Write( bytes.ToString() );
				if ( rem <= 8 )
					output.Write( "   " );
				else
					output.Write( "  " );
				output.WriteLine( chars.ToString() );
			}
		}

		private static char[] pathChars = new char[]{ '\\', '/' };
		public static string PathDisplayStr( string path, int maxLen )
		{
			if ( path == null || path.Length <= maxLen || path.Length < 5 )
				return path;

			int first = (maxLen - 3) / 2;
			int last = path.LastIndexOfAny( pathChars );
			if ( last == -1 || last < maxLen / 4 )
				last = path.Length - first;
			first = maxLen - last - 3;
			if ( first < 0 )
				first = 1;
			if ( last < first )
				last = first;

			return String.Format( "{0}...{1}", path.Substring( 0, first ), path.Substring( last ) );
		}

		public static string FormatSize( long size )
		{
			if ( size < 1024 ) // 1 K
				return String.Format( "{0:#,##0} B", size );
			else if ( size < 1048576 ) // 1 M
				return String.Format( "{0:#,###.0} KB", size / 1024.0 );
			else
				return String.Format( "{0:#,###.0} MB", size / 1048576.0 );
		}

		public static string FormatTime( int sec )
		{
			int m = sec/60;
			int h = m/60;
			m = m%60;
			return String.Format( "{0:#0}:{1:00}:{2:00}", h, m, sec%60 );
		}

		public static string FormatTimeMS( int ms )
		{
			int s = ms/1000;
			int m = s/60;
			int h = m/60;

			ms = ms%1000;
			s = s%60;
			m = m%60;
			
			if ( h > 0 || m > 55 )
				return String.Format( "{0:#0}:{1:00}:{2:00}.{3:000}", h, m, s, ms );
			else
				return String.Format( "{0:00}:{1:00}.{2:000}", m, s, ms );
		}

		public static int ToInt32( string str, int def )
		{
			if ( str == null )
				return def;

			try
			{
				if ( str.Length > 2 && str.Substring( 0, 2 ).ToLower() == "0x" )
					return Convert.ToInt32( str.Substring( 2 ), 16 );
				else
					return Convert.ToInt32( str );
			}
			catch
			{
				return def;
			}
		}
	}
}
