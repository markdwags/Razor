using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Assistant.Macros
{
    public class DoubleClickVariables
    {
        public class DoubleClickVariable
        {
            public Serial Serial { get; set; }
            public ushort Gfx { get; set; }
            public string Name { get; set; }

            public DoubleClickVariable(string targetVarName, Serial serial, ushort gfx)
            {
                Serial = serial;
                Gfx = gfx;
                Name = targetVarName;
            }
        }

        public static List<DoubleClickVariable> DoubleClickTargetList = new List<DoubleClickVariable>();

        public static void Save(XmlTextWriter xml)
        {
            foreach (DoubleClickVariable target in DoubleClickTargetList)
            {
                xml.WriteStartElement("doubleclickvariable");
                xml.WriteAttributeString("serial", target.Serial.ToString());
                xml.WriteAttributeString("gfx", target.Gfx.ToString());
                xml.WriteAttributeString("name", target.Name);
                xml.WriteEndElement();
            }
        }

        public static void Load(XmlElement node)
        {
            ClearAll();

            try
            {

                foreach (XmlElement el in node.GetElementsByTagName("doubleclickvariable"))
                {
                    ushort gfx = Convert.ToUInt16(el.GetAttribute("gfx"));
                    Serial serial = Convert.ToUInt32(Serial.Parse(el.GetAttribute("serial")));

                    DoubleClickVariable doubleClickVariable = new DoubleClickVariable(el.GetAttribute("name"), serial, gfx);

                    DoubleClickTargetList.Add(doubleClickVariable);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void ClearAll()
        {
            DoubleClickTargetList.Clear();
        }
    }
}
