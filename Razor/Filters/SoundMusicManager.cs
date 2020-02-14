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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using Assistant.Core;
using Assistant.UI;

namespace Assistant.Filters
{
    public static class SoundMusicManager
    {
        private static CheckedListBox _soundFilterList;
        private static ComboBox _playableMusicList;

        private static List<Sound> SoundFilters = new List<Sound>();
        private static List<Sound> SoundList = new List<Sound>();

        private static List<Music> MusicList = new List<Music>();

        public static void SetControls(CheckedListBox soundFilterList, ComboBox playableMusicList)
        {
            _soundFilterList = soundFilterList;
            _playableMusicList = playableMusicList;
        }

        public class Sound
        {
            public string Name { get; set; }
            public Serial Serial { get; set; }

            public override string ToString()
            {
                return $"{Name} ({Serial})";
            }
        }

        public class Music
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public bool Loop { get; set; }

            public override string ToString()
            {
                return $"{Name} ({Id})";
            }
        }

        public static bool IsFilteredSound(Serial serial, out string name)
        {
            foreach (Sound filter in SoundFilters)
            {
                if (filter.Serial == serial)
                {
                    name = filter.Name;
                    return true;
                }
            }

            name = string.Empty;
            return false;
        }

        public static bool IsFilteredSound(ushort serial, out string name)
        {
            foreach (Sound filter in SoundFilters)
            {
                if (filter.Serial == serial)
                {
                    name = filter.Name;
                    return true;
                }
            }

            name = string.Empty;
            return false;
        }

        public static string GetSoundName(ushort soundId)
        {
            if (SoundList.Count == 0)
                RedrawList();

            foreach (Sound sound in SoundList)
            {
                if (sound.Serial == soundId)
                {
                    return sound.Name;
                }
            }

            return "(unknown)";
        }

        public static string GetMusicName(int musicId, out bool loop)
        {
            if (MusicList.Count == 0)
                RedrawList();

            foreach (Music music in MusicList)
            {
                if (music.Id == musicId)
                {
                    loop = music.Loop;
                    return music.Name;
                }
            }

            loop = false;
            return "(unknown)";
        }

        public static void AddSoundFilter(Sound filter)
        {
            if (!SoundFilters.Contains(filter))
                SoundFilters.Add(filter);
        }

        public static void RemoveSoundFilter(Sound filter)
        {
            if (SoundFilters.Contains(filter))
                SoundFilters.Remove(filter);
        }

        public static void LoadMusic()
        {
            MusicList.Clear();

            if (File.Exists($"{Client.Instance.GetUoFilePath()}\\Music\\Digital\\Config.txt"))
            {
                string[] musicInfo =
                    File.ReadAllLines($"{Client.Instance.GetUoFilePath()}\\Music\\Digital\\Config.txt");

                foreach (string music in musicInfo)
                {
                    string[] parsedMusic = music.Split(' ');

                    MusicList.Add(new Music
                    {
                        Id = Convert.ToInt32(parsedMusic[0]),
                        Name = parsedMusic[1].Split(',')[0],
                        Loop = parsedMusic[1].Contains(",") && parsedMusic[1].Split(',')[1].Equals("loop")
                    });
                }
            }
            else
            {
                MusicList.Add(new Music
                {
                    Id = 0,
                    Name = "oldult01",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 1,
                    Name = "create1",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 2,
                    Name = "dragflit",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 3,
                    Name = "oldult02",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 4,
                    Name = "oldult03",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 5,
                    Name = "oldult04",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 6,
                    Name = "oldult05",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 7,
                    Name = "oldult06",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 8,
                    Name = "stones2",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 9,
                    Name = "britain1",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 10,
                    Name = "britain2",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 11,
                    Name = "bucsden",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 12,
                    Name = "jhelom",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 13,
                    Name = "lbcastle",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 14,
                    Name = "linelle",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 15,
                    Name = "magincia",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 16,
                    Name = "minoc",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 17,
                    Name = "ocllo",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 18,
                    Name = "samlethe",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 19,
                    Name = "serpents",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 20,
                    Name = "skarabra",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 21,
                    Name = "trinsic",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 22,
                    Name = "vesper",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 23,
                    Name = "wind",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 24,
                    Name = "yew",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 25,
                    Name = "cave01",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 26,
                    Name = "dungeon9",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 27,
                    Name = "forest_a",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 28,
                    Name = "intown01",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 29,
                    Name = "jungle_a",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 30,
                    Name = "mountn_a",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 31,
                    Name = "plains_a",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 32,
                    Name = "sailing",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 33,
                    Name = "swamp_a",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 34,
                    Name = "tavern01",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 35,
                    Name = "tavern02",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 36,
                    Name = "tavern03",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 37,
                    Name = "tavern04",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 38,
                    Name = "combat1",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 39,
                    Name = "combat2",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 40,
                    Name = "combat3",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 41,
                    Name = "approach",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 42,
                    Name = "death",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 43,
                    Name = "victory",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 44,
                    Name = "btcastle",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 45,
                    Name = "nujelm",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 46,
                    Name = "dungeon2",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 47,
                    Name = "cove",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 48,
                    Name = "moonglow",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 49,
                    Name = "zento",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 50,
                    Name = "tokunodungeon",
                    Loop = false
                });


                MusicList.Add(new Music
                {
                    Id = 51,
                    Name = "Taiko",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 52,
                    Name = "dreadhornarea",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 53,
                    Name = "elfcity",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 54,
                    Name = "grizzledungeon",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 55,
                    Name = "melisandeslair",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 56,
                    Name = "paroxysmuslair",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 57,
                    Name = "gwennoconversation",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 58,
                    Name = "goodendgame",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 59,
                    Name = "goodvsevil",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 60,
                    Name = "greatearthserpents",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 61,
                    Name = "humanoids_u9",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 62,
                    Name = "minocnegative",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 63,
                    Name = "paws",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 64,
                    Name = "selimsbar",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 65,
                    Name = "serpentislecombat_u7",
                    Loop = false
                });

                MusicList.Add(new Music
                {
                    Id = 66,
                    Name = "valoriaships",
                    Loop = false
                });
            }
        }

        public static void Save(XmlTextWriter xml)
        {
            foreach (var filter in SoundFilters)
            {
                xml.WriteStartElement("soundfilter");
                xml.WriteAttributeString("name", filter.Name);
                xml.WriteAttributeString("serial", filter.Serial.ToString());
                xml.WriteEndElement();
            }
        }

        public static void Load(XmlElement node)
        {
            ClearAll();

            try
            {
                foreach (XmlElement el in node.GetElementsByTagName("soundfilter"))
                {
                    try
                    {
                        Sound filter = new Sound
                        {
                            Name = el.GetAttribute("name"),
                            Serial = Serial.Parse(el.GetAttribute("serial")),
                        };

                        SoundFilters.Add(filter);
                    }
                    catch
                    {
                        // bad entry, ignore
                    }
                }
            }
            catch
            {
                // must not be in the profile, move on
            }

            RedrawList();
        }

        public static void ClearAll()
        {
            SoundFilters.Clear();
        }

        public static void RedrawList()
        {
            _soundFilterList?.SafeAction(s =>
            {
                s.BeginUpdate();
                s.Items.Clear();

                SoundList.Clear();

                for (int i = 1; i <= 0xFFF; ++i)
                {
                    if (Ultima.Sounds.IsValidSound(i - 1, out string wavName))
                    {
                        Serial serial = Serial.Parse($"0x{i:X3}");

                        Sound sound = new Sound
                        {
                            Name = wavName,
                            Serial = serial
                        };

                        s.Items.Add(sound, IsFilteredSound(serial, out string name));

                        SoundList.Add(sound);
                    }
                }

                s.EndUpdate();
            });

            _playableMusicList?.SafeAction(s =>
            {
                s.BeginUpdate();
                s.Items.Clear();

                LoadMusic();

                foreach (Music music in MusicList)
                {
                    s.Items.Add(music);
                }

                s.SelectedIndex = 0;
                s.EndUpdate();
            });
        }
    }
}