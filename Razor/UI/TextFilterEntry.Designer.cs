
namespace Assistant.UI
{
    partial class TextFilterEntry
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
            this.checkBoxFilterSysMessages = new System.Windows.Forms.CheckBox();
            this.filterTextBox = new System.Windows.Forms.TextBox();
            this.enterTextLabel = new System.Windows.Forms.Label();
            this.checkBoxFilterOverhead = new System.Windows.Forms.CheckBox();
            this.checkBoxFilterSpeech = new System.Windows.Forms.CheckBox();
            this.checkBoxIgnoreFilteredInScripts = new System.Windows.Forms.CheckBox();
            this.cancel = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkBoxFilterSysMessages
            // 
            this.checkBoxFilterSysMessages.AutoSize = true;
            this.checkBoxFilterSysMessages.Location = new System.Drawing.Point(8, 68);
            this.checkBoxFilterSysMessages.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBoxFilterSysMessages.Name = "checkBoxFilterSysMessages";
            this.checkBoxFilterSysMessages.Size = new System.Drawing.Size(181, 24);
            this.checkBoxFilterSysMessages.TabIndex = 1;
            this.checkBoxFilterSysMessages.Text = "Filter system messages";
            this.checkBoxFilterSysMessages.UseVisualStyleBackColor = true;
            // 
            // filterTextBox
            // 
            this.filterTextBox.Location = new System.Drawing.Point(8, 33);
            this.filterTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.filterTextBox.Name = "filterTextBox";
            this.filterTextBox.Size = new System.Drawing.Size(355, 27);
            this.filterTextBox.TabIndex = 0;
            this.filterTextBox.TextChanged += new System.EventHandler(this.filterTextBox_TextChanged);
            // 
            // enterTextLabel
            // 
            this.enterTextLabel.AutoSize = true;
            this.enterTextLabel.Location = new System.Drawing.Point(5, 9);
            this.enterTextLabel.Name = "enterTextLabel";
            this.enterTextLabel.Size = new System.Drawing.Size(125, 20);
            this.enterTextLabel.TabIndex = 2;
            this.enterTextLabel.Text = "Enter text to filter";
            // 
            // checkBoxFilterOverhead
            // 
            this.checkBoxFilterOverhead.Location = new System.Drawing.Point(8, 100);
            this.checkBoxFilterOverhead.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBoxFilterOverhead.Name = "checkBoxFilterOverhead";
            this.checkBoxFilterOverhead.Size = new System.Drawing.Size(130, 24);
            this.checkBoxFilterOverhead.TabIndex = 2;
            this.checkBoxFilterOverhead.Text = "Filter overhead";
            this.checkBoxFilterOverhead.UseVisualStyleBackColor = true;
            // 
            // checkBoxFilterSpeech
            // 
            this.checkBoxFilterSpeech.AutoSize = true;
            this.checkBoxFilterSpeech.Location = new System.Drawing.Point(8, 132);
            this.checkBoxFilterSpeech.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBoxFilterSpeech.Name = "checkBoxFilterSpeech";
            this.checkBoxFilterSpeech.Size = new System.Drawing.Size(114, 24);
            this.checkBoxFilterSpeech.TabIndex = 3;
            this.checkBoxFilterSpeech.Text = "Filter speech";
            this.checkBoxFilterSpeech.UseVisualStyleBackColor = true;
            // 
            // checkBoxIgnoreFilteredInScripts
            // 
            this.checkBoxIgnoreFilteredInScripts.AutoSize = true;
            this.checkBoxIgnoreFilteredInScripts.Location = new System.Drawing.Point(8, 164);
            this.checkBoxIgnoreFilteredInScripts.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBoxIgnoreFilteredInScripts.Name = "checkBoxIgnoreFilteredInScripts";
            this.checkBoxIgnoreFilteredInScripts.Size = new System.Drawing.Size(351, 24);
            this.checkBoxIgnoreFilteredInScripts.TabIndex = 4;
            this.checkBoxIgnoreFilteredInScripts.Text = "Ignore filtered messages in overhead and scripts";
            this.checkBoxIgnoreFilteredInScripts.UseVisualStyleBackColor = true;
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(8, 196);
            this.cancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(117, 29);
            this.cancel.TabIndex = 6;
            this.cancel.Text = "Cancel";
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(250, 196);
            this.ok.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(117, 29);
            this.ok.TabIndex = 5;
            this.ok.Text = "Okay";
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // TextFilterEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(379, 238);
            this.ControlBox = false;
            this.Controls.Add(this.ok);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.checkBoxIgnoreFilteredInScripts);
            this.Controls.Add(this.checkBoxFilterSpeech);
            this.Controls.Add(this.checkBoxFilterOverhead);
            this.Controls.Add(this.enterTextLabel);
            this.Controls.Add(this.filterTextBox);
            this.Controls.Add(this.checkBoxFilterSysMessages);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "TextFilterEntry";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Text filter options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxFilterSysMessages;
        private System.Windows.Forms.TextBox filterTextBox;
        private System.Windows.Forms.Label enterTextLabel;
        private System.Windows.Forms.CheckBox checkBoxFilterOverhead;
        private System.Windows.Forms.CheckBox checkBoxFilterSpeech;
        private System.Windows.Forms.CheckBox checkBoxIgnoreFilteredInScripts;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button ok;
    }
}