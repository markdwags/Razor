using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Assistant.Core
{
    public class ContainerLabels
    {
        public class ContainerLabel
        {
            public string Id { get; set; }
            public string Type { get; set; }
            public string Label { get; set; }
            public int Hue { get; set; }
            public string Alias { get; set; }
        }

        public static Serial LastContainerLabelDisplayed;

        public static List<ContainerLabel> ContainerLabelList = new List<ContainerLabel>();

        public static void Save(XmlTextWriter xml)
        {
            foreach (var label in ContainerLabelList)
            {
                xml.WriteStartElement("containerlabel");
                xml.WriteAttributeString("id", label.Id);
                xml.WriteAttributeString("type", label.Type);
                xml.WriteAttributeString("label", label.Label);
                xml.WriteAttributeString("hue", label.Hue.ToString());
                xml.WriteAttributeString("alias", label.Alias);
                xml.WriteEndElement();
            }
        }

        public static void Load(XmlElement node)
        {
            ClearAll();

            try
            {

                foreach (XmlElement el in node.GetElementsByTagName("containerlabel"))
                {
                    ContainerLabel label = new ContainerLabel
                    {
                        Id = el.GetAttribute("id"),
                        Type = el.GetAttribute("type"),
                        Label = el.GetAttribute("label"),
                        Hue = Convert.ToInt32(el.GetAttribute("hue")),
                        Alias = el.GetAttribute("alias")
                    };

                    ContainerLabelList.Add(label);
                }
            }
            catch
            {
            }
        }

        public static void ClearAll()
        {
            ContainerLabelList.Clear();
        }
    }
}
