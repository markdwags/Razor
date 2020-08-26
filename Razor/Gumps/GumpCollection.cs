/* Copyright (C) 2009 Matthew Geyer
 * 
 * This file is part of UO Machine.
 * 
 * UO Machine is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * UO Machine is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with UO Machine.  If not, see <http://www.gnu.org/licenses/>. */

using System.Collections.Concurrent;
using System.Linq;

namespace Assistant.Gumps
{
    public sealed class GumpCollection
    {
        private readonly ConcurrentDictionary<uint, Gump> _dictionary;

        public GumpCollection()
        {
            _dictionary = new ConcurrentDictionary<uint, Gump>();
        }

        public void Add(Gump gump)
        {
            bool result = _dictionary.AddOrUpdate(gump.ID, gump, (k, v) => gump) != null;

            if (result)
            {
                OnCollectionChanged();
            }
        }

        public bool Remove(uint id, int buttonId = 0, int[] switches = null, GumpTextEntry[] textEntries = null)
        {
            bool result = _dictionary.TryRemove(id, out Gump g);

            g?.OnResponse(buttonId, switches, textEntries);

            if (result)
            {
                OnCollectionChanged();
            }

            return result;
        }

        public bool GetGump(uint id, out Gump gump)
        {
            return _dictionary.TryGetValue(id, out gump);
        }

        public bool FindGump(int serial, out Gump gump)
        {
            gump = _dictionary.Values.FirstOrDefault(g => g.Serial == serial);

            return gump != null;
        }

        public bool GetGumps(out Gump[] gumps)
        {
            gumps = null;

            if (_dictionary.Values.Count == 0)
            {
                return false;
            }

            gumps = _dictionary.Values.ToArray();

            return gumps.Length > 0;
        }

        public void Clear()
        {
            int previousCount = _dictionary.Count;

            _dictionary.Clear();

            if (previousCount > 0)
            {
                OnCollectionChanged();
            }
        }

        private void OnCollectionChanged()
        {
        }
    }
}