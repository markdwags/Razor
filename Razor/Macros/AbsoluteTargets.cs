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
            public TargetInfo TargetInfo { get; set; }
            public string TargetVariableName { get; set; }

            public AbsoluteTarget(string targetVarName, TargetInfo t)
            {
                TargetInfo = t;
                TargetVariableName = targetVarName;
            }
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
            //object[] serialString = new object[]
            //{
            //    TargetVariableName, TargetVariableProfile, TargetInfo.Type, TargetInfo.Flags, TargetInfo.Serial.Value, TargetInfo.X, TargetInfo.Y,
            //    TargetInfo.Z, TargetInfo.Gfx
            //};

            foreach (AbsoluteTarget target in AbsoluteTargetList)
            {
                xml.WriteStartElement("absolutetarget");
                xml.WriteAttributeString("type", target.TargetInfo.Type.ToString());
                xml.WriteAttributeString("flags", target.TargetInfo.Flags.ToString());
                xml.WriteAttributeString("serial", target.TargetInfo.Serial.ToString());
                xml.WriteAttributeString("x", target.TargetInfo.X.ToString());
                xml.WriteAttributeString("y", target.TargetInfo.Y.ToString());
                xml.WriteAttributeString("z", target.TargetInfo.X.ToString());
                xml.WriteAttributeString("gfx", target.TargetInfo.Gfx.ToString());
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

                    AbsoluteTarget absoluteTarget = new AbsoluteTarget(el.GetAttribute("name"), target);
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
