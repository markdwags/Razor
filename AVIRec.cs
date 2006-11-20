using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Assistant
{
	public class AVIRec
	{
		public static bool Recording { get { return m_RecTimer != null && m_RecTimer.Running; } }
		private static AVIFile m_Avi;
		private static Timer m_RecTimer;
		private static int m_ResX, m_ResY;

		private static RotateFlipType m_Flip = RotateFlipType.RotateNoneFlipNone;

		public static void UpdateFlip()
		{
			bool h = Config.GetBool( "FlipVidH" ), v = Config.GetBool( "FlipVidV" );

			if ( !h && !v )
				m_Flip = RotateFlipType.RotateNoneFlipNone;
			else if ( h && !v )
				m_Flip = RotateFlipType.RotateNoneFlipX;
			else if ( !h && v )
				m_Flip = RotateFlipType.RotateNoneFlipY;
			else //if ( h && v )
				m_Flip = RotateFlipType.RotateNoneFlipXY;
		}

		public static bool Record( int fps, double sizeFactor )
		{
			IntPtr hBmp = ClientCommunication.CaptureScreen( false, "" );
			using ( Bitmap bmp = Bitmap.FromHbitmap( hBmp ) )
			{
				m_ResX = (int)(bmp.Width * sizeFactor);
				m_ResY = (int)(bmp.Height * sizeFactor);
			}
			ClientCommunication.DeleteObject( hBmp );

			string filename;
			string name = "Unknown";
			string path = Config.GetString( "RecFolder" );
				
			if ( World.Player != null )
				name = World.Player.Name;
			if ( name == null || name.Trim() == "" || name.IndexOfAny( Path.InvalidPathChars ) != -1 )
				name = "Unknown";

			name = String.Format( "{0}_{1}", name, DateTime.Now.ToString( "M-d_HH.mm" ) );
			Engine.EnsureDirectory( path );
			int count  = 0;
			do
			{
				filename = Path.Combine( path, String.Format( "{0}{1}.avi", name, count != 0 ? count.ToString() : ""  ) );
				count--; // cause a - to be put in front of the number 
			}
			while ( File.Exists( filename ) );

			m_Avi = new AVIFile();
			if ( !m_Avi.Open( filename, fps, m_ResX, m_ResY ) )
				return false;

			UpdateFlip();
			
			m_RecTimer = new RecordTimer( fps );
			m_RecTimer.Start();

			return true;
		}

		public static void Stop()
		{
			if ( m_RecTimer != null )
				m_RecTimer.Stop();
			m_RecTimer = null;

			if ( m_Avi != null )
				m_Avi.Close();
			m_Avi = null;
		}

		private class RecordTimer : Timer
		{
			public RecordTimer( double fps ) : base( TimeSpan.FromSeconds( 1.0 / fps ), TimeSpan.FromSeconds( 1.0 / fps ) )
			{
			}

			protected override void OnTick()
			{
				if ( !Recording )
					return;

				StringBuilder sb = new StringBuilder();
				if ( m_Avi.FrameCount < 15 )
				{
					if ( World.Player != null && World.Player.Name != null )
					{
						sb.Append( World.Player.Name );
						sb.Append( " " );
					}

					if ( World.ShardName != null && World.ShardName != "" )
					{
						sb.Append( '(' );
						sb.Append( World.ShardName );
						sb.Append( ") " );
					}

					if ( sb.Length > 0 )
						sb.Append( "- " );

					sb.Append( DateTime.Now.ToString( @"M/dd/yy - HH:mm:ss" ) );
				}

				bool ok = false;
				try
				{
					IntPtr hBmp = ClientCommunication.CaptureScreen( false, sb.ToString() );
					using ( Bitmap bmp = Bitmap.FromHbitmap( hBmp ) )
					{
						if ( bmp.Width != m_ResX )
						{
							using ( Bitmap copy = new Bitmap( bmp, m_ResX, m_ResY ) )
							{
								copy.RotateFlip( m_Flip );
								ok = m_Avi.AddFrame( copy );
							}
						}
						else
						{
							bmp.RotateFlip( m_Flip );
							ok = m_Avi.AddFrame( bmp );
						}
					}
					ClientCommunication.DeleteObject( hBmp );
				}
				catch
				{
					ok = false;
				}

				if ( !ok )
				{
					AVIRec.Stop();
					throw new Exception( "There was an error writing a frame." );
				}
			}
		}
	}

	public class AVIFile
	{
		private int m_hFile;
		private IntPtr m_pStream;
		private IntPtr m_pCompStream;
		private uint m_FrameRate;
		private int m_FrameCount;
		private int m_Width;
		private int m_Height;

		private int m_Stride;

		public int FrameCount { get { return m_FrameCount; } }

		public bool Open( string fileName, int frameRate, int width, int height ) 
		{
			m_FrameRate = (uint)frameRate;
			m_Width = width;
			m_Height = height;
			m_Stride = width * 3;
			
			AVIFileInit();

			if ( AVIFileOpen( ref m_hFile, fileName, 4097 /* OF_WRITE | OF_CREATE (winbase.h) */, 0 ) != 0 )
				return false;
			else
				return CreateStream() && SetOptions();
		}

		public bool AddFrame( Bitmap bmp ) 
		{
			BitmapData bmpDat = bmp.LockBits( new Rectangle( 0, 0, m_Width, m_Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb );
			int hr = AVIStreamWrite(m_pCompStream, m_FrameCount, 1, bmpDat.Scan0, bmpDat.Stride  * bmpDat.Height, 0x10, 0, 0); // AVIIF_KEYFRAME = 0x10
			bmp.UnlockBits( bmpDat );

			if (hr != 0) 
				return false;
			m_FrameCount ++;
			return true;
		}

		public void Close() 
		{
			AVIStreamRelease(m_pStream);
			AVIStreamRelease(m_pCompStream);

			AVIFileRelease(m_hFile);
			AVIFileExit();
		}

		private bool CreateStream() 
		{
			AVISTREAMINFO strhdr         = new AVISTREAMINFO();
			strhdr.fccType               = mmioFOURCC('v','i','d','s');
			strhdr.fccHandler            = mmioFOURCC('I','V','5','0'); // CVID? IV50?
			strhdr.dwScale               = 1;
			strhdr.dwRate                = m_FrameRate; // fps
			strhdr.dwSuggestedBufferSize = (uint)(m_Height * m_Stride);
			strhdr.dwQuality             = -1; // Use default
			strhdr.dwSampleSize          = 0;
			strhdr.rcFrameTOP            = 0;
			strhdr.rcFrameLEFT           = 0;
			strhdr.rcFrameBOTTOM         = m_Height;
			strhdr.rcFrameRIGHT          = m_Width;
			strhdr.szName				 = new byte[64];

			return AVIFileCreateStream( m_hFile, out m_pStream, ref strhdr ) == 0;
		}

		unsafe private bool SetOptions() 
		{
			int hr;
			AVICOMPRESSOPTIONS opts = new AVICOMPRESSOPTIONS();
			AVICOMPRESSOPTIONS *pOpts = &opts;
			/*
			opts.fccType           = mmioFOURCC('v','i','d','s');
			opts.fccHandler        = 0;//cvid? iv50?
			opts.dwQuality         = 0;  // 0 .. 10000
			opts.dwFlags           = 0;  // AVICOMRPESSF_KEYFRAMES = 4
			*/

			hr = AVISaveOptions( Engine.MainWindow.Handle, 3, 1, ref m_pStream, &pOpts );
			if (hr != 1) 
				return false;

			// TODO: AVISaveOptionsFree(...)
    
			hr = AVIMakeCompressedStream( out m_pCompStream, m_pStream, ref opts, 0 );
			if (hr != 0) 
				return false;

			BITMAPINFOHEADER bi = new BITMAPINFOHEADER();
			bi.biSize         = 40;
			bi.biWidth        = m_Width;
			bi.biHeight       = m_Height;
			bi.biPlanes       = 1;
			bi.biBitCount     = 24;
			bi.biCompression  = 0;  // 0 = BI_RGB
			bi.biSizeImage    = (uint)(m_Width*m_Height*(bi.biBitCount/8));

			hr = AVIStreamSetFormat( m_pCompStream, 0, ref bi, 40 );
			return hr == 0;
		}

		[DllImport("avifil32.dll")]
		private static extern void AVIFileInit();

		[DllImport("avifil32.dll")]
		private static extern int AVIFileOpen( ref int pFile, string fileName, int flags, int dummy );

		[DllImport("avifil32.dll")]
		private static extern int AVIFileCreateStream( int pFile, out IntPtr pAvi, ref AVISTREAMINFO pStrmInfo ); 

		[DllImport("avifil32.dll")]
		private static extern int AVIMakeCompressedStream( out IntPtr ppsCompressed, IntPtr aviStream, ref AVICOMPRESSOPTIONS aco, int dummy );

		[DllImport("avifil32.dll")]
		private static extern int AVIStreamSetFormat(IntPtr aviStream, int lPos, ref BITMAPINFOHEADER lpFormat, int cbFormat);

		[DllImport("avifil32.dll")]
		unsafe private static extern int AVISaveOptions( IntPtr hwnd, uint flags, int nStreams, ref IntPtr pStream, AVICOMPRESSOPTIONS** aco);

		[DllImport("avifil32.dll")]
		private static extern int AVIStreamWrite(IntPtr aviStream, int lStart, int lSamples, IntPtr lpBuffer, int cbBuffer, int dwFlags, int dummy1, int dummy2);

		[DllImport("avifil32.dll")]
		private static extern int AVIStreamRelease( IntPtr aviStream );

		[DllImport("avifil32.dll")]
		private static extern int AVIFileRelease( int pFile );

		[DllImport("avifil32.dll")]
		private static extern void AVIFileExit();
  
		private static uint mmioFOURCC(char ch0, char ch1, char ch2, char ch3)
		{
			return (uint)( ((byte)ch0) | ( ((byte)ch1) << 8 ) | ( ((byte)ch2) << 16 ) | ( ((byte)ch3) << 24 ) );
		}

		[StructLayout(LayoutKind.Sequential, Pack=1)]
		private struct AVISTREAMINFO
		{
			public uint fccType;
			public uint fccHandler;
			public uint dwFlags; // Contains AVITF_* flags
			public uint dwCaps;
			public ushort wPriority;
			public ushort wLanguage;
			public uint dwScale;
			public uint dwRate;  // dwRate / dwScale == samples/second
			public uint dwStart;
			public uint dwLength;
			public uint dwInitialFrames;
			public uint dwSuggestedBufferSize;
			public int dwQuality;
			public uint dwSampleSize;
			public int rcFrameLEFT;
			public int rcFrameTOP;
			public int rcFrameRIGHT;
			public int rcFrameBOTTOM;
			public uint dwEditCount;
			public uint dwFormatChangeCount;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst=64)]
			public byte[] szName;
		}

		// vfw.h
		[StructLayout(LayoutKind.Sequential, Pack=1)]
		private struct AVICOMPRESSOPTIONS 
		{
			public uint fccType;
			public uint fccHandler;
			public uint dwKeyFrameEvery;  // only used with AVICOMRPESSF_KEYFRAMES
			public uint dwQuality;
			public uint dwBytesPerSecond; // only used with AVICOMPRESSF_DATARATE
			public uint dwFlags;
			public IntPtr lpFormat;
			public uint cbFormat;
			public IntPtr lpParms;
			public uint cbParms;
			public uint dwInterleaveEvery;
		}

		[StructLayout(LayoutKind.Sequential, Pack=1)]
		private struct BITMAPINFOHEADER 
		{
			public uint biSize;
			public int biWidth;
			public int biHeight;
			public short biPlanes;
			public short biBitCount;
			public uint biCompression;
			public uint biSizeImage;
			public int biXPelsPerMeter;
			public int biYPelsPerMeter;
			public uint biClrUsed;
			public uint biClrImportant;
		}
	}
}
