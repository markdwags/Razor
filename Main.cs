using System;
using System.Reflection;
using System.Threading;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Assistant
{
	public class Engine
	{
		private static void CurrentDomain_UnhandledException( object sender, UnhandledExceptionEventArgs e )
		{
			if ( e.IsTerminating )
			{
				ClientCommunication.Close();
				m_Running = false;

				new MessageDialog( "Unhandled Exception", !e.IsTerminating, e.ExceptionObject.ToString() ).ShowDialog( Engine.ActiveWindow );
			}

			LogCrash( e.ExceptionObject as Exception );
		}

		public static void LogCrash( object exception )
		{
			if ( exception == null || ( exception is ThreadAbortException ) )
				return;

			using ( StreamWriter txt = new StreamWriter( "Crash.log", true ) )
			{
				txt.AutoFlush = true;
				txt.WriteLine( "Exception @ {0}", DateTime.Now.ToString( "MM-dd-yy HH:mm:ss.ffff" ) );
				txt.WriteLine( exception.ToString() );
				txt.WriteLine( "" );
				txt.WriteLine( "" );
			}
		}

		public static string ExePath{ get{ return Process.GetCurrentProcess().MainModule.FileName; } }
		public static string BaseDirectory{ get{ return m_BaseDir; } }
		public static MainForm MainWindow{ get{ return m_MainWnd; } }
		public static bool Running{ get{ return m_Running; } }
		public static Form ActiveWindow{ get{ return m_ActiveWnd; } set{ m_ActiveWnd = value; } }
		
		public static string Version 
		{ 
			get
			{ 
				if ( m_Version == null )
				{
					Version v = Assembly.GetCallingAssembly().GetName().Version;
					m_Version = String.Format( "{0}.{1}.{2}", v.Major, v.Minor, v.Build );//, v.Revision
				}

				return m_Version; 
			}
		}

		private static MainForm m_MainWnd;
		private static Form m_ActiveWnd;
		//private static Thread m_TimerThread;
		private static bool m_Running;
		private static string m_BaseDir;
		private static string m_Version;

		[STAThread]
		public static void Main( string[] Args ) 
		{
			m_Running = true;
#if !DEBUG
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler( CurrentDomain_UnhandledException );
#endif
			Thread.CurrentThread.Name = "Razor Main Thread";

#if DEBUG
			// dont use the registry in debug mode (use the files in our working directory)
			m_BaseDir = Directory.GetCurrentDirectory();
#else
			m_BaseDir = Config.BaseDirectory;
			Directory.SetCurrentDirectory( m_BaseDir );
#endif
			if ( ClientCommunication.InitializeLibrary( Engine.Version ) == 0 )
				throw new InvalidOperationException( "This Razor installation is corrupted." );

			bool patch = Utility.ToInt32( Config.GetRegString( Microsoft.Win32.Registry.CurrentUser, "PatchEncy" ), 1 ) != 0;
			bool showWelcome = Utility.ToInt32( Config.GetRegString( Microsoft.Win32.Registry.CurrentUser, "ShowWelcome" ), 1 ) != 0;
			ClientLaunch launch = ClientLaunch.TwoD;
			int attPID = -1;
			string dataDir;

			ClientCommunication.ClientEncrypted = false;

			// check if the new ServerEncryption option is in the registry yet
			dataDir = Config.GetRegString( Microsoft.Win32.Registry.CurrentUser, "ServerEnc" );
			if ( dataDir == null )
			{
				// if not, add it (copied from UseOSIEnc)
				dataDir = Config.GetRegString( Microsoft.Win32.Registry.CurrentUser, "UseOSIEnc" );
				if ( dataDir == "1" )
				{
					ClientCommunication.ServerEncrypted = true;
					Config.SetRegString( Microsoft.Win32.Registry.CurrentUser, "ServerEnc", "1" );
				}
				else
				{
					Config.SetRegString( Microsoft.Win32.Registry.CurrentUser, "ServerEnc", "0" );
					ClientCommunication.ServerEncrypted = false;
				}

				Config.SetRegString( Microsoft.Win32.Registry.CurrentUser, "PatchEncy", "1" ); // reset the patch encryption option to TRUE
				patch = true;

				Config.DeleteRegValue( Microsoft.Win32.Registry.CurrentUser, "UseOSIEnc" ); // delete the old value
			}
			else
			{
				ClientCommunication.ServerEncrypted = Utility.ToInt32( dataDir, 0 ) != 0;
			}
			dataDir = null;
			
			for (int i=0;i<Args.Length;i++)
			{
				string arg = Args[i].ToLower();
				if ( arg == "--nopatch" )
				{
					patch = false;
				}
				else if ( arg == "--clientenc" )
				{
					ClientCommunication.ClientEncrypted = true;
					patch = false;
				}
				else if ( arg == "--serverenc" )
				{
					ClientCommunication.ServerEncrypted = true;
				}
				else if ( arg == "--welcome" )
				{
					showWelcome = true;
				}
				else if ( arg == "--pid" && i+1 < Args.Length )
				{
					i++;
					patch = false;
					attPID = Utility.ToInt32( Args[i], 0 );
				}
				else if ( arg.Substring( 0, 5 ) == "--pid" && arg.Length > 5 ) //support for uog 1.8 (damn you fixit)
				{
					patch = false;
					attPID = Utility.ToInt32( arg.Substring(5), 0 );
				}
				else if ( arg == "--uodata" && i+1 < Args.Length )
				{
					i++;
					dataDir = Args[i];
				}
			}

			if ( !Language.Load( "ENU" ) )
			{
				MessageBox.Show( "Fatal Error: Unable to load required file Language/Razor_lang.enu\nRazor cannot continue.", "No Language Pack", MessageBoxButtons.OK, MessageBoxIcon.Stop );
				return;
			}

			string defLang = Config.GetRegString( Microsoft.Win32.Registry.CurrentUser, "DefaultLanguage" );
			if ( defLang != null && !Language.Load( defLang ) )
				MessageBox.Show( String.Format( "WARNING: Razor was unable to load the file Language/Razor_lang.{0}\nENU will be used instead.", defLang ), "Language Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning );
			
			string clientPath = "";

			// welcome only needed when not loaded by a launcher (ie uogateway)
			if ( attPID == -1 )
			{
				if ( !showWelcome )
				{
					int cli = Utility.ToInt32( Config.GetRegString( Microsoft.Win32.Registry.CurrentUser, "DefClient" ), 0 );
					if ( cli < 0 || cli > 1 )
					{
						launch = ClientLaunch.Custom;
						clientPath = Config.GetRegString( Microsoft.Win32.Registry.CurrentUser, String.Format( "Client{0}", cli - 1 ) );
						if ( clientPath == null || clientPath == "" )
							showWelcome = true;
					}
					else
					{
						launch = (ClientLaunch)cli;
					}
				}

				if ( showWelcome )
				{
					WelcomeForm welcome = new WelcomeForm();
					m_ActiveWnd = welcome;
					if ( welcome.ShowDialog() == DialogResult.Cancel )
						return;
					patch = welcome.PatchEncryption;
					launch = welcome.Client;
					dataDir = welcome.DataDirectory;
					if ( launch == ClientLaunch.Custom )
						clientPath = welcome.ClientPath;
				}			
			}

			if ( dataDir != null && Directory.Exists( dataDir ) )
				Ultima.Client.Directories.Insert( 0, dataDir );

			Language.LoadCliLoc();
			
			SplashScreen.Start();
			m_ActiveWnd = SplashScreen.Instance;

			string update = Config.GetRegString( Microsoft.Win32.Registry.CurrentUser, "UpdateCheck" );
			if ( update != "N" )
			{
				bool doit = false;
				if ( update != "E" )
				{
					string now = DateTime.Now.ToString( "M/dd/yy" );
					if ( update != now )
					{
						doit = true;
						Config.SetRegString( Microsoft.Win32.Registry.CurrentUser, "UpdateCheck", now );
					}
				}
				else
				{
					doit = true;
				}

				if ( doit )
					new Thread( new ThreadStart( CheckVersion ) ).Start();
			}

			SplashScreen.Message = "Initializing...";

			//m_TimerThread = new Thread( new ThreadStart( Timer.TimerThread.TimerMain ) );
			//m_TimerThread.Name = "Razor Timers";

			Initialize( typeof( Assistant.Engine ).Assembly ); //Assembly.GetExecutingAssembly()

			SplashScreen.Message = "Loading last used profile...";
			Config.LoadCharList();
			if ( !Config.LoadLastProfile() )
				MessageBox.Show( SplashScreen.Instance, "The selected profile could not be loaded, using default instead.", "Profile Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning );

			if ( attPID == -1 )
			{
				SplashScreen.Message = "Loading client...";
				bool result = false;
				// both clients are POS, wont launch if current directory is not its home dir...   work around here
				if ( launch == ClientLaunch.TwoD )
				{
					//result = ClientCommunication.LaunchClient( UOGLite2.ClientType.Regular );
					if ( !result )
						clientPath = Ultima.Client.GetFilePath( "client.exe" );
				}
				else if ( launch == ClientLaunch.ThirdDawn )
				{
					//result = ClientCommunication.LaunchClient( UOGLite2.ClientType.ThirdDawn );
					if ( !result )
						clientPath = Ultima.Client.GetFilePath( "uotd.exe" );
				}

				if ( !result && clientPath != null && File.Exists( clientPath ) )
					result = ClientCommunication.LaunchClient( clientPath );

				if ( !result )
				{
					MessageBox.Show( SplashScreen.Instance, String.Format( "Razor.exe/UOG.dll: Unable to find the client specified.\n{0}: \"{1}\"", launch.ToString(), clientPath != null ? clientPath : "-null-" ), "Could Not Start Client", MessageBoxButtons.OK, MessageBoxIcon.Stop );
					SplashScreen.End();
					return;
				}

				if ( patch )
				{
					SplashScreen.Message = "Patching encryption...";
					if ( !ClientCommunication.PatchEncryption() )
					{
						MessageBox.Show( SplashScreen.Instance, "Razor.exe/UOG.dll: Unable to patch the client's encryption.\nYour client may not be compatible with this version of Razor(UOG.dll).\nConsult the documentation for more info.", "Can't Patch Client", MessageBoxButtons.OK, MessageBoxIcon.Stop );
						SplashScreen.End();
						return;
					}
				}

				//ClientCommunication.PreLaunchPatch();

				ClientCommunication.ResumeClient();
			}
			else
			{
				string error = "Error attaching to the UO client.";
				bool result = false;
				try
				{
					result = ClientCommunication.Attach( attPID );
				}
				catch ( Exception e )
				{
					result = false;
					error = e.Message;
				}

				if ( !result )
				{
					MessageBox.Show( SplashScreen.Instance, String.Format( "{1}\nThe specified PID '{0}' may be invalid.", attPID, error ), "Attach Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
					SplashScreen.End();
					return;
				}
			}

			if ( Utility.Random(2) == 0 )
				SplashScreen.Message = "Waiting for client to start...";
			else
				SplashScreen.Message = "Don't forget to Donate!!";

			m_MainWnd = new MainForm();
			//SplashScreen.End();
			//m_TimerThread.Start();
			Application.Run( m_MainWnd );
			
			m_Running = false;
			ClientCommunication.Close();
			Counter.Save();
			Macros.MacroManager.Save();
			Config.Save();

			/*try
			{
				if ( m_TimerThread.IsAlive )
					m_TimerThread.Abort();
			}
			catch
			{
			}*/
		}

		public static string GetDirectory( string relPath )
		{
			string path = Path.Combine( BaseDirectory, relPath );
			EnsureDirectory( path );
			return path;
		}

		public static void EnsureDirectory( string dir )
		{
			if ( !Directory.Exists( dir ) )
				Directory.CreateDirectory( dir );
		}

		private static void Initialize( Assembly a )
		{
			Type[] types = a.GetTypes();

			for (int i=0;i<types.Length;i++)
			{
				MethodInfo init = types[i].GetMethod( "Initialize", BindingFlags.Static | BindingFlags.Public );

				if ( init != null )
					init.Invoke( null, null );
			}
		}

		private static void CheckVersion()
		{
			int uid = 0;
			try
			{
				string str = Config.GetRegString( Microsoft.Win32.Registry.LocalMachine, "UId" );
				if ( str == null || str.Length <= 0 )
					str = Config.GetRegString( Microsoft.Win32.Registry.CurrentUser, "UId" );

				if ( str != null && str.Length > 0 )
					uid = Convert.ToInt32( str, 16 );
			}
			catch
			{
				uid = 0;
			}
			
			if ( uid == 0 )
			{
				try
				{
					uid = Utility.Random( int.MaxValue - 1 );
					if ( !Config.SetRegString( Microsoft.Win32.Registry.LocalMachine, "UId", String.Format( "{0:x}", uid ) ) )
					{
						if ( !Config.SetRegString( Microsoft.Win32.Registry.CurrentUser, "UId", String.Format( "{0:x}", uid ) ) )
							uid = 0;
					}
				}
				catch
				{
					uid = 0;
				}
			}
			
			try
			{
				WebRequest req = WebRequest.Create( String.Format( "http://www.runuo.com/razor/version.php?id={0}", uid ) );

				using ( StreamReader reader = new StreamReader( req.GetResponse().GetResponseStream() ) )
				{
					Version newVer = new Version( reader.ReadToEnd().Trim() );
					Version v = Assembly.GetCallingAssembly().GetName().Version;
					if ( v.CompareTo( newVer ) < 0 ) // v < newVer
					{
						if ( MessageBox.Show( Language.GetString( LocString.NewerVersion ), "New Razor Version", MessageBoxButtons.YesNo, MessageBoxIcon.Information ) == DialogResult.Yes )
						{
							/*
							string args = String.Format( "{0} Razor.exe", v.ToString() );//, Process.GetCurrentProcess().ProcessName );
							Process.Start( "Patcher.exe", args );
							Process.GetCurrentProcess().Kill();
							*/
							MainForm.LaunchBrowser( "http://www.runuo.com/razor/" );
						}
					}
				}
			}
			catch //( Exception e )
			{
				//LogCrash( e );
			}
		}
	}
}

