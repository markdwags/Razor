#region license
// Razor: An Ultima Online Assistant
// Copyright (c) 2022 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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
            public bool TargetWasSet { get; set; }
            
            public ScriptVariable(string targetVarName, TargetInfo t)
            {
                TargetInfo = t;
                Name = targetVarName;
            }

            public void SetTarget()
            {
                if (World.Player != null)
                {
                    TargetWasSet = false;

                    Targeting.OneTimeTarget(OnScriptVariableTarget);
                    World.Player.SendMessage(MsgLevel.Force, $"Select target for variable '{Name}'");

                    //OneTimeTarget(false, new Targeting.TargetResponseCallback(OnMacroVariableTarget), new Targeting.CancelTargetCallback(OnSLTCancel));
                }
            }

            private void OnScriptVariableTarget(bool ground, Serial serial, Point3D pt, ushort gfx)
            {
                TargetInfo t = new TargetInfo
                {
                    Gfx = gfx,
                    Serial = serial,
                    Type = (byte) (ground ? 1 : 0),
                    X = pt.X,
                    Y = pt.Y,
                    Z = pt.Z
                };

                bool foundVar = false;

                foreach (ScriptVariable sV in ScriptVariableList
                )
                {
                    if (sV.Name.Equals(Name, StringComparison.OrdinalIgnoreCase))
                    {
                        foundVar = true;
                        sV.TargetInfo = t;

                        World.Player.SendMessage(MsgLevel.Force,
                            $"'{sV.Name}' script variable updated to '{t.Serial}'");

                        break;
                    }
                }

                // Save and reload the vars
                if (foundVar)
                    Assistant.Engine.MainWindow.SaveScriptVariables();

                TargetWasSet = true;
            }
        }

        public static List<ScriptVariable> ScriptVariableList { get; set; } = new List<ScriptVariable>();

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
            catch
            {
                // ignored
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

        public static ScriptVariable GetVariable(string name)
        {
            foreach (ScriptVariable scriptVariable in ScriptVariableList)
            {
                if (scriptVariable.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return scriptVariable;
                }
            }

            return null;
        }
    }
}