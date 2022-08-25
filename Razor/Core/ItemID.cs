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

namespace Assistant
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
                return $"{Ultima.TileData.ItemTable[m_ID].Name} ({m_ID:X4})";
            }
            catch
            {
                return $" ({m_ID:X4})";
            }
        }

        public Ultima.ItemData ItemData
        {
            get
            {
                try
                {
                    return Ultima.TileData.ItemTable[m_ID];
                }
                catch
                {
                    return new Ultima.ItemData("", Ultima.TileFlag.None, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
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