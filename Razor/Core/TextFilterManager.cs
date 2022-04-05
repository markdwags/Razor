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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Assistant.UI;

namespace Assistant.Core
{
    public class TextFilterEntryModel
    {
        public string Text { get; set; }

        public bool FilterSysMessages { get; set; }

        public bool FilterOverhead { get; set; }

        public bool FilterSpeech { get; set; }

        public bool IgnoreFilteredMessageInScripts { get; set; }

        public TextFilterEntryModel()
        {
        }

        public TextFilterEntryModel(XmlElement element)
        {
            Text = element.GetAttribute("text");
            FilterSpeech = ConvertToBool(element.GetAttribute("speech"), true);
            FilterOverhead = ConvertToBool(element.GetAttribute("overhead"));
            FilterSysMessages = ConvertToBool(element.GetAttribute("sysmessages"));
            IgnoreFilteredMessageInScripts = ConvertToBool(element.GetAttribute("ignoreinscripts"), true);
        }

        private bool ConvertToBool(string attributeString, bool defaultValue = false)
        {
            if (string.IsNullOrWhiteSpace(attributeString))
                return defaultValue;

            return Convert.ToBoolean(attributeString);
        }
    }

    public static class TextFilterManager
    {
        private static ListBox _filterTextList;

        public static List<TextFilterEntryModel> FilteredText = new List<TextFilterEntryModel>();

        public static void SetControls(ListBox filterTextList)
        {
            filterTextList.DataSource = FilteredText;
            filterTextList.DisplayMember = nameof(TextFilterEntryModel.Text);
            _filterTextList = filterTextList;
        }

        public static void AddFilter(TextFilterEntryModel entry)
        {
            FilteredText.Add(entry);

            RedrawList();
        }

        public static void RemoveFilter(int index)
        {
            FilteredText.RemoveAt(index);

            RedrawList();
        }

        public static void UpdateFilter(TextFilterEntryModel entry, int index)
        {
            FilteredText[index] = entry;

            RedrawList();
        }

        public static void Save(XmlTextWriter xml)
        {
            foreach (var entry in FilteredText)
            {
                xml.WriteStartElement("filter");
                xml.WriteAttributeString("text", entry.Text);
                xml.WriteAttributeString("sysmessages", entry.FilterSysMessages.ToString());
                xml.WriteAttributeString("overhead", entry.FilterOverhead.ToString());
                xml.WriteAttributeString("speech", entry.FilterSpeech.ToString());
                xml.WriteAttributeString("ignoreinscripts", entry.IgnoreFilteredMessageInScripts.ToString());
                xml.WriteEndElement();
            }
        }

        public static bool IsFiltered(string text)
        {
            if (!Config.GetBool("EnableTextFilter"))
                return false;

            foreach (var entry in FilteredText)
            {
                if (text.IndexOf(entry.Text, StringComparison.OrdinalIgnoreCase) != -1)
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
                foreach (var entry in node.ChildNodes.Cast<XmlElement>().Select(el => new TextFilterEntryModel(el)))
                {
                    FilteredText.Add(entry);
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
            _filterTextList?.SafeAction((lb) =>
            {
                lb.BeginUpdate();
                lb.DataSource = null;
                lb.DataSource = FilteredText;
                lb.DisplayMember = nameof(TextFilterEntryModel.Text);
                lb.EndUpdate();
            });
        }
    }
}
