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
using System.Collections.Generic;
using System.IO;
using System.Text;
using Assistant;

namespace Ultima
{
    public sealed class Skills
    {
        private static FileIndex _fileIndex = new FileIndex("skills.idx", "skills.mul", 16);
        public static Dictionary<string, SkillInfo> SkillsByName { get; private set; }
        public static Dictionary<int, SkillInfo> SkillsByIndex { get; private set; }

        public static void Initialize()
        {
            Reload();
        }
        
        public Skills()
        {
        }

        /// <summary>
        /// ReReads skills.mul
        /// </summary>
        private static void Reload()
        {
            _fileIndex = new FileIndex("skills.idx", "skills.mul", 16);

            SkillsByIndex = new Dictionary<int, SkillInfo>();
            SkillsByName = new Dictionary<string, SkillInfo>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < _fileIndex.Index.Length; ++i)
            {
                SkillInfo info = GetSkill(i);

                if (info == null)
                    break;

                SkillsByIndex.Add(i, info);
                SkillsByName.Add(info.Name.Replace(" ", string.Empty), info);
            }
        }

        public static List<int> GetUsableSkillIndexes()
        {
            if (SkillsByIndex == null)
            {
                Reload();
            }

            List<int> indexes = new List<int>();

            foreach (KeyValuePair<int, SkillInfo> keyValuePair in SkillsByIndex)
            {
                if (keyValuePair.Value.IsAction)
                {
                    indexes.Add(keyValuePair.Key);
                }
            }

            return indexes;
        }

        public static List<string> GetUsableSkillNames()
        {
            if (SkillsByName == null)
            {
                Reload();
            }

            List<string> names = new List<string>();

            foreach (KeyValuePair<string, SkillInfo> keyValuePair in SkillsByName)
            {
                if (keyValuePair.Value.IsAction)
                {
                    names.Add(keyValuePair.Key);
                }
            }

            return names;
        }

        public static string GetSkillDisplayName(int index)
        {
            if (SkillsByIndex.TryGetValue(index, out SkillInfo skill))
            {
                return skill.DisplayName;
            }

            return string.Empty;
        }

        /// <summary>
        /// Returns <see cref="SkillInfo"/> of index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private static SkillInfo GetSkill(int index)
        {
            Stream stream = _fileIndex.Seek(index, out int length, out int extra, out bool patched);
            
            if (stream != null)
            {
                if (length == 0)
                    return null;

                using (BinaryReader bin = new BinaryReader(stream))
                {
                    bool action = bin.ReadBoolean();
                    string name = ReadNameString(bin, length - 1);
                    string displayName = Language.GetSkillCliloc(index);
                    return new SkillInfo(index, name, displayName, action, extra);
                }
            }

            return null;
        }

        private static readonly byte[] StringBuffer = new byte[1024];

        private static string ReadNameString(BinaryReader bin, int length)
        {
            bin.Read(StringBuffer, 0, length);
            int count;
            for (count = 0; count < length && StringBuffer[count] != 0; ++count) ;

            return Encoding.Default.GetString(StringBuffer, 0, count);
        }

        public static void Save(string path)
        {
            string idx = Path.Combine(path, "skills.idx");
            string mul = Path.Combine(path, "skills.mul");

            using (FileStream fsSkillsIndex = new FileStream(idx, FileMode.Create, FileAccess.Write, FileShare.Write), fsSkillsMul = new FileStream(mul, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (BinaryWriter binSkillsIndex = new BinaryWriter(fsSkillsIndex), binSkillsMul = new BinaryWriter(fsSkillsMul))
                {
                    for (int i = 0; i < _fileIndex.Index.Length; ++i)
                    {
                        if (SkillsByIndex.TryGetValue(i, out SkillInfo skill))
                        {
                            binSkillsIndex.Write((int)fsSkillsMul.Position); //lookup
                            int length = (int)fsSkillsMul.Position;
                            binSkillsMul.Write(skill.IsAction);

                            byte[] namebytes = Encoding.Default.GetBytes(skill.Name);
                            binSkillsMul.Write(namebytes);
                            binSkillsMul.Write((byte)0); //nullterminated

                            length = (int)fsSkillsMul.Position - length;
                            binSkillsIndex.Write(length);
                            binSkillsIndex.Write(skill.Extra);
                        }
                        else
                        {
                            binSkillsIndex.Write((int)-1); // lookup
                            binSkillsIndex.Write((int)0); // length
                            binSkillsIndex.Write((int)0); // extra
                        }
                    }
                }
            }
        }
    }

    public sealed class SkillInfo
    {
        public int Index { get; set; }
        public bool IsAction { get; set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }

        public int Extra { get; private set; }


        public SkillInfo(int nr, string name, string displayName, bool action, int extra)
        {
            Index = nr;
            Name = name;
            DisplayName = displayName;
            IsAction = action;
            Extra = extra;
        }
    }
}