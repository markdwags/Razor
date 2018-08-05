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
        }

        public static List<OverheadMessage> OverheadMessageList = new List<OverheadMessage>();

        public static void Save(XmlTextWriter xml)
        {
            foreach (var message in OverheadMessageList)
            {
                xml.WriteStartElement("overheadmessage");
                xml.WriteAttributeString("searchtext", message.SearchMessage);
                xml.WriteAttributeString("message", (message.MessageOverhead));
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
                        SearchMessage = el.GetAttribute("searchtext")
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
