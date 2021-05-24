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
using Ultima;

namespace Assistant
{
    public class MultiTileEntry
    {
        public ushort m_ItemID;
        public short m_OffsetX;
        public short m_OffsetY;
        public short m_OffsetZ;
    }

    public class Map
    {
        public static Ultima.Map GetMap(int mapNum)
        {
            switch (mapNum)
            {
                case 1: return Ultima.Map.Trammel;
                case 2: return Ultima.Map.Ilshenar;
                case 3: return Ultima.Map.Malas;
                case 4: return Ultima.Map.Tokuno;
                case 5: return Ultima.Map.TerMur;
                case 0:
                default: return Ultima.Map.Felucca;
            }
        }

        public static int Parse(string name)
        {
            if (string.IsNullOrEmpty(name))
                return 0;

            name = name.ToLower();

            if (name == "felucca")
                return 0;
            else if (name == "trammel")
                return 1;
            else if (name == "ilshenar")
                return 2;
            else if (name == "malas")
                return 3;
            else if (name == "samurai" || name == "tokuno")
                return 4;
            else
                return 0;
        }

        public static HuedTile GetTileNear(int mapNum, int x, int y, int z)
        {
            try
            {
                Ultima.Map map = GetMap(mapNum);

                HuedTile[] tiles = map.Tiles.GetStaticTiles(x, y);
                if (tiles != null && tiles.Length > 0)
                {
                    for (int i = 0; i < tiles.Length; i++)
                    {
                        if (tiles[i].Z >= z - 5 && tiles[i].Z <= z + 5)
                            return tiles[i];
                    }
                }
            }
            catch
            {
            }

            return new HuedTile(0, 0, (sbyte) z);
        }

        private static void GetAverageZ(Ultima.Map map, int x, int y, ref int z, ref int avg, ref int top)
        {
            try
            {
                int zTop = map.Tiles.GetLandTile(x, y).Z;
                int zLeft = map.Tiles.GetLandTile(x, y + 1).Z;
                int zRight = map.Tiles.GetLandTile(x + 1, y).Z;
                int zBottom = map.Tiles.GetLandTile(x + 1, y + 1).Z;

                z = zTop;
                if (zLeft < z)
                    z = zLeft;
                if (zRight < z)
                    z = zRight;
                if (zBottom < z)
                    z = zBottom;

                top = zTop;
                if (zLeft > top)
                    top = zLeft;
                if (zRight > top)
                    top = zRight;
                if (zBottom > top)
                    top = zBottom;

                if (Math.Abs(zTop - zBottom) > Math.Abs(zLeft - zRight))
                    avg = (int) Math.Floor((zLeft + zRight) / 2.0);
                else
                    avg = (int) Math.Floor((zTop + zBottom) / 2.0);
            }
            catch
            {
            }
        }

        public static sbyte ZTop(int mapNum, int xCheck, int yCheck, int oldZ)
        {
            try
            {
                Ultima.Map map = GetMap(mapNum);

                Tile landTile = map.Tiles.GetLandTile(xCheck, yCheck);
                int landZ = 0, landCenter = 0, zTop = 0;

                GetAverageZ(map, xCheck, yCheck, ref landZ, ref landCenter, ref zTop);

                if (zTop > oldZ)
                    oldZ = zTop;

                bool isSet = false;
                HuedTile[] staticTiles = map.Tiles.GetStaticTiles(xCheck, yCheck);
                for (int i = 0; i < staticTiles.Length; ++i)
                {
                    HuedTile tile = staticTiles[i];
                    ItemData id = TileData.ItemTable[tile.ID & 0x3FFF];

                    int calcTop = (tile.Z + id.CalcHeight);

                    if (calcTop <= oldZ + 5 && (!isSet || calcTop > zTop) &&
                        ((id.Flags & TileFlag.Surface) != 0 || (id.Flags & TileFlag.Wet) != 0))
                    {
                        zTop = calcTop;
                        isSet = true;
                    }
                }

                return (sbyte) zTop;
            }
            catch
            {
                return (sbyte) oldZ;
            }
        }
    }
}