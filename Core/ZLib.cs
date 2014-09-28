using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Assistant
{
	public enum ZLibError : int
	{
		Z_OK = 0,
		Z_STREAM_END = 1,
		Z_NEED_DICT = 2,
		Z_ERRNO = (-1),
		Z_STREAM_ERROR = (-2),
		Z_DATA_ERROR = (-3), // Data was corrupt
		Z_MEM_ERROR = (-4), //  Not Enough Memory
		Z_BUF_ERROR = (-5), // Not enough buffer space
		Z_VERSION_ERROR = (-6),
	}

	[Flags]
	public enum ZLibCompressionLevel : int
	{
		Z_NO_COMPRESSION = 0,
		Z_BEST_SPEED = 1,
		Z_BEST_COMPRESSION = 9,
		Z_DEFAULT_COMPRESSION = (-1)
	}

	public class ZLib
	{
		[DllImport( "zlib" )]
		internal static extern string zlibVersion();
		[DllImport( "zlib" )]
		internal static extern ZLibError compress(byte[] dest, ref int destLength, byte[] source, int sourceLength);
		[DllImport( "zlib" )]
		internal static extern ZLibError compress2(byte[] dest, ref int destLength, byte[] source, int sourceLength, ZLibCompressionLevel level);
		[DllImport( "zlib" )]
		internal static extern ZLibError uncompress(byte[] dest, ref int destLen, byte[] source, int sourceLen);
	}

	// Be careful when writing raw data, as it may confuse the GZBlockIn if not accounted for when reading.
	// Seeking in the compressed stream is HIGHLY unrecommended
	// If you need to seek, use BufferAll to keep all data in the buffer, seek as much as you want, then 
	// turn off BufferAll and flush the data to disk.
	// Once the data is flushed, you CANNOT seek back to it!
	public class GZBlockOut : Stream
	{
		private BinaryWriter m_Out;
		private MemoryStream m_Buffer;
		private BinaryWriter m_Self;
		private int m_BlockSize;
		private bool m_BufferAll;
		private bool m_IsCompressed;
		
		public override bool CanSeek { get { return false; } }
		public override bool CanRead { get { return false; } }
		public override bool CanWrite { get { return true; } }
		public override long Length { get { return RawStream.Length; } }
		public override long Position { get { return m_IsCompressed ? m_Buffer.Position : RawStream.Position; } set {} }

		public Stream RawStream { get { return m_Out.BaseStream; } }
		public BinaryWriter Raw { get { return m_Out; } }
		public BinaryWriter Compressed { get { return m_Self; } }
		public MemoryStream Buffer { get { return m_Buffer; } }
		public int BlockSize { get { return m_BlockSize; } set { m_BlockSize = value; } }
		public bool BufferAll { get { return m_BufferAll; } set { m_BufferAll = value; } }
		public bool IsCompressed { get { return m_IsCompressed; } set { ForceFlush(); m_IsCompressed = value; } }

		public GZBlockOut( string filename, int blockSize )
		{
			m_IsCompressed = true;

			m_Out = new BinaryWriter( new FileStream( filename, FileMode.Create, FileAccess.ReadWrite, FileShare.None ) );
			m_BlockSize = blockSize;
			m_Buffer = new MemoryStream( blockSize + 1024 );

			m_Self = new BinaryWriter( this );
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if ( m_IsCompressed )
			{
				m_Buffer.Write( buffer, offset, count );
				if ( m_Buffer.Position >= m_BlockSize )
					FlushBuffer();
			}
			else
			{
				RawStream.Write( buffer, offset, count );
			}
		}

		public override void WriteByte(byte value)
		{
			if ( m_IsCompressed )
			{
				m_Buffer.WriteByte( value );
				if ( m_Buffer.Position >= m_BlockSize )
					FlushBuffer();
			}
			else
			{
				RawStream.WriteByte( value );
			}
		}

		private static byte[] m_CompBuff = null;
		public void FlushBuffer()
		{
			if ( !m_IsCompressed || m_BufferAll || m_Buffer.Position <= 0 )
				return;

			int outLen = (int)(m_Buffer.Position * 1.1);
			if ( m_CompBuff == null || m_CompBuff.Length < outLen )
				m_CompBuff = new byte[outLen];
			else
				outLen = m_CompBuff.Length;

			ZLibError error = ZLib.compress2( m_CompBuff, ref outLen, m_Buffer.ToArray(), (int)m_Buffer.Position, ZLibCompressionLevel.Z_BEST_COMPRESSION );
			if ( error != ZLibError.Z_OK )
				throw new Exception( "ZLib error during copression: " + error.ToString() );

			Raw.Write( (int)outLen );
			Raw.Write( (int)m_Buffer.Position );
			Raw.Write( m_CompBuff, 0, outLen );

			m_Buffer.Position = 0;
		}

		public override void Flush()
		{
			FlushBuffer();
			RawStream.Flush();
		}

		public void ForceFlush()
		{
			bool old = m_BufferAll;
			m_BufferAll = false;
			Flush();
			m_BufferAll = old;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			if ( m_IsCompressed )
				return m_Buffer.Seek( offset, origin );
			else
				return RawStream.Seek( offset, origin );
		}

		public override void SetLength(long value)
		{
			RawStream.SetLength( value );
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return 0;
		}

		public override void Close()
		{
			ForceFlush();

			base.Close ();
			m_Out.Close();
			m_Buffer.Close();
			m_Self = null;
		}
	}

	// Represents a block compressed stream written by GZBlockOut
	// If there is uncompressed data in the stream, you may seek to 
	// it and read from is as you wish using Raw/RawStream.  If you have
	// not yet started reading compressed data, you must position rawstream 
	// at the begining of the compressed data.  If you've already read 
	// compressed data, you must reposition the file pointer back to its previous
	// position in the stream.  This is really important.
	//
	// Seeking in the compressed stream should be okay, DO NOT attempt to seek outside
	// of the compressed data.
	public class GZBlockIn : Stream
	{
		private MemoryStream m_Uncomp;
		private BinaryReader m_In;
		private BinaryReader m_Self;
		private bool m_Compressed;

		public Stream RawStream { get { return m_In.BaseStream; } }
		public BinaryReader Raw { get { return m_In; } }
		public BinaryReader Compressed { get { return m_Compressed ? m_Self : m_In; } }
		public bool IsCompressed { get { return m_Compressed; } set { m_Compressed = value; } }

		public override bool CanSeek { get { return true; } }
		public override bool CanRead { get { return true; } }
		public override bool CanWrite { get { return false; } }
		public override long Length { get { return m_Compressed ? ( RawStream.Position < RawStream.Length ? int.MaxValue : m_Uncomp.Length ) : RawStream.Length; } }
		public override long Position { get { return m_Compressed ? m_Uncomp.Position : RawStream.Position; } set { if ( m_Compressed ) m_Uncomp.Position = value; else RawStream.Position = value; } }

		public GZBlockIn( string filename )
		{
			m_Compressed = true;

			m_In = new BinaryReader( new FileStream( filename, FileMode.Open, FileAccess.Read, FileShare.Read ) );
			m_Uncomp = new MemoryStream();
			m_Self = new BinaryReader( this );
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
		}

		public override void Flush()
		{
			RawStream.Flush();
			m_Uncomp.Flush();
		}

		private static byte[] m_ReadBuff = null;
		private static byte[] m_CompBuff = null;
		public override long Seek(long offset, SeekOrigin origin)
		{
			if ( m_Compressed )
			{
				long absPos = offset;
				if ( origin == SeekOrigin.Current )
					absPos += m_Uncomp.Position;

				if ( absPos < 0 )
					throw new Exception( "Cannot seek past the begining of the stream." );

				long pos = m_Uncomp.Position;
				m_Uncomp.Seek( 0, SeekOrigin.End );

				while ( ( origin == SeekOrigin.End || absPos >= m_Uncomp.Length ) && RawStream.Position < RawStream.Length )
				{
					int block = Raw.ReadInt32();
					int ucLen = Raw.ReadInt32();
					if ( m_ReadBuff == null || m_ReadBuff.Length < block )
						m_ReadBuff = new byte[block];
					
					if ( m_CompBuff == null || m_CompBuff.Length < ucLen )
						m_CompBuff = new byte[ucLen];
					else
						ucLen = m_CompBuff.Length;

					Raw.Read( m_ReadBuff, 0, block );

					ZLibError error = ZLib.uncompress( m_CompBuff, ref ucLen, m_ReadBuff, block );
					if ( error != ZLibError.Z_OK )
						throw new Exception( "ZLib error uncompressing: " + error.ToString() );

					m_Uncomp.Write( m_CompBuff, 0, ucLen );
				}

				m_Uncomp.Position = pos;
				return m_Uncomp.Seek( offset, origin );
			}
			else
			{
				return RawStream.Seek( offset, origin );
			}
		}

		public override void SetLength(long value)
		{
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if ( m_Compressed )
			{
				long pos = m_Uncomp.Position;
				m_Uncomp.Seek( 0, SeekOrigin.End );

				while ( pos + count > m_Uncomp.Length && RawStream.Position+8 < RawStream.Length )
				{
					int block = Raw.ReadInt32();
					int ucLen = Raw.ReadInt32();

					if ( block > 0x10000000 || block <= 0 || ucLen > 0x10000000 || ucLen <= 0 )
						break;

					if ( RawStream.Position+block > RawStream.Length )
						break;

					if ( m_ReadBuff == null || m_ReadBuff.Length < block )
						m_ReadBuff = new byte[block];
					
					if ( m_CompBuff == null || m_CompBuff.Length < ucLen )
						m_CompBuff = new byte[ucLen];
					else
						ucLen = m_CompBuff.Length;

					Raw.Read( m_ReadBuff, 0, block );

					ZLibError error = ZLib.uncompress( m_CompBuff, ref ucLen, m_ReadBuff, block );
					if ( error != ZLibError.Z_OK )
						throw new Exception( "ZLib error uncompressing: " + error.ToString() );

					m_Uncomp.Write( m_CompBuff, 0, ucLen );
				}

				m_Uncomp.Position = pos;
				return m_Uncomp.Read( buffer, offset, count );
			}
			else
			{
				return RawStream.Read( buffer, offset, count );
			}
		}

		public override void Close()
		{
			m_In.Close();
			m_Uncomp.Close();
			m_Self = null;
		}

		public bool EndOfFile
		{
			get 
			{
				return ( ( !m_Compressed || m_Uncomp.Position >= m_Uncomp.Length ) && RawStream.Position >= RawStream.Length );
			}
		}
	}
}
