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
using System.Text;

namespace Ultima
{
    public sealed class SkillGroup
    {
        public string Name { get; set; }

        public SkillGroup(string name)
        {
            Name = name;
        }
    }

    public sealed class SkillGroups
    {
        public static List<SkillGroup> List { get; private set; }
        public static List<int> SkillList { get; private set; }
        private static bool unicode = false;

        static SkillGroups()
        {
            Initialize();
        }

        public static void Initialize()
        {
            string path = Files.GetFilePath("skillgrp.mul");

            List = new List<SkillGroup>();
            SkillList = new List<int>();

            if (path != null)
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (BinaryReader bin = new BinaryReader(fs))
                    {
                        int start = 4;
                        int strlen = 17;
                        int count = bin.ReadInt32();
                        if (count == -1)
                        {
                            unicode = true;
                            count = bin.ReadInt32();
                            start *= 2;
                            strlen *= 2;
                        }

                        List.Add(new SkillGroup("Misc"));
                        for (int i = 0; i < count - 1; ++i)
                        {
                            int strbuild;
                            fs.Seek((long) (start + (i * strlen)), SeekOrigin.Begin);
                            StringBuilder builder2 = new StringBuilder(17);
                            if (unicode)
                            {
                                while ((strbuild = bin.ReadInt16()) != 0)
                                    builder2.Append((char) strbuild);
                            }
                            else
                            {
                                while ((strbuild = bin.ReadByte()) != 0)
                                    builder2.Append((char) strbuild);
                            }

                            List.Add(new SkillGroup(builder2.ToString()));
                        }

                        fs.Seek((long) (start + ((count - 1) * strlen)), SeekOrigin.Begin);
                        try
                        {
                            while (bin.BaseStream.Length != bin.BaseStream.Position)
                                SkillList.Add(bin.ReadInt32());
                        }
                        catch // just for safety
                        {
                        }
                    }
                }
            }
        }

        public static void Save(string path)
        {
            string mul = Path.Combine(path, "skillgrp.mul");
            using (FileStream fs = new FileStream(mul, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (BinaryWriter bin = new BinaryWriter(fs))
                {
                    if (unicode)
                        bin.Write((int) -1);
                    bin.Write((int) List.Count);

                    foreach (SkillGroup group in List)
                    {
                        if (group.Name == "Misc")
                            continue;
                        byte[] name;
                        if (unicode)
                            name = new byte[34];
                        else
                            name = new byte[17];
                        if (group.Name != null)
                        {
                            if (unicode)
                            {
                                byte[] bb = Encoding.Unicode.GetBytes(group.Name);
                                if (bb.Length > 34)
                                    Array.Resize(ref bb, 34);
                                bb.CopyTo(name, 0);
                            }
                            else
                            {
                                byte[] bb = Encoding.Default.GetBytes(group.Name);
                                if (bb.Length > 17)
                                    Array.Resize(ref bb, 17);
                                bb.CopyTo(name, 0);
                            }
                        }

                        bin.Write(name);
                    }

                    foreach (int group in SkillList)
                        bin.Write(group);
                }
            }
        }
    }
}