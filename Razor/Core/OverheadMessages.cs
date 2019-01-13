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
                        Hue = string.IsNullOrEmpty(el.GetAttribute("hue")) ? 68 : Convert.ToInt32(el.GetAttribute("hue"))
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
