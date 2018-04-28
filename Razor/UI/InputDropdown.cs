using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Assistant
{
    /// <summary>
    /// Summary description for NewProfile.
    /// </summary>
    public class InputDropdown : System.Windows.Forms.Form
    {
        private static InputDropdown m_Instance;
        

        public static bool Show(Form parent, string prompt, string[] dropDownItems)
        {
            return Show(parent, prompt, dropDownItems, Language.GetString(LocString.InputReq));
        }

        public static bool Show(Form parent, string prompt, string title)
        {
            return Show(parent, prompt, new string[] {}, title);
        }

        public static bool Show(Form parent, string prompt, string[] dropDownItems, string title)
        {
            if (m_Instance == null)
                m_Instance = new InputDropdown();
            m_Instance.Prompt.Text = prompt;
            m_Instance.Text = title;
            m_Instance.m_String = "";

            m_Instance.EntryCombo.Items.Clear();
            m_Instance.EntryCombo.Items.AddRange(dropDownItems);
            m_Instance.EntryCombo.SelectedIndex = 0;

            if (parent != null)
                return m_Instance.ShowDialog() == DialogResult.OK;
            else
                return m_Instance.ShowDialog(parent) == DialogResult.OK;
        }

        public static string GetString()
        {
            return m_Instance != null ? m_Instance.m_String : null;
        }

        public static int GetInt(int def)
        {
            try
            {
                string conv = m_Instance.m_String;
                int b = 10;
                if (conv[0] == '0' && conv[1] == 'x')
                {
                    b = 16;
                    conv = conv.Substring(2);
                }
                else if (conv[0] == 'x' || conv[0] == 'X')
                {
                    b = 16;
                    conv = conv.Substring(1);
                }

                return Convert.ToInt32(conv, b);
            }
            catch
            {
                return def;
            }
        }

        public static int GetInt()
        {
            return GetInt(0);
        }

        private string m_String;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label Prompt;
        private ComboBox EntryCombo;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        private InputDropdown()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
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
            this.Prompt = new System.Windows.Forms.Label();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.EntryCombo = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Prompt
            // 
            this.Prompt.Location = new System.Drawing.Point(4, 2);
            this.Prompt.Name = "Prompt";
            this.Prompt.Size = new System.Drawing.Size(210, 16);
            this.Prompt.TabIndex = 0;
            this.Prompt.Text = ":";
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(32, 46);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(72, 27);
            this.ok.TabIndex = 2;
            this.ok.Text = "Okay";
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(112, 46);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(72, 27);
            this.cancel.TabIndex = 3;
            this.cancel.Text = "Cancel";
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // EntryCombo
            // 
            this.EntryCombo.FormattingEnabled = true;
            this.EntryCombo.Location = new System.Drawing.Point(7, 21);
            this.EntryCombo.Name = "EntryCombo";
            this.EntryCombo.Size = new System.Drawing.Size(207, 21);
            this.EntryCombo.TabIndex = 4;
            // 
            // InputDropdown
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 15);
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(218, 78);
            this.ControlBox = false;
            this.Controls.Add(this.EntryCombo);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.Prompt);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "InputDropdown";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Input";
            this.Load += new System.EventHandler(this.InputDropdown_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private void ok_Click(object sender, System.EventArgs e)
        {
            m_String = EntryCombo.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void InputDropdown_Load(object sender, System.EventArgs e)
        {
            Language.LoadControlNames(this);

            if (this.Location.X <= 0 || this.Location.Y <= 0)
                this.Location = new System.Drawing.Point(Config.GetInt("WindowX"), Config.GetInt("WindowY"));

            this.WindowState = FormWindowState.Normal;
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
            this.Activate();

            EntryCombo.Focus();

            if (_ShowTimer != null)
                _ShowTimer.Stop();
        }
    }
}