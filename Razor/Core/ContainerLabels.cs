#region license

// Razor: An Ultima Online Assistant
// Copyright (C) 2021 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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

            MessageManager.OnLabelMessage += HandleLabelMessage;
        }

        private static void HandleLabelMessage(Packet p, PacketHandlerEventArgs args, Serial source, ushort graphic,
                         MessageType type, ushort hue, ushort font, string lang, string sourceName,
                         string text)
        {
            if (!Config.GetBool("ShowContainerLabels"))
                return;

            if (!source.IsItem)
                return;

            Item item = World.FindItem(source);

            if (item == null)
                return;

            if (!item.IsContainer)
                return;

            foreach (var label in ContainerLabelList)
            {
                // Check if its the serial match and if the text matches the name (since we override that for the label)
                if (Serial.Parse(label.Id) == source &&
                    (item.ItemID.ItemData.Name.Equals(text) ||
                        label.Alias.Equals(text, StringComparison.InvariantCultureIgnoreCase)))
                {
                    string labelDisplay =
                        $"{Config.GetString("ContainerLabelFormat").Replace("{label}", label.Label).Replace("{type}", text)}";

                    //ContainerLabelStyle
                    if (Config.GetInt("ContainerLabelStyle") == 0)
                    {
                        Client.Instance.SendToClient(new AsciiMessage(source, item.ItemID.Value, MessageType.Label,
                            label.Hue, 3, Language.CliLocName, labelDisplay));
                    }
                    else
                    {
                        Client.Instance.SendToClient(new UnicodeMessage(source, item.ItemID.Value,
                            MessageType.Label, label.Hue, 3, Language.CliLocName, "", labelDisplay));
                    }

                    // block the actual message from coming through since we have it in the label
                    args.Block = true;

                    LastContainerLabelDisplayed = source;
                    break;
                }
            }
        }

        public static void ClearAll()
        {
            ContainerLabelList.Clear();
        }
    }
}