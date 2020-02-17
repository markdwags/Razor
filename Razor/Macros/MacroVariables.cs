#region license

// Razor: An Ultima Online Assistant
// Copyright (C) 2020 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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

namespace Assistant.Macros
{
    public class MacroVariables
    {
        public class MacroVariable
        {
            public TargetInfo TargetInfo { get; set; }
            public string Name { get; set; }

            public bool TargetWasSet { get; set; }

            public MacroVariable(string targetVarName, TargetInfo t)
            {
                TargetInfo = t;
                Name = targetVarName;
                TargetWasSet = true;
            }

            public void TargetSetMacroVariable()
            {
                if (World.Player != null)
                {
                    TargetWasSet = false;

                    Targeting.OneTimeTarget(OnMacroVariableTarget);
                    World.Player.SendMessage(MsgLevel.Force, $"Select target for ${Name}");

                    //OneTimeTarget(false, new Targeting.TargetResponseCallback(OnMacroVariableTarget), new Targeting.CancelTargetCallback(OnSLTCancel));
                }
            }

            private void OnMacroVariableTarget(bool ground, Serial serial, Point3D pt, ushort gfx)
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

                foreach (MacroVariables.MacroVariable mV in MacroVariables.MacroVariableList
                )
                {
                    if (mV.Name.ToLower().Equals(Name.ToLower()))
                    {
                        foundVar = true;
                        // macro exists, update
                        mV.TargetInfo = t;

                        World.Player.SendMessage(MsgLevel.Force, $"'{mV.Name}' macro variable updated to '{t.Serial}'");

                        break;
                    }
                }

                if (!foundVar)
                {
                    //MacroVariables.MacroVariableList.Add(new MacroVariables.MacroVariable(_lastMacroVariable, t));
                    //World.Player.SendMessage(MsgLevel.Force, $"'{_lastMacroVariable}' not found, created variable and set to '{t.Serial}'");
                }

                // Save and reload the macros and vars
                Engine.MainWindow.SaveMacroVariables();

                TargetWasSet = true;
            }

            /*
            private void OnSLTCancel()
            {
                if (m_LastTarget != null)
                    m_LTWasSet = true;
            }*/
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
            catch
            {
                // ignored
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
            catch
            {
                // ignored
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
            catch
            {
                // ignored
            }
        }

        public static void ClearAll()
        {
            MacroVariableList.Clear();
        }
    }
}