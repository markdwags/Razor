using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Assistant.MapUO
{
	/// <summary>
	/// Summary description for MapWindow.
	/// </summary>
	public class MapWindow : System.Windows.Forms.Form
	{
		public const int WM_NCLBUTTONDOWN = 0xA1;
		private Assistant.MapUO.UOMapControl Map;
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

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.Map.Active = true;
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
			this.lstUsers = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// Map
			// 
			this.Map.Active = false;
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

		private void MapWindow_Resize(object sender, System.EventArgs e)
		{
			this.lstUsers.Location = new System.Drawing.Point(this.Width-this.lstUsers.Width-20, 4);
			this.lstUsers.Height = this.Height;

			//Display party list?
			if(this.lstUsers.Visible)
				this.Map.Width = this.Width - 120;
			else
				this.Map.Width = this.Width;

			this.Map.Height = this.Height;
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
