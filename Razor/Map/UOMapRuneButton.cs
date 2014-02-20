using System;
using System.Text;
using System.Drawing;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using Ultima;

namespace Assistant.MapUO
{
    class UOMapRuneButton
    {
        private int m_BookID;
        private int m_RuneSpot;
        private int m_X;
        private int m_Y;
        private Bitmap m_Icon;
        public UOMapRuneButton(int bookid,int runeSpot,int x,int y)
        {
            this.m_BookID = bookid;
            this.m_RuneSpot = runeSpot;
            this.m_X = x;
            this.m_Y = y;
            this.m_Icon = Ultima.Art.GetStatic(7956);
        }
        public static ArrayList Load(string path)
        {
            
            ArrayList buttonlist = new ArrayList();
            //if (!File.Exists(path))
           // {
            //    return buttonlist;
           // }
            buttonlist.Add(new UOMapRuneButton(0, 0, 1158, 743));
            buttonlist.Add(new UOMapRuneButton(0, 0, 3230, 305));
            //XML shit
            return buttonlist;
        }
        public void OnClick(MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    //recall
                    break;
                case MouseButtons.Right:
                    //gate
                    break;
            }

        }
        public int X
        {
            get { return m_X; }
            set { m_X = value; }
        }
        public int Y
        {
            get { return m_Y; }
            set { m_Y = value; }
        }
        public int BookID
        {
            get { return m_BookID; }
            set { m_BookID = value; }
        }
        public int RuneSpot
        {
            get { return m_RuneSpot; }
            set { m_RuneSpot = value; }
        }
        public Bitmap Icon
        {
            get { return m_Icon; }
            set { m_Icon = value; }
        }

    }
}
