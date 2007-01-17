using System;
using System.Drawing;
using System.Collections;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using Microsoft.Win32;

namespace Assistant
{
	public enum ClientLaunch
	{
		TwoD,
		ThirdDawn,
		Custom,
	}

	public class WelcomeForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox clientList;
		private System.Windows.Forms.CheckBox patchEncy;
		private System.Windows.Forms.Button okay;
		private System.Windows.Forms.Button quit;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox serverList;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox port;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.OpenFileDialog openFile;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button browse;
		private System.Windows.Forms.Button makeDef;
		
		private System.Windows.Forms.CheckBox showAtStart;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox langSel;
		private System.Windows.Forms.CheckBox useEnc;
		private System.Windows.Forms.Button dataBrowse;
		private System.Windows.Forms.ComboBox dataDir;
		private System.Windows.Forms.GroupBox groupBox3;

		public string ClientPath{ get{ return m_ClientPath; } }
		public ClientLaunch Client{ get{ return m_Launch; } } 
		public bool PatchEncryption{ get{ return m_PatchEncy; } }
		public string DataDirectory{ get{ if ( m_DataDir == "" || m_DataDir == "(Auto Detect)" ) m_DataDir = null; return m_DataDir; } }

		private bool m_PatchEncy = false;
		private string m_ClientPath = "";
		private ClientLaunch m_Launch = ClientLaunch.Custom;
		private string m_DataDir = "";

		public WelcomeForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			this.label1 = new System.Windows.Forms.Label();
			this.clientList = new System.Windows.Forms.ComboBox();
			this.browse = new System.Windows.Forms.Button();
			this.patchEncy = new System.Windows.Forms.CheckBox();
			this.okay = new System.Windows.Forms.Button();
			this.quit = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.showAtStart = new System.Windows.Forms.CheckBox();
			this.serverList = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.port = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.useEnc = new System.Windows.Forms.CheckBox();
			this.makeDef = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.openFile = new System.Windows.Forms.OpenFileDialog();
			this.label5 = new System.Windows.Forms.Label();
			this.langSel = new System.Windows.Forms.ComboBox();
			this.dataBrowse = new System.Windows.Forms.Button();
			this.dataDir = new System.Windows.Forms.ComboBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(68, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Load Client:";
			// 
			// clientList
			// 
			this.clientList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.clientList.Location = new System.Drawing.Point(76, 16);
			this.clientList.Name = "clientList";
			this.clientList.Size = new System.Drawing.Size(192, 21);
			this.clientList.TabIndex = 1;
			// 
			// browse
			// 
			this.browse.Location = new System.Drawing.Point(274, 16);
			this.browse.Name = "browse";
			this.browse.Size = new System.Drawing.Size(64, 20);
			this.browse.TabIndex = 2;
			this.browse.Text = "Browse...";
			this.browse.Click += new System.EventHandler(this.browse_Click);
			// 
			// patchEncy
			// 
			this.patchEncy.Location = new System.Drawing.Point(8, 42);
			this.patchEncy.Name = "patchEncy";
			this.patchEncy.Size = new System.Drawing.Size(140, 20);
			this.patchEncy.TabIndex = 3;
			this.patchEncy.Text = "Patch client encryption";
			this.patchEncy.CheckedChanged += new System.EventHandler(this.patchEncy_CheckedChanged);
			// 
			// okay
			// 
			this.okay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okay.Location = new System.Drawing.Point(8, 228);
			this.okay.Name = "okay";
			this.okay.Size = new System.Drawing.Size(72, 20);
			this.okay.TabIndex = 6;
			this.okay.Text = "&Okay";
			this.okay.Click += new System.EventHandler(this.okay_Click);
			// 
			// quit
			// 
			this.quit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.quit.Location = new System.Drawing.Point(88, 228);
			this.quit.Name = "quit";
			this.quit.Size = new System.Drawing.Size(72, 20);
			this.quit.TabIndex = 7;
			this.quit.Text = "&Quit";
			this.quit.Click += new System.EventHandler(this.quit_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 20);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(44, 16);
			this.label3.TabIndex = 9;
			this.label3.Text = "Server:";
			// 
			// showAtStart
			// 
			this.showAtStart.Location = new System.Drawing.Point(180, 228);
			this.showAtStart.Name = "showAtStart";
			this.showAtStart.Size = new System.Drawing.Size(168, 20);
			this.showAtStart.TabIndex = 10;
			this.showAtStart.Text = "Show this when Razor starts";
			this.showAtStart.CheckedChanged += new System.EventHandler(this.showAtStart_CheckedChanged);
			// 
			// serverList
			// 
			this.serverList.Location = new System.Drawing.Point(52, 16);
			this.serverList.Name = "serverList";
			this.serverList.Size = new System.Drawing.Size(196, 21);
			this.serverList.TabIndex = 11;
			this.serverList.TextChanged += new System.EventHandler(this.serverList_TextChanged);
			this.serverList.SelectedIndexChanged += new System.EventHandler(this.serverList_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(260, 20);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(32, 16);
			this.label4.TabIndex = 12;
			this.label4.Text = "Port:";
			// 
			// port
			// 
			this.port.Location = new System.Drawing.Point(296, 16);
			this.port.Name = "port";
			this.port.Size = new System.Drawing.Size(40, 20);
			this.port.TabIndex = 13;
			this.port.Text = "";
			this.port.TextChanged += new System.EventHandler(this.port_TextChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.useEnc);
			this.groupBox1.Controls.Add(this.makeDef);
			this.groupBox1.Controls.Add(this.browse);
			this.groupBox1.Controls.Add(this.clientList);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.patchEncy);
			this.groupBox1.Location = new System.Drawing.Point(4, 4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(344, 84);
			this.groupBox1.TabIndex = 14;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Client Options";
			// 
			// useEnc
			// 
			this.useEnc.Location = new System.Drawing.Point(8, 58);
			this.useEnc.Name = "useEnc";
			this.useEnc.Size = new System.Drawing.Size(140, 20);
			this.useEnc.TabIndex = 5;
			this.useEnc.Text = "Use OSI Encryption";
			this.useEnc.CheckedChanged += new System.EventHandler(this.useEnc_CheckedChanged);
			// 
			// makeDef
			// 
			this.makeDef.Location = new System.Drawing.Point(178, 58);
			this.makeDef.Name = "makeDef";
			this.makeDef.Size = new System.Drawing.Size(160, 20);
			this.makeDef.TabIndex = 4;
			this.makeDef.Text = "Make These Settings Default";
			this.makeDef.Click += new System.EventHandler(this.makeDef_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.port);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.serverList);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Location = new System.Drawing.Point(4, 144);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(344, 48);
			this.groupBox2.TabIndex = 15;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Server";
			// 
			// openFile
			// 
			this.openFile.DefaultExt = "exe";
			this.openFile.FileName = "client.exe";
			this.openFile.Filter = "Executable Files|*.exe";
			this.openFile.Title = "Select Client";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 200);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(60, 16);
			this.label5.TabIndex = 17;
			this.label5.Text = "Language:";
			// 
			// langSel
			// 
			this.langSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.langSel.Location = new System.Drawing.Point(68, 196);
			this.langSel.Name = "langSel";
			this.langSel.Size = new System.Drawing.Size(56, 21);
			this.langSel.TabIndex = 18;
			this.langSel.SelectedIndexChanged += new System.EventHandler(this.langSel_SelectedIndexChanged);
			// 
			// dataBrowse
			// 
			this.dataBrowse.Location = new System.Drawing.Point(274, 20);
			this.dataBrowse.Name = "dataBrowse";
			this.dataBrowse.Size = new System.Drawing.Size(64, 20);
			this.dataBrowse.TabIndex = 21;
			this.dataBrowse.Text = "Browse...";
			this.dataBrowse.Click += new System.EventHandler(this.dataBrowse_Click);
			// 
			// dataDir
			// 
			this.dataDir.Location = new System.Drawing.Point(8, 20);
			this.dataDir.Name = "dataDir";
			this.dataDir.Size = new System.Drawing.Size(258, 21);
			this.dataDir.TabIndex = 22;
			this.dataDir.TextChanged += new System.EventHandler(this.dataDir_TextChanged);
			this.dataDir.SelectedIndexChanged += new System.EventHandler(this.dataDir_SelectedIndexChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.dataDir);
			this.groupBox3.Controls.Add(this.dataBrowse);
			this.groupBox3.Location = new System.Drawing.Point(4, 92);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(344, 48);
			this.groupBox3.TabIndex = 23;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "UO Data Directory";
			// 
			// WelcomeForm
			// 
			this.AcceptButton = this.okay;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.quit;
			this.ClientSize = new System.Drawing.Size(352, 256);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.langSel);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.showAtStart);
			this.Controls.Add(this.quit);
			this.Controls.Add(this.okay);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "WelcomeForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Welcome to Razor!";
			this.Load += new System.EventHandler(this.WelcomeForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private class ServerEntry
		{
			public ServerEntry( string addr, int p )
			{
				Address = addr;
				Port = p;
			}

			public string Address;
			public int Port;

			public override string ToString()
			{
				return Address;
			}
		}

		private class LoginCFG_SE : ServerEntry
		{
			public string RealAddress;
			public LoginCFG_SE() : base( "Use Last", 0 )
			{
				RealAddress = Config.GetRegString( Registry.CurrentUser, "LastServer" );
				Port = Utility.ToInt32( Config.GetRegString( Registry.CurrentUser, "LastPort" ), 0 );

				if ( RealAddress == null || RealAddress == "" || Port == 0 )
				{
					RealAddress = "";
					Port = 0;
				
					try 
					{
						string fileName = Ultima.Client.GetFilePath( "Login.cfg" );
						if ( fileName == null || fileName == "" )
							return;
						string server = null, port = null;

						if ( File.Exists( fileName ) )
						{
							using ( FileStream file = new FileStream( fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ) )
							using ( StreamReader cfg = new StreamReader( file ) )
							{
								string line;
								while ( (line = cfg.ReadLine()) != null )
								{
									line = line.Trim();
									if ( line != "" && Char.ToUpper( line[0] ) == 'L' && line.Length > 12 )
									{
										int comma = line.IndexOf( ',' );
										if ( comma > 12 )
										{
											server = line.Substring( 12, comma-12 );
											port = line.Substring( comma+1 );

											break;
										}
									}
								}
							}
						}

						if ( server != null )
						{
							Address = "(Use Last: "+server+")";
							RealAddress = server;
						}
						if ( port != null )
							Port = Utility.ToInt32( port, 0 );
					}
					catch
					{
						RealAddress = Address = "Use Last";
						Port = 0;
					}
				}
			}
		}

		private class UOGamers_SE : ServerEntry
		{
			public string RealAddress;
			public UOGamers_SE( string name, string addr ) : base( name, 0 )
			{
				RealAddress = addr;
			}
		}

		private class PathElipsis
		{
			private string m_Path;
			private string m_Show;
			public PathElipsis( string path )
			{
				m_Path = path;
				m_Show = GetPathElipsis( path, 23 );
			}

			public string GetPath(){return m_Path;}
			public void SetPath(string value){m_Path=value;m_Show=GetPathElipsis(m_Path, 23);}
			public override string ToString()
			{
				return m_Show;
			}

			private static char[] pathChars = new char[]{ Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar, Path.PathSeparator, Path.VolumeSeparatorChar };
			public static string GetPathElipsis( string path, int PathMaxLength )
			{
				if ( path.Length <= PathMaxLength )
					return path;

				System.Text.StringBuilder sb = new System.Text.StringBuilder( path );
				int remlen = path.Length - PathMaxLength - 3;

				int ls = path.LastIndexOfAny( pathChars );
				if ( ls == -1 )
					ls = 15 + remlen;

				if ( ls - remlen < 4 )
					ls = remlen+4;

				if ( ls > remlen && remlen > 0 )
				{
					try
					{
						sb.Remove( ls - remlen, remlen );
						sb.Insert( ls - remlen, "..." );
					}
					catch
					{
					}
				}
				return sb.ToString();
			}
		}

		private void WelcomeForm_Load(object sender, System.EventArgs e)
		{
			Language.LoadControlNames( this );

			this.BringToFront();

			langSel.Items.AddRange( Language.GetPackNames() );
			langSel.SelectedItem = Language.Current;

			showAtStart.Checked = Utility.ToInt32( Config.GetRegString( Microsoft.Win32.Registry.CurrentUser, "ShowWindow" ), 1 ) == 1;

			clientList.Items.Add( "2D Client (Auto-Detect)" );
			clientList.Items.Add( "3D Client (Auto-Detect)" );
			for (int i=1; ;i++)
			{
				string val = String.Format( "Client{0}", i );
				string cli = Config.GetRegString( Microsoft.Win32.Registry.CurrentUser, val );
				if ( cli == null || cli == "" )
					break;
				if ( File.Exists( cli )	)
					clientList.Items.Add( new PathElipsis( cli ) );
				Config.DeleteRegValue( Microsoft.Win32.Registry.CurrentUser, val );
			}
			int sel = Utility.ToInt32( Config.GetRegString( Microsoft.Win32.Registry.CurrentUser, "DefClient" ), 0 );
			if ( sel >= clientList.Items.Count )
			{
				sel = 0;
				Config.SetRegString( Microsoft.Win32.Registry.CurrentUser, "DefClient", "0" );
			}
			clientList.SelectedIndex = sel;

			dataDir.Items.Add( "(Auto Detect)" );
			for ( int i=1; ;i++)
			{
				string val = String.Format( "Dir{0}", i );
				string dir = Config.GetRegString( Microsoft.Win32.Registry.CurrentUser, val );
				if ( dir == null || dir == "" )
					break;
				if ( Directory.Exists( dir ) )
					dataDir.Items.Add( dir );
				Config.DeleteRegValue( Microsoft.Win32.Registry.CurrentUser, val );
			}

			try
			{
				dataDir.SelectedIndex = Convert.ToInt32( Config.GetRegString( Microsoft.Win32.Registry.CurrentUser, "LastDir" ) );
			}
			catch
			{
				dataDir.SelectedIndex = 0;
			}

			patchEncy.Checked = Utility.ToInt32( Config.GetRegString( Microsoft.Win32.Registry.CurrentUser, "PatchEncy" ), 1 ) != 0;
			useEnc.Checked = Utility.ToInt32( Config.GetRegString( Microsoft.Win32.Registry.CurrentUser, "ServerEnc" ), 0 ) != 0;
			
			LoginCFG_SE lse = new LoginCFG_SE();
			UOGamers_SE uog;

			serverList.BeginUpdate();

			//serverList.Items.Add( lse=new LoginCFG_SE() );
			//serverList.SelectedItem = lse;

			for (int i=1; ;i++)
			{
				ServerEntry se;
				string sval = String.Format( "Server{0}", i );
				string serv = Config.GetRegString( Microsoft.Win32.Registry.CurrentUser, sval );
				if ( serv == null )
					break;
				string pval = String.Format( "Port{0}", i );
				int port = Utility.ToInt32( Config.GetRegString( Microsoft.Win32.Registry.CurrentUser, pval ), 0 );
				serverList.Items.Add( se=new ServerEntry( serv, port ) );
				if ( serv == lse.RealAddress && port == lse.Port )
					serverList.SelectedItem = se;
				Config.DeleteRegValue( Microsoft.Win32.Registry.CurrentUser, sval );
				Config.DeleteRegValue( Microsoft.Win32.Registry.CurrentUser, pval );
			}

			/*
			serverList.Items.Add( uog=new UOGamers_SE( "UOGamers: Rebirth (PreT2A)", "login.uorebirth.com" ) );
			if ( lse.RealAddress == uog.RealAddress && lse.Port == 2593 )
				serverList.SelectedItem = uog;
			*/
			serverList.Items.Add( uog=new UOGamers_SE( "UOGamers: Divinity (T2A)", "login.uodivinity.com" ) );
			if ( lse.RealAddress == uog.RealAddress && lse.Port == 2593 )
				serverList.SelectedItem = uog;

			serverList.Items.Add( uog=new UOGamers_SE( "UOGamers: Hybrid (UOR)", "login.uogamers.com" ) );
			if ( lse.RealAddress == uog.RealAddress && lse.Port == 2593 )
				serverList.SelectedItem = uog;

			serverList.Items.Add( uog=new UOGamers_SE( "UOGamers: Demise (SE/ML)", "login.uodemise.com" ) );
			if ( lse.RealAddress == uog.RealAddress && lse.Port == 2593 )
				serverList.SelectedItem = uog;

			serverList.Items.Add( uog=new UOGamers_SE( "Electronic Arts/Origin Servers", "login.owo.com" ) );
			if ( lse.RealAddress == uog.RealAddress && ( lse.Port >= 7775 && lse.Port <= 7778 ) )
				serverList.SelectedItem = uog;

			serverList.EndUpdate();

			serverList.Refresh();

			WindowState = FormWindowState.Normal;
			this.BringToFront();
			this.TopMost = true;

			System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
			t.Interval = 500;
			t.Enabled = true;
			t.Tick += new EventHandler(timer_Tick);
		}
		
		private void timer_Tick(object sender, EventArgs e)
		{
			this.TopMost = false;
			this.BringToFront();
		}

		private void browse_Click(object sender, System.EventArgs e)
		{
			if ( openFile.ShowDialog( this ) == DialogResult.OK )
			{
				PathElipsis pe = new PathElipsis( openFile.FileName );
				clientList.Items.Add( pe );
				clientList.SelectedItem = pe;
			}
		}

		private void makeDef_Click(object sender, System.EventArgs e)
		{
			Config.SetRegString( Microsoft.Win32.Registry.CurrentUser, "DefClient", ( clientList.SelectedIndex >= 0 ? clientList.SelectedIndex : 0 ).ToString() );
			Config.SetRegString( Microsoft.Win32.Registry.CurrentUser, "PatchEncy", patchEncy.Checked ? "1" : "0" );
			Config.SetRegString( Microsoft.Win32.Registry.CurrentUser, "ServerEnc", useEnc.Checked ? "1" : "0" );
			MessageBox.Show( this, Language.GetString( LocString.SaveOK ), "Done", MessageBoxButtons.OK, MessageBoxIcon.Information );
		}

		private void serverList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			port.Enabled = !( serverList.SelectedItem is UOGamers_SE || serverList.SelectedItem is LoginCFG_SE );

			if ( serverList.SelectedItem != null )
			{
				if ( serverList.SelectedItem is LoginCFG_SE && ((ServerEntry)serverList.SelectedItem).Port == 0 )
					port.Text = "";
				else
					port.Text = ((ServerEntry)serverList.SelectedItem).Port.ToString();
			}
		}

		private void serverList_TextChanged(object sender, System.EventArgs e)
		{
			string txt = serverList.Text;
			if ( ( serverList.SelectedItem is UOGamers_SE || serverList.SelectedItem is LoginCFG_SE ) && txt != (serverList.SelectedItem).ToString() ) 
			{
				port.Text = "";
				serverList.BeginUpdate();
				serverList.SelectedIndex = -1;
				serverList.Text = txt;
				serverList.Select( txt.Length, 0 );
				serverList.EndUpdate();
			}
		}
		
		private void port_TextChanged(object sender, System.EventArgs e)
		{
			if ( port.Text != "" )
			{
				if ( ( serverList.SelectedItem is LoginCFG_SE && ((ServerEntry)serverList.SelectedItem).Port == 0 ) || serverList.SelectedItem is UOGamers_SE )
					port.Text = "";
				else if ( serverList.SelectedItem != null )
					((ServerEntry)serverList.SelectedItem).Port = Utility.ToInt32( port.Text, 0 );
			}
		}

		private void okay_Click(object sender, System.EventArgs e)
		{
			m_PatchEncy = patchEncy.Checked;

			if ( clientList.SelectedIndex < 2 )
			{
				m_Launch = (ClientLaunch)clientList.SelectedIndex;
			}
			else
			{
				m_Launch = ClientLaunch.Custom;
				m_ClientPath = ((PathElipsis)clientList.SelectedItem).GetPath();
			}
			
			ServerEntry se = null;
			if ( serverList.SelectedItem != null )
			{
				if ( serverList.SelectedItem is UOGamers_SE )
				{
					int port = 2593;
					string addr = ((UOGamers_SE)serverList.SelectedItem).RealAddress;
					if ( addr == "login.owo.com" )
					{
						port = 7776;
						ClientCommunication.ServerEncrypted = true;
					}

					se = new ServerEntry( addr, port );
				}
				else if ( !(serverList.SelectedItem is LoginCFG_SE) )
				{
					se = (ServerEntry)serverList.SelectedItem;
					se.Port = Utility.ToInt32( port.Text.Trim(), 0 );
					if ( se.Port <= 0 || se.Port > 65535 )
					{
						MessageBox.Show( this, Language.GetString( LocString.NeedPort ), "Need Port", MessageBoxButtons.OK, MessageBoxIcon.Information );
						return;
					}
				}
			}
			else if ( serverList.Text != "" )
			{
				int thePort = Utility.ToInt32( port.Text.Trim(), 0 );
				if ( thePort <= 0 || thePort > 65535 )
				{
					MessageBox.Show( this, Language.GetString( LocString.NeedPort ), "Need Port", MessageBoxButtons.OK, MessageBoxIcon.Information );
					return;
				}
				se = new ServerEntry( serverList.Text.Trim(), thePort );
			}

			if ( se != null && se.Address != null )
			{
				if ( !( serverList.SelectedItem is UOGamers_SE ) )
				{
					serverList.Items.Remove( se );
					serverList.Items.Insert( 1, se );
				}

				//if ( se.Address != "" )
				//	WriteLoginCFG( se.Address, se.Port );

				Config.SetRegString( Registry.CurrentUser, "LastServer", se.Address );
				Config.SetRegString( Registry.CurrentUser, "LastPort", se.Port.ToString() );
			}

			SaveData();

			this.Close();
		}

		private void quit_Click(object sender, System.EventArgs e)
		{
			SaveData();
			this.Close();
		}

		private void SaveData()
		{
			for (int i=0;i<serverList.Items.Count;i++)
			{
				for (int j=i+1;j<serverList.Items.Count;j++)
				{
					ServerEntry si = (ServerEntry)serverList.Items[i];
					ServerEntry sj = (ServerEntry)serverList.Items[j];
					if ( si.Address == sj.Address && si.Port == sj.Port )
						serverList.Items.RemoveAt( j );
				}
			}
		
			int num = 1;
			for (int i=0;i<serverList.Items.Count;i++)
			{
				ServerEntry se = (ServerEntry)serverList.Items[i];
				if ( se is UOGamers_SE || se is LoginCFG_SE )
					continue;
				
				if ( se.Address != "" )
				{
					Config.SetRegString( Microsoft.Win32.Registry.CurrentUser, String.Format( "Server{0}", num ), se.Address );
					Config.SetRegString( Microsoft.Win32.Registry.CurrentUser, String.Format( "Port{0}", num ), se.Port.ToString() );
					num++;
				}
			}

			for(int i=2;i<clientList.Items.Count;i++)
				Config.SetRegString( Microsoft.Win32.Registry.CurrentUser, String.Format( "Client{0}", i-1 ), ((PathElipsis)clientList.Items[i]).GetPath() );

			num = 1;
			if ( dataDir.SelectedIndex == -1 )
			{
				string dir = dataDir.Text;
				dir = dir.Trim();

				if ( dir.Length > 0 && dir != "(Auto Detect)" )
				{
					Config.SetRegString( Microsoft.Win32.Registry.CurrentUser, "Dir1", dir );
					Config.SetRegString( Microsoft.Win32.Registry.CurrentUser, "LastDir", "1" );
					m_DataDir = dir;
					num = 2;
				}
			}

			if ( num == 1 )
			{
				Config.SetRegString( Microsoft.Win32.Registry.CurrentUser, "LastDir", (dataDir.SelectedIndex != -1 ? dataDir.SelectedIndex : 0).ToString() );
				try
				{
					if ( dataDir.SelectedIndex != 0 )
						m_DataDir = dataDir.SelectedItem as string;
					else
						m_DataDir = null;
				}
				catch
				{
				}
			}

			for (int i=1;i<dataDir.Items.Count;i++)
				Config.SetRegString( Microsoft.Win32.Registry.CurrentUser, String.Format( "Dir{0}", num++ ), (string)dataDir.Items[i] );
		}

		/*private const string RazorLine = "; Razor Generated Entry";
		private void WriteLoginCFG( string addr, int port )
		{
			string fileName = Ultima.Client.GetFilePath( "Login.cfg" );
			if ( fileName == null || fileName == "" )
				return;
			ArrayList lines = new ArrayList();

			if ( File.Exists( fileName ) )
			{
				using ( StreamReader cfg = new StreamReader( fileName ) )
				{
					string line;
					while ( (line = cfg.ReadLine()) != null )
					{
						line = line.Trim();
						if ( line == RazorLine )
						{
							cfg.ReadLine(); // skip the next line which is the razor server
							continue;
						}
						else if ( line != "" )
						{
							lines.Add( line );
						}
					}
				}
			}

			using ( StreamWriter cfg = new StreamWriter( fileName ) )
			{
				foreach ( string line in lines )
				{
					if ( line.Length > 0 && line[0] != ';' )
						cfg.Write( ';' );
					cfg.WriteLine( line );
				}
				cfg.WriteLine( RazorLine );
				cfg.WriteLine( "LoginServer={0},{1}", addr.Trim(), port );
			}
		}*/

		private void showAtStart_CheckedChanged(object sender, System.EventArgs e)
		{
			Config.SetRegString( Microsoft.Win32.Registry.CurrentUser, "ShowWelcome", ( showAtStart.Checked ? 1 : 0 ).ToString() );
		}

		private void langSel_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string lang = langSel.SelectedItem as string;

			if ( lang != null && lang != Language.Current )
			{
				if ( !Language.Load( lang ) )
				{
					MessageBox.Show( "There was an error loading that language.", "Language Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning );
					langSel.SelectedItem = Language.Current;
				}
				else
				{
					Config.SetRegString( Microsoft.Win32.Registry.CurrentUser, "DefaultLanguage", Language.Current );
					Language.LoadControlNames( this );
				}
			}
		}

		private void patchEncy_CheckedChanged(object sender, System.EventArgs e)
		{
			if ( !patchEncy.Checked )
			{
				if ( MessageBox.Show( this, Language.GetString( LocString.NoPatchWarning ), Language.GetString( LocString.Confirm ), MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.No )
					patchEncy.Checked = true;
			}
		}

		private void useEnc_CheckedChanged(object sender, System.EventArgs e)
		{
			ClientCommunication.ServerEncrypted = useEnc.Checked;
		}

		private void dataBrowse_Click(object sender, System.EventArgs e)
		{
			FolderBrowserDialog folder = new FolderBrowserDialog();

			folder.Description = "Select a UO Data Directory...";

			if ( m_DataDir != null )
				folder.SelectedPath = m_DataDir;

			folder.ShowNewFolderButton = false;
			if ( folder.ShowDialog() == DialogResult.OK )
			{
				dataDir.SelectedIndex = -1;
				dataDir.Text = m_DataDir = folder.SelectedPath;
			}
		}

		private void dataDir_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		}

		private void dataDir_TextChanged(object sender, System.EventArgs e)
		{
		}
	}
}
