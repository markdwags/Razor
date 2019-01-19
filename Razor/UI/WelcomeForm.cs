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

        private System.Windows.Forms.CheckBox showAtStart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox langSel;

        public string ClientPath { get { return m_ClientPath; } }
        public ClientLaunch Client { get { return m_Launch; } }
        public bool PatchEncryption { get { return m_PatchEncy; } }
        public string DataDirectory { get { if (m_DataDir == "" || m_DataDir == "(Auto Detect)") m_DataDir = null; return m_DataDir; } }

        private bool m_PatchEncy = false;
        private string m_ClientPath = "";
        private ClientLaunch m_Launch = ClientLaunch.Custom;
        private TextBox uoClient;
        private Label serverInfo;
        private TextBox dataDir;
        private Button dataBrowse;
        private Label label2;
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
            this.label3 = new System.Windows.Forms.Label();
            this.showAtStart = new System.Windows.Forms.CheckBox();
            this.serverList = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataDir = new System.Windows.Forms.TextBox();
            this.dataBrowse = new System.Windows.Forms.Button();
            this.uoClient = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.serverInfo = new System.Windows.Forms.Label();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.langSel = new System.Windows.Forms.ComboBox();
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
            this.browse.TabIndex = 2;
            this.browse.Text = "Browse...";
            this.browse.Click += new System.EventHandler(this.browse_Click);
            // 
            // patchEncy
            // 
            this.patchEncy.Location = new System.Drawing.Point(11, 76);
            this.patchEncy.Name = "patchEncy";
            this.patchEncy.Size = new System.Drawing.Size(152, 20);
            this.patchEncy.TabIndex = 3;
            this.patchEncy.Text = "Patch client encryption";
            this.patchEncy.CheckedChanged += new System.EventHandler(this.patchEncy_CheckedChanged);
            // 
            // okay
            // 
            this.okay.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okay.Location = new System.Drawing.Point(317, 201);
            this.okay.Name = "okay";
            this.okay.Size = new System.Drawing.Size(72, 43);
            this.okay.TabIndex = 6;
            this.okay.Text = "&Launch";
            this.okay.Click += new System.EventHandler(this.okay_Click);
            // 
            // quit
            // 
            this.quit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.quit.Location = new System.Drawing.Point(239, 201);
            this.quit.Name = "quit";
            this.quit.Size = new System.Drawing.Size(72, 43);
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
            this.showAtStart.Location = new System.Drawing.Point(12, 201);
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
            this.serverList.Size = new System.Drawing.Size(319, 23);
            this.serverList.TabIndex = 11;
            this.serverList.SelectedIndexChanged += new System.EventHandler(this.serverList_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataDir);
            this.groupBox1.Controls.Add(this.dataBrowse);
            this.groupBox1.Controls.Add(this.uoClient);
            this.groupBox1.Controls.Add(this.browse);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.patchEncy);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(379, 111);
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
            this.dataDir.TabIndex = 24;
            // 
            // dataBrowse
            // 
            this.dataBrowse.Location = new System.Drawing.Point(308, 47);
            this.dataBrowse.Name = "dataBrowse";
            this.dataBrowse.Size = new System.Drawing.Size(63, 23);
            this.dataBrowse.TabIndex = 23;
            this.dataBrowse.Text = "Browse...";
            this.dataBrowse.Click += new System.EventHandler(this.dataBrowse_Click);
            // 
            // uoClient
            // 
            this.uoClient.Location = new System.Drawing.Point(100, 18);
            this.uoClient.Name = "uoClient";
            this.uoClient.ReadOnly = true;
            this.uoClient.Size = new System.Drawing.Size(202, 23);
            this.uoClient.TabIndex = 6;
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
            this.groupBox2.Controls.Add(this.serverInfo);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.serverList);
            this.groupBox2.Location = new System.Drawing.Point(12, 129);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(379, 66);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Server";
            // 
            // serverInfo
            // 
            this.serverInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.serverInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverInfo.ForeColor = System.Drawing.Color.MidnightBlue;
            this.serverInfo.Location = new System.Drawing.Point(52, 42);
            this.serverInfo.Name = "serverInfo";
            this.serverInfo.Size = new System.Drawing.Size(319, 21);
            this.serverInfo.TabIndex = 12;
            this.serverInfo.Text = "login.server.com,2593";
            this.serverInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // openFile
            // 
            this.openFile.DefaultExt = "exe";
            this.openFile.FileName = "client.exe";
            this.openFile.Filter = "UO Client|client.exe";
            this.openFile.Title = "Select UO Client";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 224);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 20);
            this.label5.TabIndex = 17;
            this.label5.Text = "Language:";
            // 
            // langSel
            // 
            this.langSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.langSel.Location = new System.Drawing.Point(80, 221);
            this.langSel.Name = "langSel";
            this.langSel.Size = new System.Drawing.Size(95, 23);
            this.langSel.TabIndex = 18;
            this.langSel.SelectedIndexChanged += new System.EventHandler(this.langSel_SelectedIndexChanged);
            // 
            // WelcomeForm
            // 
            this.AcceptButton = this.okay;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            this.CancelButton = this.quit;
            this.ClientSize = new System.Drawing.Size(401, 252);
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
            this.Text = "Welcome to Razor: UOR Community Edition!";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.WelcomeForm_Closing);
            this.Load += new System.EventHandler(this.WelcomeForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
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
            public string RealAddress;
            public Custom_SE(string name, string addr) : base(name, 0)
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

            // Always add the default UOR servers
            serverList.Items.Add(cse = new Custom_SE("UO Renaissance (Prod)", "login.uorenaissance.com", 2593));

            if (serverList.SelectedItem == null)
            {
                serverList.SelectedItem = cse;
            }

            serverList.Items.Add(new Custom_SE("UO Renaissance (Test)", "test.uorenaissance.com", 2597));
            serverList.Items.Add(new Custom_SE("UO Renaissance (Dev)", "dev.uorenaissance.com", 2598));

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
            Custom_SE server = (Custom_SE)serverList.SelectedItem;

            serverInfo.Text = $"{server.RealAddress},{server.Port}";
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
            else if (serverList.Text != null)
            {
                string[] addr = serverList.Text.Split(',');

                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                var servers = (AppSettingsSection)config.GetSection("Servers");
                servers.Settings.Add(addr[0], serverList.Text);
                config.Save(ConfigurationSaveMode.Modified, true);
                ConfigurationManager.RefreshSection("Servers");

                serverList.Items.Add(new Custom_SE(addr[0], addr[0], Convert.ToInt32(addr[1])));

                se = new ServerEntry(addr[0], Convert.ToInt32(addr[1]));
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

    }
}
