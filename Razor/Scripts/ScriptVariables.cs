using System;
using System.Collections.Generic;
using System.Xml;
using Assistant.Scripts.Engine;

namespace Assistant.Scripts
{
    public class ScriptVariables
    {
        public class ScriptVariable
        {
            public TargetInfo TargetInfo { get; set; }
            public string Name { get; set; }

            public ScriptVariable(string targetVarName, TargetInfo t)
            {
                TargetInfo = t;
                Name = targetVarName;
            }
        }

        public static List<ScriptVariable> ScriptVariableList = new List<ScriptVariable>();

        public static void Save(XmlTextWriter xml)
        {
            foreach (ScriptVariable target in ScriptVariableList)
            {
                xml.WriteStartElement("scriptvariable");
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
                foreach (XmlElement el in node.GetElementsByTagName("scriptvariable"))
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

                    ScriptVariable scriptVariable = new ScriptVariable(el.GetAttribute("name"), target);
                    ScriptVariableList.Add(scriptVariable);

                    RegisterVariable(scriptVariable.Name);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void RegisterVariable(string name)
        {
            Interpreter.RegisterAliasHandler(name, ScriptVariableHandler);
        }

        public static void UnregisterVariable(string name)
        {
            Interpreter.UnregisterAliasHandler(name);
        }

        private static uint ScriptVariableHandler(string alias)
        {
            foreach (ScriptVariable scriptVariable in ScriptVariableList)
            {
                if (scriptVariable.Name.Equals(alias))
                {
                    return scriptVariable.TargetInfo.Serial;
                }
            }

            return 0;
        }

        public static void ClearAll()
        {
            foreach (ScriptVariable scriptVariable in ScriptVariableList)
            {
                Interpreter.UnregisterAliasHandler(scriptVariable.Name);
            }

            ScriptVariableList.Clear();
        }
    }
}