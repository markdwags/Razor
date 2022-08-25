#region license
// Razor: An Ultima Online Assistant
// Copyright (c) 2022 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Assistant.Filters;

namespace Assistant
{
    /// <summary>
    /// 
    /// </summary>
    public class SoundEntry : Form
    {
        private Button btnOk;
        private Button btnCancel;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private int _soundId;
        private string _soundName;
        private ComboBox _soundList;
        private CheckBox _playInClient;
        private Button _playSound;

        public delegate void SoundEntryCallback(int sound);

        public static SoundEntryCallback Callback = null;

        public int Sound => _soundId;

        public SoundEntry() : this(0, string.Empty)
        {
        }

        public SoundEntry(int soundId, string soundName)
        {
            _soundId = soundId;
            _soundName = soundName;
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this._soundList = new System.Windows.Forms.ComboBox();
            this._playInClient = new System.Windows.Forms.CheckBox();
            this._playSound = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(107, 92);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(52, 35);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "&OK";
            this.btnOk.Click += new System.EventHandler(this.okay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 92);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(52, 35);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // soundList
            // 
            this._soundList.FormattingEnabled = true;
            this._soundList.Location = new System.Drawing.Point(12, 12);
            this._soundList.Name = "_soundList";
            this._soundList.Size = new System.Drawing.Size(147, 23);
            this._soundList.TabIndex = 6;
            // 
            // playInClient
            // 
            this._playInClient.AutoSize = true;
            this._playInClient.Location = new System.Drawing.Point(41, 70);
            this._playInClient.Name = "_playInClient";
            this._playInClient.Size = new System.Drawing.Size(93, 19);
            this._playInClient.TabIndex = 8;
            this._playInClient.Text = "Play in client";
            this._playInClient.UseVisualStyleBackColor = true;
            // 
            // playSound
            // 
            this._playSound.Location = new System.Drawing.Point(22, 41);
            this._playSound.Name = "_playSound";
            this._playSound.Size = new System.Drawing.Size(127, 23);
            this._playSound.TabIndex = 7;
            this._playSound.Text = "Play Selected Sound";
            this._playSound.UseVisualStyleBackColor = true;
            this._playSound.Click += new System.EventHandler(this.playSound_Click);
            // 
            // SoundEntry
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(174, 142);
            this.ControlBox = false;
            this.Controls.Add(this._playInClient);
            this.Controls.Add(this._playSound);
            this.Controls.Add(this._soundList);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SoundEntry";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select a Sound";
            this.Load += new System.EventHandler(this.SoundEntry_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        
        private void okay_Click(object sender, EventArgs e)
        {
            if (_soundList.SelectedIndex > 0)
            {
                SoundMusicManager.Sound sound = (SoundMusicManager.Sound)_soundList.SelectedItem;

                _soundId = sound.Serial;
                _soundName = sound.Name;
                DialogResult = DialogResult.OK;
                Close();
                Callback = null;
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
            Callback = null;
        }

        private void SoundEntry_Load(object sender, EventArgs e)
        {
            Language.LoadControlNames(this);

            int selectedIndex = 0;
            
            _soundList.BeginUpdate();
            _soundList.Items.Clear();

            int index = 0;
            foreach (SoundMusicManager.Sound sound in SoundMusicManager.GetAllSounds())
            {
                _soundList.Items.Add(sound);

                if (sound.Name.Equals(_soundName))
                {
                    selectedIndex = index;
                }

                index++;
            }

            _soundList.SelectedIndex = selectedIndex;
            
            _soundList.EndUpdate();
        }

        private System.Media.SoundPlayer sp = new System.Media.SoundPlayer();

        private void playSound_Click(object sender, EventArgs e)
        {
            sp.Stop();

            if (_soundList.SelectedIndex < 0)
                return;

            SoundMusicManager.Sound sound = (SoundMusicManager.Sound)_soundList.SelectedItem;

            if (_playInClient.Checked && World.Player != null)
            {
                Client.Instance.SendToClient(new PlaySound(sound.Serial));
            }
            else
            {
                Ultima.UOSound s = Ultima.Sounds.GetSound(sound.Serial);
                using (MemoryStream m = new MemoryStream(s.buffer))
                {
                    sp.Stream = m;
                    sp.Play();
                }
            }
        }
    }
}