using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Assistant.Macros
{
    public class AbsoluteTargets
    {
        public TargetInfo TargetInfo { get; set; }
        public string TargetVariableName { get; set; }
        public string TargetVariableProfile { get; set; }

        public class AbsoluteTarget
        {
            public byte Type { get; set; }
            public byte Flags { get; set; }
            public uint Serial { get; set; }
            public ushort X { get; set; }
            public ushort Y { get; set; }
            public ushort Z { get; set; }
            public ushort Gfx { get; set; }
            public string TargetVariableName { get; set; }    

            public TargetInfo TargetInfo { get; set; }
        }

        public static List<AbsoluteTarget> AbsoluteTargetList = new List<AbsoluteTarget>();

        //public AbsoluteTarget(string name, string profile, TargetInfo info)
        //{
        //    TargetInfo = new TargetInfo
        //    {
        //        Type = info.Type,
        //        Flags = info.Flags,
        //        Serial = info.Serial,
        //        X = info.X,
        //        Y = info.Y,
        //        Z = info.Z,
        //        Gfx = info.Gfx
        //    };

        //    TargetVariableName = name;
        //    TargetVariableProfile = profile;
        //}

        public static void Save(XmlTextWriter xml)
        {
            foreach (var target in AbsoluteTargetList)
            {
                xml.WriteStartElement("absolutetarget");
                xml.WriteAttributeString("type", target.Type.ToString());
                xml.WriteAttributeString("flags", target.Flags.ToString());
                xml.WriteAttributeString("serial", target.Serial.ToString());
                xml.WriteAttributeString("x", target.X.ToString());
                xml.WriteAttributeString("y", target.Y.ToString());
                xml.WriteAttributeString("z", target.X.ToString());
                xml.WriteAttributeString("gfx", target.Gfx.ToString());
                xml.WriteAttributeString("name", target.TargetVariableName);
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
                    AbsoluteTarget absoluteTarget = new AbsoluteTarget
                    {
                        Type = Convert.ToByte(el.GetAttribute("type")),
                        Flags = Convert.ToByte(el.GetAttribute("flags")),
                        Serial = Convert.ToUInt32(el.GetAttribute("serial")),
                        X = Convert.ToUInt16(el.GetAttribute("x")),
                        Y = Convert.ToUInt16(el.GetAttribute("y")),
                        Z = Convert.ToUInt16(el.GetAttribute("z")),
                        Gfx = Convert.ToUInt16(el.GetAttribute("gfx")),
                        TargetVariableName = el.GetAttribute("name")
                    };

                    AbsoluteTargetList.Add(absoluteTarget);
                }
            }
            catch
            {
            }
        }

        public static void ClearAll()
        {
            AbsoluteTargetList.Clear();
        }
    }
}
