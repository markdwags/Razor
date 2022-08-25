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