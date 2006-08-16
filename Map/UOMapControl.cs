using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Assistant.MapUO
{
	public class UOMapControl : PictureBox
	{
		delegate void UpdateMapCallback();

		private bool m_Active;
		private Region[] m_Regions;
		private ArrayList m_MapButtons;
		private Image m_Image;
		private Point prevPoint;
		private Mobile m_Focus;

		public Mobile FocusMobile
		{
			get 
			{
				if ( m_Focus == null || m_Focus.Deleted || !PacketHandlers.Party.Contains( m_Focus.Serial ) )
				{
					/*if ( World.Player == null )
						return new Mobile( Serial.Zero );
					else*/
					return World.Player;
				}
				return m_Focus; 
			}
			set { m_Focus = value; }
		}

		public UOMapControl()
		{
			Active = false;
			this.prevPoint = new Point(0, 0);
			this.BorderStyle = BorderStyle.Fixed3D;
			this.m_MapButtons = new ArrayList();
			m_Regions = Assistant.MapUO.Region.Load("guardlines.def");
			m_MapButtons = UOMapRuneButton.Load("test.xml");
		}

		public void MapClick(System.Windows.Forms.MouseEventArgs e)
		{
			if (Active)
			{
				Point clickedbox = MousePointToMapPoint(new Point(e.X, e.Y));
				UOMapRuneButton button = ButtonCheck(new Rectangle( clickedbox.X - 2, clickedbox.Y - 2, 5, 5 ));
				if (button != null)
					button.OnClick(e);
			}
		}

		private Point MousePointToMapPoint(Point p)
		{
			double rad = (Math.PI / 180) * 45;
			int w = (this.Width) >> 3;
			int h = (this.Height) >> 3;
			Point3D focus = this.FocusMobile.Position;
		
			Point mapOrigin = new Point((focus.X >> 3) - (w / 2), (focus.Y >> 3) - (h / 2));
			Point pnt1 = new Point((mapOrigin.X << 3) + (p.X), (mapOrigin.Y << 3) + (p.Y));
			Point check = new Point(pnt1.X - focus.X, pnt1.Y - focus.Y);
			check = RotatePoint(new Point((int)(check.X * 0.695), (int)(check.Y * 0.68)), rad, 1);
			return new Point(check.X + focus.X, check.Y + focus.Y);
		}

		private Point RotatePoint(Point p, double angle, double dist)
		{
			int x = (int)((p.X * Math.Cos(angle) + p.Y * Math.Sin(angle)) * dist);
			int y = (int)((-p.X * Math.Sin(angle) + p.Y * Math.Cos(angle)) * dist);

			return new Point(x, y);
		}

		private UOMapRuneButton ButtonCheck(Rectangle rec)
		{
			ArrayList buttons = new ArrayList();
			foreach (UOMapRuneButton mbutton in this.m_MapButtons)
			{
				Rectangle rec2 = new Rectangle( mbutton.X, mbutton.Y, 10, 10 );
				if (rec2.IntersectsWith(rec))
					return mbutton;

			}
			return null;
		}

		protected override void Dispose(bool disposing)
		{
			this.m_Image.Dispose();
			base.Dispose(disposing);
		}

		private static Font m_BoldFont = new Font( "Courier New", 8, FontStyle.Bold );
		private static Font m_SmallFont = new Font( "Courier New", 6 );
		private static Font m_RegFont = new Font( "Courier New", 8 );

		protected override void OnPaint(PaintEventArgs pe)
		{
			try
			{
				if (Active)
				{
					CreateMap();

					Bitmap map = DrawLocals();
					pe.Graphics.FillRectangle(Brushes.Black, new Rectangle(0, 0, this.Width, this.Height));
					int xtrans = this.Width / 2;
					int ytrans = this.Height / 2;

					pe.Graphics.TranslateTransform(-xtrans, -ytrans, MatrixOrder.Append);
					pe.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					pe.Graphics.PageUnit = GraphicsUnit.Pixel;
					pe.Graphics.ScaleTransform(1.5F, 1.5F, MatrixOrder.Append);
					pe.Graphics.RotateTransform(45, MatrixOrder.Append);
					pe.Graphics.TranslateTransform(xtrans, ytrans, MatrixOrder.Append);


					Point3D focus = this.FocusMobile.Position;
					Point offset = new Point(focus.X & 7, focus.Y & 7);
					if (this.m_Image != null)
					{
						pe.Graphics.DrawImage(this.m_Image, 0 - offset.X, 0 - offset.Y);
						pe.Graphics.DrawImage(map, 0 - offset.X, 0 - offset.Y);
					}
					pe.Graphics.ScaleTransform(1F, 1F, MatrixOrder.Append);
					//1218 738
					//create compass
                    
					int w = (this.Width) >> 3;
					int h = (this.Height) >> 3;
					Point mapOrigin = new Point((focus.X >> 3) - (w / 2), (focus.Y >> 3) - (h / 2));
					Point pntPlayer = new Point((focus.X) - (mapOrigin.X << 3) - offset.X, (focus.Y) - (mapOrigin.Y << 3) - offset.Y);
					Brush compass = Brushes.Red;

					pe.Graphics.DrawString("W", m_BoldFont, compass, pntPlayer.X - 35, pntPlayer.Y - 5);
					pe.Graphics.DrawString("E", m_BoldFont, compass, pntPlayer.X + 25, pntPlayer.Y - 5);
					pe.Graphics.DrawString("N", m_BoldFont, compass, pntPlayer.X - 5, pntPlayer.Y - 35);
					pe.Graphics.DrawString("S", m_BoldFont, compass, pntPlayer.X - 5, pntPlayer.Y + 25);

					foreach ( Serial s in PacketHandlers.Party )
					{
						Mobile mob = World.FindMobile( s );
						if ( mob == null )
							continue;

						if ( mob != this.FocusMobile )
						{
							string name = mob.Name;
							if ( name.Length < 1 )
								name = "(Not Seen)";
						
							Point pntTest = new Point((mob.Position.X) - (mapOrigin.X << 3) - offset.X, (mob.Position.Y) - (mapOrigin.Y << 3) - offset.Y);
							pe.Graphics.FillRectangle(Brushes.Gold, pntTest.X, pntTest.Y, 2, 2);
							pe.Graphics.DrawString(name, m_SmallFont, Brushes.Wheat, pntTest);
						}
					}

					int xLong = 0, yLat = 0;
					int xMins = 0, yMins = 0;
					bool xEast = false, ySouth = false;

					/*Point pntTest2 = new Point((3251) - (mapOrigin.X << 3) - offset.X, (305) - (mapOrigin.Y << 3) - offset.Y);
					pe.Graphics.FillRectangle(Brushes.Blue, pntTest2.X, pntTest2.Y, 2, 2);


					// pntTest2 = RotatePoint(new Point(pntTest2.X-pntPlayer.X,pntTest2.Y-pntPlayer.Y), 45, 0);
					// pntTest2 = new Point(pntTest2.X + m_FocusUser.X, pntTest2.Y + m_FocusUser.Y);
					//pntTest2 = new Point((pntTest2.X) - (mapOrigin.X << 3) - offset.X, (pntTest2.Y) - (mapOrigin.Y << 3) - offset.Y);
					//Point mapOrigin2 = new Point((pntTest2.X >> 3) - (w / 2), (pntTest2.Y >> 3) - (h / 2));
					// pntTest2 = new Point((3224) - (mapOrigin.X << 3) - offset.X, (293) - (mapOrigin.Y << 3) - offset.Y);
					pe.Graphics.FillRectangle(Brushes.Pink, pntTest2.X, pntTest2.Y, 2, 2 );
					pe.Graphics.DrawString("Jenova", m_RegFont, Brushes.Wheat, pntTest2);*/

					if (Format(new Point(focus.X, focus.Y), Ultima.Map.Felucca, ref xLong, ref yLat, ref xMins, ref yMins, ref xEast, ref ySouth))
					{
						string locString = String.Format("{0}°{1}'{2} {3}°{4}'{5}", yLat, yMins, ySouth ? "S" : "N", xLong, xMins, xEast ? "E" : "W");
						pe.Graphics.ResetTransform();
						pe.Graphics.FillRectangle(Brushes.Wheat, 0, 0, locString.Length * 7, 12);
						pe.Graphics.DrawRectangle(Pens.Black, 0, 0, locString.Length * 7, 12);
						pe.Graphics.DrawString(locString, m_RegFont, Brushes.Black, 0, 0 );
					}
				}
			}
			catch { }
			base.OnPaint(pe);
		}
		
		private void CreateMap()
		{
			int w = (this.Width) >> 3;
			int h = (this.Height) >> 3;
			Point pnt = new Point((this.FocusMobile.Position.X >> 3) - (w / 2), (this.FocusMobile.Position.Y >> 3) - (h / 2));

			if (pnt != this.prevPoint)
			{
				this.prevPoint = pnt;
				Bitmap bmp = new Bitmap(this.Width, this.Height);
				Graphics pe = Graphics.FromImage(bmp);
				int xtrans = this.Width / 2;
				int ytrans = this.Height / 2;
				pe.DrawImage(Ultima.Map.Felucca.GetImage(pnt.X, pnt.Y, w + (this.Width & 7), h + (this.Height & 7), true), new Point(0, 0));
				this.m_Image = bmp;
				pe.Dispose();
			}
		}

		private Bitmap DrawLocals()
		{
			Point3D focus = this.FocusMobile.Position;

			int w = (this.Width) >> 3;
			int h = (this.Height) >> 3;
			Bitmap bmp = new Bitmap(this.m_Image.Width, this.m_Image.Height);
			Graphics gf = Graphics.FromImage(bmp);
			//Region Display and buttons
			ArrayList regions = new ArrayList();
			ArrayList mButtons = new ArrayList();
			if (this.Width > this.Height)
			{
				regions = RegionList(focus.X, focus.Y, this.Width);
				mButtons = ButtonList(focus.X, focus.Y, this.Width);
			}
			else
			{
				regions = RegionList(focus.X, focus.Y, this.Height);
				mButtons = ButtonList(focus.X, focus.Y, this.Height);
			}
			Point mapOrigin = new Point((focus.X >> 3) - (w / 2), (focus.Y >> 3) - (h / 2));

			//Player point
			Point pntPlayer = new Point((focus.X) - (mapOrigin.X << 3), (focus.Y) - (mapOrigin.Y << 3));
			gf.FillRectangle(Brushes.Red, pntPlayer.X, pntPlayer.Y, 1, 1);
			gf.DrawEllipse(Pens.Silver, pntPlayer.X - 2, pntPlayer.Y - 2, 4, 4);
			foreach (Assistant.MapUO.Region region in regions)
			{
				gf.DrawRectangle(Pens.LimeGreen, (region.X) - ((mapOrigin.X << 3) + (0)), (region.Y) - ((mapOrigin.Y << 3) + (0)), region.Width, region.Length);
			}
			gf.Dispose();
			return bmp;
		}

		protected override void OnResize(EventArgs e)
		{
			this.Refresh();
			base.OnResize(e);
		}

		public void UpdateMap()
		{
			try
			{
				if (this.InvokeRequired)
				{
					UpdateMapCallback d = new UpdateMapCallback(UpdateMap);
					this.Invoke(d, new object[0] );
				}
				else
				{
					this.Refresh();
				}
			}
			catch { }
		}

		private ArrayList RegionList(int x, int y, int maxDist)
		{

			int count = m_Regions.Length;
			ArrayList aList = new ArrayList();
			for (int i = 0; i < count; i++)
			{
				Assistant.MapUO.Region rg1 = this.m_Regions[i];
				if (Utility.Distance(rg1.X, rg1.Y, x, y) <= maxDist * 2)
				{
					aList.Add(rg1);
				}
			}
			return aList;
		}

		private ArrayList ButtonList(int x, int y, int maxDist)
		{
			if (this.m_MapButtons == null)
				return null;
			int count = this.m_MapButtons.Count;
			ArrayList aList = new ArrayList();
			for (int i = 0; i < count; i++)
			{
				UOMapRuneButton btn = (UOMapRuneButton)this.m_MapButtons[i];
				if (Utility.Distance(btn.X, btn.Y, x, y) <= maxDist * 2)
				{
					aList.Add(btn);
				}
			}
			return aList;
		}

		public static bool Format(Point p, Ultima.Map map, ref int xLong, ref int yLat, ref int xMins, ref int yMins, ref bool xEast, ref bool ySouth)
		{
			if (map == null)
				return false;

			int x = p.X, y = p.Y;
			int xCenter, yCenter;
			int xWidth, yHeight;

			if (!ComputeMapDetails(map, x, y, out xCenter, out yCenter, out xWidth, out yHeight))
				return false;

			double absLong = (double)((x - xCenter) * 360) / xWidth;
			double absLat = (double)((y - yCenter) * 360) / yHeight;

			if (absLong > 180.0)
				absLong = -180.0 + (absLong % 180.0);

			if (absLat > 180.0)
				absLat = -180.0 + (absLat % 180.0);

			bool east = (absLong >= 0), south = (absLat >= 0);

			if (absLong < 0.0)
				absLong = -absLong;

			if (absLat < 0.0)
				absLat = -absLat;

			xLong = (int)absLong;
			yLat = (int)absLat;

			xMins = (int)((absLong % 1.0) * 60);
			yMins = (int)((absLat % 1.0) * 60);

			xEast = east;
			ySouth = south;

			return true;
		}

		public static bool ComputeMapDetails(Ultima.Map map, int x, int y, out int xCenter, out int yCenter, out int xWidth, out int yHeight)
		{
			xWidth = 5120; yHeight = 4096;

			if (map == Ultima.Map.Trammel || map == Ultima.Map.Felucca)
			{
				if (x >= 0 && y >= 0 && x < 5120 && y < 4096)
				{
					xCenter = 1323; yCenter = 1624;
				}
				else if (x >= 5120 && y >= 2304 && x < 6144 && y < 4096)
				{
					xCenter = 5936; yCenter = 3112;
				}
				else
				{
					xCenter = 0; yCenter = 0;
					return false;
				}
			}
			else if (x >= 0 && y >= 0 /*&& x < map.Width && y < map.Height*/)
			{
				xCenter = 1323; yCenter = 1624;
			}
			else
			{
				xCenter = 0; yCenter = 0;
				return false;
			}

			return true;
		}

		public bool Active
		{
			get { return m_Active; }
			set { m_Active = value; if ( value ) { this.Refresh(); } }
		}

		public ArrayList MapButtons
		{
			get { return m_MapButtons; }
			set { m_MapButtons = value; }
		}
	}
}