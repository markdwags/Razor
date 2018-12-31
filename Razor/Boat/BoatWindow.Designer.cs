using System.Windows.Forms;

namespace Assistant.Boat
{
    partial class BoatWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BoatWindow));
            this.boatNorth = new System.Windows.Forms.PictureBox();
            this.boatNortheast = new System.Windows.Forms.PictureBox();
            this.boatEast = new System.Windows.Forms.PictureBox();
            this.boatSoutheast = new System.Windows.Forms.PictureBox();
            this.boatSouth = new System.Windows.Forms.PictureBox();
            this.boatSouthwest = new System.Windows.Forms.PictureBox();
            this.boatWest = new System.Windows.Forms.PictureBox();
            this.boatNorthwest = new System.Windows.Forms.PictureBox();
            this.boatAnchor = new System.Windows.Forms.PictureBox();
            this.boatT2a = new System.Windows.Forms.PictureBox();
            this.boatSpeedNormal = new System.Windows.Forms.RadioButton();
            this.boatSpeedSlow = new System.Windows.Forms.RadioButton();
            this.boatSpeedOne = new System.Windows.Forms.RadioButton();
            this.boatStop = new System.Windows.Forms.PictureBox();
            this.boatTurnRight = new System.Windows.Forms.PictureBox();
            this.boatTurnLeft = new System.Windows.Forms.PictureBox();
            this.boatTurnAround = new System.Windows.Forms.PictureBox();
            this.boatGoto1 = new System.Windows.Forms.Button();
            this.boatGoto2 = new System.Windows.Forms.Button();
            this.boatGoto3 = new System.Windows.Forms.Button();
            this.boatGoto4 = new System.Windows.Forms.Button();
            this.boatGoto5 = new System.Windows.Forms.Button();
            this.boatGoto6 = new System.Windows.Forms.Button();
            this.boatGoto7 = new System.Windows.Forms.Button();
            this.boatGoto8 = new System.Windows.Forms.Button();
            this.boatGoto9 = new System.Windows.Forms.Button();
            this.boatStart = new System.Windows.Forms.Button();
            this.boatContinue = new System.Windows.Forms.Button();
            this.boatNav = new System.Windows.Forms.Button();
            this.alwaysOnTop = new System.Windows.Forms.CheckBox();
            this.boatBorder = new System.Windows.Forms.PictureBox();
            this.boatWindowsBorder = new System.Windows.Forms.CheckBox();
            this.boatClose = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.boatNorth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatNortheast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatEast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatSoutheast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatSouth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatSouthwest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatWest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatNorthwest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatAnchor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatT2a)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatStop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatTurnRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatTurnLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatTurnAround)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatBorder)).BeginInit();
            this.SuspendLayout();
            // 
            // boatNorth
            // 
            this.boatNorth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.boatNorth.Image = ((System.Drawing.Image)(resources.GetObject("boatNorth.Image")));
            this.boatNorth.Location = new System.Drawing.Point(80, 40);
            this.boatNorth.Name = "boatNorth";
            this.boatNorth.Size = new System.Drawing.Size(27, 28);
            this.boatNorth.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.boatNorth.TabIndex = 0;
            this.boatNorth.TabStop = false;
            this.boatNorth.MouseClick += new System.Windows.Forms.MouseEventHandler(this.boatNorth_MouseClick);
            // 
            // boatNortheast
            // 
            this.boatNortheast.Image = ((System.Drawing.Image)(resources.GetObject("boatNortheast.Image")));
            this.boatNortheast.Location = new System.Drawing.Point(113, 40);
            this.boatNortheast.Name = "boatNortheast";
            this.boatNortheast.Size = new System.Drawing.Size(27, 28);
            this.boatNortheast.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.boatNortheast.TabIndex = 1;
            this.boatNortheast.TabStop = false;
            this.boatNortheast.MouseClick += new MouseEventHandler(this.boatNortheast_Click);
            // 
            // boatEast
            // 
            this.boatEast.Image = ((System.Drawing.Image)(resources.GetObject("boatEast.Image")));
            this.boatEast.Location = new System.Drawing.Point(113, 74);
            this.boatEast.Name = "boatEast";
            this.boatEast.Size = new System.Drawing.Size(27, 28);
            this.boatEast.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.boatEast.TabIndex = 2;
            this.boatEast.TabStop = false;
            this.boatEast.Click += new System.EventHandler(this.boatEast_Click);
            // 
            // boatSoutheast
            // 
            this.boatSoutheast.Image = ((System.Drawing.Image)(resources.GetObject("boatSoutheast.Image")));
            this.boatSoutheast.Location = new System.Drawing.Point(113, 108);
            this.boatSoutheast.Name = "boatSoutheast";
            this.boatSoutheast.Size = new System.Drawing.Size(27, 28);
            this.boatSoutheast.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.boatSoutheast.TabIndex = 3;
            this.boatSoutheast.TabStop = false;
            this.boatSoutheast.MouseClick += new MouseEventHandler(this.boatSoutheast_Click);
            // 
            // boatSouth
            // 
            this.boatSouth.Image = ((System.Drawing.Image)(resources.GetObject("boatSouth.Image")));
            this.boatSouth.Location = new System.Drawing.Point(80, 108);
            this.boatSouth.Name = "boatSouth";
            this.boatSouth.Size = new System.Drawing.Size(27, 28);
            this.boatSouth.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.boatSouth.TabIndex = 4;
            this.boatSouth.TabStop = false;
            this.boatSouth.Click += new System.EventHandler(this.boatSouth_Click);
            // 
            // boatSouthwest
            // 
            this.boatSouthwest.Image = ((System.Drawing.Image)(resources.GetObject("boatSouthwest.Image")));
            this.boatSouthwest.Location = new System.Drawing.Point(47, 108);
            this.boatSouthwest.Name = "boatSouthwest";
            this.boatSouthwest.Size = new System.Drawing.Size(27, 28);
            this.boatSouthwest.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.boatSouthwest.TabIndex = 5;
            this.boatSouthwest.TabStop = false;
            this.boatSouthwest.MouseClick += new MouseEventHandler(this.boatSouthwest_Click);
            // 
            // boatWest
            // 
            this.boatWest.Image = ((System.Drawing.Image)(resources.GetObject("boatWest.Image")));
            this.boatWest.Location = new System.Drawing.Point(47, 74);
            this.boatWest.Name = "boatWest";
            this.boatWest.Size = new System.Drawing.Size(27, 28);
            this.boatWest.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.boatWest.TabIndex = 6;
            this.boatWest.TabStop = false;
            this.boatWest.Click += new System.EventHandler(this.boatWest_Click);
            // 
            // boatNorthwest
            // 
            this.boatNorthwest.Image = ((System.Drawing.Image)(resources.GetObject("boatNorthwest.Image")));
            this.boatNorthwest.Location = new System.Drawing.Point(47, 40);
            this.boatNorthwest.Name = "boatNorthwest";
            this.boatNorthwest.Size = new System.Drawing.Size(27, 28);
            this.boatNorthwest.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.boatNorthwest.TabIndex = 7;
            this.boatNorthwest.TabStop = false;
            this.boatNorthwest.MouseClick += new MouseEventHandler(this.boatNorthwest_Click);
            // 
            // boatAnchor
            // 
            this.boatAnchor.Image = ((System.Drawing.Image)(resources.GetObject("boatAnchor.Image")));
            this.boatAnchor.Location = new System.Drawing.Point(15, 142);
            this.boatAnchor.Name = "boatAnchor";
            this.boatAnchor.Size = new System.Drawing.Size(27, 28);
            this.boatAnchor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.boatAnchor.TabIndex = 8;
            this.boatAnchor.TabStop = false;
            this.boatAnchor.Click += new System.EventHandler(this.boatAnchor_Click);
            // 
            // boatT2a
            // 
            this.boatT2a.Image = ((System.Drawing.Image)(resources.GetObject("boatT2a.Image")));
            this.boatT2a.Location = new System.Drawing.Point(145, 142);
            this.boatT2a.Name = "boatT2a";
            this.boatT2a.Size = new System.Drawing.Size(27, 28);
            this.boatT2a.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.boatT2a.TabIndex = 9;
            this.boatT2a.TabStop = false;
            this.boatT2a.Click += new System.EventHandler(this.boatT2a_Click);
            // 
            // boatSpeedNormal
            // 
            this.boatSpeedNormal.AutoSize = true;
            this.boatSpeedNormal.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boatSpeedNormal.ForeColor = System.Drawing.Color.DarkGray;
            this.boatSpeedNormal.Location = new System.Drawing.Point(12, 12);
            this.boatSpeedNormal.Name = "boatSpeedNormal";
            this.boatSpeedNormal.Size = new System.Drawing.Size(62, 17);
            this.boatSpeedNormal.TabIndex = 11;
            this.boatSpeedNormal.TabStop = true;
            this.boatSpeedNormal.Text = "Normal";
            this.boatSpeedNormal.UseVisualStyleBackColor = true;
            // 
            // boatSpeedSlow
            // 
            this.boatSpeedSlow.AutoSize = true;
            this.boatSpeedSlow.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boatSpeedSlow.ForeColor = System.Drawing.Color.DarkGray;
            this.boatSpeedSlow.Location = new System.Drawing.Point(74, 12);
            this.boatSpeedSlow.Name = "boatSpeedSlow";
            this.boatSpeedSlow.Size = new System.Drawing.Size(50, 17);
            this.boatSpeedSlow.TabIndex = 12;
            this.boatSpeedSlow.TabStop = true;
            this.boatSpeedSlow.Text = "Slow";
            this.boatSpeedSlow.UseVisualStyleBackColor = true;
            // 
            // boatSpeedOne
            // 
            this.boatSpeedOne.AutoSize = true;
            this.boatSpeedOne.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boatSpeedOne.ForeColor = System.Drawing.Color.DarkGray;
            this.boatSpeedOne.Location = new System.Drawing.Point(128, 12);
            this.boatSpeedOne.Name = "boatSpeedOne";
            this.boatSpeedOne.Size = new System.Drawing.Size(47, 17);
            this.boatSpeedOne.TabIndex = 13;
            this.boatSpeedOne.TabStop = true;
            this.boatSpeedOne.Text = "One";
            this.boatSpeedOne.UseVisualStyleBackColor = true;
            // 
            // boatStop
            // 
            this.boatStop.Image = ((System.Drawing.Image)(resources.GetObject("boatStop.Image")));
            this.boatStop.Location = new System.Drawing.Point(80, 74);
            this.boatStop.Name = "boatStop";
            this.boatStop.Size = new System.Drawing.Size(27, 28);
            this.boatStop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.boatStop.TabIndex = 14;
            this.boatStop.TabStop = false;
            this.boatStop.Click += new System.EventHandler(this.boatStop_Click);
            // 
            // boatTurnRight
            // 
            this.boatTurnRight.Image = ((System.Drawing.Image)(resources.GetObject("boatTurnRight.Image")));
            this.boatTurnRight.Location = new System.Drawing.Point(146, 74);
            this.boatTurnRight.Name = "boatTurnRight";
            this.boatTurnRight.Size = new System.Drawing.Size(26, 19);
            this.boatTurnRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.boatTurnRight.TabIndex = 16;
            this.boatTurnRight.TabStop = false;
            this.boatTurnRight.Click += new System.EventHandler(this.boatTurnRight_Click);
            // 
            // boatTurnLeft
            // 
            this.boatTurnLeft.Image = ((System.Drawing.Image)(resources.GetObject("boatTurnLeft.Image")));
            this.boatTurnLeft.Location = new System.Drawing.Point(15, 74);
            this.boatTurnLeft.Name = "boatTurnLeft";
            this.boatTurnLeft.Size = new System.Drawing.Size(26, 19);
            this.boatTurnLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.boatTurnLeft.TabIndex = 17;
            this.boatTurnLeft.TabStop = false;
            this.boatTurnLeft.Click += new System.EventHandler(this.boatTurnLeft_Click);
            // 
            // boatTurnAround
            // 
            this.boatTurnAround.Image = ((System.Drawing.Image)(resources.GetObject("boatTurnAround.Image")));
            this.boatTurnAround.Location = new System.Drawing.Point(80, 142);
            this.boatTurnAround.Name = "boatTurnAround";
            this.boatTurnAround.Size = new System.Drawing.Size(27, 28);
            this.boatTurnAround.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.boatTurnAround.TabIndex = 18;
            this.boatTurnAround.TabStop = false;
            this.boatTurnAround.Click += new System.EventHandler(this.boatTurnAround_Click);
            // 
            // boatGoto1
            // 
            this.boatGoto1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.boatGoto1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boatGoto1.ForeColor = System.Drawing.Color.DarkGray;
            this.boatGoto1.Location = new System.Drawing.Point(26, 185);
            this.boatGoto1.Name = "boatGoto1";
            this.boatGoto1.Size = new System.Drawing.Size(22, 21);
            this.boatGoto1.TabIndex = 19;
            this.boatGoto1.Text = "1";
            this.boatGoto1.UseVisualStyleBackColor = true;
            this.boatGoto1.Click += new System.EventHandler(this.boatGoto1_Click);
            // 
            // boatGoto2
            // 
            this.boatGoto2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.boatGoto2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boatGoto2.ForeColor = System.Drawing.Color.DarkGray;
            this.boatGoto2.Location = new System.Drawing.Point(54, 185);
            this.boatGoto2.Name = "boatGoto2";
            this.boatGoto2.Size = new System.Drawing.Size(22, 21);
            this.boatGoto2.TabIndex = 20;
            this.boatGoto2.Text = "2";
            this.boatGoto2.UseVisualStyleBackColor = true;
            this.boatGoto2.Click += new System.EventHandler(this.boatGoto2_Click);
            // 
            // boatGoto3
            // 
            this.boatGoto3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.boatGoto3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boatGoto3.ForeColor = System.Drawing.Color.DarkGray;
            this.boatGoto3.Location = new System.Drawing.Point(82, 185);
            this.boatGoto3.Name = "boatGoto3";
            this.boatGoto3.Size = new System.Drawing.Size(22, 21);
            this.boatGoto3.TabIndex = 21;
            this.boatGoto3.Text = "3";
            this.boatGoto3.UseVisualStyleBackColor = true;
            this.boatGoto3.Click += new System.EventHandler(this.boatGoto3_Click);
            // 
            // boatGoto4
            // 
            this.boatGoto4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.boatGoto4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boatGoto4.ForeColor = System.Drawing.Color.DarkGray;
            this.boatGoto4.Location = new System.Drawing.Point(26, 212);
            this.boatGoto4.Name = "boatGoto4";
            this.boatGoto4.Size = new System.Drawing.Size(22, 21);
            this.boatGoto4.TabIndex = 22;
            this.boatGoto4.Text = "4";
            this.boatGoto4.UseVisualStyleBackColor = true;
            this.boatGoto4.Click += new System.EventHandler(this.boatGoto4_Click);
            // 
            // boatGoto5
            // 
            this.boatGoto5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.boatGoto5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boatGoto5.ForeColor = System.Drawing.Color.DarkGray;
            this.boatGoto5.Location = new System.Drawing.Point(54, 212);
            this.boatGoto5.Name = "boatGoto5";
            this.boatGoto5.Size = new System.Drawing.Size(22, 21);
            this.boatGoto5.TabIndex = 23;
            this.boatGoto5.Text = "5";
            this.boatGoto5.UseVisualStyleBackColor = true;
            this.boatGoto5.Click += new System.EventHandler(this.boatGoto5_Click);
            // 
            // boatGoto6
            // 
            this.boatGoto6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.boatGoto6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boatGoto6.ForeColor = System.Drawing.Color.DarkGray;
            this.boatGoto6.Location = new System.Drawing.Point(82, 212);
            this.boatGoto6.Name = "boatGoto6";
            this.boatGoto6.Size = new System.Drawing.Size(22, 21);
            this.boatGoto6.TabIndex = 24;
            this.boatGoto6.Text = "6";
            this.boatGoto6.UseVisualStyleBackColor = true;
            this.boatGoto6.Click += new System.EventHandler(this.boatGoto6_Click);
            // 
            // boatGoto7
            // 
            this.boatGoto7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.boatGoto7.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boatGoto7.ForeColor = System.Drawing.Color.DarkGray;
            this.boatGoto7.Location = new System.Drawing.Point(26, 239);
            this.boatGoto7.Name = "boatGoto7";
            this.boatGoto7.Size = new System.Drawing.Size(22, 21);
            this.boatGoto7.TabIndex = 25;
            this.boatGoto7.Text = "7";
            this.boatGoto7.UseVisualStyleBackColor = true;
            this.boatGoto7.Click += new System.EventHandler(this.boatGoto7_Click);
            // 
            // boatGoto8
            // 
            this.boatGoto8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.boatGoto8.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boatGoto8.ForeColor = System.Drawing.Color.DarkGray;
            this.boatGoto8.Location = new System.Drawing.Point(54, 239);
            this.boatGoto8.Name = "boatGoto8";
            this.boatGoto8.Size = new System.Drawing.Size(22, 21);
            this.boatGoto8.TabIndex = 26;
            this.boatGoto8.Text = "8";
            this.boatGoto8.UseVisualStyleBackColor = true;
            this.boatGoto8.Click += new System.EventHandler(this.boatGoto8_Click);
            // 
            // boatGoto9
            // 
            this.boatGoto9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.boatGoto9.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boatGoto9.ForeColor = System.Drawing.Color.DarkGray;
            this.boatGoto9.Location = new System.Drawing.Point(82, 239);
            this.boatGoto9.Name = "boatGoto9";
            this.boatGoto9.Size = new System.Drawing.Size(22, 21);
            this.boatGoto9.TabIndex = 27;
            this.boatGoto9.Text = "9";
            this.boatGoto9.UseVisualStyleBackColor = true;
            this.boatGoto9.Click += new System.EventHandler(this.boatGoto9_Click);
            // 
            // boatStart
            // 
            this.boatStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.boatStart.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boatStart.ForeColor = System.Drawing.Color.DarkGray;
            this.boatStart.Location = new System.Drawing.Point(110, 185);
            this.boatStart.Name = "boatStart";
            this.boatStart.Size = new System.Drawing.Size(52, 21);
            this.boatStart.TabIndex = 28;
            this.boatStart.Text = "Start";
            this.boatStart.UseVisualStyleBackColor = true;
            this.boatStart.Click += new System.EventHandler(this.boatStart_Click);
            // 
            // boatContinue
            // 
            this.boatContinue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.boatContinue.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boatContinue.ForeColor = System.Drawing.Color.DarkGray;
            this.boatContinue.Location = new System.Drawing.Point(110, 212);
            this.boatContinue.Name = "boatContinue";
            this.boatContinue.Size = new System.Drawing.Size(52, 21);
            this.boatContinue.TabIndex = 29;
            this.boatContinue.Text = "Cont.";
            this.boatContinue.UseVisualStyleBackColor = true;
            this.boatContinue.Click += new System.EventHandler(this.boatContinue_Click);
            // 
            // boatNav
            // 
            this.boatNav.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.boatNav.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boatNav.ForeColor = System.Drawing.Color.DarkGray;
            this.boatNav.Location = new System.Drawing.Point(110, 239);
            this.boatNav.Name = "boatNav";
            this.boatNav.Size = new System.Drawing.Size(52, 21);
            this.boatNav.TabIndex = 30;
            this.boatNav.Text = "Nav";
            this.boatNav.UseVisualStyleBackColor = true;
            this.boatNav.Click += new System.EventHandler(this.boatNav_Click);
            // 
            // alwaysOnTop
            // 
            this.alwaysOnTop.AutoSize = true;
            this.alwaysOnTop.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alwaysOnTop.ForeColor = System.Drawing.Color.DarkGray;
            this.alwaysOnTop.Location = new System.Drawing.Point(11, 270);
            this.alwaysOnTop.Name = "alwaysOnTop";
            this.alwaysOnTop.Size = new System.Drawing.Size(101, 17);
            this.alwaysOnTop.TabIndex = 31;
            this.alwaysOnTop.Text = "Always On Top";
            this.alwaysOnTop.UseVisualStyleBackColor = true;
            this.alwaysOnTop.CheckedChanged += new System.EventHandler(this.alwaysOnTop_CheckedChanged);
            // 
            // boatBorder
            // 
            this.boatBorder.Image = ((System.Drawing.Image)(resources.GetObject("boatBorder.Image")));
            this.boatBorder.Location = new System.Drawing.Point(0, 0);
            this.boatBorder.Name = "boatBorder";
            this.boatBorder.Size = new System.Drawing.Size(190, 296);
            this.boatBorder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.boatBorder.TabIndex = 32;
            this.boatBorder.TabStop = false;
            this.boatBorder.Visible = false;
            // 
            // boatWindowsBorder
            // 
            this.boatWindowsBorder.AutoSize = true;
            this.boatWindowsBorder.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boatWindowsBorder.ForeColor = System.Drawing.Color.DarkGray;
            this.boatWindowsBorder.Location = new System.Drawing.Point(114, 270);
            this.boatWindowsBorder.Name = "boatWindowsBorder";
            this.boatWindowsBorder.Size = new System.Drawing.Size(61, 17);
            this.boatWindowsBorder.TabIndex = 33;
            this.boatWindowsBorder.Text = "Border";
            this.boatWindowsBorder.UseVisualStyleBackColor = true;
            this.boatWindowsBorder.CheckedChanged += new System.EventHandler(this.boatWindowsBorder_CheckedChanged);
            // 
            // boatClose
            // 
            this.boatClose.AutoSize = true;
            this.boatClose.ForeColor = System.Drawing.Color.DimGray;
            this.boatClose.Location = new System.Drawing.Point(172, 4);
            this.boatClose.Name = "boatClose";
            this.boatClose.Size = new System.Drawing.Size(14, 15);
            this.boatClose.TabIndex = 34;
            this.boatClose.Text = "X";
            this.boatClose.Click += new System.EventHandler(this.boatClose_Click);
            // 
            // BoatWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(190, 296);
            this.Controls.Add(this.boatClose);
            this.Controls.Add(this.boatWindowsBorder);
            this.Controls.Add(this.alwaysOnTop);
            this.Controls.Add(this.boatNav);
            this.Controls.Add(this.boatContinue);
            this.Controls.Add(this.boatStart);
            this.Controls.Add(this.boatGoto9);
            this.Controls.Add(this.boatGoto8);
            this.Controls.Add(this.boatGoto7);
            this.Controls.Add(this.boatGoto6);
            this.Controls.Add(this.boatGoto5);
            this.Controls.Add(this.boatGoto4);
            this.Controls.Add(this.boatGoto3);
            this.Controls.Add(this.boatGoto2);
            this.Controls.Add(this.boatGoto1);
            this.Controls.Add(this.boatTurnAround);
            this.Controls.Add(this.boatTurnLeft);
            this.Controls.Add(this.boatTurnRight);
            this.Controls.Add(this.boatStop);
            this.Controls.Add(this.boatSpeedOne);
            this.Controls.Add(this.boatSpeedSlow);
            this.Controls.Add(this.boatSpeedNormal);
            this.Controls.Add(this.boatT2a);
            this.Controls.Add(this.boatAnchor);
            this.Controls.Add(this.boatNorthwest);
            this.Controls.Add(this.boatWest);
            this.Controls.Add(this.boatSouthwest);
            this.Controls.Add(this.boatSouth);
            this.Controls.Add(this.boatSoutheast);
            this.Controls.Add(this.boatEast);
            this.Controls.Add(this.boatNortheast);
            this.Controls.Add(this.boatNorth);
            this.Controls.Add(this.boatBorder);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BoatWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Razor Boat Control";
            this.Load += new System.EventHandler(this.BoatWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.boatNorth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatNortheast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatEast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatSoutheast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatSouth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatSouthwest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatWest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatNorthwest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatAnchor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatT2a)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatStop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatTurnRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatTurnLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatTurnAround)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boatBorder)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox boatNorth;
        private System.Windows.Forms.PictureBox boatNortheast;
        private System.Windows.Forms.PictureBox boatEast;
        private System.Windows.Forms.PictureBox boatSoutheast;
        private System.Windows.Forms.PictureBox boatSouth;
        private System.Windows.Forms.PictureBox boatSouthwest;
        private System.Windows.Forms.PictureBox boatWest;
        private System.Windows.Forms.PictureBox boatNorthwest;
        private System.Windows.Forms.PictureBox boatAnchor;
        private System.Windows.Forms.PictureBox boatT2a;
        private System.Windows.Forms.RadioButton boatSpeedNormal;
        private System.Windows.Forms.RadioButton boatSpeedSlow;
        private System.Windows.Forms.RadioButton boatSpeedOne;
        private System.Windows.Forms.PictureBox boatStop;
        private System.Windows.Forms.PictureBox boatTurnRight;
        private System.Windows.Forms.PictureBox boatTurnLeft;
        private System.Windows.Forms.PictureBox boatTurnAround;
        private System.Windows.Forms.Button boatGoto1;
        private System.Windows.Forms.Button boatGoto2;
        private System.Windows.Forms.Button boatGoto3;
        private System.Windows.Forms.Button boatGoto4;
        private System.Windows.Forms.Button boatGoto5;
        private System.Windows.Forms.Button boatGoto6;
        private System.Windows.Forms.Button boatGoto7;
        private System.Windows.Forms.Button boatGoto8;
        private System.Windows.Forms.Button boatGoto9;
        private System.Windows.Forms.Button boatStart;
        private System.Windows.Forms.Button boatContinue;
        private System.Windows.Forms.Button boatNav;
        private System.Windows.Forms.CheckBox alwaysOnTop;
        private System.Windows.Forms.PictureBox boatBorder;
        private System.Windows.Forms.CheckBox boatWindowsBorder;
        private System.Windows.Forms.Label boatClose;
    }
}