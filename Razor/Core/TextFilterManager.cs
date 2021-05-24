﻿#region license

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
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using Assistant.UI;

namespace Assistant.Core
{
    public static class TextFilterManager
    {
        private static ListBox _filterTextList;

        public static List<string> FilteredText = new List<string>();

        public static void SetControls(ListBox filterTextList)
        {
            _filterTextList = filterTextList;
        }

        public static void AddFilter(string filter)
        {
            FilteredText.Add(filter);

            RedrawList();
        }

        public static void RemoveFilter(string filter)
        {
            FilteredText.Remove(filter);

            RedrawList();
        }

        public static void Save(XmlTextWriter xml)
        {
            foreach (var text in FilteredText)
            {
                xml.WriteStartElement("filter");
                xml.WriteAttributeString("text", text);
                xml.WriteEndElement();
            }
        }

        public static bool IsFiltered(string text)
        {
            if (!Config.GetBool("EnableTextFilter"))
                return false;

            foreach (string filteredText in FilteredText)
            {
                if (text.IndexOf(filteredText, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    return true;
                }
            }

            return false;
        }

        public static void Load(XmlElement node)
        {
            ClearAll();

            try
            {
                foreach (XmlElement el in node.GetElementsByTagName("filter"))
                {
                    FilteredText.Add(Convert.ToString(el.GetAttribute("text")));
                }

                RedrawList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static void ClearAll()
        {
            FilteredText.Clear();
        }

        public static void RedrawList()
        {
            _filterTextList?.SafeAction(s =>
            {
                s.BeginUpdate();
                s.Items.Clear();

                foreach (string text in FilteredText)
                {
                    s.Items.Add(text);
                }

                s.EndUpdate();
            });
        }
    }
}