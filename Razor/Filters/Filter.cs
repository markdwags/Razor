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

namespace Assistant.Filters
{
    public abstract class Filter
    {
        private static readonly List<Filter> _filters = new List<Filter>();

        public static IList<Filter> List
        {
            get { return _filters; }
        }

        public static void Register(Filter filter)
        {
            _filters.Add(filter);
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

                    for (int i = 0; i < _filters.Count; i++)
                    {
                        Filter f = (Filter) _filters[i];
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
            for (int i = 0; i < _filters.Count; i++)
                ((Filter) _filters[i]).OnDisable();
        }

        public static void Save(XmlTextWriter xml)
        {
            for (int i = 0; i < _filters.Count; i++)
            {
                Filter f = (Filter) _filters[i];
                if (f.Enabled)
                {
                    xml.WriteStartElement("filter");
                    xml.WriteAttributeString("name", ((int) f.Name).ToString());
                    xml.WriteAttributeString("enable", f.Enabled.ToString());
                    xml.WriteEndElement();
                }
            }
        }

        public abstract void OnFilter(PacketReader p, PacketHandlerEventArgs args);
        public abstract byte[] PacketIDs { get; }
        public abstract LocString Name { get; }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (value == _enabled)
                    return;

                if (value)
                    OnEnable();
                else
                    OnDisable();
            }
        }

        private bool _enabled;
        private PacketViewerCallback _callback;

        protected Filter()
        {
            _enabled = false;
            _callback = new PacketViewerCallback(this.OnFilter);
        }

        public override string ToString()
        {
            return Language.GetString(this.Name);
        }

        public virtual void OnEnable()
        {
            _enabled = true;
            for (int i = 0; i < PacketIDs.Length; i++)
                PacketHandler.RegisterServerToClientViewer(PacketIDs[i], _callback);
        }

        public virtual void OnDisable()
        {
            _enabled = false;
            for (int i = 0; i < PacketIDs.Length; i++)
                PacketHandler.RemoveServerToClientViewer(PacketIDs[i], _callback);
        }
    }
}