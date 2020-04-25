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
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assistant.Core
{
    public class EncodedSpeech
    {
        internal class SpeechEntry : IComparable<SpeechEntry>
        {
            internal short m_KeywordID;
            internal string[] m_Keywords;

            internal SpeechEntry(int idKeyword, string keyword)
            {
                m_KeywordID = (short) idKeyword;
                m_Keywords = keyword.Split(new char[] {'*'});
            }

            public int CompareTo(SpeechEntry entry)
            {
                if (entry == null)
                {
                    return -1;
                }

                if (entry != this)
                {
                    if (m_KeywordID < entry.m_KeywordID)
                    {
                        return -1;
                    }

                    if (m_KeywordID > entry.m_KeywordID)
                    {
                        return 1;
                    }
                }

                return 0;
            }
        }

        private static List<SpeechEntry> m_Speech;
        internal static unsafe void LoadSpeechTable()
        {
            string path = Ultima.Files.GetFilePath("Speech.mul");

            if (!File.Exists(path))
            {
                m_Speech = new List<SpeechEntry>();
            }
            else
            {
                m_Speech = new List<SpeechEntry>();
                byte[] buffer = new byte[0x400];
                fixed (byte* numRef = buffer)
                {
                    using(var file = new FileStream(path, FileMode.Open,FileAccess.Read)) 
                        while (file.Position < file.Length)
                        {
                            int id = (ushort)((file.ReadByte() << 8) | file.ReadByte());
                            int length = (ushort)((file.ReadByte() << 8) | file.ReadByte());
                            if (length > 0 && file.Position + length <= file.Length)
                            {
                                file.Read(buffer, 0, length);
                                m_Speech.Add(new SpeechEntry(id, new string((sbyte*) numRef,0, length)));
                            }
                        }
                }

            }
            
        }

        internal static List<ushort> GetKeywords(string text)
        {
            List<ushort> keynumber = new List<ushort>();

            if (m_Speech == null)
            {
                LoadSpeechTable();
            }

            text = text.ToLower();

            List<SpeechEntry> keywords = new List<SpeechEntry>();
            List<SpeechEntry> speech = m_Speech.ToList();
            foreach (SpeechEntry entry in speech)
            {
                if (IsMatch(text, entry.m_Keywords))
                {
                    keywords.Add(entry);
                }
            }

            keywords.Sort();

            bool flag = false;

            int numk = keywords.Count & 15;
            int index = 0;
            while (index < keywords.Count)
            {
                SpeechEntry entry = keywords[index];
                int keywordID = entry.m_KeywordID;

                if (flag)
                {
                    keynumber.Add((byte) (keywordID >> 4));
                    numk = keywordID & 15;
                }
                else
                {
                    keynumber.Add((byte) ((numk << 4) | ((keywordID >> 8) & 15)));
                    keynumber.Add((byte) keywordID);
                }

                index++;
                flag = !flag;
            }

            if (!flag)
            {
                keynumber.Add((byte) (numk << 4));
            }

            return keynumber;
        }

        private static bool IsMatch(string input, string[] split)
        {
            int startIndex = 0;

            for (int i = 0; i < split.Length; i++)
            {
                if (split[i].Length > 0)
                {
                    int index = input.IndexOf(split[i], startIndex);
                    if ((index > 0) && (i == 0))
                    {
                        return false;
                    }

                    if (index < 0)
                    {
                        return false;
                    }

                    startIndex = index + split[i].Length;
                }
            }

            return ((split[split.Length - 1].Length <= 0) || (startIndex == input.Length));
        }
    }
}