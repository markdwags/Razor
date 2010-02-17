using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace Assistant
{
	public class ScreenCapManager
	{
		[System.Runtime.InteropServices.DllImport( "Gdi32.dll" )]
		private static extern IntPtr DeleteObject( IntPtr hGdiObj );

		private static TimerCallback m_DoCaptureCall = new TimerCallback( CaptureNow );
		public static void Initialize()
		{
			HotKey.Add( HKCategory.Misc, LocString.TakeSS, new HotKeyCallback( CaptureNow ) );
		}

		public static void DeathCapture()
		{
			if ( !PacketPlayer.Playing ) // dont take SS of RPV deaths
				Timer.DelayedCallback( TimeSpan.FromSeconds( 0.5 ), m_DoCaptureCall ).Start();
		}

		public static void CaptureNow()
		{
			string filename;
			string timestamp;
			string name = "Unknown";
			string path = Config.GetString( "CapPath" );
			string type = Config.GetString( "ImageFormat" ).ToLower();
				
			if ( World.Player != null )
				name = World.Player.Name;
			if ( name == null || name.Trim() == "" || name.IndexOfAny( Path.GetInvalidPathChars() ) != -1 )
				name = "Unknown";
			
			if ( Config.GetBool( "CapTimeStamp" ) )
				timestamp = String.Format( "{0} ({1}) - {2}", name, World.ShardName, DateTime.Now.ToString( @"M/dd/yy - HH:mm:ss" ) );
			else
				timestamp = "";

			name = String.Format( "{0}_{1}", name, DateTime.Now.ToString( "M-d_HH.mm" ) );
			try
			{
				Engine.EnsureDirectory( path );
			}
			catch
			{
				try 
				{
					path = Engine.GetDirectory( "ScreenShots" );
					Config.SetProperty( "CapPath", path );
				}
				catch
				{
					path = "";
				}
			}

			int count  = 0;
			do
			{
				filename = Path.Combine( path, String.Format( "{0}{1}.{2}", name, count != 0 ? count.ToString() : "" , type ) );
				count--; // cause a - to be put in front of the number 
			}
			while ( File.Exists( filename ) );
			
			try
			{
				IntPtr hBmp = ClientCommunication.CaptureScreen( Config.GetBool( "CapFullScreen" ), timestamp );
				using ( Image img = Image.FromHbitmap( hBmp ) )
					img.Save( filename, GetFormat( type ) );
				DeleteObject( hBmp );
			}
			catch
			{
			}

			Engine.MainWindow.ReloadScreenShotsList();
		} 

		private static ImageFormat GetFormat( string fmt )
		{
			//string fmt = Config.GetString( "ImageFormat" ).ToLower();
			if ( fmt == "jpeg" || fmt == "jpg" )
				return ImageFormat.Jpeg;
			else if ( fmt == "png" )
				return ImageFormat.Png;
			else if ( fmt == "bmp" )
				return ImageFormat.Bmp;
			else if ( fmt == "gif" )
				return ImageFormat.Gif;
			else if ( fmt == "tiff" || fmt == "tif" )
				return ImageFormat.Tiff;
			else if ( fmt == "wmf" )
				return ImageFormat.Wmf;
			else if ( fmt == "exif" )
				return ImageFormat.Exif;
			else if ( fmt == "emf" )
				return ImageFormat.Emf;
			else
				return ImageFormat.Jpeg;
		}

		public static void DisplayTo( ListBox list )
		{
			string path = Config.GetString( "CapPath" );
			Engine.EnsureDirectory( path );

			//list.BeginUpdate();
			list.Items.Clear();

			AddFiles( list, path, "jpeg" );
			AddFiles( list, path, "jpg" );
			AddFiles( list, path, "png" );
			AddFiles( list, path, "bmp" );
			AddFiles( list, path, "gif" );
			AddFiles( list, path, "tiff" );
			AddFiles( list, path, "tif" );
			AddFiles( list, path, "wmf" );
			AddFiles( list, path, "exif" );
			AddFiles( list, path, "emf" );
			//list.EndUpdate();
		}

		public static void AddFiles( ListBox list, string path, string ext )
		{
			if ( list.Items.Count >= 500 )
				return;

			string[] files = Directory.GetFiles( path, String.Format( "*.{0}", ext ) );
			for (int i=0;i<files.Length && list.Items.Count < 500;i++)
				list.Items.Add( Path.GetFileName( files[i] ) );
		}
	}
}

