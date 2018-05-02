using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Assistant
{
	public enum ClientLaunch
	{
		TwoD,
		ThirdDawn,
		Custom
	}

	public class WelcomeForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox patchEncy;
		private System.Windows.Forms.Button okay;
		private System.Windows.Forms.Button quit;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox serverList;
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
		private System.Windows.Forms.GroupBox groupBox3;

		public string ClientPath{ get{ return m_ClientPath; } }
		public ClientLaunch Client{ get{ return m_Launch; } } 
		public bool PatchEncryption{ get{ return m_PatchEncy; } }
		public string DataDirectory{ get{ if ( m_DataDir == "" || m_DataDir == "(Auto Detect)" ) m_DataDir = null; return m_DataDir; } }

		private bool m_PatchEncy = false;
		private string m_ClientPath = "";
		private ClientLaunch m_Launch = ClientLaunch.Custom;
        private TextBox uoClient;
        private TextBox dataDir;
        private Label serverInfo;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeForm));
            this.label1 = new System.Windows.Forms.Label();
            this.browse = new System.Windows.Forms.Button();
            this.patchEncy = new System.Windows.Forms.CheckBox();
            this.okay = new System.Windows.Forms.Button();
            this.quit = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.showAtStart = new System.Windows.Forms.CheckBox();
            this.serverList = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.uoClient = new System.Windows.Forms.TextBox();
            this.useEnc = new System.Windows.Forms.CheckBox();
            this.makeDef = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.langSel = new System.Windows.Forms.ComboBox();
            this.dataBrowse = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataDir = new System.Windows.Forms.TextBox();
            this.serverInfo = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Load Client:";
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(289, 16);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(64, 23);
            this.browse.TabIndex = 2;
            this.browse.Text = "Browse...";
            this.browse.Click += new System.EventHandler(this.browse_Click);
            // 
            // patchEncy
            // 
            this.patchEncy.Location = new System.Drawing.Point(8, 45);
            this.patchEncy.Name = "patchEncy";
            this.patchEncy.Size = new System.Drawing.Size(171, 20);
            this.patchEncy.TabIndex = 3;
            this.patchEncy.Text = "Patch client encryption";
            this.patchEncy.CheckedChanged += new System.EventHandler(this.patchEncy_CheckedChanged);
            // 
            // okay
            // 
            this.okay.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okay.Location = new System.Drawing.Point(9, 257);
            this.okay.Name = "okay";
            this.okay.Size = new System.Drawing.Size(72, 34);
            this.okay.TabIndex = 6;
            this.okay.Text = "&Launch";
            this.okay.Click += new System.EventHandler(this.okay_Click);
            // 
            // quit
            // 
            this.quit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.quit.Location = new System.Drawing.Point(89, 257);
            this.quit.Name = "quit";
            this.quit.Size = new System.Drawing.Size(72, 34);
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
            this.showAtStart.Location = new System.Drawing.Point(187, 265);
            this.showAtStart.Name = "showAtStart";
            this.showAtStart.Size = new System.Drawing.Size(176, 20);
            this.showAtStart.TabIndex = 10;
            this.showAtStart.Text = "Show this when Razor starts";
            this.showAtStart.CheckedChanged += new System.EventHandler(this.showAtStart_CheckedChanged);
            // 
            // serverList
            // 
            this.serverList.Location = new System.Drawing.Point(52, 16);
            this.serverList.Name = "serverList";
            this.serverList.Size = new System.Drawing.Size(284, 23);
            this.serverList.TabIndex = 11;
            this.serverList.SelectedIndexChanged += new System.EventHandler(this.serverList_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.uoClient);
            this.groupBox1.Controls.Add(this.useEnc);
            this.groupBox1.Controls.Add(this.makeDef);
            this.groupBox1.Controls.Add(this.browse);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.patchEncy);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(359, 97);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Client Options";
            // 
            // uoClient
            // 
            this.uoClient.Location = new System.Drawing.Point(81, 16);
            this.uoClient.Name = "uoClient";
            this.uoClient.ReadOnly = true;
            this.uoClient.Size = new System.Drawing.Size(202, 23);
            this.uoClient.TabIndex = 6;
            // 
            // useEnc
            // 
            this.useEnc.Location = new System.Drawing.Point(8, 68);
            this.useEnc.Name = "useEnc";
            this.useEnc.Size = new System.Drawing.Size(140, 20);
            this.useEnc.TabIndex = 5;
            this.useEnc.Text = "Use OSI Encryption";
            this.useEnc.CheckedChanged += new System.EventHandler(this.useEnc_CheckedChanged);
            // 
            // makeDef
            // 
            this.makeDef.Location = new System.Drawing.Point(169, 56);
            this.makeDef.Name = "makeDef";
            this.makeDef.Size = new System.Drawing.Size(184, 35);
            this.makeDef.TabIndex = 4;
            this.makeDef.Text = "Make These Settings Default";
            this.makeDef.Click += new System.EventHandler(this.makeDef_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.serverInfo);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.serverList);
            this.groupBox2.Location = new System.Drawing.Point(4, 161);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(344, 66);
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
            this.label5.Location = new System.Drawing.Point(210, 236);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 20);
            this.label5.TabIndex = 17;
            this.label5.Text = "Language:";
            // 
            // langSel
            // 
            this.langSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.langSel.Location = new System.Drawing.Point(276, 233);
            this.langSel.Name = "langSel";
            this.langSel.Size = new System.Drawing.Size(72, 23);
            this.langSel.TabIndex = 18;
            this.langSel.SelectedIndexChanged += new System.EventHandler(this.langSel_SelectedIndexChanged);
            // 
            // dataBrowse
            // 
            this.dataBrowse.Location = new System.Drawing.Point(272, 18);
            this.dataBrowse.Name = "dataBrowse";
            this.dataBrowse.Size = new System.Drawing.Size(64, 23);
            this.dataBrowse.TabIndex = 21;
            this.dataBrowse.Text = "Browse...";
            this.dataBrowse.Click += new System.EventHandler(this.dataBrowse_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataDir);
            this.groupBox3.Controls.Add(this.dataBrowse);
            this.groupBox3.Location = new System.Drawing.Point(4, 107);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(344, 48);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "UO Data Directory";
            // 
            // dataDir
            // 
            this.dataDir.Location = new System.Drawing.Point(7, 19);
            this.dataDir.Name = "dataDir";
            this.dataDir.ReadOnly = true;
            this.dataDir.Size = new System.Drawing.Size(261, 23);
            this.dataDir.TabIndex = 22;
            // 
            // serverInfo
            // 
            this.serverInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.serverInfo.AutoSize = true;
            this.serverInfo.Font = new System.Drawing.Font("Segoe UI Semilight", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverInfo.Location = new System.Drawing.Point(49, 42);
            this.serverInfo.Name = "serverInfo";
            this.serverInfo.Size = new System.Drawing.Size(119, 15);
            this.serverInfo.TabIndex = 12;
            this.serverInfo.Text = "login.server.com,2593";
            this.serverInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WelcomeForm
            // 
            this.AcceptButton = this.okay;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            this.CancelButton = this.quit;
            this.ClientSize = new System.Drawing.Size(367, 300);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.langSel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.showAtStart);
            this.Controls.Add(this.quit);
            this.Controls.Add(this.okay);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WelcomeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Welcome to Razor!";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.WelcomeForm_Closing);
            this.Load += new System.EventHandler(this.WelcomeForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
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

		    public LoginCFG_SE() : base("Use Last", 0)
		    {
		        RealAddress = Config.GetAppSetting<string>("LastServer");
		        Port = Config.GetAppSetting<int>("LastPort");

                if (string.IsNullOrEmpty(RealAddress) || Port == 0)
		        {
		            RealAddress = "";
		            Port = 0;

		            try
		            {
		                string fileName = Ultima.Files.GetFilePath("Login.cfg");
		                if (string.IsNullOrEmpty(fileName))
		                    return;
		                string server = null, port = null;

		                if (File.Exists(fileName))
		                {
		                    using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read,
		                        FileShare.ReadWrite))
		                    using (StreamReader cfg = new StreamReader(file))
		                    {
		                        string line;
		                        while ((line = cfg.ReadLine()) != null)
		                        {
		                            line = line.Trim();
		                            if (line != "" && Char.ToUpper(line[0]) == 'L' && line.Length > 12)
		                            {
		                                int comma = line.IndexOf(',');
		                                if (comma > 12)
		                                {
		                                    server = line.Substring(12, comma - 12);
		                                    port = line.Substring(comma + 1);

		                                    break;
		                                }
		                            }
		                        }
		                    }
		                }

		                if (server != null)
		                {
		                    Address = "(Use Last: " + server + ")";
		                    RealAddress = server;
		                }

		                if (port != null)
		                    Port = Utility.ToInt32(port, 0);
		            }
		            catch
		            {
		                RealAddress = Address = "Use Last";
		                Port = 0;
		            }
		        }
		    }
		}

		private class ShardEntry
		{
			public string name { get; set; }
			public string type { get; set; }
			public string host { get; set; }
			public int port { get; set; }
		}

		private class Custom_SE : ServerEntry
		{
			public string RealAddress;
			public Custom_SE( string name, string addr ) : base( name, 0 )
			{
				RealAddress = addr;
			}

			public Custom_SE(string name, string addr, int port)
				: base(name, port)
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
	        Language.LoadControlNames(this);

	        this.BringToFront();

	        langSel.Items.AddRange(Language.GetPackNames());
	        langSel.SelectedItem = Language.Current;

	        showAtStart.Checked = Config.GetAppSetting<int>("ShowWelcome") == 1;

	        uoClient.Text = Config.GetAppSetting<string>("UOClient");
            dataDir.Text = Config.GetAppSetting<string>("UODataDir");

	        if (string.IsNullOrEmpty(uoClient.Text) || string.IsNullOrEmpty(dataDir.Text))
	        {
                throw new Exception("UOClient or UODataDir is not set correctly.");
	        }

	        patchEncy.Checked = Config.GetAppSetting<int>("PatchEncy") != 0;
	        useEnc.Checked = Config.GetAppSetting<int>("ServerEnc") != 0;

	        LoginCFG_SE lse = new LoginCFG_SE();
	        Custom_SE cse;
            
	        serverList.BeginUpdate();

	        NameValueCollection servers =
	            (NameValueCollection)ConfigurationManager.GetSection("Servers");

	        serverList.Items.Add(cse = new Custom_SE("UO Renaissance (Prod)", "login.uorenaissance.com", 2593));

	        if (serverList.SelectedItem == null)
	        {
	            serverList.SelectedItem = cse;
	        }

	        serverList.Items.Add(new Custom_SE("UO Renaissance (Test)", "test.uorenaissance.com", 2597));

	        foreach (string server in servers.AllKeys)
	        {
	            string[] serverInfo = servers[server].Split(',');
	            string serverHost = serverInfo[0];
	            string serverPort = serverInfo[1];

	            serverList.Items.Add(new Custom_SE(server, serverHost, Convert.ToInt32(serverPort)));
	        }

	        serverList.EndUpdate();

	        serverList.Refresh();

	        WindowState = FormWindowState.Normal;
	        this.BringToFront();
	        this.TopMost = true;

	        _ShowTimer = new System.Windows.Forms.Timer();
	        _ShowTimer.Interval = 250;
	        _ShowTimer.Enabled = true;
	        _ShowTimer.Tick += new EventHandler(timer_Tick);
	    }
		
		private System.Windows.Forms.Timer _ShowTimer;
		private void timer_Tick(object sender, EventArgs e)
		{
			this.TopMost = false;
			this.BringToFront();

			if ( _ShowTimer != null )
				_ShowTimer.Stop();
		}

		private void browse_Click(object sender, System.EventArgs e)
		{
			if ( openFile.ShowDialog( this ) == DialogResult.OK )
			{
				PathElipsis pe = new PathElipsis( openFile.FileName );

			    uoClient.Text = pe.GetPath();
			}
		}

	    private void makeDef_Click(object sender, System.EventArgs e)
	    {

	        Config.SetAppSetting("PatchEncy", patchEncy.Checked ? "1" : "0");
	        Config.SetAppSetting("ServerEnc", useEnc.Checked ? "1" : "0");

	        MessageBox.Show(this, Language.GetString(LocString.SaveOK), "Done", MessageBoxButtons.OK,
	            MessageBoxIcon.Information);
	    }

		private void serverList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			/*port.Enabled = !( serverList.SelectedItem is Custom_SE || serverList.SelectedItem is LoginCFG_SE );

			if ( serverList.SelectedItem != null )
			{
				if ( serverList.SelectedItem is LoginCFG_SE && ((ServerEntry)serverList.SelectedItem).Port == 0 )
					port.Text = "";
				else
					port.Text = ((ServerEntry)serverList.SelectedItem).Port.ToString();
			}*/

		    Custom_SE server = (Custom_SE) serverList.SelectedItem;

		    serverInfo.Text = $"{server.RealAddress},{server.Port}";
		}
		
		private void okay_Click(object sender, System.EventArgs e)
		{
            m_PatchEncy = patchEncy.Checked;

		    m_Launch = ClientLaunch.TwoD;
		    m_ClientPath = uoClient.Text;

            ServerEntry se = null;

            if (serverList.SelectedItem != null)
            {
                if (serverList.SelectedItem is Custom_SE)
                {
                    int port = ((Custom_SE)serverList.SelectedItem).Port;

                    string addr = ((Custom_SE)serverList.SelectedItem).RealAddress;

                    if (addr == "login.ultimaonline.com")
                    {
                        ClientCommunication.ServerEncrypted = true;
                    }

                    if (port == 0)
                        port = 2593; // runuo default

                    se = new ServerEntry(addr, port);
                }
            }

            if (se != null && se.Address != null)
            {
                Config.SetAppSetting("LastServer", se.Address);
                Config.SetAppSetting("LastPort", se.Port.ToString());
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
				if ( se is Custom_SE || se is LoginCFG_SE )
					continue;
				
				if ( se.Address != "" )
				{
					Config.SetAppSetting($"Server{num}", se.Address );
				    Config.SetAppSetting($"Port{num}", se.Port.ToString());
					num++;
				}
			}

		    Config.SetAppSetting("UOClient", uoClient.Text);
		    Config.SetAppSetting("UODataDir", dataDir.Text);
		    m_DataDir = dataDir.Text;
        }

	    private void showAtStart_CheckedChanged(object sender, System.EventArgs e)
	    {
	        Config.SetAppSetting("ShowWelcome", (showAtStart.Checked ? 1 : 0).ToString());
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
				    Config.SetAppSetting("DefaultLanguage", Language.Current );
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
				dataDir.Text = m_DataDir = folder.SelectedPath;
			    Config.SetAppSetting("UODataDir", dataDir.Text);
            }
		}

		private void dataDir_TextChanged(object sender, System.EventArgs e)
		{
		}

		private void WelcomeForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			SaveData();
		}
    }
}
