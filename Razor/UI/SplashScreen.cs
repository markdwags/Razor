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

				m_Screen.CreateControl();
			}
		}

        private delegate void CloseDelegate();
		public static void End()
		{
            if (m_Screen != null)
            {
                if (m_Screen.InvokeRequired)
                    m_Screen.Invoke(new CloseDelegate(m_Screen.Close));
                else
                    m_Screen.Close();
            }
		}

        private delegate void SetMsgDelegate(SplashScreen screen, string arg);
        private static void SetMessage(SplashScreen screen, string msg)
        {
            //screen.message.Text = msg;
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
		
		private static void ThreadMain()
		{
			try
			{
                using (m_Screen = new SplashScreen())
                {
                    Application.EnableVisualStyles();
                    Application.Run(m_Screen);
                }
                m_Screen = null;
			}
			catch
			{
			}
		}
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
			this.SuspendLayout();
			// 
			// SplashScreen
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.White;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ClientSize = new System.Drawing.Size(435, 331);
			this.ControlBox = false;
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "SplashScreen";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Razor Splash Screen";
			this.TopMost = true;
			this.TransparencyKey = System.Drawing.Color.White;
			this.Load += new System.EventHandler(this.SplashScreen_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void SplashScreen_Load(object sender, System.EventArgs e)
		{
			this.Activate();
			this.BringToFront();
			this.Focus();
		}
	}
}
