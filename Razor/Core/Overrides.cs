using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

namespace Assistant.Core
{
    public static class Overrides
    {
        public static Dictionary<ushort, bool> TwoHanded { get; private set; }

        public static void Load()
        {
            if (TwoHanded == null)
                TwoHanded = new Dictionary<ushort, bool>();
            else
                TwoHanded.Clear();

            string file = Path.Combine(Config.GetInstallDirectory(), "overrides.def");

            if (!File.Exists(file))
            {
                return;
            }

            string[] lines = File.ReadAllLines(file);

            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line) || line[0] == '#')
                {
                    continue;
                }

                string[] split = line.Trim().Split(' ');

                switch (split[0])
                {
                    case "twohanded":
                        TwoHanded.Add(Convert.ToUInt16(split[1], 16), Convert.ToBoolean(split[2]));
                        break;
                }
            }
        }
    }
}
