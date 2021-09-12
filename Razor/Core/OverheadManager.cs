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
    public class OverheadMessage
    {
        public string SearchMessage { get; set; }
        public string MessageOverhead { get; set; }
        public int Hue { get; set; }
    }

    public static class OverheadManager
    {
        private static readonly List<OverheadMessage> _overheadMessages = new List<OverheadMessage>();
        public static IReadOnlyList<OverheadMessage> OverheadMessages
        {
            get { return _overheadMessages; }
        }

        public static void Save(XmlTextWriter xml)
        {
            foreach (var message in OverheadMessages)
            {
                xml.WriteStartElement("overheadmessage");
                xml.WriteAttributeString("searchtext", message.SearchMessage);
                xml.WriteAttributeString("message", (message.MessageOverhead));
                xml.WriteAttributeString("hue", Convert.ToString(message.Hue));
                xml.WriteEndElement();
            }
        }

        public static void Load(XmlElement node)
        {
            ClearAll();

            try
            {
                foreach (XmlElement el in node.GetElementsByTagName("overheadmessage"))
                {
                    OverheadMessage overheadMessage = new OverheadMessage
                    {
                        MessageOverhead = el.GetAttribute("message"),
                        SearchMessage = el.GetAttribute("searchtext"),
                        Hue = string.IsNullOrEmpty(el.GetAttribute("hue"))
                            ? 68
                            : Convert.ToInt32(el.GetAttribute("hue"))
                    };

                    _overheadMessages.Add(overheadMessage);
                }
            }
            catch
            {
                // ignored
            }
        }

        public static void ClearAll()
        {
            _overheadMessages.Clear();
        }

        public static void Remove(string text)
        {
            foreach (OverheadMessage message in OverheadMessages)
            {
                if (message.SearchMessage.Equals(text))
                {
                    _overheadMessages.Remove(message);
                    break;
                }
            }
        }

        public static void DisplayOverheadMessage(string text)
        {
            if (Config.GetBool("ShowOverheadMessages") && OverheadMessages.Count > 0)
            {
                string overheadFormat = Config.GetString("OverheadFormat");

                foreach (OverheadMessage message in OverheadMessages)
                {
                    if (text.IndexOf(message.SearchMessage, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        string ohMessage = overheadFormat.Replace("{msg}", message.MessageOverhead);
                        string[] splitText = text.Split(' ');

                        if (splitText.Length > 0)
                        {
                            for (int wordNum = 1; wordNum < splitText.Length + 1; wordNum++)
                            {
                                ohMessage = ohMessage.Replace($"{{{wordNum}}}", splitText[wordNum - 1]);
                            }
                        }

                        World.Player.OverheadMessage(message.Hue, ohMessage);
                        break;
                    }
                }
            }
        }

        public static void SetMessageHue(string text, int hue)
        {
            foreach (var message in OverheadMessages)
            {
                if (message.SearchMessage.Equals(text))
                {
                    message.Hue = hue;
                    break;
                }
            }
        }

        public static void AddOverheadMessage(OverheadMessage message)
        {
            _overheadMessages.Add(message);
        }

        public static void ReplaceOverheadMessage(string oldMessage, string newMessage)
        {
            foreach (var message in OverheadMessages)
            {
                if (message.MessageOverhead.Equals(oldMessage))
                {
                    message.MessageOverhead = newMessage;
                    break;
                }
            }
        }

        public static int GetHue(string id)
        {
            int hue = 0;

            foreach (OverheadMessage list in OverheadManager.OverheadMessages)
            {
                if (list.MessageOverhead.Equals(id))
                {
                    return list.Hue;
                }
            }

            return hue;
        }
    }
}