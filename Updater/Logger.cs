using System;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace Updater
{
    public class Logger
    {
        private static FileStream _File;
		private static StreamWriter _Writer;
        
        public static FileStream File { get { return _File; } }
		public static StreamWriter Writer { get { return _Writer; } }

		static Logger()
		{
			_File = new FileStream( "Updater.log", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite );
			_File.Seek( 0, SeekOrigin.End );

			_Writer = new StreamWriter( _File );
			_Writer.AutoFlush = true;

			_Writer.WriteLine( "" );
			_Writer.WriteLine( "" );
			_Writer.WriteLine( "[{3} ({0}/{1}/{2:D2})]: Logging started", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year%100, GetTimeStamp() );
		}

        private static string GetTimeStamp()
		{
			return DateTime.Now.ToString( "HH:mm:ss.ffff" );
		}

        public static void Log(string text)
        {
			lock ( Writer )
			{
				Writer.WriteLine( "[{0}]: {1}", GetTimeStamp(), text );
				Writer.Flush();
			}
        }

		public static void Log( string format, params object[] args )
		{
			Log( String.Format( format, args ) );
		}
    }
}
