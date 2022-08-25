#region license
// Razor: An Ultima Online Assistant
// Copyright (c) 2022 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace Assistant.MapUO
{
    class UOMapRuneButton
    {
        private int m_BookID;
        private int m_RuneSpot;
        private int m_X;
        private int m_Y;
        private Bitmap m_Icon;

        public UOMapRuneButton(int bookid, int runeSpot, int x, int y)
        {
            this.m_BookID = bookid;
            this.m_RuneSpot = runeSpot;
            this.m_X = x;
            this.m_Y = y;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
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