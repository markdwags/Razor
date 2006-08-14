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
        private DateTime m_OldTime;
        private Thread thrdPartyLocs;
        private bool StopThread;
		public const int WM_NCLBUTTONDOWN = 0xA1;
		private Assistant.MapUO.UOMapControl Map;
		private int ButtonRows;
		public const int HT_CAPTION = 0x2;



		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd,
			int Msg, int wParam, int lParam);
		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();
		private System.Windows.Forms.ListBox lstUsers;
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
            this.Map.UserList.Add(new User(World.Player.Serial, "**You**"));
            this.Map.FocusUser = (User)this.Map.UserList[0];
            StopThread = false;
           
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
            m_OldTime = DateTime.Now;
            if(Assistant.PacketHandlers.Party != null)
            this.UpdateList(Assistant.PacketHandlers.Party);
		}

        public void TogglePartyLocThread()
        {
            if (this.Map.UserList.Count > 1)
            {
                if (thrdPartyLocs != null)
                {
                    if (!thrdPartyLocs.IsAlive)
                    {
                        StopThread = false;
                        thrdPartyLocs = new Thread(new ThreadStart(RequestPartyLocations));
                        thrdPartyLocs.Start();
                    }
                }
                else
                {
                    StopThread = false;
                    thrdPartyLocs = new Thread(new ThreadStart(RequestPartyLocations));
                    thrdPartyLocs.Start();

                }
            }
            else
            {
                MessageBox.Show("Need to be in a party");
            }
        }
        private void RequestPartyLocations()
        {
            while (!(StopThread))
            {
                if (World.Player == null || this.Map.UserList.Count <= 1)
                {
                    this.StopThread = true;
                    break;
                }
                
                ClientCommunication.SendToServer(new QueryPartyLocs());
                Thread.Sleep(200);
           }
        }
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
            StopThread = true;
            if (thrdPartyLocs != null)
                thrdPartyLocs.Abort();
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
			this.lstUsers = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// Map
			// 
			this.Map.Active = true;
			this.Map.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Map.FocusUser = null;
			this.Map.Location = new System.Drawing.Point(0, 0);
			this.Map.Name = "Map";
			this.Map.Size = new System.Drawing.Size(296, 272);
			this.Map.TabIndex = 0;
			this.Map.TabStop = false;
			this.Map.MouseDown += new System.Windows.Forms.MouseEventHandler(this.map_MouseDown);
			// 
			// lstUsers
			// 
			this.lstUsers.Location = new System.Drawing.Point(283, 0);
			this.lstUsers.Name = "lstUsers";
			this.lstUsers.Size = new System.Drawing.Size(96, 238);
			this.lstUsers.TabIndex = 2;
			this.lstUsers.Visible = false;
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

        public void UpdateUser(uint Serial, short x, short y, string name)
        {
            bool found = false;
            foreach (User usr in this.Map.UserList)
            {
                if (usr.Serial == Serial)
                {
                    found = true;
                    usr.X = x;
                    usr.Y = y;
                    usr.Name = name;
                    
                }
            }
            if (!found)
            {
                User usr = new User(Serial, name);
                usr.X = x;
                usr.Y = y;
                this.Map.UserList.Add(usr);
            }
            this.Refresh();
        }
        public void CheckLocalUpdate(Mobile mob)
        {

            TimeSpan tSpan = DateTime.Now - m_OldTime;
            if (tSpan.TotalMilliseconds >= 100)
            {
                m_OldTime = DateTime.Now;
                if (this.Map.UserList != null)
                {
                    foreach (User usr in this.Map.UserList)
                    {
                        if (usr.Serial == mob.Serial.Value)
                        {
                            this.Map.Refresh();
                            break;

                        }

                    }
                }
            }

        }
        public void UpdateList(ArrayList pList)
        {
            if (pList.Count > 0)
            {

                this.Map.UserList.Clear();
                this.UpdateUser(World.Player.Serial.Value, System.Convert.ToInt16(World.Player.Position.X), Convert.ToInt16(World.Player.Position.Y), "**You**");
                this.Map.FocusUser = (User)this.Map.UserList[0];
                foreach (Serial srl in pList)
                {
                    Mobile mobile = World.FindMobile(srl);

                    if (mobile == null)
                    {
                        World.Player.SendMessage(MsgLevel.Warning, "Players not found");
                        Mobile mob = new Mobile(srl);
                        mob.Name = "Not Seen";
                        mob.Position = new Point3D(0, 0, 0);
                        World.AddMobile(mob);
                        mobile = mob;
                    }
                    if (mobile.Name.Length > 0)
                    {
                        this.UpdateUser(srl.Value, System.Convert.ToInt16(mobile.Position.X), Convert.ToInt16(mobile.Position.Y), mobile.Name);
                    }
                }
                TogglePartyLocThread();
            }
            else
            {
                this.Map.UserList.Clear();
                this.UpdateUser(World.Player.Serial.Value, System.Convert.ToInt16(World.Player.Position.X), Convert.ToInt16(World.Player.Position.Y), "**You**");
                this.Map.FocusUser = (User)this.Map.UserList[0];
            }
            this.Refresh();
        }
        public void ClearUsers()
        {
            this.Map.UserList.Clear();
        }
		protected override void OnPaint(PaintEventArgs e)
		{
			if (this.Map.UserList.Count > 1)
			{
				//75x15
				int xcount = 0;
				int ycount = 0;
				Point org = new Point(0, this.Height - 50 - (ButtonRows * 15));
				if (this.FormBorderStyle == FormBorderStyle.None)
				{
					org = new Point(0, this.Height - 50 - (ButtonRows * 15) + 32);
				}
				Font fnt = new Font("Courier New", 8, FontStyle.Regular);
				foreach (User usr in this.Map.UserList)
				{
					if (((75 * (xcount+1)) - this.Width) > 0)
					{
						xcount = 0;
						ycount++;
					}
                    Mobile mob = World.FindMobile(usr.Serial);
                    string name = mob.Name;
					if (name.Length > 8)
					{
						name = name.Substring(0, 8);
						name += "...";

					}
                    else if (name.Length < 1)
                    {
                        name = "Not seen";
                    }

					Point drawPoint = new Point(org.X + (75 * xcount), org.Y + (15*ycount));
					usr.ButtonPoint = drawPoint;
					e.Graphics.FillRectangle(Brushes.Black, new Rectangle(drawPoint, new Size(75, 15)));
					e.Graphics.DrawRectangle(new Pen(Brushes.Gray), new Rectangle(drawPoint, new Size(75, 15)));
					e.Graphics.DrawString(name, fnt, Brushes.White, drawPoint);
					xcount++;
				}
				if(ycount > 0)
					ButtonRows = ycount;
			}
			base.OnPaint(e);
		}
		 private void MapWindow_Resize(object sender, System.EventArgs e)
		{
			this.lstUsers.Location = new System.Drawing.Point(this.Width-this.lstUsers.Width-20, 4);
			this.lstUsers.Height = this.Height;

			//Display party list?
			if (this.Map.UserList.Count > 1)
			{
				if (this.FormBorderStyle == FormBorderStyle.None)
				{
					this.Map.Height = this.Height - 50 - (ButtonRows * 15) + 32;
				}
				else
				{
					this.Map.Height = this.Height - 50 - (ButtonRows * 15);
				}
			}
			else
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
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                Rectangle pointRec = new Rectangle(e.X, e.Y, 1, 1);
                foreach (User usr in this.Map.UserList)
                {       
                    Rectangle rec = new Rectangle(usr.ButtonPoint, new Size(75, 15));
                    if (pointRec.IntersectsWith(rec))
                    {
                        this.Map.FocusUser = usr;
                        this.Map.Refresh();
                        
                    }

                }
			}
			this.Map.MapClick(e);
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
