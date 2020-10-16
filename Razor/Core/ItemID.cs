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
using Assistant.UltimaSDK;

namespace Assistant.Core
{
    public struct ItemID
    {
        private ushort m_ID;

        public ItemID(ushort id)
        {
            m_ID = id;
        }

        public ushort Value
        {
            get { return m_ID; }
        }

        public static implicit operator ushort(ItemID a)
        {
            return a.m_ID;
        }

        public static implicit operator ItemID(ushort a)
        {
            return new ItemID(a);
        }

        public override string ToString()
        {
            try
            {
                return string.Format("{0} ({1:X4})", TileData.ItemTable[m_ID].Name, m_ID);
            }
            catch
            {
                return String.Format(" ({0:X4})", m_ID);
            }
        }

        public ItemData ItemData
        {
            get
            {
                try
                {
                    return TileData.ItemTable[m_ID];
                }
                catch
                {
                    return new ItemData("", TileFlag.None, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                }
            }
        }

        public override int GetHashCode()
        {
            return m_ID;
        }

        public override bool Equals(object o)
        {
            if (o == null || !(o is ItemID)) return false;

            return ((ItemID) o).m_ID == m_ID;
        }

        public static bool operator ==(ItemID l, ItemID r)
        {
            return l.m_ID == r.m_ID;
        }

        public static bool operator !=(ItemID l, ItemID r)
        {
            return l.m_ID != r.m_ID;
        }

        public static bool operator >(ItemID l, ItemID r)
        {
            return l.m_ID > r.m_ID;
        }

        public static bool operator >=(ItemID l, ItemID r)
        {
            return l.m_ID >= r.m_ID;
        }

        public static bool operator <(ItemID l, ItemID r)
        {
            return l.m_ID < r.m_ID;
        }

        public static bool operator <=(ItemID l, ItemID r)
        {
            return l.m_ID <= r.m_ID;
        }
    }
}