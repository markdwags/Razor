﻿#region license

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
    public sealed class RadarCol
    {
        static RadarCol()
        {
            Initialize();
        }

        private static short[] m_Colors;

        public static short[] Colors
        {
            get { return m_Colors; }
        }

        public static short GetItemColor(int index)
        {
            if (index + 0x4000 < m_Colors.Length)
                return m_Colors[index + 0x4000];
            return 0;
        }

        public static short GetLandColor(int index)
        {
            if (index < m_Colors.Length)
                return m_Colors[index];
            return 0;
        }

        public static void SetItemColor(int index, short value)
        {
            m_Colors[index + 0x4000] = value;
        }

        public static void SetLandColor(int index, short value)
        {
            m_Colors[index] = value;
        }

        public static void Initialize()
        {
            string path = Files.GetFilePath("radarcol.mul");
            if (path != null)
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    m_Colors = new short[fs.Length / 2];
                    GCHandle gc = GCHandle.Alloc(m_Colors, GCHandleType.Pinned);
                    byte[] buffer = new byte[(int) fs.Length];
                    fs.Read(buffer, 0, (int) fs.Length);
                    Marshal.Copy(buffer, 0, gc.AddrOfPinnedObject(), (int) fs.Length);
                    gc.Free();
                }
            }
            else
                m_Colors = new short[0x8000];
        }

        public static void Save(string FileName)
        {
            using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (BinaryWriter bin = new BinaryWriter(fs))
                {
                    for (int i = 0; i < m_Colors.Length; ++i)
                    {
                        bin.Write(m_Colors[i]);
                    }
                }
            }
        }

        public static void ExportToCSV(string FileName)
        {
            using (StreamWriter Tex = new StreamWriter(new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite),
                System.Text.Encoding.GetEncoding(1252)))
            {
                Tex.WriteLine("ID;Color");

                for (int i = 0; i < m_Colors.Length; ++i)
                {
                    Tex.WriteLine($"0x{i:X4};{m_Colors[i]}");
                }
            }
        }

        public static void ImportFromCSV(string FileName)
        {
            if (!File.Exists(FileName))
                return;
            using (StreamReader sr = new StreamReader(FileName))
            {
                string line;
                int count = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if ((line = line.Trim()).Length == 0 || line.StartsWith("#"))
                        continue;
                    if (line.StartsWith("ID;"))
                        continue;
                    ++count;
                }

                m_Colors = new short[count];
            }

            using (StreamReader sr = new StreamReader(FileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if ((line = line.Trim()).Length == 0 || line.StartsWith("#"))
                        continue;
                    if (line.StartsWith("ID;"))
                        continue;
                    try
                    {
                        string[] split = line.Split(';');
                        if (split.Length < 2)
                            continue;

                        int id = ConvertStringToInt(split[0]);
                        int color = ConvertStringToInt(split[1]);
                        m_Colors[id] = (short) color;
                    }
                    catch
                    {
                    }
                }
            }
        }

        private static int ConvertStringToInt(string text)
        {
            int result;
            if (text.Contains("0x"))
            {
                string convert = text.Replace("0x", "");
                int.TryParse(convert, System.Globalization.NumberStyles.HexNumber, null, out result);
            }
            else
                int.TryParse(text, System.Globalization.NumberStyles.Integer, null, out result);

            return result;
        }
    }
}