using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Assistant.Macros
{
    public class MacroVariables
    {
        public class MacroVariable
        {
            public TargetInfo TargetInfo { get; set; }
            public string Name { get; set; }

            public MacroVariable(string targetVarName, TargetInfo t)
            {
                TargetInfo = t;
                Name = targetVarName;
            }
        }

        public static List<MacroVariable> MacroVariableList = new List<MacroVariable>();

        public static void Save(XmlTextWriter xml)
        {
            foreach (MacroVariable target in MacroVariableList)
            {
                xml.WriteStartElement("macrovariable");
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
                foreach (XmlElement el in node.GetElementsByTagName("macrovariable"))
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

                    MacroVariable macroVariable = new MacroVariable(el.GetAttribute("name"), target);
                    MacroVariableList.Add(macroVariable);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void Import(XmlElement node)
        {
            //import absolutetargets and doubleclickvariables to macrovariables

            try
            {
                foreach (XmlElement el in node["absolutetargets"].GetElementsByTagName("absolutetarget"))
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

                    MacroVariable macroVariable = new MacroVariable(el.GetAttribute("name"), target);
                    MacroVariableList.Add(macroVariable);
                }
            }
            catch (Exception ex)
            {
            }
            try
            {
                foreach (XmlElement el in node["doubleclickvariables"].GetElementsByTagName("doubleclickvariable"))
                {
                    TargetInfo target = new TargetInfo
                    {
                        Type = 0,
                        Flags = 0,
                        Serial = Convert.ToUInt32(Serial.Parse(el.GetAttribute("serial"))),
                        X = 0,
                        Y = 0,
                        Z = 0,
                        Gfx = Convert.ToUInt16(el.GetAttribute("gfx"))
                    };

                    MacroVariable macroVariable = new MacroVariable(el.GetAttribute("name"), target);
                    MacroVariableList.Add(macroVariable);
                }
            }
            catch (Exception ex)
            {
            }

        }

        public static void ClearAll()
        {
            MacroVariableList.Clear();
        }
    }
}