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

using System.IO;

namespace Assistant.Filters
{
    public static class MobileFilter
    {
        public static void Apply(Packet p, Mobile m)
        {
            Apply(p, m.Body);
        }

        public static void Apply(Packet p, ushort body)
        {
            if ((body == 0xC || body == 0x3B) && Config.GetBool("FilterDragonGraphics"))
            {
                p.Seek(-2, SeekOrigin.Current);
                p.Write((ushort)Config.GetInt("DragonGraphic"));
            }
            else if ((body == 0x3C || body == 0x3D) && Config.GetBool("FilterDrakeGraphics"))
            {
                p.Seek(-2, SeekOrigin.Current);
                p.Write((ushort)Config.GetInt("DrakeGraphic"));
            }
            else if (body == 0x9 && Config.GetBool("FilterDaemonGraphics"))
            {
                p.Seek(-2, SeekOrigin.Current);
                p.Write((ushort)Config.GetInt("DaemonGraphic"));
            }
            else if ((body == 0x31 || body == 0xB4) && Config.GetBool("FilterWyrmGraphics"))
            {
                p.Seek(-2, SeekOrigin.Current);
                p.Write((ushort)Config.GetInt("WyrmGraphic"));
            }
        }
    }
}