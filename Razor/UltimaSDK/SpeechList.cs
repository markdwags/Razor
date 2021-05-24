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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Ultima
{
    public sealed class SpeechList
    {
        public static List<SpeechEntry> Entries { get; set; }

        private static byte[] m_Buffer = new byte[128];

        static SpeechList()
        {
            Initialize();
        }

        /// <summary>
        /// Loads speech.mul in <see cref="SpeechList.Entries"/>
        /// </summary>
        public static void Initialize()
        {
            string path = Files.GetFilePath("speech.mul");
            if (path == null)
            {
                Entries = new List<SpeechEntry>(0);
                return;
            }

            Entries = new List<SpeechEntry>();
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] buffer = new byte[fs.Length];
                unsafe
                {
                    int order = 0;
                    fs.Read(buffer, 0, buffer.Length);
                    fixed (byte* data = buffer)
                    {
                        byte* bindat = (byte*) data;
                        byte* bindatend = bindat + buffer.Length;

                        while (bindat != bindatend)
                        {
                            short id = (short) ((*bindat++ >> 8) | (*bindat++)); //Swapped Endian
                            short length = (short) ((*bindat++ >> 8) | (*bindat++));
                            if (length > 128)
                                length = 128;
                            for (int i = 0; i < length; ++i)
                                m_Buffer[i] = *bindat++;
                            string keyword = Encoding.UTF8.GetString(m_Buffer, 0, length);
                            Entries.Add(new SpeechEntry(id, keyword, order));
                            ++order;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves speech.mul to <see cref="FileName"/>
        /// </summary>
        /// <param name="FileName"></param>
        public static void SaveSpeechList(string FileName)
        {
            Entries.Sort(new OrderComparer());
            using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (BinaryWriter bin = new BinaryWriter(fs))
                {
                    foreach (SpeechEntry entry in Entries)
                    {
                        bin.Write(NativeMethods.SwapEndian(entry.ID));
                        byte[] utf8String = Encoding.UTF8.GetBytes(entry.KeyWord);
                        short length = (short) utf8String.Length;
                        bin.Write(NativeMethods.SwapEndian(length));
                        bin.Write(utf8String);
                    }
                }
            }
        }

        public static void ExportToCSV(string FileName)
        {
            using (StreamWriter Tex = new StreamWriter(new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite),
                System.Text.Encoding.Unicode))
            {
                Tex.WriteLine("Order;ID;KeyWord");
                foreach (SpeechEntry entry in Entries)
                {
                    Tex.WriteLine($"{entry.Order};{entry.ID};{entry.KeyWord}");
                }
            }
        }

        public static void ImportFromCSV(string FileName)
        {
            Entries = new List<SpeechEntry>(0);
            if (!File.Exists(FileName))
                return;
            using (StreamReader sr = new StreamReader(FileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if ((line = line.Trim()).Length == 0 || line.StartsWith("#"))
                        continue;
                    if ((line.Contains("Order")) && (line.Contains("KeyWord")))
                        continue;
                    try
                    {
                        string[] split = line.Split(';');
                        if (split.Length < 3)
                            continue;

                        int order = ConvertStringToInt(split[0]);
                        int id = ConvertStringToInt(split[1]);
                        string word = split[2];
                        word = word.Replace("\"", "");
                        Entries.Add(new SpeechEntry((short) id, word, order));
                    }
                    catch
                    {
                    }
                }
            }
        }

        public static int ConvertStringToInt(string text)
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

        #region SortComparer

        public class IDComparer : IComparer<SpeechEntry>
        {
            private bool m_desc;

            public IDComparer(bool desc)
            {
                m_desc = desc;
            }

            public int Compare(SpeechEntry objA, SpeechEntry objB)
            {
                if (objA.ID == objB.ID)
                    return 0;
                else if (m_desc)
                    return (objA.ID < objB.ID) ? 1 : -1;
                else
                    return (objA.ID < objB.ID) ? -1 : 1;
            }
        }

        public class KeyWordComparer : IComparer<SpeechEntry>
        {
            private bool m_desc;

            public KeyWordComparer(bool desc)
            {
                m_desc = desc;
            }

            public int Compare(SpeechEntry objA, SpeechEntry objB)
            {
                if (m_desc)
                    return String.Compare(objB.KeyWord, objA.KeyWord);
                else
                    return String.Compare(objA.KeyWord, objB.KeyWord);
            }
        }

        public class OrderComparer : IComparer<SpeechEntry>
        {
            public int Compare(SpeechEntry objA, SpeechEntry objB)
            {
                if (objA.Order == objB.Order)
                    return 0;
                else
                    return (objA.Order < objB.Order) ? -1 : 1;
            }
        }

        #endregion
    }

    public sealed class SpeechEntry
    {
        public short ID { get; set; }
        public string KeyWord { get; set; }

        [Browsable(false)] public int Order { get; private set; }

        public SpeechEntry(short id, string keyword, int order)
        {
            ID = id;
            KeyWord = keyword;
            Order = order;
        }
    }

    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SpeechMul
    {
        public short id;
        public short length;
        public byte[] keyword;
    }
}