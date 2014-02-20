using System;
using System.Text;
using System.Collections;

namespace Assistant
{
	public class ObjectProperty
	{
		private int m_Num;
		private string m_Args;

		public ObjectProperty( int num, string args )
		{
			m_Num = num;
			m_Args = args;
		}

		public int Number { get{ return m_Num; } set{ m_Num = value; } }
		public string Args { get{ return m_Args; } set{ m_Args = value; } }
	}

	public class ObjectPropertyList
	{
		private static Hashtable m_Entries = new Hashtable();

		public static Hashtable Entries { get { return m_Entries; } }

		public static void Read( PacketReader p )
		{
			Serial ser = p.ReadUInt32();
			ushort zero = p.ReadUInt16();
			int hash = p.ReadInt32();

			object old = m_Entries[ser];
			if ( old is ObjectPropertyList )
			{
				if ( ((ObjectPropertyList)old).ServerHash+0x40000000 == hash )
					return;
			}

			ArrayList props = new ArrayList();
			while ( true )
			{
				int num = p.ReadInt32();
				if ( num == 0 || num == -1 )
					break;

				short argLen = p.ReadInt16();
				string args = String.Empty;

				if ( argLen > 0 )
					args = p.ReadUnicodeString( argLen >> 1 );

				props.Add( new ObjectProperty( num, args ) );
			}

			ObjectPropertyList list;
			m_Entries[ser] = list = new ObjectPropertyList( ser, hash, props );
			if ( old is ObjectPropertyList )
				list.Add( ((ObjectPropertyList)old).MyProps );
		}

		public static void Remove( Serial s )
		{
			m_Entries.Remove( s );
		}

		public int GetHash() { return 0x40000000 + ( m_ServerHash ^ m_MyHash ); }
		public Serial Serial { get { return m_Serial; } }
		public ArrayList MyProps { get { return m_MyProps; } }
		public int ServerHash { get { return m_ServerHash; } }
		public int MyHash { get { return m_MyHash; } }

		public ArrayList GetProps()
		{
			ArrayList list = new ArrayList( m_ServerProps );
			if ( m_MyProps != null && m_MyProps.Count > 0 )
				list.AddRange( m_MyProps );
			return list;
		}

		private Serial m_Serial;
		private int m_ServerHash;
		private ArrayList m_ServerProps;

		private int m_MyHash;
		private ArrayList m_MyProps;

		public ObjectPropertyList( Serial ser, int hash, ArrayList props )
		{
			m_Serial = ser;
			m_ServerHash = hash;
			m_ServerProps = props;

			m_MyHash = 0;
			m_MyProps = null;
		}

		public void Add( int number )
		{
			Add( number, String.Empty );
		}

		private static byte[] m_Buffer = new byte[1024];
		private static Encoding m_Encoding = Encoding.Unicode;

		public void AddHash( int val )
		{
			m_MyHash ^= (val & 0x3FFFFFF);
			m_MyHash ^= (val >> 26) & 0x3F;
		}

		public void Add( int number, string arguments )
		{
			if ( number == 0 )
				return;

			if ( arguments == null )
				arguments = String.Empty;

			AddHash( number );
			AddHash( arguments.GetHashCode() );

			if ( m_MyProps == null )
				m_MyProps = new ArrayList();

			m_MyProps.Add( new ObjectProperty( number, arguments ) );
		}

		public void Add( int number, string format, params object[] args )
		{
			Add( number, String.Format( format, args ) );
		}

		public void Add( string text )
		{
			Add( 1042971, text );
		}

		public void Add( string format, params object[] args )
		{
			Add( 1042971, String.Format( format, args ) );
		}

		public void Add( ArrayList list )
		{
			if ( list == null || list.Count <= 0 )
				return;

			for(int i=0;i<list.Count;i++)
			{
				if ( list[i] is ObjectProperty )
					Add( ((ObjectProperty)list[i]).Number, ((ObjectProperty)list[i]).Args );
			}
		}
	}

	public class OPLPacket : Packet
	{
		public OPLPacket( ObjectPropertyList list ) : base( 0xD6 )
		{
			EnsureCapacity( 128 );

			Write( (short) 1 );
			Write( (int) list.Serial );
			Write( (short) 0 );
			Write( (int) list.GetHash() );

			ArrayList props = list.GetProps();
			for(int i=0;i<props.Count;i++)
			{
				ObjectProperty prop = (ObjectProperty)props[i];
				Write( (int) prop.Number );

				if ( prop.Args.Length > 0 )
				{
					byte[] buff = Encoding.Unicode.GetBytes( prop.Args );
					Write( (short) buff.Length );
					Write( buff, 0, buff.Length );
				}
				else
				{
					Write( (short) 0 );
				}
			}

			Write( (int) 0 );
		}
	}

	public class OPLInfoPacket : Packet
	{
		public OPLInfoPacket( ObjectPropertyList list ) : base( 0xBF )
		{
			EnsureCapacity( 13 );

			Write( (short) 0x10 );
			Write( (int) list.Serial );
			Write( (int) list.GetHash() );
		}

		public OPLInfoPacket( Serial serial, int hash ) : base( 0xBF )
		{
			EnsureCapacity( 13 );

			Write( (short) 0x10 );

			ObjectPropertyList list = ObjectPropertyList.Entries[serial] as ObjectPropertyList;
			if ( list != null && list.MyHash != 0 )
				hash = 0x40000000 + ((hash-0x40000000) ^ list.MyHash);

			Write( (int) serial );
			Write( (int) hash );
		}
	}
}
