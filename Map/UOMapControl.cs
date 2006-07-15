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
        private ArrayList m_UserList;
        private User m_FocusUser;
        private Image m_Image;
        private Point prevPoint;
        public UOMapControl()
        {
            Active = false;
            this.prevPoint = new Point(0, 0);
            this.BorderStyle = BorderStyle.Fixed3D;
            this.m_UserList = new ArrayList();
            this.m_MapButtons = new ArrayList();
            m_Regions = Assistant.MapUO.Region.Load("guardlines.def");
            m_MapButtons = UOMapRuneButton.Load("test.xm");
        }

        public void MapClick(System.Windows.Forms.MouseEventArgs e)
        {
            if (Active)
            {
                Point clickedbox = MousePointToMapPoint(new Point(e.X, e.Y));
                UOMapRuneButton button = ButtonCheck(new Rectangle(new Point(clickedbox.X-2,clickedbox.Y-2), new Size(5, 5)));
                if(button != null)
                button.OnClick(e);
            }
        }
        
        private Point MousePointToMapPoint(Point p)
        {
            double rad = (Math.PI / 180) * 45;
            int w = (this.Width) >> 3;
            int h = (this.Height) >> 3;
            Point mapOrigin = new Point((m_FocusUser.X >> 3) - (w / 2), (m_FocusUser.Y >> 3) - (h / 2));
            Point pnt1 = new Point((mapOrigin.X << 3) + (p.X), (mapOrigin.Y << 3) + (p.Y));
            Point check = new Point(pnt1.X - m_FocusUser.X, pnt1.Y - m_FocusUser.Y);
            check = RotatePoint(new Point((int)(check.X*0.695),(int)(check.Y*0.68)), rad, 1);
            return new Point(check.X + m_FocusUser.X, check.Y + m_FocusUser.Y);
        }
        private Point RotatePoint(Point p, double angle,double dist)
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
                Rectangle rec2 = new Rectangle(new Point(mbutton.X, mbutton.Y), new Size(10, 10));
                if (rec2.IntersectsWith(rec))
                    return mbutton;

            }
            return null;
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            try
            {
                if (Active)
                {
                    
                    CreateMap();
                    Bitmap map = DrawLocals();
                    pe.Graphics.FillRectangle(Brushes.Black, new Rectangle(0, 0, this.Width, this.Height));
                    int xtrans = Size.Width / 2;
                    int ytrans = Size.Height / 2;


                    pe.Graphics.TranslateTransform(-xtrans, -ytrans, MatrixOrder.Append);
                   
                    pe.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    pe.Graphics.PageUnit = GraphicsUnit.Pixel;
                    pe.Graphics.ScaleTransform(1.5F, 1.5F, MatrixOrder.Append);
                    pe.Graphics.RotateTransform(45, MatrixOrder.Append);
                    pe.Graphics.TranslateTransform(xtrans, ytrans, MatrixOrder.Append);
                    
                    Point offset = new Point(this.m_FocusUser.X & 7, this.m_FocusUser.Y & 7);
                    if (this.m_Image != null)
                    {
                           pe.Graphics.DrawImage(this.m_Image, 0 - offset.X, 0 - offset.Y);
                           pe.Graphics.DrawImage(map, 0 -offset.X, 0-offset.Y );
                    }
                    pe.Graphics.ScaleTransform(1F, 1F, MatrixOrder.Append);
                    Font font2 = new Font("Courier New", 8,FontStyle.Bold);

                    int w = (Size.Width) >> 3;
                    int h = (Size.Height) >> 3;
                    Point mapOrigin = new Point((m_FocusUser.X >> 3) - (w / 2), (m_FocusUser.Y >> 3) - (h / 2));
                    Point pntPlayer = new Point((m_FocusUser.X) - (mapOrigin.X << 3) - offset.X, (m_FocusUser.Y) - (mapOrigin.Y << 3) - offset.Y);
                    Brush compass = Brushes.Red;
                    pe.Graphics.DrawString("W", font2, compass, new Point(pntPlayer.X - 25, pntPlayer.Y - 5));
                    pe.Graphics.DrawString("E", font2, compass, new Point(pntPlayer.X + 15, pntPlayer.Y - 5));
                    pe.Graphics.DrawString("N", font2, compass, new Point(pntPlayer.X - 5, pntPlayer.Y - 25));
                    pe.Graphics.DrawString("S", font2, compass, new Point(pntPlayer.X - 5, pntPlayer.Y + 15));

                   // Point pntTest = new Point((3088) - (mapOrigin.X << 3) - offset.X, (296) - (mapOrigin.Y << 3) - offset.Y);
                   // pe.Graphics.FillRectangle(Brushes.Wheat, new Rectangle(pntTest, new Size(3,3)));
                    int xLong = 0, yLat = 0;
                    int xMins = 0, yMins = 0;
                    bool xEast = false, ySouth = false;

                    if (Format(new Point(this.m_FocusUser.X,this.m_FocusUser.Y),Ultima.Map.Felucca, ref xLong, ref yLat, ref xMins, ref yMins, ref xEast, ref ySouth))
                    {
                        
                        string locString = String.Format("{0}° {1}'{2}", yLat, yMins, ySouth ? "S" : "N");
                        locString += " " + String.Format("{0}° {1}'{2}", xLong, xMins, xEast ? "E" : "W");
                        locString += " | (" + this.m_FocusUser.X + " : " + this.m_FocusUser.Y + ")";
                        pe.Graphics.ResetTransform();

                        Font font = new Font("Courier New", 8);
                        pe.Graphics.FillRectangle(Brushes.Wheat, new Rectangle(0, 0, locString.Length * 7, 12));
                        pe.Graphics.DrawRectangle(new Pen(Brushes.Black), new Rectangle(0, 0, locString.Length * 7, 12));
                        pe.Graphics.DrawString(locString, font, Brushes.Black, new Point(0, 0));
                    }
                    //1479 1624
                  
                    

                    
                }
            }
            catch { }
            base.OnPaint(pe);
        }
        
        private void CreateMap()
        {
            if (m_FocusUser == null)
                m_FocusUser = new User("default");

            int w = (Size.Width) >> 3;
            int h = (Size.Height) >> 3;
            Point pnt = new Point((this.m_FocusUser.X >> 3) - (w / 2), (m_FocusUser.Y >> 3) - (h / 2));

            if (pnt != this.prevPoint)
            {
                this.prevPoint = pnt;
                Bitmap bmp = new Bitmap(this.Width, this.Height);
                Graphics pe = Graphics.FromImage(bmp);
                int xtrans = Size.Width / 2;
                int ytrans = Size.Height / 2;
                pe.DrawImage(Ultima.Map.Felucca.GetImage(pnt.X, pnt.Y, w + (Size.Width & 7), h + (Size.Height & 7), true), new Point(0, 0));
                this.m_Image = bmp;
                pe.Dispose();
            }
            
        }
        private Bitmap DrawLocals()
        {
            
            int w = (Size.Width) >> 3;
            int h = (Size.Height) >> 3;
            Bitmap bmp = new Bitmap(this.m_Image.Width,this.m_Image.Height);
            Graphics gf = Graphics.FromImage(bmp);
            //Region Display and buttons
            ArrayList regions = new ArrayList();
            ArrayList mButtons = new ArrayList();
            if (this.Width > this.Height)
            {
                regions = RegionList(m_FocusUser.X, m_FocusUser.Y, this.Width);
                mButtons = ButtonList(m_FocusUser.X, m_FocusUser.Y, this.Width);
            }
            else
            {
                regions = RegionList(m_FocusUser.X, m_FocusUser.Y, this.Height);
                mButtons = ButtonList(m_FocusUser.X, m_FocusUser.Y, this.Height);
            }
            Point mapOrigin = new Point((m_FocusUser.X >> 3) - (w / 2), (m_FocusUser.Y >> 3) - (h / 2));

            //Player point
            Point pntPlayer = new Point((m_FocusUser.X) - (mapOrigin.X << 3), (m_FocusUser.Y) - (mapOrigin.Y << 3));
            gf.FillRectangle(Brushes.Red, new Rectangle(pntPlayer.X, pntPlayer.Y,1,1));
            gf.DrawEllipse(new Pen(Brushes.Silver), new Rectangle(pntPlayer.X - 2, pntPlayer.Y - 2, 4, 4));
            foreach (Assistant.MapUO.Region region in regions)
            {
                Point pnt1 = new Point((region.X) - ((mapOrigin.X << 3)+(0)), (region.Y) - ((mapOrigin.Y << 3)+(0)));
               gf.DrawRectangle(new Pen(Brushes.LimeGreen), new Rectangle(pnt1, new Size(region.Width, region.Length)));

            }
          
            foreach (User usr in this.m_UserList)
            {
                if (usr != null)
                {
                    if (usr != this.m_FocusUser)
                    {
                        Point pnt1 = new Point((usr.X) - (mapOrigin.X << 3), (usr.Y) - (mapOrigin.Y << 3));
                        gf.FillRectangle(Brushes.Wheat, new Rectangle(new Point(pnt1.X, pnt1.Y), new Size(1, 1)));
                    }
                }
            }
           
            gf.Dispose();
            return bmp;

        }
        private Bitmap CreateText(string text)
        {

            Bitmap bmp = new Bitmap(100, 100);
            Graphics gf = Graphics.FromImage(bmp);
            int xtrans = bmp.Width / 2;
            int ytrans = bmp.Height / 2;

            gf.TranslateTransform(-xtrans, -ytrans, MatrixOrder.Append);
            gf.RotateTransform(-45, MatrixOrder.Append);
            gf.TranslateTransform(xtrans, ytrans, MatrixOrder.Append);

            Font fnt = new Font("Arial", 10,FontStyle.Bold);
            gf.DrawString(text, fnt, Brushes.Wheat, new Point(xtrans - (int)(text.Length * 3.2), ytrans - 8));
            gf.RotateTransform(45, MatrixOrder.Append);
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
                    this.Invoke(d, new object[] { });
                }
                else
                {
                    if (this.m_UserList.Count < 1)
                        this.m_UserList.Add(this.m_FocusUser);
                    Point pnt = new Point( Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y );
                    this.m_FocusUser.X = System.Convert.ToInt16(pnt.X);
                    this.m_FocusUser.Y = System.Convert.ToInt16(pnt.Y);
                    this.Refresh();
                }
            }
            catch { }
        }
        private ArrayList RegionList(int x,int y, int maxDist)
        {
            int count = m_Regions.Length;
            ArrayList aList = new ArrayList();
            for (int i = 0; i < count; i++)
            {
                Assistant.MapUO.Region rg1 = this.m_Regions[i];
                if (Distance(rg1.X, rg1.Y, x, y) <= maxDist*2)
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
                if (Distance(btn.X, btn.Y, x, y) <= maxDist * 2)
                {
                    aList.Add(btn);
                }
            }
            return aList;
        }
       
        public int Distance(int x1, int y1, int x, int y)
        {
            int xd = Math.Abs((int)(x1 - x));
            int yd = Math.Abs((int)(y1 - y));
            if (xd <= yd)
            {
                return yd;
            }
            return xd;
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
            else if (x >= 0 && y >= 0/* && x < map.Width && y < map.Height*/)
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
            set { m_Active = value; this.Refresh(); }
        }
        public ArrayList UserList
        {
            get { return m_UserList; }
            set { m_UserList = value; }
        }
        public ArrayList MapButtons
        {
            get { return m_MapButtons; }
            set { m_MapButtons = value; }
        }
        public User FocusUser
        {
            get { return m_FocusUser; }
            set { m_FocusUser = value; }
        }
    }
   
}
