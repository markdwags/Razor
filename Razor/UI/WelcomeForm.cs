using System;
using System.Collections.Generic;
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
        private System.Windows.Forms.Label selectServerLbl;
        private System.Windows.Forms.ComboBox serverList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.OpenFileDialog openFile;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Button browse;

        public string ClientPath { get { return m_ClientPath; } }
        public ClientLaunch Client { get { return m_Launch; } }
        public bool PatchEncryption { get { return m_PatchEncy; } }
        public string DataDirectory { get { if (m_DataDir == "" || m_DataDir == "(Auto Detect)") m_DataDir = null; return m_DataDir; } }

        private bool m_PatchEncy = false;
        private string m_ClientPath = "";
        private ClientLaunch m_Launch = ClientLaunch.Custom;
        private TextBox uoClient;
        private TextBox dataDir;
        private Button dataBrowse;
        private Label label2;
        private TextBox serverPort;
        private TextBox serverHost;
        private Label portLabel;
        private Label serverHostLbl;
        private Button removeServer;
        private Button saveServer;
        private TextBox serverName;
        private Label serverNameLbl;
        private Button newServer;
        private ComboBox langSel;
        private Label label5;
        private CheckBox showAtStart;
        private string m_DataDir = "";

        public WelcomeForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
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
            this.selectServerLbl = new System.Windows.Forms.Label();
            this.serverList = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataDir = new System.Windows.Forms.TextBox();
            this.dataBrowse = new System.Windows.Forms.Button();
            this.uoClient = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.saveServer = new System.Windows.Forms.Button();
            this.removeServer = new System.Windows.Forms.Button();
            this.serverHostLbl = new System.Windows.Forms.Label();
            this.portLabel = new System.Windows.Forms.Label();
            this.serverHost = new System.Windows.Forms.TextBox();
            this.serverPort = new System.Windows.Forms.TextBox();
            this.serverNameLbl = new System.Windows.Forms.Label();
            this.serverName = new System.Windows.Forms.TextBox();
            this.newServer = new System.Windows.Forms.Button();
            this.langSel = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.showAtStart = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Load UO Client:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(308, 17);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(64, 24);
            this.browse.TabIndex = 1;
            this.browse.Text = "Browse...";
            this.browse.Click += new System.EventHandler(this.browse_Click);
            // 
            // patchEncy
            // 
            this.patchEncy.Location = new System.Drawing.Point(219, 76);
            this.patchEncy.Name = "patchEncy";
            this.patchEncy.Size = new System.Drawing.Size(152, 20);
            this.patchEncy.TabIndex = 4;
            this.patchEncy.Text = "Patch client encryption";
            this.patchEncy.CheckedChanged += new System.EventHandler(this.patchEncy_CheckedChanged);
            // 
            // okay
            // 
            this.okay.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okay.Location = new System.Drawing.Point(319, 306);
            this.okay.Name = "okay";
            this.okay.Size = new System.Drawing.Size(72, 43);
            this.okay.TabIndex = 14;
            this.okay.Text = "&Launch";
            this.okay.Click += new System.EventHandler(this.okay_Click);
            // 
            // quit
            // 
            this.quit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.quit.Location = new System.Drawing.Point(241, 306);
            this.quit.Name = "quit";
            this.quit.Size = new System.Drawing.Size(72, 43);
            this.quit.TabIndex = 13;
            this.quit.Text = "&Quit";
            this.quit.Click += new System.EventHandler(this.quit_Click);
            // 
            // selectServerLbl
            // 
            this.selectServerLbl.Location = new System.Drawing.Point(8, 24);
            this.selectServerLbl.Name = "selectServerLbl";
            this.selectServerLbl.Size = new System.Drawing.Size(79, 19);
            this.selectServerLbl.TabIndex = 9;
            this.selectServerLbl.Text = "Select Server:";
            this.selectServerLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // serverList
            // 
            this.serverList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.serverList.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverList.Location = new System.Drawing.Point(101, 19);
            this.serverList.Name = "serverList";
            this.serverList.Size = new System.Drawing.Size(269, 28);
            this.serverList.TabIndex = 5;
            this.serverList.SelectedIndexChanged += new System.EventHandler(this.serverList_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.langSel);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.showAtStart);
            this.groupBox1.Controls.Add(this.dataDir);
            this.groupBox1.Controls.Add(this.dataBrowse);
            this.groupBox1.Controls.Add(this.uoClient);
            this.groupBox1.Controls.Add(this.browse);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.patchEncy);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(379, 135);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Client Options";
            // 
            // dataDir
            // 
            this.dataDir.Location = new System.Drawing.Point(100, 47);
            this.dataDir.Name = "dataDir";
            this.dataDir.ReadOnly = true;
            this.dataDir.Size = new System.Drawing.Size(202, 23);
            this.dataDir.TabIndex = 2;
            // 
            // dataBrowse
            // 
            this.dataBrowse.Location = new System.Drawing.Point(308, 47);
            this.dataBrowse.Name = "dataBrowse";
            this.dataBrowse.Size = new System.Drawing.Size(63, 23);
            this.dataBrowse.TabIndex = 3;
            this.dataBrowse.Text = "Browse...";
            this.dataBrowse.Click += new System.EventHandler(this.dataBrowse_Click);
            // 
            // uoClient
            // 
            this.uoClient.Location = new System.Drawing.Point(100, 18);
            this.uoClient.Name = "uoClient";
            this.uoClient.ReadOnly = true;
            this.uoClient.Size = new System.Drawing.Size(202, 23);
            this.uoClient.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 19);
            this.label2.TabIndex = 25;
            this.label2.Text = "UO Data Folder:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.newServer);
            this.groupBox2.Controls.Add(this.serverName);
            this.groupBox2.Controls.Add(this.serverNameLbl);
            this.groupBox2.Controls.Add(this.serverPort);
            this.groupBox2.Controls.Add(this.serverHost);
            this.groupBox2.Controls.Add(this.portLabel);
            this.groupBox2.Controls.Add(this.serverHostLbl);
            this.groupBox2.Controls.Add(this.removeServer);
            this.groupBox2.Controls.Add(this.saveServer);
            this.groupBox2.Controls.Add(this.selectServerLbl);
            this.groupBox2.Controls.Add(this.serverList);
            this.groupBox2.Location = new System.Drawing.Point(12, 153);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(379, 147);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Server";
            // 
            // openFile
            // 
            this.openFile.DefaultExt = "exe";
            this.openFile.FileName = "client.exe";
            this.openFile.Filter = "UO Client|client.exe";
            this.openFile.Title = "Select UO Client";
            // 
            // saveServer
            // 
            this.saveServer.Enabled = false;
            this.saveServer.Location = new System.Drawing.Point(267, 53);
            this.saveServer.Name = "saveServer";
            this.saveServer.Size = new System.Drawing.Size(63, 27);
            this.saveServer.TabIndex = 9;
            this.saveServer.Text = "Save";
            this.saveServer.UseVisualStyleBackColor = true;
            this.saveServer.Click += new System.EventHandler(this.saveServer_Click);
            // 
            // removeServer
            // 
            this.removeServer.Location = new System.Drawing.Point(198, 53);
            this.removeServer.Name = "removeServer";
            this.removeServer.Size = new System.Drawing.Size(63, 27);
            this.removeServer.TabIndex = 10;
            this.removeServer.Text = "Remove";
            this.removeServer.UseVisualStyleBackColor = true;
            this.removeServer.Click += new System.EventHandler(this.removeServer_Click);
            // 
            // serverHostLbl
            // 
            this.serverHostLbl.AutoSize = true;
            this.serverHostLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverHostLbl.Location = new System.Drawing.Point(7, 118);
            this.serverHostLbl.Name = "serverHostLbl";
            this.serverHostLbl.Size = new System.Drawing.Size(85, 15);
            this.serverHostLbl.TabIndex = 15;
            this.serverHostLbl.Text = "Server Host/IP:";
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portLabel.Location = new System.Drawing.Point(269, 118);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(32, 15);
            this.portLabel.TabIndex = 16;
            this.portLabel.Text = "Port:";
            // 
            // serverHost
            // 
            this.serverHost.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverHost.Location = new System.Drawing.Point(100, 115);
            this.serverHost.Name = "serverHost";
            this.serverHost.Size = new System.Drawing.Size(163, 22);
            this.serverHost.TabIndex = 7;
            this.serverHost.TextChanged += new System.EventHandler(this.serverHost_TextChanged);
            // 
            // serverPort
            // 
            this.serverPort.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverPort.Location = new System.Drawing.Point(307, 116);
            this.serverPort.Name = "serverPort";
            this.serverPort.Size = new System.Drawing.Size(63, 22);
            this.serverPort.TabIndex = 8;
            this.serverPort.Text = "2593";
            this.serverPort.TextChanged += new System.EventHandler(this.serverPort_TextChanged);
            // 
            // serverNameLbl
            // 
            this.serverNameLbl.AutoSize = true;
            this.serverNameLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverNameLbl.Location = new System.Drawing.Point(7, 89);
            this.serverNameLbl.Name = "serverNameLbl";
            this.serverNameLbl.Size = new System.Drawing.Size(77, 15);
            this.serverNameLbl.TabIndex = 19;
            this.serverNameLbl.Text = "Server Name:";
            // 
            // serverName
            // 
            this.serverName.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverName.Location = new System.Drawing.Point(100, 86);
            this.serverName.Name = "serverName";
            this.serverName.Size = new System.Drawing.Size(270, 22);
            this.serverName.TabIndex = 6;
            this.serverName.TextChanged += new System.EventHandler(this.serverName_TextChanged);
            // 
            // newServer
            // 
            this.newServer.Location = new System.Drawing.Point(130, 53);
            this.newServer.Name = "newServer";
            this.newServer.Size = new System.Drawing.Size(62, 27);
            this.newServer.TabIndex = 20;
            this.newServer.Text = "New";
            this.newServer.UseVisualStyleBackColor = true;
            this.newServer.Click += new System.EventHandler(this.newServer_Click);
            // 
            // langSel
            // 
            this.langSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.langSel.Location = new System.Drawing.Point(77, 101);
            this.langSel.Name = "langSel";
            this.langSel.Size = new System.Drawing.Size(110, 23);
            this.langSel.TabIndex = 27;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 20);
            this.label5.TabIndex = 28;
            this.label5.Text = "Language:";
            // 
            // showAtStart
            // 
            this.showAtStart.Location = new System.Drawing.Point(11, 76);
            this.showAtStart.Name = "showAtStart";
            this.showAtStart.Size = new System.Drawing.Size(176, 20);
            this.showAtStart.TabIndex = 26;
            this.showAtStart.Text = "Show this when Razor starts";
            // 
            // WelcomeForm
            // 
            this.AcceptButton = this.okay;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            this.CancelButton = this.quit;
            this.ClientSize = new System.Drawing.Size(401, 358);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.quit);
            this.Controls.Add(this.okay);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WelcomeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Welcome to Razor: UOR Community Edition!";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.WelcomeForm_Closing);
            this.Load += new System.EventHandler(this.WelcomeForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private class ServerEntry
        {
            public ServerEntry(string addr, int p)
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
            public string RealAddress { get; set; }
            public string Name { get; set; }
            public Custom_SE(string name, string addr) : base(name, 0)
            {
                Name = name;
                RealAddress = addr;
            }

            public Custom_SE(string name, string addr, int port)
                 : base(name, port)
            {
                Name = name;
                RealAddress = addr;
            }
        }

        private class PathElipsis
        {
            private string m_Path;
            private string m_Show;
            public PathElipsis(string path)
            {
                m_Path = path;
                m_Show = GetPathElipsis(path, 23);
            }

            public string GetPath() { return m_Path; }
            public void SetPath(string value) { m_Path = value; m_Show = GetPathElipsis(m_Path, 23); }
            public override string ToString()
            {
                return m_Show;
            }

            private static char[] pathChars = new char[] { Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar, Path.PathSeparator, Path.VolumeSeparatorChar };
            public static string GetPathElipsis(string path, int PathMaxLength)
            {
                if (path.Length <= PathMaxLength)
                    return path;

                System.Text.StringBuilder sb = new System.Text.StringBuilder(path);
                int remlen = path.Length - PathMaxLength - 3;

                int ls = path.LastIndexOfAny(pathChars);
                if (ls == -1)
                    ls = 15 + remlen;

                if (ls - remlen < 4)
                    ls = remlen + 4;

                if (ls > remlen && remlen > 0)
                {
                    try
                    {
                        sb.Remove(ls - remlen, remlen);
                        sb.Insert(ls - remlen, "...");
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

            IsValidClientAndDataDir();

            patchEncy.Checked = Config.GetAppSetting<int>("PatchEncy") != 0;
            
            LoginCFG_SE lse = new LoginCFG_SE();
            Custom_SE cse;

            serverList.BeginUpdate();

            AddUORServers();

            // Load any custom servers they might have added
            NameValueCollection servers =
                (NameValueCollection)ConfigurationManager.GetSection("Servers");

            foreach (string server in servers.AllKeys)
            {
                string[] serverHostAndPort = servers[server].Split(',');
                string serverHost = serverHostAndPort[0];
                string serverPort = serverHostAndPort[1];

                serverList.Items.Add(new Custom_SE(server, serverHost, Convert.ToInt32(serverPort)));
            }

            serverList.EndUpdate();
            serverList.Refresh();

            // Set it to the last used one, or just set it to default UOR Prod
            try
            {
                serverList.SelectedIndex = Config.GetAppSetting<int>("LastServerId");
            }
            catch
            {
                serverList.SelectedIndex = 0;
            }

            WindowState = FormWindowState.Normal;
            this.BringToFront();
            this.TopMost = true;

            _ShowTimer = new System.Windows.Forms.Timer();
            _ShowTimer.Interval = 250;
            _ShowTimer.Enabled = true;
            _ShowTimer.Tick += new EventHandler(timer_Tick);


            if (Config.GetAppSetting<bool>("ImportProfilesAndMacros"))
            {
                string appDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Razor");

                if (Directory.Exists(appDataDir))
                {
                    if (MessageBox.Show(Language.GetString(LocString.ImportFromPrevious), "Import Razor Profiles and Macros",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Config.ImportProfilesMacros(appDataDir);

                        MessageBox.Show("Import complete", "Import Razor Profiles and Macros", MessageBoxButtons.OK);
                    }
                }

                Config.SetAppSetting("ImportProfilesAndMacros", "false");
            }
        }

        private void AddUORServers()
        {
            Custom_SE cse;
            // Always add the default UOR servers
            serverList.Items.Add(cse = new Custom_SE("UO Renaissance (Prod)", "login.uorenaissance.com", 2593));

            if (serverList.SelectedItem == null)
            {
                serverList.SelectedItem = cse;
            }

            serverList.Items.Add(new Custom_SE("UO Renaissance (Test)", "test.uorenaissance.com", 2597));
            serverList.Items.Add(new Custom_SE("UO Renaissance (Dev)", "dev.uorenaissance.com", 2598));
        }

        private System.Windows.Forms.Timer _ShowTimer;
        private void timer_Tick(object sender, EventArgs e)
        {
            this.TopMost = false;
            this.BringToFront();

            if (_ShowTimer != null)
                _ShowTimer.Stop();
        }

        private void browse_Click(object sender, System.EventArgs e)
        {
            if (openFile.ShowDialog(this) == DialogResult.OK)
            {
                PathElipsis pe = new PathElipsis(openFile.FileName);

                uoClient.Text = pe.GetPath();
                
                dataDir.Text = m_DataDir = Path.GetDirectoryName(uoClient.Text);

                Config.SetAppSetting("UODataDir", dataDir.Text);
                Config.SetAppSetting("UOClient", uoClient.Text);

                IsValidClientAndDataDir();
            }
        }

        private void serverList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (serverList.SelectedIndex < 0)
            {
                serverName.Enabled = true;
                serverHost.Enabled = true;
                serverPort.Enabled = true;

                return;
            }

            Custom_SE server = (Custom_SE)serverList.SelectedItem;

            serverName.Enabled = !(serverList.SelectedIndex >= 0 && serverList.SelectedIndex <= 2);
            serverHost.Enabled = !(serverList.SelectedIndex >= 0 && serverList.SelectedIndex <= 2);
            serverPort.Enabled = !(serverList.SelectedIndex >= 0 && serverList.SelectedIndex <= 2);
            removeServer.Enabled = !(serverList.SelectedIndex >= 0 && serverList.SelectedIndex <= 2);

            serverName.Text = server.Name;
            serverHost.Text = server.RealAddress;
            serverPort.Text = server.Port.ToString();
            saveServer.Enabled = false;
        }

        private void okay_Click(object sender, System.EventArgs e)
        {
            Config.SetAppSetting("PatchEncy", patchEncy.Checked ? "1" : "0");

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

                Config.SetAppSetting("LastServerId", serverList.SelectedIndex.ToString());
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
            for (int i = 0; i < serverList.Items.Count; i++)
            {
                for (int j = i + 1; j < serverList.Items.Count; j++)
                {
                    ServerEntry si = (ServerEntry)serverList.Items[i];
                    ServerEntry sj = (ServerEntry)serverList.Items[j];
                    if (si.Address == sj.Address && si.Port == sj.Port)
                        serverList.Items.RemoveAt(j);
                }
            }

            int num = 1;
            for (int i = 0; i < serverList.Items.Count; i++)
            {
                ServerEntry se = (ServerEntry)serverList.Items[i];
                if (se is Custom_SE || se is LoginCFG_SE)
                    continue;

                if (se.Address != "")
                {
                    Config.SetAppSetting($"Server{num}", se.Address);
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

            if (lang != null && lang != Language.Current)
            {
                if (!Language.Load(lang))
                {
                    MessageBox.Show("There was an error loading that language.", "Language Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    langSel.SelectedItem = Language.Current;
                }
                else
                {
                    Config.SetAppSetting("DefaultLanguage", Language.Current);
                    Language.LoadControlNames(this);
                }
            }
        }

        private void patchEncy_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!patchEncy.Checked)
            {
                if (MessageBox.Show(this, Language.GetString(LocString.NoPatchWarning), Language.GetString(LocString.Confirm), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    patchEncy.Checked = true;
            }
        }

        private void dataBrowse_Click(object sender, System.EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();

            folder.Description = "Select a UO Data Directory...";

            if (m_DataDir != null)
                folder.SelectedPath = m_DataDir;

            folder.ShowNewFolderButton = false;
            if (folder.ShowDialog() == DialogResult.OK)
            {
                dataDir.Text = m_DataDir = folder.SelectedPath;
                Config.SetAppSetting("UODataDir", dataDir.Text);

                IsValidClientAndDataDir();

                if (!dataDir.Text.Equals(Path.GetDirectoryName(uoClient.Text)))
                {
                    MessageBox.Show(Language.GetString(LocString.ClientDataMismatch), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void WelcomeForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveData();
        }

        private bool IsValidClientAndDataDir()
        {
            okay.Enabled = false;

            if (string.IsNullOrEmpty(uoClient.Text) || string.IsNullOrEmpty(dataDir.Text))
            {
                return false;
            }


            if (!File.Exists(uoClient.Text) || !Directory.Exists(dataDir.Text))
            {
                return false;
            }

            okay.Enabled = true;
            return true;
        }

        private void saveServer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(serverName.Text) || string.IsNullOrEmpty(serverHost.Text) ||
                string.IsNullOrEmpty(serverPort.Text))
            {
                MessageBox.Show("You must fill out all three fields", "Server Add/Edit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (int.TryParse(serverPort.Text, out _) == false)
            {
                MessageBox.Show("Invalid port", "Server Add/Edit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AppSettingsSection servers = (AppSettingsSection)config.GetSection("Servers");

            if (string.IsNullOrEmpty(serverList.Text)) // new entry
            {
                servers.Settings.Add(serverName.Text, $"{serverHost.Text},{serverPort.Text}");
                config.Save(ConfigurationSaveMode.Modified, true);

                ConfigurationManager.RefreshSection("Servers");

                serverList.Items.Add(new Custom_SE(serverName.Text, serverHost.Text, Convert.ToInt32(serverPort.Text)));

                serverList.SelectedIndex = serverList.Items.Count - 1;
            }
            else
            {
                Dictionary<string, string> entries = new Dictionary<string, string>();

                int index = 3;
                foreach (KeyValueConfigurationElement server in servers.Settings)
                {
                    if (serverList.SelectedIndex == index)
                    {
                        entries.Add(serverName.Text, $"{serverHost.Text},{serverPort.Text}");
                    }
                    else
                    {
                        entries.Add(server.Key, server.Value);
                    }

                    index++;
                }

                servers.Settings.Clear();
                serverList.Items.Clear();

                AddUORServers();

                foreach (KeyValuePair<string, string> entry in entries)
                {
                    servers.Settings.Add(entry.Key, entry.Value);

                    string[] address = entry.Value.Split(',');

                    serverList.Items.Add(new Custom_SE(entry.Key, address[0], Convert.ToInt32(address[1])));
                }

                config.Save(ConfigurationSaveMode.Modified, true);
                ConfigurationManager.RefreshSection("Servers");
            }

            saveServer.Enabled = false;
        }

        private void newServer_Click(object sender, EventArgs e)
        {
            serverName.Text = string.Empty;
            serverHost.Text = string.Empty;
            serverPort.Text = string.Empty;
            serverList.SelectedIndex = -1;

            saveServer.Enabled = true;
        }

        private void serverName_TextChanged(object sender, EventArgs e)
        {
            saveServer.Enabled = true;
        }

        private void serverHost_TextChanged(object sender, EventArgs e)
        {
            saveServer.Enabled = true;
        }

        private void serverPort_TextChanged(object sender, EventArgs e)
        {
            saveServer.Enabled = true;
        }

        private void removeServer_Click(object sender, EventArgs e)
        {
            if (serverList.SelectedIndex < 0)
                return;

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AppSettingsSection servers = (AppSettingsSection)config.GetSection("Servers");

            Dictionary<string, string> entries = new Dictionary<string, string>();

            int index = 0;
            foreach (KeyValueConfigurationElement server in servers.Settings)
            {
                if (serverList.SelectedIndex != index + 3)
                {
                    entries.Add(server.Key, server.Value);
                }

                index++;
            }

            servers.Settings.Clear();
            serverList.Items.Clear();

            AddUORServers();

            foreach (KeyValuePair<string, string> entry in entries)
            {
                servers.Settings.Add(entry.Key, entry.Value);

                string[] address = entry.Value.Split(',');

                serverList.Items.Add(new Custom_SE(entry.Key, address[0], Convert.ToInt32(address[1])));
            }

            config.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("Servers");

            serverName.Text = string.Empty;
            serverHost.Text = string.Empty;
            serverPort.Text = string.Empty;
        }
    }
}
