#region license
// Razor: An Ultima Online Assistant
// Copyright (c) 2022 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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
        private static ushort[] _filters;

        public static void Load()
        {
            _filters = new ushort[4];
            _filters[0] = (ushort) Config.GetInt("DragonGraphic");
            _filters[1] = (ushort) Config.GetInt("DrakeGraphic");
            _filters[2] = (ushort) Config.GetInt("DaemonGraphic");
            _filters[3] = (ushort) Config.GetInt("WyrmGraphic");
        }

        public static void Apply(Packet p, Mobile m)
        {
            Apply(p, m.Body);
        }

        public static void Apply(Packet p, ushort body)
        {
            if ((body == 0xC || body == 0x3B) && Config.GetBool("FilterDragonGraphics"))
            {
                p.Seek(-2, SeekOrigin.Current);
                p.Write(_filters[0]);
            }
            else if ((body == 0x3C || body == 0x3D) && Config.GetBool("FilterDrakeGraphics"))
            {
                p.Seek(-2, SeekOrigin.Current);
                p.Write(_filters[1]);
            }
            else if (body == 0x9 && Config.GetBool("FilterDaemonGraphics"))
            {
                p.Seek(-2, SeekOrigin.Current);
                p.Write(_filters[2]);
            }
            else if ((body == 0x31 || body == 0xB4) && Config.GetBool("FilterWyrmGraphics"))
            {
                p.Seek(-2, SeekOrigin.Current);
                p.Write(_filters[3]);
            }
        }

        private static int Get(ushort body)
        {
            switch (body)
            {
                case 0xC:
                case 0x3B:
                    return _filters[0];
                case 0x3C:
                case 0x3D:
                    return _filters[1];
                case 0x9:
                    return _filters[2];
                case 0x31:
                case 0xB4:
                    return _filters[3];
                default:
                    return body;
            }
        }

        public static void ApplyToMobiles()
        {
            foreach (Mobile mobile in World.MobilesInRange())
            {
                ushort body = (ushort)Get(mobile.Body);

                if (mobile.Body != body)
                {
                    mobile.Body = body;
                    Client.Instance.SendToClient(new MobileIncoming(mobile));
                }
            }
        }
    }
}