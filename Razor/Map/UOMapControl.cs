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
		private Point prevPoint;
		private Mobile m_Focus;
	    private const double RotateAngle = Math.PI / 4 + Math.PI;
		private Bitmap m_Background;
        private DateTime LastRefresh;

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

		private static Font m_BoldFont = new Font( "Courier New", 8, FontStyle.Bold );
		private static Font m_SmallFont = new Font( "Arial", 6 );
		private static Font m_RegFont = new Font( "Arial", 8 );
        public override void Refresh()
        {
            TimeSpan now =  DateTime.Now - LastRefresh;
            if (now.TotalMilliseconds <= 100)
                return;
            LastRefresh = DateTime.Now;
            base.Refresh();
        }

        private PointF AdjustPoint(PointF center, PointF pos)
        {
            PointF newp = new PointF(center.X - pos.X, center.Y - pos.Y);
            float dis = (float)Distance(center, pos);
            dis += dis * 0.50f;
            float slope = 0;
            if (newp.X != 0)
                slope = (float)newp.Y / (float)newp.X;
            else
                return new PointF(0 + center.X, -1f * (newp.Y + (newp.Y * 0.25f)) + center.Y);
            slope *= -1;
            //Both of these algorithms oddly produce the same results.
           //float x = dis / (float)(Math.Sqrt(1f + Math.Pow(slope, 2)));
            float x = newp.X + (newp.X * 0.5f);
           // if (newp.X > 0)
                x *= -1;
            float y = (-1) * slope * x;

            PointF def = new PointF(x + center.X, y + center.Y);

            return def;
        }
		public double Distance(PointF center, PointF pos)
		{

			PointF newp = new PointF(center.X - pos.X, center.Y - pos.Y);
			double distX = Math.Pow(newp.X,2);
			double distY = Math.Pow(newp.Y, 2);
			return Math.Sqrt(distX + distY);
		}
		private PointF RotatePoint(PointF center, PointF pos)
		{
			PointF newp = new PointF(center.X - pos.X, center.Y - pos.Y);
			double x = newp.X * Math.Cos(RotateAngle ) - newp.Y * Math.Sin(RotateAngle);
			double y = newp.X * Math.Sin(RotateAngle ) + newp.Y * Math.Sin(RotateAngle);
            return AdjustPoint(center, new PointF((float)(x) + center.X, (float)(y) + center.Y));
		}
		
		public void FullUpdate()
		{
			if ( !Active )
				return;

			if ( m_Background != null )
				m_Background.Dispose();
			m_Background = new Bitmap( this.Width, this.Height );

			int xLong = 0, yLat = 0;
			int xMins = 0, yMins = 0;
			bool xEast = false, ySouth = false;

			int w = (this.Width) >> 3;
			int h = (this.Height) >> 3;
			int xtrans = this.Width / 2;
			int ytrans = this.Height / 2;
			Point3D focus = this.FocusMobile.Position;
			Point offset = new Point(focus.X & 7, focus.Y & 7);
			Point mapOrigin = new Point((focus.X >> 3) - (w / 2), (focus.Y >> 3) - (h / 2));
			Point pntPlayer = new Point((focus.X) - (mapOrigin.X << 3) - offset.X, (focus.Y) - (mapOrigin.Y << 3) - offset.Y);

			Graphics gfx = Graphics.FromImage(m_Background);

			gfx.FillRectangle( Brushes.Black, 0, 0, this.Width, this.Height );
			
			gfx.TranslateTransform( -xtrans, -ytrans, MatrixOrder.Append );
			gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			gfx.PageUnit = GraphicsUnit.Pixel;
			gfx.ScaleTransform( 1.5f, 1.5f, MatrixOrder.Append );
			gfx.RotateTransform( 45, MatrixOrder.Append );
			gfx.TranslateTransform( xtrans, ytrans, MatrixOrder.Append );

			Ultima.Map map = Map.GetMap( this.FocusMobile.Map );
			if ( map == null )
				map = Ultima.Map.Felucca;

			gfx.DrawImage( map.GetImage( mapOrigin.X, mapOrigin.Y, w + offset.X, h + offset.Y, true ), -offset.X, -offset.Y );
			
			gfx.ScaleTransform( 1.0f, 1.0f, MatrixOrder.Append );

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

			foreach ( Assistant.MapUO.Region region in regions )
				gfx.DrawRectangle( Pens.LimeGreen, (region.X) - ((mapOrigin.X << 3) + offset.X), (region.Y) - ((mapOrigin.Y << 3) + offset.Y), region.Width, region.Length );
			
			gfx.DrawLine( Pens.Silver, pntPlayer.X-2, pntPlayer.Y-2, pntPlayer.X+2, pntPlayer.Y+2 );
			gfx.DrawLine( Pens.Silver, pntPlayer.X-2, pntPlayer.Y+2, pntPlayer.X+2, pntPlayer.Y-2 );
			gfx.FillRectangle( Brushes.Red, pntPlayer.X, pntPlayer.Y, 1, 1 );
			//gfx.DrawEllipse( Pens.Silver, pntPlayer.X - 2, pntPlayer.Y - 2, 4, 4 );
			
			gfx.DrawString("W", m_BoldFont, Brushes.Red, pntPlayer.X - 35, pntPlayer.Y - 5);
			gfx.DrawString("E", m_BoldFont, Brushes.Red, pntPlayer.X + 25, pntPlayer.Y - 5);
			gfx.DrawString("N", m_BoldFont, Brushes.Red, pntPlayer.X - 5, pntPlayer.Y - 35);
			gfx.DrawString("S", m_BoldFont, Brushes.Red, pntPlayer.X - 5, pntPlayer.Y + 25);

			gfx.ResetTransform();

			
			if (Format(new Point(focus.X, focus.Y), Ultima.Map.Felucca, ref xLong, ref yLat, ref xMins, ref yMins, ref xEast, ref ySouth))
			{
				string locString = String.Format( "{0}°{1}'{2} {3}°{4}'{5} | ({6},{7})", yLat, yMins, ySouth ? "S" : "N", xLong, xMins, xEast ? "E" : "W", World.Player.Position.X, World.Player.Position.Y );
				SizeF size = gfx.MeasureString( locString, m_RegFont );
				gfx.FillRectangle( Brushes.Wheat, 0, 0, size.Width + 2, size.Height + 2 );
				gfx.DrawRectangle( Pens.Black, 0, 0, size.Width + 2, size.Height + 2 );
				gfx.DrawString( locString, m_RegFont, Brushes.Black, 1, 1 );
			}
			
			gfx.ResetTransform();
			gfx.Dispose();

			this.Refresh();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			try
			{
				if (Active)
				{
					Graphics gfx = pe.Graphics;

					gfx.DrawImageUnscaled( m_Background, 0, 0 );

					int w = (this.Width) >> 3;
					int h = (this.Height) >> 3;
					int xtrans = this.Width / 2;
					int ytrans = this.Height / 2;
					Point3D focus = this.FocusMobile.Position;
					Point offset = new Point(focus.X & 7, focus.Y & 7);
					Point mapOrigin = new Point((focus.X >> 3) - (w / 2), (focus.Y >> 3) - (h / 2));

					gfx.TranslateTransform( -xtrans, -ytrans, MatrixOrder.Append );
					gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					gfx.PageUnit = GraphicsUnit.Pixel;
					gfx.ScaleTransform( 1.5f, 1.5f, MatrixOrder.Append );
					gfx.RotateTransform( 45, MatrixOrder.Append );
					gfx.TranslateTransform( xtrans, ytrans, MatrixOrder.Append );
					gfx.ScaleTransform( 1.0f, 1.0f, MatrixOrder.Append );
					gfx.ResetTransform();
					
					//Point pntTest2 = new Point((3256) - (mapOrigin.X << 3) - offset.X, (326) - (mapOrigin.Y << 3) - offset.Y);
					//PointF pntTest2F = RotatePoint(new Point(xtrans, ytrans), pntTest2);
					//gfx.FillRectangle(Brushes.LimeGreen, pntTest2F.X, pntTest2F.Y, 4, 4);
					/*foreach ( Serial s in PacketHandlers.Party )
					{
						Mobile mob = World.FindMobile( s );
						if ( mob == null )
							continue;
						if (mob == this.FocusMobile)
							continue;
						Point drawPoint = new Point((mob.Position.X) - (mapOrigin.X << 3) - offset.X, (mob.Position.Y) - (mapOrigin.Y << 3) - offset.Y);

						if (drawPoint.X < 0)
							drawPoint.X = 0;
						if (drawPoint.X > this.Width)
							drawPoint.X = this.Width;
						if (drawPoint.Y < 0)
							drawPoint.Y = 0;
						if (drawPoint.Y > this.Height)
							drawPoint.Y = this.Height;
						PointF drawPointF = RotatePoint(new Point(xtrans,ytrans),drawPoint);
						//gfx.FillRectangle(Brushes.Gold, drawPointF.X, drawPointF.Y, 2f, 2f);
					}*/
					foreach ( Serial s in PacketHandlers.Party )
					{
						Mobile mob = World.FindMobile( s );
						if ( mob == null )
							continue;

						if (mob == this.FocusMobile && mob == World.Player)
							continue;
						
						string name = mob.Name;
						if ( name == null || name.Length < 1 )
							name = "(Not Seen)";
						if (name != null && name.Length > 8)
							name = name.Substring(0, 8);
						Point drawPoint = new Point((mob.Position.X) - (mapOrigin.X << 3) - offset.X, (mob.Position.Y) - (mapOrigin.Y << 3) - offset.Y );
						if (drawPoint.X < 0)
							drawPoint.X = 0;
						if (drawPoint.X > this.Width)
							drawPoint.X = this.Width;
						if (drawPoint.Y < 0)
							drawPoint.Y = 0;
						if (drawPoint.Y > this.Height)
							drawPoint.Y = this.Height;

						PointF drawPointF = RotatePoint(new Point(xtrans,ytrans),drawPoint);
						gfx.FillRectangle(Brushes.Gold, drawPointF.X, drawPointF.Y, 2f, 2f);
						gfx.DrawString(name, m_RegFont, Brushes.White, drawPointF.X, drawPointF.Y);
					}

					if (World.Player != null)
					{
						if (World.Player != this.FocusMobile)
						{
							Mobile mob = World.Player;
							Point drawPoint = new Point((mob.Position.X) - (mapOrigin.X << 3) - offset.X, (mob.Position.Y) - (mapOrigin.Y << 3) - offset.Y);
							PointF drawPointF = RotatePoint(new Point(xtrans, ytrans), drawPoint);
							gfx.FillRectangle(Brushes.Gold, drawPointF.X, drawPointF.Y, 2, 2);
							drawPoint = new Point((mob.Position.X) - (mapOrigin.X << 3) - offset.X, (mob.Position.Y) - (mapOrigin.Y << 3) - offset.Y);
							drawPointF = RotatePoint(new Point(xtrans, ytrans), drawPoint);
							string name = mob.Name;
							if (name != null && name.Length > 8)
								name = name.Substring(0, 8);

							gfx.DrawString(name, m_RegFont, Brushes.White, drawPointF.X, drawPointF.Y);
						}
					}
				}
			}
			catch { }
			base.OnPaint(pe);
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
			m_Background.Dispose();
			m_Background = null;
			base.Dispose(disposing);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			FullUpdate();
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
				if (x >= 0 && y >= 0 && x < 5120 && y < map.Height)
				{
					xCenter = 1323; yCenter = 1624;
				}
				else if (x >= 5120 && y >= 2304 && x < 6144 && y < map.Height)
				{
					xCenter = 5936; yCenter = 3112;
				}
				else
				{
					xCenter = 0; yCenter = 0;
					return false;
				}
			}
			else if ( x >= 0 && y >= 0 && x < map.Width && y < map.Height )
			{
				xCenter = 1323; yCenter = 1624;
			}
			else
			{
				xCenter = map.Width / 2; yCenter = map.Height / 2;
				return false;
			}

			return true;
		}

		public bool Active
		{
			get { return m_Active; }
			set { m_Active = value; if ( value ) { FullUpdate(); } }
		}

		public ArrayList MapButtons
		{
			get { return m_MapButtons; }
			set { m_MapButtons = value; }
		}
	}
}
