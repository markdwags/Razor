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

using System.IO;

// FileIDs
//0 - map0.mul
//1 - staidx0.mul
//2 - statics0.mul
//3 - artidx.mul
//4 - art.mul
//5 - anim.idx
//6 - anim.mul
//7 - soundidx.mul
//8 - sound.mul
//9 - texidx.mul
//10 - texmaps.mul
//11 - gumpidx.mul
//12 - gumpart.mul
//13 - multi.idx
//14 - multi.mul
//15 - skills.idx
//16 - skills.mul
//30 - tiledata.mul
//31 - animdata.mul

namespace Ultima
{
    public sealed class Verdata
    {
        public static Stream Stream { get; private set; }
        public static Entry5D[] Patches { get; private set; }

        private static string path;

        static Verdata()
        {
            Initialize();
        }

        public static void Initialize()
        {
            path = Files.GetFilePath("verdata.mul");

            if (path == null)
            {
                Patches = new Entry5D[0];
                Stream = Stream.Null;
            }
            else
            {
                using (Stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (BinaryReader bin = new BinaryReader(Stream))
                    {
                        Patches = new Entry5D[bin.ReadInt32()];

                        for (int i = 0; i < Patches.Length; ++i)
                        {
                            Patches[i].file = bin.ReadInt32();
                            Patches[i].index = bin.ReadInt32();
                            Patches[i].lookup = bin.ReadInt32();
                            Patches[i].length = bin.ReadInt32();
                            Patches[i].extra = bin.ReadInt32();
                        }
                    }
                }

                Stream.Close();
            }
        }

        public static void Seek(int lookup)
        {
            if (Stream == null || !Stream.CanRead || !Stream.CanSeek)
            {
                if (path != null)
                    Stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            }

            Stream.Seek(lookup, SeekOrigin.Begin);
        }
    }

    public struct Entry5D
    {
        public int file;
        public int index;
        public int lookup;
        public int length;
        public int extra;
    }
}