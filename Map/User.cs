using System;
using System.Drawing;
namespace Assistant.MapUO
{
    public class User
    {
		public Point ButtonPoint;
        private short m_X;
        private short m_Y;
        private string m_Name;
        private uint m_Serial;
        public User(uint serial, string name)
        {
            this.m_Serial = serial;
            this.m_Name = name;
        }
        public uint Serial
        {
            get { return m_Serial; }
            set { m_Serial = value; }
        }
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        public short X
        {
            get { return m_X; }
            set { m_X = value; }
        }
        public short Y
        {
            get { return m_Y; }
            set { m_Y = value; }
        }
    }
}
