#region license

// Razor: An Ultima Online Assistant
// Copyright (C) 2020 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Assistant.Core
{
    public class OverheadMessages
    {
        public class OverheadMessage
        {
            public string SearchMessage { get; set; }
            public string MessageOverhead { get; set; }
            public int Hue { get; set; }
        }

        public static List<OverheadMessage> OverheadMessageList = new List<OverheadMessage>();

        public static void Save(XmlTextWriter xml)
        {
            foreach (var message in OverheadMessageList)
            {
                xml.WriteStartElement("overheadmessage");
                xml.WriteAttributeString("searchtext", message.SearchMessage);
                xml.WriteAttributeString("message", (message.MessageOverhead));
                xml.WriteAttributeString("hue", Convert.ToString(message.Hue));
                xml.WriteEndElement();
            }
        }

        public static void Load(XmlElement node)
        {
            ClearAll();

            try
            {
                foreach (XmlElement el in node.GetElementsByTagName("overheadmessage"))
                {
                    OverheadMessage overheadMessage = new OverheadMessage
                    {
                        MessageOverhead = el.GetAttribute("message"),
                        SearchMessage = el.GetAttribute("searchtext"),
                        Hue = string.IsNullOrEmpty(el.GetAttribute("hue"))
                            ? 68
                            : Convert.ToInt32(el.GetAttribute("hue"))
                    };

                    OverheadMessageList.Add(overheadMessage);
                }
            }
            catch
            {
            }
        }

        public static void ClearAll()
        {
            OverheadMessageList.Clear();
        }
    }
}