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
using System.IO;
using System.Runtime.InteropServices;

namespace Ultima
{
    public sealed class TileMatrixPatch
    {
        public int LandBlocksCount { get; private set; }
        public int StaticBlocksCount { get; private set; }

        public Tile[][][] LandBlocks { get; private set; }
        public HuedTile[][][][][] StaticBlocks { get; private set; }

        private int BlockWidth;
        private int BlockHeight;

        private static byte[] m_Buffer;
        private static StaticTile[] m_TileBuffer = new StaticTile[128];

        public bool IsLandBlockPatched(int x, int y)
        {
            if (x < 0 || y < 0 || x >= BlockWidth || y >= BlockHeight)
                return false;
            if (LandBlocks[x] == null)
                return false;
            if (LandBlocks[x][y] == null)
                return false;
            return true;
        }

        public Tile[] GetLandBlock(int x, int y)
        {
            if (x < 0 || y < 0 || x >= BlockWidth || y >= BlockHeight)
                return TileMatrix.InvalidLandBlock;
            if (LandBlocks[x] == null)
                return TileMatrix.InvalidLandBlock;
            return LandBlocks[x][y];
        }

        public Tile GetLandTile(int x, int y)
        {
            return GetLandBlock(x >> 3, y >> 3)[((y & 0x7) << 3) + (x & 0x7)];
        }

        public bool IsStaticBlockPatched(int x, int y)
        {
            if (x < 0 || y < 0 || x >= BlockWidth || y >= BlockHeight)
                return false;
            if (StaticBlocks[x] == null)
                return false;
            if (StaticBlocks[x][y] == null)
                return false;
            return true;
        }

        public HuedTile[][][] GetStaticBlock(int x, int y)
        {
            if (x < 0 || y < 0 || x >= BlockWidth || y >= BlockHeight)
                return TileMatrix.EmptyStaticBlock;
            if (StaticBlocks[x] == null)
                return TileMatrix.EmptyStaticBlock;
            return StaticBlocks[x][y];
        }

        public HuedTile[] GetStaticTiles(int x, int y)
        {
            return GetStaticBlock(x >> 3, y >> 3)[x & 0x7][y & 0x7];
        }

        public TileMatrixPatch(TileMatrix matrix, int index, string path)
        {
            BlockWidth = matrix.BlockWidth;
            BlockHeight = matrix.BlockWidth;

            LandBlocksCount = StaticBlocksCount = 0;
            string mapDataPath, mapIndexPath;
            if (path == null)
            {
                mapDataPath = Files.GetFilePath("mapdif{0}.mul", index);
                mapIndexPath = Files.GetFilePath("mapdifl{0}.mul", index);
            }
            else
            {
                mapDataPath = Path.Combine(path, $"mapdif{index}.mul");
                if (!File.Exists(mapDataPath))
                    mapDataPath = null;
                mapIndexPath = Path.Combine(path, $"mapdifl{index}.mul");
                if (!File.Exists(mapIndexPath))
                    mapIndexPath = null;
            }

            if (mapDataPath != null && mapIndexPath != null)
            {
                LandBlocks = new Tile[matrix.BlockWidth][][];
                LandBlocksCount = PatchLand(matrix, mapDataPath, mapIndexPath);
            }

            string staDataPath, staIndexPath, staLookupPath;
            if (path == null)
            {
                staDataPath = Files.GetFilePath("stadif{0}.mul", index);
                staIndexPath = Files.GetFilePath("stadifl{0}.mul", index);
                staLookupPath = Files.GetFilePath("stadifi{0}.mul", index);
            }
            else
            {
                staDataPath = Path.Combine(path, $"stadif{index}.mul");
                if (!File.Exists(staDataPath))
                    staDataPath = null;
                staIndexPath = Path.Combine(path, $"stadifl{index}.mul");
                if (!File.Exists(staIndexPath))
                    staIndexPath = null;
                staLookupPath = Path.Combine(path, $"stadifi{index}.mul");
                if (!File.Exists(staLookupPath))
                    staLookupPath = null;
            }

            if (staDataPath != null && staIndexPath != null && staLookupPath != null)
            {
                StaticBlocks = new HuedTile[matrix.BlockWidth][][][][];
                StaticBlocksCount = PatchStatics(matrix, staDataPath, staIndexPath, staLookupPath);
            }
        }

        private unsafe int PatchLand(TileMatrix matrix, string dataPath, string indexPath)
        {
            using (FileStream fsData = new FileStream(dataPath, FileMode.Open, FileAccess.Read, FileShare.Read),
                fsIndex = new FileStream(indexPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (BinaryReader indexReader = new BinaryReader(fsIndex))
                {
                    int count = (int) (indexReader.BaseStream.Length / 4);

                    for (int i = 0; i < count; ++i)
                    {
                        int blockID = indexReader.ReadInt32();
                        int x = blockID / matrix.BlockHeight;
                        int y = blockID % matrix.BlockHeight;

                        fsData.Seek(4, SeekOrigin.Current);

                        Tile[] tiles = new Tile[64];

                        GCHandle gc = GCHandle.Alloc(tiles, GCHandleType.Pinned);
                        try
                        {
                            if (m_Buffer == null || m_Buffer.Length < 192)
                                m_Buffer = new byte[192];

                            fsData.Read(m_Buffer, 0, 192);

                            Marshal.Copy(m_Buffer, 0, gc.AddrOfPinnedObject(), 192);
                        }
                        finally
                        {
                            gc.Free();
                        }

                        if (LandBlocks[x] == null)
                            LandBlocks[x] = new Tile[matrix.BlockHeight][];
                        LandBlocks[x][y] = tiles;
                    }

                    return count;
                }
            }
        }

        private unsafe int PatchStatics(TileMatrix matrix, string dataPath, string indexPath, string lookupPath)
        {
            using (FileStream fsData = new FileStream(dataPath, FileMode.Open, FileAccess.Read, FileShare.Read),
                fsIndex = new FileStream(indexPath, FileMode.Open, FileAccess.Read, FileShare.Read),
                fsLookup = new FileStream(lookupPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (BinaryReader indexReader = new BinaryReader(fsIndex),
                    lookupReader = new BinaryReader(fsLookup))
                {
                    int count = Math.Min((int) (indexReader.BaseStream.Length / 4),
                        (int) (lookupReader.BaseStream.Length / 12));

                    HuedTileList[][] lists = new HuedTileList[8][];

                    for (int x = 0; x < 8; ++x)
                    {
                        lists[x] = new HuedTileList[8];

                        for (int y = 0; y < 8; ++y)
                            lists[x][y] = new HuedTileList();
                    }

                    for (int i = 0; i < count; ++i)
                    {
                        int blockID = indexReader.ReadInt32();
                        int blockX = blockID / matrix.BlockHeight;
                        int blockY = blockID % matrix.BlockHeight;

                        int offset = lookupReader.ReadInt32();
                        int length = lookupReader.ReadInt32();
                        lookupReader.ReadInt32(); // Extra

                        if (offset < 0 || length <= 0)
                        {
                            if (StaticBlocks[blockX] == null)
                                StaticBlocks[blockX] = new HuedTile[matrix.BlockHeight][][][];

                            StaticBlocks[blockX][blockY] = TileMatrix.EmptyStaticBlock;
                            continue;
                        }

                        fsData.Seek(offset, SeekOrigin.Begin);

                        int tileCount = length / 7;

                        if (m_TileBuffer.Length < tileCount)
                            m_TileBuffer = new StaticTile[tileCount];

                        StaticTile[] staTiles = m_TileBuffer;

                        GCHandle gc = GCHandle.Alloc(staTiles, GCHandleType.Pinned);
                        try
                        {
                            if (m_Buffer == null || m_Buffer.Length < length)
                                m_Buffer = new byte[length];

                            fsData.Read(m_Buffer, 0, length);

                            Marshal.Copy(m_Buffer, 0, gc.AddrOfPinnedObject(), length);

                            for (int j = 0; j < tileCount; ++j)
                            {
                                StaticTile cur = staTiles[j];
                                lists[cur.m_X & 0x7][cur.m_Y & 0x7]
                                    .Add(Art.GetLegalItemID(cur.m_ID), cur.m_Hue, cur.m_Z);
                            }

                            HuedTile[][][] tiles = new HuedTile[8][][];

                            for (int x = 0; x < 8; ++x)
                            {
                                tiles[x] = new HuedTile[8][];

                                for (int y = 0; y < 8; ++y)
                                    tiles[x][y] = lists[x][y].ToArray();
                            }

                            if (StaticBlocks[blockX] == null)
                                StaticBlocks[blockX] = new HuedTile[matrix.BlockHeight][][][];

                            StaticBlocks[blockX][blockY] = tiles;
                        }
                        finally
                        {
                            gc.Free();
                        }
                    }

                    return count;
                }
            }
        }
    }
}