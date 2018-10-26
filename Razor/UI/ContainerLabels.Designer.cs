namespace Assistant.UI
{
    partial class ContainerLabels
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
            this.cancelContainerLabels = new System.Windows.Forms.Button();
            this.saveContainerLabels = new System.Windows.Forms.Button();
            this.lblFormat = new System.Windows.Forms.Label();
            this.containerLabelFormat = new System.Windows.Forms.TextBox();
            this.removeContainerLabel = new System.Windows.Forms.Button();
            this.addContainLabel = new System.Windows.Forms.Button();
            this.containerView = new System.Windows.Forms.ListView();
            this.containerId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.containerType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.containerLabel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.setExHue = new System.Windows.Forms.Button();
            this.lblContainerHue = new System.Windows.Forms.Label();
            this.setColorHue = new System.Windows.Forms.Button();
            this.asciiStyle = new System.Windows.Forms.RadioButton();
            this.unicodeStyle = new System.Windows.Forms.RadioButton();
            this.lblStyle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cancelContainerLabels
            // 
            this.cancelContainerLabels.Location = new System.Drawing.Point(391, 233);
            this.cancelContainerLabels.Name = "cancelContainerLabels";
            this.cancelContainerLabels.Size = new System.Drawing.Size(54, 28);
            this.cancelContainerLabels.TabIndex = 20;
            this.cancelContainerLabels.Text = "Cancel";
            this.cancelContainerLabels.UseVisualStyleBackColor = true;
            this.cancelContainerLabels.Click += new System.EventHandler(this.cancelOverheadMessages_Click);
            // 
            // saveContainerLabels
            // 
            this.saveContainerLabels.Location = new System.Drawing.Point(449, 233);
            this.saveContainerLabels.Name = "saveContainerLabels";
            this.saveContainerLabels.Size = new System.Drawing.Size(50, 28);
            this.saveContainerLabels.TabIndex = 19;
            this.saveContainerLabels.Text = "OK";
            this.saveContainerLabels.UseVisualStyleBackColor = true;
            this.saveContainerLabels.Click += new System.EventHandler(this.saveContainerLabels_Click);
            // 
            // lblFormat
            // 
            this.lblFormat.AutoSize = true;
            this.lblFormat.Location = new System.Drawing.Point(12, 240);
            this.lblFormat.Name = "lblFormat";
            this.lblFormat.Size = new System.Drawing.Size(79, 15);
            this.lblFormat.TabIndex = 30;
            this.lblFormat.Text = "Label Format:";
            // 
            // containerLabelFormat
            // 
            this.containerLabelFormat.Location = new System.Drawing.Point(97, 237);
            this.containerLabelFormat.Name = "containerLabelFormat";
            this.containerLabelFormat.Size = new System.Drawing.Size(132, 23);
            this.containerLabelFormat.TabIndex = 29;
            this.containerLabelFormat.Text = "[{label}] ({name})";
            this.containerLabelFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.containerLabelFormat.TextChanged += new System.EventHandler(this.containerLabelFormat_TextChanged);
            // 
            // removeContainerLabel
            // 
            this.removeContainerLabel.Location = new System.Drawing.Point(391, 104);
            this.removeContainerLabel.Name = "removeContainerLabel";
            this.removeContainerLabel.Size = new System.Drawing.Size(108, 40);
            this.removeContainerLabel.TabIndex = 28;
            this.removeContainerLabel.Text = "Remove Selected";
            this.removeContainerLabel.UseVisualStyleBackColor = true;
            this.removeContainerLabel.Click += new System.EventHandler(this.removeContainerLabel_Click);
            // 
            // addContainLabel
            // 
            this.addContainLabel.Location = new System.Drawing.Point(391, 12);
            this.addContainLabel.Name = "addContainLabel";
            this.addContainLabel.Size = new System.Drawing.Size(108, 40);
            this.addContainLabel.TabIndex = 27;
            this.addContainLabel.Text = "Add Container Label";
            this.addContainLabel.UseVisualStyleBackColor = true;
            this.addContainLabel.Click += new System.EventHandler(this.addContainLabel_Click);
            // 
            // containerView
            // 
            this.containerView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.containerId,
            this.containerType,
            this.containerLabel});
            this.containerView.FullRowSelect = true;
            this.containerView.Location = new System.Drawing.Point(12, 12);
            this.containerView.MultiSelect = false;
            this.containerView.Name = "containerView";
            this.containerView.Size = new System.Drawing.Size(373, 215);
            this.containerView.TabIndex = 22;
            this.containerView.UseCompatibleStateImageBehavior = false;
            this.containerView.View = System.Windows.Forms.View.Details;
            this.containerView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDownContainerView);
            // 
            // containerId
            // 
            this.containerId.Text = "ID";
            this.containerId.Width = 88;
            // 
            // containerType
            // 
            this.containerType.Text = "Type";
            this.containerType.Width = 100;
            // 
            // containerLabel
            // 
            this.containerLabel.Text = "Label";
            this.containerLabel.Width = 180;
            // 
            // setExHue
            // 
            this.setExHue.Location = new System.Drawing.Point(182, 263);
            this.setExHue.Name = "setExHue";
            this.setExHue.Size = new System.Drawing.Size(47, 21);
            this.setExHue.TabIndex = 39;
            this.setExHue.Text = "Set";
            this.setExHue.Click += new System.EventHandler(this.setExHue_Click);
            // 
            // lblContainerHue
            // 
            this.lblContainerHue.Location = new System.Drawing.Point(12, 263);
            this.lblContainerHue.Name = "lblContainerHue";
            this.lblContainerHue.Size = new System.Drawing.Size(217, 21);
            this.lblContainerHue.TabIndex = 38;
            this.lblContainerHue.Text = "Default Label Hue:";
            this.lblContainerHue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // setColorHue
            // 
            this.setColorHue.Location = new System.Drawing.Point(391, 58);
            this.setColorHue.Name = "setColorHue";
            this.setColorHue.Size = new System.Drawing.Size(108, 40);
            this.setColorHue.TabIndex = 40;
            this.setColorHue.Text = "Set Color/Hue";
            this.setColorHue.UseVisualStyleBackColor = true;
            this.setColorHue.Click += new System.EventHandler(this.setColorHue_Click);
            // 
            // asciiStyle
            // 
            this.asciiStyle.AutoSize = true;
            this.asciiStyle.Location = new System.Drawing.Point(309, 238);
            this.asciiStyle.Name = "asciiStyle";
            this.asciiStyle.Size = new System.Drawing.Size(53, 19);
            this.asciiStyle.TabIndex = 41;
            this.asciiStyle.TabStop = true;
            this.asciiStyle.Text = "ASCII";
            this.asciiStyle.UseVisualStyleBackColor = true;
            this.asciiStyle.CheckedChanged += new System.EventHandler(this.asciiFormat_CheckedChanged);
            // 
            // unicodeStyle
            // 
            this.unicodeStyle.AutoSize = true;
            this.unicodeStyle.Location = new System.Drawing.Point(309, 263);
            this.unicodeStyle.Name = "unicodeStyle";
            this.unicodeStyle.Size = new System.Drawing.Size(69, 19);
            this.unicodeStyle.TabIndex = 42;
            this.unicodeStyle.TabStop = true;
            this.unicodeStyle.Text = "Unicode";
            this.unicodeStyle.UseVisualStyleBackColor = true;
            // 
            // lblStyle
            // 
            this.lblStyle.AutoSize = true;
            this.lblStyle.Location = new System.Drawing.Point(237, 240);
            this.lblStyle.Name = "lblStyle";
            this.lblStyle.Size = new System.Drawing.Size(66, 15);
            this.lblStyle.TabIndex = 43;
            this.lblStyle.Text = "Label Style:";
            // 
            // ContainerLabels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(509, 294);
            this.Controls.Add(this.lblStyle);
            this.Controls.Add(this.unicodeStyle);
            this.Controls.Add(this.asciiStyle);
            this.Controls.Add(this.setColorHue);
            this.Controls.Add(this.setExHue);
            this.Controls.Add(this.lblContainerHue);
            this.Controls.Add(this.lblFormat);
            this.Controls.Add(this.containerLabelFormat);
            this.Controls.Add(this.removeContainerLabel);
            this.Controls.Add(this.addContainLabel);
            this.Controls.Add(this.containerView);
            this.Controls.Add(this.cancelContainerLabels);
            this.Controls.Add(this.saveContainerLabels);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ContainerLabels";
            this.Text = "Container Labels";
            this.Load += new System.EventHandler(this.ContainerLabels_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button cancelContainerLabels;
        private System.Windows.Forms.Button saveContainerLabels;
        private System.Windows.Forms.Label lblFormat;
        private System.Windows.Forms.TextBox containerLabelFormat;
        private System.Windows.Forms.Button removeContainerLabel;
        private System.Windows.Forms.Button addContainLabel;
        private System.Windows.Forms.ListView containerView;
        private System.Windows.Forms.ColumnHeader containerId;
        private System.Windows.Forms.ColumnHeader containerType;
        private System.Windows.Forms.ColumnHeader containerLabel;
        private System.Windows.Forms.Button setExHue;
        private System.Windows.Forms.Label lblContainerHue;
        private System.Windows.Forms.Button setColorHue;
        private System.Windows.Forms.RadioButton asciiStyle;
        private System.Windows.Forms.RadioButton unicodeStyle;
        private System.Windows.Forms.Label lblStyle;
    }
}