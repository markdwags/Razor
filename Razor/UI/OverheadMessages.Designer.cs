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
            this.cliLocSearchView = new System.Windows.Forms.ListView();
            this.cliLocSearchNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cliLocSearchText = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.cliLocTextSearch = new System.Windows.Forms.TextBox();
            this.cliLocSearch = new System.Windows.Forms.Button();
            this.cliLocOverheadView = new System.Windows.Forms.ListView();
            this.cliLocOriginal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cliLocOverheadMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.setOverheadMessage = new System.Windows.Forms.Button();
            this.saveOverheadMessages = new System.Windows.Forms.Button();
            this.cancelOverheadMessages = new System.Windows.Forms.Button();
            this.removeOverheadMessage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cliLocSearchView
            // 
            this.cliLocSearchView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cliLocSearchNumber,
            this.cliLocSearchText});
            this.cliLocSearchView.Location = new System.Drawing.Point(12, 45);
            this.cliLocSearchView.Name = "cliLocSearchView";
            this.cliLocSearchView.Size = new System.Drawing.Size(433, 98);
            this.cliLocSearchView.TabIndex = 0;
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search Text:";
            // 
            // cliLocTextSearch
            // 
            this.cliLocTextSearch.Location = new System.Drawing.Point(84, 10);
            this.cliLocTextSearch.Name = "cliLocTextSearch";
            this.cliLocTextSearch.Size = new System.Drawing.Size(179, 23);
            this.cliLocTextSearch.TabIndex = 2;
            // 
            // cliLocSearch
            // 
            this.cliLocSearch.Location = new System.Drawing.Point(269, 10);
            this.cliLocSearch.Name = "cliLocSearch";
            this.cliLocSearch.Size = new System.Drawing.Size(78, 23);
            this.cliLocSearch.TabIndex = 3;
            this.cliLocSearch.Text = "Search";
            this.cliLocSearch.UseVisualStyleBackColor = true;
            this.cliLocSearch.Click += new System.EventHandler(this.cliLocSearch_Click);
            // 
            // cliLocOverheadView
            // 
            this.cliLocOverheadView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cliLocOriginal,
            this.cliLocOverheadMessage});
            this.cliLocOverheadView.Location = new System.Drawing.Point(12, 183);
            this.cliLocOverheadView.Name = "cliLocOverheadView";
            this.cliLocOverheadView.Size = new System.Drawing.Size(433, 122);
            this.cliLocOverheadView.TabIndex = 4;
            this.cliLocOverheadView.UseCompatibleStateImageBehavior = false;
            this.cliLocOverheadView.View = System.Windows.Forms.View.Details;
            // 
            // cliLocOriginal
            // 
            this.cliLocOriginal.Text = "Original Text";
            this.cliLocOriginal.Width = 103;
            // 
            // cliLocOverheadMessage
            // 
            this.cliLocOverheadMessage.Text = "Overhead Message";
            this.cliLocOverheadMessage.Width = 229;
            // 
            // setOverheadMessage
            // 
            this.setOverheadMessage.Location = new System.Drawing.Point(12, 149);
            this.setOverheadMessage.Name = "setOverheadMessage";
            this.setOverheadMessage.Size = new System.Drawing.Size(150, 28);
            this.setOverheadMessage.TabIndex = 5;
            this.setOverheadMessage.Text = "Set Overhead Message";
            this.setOverheadMessage.UseVisualStyleBackColor = true;
            this.setOverheadMessage.Click += new System.EventHandler(this.setOverheadMessage_Click);
            // 
            // saveOverheadMessages
            // 
            this.saveOverheadMessages.Location = new System.Drawing.Point(391, 311);
            this.saveOverheadMessages.Name = "saveOverheadMessages";
            this.saveOverheadMessages.Size = new System.Drawing.Size(54, 28);
            this.saveOverheadMessages.TabIndex = 6;
            this.saveOverheadMessages.Text = "OK";
            this.saveOverheadMessages.UseVisualStyleBackColor = true;
            this.saveOverheadMessages.Click += new System.EventHandler(this.saveOverheadMessages_Click);
            // 
            // cancelOverheadMessages
            // 
            this.cancelOverheadMessages.Location = new System.Drawing.Point(331, 311);
            this.cancelOverheadMessages.Name = "cancelOverheadMessages";
            this.cancelOverheadMessages.Size = new System.Drawing.Size(54, 28);
            this.cancelOverheadMessages.TabIndex = 7;
            this.cancelOverheadMessages.Text = "Cancel";
            this.cancelOverheadMessages.UseVisualStyleBackColor = true;
            this.cancelOverheadMessages.Click += new System.EventHandler(this.cancelOverheadMessages_Click);
            // 
            // removeOverheadMessage
            // 
            this.removeOverheadMessage.Location = new System.Drawing.Point(12, 311);
            this.removeOverheadMessage.Name = "removeOverheadMessage";
            this.removeOverheadMessage.Size = new System.Drawing.Size(121, 28);
            this.removeOverheadMessage.TabIndex = 8;
            this.removeOverheadMessage.Text = "Remove Selected";
            this.removeOverheadMessage.UseVisualStyleBackColor = true;
            this.removeOverheadMessage.Click += new System.EventHandler(this.removeOverheadMessage_Click);
            // 
            // OverheadMessages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 346);
            this.Controls.Add(this.removeOverheadMessage);
            this.Controls.Add(this.cancelOverheadMessages);
            this.Controls.Add(this.saveOverheadMessages);
            this.Controls.Add(this.setOverheadMessage);
            this.Controls.Add(this.cliLocOverheadView);
            this.Controls.Add(this.cliLocSearch);
            this.Controls.Add(this.cliLocTextSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cliLocSearchView);
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

        private System.Windows.Forms.ListView cliLocSearchView;
        private System.Windows.Forms.ColumnHeader cliLocSearchNumber;
        private System.Windows.Forms.ColumnHeader cliLocSearchText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox cliLocTextSearch;
        private System.Windows.Forms.Button cliLocSearch;
        private System.Windows.Forms.ListView cliLocOverheadView;
        private System.Windows.Forms.ColumnHeader cliLocOriginal;
        private System.Windows.Forms.ColumnHeader cliLocOverheadMessage;
        private System.Windows.Forms.Button setOverheadMessage;
        private System.Windows.Forms.Button saveOverheadMessages;
        private System.Windows.Forms.Button cancelOverheadMessages;
        private System.Windows.Forms.Button removeOverheadMessage;
    }
}