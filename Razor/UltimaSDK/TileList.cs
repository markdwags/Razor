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

using System.Collections.Generic;

namespace Assistant.UltimaSDK
{
    public sealed class HuedTileList
    {
        private List<HuedTile> m_Tiles;

        public HuedTileList()
        {
            m_Tiles = new List<HuedTile>();
        }

        public int Count
        {
            get { return m_Tiles.Count; }
        }

        public void Add(ushort id, short hue, sbyte z)
        {
            m_Tiles.Add(new HuedTile(id, hue, z));
        }

        public HuedTile[] ToArray()
        {
            HuedTile[] tiles = new HuedTile[Count];

            if (m_Tiles.Count > 0)
                m_Tiles.CopyTo(tiles);
            m_Tiles.Clear();

            return tiles;
        }
    }

    public sealed class TileList
    {
        private List<Tile> m_Tiles;

        public TileList()
        {
            m_Tiles = new List<Tile>();
        }

        public int Count
        {
            get { return m_Tiles.Count; }
        }

        public void Add(ushort id, sbyte z)
        {
            m_Tiles.Add(new Tile(id, z));
        }

        public void Add(ushort id, sbyte z, sbyte flag)
        {
            m_Tiles.Add(new Tile(id, z, flag));
        }

        public Tile[] ToArray()
        {
            Tile[] tiles = new Tile[Count];
            if (m_Tiles.Count > 0)
                m_Tiles.CopyTo(tiles);
            m_Tiles.Clear();

            return tiles;
        }

        public Tile Get(int i)
        {
            return m_Tiles[i];
        }
    }

    public sealed class MTileList
    {
        private List<MTile> m_Tiles;

        public MTileList()
        {
            m_Tiles = new List<MTile>();
        }

        public int Count
        {
            get { return m_Tiles.Count; }
        }

        public void Add(ushort id, sbyte z)
        {
            m_Tiles.Add(new MTile(id, z));
        }

        public void Add(ushort id, sbyte z, sbyte flag)
        {
            m_Tiles.Add(new MTile(id, z, flag));
        }

        public void Add(ushort id, sbyte z, sbyte flag, int unk1)
        {
            m_Tiles.Add(new MTile(id, z, flag, unk1));
        }

        public MTile[] ToArray()
        {
            MTile[] tiles = new MTile[Count];

            if (m_Tiles.Count > 0)
                m_Tiles.CopyTo(tiles);
            m_Tiles.Clear();

            return tiles;
        }

        public MTile Get(int i)
        {
            return m_Tiles[i];
        }

        public void Set(int i, ushort id, sbyte z)
        {
            if (i < Count)
                m_Tiles[i].Set(id, z);
        }

        public void Set(int i, ushort id, sbyte z, sbyte flag)
        {
            if (i < Count)
                m_Tiles[i].Set(id, z, flag);
        }

        public void Set(int i, ushort id, sbyte z, sbyte flag, int unk1)
        {
            if (i < Count)
                m_Tiles[i].Set(id, z, flag, unk1);
        }

        public void Remove(int i)
        {
            if (i < Count)
                m_Tiles.RemoveAt(i);
        }
    }
}