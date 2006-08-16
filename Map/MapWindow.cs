using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
namespace Assistant.MapUO
{
	/// <summary>
	/// Summary description for MapWindow.
	/// </summary>
	public class MapWindow : System.Windows.Forms.Form
	{
		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;

		private Assistant.MapUO.UOMapControl Map;
		private int ButtonRows;

		[DllImport("user32.dll")]
		private static extern int SendMessage( IntPtr hWnd, int Msg, int wParam, int lParam );
		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MapWindow()
		{
        
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.BackColor = Color.Black;
           
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
            
			UpdateMap();
		}

		public static void Initialize()
		{
			new ReqPartyLocTimer().Start();
		}

		private class ReqPartyLocTimer : Timer
		{
			public ReqPartyLocTimer() : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 0.25 ) )
			{
			}

			protected override void OnTick()
			{
				if ( World.Player != null && PacketHandlers.Party.Count > 0 )
				{
					this.Interval = TimeSpan.FromSeconds( 0.25 );
					ClientCommunication.SendToServer(new QueryPartyLocs());
				}
				else
				{
					this.Interval = TimeSpan.FromSeconds( 1.0 );
				}
			}
		}

		private void RequestPartyLocations()
		{
			if ( World.Player != null && PacketHandlers.Party.Count > 0 )
				ClientCommunication.SendToServer(new QueryPartyLocs());
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
			this.Map = new Assistant.MapUO.UOMapControl();
			this.SuspendLayout();
			// 
			// Map
			// 
			this.Map.Active = true;
			this.Map.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Map.Location = new System.Drawing.Point(0, 0);
			this.Map.Name = "Map";
			this.Map.Size = new System.Drawing.Size(296, 272);
			this.Map.TabIndex = 0;
			this.Map.TabStop = false;
			this.Map.MouseDown += new System.Windows.Forms.MouseEventHandler(this.map_MouseDown);
			// 
			// MapWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.Map);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximizeBox = false;
			this.Name = "MapWindow";
			this.Text = "UO Positioning System";
			this.Resize += new System.EventHandler(this.MapWindow_Resize);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapWindow_MouseDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.MapWindow_Closing);
			this.ResumeLayout(false);

		}
		#endregion

		public void CheckLocalUpdate(Mobile mob)
		{
			if ( mob.InParty )
				this.Map.Refresh();
		}

		/*private static Font m_RegFont = new Font( "Courier New", 8 );

		protected override void OnPaint(PaintEventArgs e)
		{
			if ( PacketHandlers.Party.Count > 0 )
			{
				//75x15
				int xcount = 0;
				int ycount = 0;
				Point org = new Point(0, this.Height - 50 - (ButtonRows * 15));
				if (this.FormBorderStyle == FormBorderStyle.None)
				{
					org = new Point(0, this.Height - 50 - (ButtonRows * 15) + 32);
				}
				
				foreach ( Serial s in PacketHandlers.Party )
				{
					Mobile mob = World.FindMobile( s );
					if ( mob == null )
						continue;

					if (((75 * (xcount+1)) - this.Width) > 0)
					{
						xcount = 0;
						ycount++;
					}
					string name = mob.Name;
					if ( name != null && name.Length > 8)
					{
						name = name.Substring(0, 8);
						name += "...";
					}
					else if ( name == null || name.Length < 1 )
					{
						name = "(Not Seen)";
					}

					Point drawPoint = new Point(org.X + (75 * xcount), org.Y + (15*ycount));
					mob.ButtonPoint = new Point2D( drawPoint.X, drawPoint.Y );
					e.Graphics.FillRectangle( Brushes.Black, drawPoint.X, drawPoint.Y, 75, 15 );
					e.Graphics.DrawRectangle(Pens.Gray, drawPoint.X, drawPoint.Y, 75, 15 );
					e.Graphics.DrawString(name, m_RegFont, Brushes.White, drawPoint);
					xcount++;
				}
				if(ycount > 0)
					ButtonRows = ycount;
			}
			base.OnPaint(e);
		}*/

		private void MapWindow_Resize(object sender, System.EventArgs e)
		{
			this.Map.Height = this.Height;
			this.Map.Width = this.Width;
			this.Refresh();
		}

		private void MapWindow_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Clicks == 2)
			{
				if (this.FormBorderStyle == FormBorderStyle.None)
				{
					this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
					this.TopMost = false;
				}
				else
				{
					this.FormBorderStyle = FormBorderStyle.None;
					this.TopMost = true;
				}

			}

			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage( Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0 );
				/*foreach ( Serial s in PacketHandlers.Party )
				{       
					Mobile m = World.FindMobile( s );
					if ( m == null )
						continue;
					Rectangle rec = new Rectangle( m.ButtonPoint.X, m.ButtonPoint.Y, 75, 15 );
					if ( rec.Contains( e.X, e.Y ) )
					{
						this.Map.FocusMobile = m;
						this.Map.Refresh();
					}
				}*/
			}
			this.Map.MapClick(e);
		}

		public void UpdateMap()
		{
			this.Map.UpdateMap();
		}

		private void map_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			MapWindow_MouseDown(sender, (System.Windows.Forms.MouseEventArgs)e);
		}

		private void MapWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if ( Assistant.Engine.Running )
			{
				e.Cancel = true;
				this.Hide();
			}
		}

		public void PlayerMoved()
		{
			if ( this.Visible && this.Map != null )
				this.Map.UpdateMap();
		}
	}
}
