using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ultima;

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
                    p.Write((ushort)Config.GetInt("DragonGraphic"));
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
                    p.Write((ushort)Config.GetInt("DrakeGraphic"));
                }
            }
        }
    }
}
