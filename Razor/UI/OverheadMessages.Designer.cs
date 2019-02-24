namespace Assistant.UI
{
    partial class OverheadMessages
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
            this.cancelOverheadMessages = new System.Windows.Forms.Button();
            this.saveOverheadMessages = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.overheadFormat = new System.Windows.Forms.TextBox();
            this.removeOverheadMessage = new System.Windows.Forms.Button();
            this.setOverheadMessage = new System.Windows.Forms.Button();
            this.cliLocOverheadView = new System.Windows.Forms.ListView();
            this.cliLocOriginal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cliLocOverheadMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cliLocSearch = new System.Windows.Forms.Button();
            this.cliLocTextSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cliLocSearchView = new System.Windows.Forms.ListView();
            this.cliLocSearchNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cliLocSearchText = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.unicodeStyle = new System.Windows.Forms.RadioButton();
            this.asciiStyle = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.setColorHue = new System.Windows.Forms.Button();
            this.editOverheadMessage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cancelOverheadMessages
            // 
            this.cancelOverheadMessages.Location = new System.Drawing.Point(334, 307);
            this.cancelOverheadMessages.Name = "cancelOverheadMessages";
            this.cancelOverheadMessages.Size = new System.Drawing.Size(54, 28);
            this.cancelOverheadMessages.TabIndex = 20;
            this.cancelOverheadMessages.Text = "Cancel";
            this.cancelOverheadMessages.UseVisualStyleBackColor = true;
            this.cancelOverheadMessages.Click += new System.EventHandler(this.cancelOverheadMessages_Click);
            // 
            // saveOverheadMessages
            // 
            this.saveOverheadMessages.Location = new System.Drawing.Point(394, 307);
            this.saveOverheadMessages.Name = "saveOverheadMessages";
            this.saveOverheadMessages.Size = new System.Drawing.Size(54, 28);
            this.saveOverheadMessages.TabIndex = 19;
            this.saveOverheadMessages.Text = "OK";
            this.saveOverheadMessages.UseVisualStyleBackColor = true;
            this.saveOverheadMessages.Click += new System.EventHandler(this.saveOverheadMessages_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(302, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 15);
            this.label2.TabIndex = 30;
            this.label2.Text = "Format:";
            // 
            // overheadFormat
            // 
            this.overheadFormat.Location = new System.Drawing.Point(356, 149);
            this.overheadFormat.Name = "overheadFormat";
            this.overheadFormat.Size = new System.Drawing.Size(92, 23);
            this.overheadFormat.TabIndex = 29;
            this.overheadFormat.Text = "[{msg}]";
            this.overheadFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // removeOverheadMessage
            // 
            this.removeOverheadMessage.Location = new System.Drawing.Point(106, 307);
            this.removeOverheadMessage.Name = "removeOverheadMessage";
            this.removeOverheadMessage.Size = new System.Drawing.Size(85, 28);
            this.removeOverheadMessage.TabIndex = 28;
            this.removeOverheadMessage.Text = "Remove";
            this.removeOverheadMessage.UseVisualStyleBackColor = true;
            this.removeOverheadMessage.Click += new System.EventHandler(this.removeOverheadMessage_Click);
            // 
            // setOverheadMessage
            // 
            this.setOverheadMessage.Location = new System.Drawing.Point(15, 145);
            this.setOverheadMessage.Name = "setOverheadMessage";
            this.setOverheadMessage.Size = new System.Drawing.Size(150, 28);
            this.setOverheadMessage.TabIndex = 27;
            this.setOverheadMessage.Text = "Set Overhead Message";
            this.setOverheadMessage.UseVisualStyleBackColor = true;
            this.setOverheadMessage.Click += new System.EventHandler(this.setOverheadMessage_Click);
            // 
            // cliLocOverheadView
            // 
            this.cliLocOverheadView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cliLocOriginal,
            this.cliLocOverheadMessage});
            this.cliLocOverheadView.Location = new System.Drawing.Point(15, 179);
            this.cliLocOverheadView.Name = "cliLocOverheadView";
            this.cliLocOverheadView.Size = new System.Drawing.Size(433, 122);
            this.cliLocOverheadView.TabIndex = 26;
            this.cliLocOverheadView.UseCompatibleStateImageBehavior = false;
            this.cliLocOverheadView.View = System.Windows.Forms.View.Details;
            // 
            // cliLocOriginal
            // 
            this.cliLocOriginal.Text = "Original Text";
            this.cliLocOriginal.Width = 168;
            // 
            // cliLocOverheadMessage
            // 
            this.cliLocOverheadMessage.Text = "Overhead Message";
            this.cliLocOverheadMessage.Width = 229;
            // 
            // cliLocSearch
            // 
            this.cliLocSearch.Location = new System.Drawing.Point(272, 6);
            this.cliLocSearch.Name = "cliLocSearch";
            this.cliLocSearch.Size = new System.Drawing.Size(78, 23);
            this.cliLocSearch.TabIndex = 25;
            this.cliLocSearch.Text = "Search";
            this.cliLocSearch.UseVisualStyleBackColor = true;
            this.cliLocSearch.Click += new System.EventHandler(this.cliLocSearch_Click);
            // 
            // cliLocTextSearch
            // 
            this.cliLocTextSearch.Location = new System.Drawing.Point(87, 6);
            this.cliLocTextSearch.Name = "cliLocTextSearch";
            this.cliLocTextSearch.Size = new System.Drawing.Size(179, 23);
            this.cliLocTextSearch.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 15);
            this.label1.TabIndex = 23;
            this.label1.Text = "Search Text:";
            // 
            // cliLocSearchView
            // 
            this.cliLocSearchView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cliLocSearchNumber,
            this.cliLocSearchText});
            this.cliLocSearchView.Location = new System.Drawing.Point(15, 41);
            this.cliLocSearchView.Name = "cliLocSearchView";
            this.cliLocSearchView.Size = new System.Drawing.Size(433, 98);
            this.cliLocSearchView.TabIndex = 22;
            this.cliLocSearchView.UseCompatibleStateImageBehavior = false;
            this.cliLocSearchView.View = System.Windows.Forms.View.Details;
            // 
            // cliLocSearchNumber
            // 
            this.cliLocSearchNumber.Text = "Number";
            this.cliLocSearchNumber.Width = 75;
            // 
            // cliLocSearchText
            // 
            this.cliLocSearchText.Text = "Text";
            this.cliLocSearchText.Width = 313;
            // 
            // unicodeStyle
            // 
            this.unicodeStyle.AutoSize = true;
            this.unicodeStyle.Location = new System.Drawing.Point(205, 342);
            this.unicodeStyle.Name = "unicodeStyle";
            this.unicodeStyle.Size = new System.Drawing.Size(69, 19);
            this.unicodeStyle.TabIndex = 44;
            this.unicodeStyle.TabStop = true;
            this.unicodeStyle.Text = "Unicode";
            this.unicodeStyle.UseVisualStyleBackColor = true;
            // 
            // asciiStyle
            // 
            this.asciiStyle.AutoSize = true;
            this.asciiStyle.Location = new System.Drawing.Point(280, 342);
            this.asciiStyle.Name = "asciiStyle";
            this.asciiStyle.Size = new System.Drawing.Size(53, 19);
            this.asciiStyle.TabIndex = 43;
            this.asciiStyle.TabStop = true;
            this.asciiStyle.Text = "ASCII";
            this.asciiStyle.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(115, 344);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 15);
            this.label3.TabIndex = 45;
            this.label3.Text = "Message Style:";
            // 
            // setColorHue
            // 
            this.setColorHue.Location = new System.Drawing.Point(197, 307);
            this.setColorHue.Name = "setColorHue";
            this.setColorHue.Size = new System.Drawing.Size(110, 28);
            this.setColorHue.TabIndex = 46;
            this.setColorHue.Text = "Set Color/Hue";
            this.setColorHue.UseVisualStyleBackColor = true;
            this.setColorHue.Click += new System.EventHandler(this.setColorHue_Click);
            // 
            // editOverheadMessage
            // 
            this.editOverheadMessage.Location = new System.Drawing.Point(15, 307);
            this.editOverheadMessage.Name = "editOverheadMessage";
            this.editOverheadMessage.Size = new System.Drawing.Size(85, 28);
            this.editOverheadMessage.TabIndex = 47;
            this.editOverheadMessage.Text = "Edit";
            this.editOverheadMessage.UseVisualStyleBackColor = true;
            this.editOverheadMessage.Click += new System.EventHandler(this.editOverheadMessage_Click);
            // 
            // OverheadMessages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 368);
            this.Controls.Add(this.editOverheadMessage);
            this.Controls.Add(this.setColorHue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.unicodeStyle);
            this.Controls.Add(this.asciiStyle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.overheadFormat);
            this.Controls.Add(this.removeOverheadMessage);
            this.Controls.Add(this.setOverheadMessage);
            this.Controls.Add(this.cliLocOverheadView);
            this.Controls.Add(this.cliLocSearch);
            this.Controls.Add(this.cliLocTextSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cliLocSearchView);
            this.Controls.Add(this.cancelOverheadMessages);
            this.Controls.Add(this.saveOverheadMessages);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "OverheadMessages";
            this.Text = "Overhead Messages";
            this.Load += new System.EventHandler(this.OverheadMessages_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button cancelOverheadMessages;
        private System.Windows.Forms.Button saveOverheadMessages;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox overheadFormat;
        private System.Windows.Forms.Button removeOverheadMessage;
        private System.Windows.Forms.Button setOverheadMessage;
        private System.Windows.Forms.ListView cliLocOverheadView;
        private System.Windows.Forms.ColumnHeader cliLocOriginal;
        private System.Windows.Forms.ColumnHeader cliLocOverheadMessage;
        private System.Windows.Forms.Button cliLocSearch;
        private System.Windows.Forms.TextBox cliLocTextSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView cliLocSearchView;
        private System.Windows.Forms.ColumnHeader cliLocSearchNumber;
        private System.Windows.Forms.ColumnHeader cliLocSearchText;
        private System.Windows.Forms.RadioButton unicodeStyle;
        private System.Windows.Forms.RadioButton asciiStyle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button setColorHue;
        private System.Windows.Forms.Button editOverheadMessage;
    }
}