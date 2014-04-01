using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace Updater
{
	public enum UpdaterStatus
	{
		Incomplete,
		Success,
		Failure,
	}

	public class UpdaterMain
	{
		private static UpdaterStatus _Status = UpdaterStatus.Incomplete;
		public static UpdaterStatus Status { get { return _Status; } set { _Status = value; } }

		public static Version UpdateVersion { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler( CurrentDomain_UnhandledException );

			Mutex instanceMutex = new Mutex( false, "RazorUpdaterMutex" );
            
			if ( !instanceMutex.WaitOne( 1, true ) )
			{
				MessageBox.Show( "Another instance of Razor Updater is running.  Please wait for that instance to close before running the Updater again.", "Already Running" );
				return;
			}

			for ( Process[] processes = Process.GetProcessesByName( "Razor.exe" ); 
				processes.Length > 0; 
				processes = Process.GetProcessesByName( "Razor.exe" ) )
			{
				Thread.Sleep( 50 );
			}

			for (int i = 0; i < args.Length; i++) {
				if (!String.IsNullOrEmpty(args[i]))
					UpdateVersion = new Version(args[i]);
			}

			Directory.SetCurrentDirectory(UpdaterMain.BaseDirectory);

			Application.EnableVisualStyles();
			Application.Run( new Updater() );

			instanceMutex.ReleaseMutex();
			instanceMutex.Close();

			if ( Status == UpdaterStatus.Success )
				Process.Start( "Razor.exe" );
			Process.GetCurrentProcess().Kill();
		}

        private static void CurrentDomain_UnhandledException( object sender, UnhandledExceptionEventArgs e )
        {
			if ( !(e.ExceptionObject is Exception) || e.ExceptionObject is System.Threading.ThreadAbortException )
				return;

            Logger.Log( ((Exception)e.ExceptionObject).ToString() );
        }

		private const string RazorRegPath = @"SOFTWARE\Razor";
		public static string GetRegString( Microsoft.Win32.RegistryKey hkey, string vname )
		{
			try
			{
				RegistryKey key = hkey.OpenSubKey( RazorRegPath ) ;
				if ( key == null )
				{
					key = hkey.CreateSubKey( RazorRegPath );
					if ( key == null )
						return null;
				}

				string v = key.GetValue( vname ) as string;

				if ( v == null )
					return null;
				return v.Trim();
			}
			catch
			{
				return null;
			}
		}

		public static string BaseDirectory
		{
			get
			{
				string inst = "";

				try
				{
					inst = GetRegString( Microsoft.Win32.Registry.LocalMachine, "InstallDir" );
					if ( inst == null || inst == "" )
						inst = GetRegString( Microsoft.Win32.Registry.CurrentUser, "InstallDir" );

					if ( inst == null || inst == "" )
						inst = Directory.GetCurrentDirectory();
				}
				catch
				{
				}

				if ( inst == null )
					inst = "";
				
				return inst;
			}
		}
    }
}