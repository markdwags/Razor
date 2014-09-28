using System;
using System.Text;
using System.IO;
using System.Collections;

namespace Assistant.MapUO
{
    class Region
    {
        private int m_Height;
        private int m_Width;
        private int m_X;
        private int m_Y;
        public Region(string line)
        {
            string[] textArray1 = line.Split(new char[] { ' ' });
            this.m_X = int.Parse(textArray1[0]);
            this.m_Y = int.Parse(textArray1[1]);
            this.m_Width = int.Parse(textArray1[2]);
            this.m_Height = int.Parse(textArray1[3]);          
        } 

        public Region(int x, int y, int width, int height )
        {
            this.m_X = x;
            this.m_Y = y;
            this.m_Width = width;
            this.m_Height = height;
        }

 

        public static Region[] Load(string path)
        {
            if (!File.Exists(path))
            {
                return new Region[0];
            }
            ArrayList list1 = new ArrayList();
            try
            {
                using (StreamReader reader1 = new StreamReader(path))
                {
                    string text1;
                    while ((text1 = reader1.ReadLine()) != null)
                    {
                        if ((text1.Length != 0) && !text1.StartsWith("#"))
                        {
                            list1.Add(new Region(text1));
                        }
                    }
                }
            }
            catch
            {
            }
            return (Region[])list1.ToArray(typeof(Region));
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
        public int Length
        {
            get { return m_Height; }
            set { m_Height = value; }
        }
        public int Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

    }
}
