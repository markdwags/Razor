using System;
using System.Threading;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Assistant
{
	public class SplashScreen : System.Windows.Forms.Form
	{
		private static SplashScreen m_Screen;
		public static SplashScreen Instance{ get{ return m_Screen; } }
		public static void Start()
		{
			Thread t = new Thread( new ThreadStart( ThreadMain ) );
			t.Name = "Razor Splash Screen";
			t.Start();
			while ( m_Screen == null )
				Thread.Sleep( 10 );
		}

		public static void End()
		{
			if ( m_Screen != null )
				m_Screen.Close();
		}

		public static string Message{ set{ if ( m_Screen != null ) m_Screen.message.Text = value; } }
		
		public static void ThreadMain()
		{
			try
			{
				m_Screen = new SplashScreen();
				Application.Run(m_Screen);
				m_Screen = null;
			}
			catch
			{
			}
		}

		private System.Windows.Forms.PictureBox logo;
		private System.Windows.Forms.Label message;
		private System.Windows.Forms.PictureBox donate;
		private System.ComponentModel.Container components = null;

		public SplashScreen()
		{
			try
			{
				InitializeComponent();
			}
			catch
			{
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SplashScreen));
			this.logo = new System.Windows.Forms.PictureBox();
			this.message = new System.Windows.Forms.Label();
			this.donate = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// logo
			// 
			this.logo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.logo.Image = ((System.Drawing.Image)(resources.GetObject("logo.Image")));
			this.logo.Location = new System.Drawing.Point(0, 0);
			this.logo.Name = "logo";
			try
			{
				this.logo.Size = new System.Drawing.Size(300, 211);
			}
			catch
			{
			}

			this.logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.logo.TabIndex = 0;
			this.logo.TabStop = false;
			// 
			// message
			// 
			this.message.BackColor = System.Drawing.Color.Black;
			this.message.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.message.Location = new System.Drawing.Point(0, 200);
			this.message.Name = "message";
			try
			{
			this.message.Size = new System.Drawing.Size(300, 20);
			}
			catch
			{
			}
			this.message.TabIndex = 1;
			this.message.Text = "Don\'t forget to Donate!";
			this.message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// donate
			// 
			this.donate.Cursor = System.Windows.Forms.Cursors.Hand;
			this.donate.Image = ((System.Drawing.Image)(resources.GetObject("donate.Image")));
			this.donate.Location = new System.Drawing.Point(4, 4);
			this.donate.Name = "donate";
			try
			{
			this.donate.Size = new System.Drawing.Size(62, 31);
			}
			catch
			{
			}
			this.donate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.donate.TabIndex = 2;
			this.donate.TabStop = false;
			this.donate.Click += new System.EventHandler(this.donate_Click);
			// 
			// SplashScreen
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(300, 220);
			this.ControlBox = false;
			this.Controls.Add(this.donate);
			this.Controls.Add(this.message);
			this.Controls.Add(this.logo);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "SplashScreen";
			this.Opacity = 0.9;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SplashScreen";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.SplashScreen_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void SplashScreen_Load(object sender, System.EventArgs e)
		{
		}

		private void donate_Click(object sender, System.EventArgs e)
		{
			MainForm.LaunchBrowser( "https://www.paypal.com/xclick/business=zippy%40runuo.com&item_name=Razor&no_shipping=1&no_note=1&tax=0&currency_code=USD" );
		}
	}
}
