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
using System.Collections;
using System.Windows.Forms;
using System.Xml;
using Assistant.Network;
using Assistant.UI;

namespace Assistant.Filters
{
    public abstract class Filter
    {
        private static ArrayList m_Filters = new ArrayList();

        public static ArrayList List
        {
            get { return m_Filters; }
        }

        public static void Register(Filter filter)
        {
            m_Filters.Add(filter);
        }

        public static void Load(XmlElement xml)
        {
            DisableAll();

            if (xml == null)
                return;

            foreach (XmlElement el in xml.GetElementsByTagName("filter"))
            {
                try
                {
                    LocString name = (LocString) Convert.ToInt32(el.GetAttribute("name"));
                    string enable = el.GetAttribute("enable");

                    for (int i = 0; i < m_Filters.Count; i++)
                    {
                        Filter f = (Filter) m_Filters[i];
                        if (f.Name == name)
                        {
                            if (Convert.ToBoolean(enable))
                                f.OnEnable();
                            break;
                        }
                    }
                }
                catch
                {
                }
            }
        }

        public static void DisableAll()
        {
            for (int i = 0; i < m_Filters.Count; i++)
                ((Filter) m_Filters[i]).OnDisable();
        }

        public static void Save(XmlTextWriter xml)
        {
            for (int i = 0; i < m_Filters.Count; i++)
            {
                Filter f = (Filter) m_Filters[i];
                if (f.Enabled)
                {
                    xml.WriteStartElement("filter");
                    xml.WriteAttributeString("name", ((int) f.Name).ToString());
                    xml.WriteAttributeString("enable", f.Enabled.ToString());
                    xml.WriteEndElement();
                }
            }
        }

        public static void Draw(CheckedListBox list)
        {
            list.BeginUpdate();
            list.Items.Clear();

            for (int i = 0; i < m_Filters.Count; i++)
            {
                Filter f = (Filter) m_Filters[i];
                list.Items.Add(f);
                list.SetItemChecked(i, f.Enabled);
            }

            list.EndUpdate();
        }

        public abstract void OnFilter(PacketReader p, PacketHandlerEventArgs args);
        public abstract byte[] PacketIDs { get; }
        public abstract LocString Name { get; }

        public bool Enabled
        {
            get { return m_Enabled; }
        }

        private bool m_Enabled;
        private PacketViewerCallback m_Callback;

        protected Filter()
        {
            m_Enabled = false;
            m_Callback = new PacketViewerCallback(this.OnFilter);
        }

        public override string ToString()
        {
            return Language.GetString(this.Name);
        }

        public virtual void OnEnable()
        {
            m_Enabled = true;
            for (int i = 0; i < PacketIDs.Length; i++)
                PacketHandler.RegisterServerToClientViewer(PacketIDs[i], m_Callback);
        }

        public virtual void OnDisable()
        {
            m_Enabled = false;
            for (int i = 0; i < PacketIDs.Length; i++)
                PacketHandler.RemoveServerToClientViewer(PacketIDs[i], m_Callback);
        }

        public void OnCheckChanged(CheckState newValue)
        {
            if (Enabled && newValue == CheckState.Unchecked)
                OnDisable();
            else if (!Enabled && newValue == CheckState.Checked)
                OnEnable();
        }
    }
}