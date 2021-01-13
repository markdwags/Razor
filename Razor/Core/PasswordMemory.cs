#region license

// Razor: An Ultima Online Assistant
// Copyright (C) 2021 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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
using System.Xml;
using System.Text;
using System.Collections;
using System.Net;

namespace Assistant
{
    public class PasswordMemory
    {
        private class Entry
        {
            public Entry()
            {
            }

            public Entry(string u, string p, IPAddress a)
            {
                User = u;
                Pass = p;
                Address = a;
            }

            public string User;
            public string Pass;
            public IPAddress Address;
        }

        private static ArrayList m_List = new ArrayList();

        public static string Encrypt(string source)
        {
            byte[] buff = ASCIIEncoding.ASCII.GetBytes(source);
            int kidx = 0;
            string key = Platform.GetWindowsUserName();
            if (key == String.Empty)
                return String.Empty;
            StringBuilder sb = new StringBuilder(source.Length * 2 + 2);
            sb.Append("1+");
            for (int i = 0; i < buff.Length; i++)
            {
                sb.AppendFormat("{0:X2}", (byte) (buff[i] ^ ((byte) key[kidx++])));
                if (kidx >= key.Length)
                    kidx = 0;
            }

            return sb.ToString();
        }

        public static string Decrypt(string source)
        {
            byte[] buff = null;

            if (source.Length > 2 && source[0] == '1' && source[1] == '+')
            {
                buff = new byte[(source.Length - 2) / 2];
                string key = Platform.GetWindowsUserName();
                if (key == String.Empty)
                    return String.Empty;
                int kidx = 0;
                for (int i = 2; i < source.Length; i += 2)
                {
                    byte c;
                    try
                    {
                        c = Convert.ToByte(source.Substring(i, 2), 16);
                    }
                    catch
                    {
                        continue;
                    }

                    buff[(i - 2) / 2] = (byte) (c ^ ((byte) key[kidx++]));
                    if (kidx >= key.Length)
                        kidx = 0;
                }
            }
            else
            {
                byte key = (byte) (source.Length / 2);
                buff = new byte[key];

                for (int i = 0; i < source.Length; i += 2)
                {
                    byte c;
                    try
                    {
                        c = Convert.ToByte(source.Substring(i, 2), 16);
                    }
                    catch
                    {
                        continue;
                    }

                    buff[i / 2] = (byte) (c ^ key++);
                }
            }

            return ASCIIEncoding.ASCII.GetString(buff);
        }

        public static void Load(XmlElement xml)
        {
            ClearAll();

            if (xml == null)
                return;

            foreach (XmlElement el in xml.GetElementsByTagName("password"))
            {
                try
                {
                    string user = el.GetAttribute("user");
                    string addr = el.GetAttribute("ip");

                    if (el.InnerText == null)
                        continue;

                    m_List.Add(new Entry(user, el.InnerText, IPAddress.Parse(addr)));
                }
                catch
                {
                }
            }
        }

        public static void Save(XmlTextWriter xml)
        {
            if (m_List == null)
                return;

            foreach (Entry e in m_List)
            {
                if (e.Pass != String.Empty)
                {
                    xml.WriteStartElement("password");
                    try
                    {
                        xml.WriteAttributeString("user", e.User);
                        xml.WriteAttributeString("ip", e.Address.ToString());
                        xml.WriteString(e.Pass);
                    }
                    catch
                    {
                    }

                    xml.WriteEndElement();
                }
            }
        }

        public static void ClearAll()
        {
            m_List.Clear();
        }

        public static void Add(string user, string pass, IPAddress addr)
        {
            if (pass == "")
                return;

            user = user.ToLower();
            for (int i = 0; i < m_List.Count; i++)
            {
                Entry e = (Entry) m_List[i];
                if (e.User == user && e.Address.Equals(addr))
                {
                    e.Pass = Encrypt(pass);
                    return;
                }
            }

            m_List.Add(new Entry(user, Encrypt(pass), addr));
        }

        public static string Find(string user, IPAddress addr)
        {
            user = user.ToLower();
            for (int i = 0; i < m_List.Count; i++)
            {
                Entry e = (Entry) m_List[i];
                if (e.User == user && e.Address.Equals(addr))
                    return Decrypt(e.Pass);
            }

            return String.Empty;
        }
    }
}