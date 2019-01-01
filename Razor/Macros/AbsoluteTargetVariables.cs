using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Assistant.Macros
{
    public class AbsoluteTargetVariables
    {
        public class AbsoluteTargetVariable
        {
            public TargetInfo TargetInfo { get; set; }
            public string Name { get; set; }

            public AbsoluteTargetVariable(string targetVarName, TargetInfo t)
            {
                TargetInfo = t;
                Name = targetVarName;
            }
        }

        public static List<AbsoluteTargetVariable> AbsoluteTargetList = new List<AbsoluteTargetVariable>();

        public static void Save(XmlTextWriter xml)
        {
            foreach (AbsoluteTargetVariable target in AbsoluteTargetList)
            {
                xml.WriteStartElement("absolutetarget");
                xml.WriteAttributeString("type", target.TargetInfo.Type.ToString());
                xml.WriteAttributeString("flags", target.TargetInfo.Flags.ToString());
                xml.WriteAttributeString("serial", target.TargetInfo.Serial.ToString());
                xml.WriteAttributeString("x", target.TargetInfo.X.ToString());
                xml.WriteAttributeString("y", target.TargetInfo.Y.ToString());
                xml.WriteAttributeString("z", target.TargetInfo.X.ToString());
                xml.WriteAttributeString("gfx", target.TargetInfo.Gfx.ToString());
                xml.WriteAttributeString("name", target.Name);
                xml.WriteEndElement();
            }
        }

        public static void Load(XmlElement node)
        {
            ClearAll();

            try
            {
                foreach (XmlElement el in node.GetElementsByTagName("absolutetarget"))
                {
                    TargetInfo target = new TargetInfo
                    {
                        Type = Convert.ToByte(el.GetAttribute("type")),
                        Flags = Convert.ToByte(el.GetAttribute("flags")),
                        Serial = Convert.ToUInt32(Serial.Parse(el.GetAttribute("serial"))),
                        X = Convert.ToUInt16(el.GetAttribute("x")),
                        Y = Convert.ToUInt16(el.GetAttribute("y")),
                        Z = Convert.ToUInt16(el.GetAttribute("z")),
                        Gfx = Convert.ToUInt16(el.GetAttribute("gfx"))
                    };

                    AbsoluteTargetVariable absoluteTarget = new AbsoluteTargetVariable(el.GetAttribute("name"), target);
                    AbsoluteTargetList.Add(absoluteTarget);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void ClearAll()
        {
            AbsoluteTargetList.Clear();
        }
    }
}