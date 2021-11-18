namespace Assistant.UI
{
    partial class BuffDebuffOptions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BuffDebuffOptions));
            this.grpFilter = new System.Windows.Forms.GroupBox();
            this.removeFilter = new System.Windows.Forms.Button();
            this.addFilter = new System.Windows.Forms.Button();
            this.buffDebuffFilters = new System.Windows.Forms.ListBox();
            this.lblSeconds = new System.Windows.Forms.Label();
            this.buffDebuffSeconds = new System.Windows.Forms.TextBox();
            this.displayBuffDebuffEvery = new System.Windows.Forms.CheckBox();
            this.buffDebuffFormat = new System.Windows.Forms.TextBox();
            this.setDebuffHue = new System.Windows.Forms.Button();
            this.lblDebuffHue = new System.Windows.Forms.Label();
            this.setBuffHue = new System.Windows.Forms.Button();
            this.lblBuffHue = new System.Windows.Forms.Label();
            this.buffFormatKey = new System.Windows.Forms.Button();
            this.okClose = new System.Windows.Forms.Button();
            this.overrideBuffDebuffFormat = new System.Windows.Forms.CheckBox();
            this.grpFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpFilter
            // 
            this.grpFilter.Controls.Add(this.removeFilter);
            this.grpFilter.Controls.Add(this.addFilter);
            this.grpFilter.Controls.Add(this.buffDebuffFilters);
            this.grpFilter.Location = new System.Drawing.Point(12, 12);
            this.grpFilter.Name = "grpFilter";
            this.grpFilter.Size = new System.Drawing.Size(319, 140);
            this.grpFilter.TabIndex = 65;
            this.grpFilter.TabStop = false;
            this.grpFilter.Text = "Filter:";
            // 
            // removeFilter
            // 
            this.removeFilter.Location = new System.Drawing.Point(230, 57);
            this.removeFilter.Name = "removeFilter";
            this.removeFilter.Size = new System.Drawing.Size(83, 29);
            this.removeFilter.TabIndex = 76;
            this.removeFilter.Text = "Remove";
            this.removeFilter.UseVisualStyleBackColor = true;
            this.removeFilter.Click += new System.EventHandler(this.RemoveFilter_Click);
            // 
            // addFilter
            // 
            this.addFilter.Location = new System.Drawing.Point(230, 22);
            this.addFilter.Name = "addFilter";
            this.addFilter.Size = new System.Drawing.Size(83, 29);
            this.addFilter.TabIndex = 75;
            this.addFilter.Text = "Add";
            this.addFilter.UseVisualStyleBackColor = true;
            this.addFilter.Click += new System.EventHandler(this.AddFilter_Click);
            // 
            // buffDebuffFilters
            // 
            this.buffDebuffFilters.FormattingEnabled = true;
            this.buffDebuffFilters.ItemHeight = 15;
            this.buffDebuffFilters.Location = new System.Drawing.Point(6, 22);
            this.buffDebuffFilters.Name = "buffDebuffFilters";
            this.buffDebuffFilters.Size = new System.Drawing.Size(218, 109);
            this.buffDebuffFilters.TabIndex = 74;
            // 
            // lblSeconds
            // 
            this.lblSeconds.AutoSize = true;
            this.lblSeconds.Location = new System.Drawing.Point(225, 213);
            this.lblSeconds.Name = "lblSeconds";
            this.lblSeconds.Size = new System.Drawing.Size(50, 15);
            this.lblSeconds.TabIndex = 82;
            this.lblSeconds.Text = "seconds";
            // 
            // buffDebuffSeconds
            // 
            this.buffDebuffSeconds.Location = new System.Drawing.Point(175, 210);
            this.buffDebuffSeconds.Name = "buffDebuffSeconds";
            this.buffDebuffSeconds.Size = new System.Drawing.Size(47, 23);
            this.buffDebuffSeconds.TabIndex = 81;
            this.buffDebuffSeconds.Text = "20";
            this.buffDebuffSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.buffDebuffSeconds.TextChanged += new System.EventHandler(this.BuffDebuffSeconds_TextChanged);
            // 
            // displayBuffDebuffEvery
            // 
            this.displayBuffDebuffEvery.AutoSize = true;
            this.displayBuffDebuffEvery.Location = new System.Drawing.Point(18, 212);
            this.displayBuffDebuffEvery.Name = "displayBuffDebuffEvery";
            this.displayBuffDebuffEvery.Size = new System.Drawing.Size(160, 19);
            this.displayBuffDebuffEvery.TabIndex = 80;
            this.displayBuffDebuffEvery.Text = "Display buff/debuff every";
            this.displayBuffDebuffEvery.UseVisualStyleBackColor = true;
            this.displayBuffDebuffEvery.CheckedChanged += new System.EventHandler(this.DisplayBuffDebuffEvery_CheckedChanged);
            // 
            // buffDebuffFormat
            // 
            this.buffDebuffFormat.Location = new System.Drawing.Point(18, 183);
            this.buffDebuffFormat.Name = "buffDebuffFormat";
            this.buffDebuffFormat.Size = new System.Drawing.Size(224, 23);
            this.buffDebuffFormat.TabIndex = 79;
            this.buffDebuffFormat.Text = "[{action}{name} {duration}]";
            this.buffDebuffFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.buffDebuffFormat.TextChanged += new System.EventHandler(this.BuffDebuffFormat_TextChanged);
            // 
            // setDebuffHue
            // 
            this.setDebuffHue.Location = new System.Drawing.Point(175, 260);
            this.setDebuffHue.Name = "setDebuffHue";
            this.setDebuffHue.Size = new System.Drawing.Size(47, 20);
            this.setDebuffHue.TabIndex = 78;
            this.setDebuffHue.Text = "Set";
            this.setDebuffHue.Click += new System.EventHandler(this.SetDebuffHue_Click);
            // 
            // lblDebuffHue
            // 
            this.lblDebuffHue.Location = new System.Drawing.Point(15, 262);
            this.lblDebuffHue.Name = "lblDebuffHue";
            this.lblDebuffHue.Size = new System.Drawing.Size(207, 18);
            this.lblDebuffHue.TabIndex = 77;
            this.lblDebuffHue.Text = "Debuff Hue:";
            // 
            // setBuffHue
            // 
            this.setBuffHue.Location = new System.Drawing.Point(175, 237);
            this.setBuffHue.Name = "setBuffHue";
            this.setBuffHue.Size = new System.Drawing.Size(47, 20);
            this.setBuffHue.TabIndex = 76;
            this.setBuffHue.Text = "Set";
            this.setBuffHue.Click += new System.EventHandler(this.SetBuffHue_Click);
            // 
            // lblBuffHue
            // 
            this.lblBuffHue.Location = new System.Drawing.Point(15, 239);
            this.lblBuffHue.Name = "lblBuffHue";
            this.lblBuffHue.Size = new System.Drawing.Size(207, 18);
            this.lblBuffHue.TabIndex = 75;
            this.lblBuffHue.Text = "Buff Hue:";
            // 
            // buffFormatKey
            // 
            this.buffFormatKey.Location = new System.Drawing.Point(248, 183);
            this.buffFormatKey.Name = "buffFormatKey";
            this.buffFormatKey.Size = new System.Drawing.Size(16, 23);
            this.buffFormatKey.TabIndex = 84;
            this.buffFormatKey.Text = "?";
            this.buffFormatKey.UseVisualStyleBackColor = true;
            this.buffFormatKey.Click += new System.EventHandler(this.BuffFormatKey_Click);
            // 
            // okClose
            // 
            this.okClose.Location = new System.Drawing.Point(248, 251);
            this.okClose.Name = "okClose";
            this.okClose.Size = new System.Drawing.Size(83, 29);
            this.okClose.TabIndex = 85;
            this.okClose.Text = "OK";
            this.okClose.UseVisualStyleBackColor = true;
            this.okClose.Click += new System.EventHandler(this.OkClose_Click);
            // 
            // overrideBuffDebuffFormat
            // 
            this.overrideBuffDebuffFormat.AutoSize = true;
            this.overrideBuffDebuffFormat.Location = new System.Drawing.Point(18, 158);
            this.overrideBuffDebuffFormat.Name = "overrideBuffDebuffFormat";
            this.overrideBuffDebuffFormat.Size = new System.Drawing.Size(175, 19);
            this.overrideBuffDebuffFormat.TabIndex = 86;
            this.overrideBuffDebuffFormat.Text = "Override buff/debuff format";
            this.overrideBuffDebuffFormat.UseVisualStyleBackColor = true;
            this.overrideBuffDebuffFormat.CheckedChanged += new System.EventHandler(this.overrideBuffDebuffFormat_CheckedChanged);
            // 
            // BuffDebuff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 291);
            this.Controls.Add(this.overrideBuffDebuffFormat);
            this.Controls.Add(this.okClose);
            this.Controls.Add(this.buffFormatKey);
            this.Controls.Add(this.lblSeconds);
            this.Controls.Add(this.buffDebuffSeconds);
            this.Controls.Add(this.displayBuffDebuffEvery);
            this.Controls.Add(this.buffDebuffFormat);
            this.Controls.Add(this.setDebuffHue);
            this.Controls.Add(this.lblDebuffHue);
            this.Controls.Add(this.setBuffHue);
            this.Controls.Add(this.lblBuffHue);
            this.Controls.Add(this.grpFilter);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "BuffDebuff";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Buff/Debuff Options";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.BuffDebuff_Closing);
            this.Load += new System.EventHandler(this.BuffDebuff_Load);
            this.grpFilter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpFilter;
        private System.Windows.Forms.Button removeFilter;
        private System.Windows.Forms.Button addFilter;
        private System.Windows.Forms.ListBox buffDebuffFilters;
        private System.Windows.Forms.Label lblSeconds;
        private System.Windows.Forms.TextBox buffDebuffSeconds;
        private System.Windows.Forms.CheckBox displayBuffDebuffEvery;
        private System.Windows.Forms.TextBox buffDebuffFormat;
        private System.Windows.Forms.Button setDebuffHue;
        private System.Windows.Forms.Label lblDebuffHue;
        private System.Windows.Forms.Button setBuffHue;
        private System.Windows.Forms.Label lblBuffHue;
        private System.Windows.Forms.Button buffFormatKey;
        private System.Windows.Forms.Button okClose;
        private System.Windows.Forms.CheckBox overrideBuffDebuffFormat;
    }
}