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
			if ( m_Screen == null )
			{
				Thread t = new Thread( new ThreadStart( ThreadMain ) );
				t.Name = "Razor Splash Screen";
				t.Start();
				while ( m_Screen == null )
					Thread.Sleep( 1 );
                Thread.Sleep(1000);
			}
		}

        public delegate void CloseDelegate();
		public static void End()
		{
			if ( m_Screen != null )
                m_Screen.Invoke(new CloseDelegate(m_Screen.Close));
		}

        public delegate void SetMsgDelegate(SplashScreen screen, string arg);
        public static void SetMessage(SplashScreen screen, string msg)
        {
            screen.message.Text = msg;
        }

        public static string MessageStr 
        { 
            set 
            {
                try
                {
                    if (m_Screen != null)
                    {
                        if (m_Screen.InvokeRequired)
                            m_Screen.Invoke(new SetMsgDelegate(SetMessage), m_Screen, value);
                        else
                            SetMessage(m_Screen, value);
                    }
                }
                catch
                {
                }
            }
        }

        public static LocString Message { set { MessageStr = Language.GetString(value); } }
		
		public static void ThreadMain()
		{
			try
			{
                using (m_Screen = new SplashScreen())
                {
                    Application.Run(m_Screen);
                }
                m_Screen = null;
			}
			catch
			{
			}
        }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreen));
            this.message = new System.Windows.Forms.Label();
            this.donate = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.donate)).BeginInit();
            this.SuspendLayout();
            // 
            // message
            // 
            this.message.BackColor = System.Drawing.Color.Transparent;
            this.message.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.message.Location = new System.Drawing.Point(-16, 308);
            this.message.Name = "message";
            this.message.Size = new System.Drawing.Size(440, 21);
            this.message.TabIndex = 1;
            this.message.Text = "Don\'t forget to Donate!";
            this.message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // donate
            // 
            this.donate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.donate.Image = ((System.Drawing.Image)(resources.GetObject("donate.Image")));
            this.donate.Location = new System.Drawing.Point(374, 0);
            this.donate.Name = "donate";
            this.donate.Size = new System.Drawing.Size(50, 25);
            this.donate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.donate.TabIndex = 2;
            this.donate.TabStop = false;
            this.donate.Click += new System.EventHandler(this.donate_Click);
            // 
            // SplashScreen
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(435, 331);
            this.ControlBox = false;
            this.Controls.Add(this.donate);
            this.Controls.Add(this.message);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SplashScreen";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Razor Splash Screen";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.White;
            this.Load += new System.EventHandler(this.SplashScreen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.donate)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void SplashScreen_Load(object sender, System.EventArgs e)
		{
			this.Activate();
			this.BringToFront();
			this.Focus();
		}

		private void donate_Click(object sender, System.EventArgs e)
		{
			MainForm.LaunchBrowser( "https://www.paypal.com/xclick/business=zippy%40runuo.com&item_name=Razor&no_shipping=1&no_note=1&tax=0&currency_code=USD" );
		}
	}
}
