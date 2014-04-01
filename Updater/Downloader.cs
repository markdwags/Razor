using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace Updater
{
	public delegate void ProgressChange( int progress );
	public delegate void ConnectionFailed();
	public delegate void OperationComplete();
	public delegate void MessageDownloaded( string message );

    public class Downloader
    {
        private string _Url;
        private string _Filename;
		private string _MsgUrl;
        private ProgressChange _ProgressCallback;
        private ConnectionFailed _FailedCallback;
        private OperationComplete _CompleteCallback;
		private MessageDownloaded _MessageDownloaded;
		private Thread _Thread;

        public Downloader( string url, string filename, string messageUrl,
            ProgressChange progressCallback, ConnectionFailed failedCallback,
            OperationComplete completeCallback, MessageDownloaded msgDone )
        {
            _Url = url;
            _Filename = filename;
			_MsgUrl = messageUrl;
            _ProgressCallback = progressCallback;
            _FailedCallback = failedCallback;
            _CompleteCallback = completeCallback;
			_MessageDownloaded = msgDone;
        }

        public void Download()
        {
            _Thread = new Thread( new ThreadStart( DownloadFile ) );
			_Thread.Start();
        }

		public void Kill()
		{
			if ( _Thread != null && _Thread.IsAlive )
			{
				try
				{
					_Thread.Join( 500 );
					if ( _Thread.IsAlive )
						_Thread.Abort();
					_Thread = null;
				}
				catch
				{
				}
			}
		}

        private void DownloadFile()
        {
            HttpWebRequest wr = null;
            HttpWebResponse ws = null;
            Stream s = null;
            FileStream fstr = null;

			ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };

			if ( _MessageDownloaded != null )
			{
				try
				{
					wr = ( HttpWebRequest )WebRequest.Create( _MsgUrl );
					wr.UserAgent = "Razor Updater";
					wr.Timeout = 5000;

					ws = ( HttpWebResponse )wr.GetResponse();
					if ( ws != null )
					{
						using ( StreamReader reader = new StreamReader( ws.GetResponseStream() ) )
						{
							_MessageDownloaded( reader.ReadToEnd() );
						}
					}
					else
					{
						_MessageDownloaded( "Unable to download changelog!" );
					}
				}
				catch ( Exception e )
				{
					Logger.Log( e.ToString() );

					_MessageDownloaded( String.Format( "{0}\r\nUnable to download changelog.", e.Message ) );
				}
				finally
				{
					if ( ws != null )
						ws.Close();
				}
			}

            try
            {
                wr = ( HttpWebRequest )WebRequest.Create( _Url );
				wr.UserAgent = "Razor Updater";
				wr.Timeout = 10000;
                ws = ( HttpWebResponse )wr.GetResponse();
				if( ws.StatusCode == HttpStatusCode.OK )
				{
					s = ws.GetResponseStream();
					fstr = new FileStream( _Filename, FileMode.Create, FileAccess.Write );

					byte[] inBuf = new byte[8192];
					long bytesToRead = ws.ContentLength;
					long totalSent = 0;

					BinaryReader str = new BinaryReader( s );
					while( totalSent < bytesToRead )
					{
						int toRead = ( int )Math.Min( ( long )inBuf.Length, ( bytesToRead - totalSent ) );

						inBuf = str.ReadBytes( toRead );
						fstr.Write( inBuf, 0, toRead );

						totalSent += toRead;

						if( _ProgressCallback != null )
							_ProgressCallback( (int)((100*totalSent)/bytesToRead) );
					}
				}
				else
				{
					throw new WebException( String.Format( "Request status code = {0}", ws.StatusCode ) );
				}
            }
            catch( Exception e )
            {
				Logger.Log( e.ToString() );

                if( _FailedCallback != null )
                    _FailedCallback();
            }
            finally
            {
                if( s != null )
                    s.Close();
                if( fstr != null )
                    fstr.Close();

                if( _CompleteCallback != null )
                    _CompleteCallback();
            }
        }
    }
}
