using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace Updater
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Updater : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtChangeLog;
		private System.Windows.Forms.ProgressBar progress;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Timer closeTimer;
		private System.ComponentModel.IContainer components;

		public Updater()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </s;ummary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Updater));
			this.txtChangeLog = new System.Windows.Forms.TextBox();
			this.progress = new System.Windows.Forms.ProgressBar();
			this.lblStatus = new System.Windows.Forms.Label();
			this.btnExit = new System.Windows.Forms.Button();
			this.closeTimer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// txtChangeLog
			// 
			this.txtChangeLog.Location = new System.Drawing.Point(4, 4);
			this.txtChangeLog.Multiline = true;
			this.txtChangeLog.Name = "txtChangeLog";
			this.txtChangeLog.ReadOnly = true;
			this.txtChangeLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtChangeLog.Size = new System.Drawing.Size(436, 176);
			this.txtChangeLog.TabIndex = 0;
			this.txtChangeLog.Text = "Fetching changelog...";
			// 
			// progress
			// 
			this.progress.Location = new System.Drawing.Point(4, 184);
			this.progress.Name = "progress";
			this.progress.Size = new System.Drawing.Size(436, 16);
			this.progress.Step = 1;
			this.progress.TabIndex = 1;
			// 
			// lblStatus
			// 
			this.lblStatus.Location = new System.Drawing.Point(4, 208);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(352, 16);
			this.lblStatus.TabIndex = 2;
			this.lblStatus.Text = "Connecting...";
			// 
			// btnExit
			// 
			this.btnExit.Location = new System.Drawing.Point(364, 204);
			this.btnExit.Name = "btnExit";
			this.btnExit.TabIndex = 3;
			this.btnExit.Text = "&Cancel";
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// closeTimer
			// 
			this.closeTimer.Enabled = true;
			this.closeTimer.Interval = 1000;
			this.closeTimer.Tick += new System.EventHandler(this.closeTimer_Tick);
			// 
			// Updater
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(444, 232);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.progress);
			this.Controls.Add(this.txtChangeLog);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Updater";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Updating Razor...";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Updater_Closing);
			this.Load += new System.EventHandler(this.Updater_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private const string UpdateRAR = "https://zenvera.com/razor/Razor_Latest.rar";
		private const string ChangelogRAR = "https://zenvera.com/razor/changelog.txt";
		private Downloader _Downloader;

		private void Updater_Load(object sender, System.EventArgs e)
		{
			closeTimer.Stop();
			
			_Downloader = new Downloader( UpdateRAR, "Update.rar", 
				ChangelogRAR,
				new ProgressChange( OnProgressChange ), new ConnectionFailed( OnConnectionFailed ), new OperationComplete( OnDownloadComplete ),
				new MessageDownloaded( OnChangelogDownloaded ) );

			UpdateStatus( "Downloading changelog..." );
			_Downloader.Download();

			txtChangeLog.Text = "(Fetching Changelog...)";
			txtChangeLog.Focus();
			txtChangeLog.Select( 0, 0 );
		}

		private void UpdateFailed()
		{
			UpdaterMain.Status = UpdaterStatus.Failure;
			btnExit.Text = "&Close";
			_Downloader.Kill();
		}

		private void UpdateComplete()
		{
			UpdaterMain.Status = UpdaterStatus.Success;
			closeTimer.Start();

			UpdateStatus( "Update Complete!" );

			btnExit.Text = "&Close";
		}

		private void UpdateStatus( string status )
		{
			lblStatus.Text = status;

			this.Activate();
		}

		private void OnChangelogDownloaded( string log )
		{
			log = log.Replace( "\n", "\r\n" );
			log = log.Replace( "\r\r", "\r" );

			txtChangeLog.Text = log;
			txtChangeLog.Select( 0, 0 );

			UpdateStatus( "Downloading update..." );
		}

		private void OnProgressChange( int percent )
		{
			progress.Value = percent; 
		}

		private void OnConnectionFailed()
		{
			Logger.Log( "Connection failed!" );
			UpdateStatus( "ERROR: Failed to connect to update server!" );
			UpdateFailed();
		}

		private void OnDownloadComplete()
		{
			if( File.Exists( "Update.rar" ) )
			{
				ExtractFiles();
			}
			else
			{
				Logger.Log( "Update download failed!" );
				UpdateStatus( "ERROR: Failed to download update!" );
				UpdateFailed();
			}
		}

		private delegate void UpdateCompleteDelegate();
		private void ExtractFiles()
		{
			string tempPath = "Update.rar";
			if( File.Exists( tempPath ) )
			{
				UpdateStatus( "Extracting files..." );
                
				Unrar rar = new Unrar( tempPath );
				rar.DestinationPath = Directory.GetCurrentDirectory();
				rar.ExtractionProgress += new ExtractionProgressHandler( rar_ExtractionProgress );

				rar.Open();

				while( rar.ReadHeader() )
				{
					if( rar.CurrentFile.FileName == "Updater.exe" )
						rar.CurrentFile.FileName = "New_Updater.exe";
					else if ( rar.CurrentFile.FileName == "UnRar.dll" )
						rar.CurrentFile.FileName = "New_UnRar.dll";

					while ( File.Exists( rar.CurrentFile.FileName ) )
					{
						try 
						{
							File.Delete( rar.CurrentFile.FileName );
						}
						catch ( Exception e )
						{
							DialogResult res = MessageBox.Show( String.Format(
								"Unable to delete file \"{0}\" (Error: {1})\nMake sure all instances of Razor are closed.\nRazor may be unusable until the update completes successfully.", 
								rar.CurrentFile.FileName, e.Message ),
								"Access Denied",
								MessageBoxButtons.AbortRetryIgnore,
								MessageBoxIcon.Warning );

							if ( res == DialogResult.Abort )
							{
								UpdateStatus( "Update ABORTED!" );
								UpdateFailed();
								return;
							}
							else if ( res == DialogResult.Ignore )
							{
								break; // just blindly try anyways
							}
							else
							{
								continue; // retry
							}
						}
					}

					rar.Extract();
				}

				rar.Close();
			}

			this.Invoke( new UpdateCompleteDelegate( UpdateComplete ) );
		}

		private void rar_ExtractionProgress( object sender, ExtractionProgressEventArgs e )
		{
			OnProgressChange( (int)e.PercentComplete );
		}

		private int _CloseTimerCount = 30;
		private void closeTimer_Tick(object sender, System.EventArgs e)
		{
			_CloseTimerCount--;

			if ( _CloseTimerCount <= 0 )
				this.Close();
			else
				UpdateStatus( String.Format( "Update Complete! (Razor will restart in {0} seconds)", _CloseTimerCount ) );
		}

		private void Updater_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if ( UpdaterMain.Status == UpdaterStatus.Incomplete )
			{
				if ( MessageBox.Show( this, "The update has not completed!\nAre you sure you want to cancel the update?", "Cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.No )
				{
					e.Cancel = true;
					return;
				}
			}

			if ( _Downloader != null )
				_Downloader.Kill();
		}

		private void btnExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}
}

