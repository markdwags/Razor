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

using System.Collections;
using System.IO;

namespace Ultima
{
    public sealed class Animdata
    {
        private static int[] m_Header;
        private static byte[] m_Unknown;

        public static Hashtable AnimData { get; set; }

        static Animdata()
        {
            Initialize();
        }

        /// <summary>
        /// Reads animdata.mul and fills <see cref="AnimData"/>
        /// </summary>
        public static void Initialize()
        {
            AnimData = new Hashtable();
            string path = Files.GetFilePath("animdata.mul");
            if (path != null)
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (BinaryReader bin = new BinaryReader(fs))
                    {
                        unsafe
                        {
                            int id = 0;
                            int h = 0;
                            byte unk;
                            byte fcount;
                            byte finter;
                            byte fstart;
                            sbyte[] fdata;
                            m_Header = new int[bin.BaseStream.Length / (4 + 8 * (64 + 4))];
                            while (h < m_Header.Length /*bin.BaseStream.Length != bin.BaseStream.Position*/)
                            {
                                m_Header[h++] = bin.ReadInt32(); // chunk header
                                // Read 8 tiles
                                byte[] buffer = bin.ReadBytes(544);
                                fixed (byte* buf = buffer)
                                {
                                    byte* data = buf;
                                    for (int i = 0; i < 8; ++i, ++id)
                                    {
                                        fdata = new sbyte[64];
                                        for (int j = 0; j < 64; ++j)
                                            fdata[j] = (sbyte) *data++;
                                        unk = *data++;
                                        fcount = *data++;
                                        finter = *data++;
                                        fstart = *data++;
                                        if (fcount > 0)
                                            AnimData[id] = new Data(fdata, unk, fcount, finter, fstart);
                                    }
                                }
                            }

                            int remaining = (int) (bin.BaseStream.Length - bin.BaseStream.Position);
                            if (remaining > 0)
                                m_Unknown = bin.ReadBytes(remaining);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets Animation <see cref="Data"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Data GetAnimData(int id)
        {
            if (AnimData.Contains(id))
                return ((Data) AnimData[id]);
            else
                return null;
        }

        public static void Save(string path)
        {
            string FileName = Path.Combine(path, "animdata.mul");
            using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (BinaryWriter bin = new BinaryWriter(fs))
                {
                    int id = 0;
                    int h = 0;
                    while (id < m_Header.Length * 8)
                    {
                        bin.Write(m_Header[h++]);
                        for (int i = 0; i < 8; ++i, ++id)
                        {
                            Data data = GetAnimData(id);
                            for (int j = 0; j < 64; ++j)
                            {
                                if (data != null)
                                    bin.Write(data.FrameData[j]);
                                else
                                    bin.Write((sbyte) 0);
                            }

                            if (data != null)
                            {
                                bin.Write(data.Unknown);
                                bin.Write(data.FrameCount);
                                bin.Write(data.FrameInterval);
                                bin.Write(data.FrameStart);
                            }
                            else
                            {
                                bin.Write((byte) 0);
                                bin.Write((byte) 0);
                                bin.Write((byte) 0);
                                bin.Write((byte) 0);
                            }
                        }
                    }

                    if (m_Unknown != null)
                        bin.Write(m_Unknown);
                }
            }
        }

        public class Data
        {
            public sbyte[] FrameData { get; set; }
            public byte Unknown { get; private set; }
            public byte FrameCount { get; set; }
            public byte FrameInterval { get; set; }
            public byte FrameStart { get; set; }

            public Data(sbyte[] frame, byte unk, byte fcount, byte finter, byte fstart)
            {
                FrameData = frame;
                Unknown = unk;
                FrameCount = fcount;
                FrameInterval = finter;
                FrameStart = fstart;
            }
        }
    }
}