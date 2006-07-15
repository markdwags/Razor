using System;

namespace Assistant.MapUO
{
    public class User
    {
        private short m_X;
        private short m_Y;
        private string m_Name;
        public User(string name)
        {
            this.m_Name = name;
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
