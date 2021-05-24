#region license

// Razor: An Ultima Online Assistant
// Copyright (C) 2021 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Windows.Forms;

namespace Assistant
{
    /// <summary>
    /// Summary description for NewProfile.
    /// </summary>
    public class InputBox : System.Windows.Forms.Form
    {
        private static InputBox m_Instance;

        public static bool Show(string prompt)
        {
            return Show(null, prompt, Language.GetString(LocString.InputReq), "");
        }

        public static bool Show(string prompt, string title)
        {
            return Show(null, prompt, title, "");
        }

        public static bool Show(string prompt, string title, string def)
        {
            return Show(null, prompt, title, def);
        }

        public static bool Show(Form parent, string prompt)
        {
            return Show(parent, prompt, Language.GetString(LocString.InputReq), "");
        }

        public static bool Show(Form parent, string prompt, string title)
        {
            return Show(parent, prompt, title, "");
        }

        public static bool Show(Form parent, string prompt, string title, string def)
        {
            if (m_Instance == null)
                m_Instance = new InputBox();
            m_Instance.Prompt.Text = prompt;
            m_Instance.Text = title;
            m_Instance.m_String = "";
            m_Instance.EntryBox.Text = def;

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
        private System.Windows.Forms.TextBox EntryBox;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private InputBox()
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
            this.EntryBox = new System.Windows.Forms.TextBox();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Prompt
            // 
            this.Prompt.Location = new System.Drawing.Point(12, 9);
            this.Prompt.Name = "Prompt";
            this.Prompt.Size = new System.Drawing.Size(210, 16);
            this.Prompt.TabIndex = 0;
            this.Prompt.Text = ":";
            // 
            // EntryBox
            // 
            this.EntryBox.Location = new System.Drawing.Point(12, 28);
            this.EntryBox.Name = "EntryBox";
            this.EntryBox.Size = new System.Drawing.Size(243, 23);
            this.EntryBox.TabIndex = 1;
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(186, 57);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(69, 30);
            this.ok.TabIndex = 2;
            this.ok.Text = "Okay";
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(15, 57);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(69, 30);
            this.cancel.TabIndex = 3;
            this.cancel.Text = "Cancel";
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // InputBox
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(266, 94);
            this.ControlBox = false;
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.EntryBox);
            this.Controls.Add(this.Prompt);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "InputBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Input";
            this.Load += new System.EventHandler(this.InputBox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private void ok_Click(object sender, System.EventArgs e)
        {
            m_String = EntryBox.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void InputBox_Load(object sender, System.EventArgs e)
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

            EntryBox.Focus();

            if (_ShowTimer != null)
                _ShowTimer.Stop();
        }
    }
}