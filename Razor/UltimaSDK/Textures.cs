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

using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace Ultima
{
    public sealed class Textures
    {
        private static FileIndex m_FileIndex = new FileIndex("Texidx.mul", "Texmaps.mul", 0x4000, 10);
        private static Bitmap[] m_Cache = new Bitmap[0x4000];
        private static bool[] m_Removed = new bool[0x4000];
        private static Hashtable m_patched = new Hashtable();

        private static byte[] m_StreamBuffer;

        struct CheckSums
        {
            public byte[] checksum;
            public int pos;
            public int length;
            public int index;
        }

        private static List<CheckSums> checksums;

        /// <summary>
        /// ReReads texmaps
        /// </summary>
        public static void Reload()
        {
            m_FileIndex = new FileIndex("Texidx.mul", "Texmaps.mul", 0x4000, 10);
            m_Cache = new Bitmap[0x4000];
            m_Removed = new bool[0x4000];
            m_patched.Clear();
        }

        public static int GetIdxLength()
        {
            return (int) (m_FileIndex.IdxLength / 12);
        }

        /// <summary>
        /// Removes Texture <see cref="m_Removed"/>
        /// </summary>
        /// <param name="index"></param>
        public static void Remove(int index)
        {
            m_Removed[index] = true;
        }

        /// <summary>
        /// Replaces Texture
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bmp"></param>
        public static void Replace(int index, Bitmap bmp)
        {
            m_Cache[index] = bmp;
            m_Removed[index] = false;
            if (m_patched.Contains(index))
                m_patched.Remove(index);
        }

        /// <summary>
        /// Tests if index is valid Texture
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool TestTexture(int index)
        {
            int length, extra;
            bool patched;
            if (m_Removed[index])
                return false;
            if (m_Cache[index] != null)
                return true;
            bool valid = m_FileIndex.Valid(index, out length, out extra, out patched);
            if ((!valid) || (length == 0))
                return false;
            return true;
        }

        /// <summary>
        /// Returns Bitmap of Texture
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public unsafe static Bitmap GetTexture(int index)
        {
            bool patched;
            return GetTexture(index, out patched);
        }

        /// <summary>
        /// Returns Bitmap of Texture with verdata bool
        /// </summary>
        /// <param name="index"></param>
        /// <param name="patched"></param>
        /// <returns></returns>
        public unsafe static Bitmap GetTexture(int index, out bool patched)
        {
            if (m_patched.Contains(index))
                patched = (bool) m_patched[index];
            else
                patched = false;
            if (m_Removed[index])
                return null;
            if (m_Cache[index] != null)
                return m_Cache[index];

            int length, extra;
            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);
            if (stream == null)
                return null;
            if (length == 0)
                return null;
            if (patched)
                m_patched[index] = true;

            int size = extra == 0 ? 64 : 128;

            Bitmap bmp = new Bitmap(size, size, PixelFormat.Format16bppArgb1555);
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, size, size), ImageLockMode.WriteOnly,
                PixelFormat.Format16bppArgb1555);

            ushort* line = (ushort*) bd.Scan0;
            int delta = bd.Stride >> 1;

            int max = size * size * 2;

            if (m_StreamBuffer == null || m_StreamBuffer.Length < max)
                m_StreamBuffer = new byte[max];
            stream.Read(m_StreamBuffer, 0, max);

            fixed (byte* data = m_StreamBuffer)
            {
                ushort* bindat = (ushort*) data;
                for (int y = 0; y < size; ++y, line += delta)
                {
                    ushort* cur = line;
                    ushort* end = cur + size;

                    while (cur < end)
                        *cur++ = (ushort) (*bindat++ ^ 0x8000);
                }
            }

            bmp.UnlockBits(bd);

            stream.Close();
            if (!Files.CacheData)
                return m_Cache[index] = bmp;
            else
                return bmp;
        }

        public unsafe static void Save(string path)
        {
            string idx = Path.Combine(path, "texidx.mul");
            string mul = Path.Combine(path, "texmaps.mul");
            checksums = new List<CheckSums>();
            using (FileStream fsidx = new FileStream(idx, FileMode.Create, FileAccess.Write, FileShare.Write),
                fsmul = new FileStream(mul, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                MemoryStream memidx = new MemoryStream();
                MemoryStream memmul = new MemoryStream();
                using (BinaryWriter binidx = new BinaryWriter(memidx),
                    binmul = new BinaryWriter(memmul))
                {
                    SHA256Managed sha = new SHA256Managed();
                    //StreamWriter Tex = new StreamWriter(new FileStream("d:/texlog.txt", FileMode.Create, FileAccess.ReadWrite));
                    for (int index = 0; index < GetIdxLength(); ++index)
                    {
                        if (m_Cache[index] == null)
                            m_Cache[index] = GetTexture(index);

                        Bitmap bmp = m_Cache[index];
                        if ((bmp == null) || (m_Removed[index]))
                        {
                            binidx.Write((int) -1); // lookup
                            binidx.Write((int) 0); // length
                            binidx.Write((int) -1); // extra
                        }
                        else
                        {
                            MemoryStream ms = new MemoryStream();
                            bmp.Save(ms, ImageFormat.Bmp);
                            byte[] checksum = sha.ComputeHash(ms.ToArray());
                            CheckSums sum;
                            if (compareSaveImages(checksum, out sum))
                            {
                                binidx.Write((int) sum.pos); //lookup
                                binidx.Write((int) sum.length);
                                binidx.Write((int) 0);
                                //Tex.WriteLine(System.String.Format("0x{0:X4} : 0x{1:X4} 0x{2:X4}", index, (int)sum.pos, (int)sum.length));
                                //Tex.WriteLine(System.String.Format("0x{0:X4} -> 0x{1:X4}", sum.index, index));
                                continue;
                            }

                            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
                            ushort* line = (ushort*) bd.Scan0;
                            int delta = bd.Stride >> 1;

                            binidx.Write((int) binmul.BaseStream.Position); //lookup
                            int length = (int) binmul.BaseStream.Position;

                            for (int Y = 0; Y < bmp.Height; ++Y, line += delta)
                            {
                                ushort* cur = line;
                                for (int X = 0; X < bmp.Width; ++X)
                                {
                                    binmul.Write((ushort) (cur[X] ^ 0x8000));
                                }
                            }

                            int start = length;
                            length = (int) binmul.BaseStream.Position - length;
                            binidx.Write(length);
                            binidx.Write((int) (bmp.Width == 64 ? 0 : 1));
                            bmp.UnlockBits(bd);
                            CheckSums s = new CheckSums()
                                {pos = start, length = length, checksum = checksum, index = index};
                            //Tex.WriteLine(System.String.Format("0x{0:X4} : 0x{1:X4} 0x{2:X4}", index, start, length));
                            checksums.Add(s);
                        }
                    }

                    memidx.WriteTo(fsidx);
                    memmul.WriteTo(fsmul);
                }
            }
        }

        private static bool compareSaveImages(byte[] newchecksum, out CheckSums sum)
        {
            sum = new CheckSums();
            for (int i = 0; i < checksums.Count; ++i)
            {
                byte[] cmp = checksums[i].checksum;
                if (((cmp == null) || (newchecksum == null))
                    || (cmp.Length != newchecksum.Length))
                {
                    return false;
                }

                bool valid = true;
                for (int j = 0; j < cmp.Length; ++j)
                {
                    if (cmp[j] != newchecksum[j])
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    sum = checksums[i];
                    return true;
                }
            }

            return false;
        }
    }
}