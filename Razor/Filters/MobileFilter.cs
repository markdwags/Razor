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

using System.IO;

namespace Assistant.Filters
{
    public static class MobileFilter
    {
        public static void ApplyDragonFilter(Packet p, Mobile m)
        {
            if (Config.GetBool("FilterDragonGraphics"))
            {
                if (m.Body == 0xC || m.Body == 0x3B)
                {
                    p.Seek(-2, SeekOrigin.Current);
                    p.Write((ushort) Config.GetInt("DragonGraphic"));
                }
            }
        }

        public static void ApplyDrakeFilter(Packet p, Mobile m)
        {
            if (Config.GetBool("FilterDrakeGraphics"))
            {
                if (m.Body == 0x3C || m.Body == 0x3D)
                {
                    p.Seek(-2, SeekOrigin.Current);
                    p.Write((ushort) Config.GetInt("DrakeGraphic"));
                }
            }
        }

        public static void ApplyDaemonFilter(Packet p, Mobile m)
        {
            if (Config.GetBool("FilterDaemonGraphics"))
            {
                if (m.Body == 0x9)
                {
                    p.Seek(-2, SeekOrigin.Current);
                    p.Write((ushort) Config.GetInt("DaemonGraphic"));
                }
            }
        }
    }
}