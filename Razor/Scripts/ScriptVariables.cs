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
        private static Dictionary<string, Serial> _variables = new Dictionary<string, Serial>();
        public static IEnumerable<KeyValuePair<string, Serial>> Variables => _variables;

        public static void Save(XmlTextWriter xml)
        {
            foreach (KeyValuePair<string, Serial> kv in _variables)
            {
                xml.WriteStartElement("scriptvariable");
                xml.WriteAttributeString("serial", kv.Value.ToString());
                xml.WriteAttributeString("name", kv.Key);
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
                    string name = el.GetAttribute("name");
                    Serial serial = Serial.Parse(el.GetAttribute("serial"));

                    RegisterVariable(name, serial);
                }
            }
            catch
            {
                // ignored
            }
        }

        public static void RegisterVariable(string name, Serial serial)
        {
            name = name.Trim();

            _variables[name] = serial;
            Interpreter.SetAlias(name, serial);
        }

        public static void UnregisterVariable(string name)
        {
            name = name.Trim();
            Interpreter.ClearAlias(name);
            _variables.Remove(name);
        }

        public static void ClearAll()
        {
            foreach (string key in new List<string>(_variables.Keys))
            {
                UnregisterVariable(key);
            }

            _variables.Clear();
        }

        public static Serial GetVariable(string name)
        {
            name = name.Trim();

            if (_variables.TryGetValue(name, out var val))
            {
                return val;
            }

            return Serial.MinusOne;
        }
    }
}