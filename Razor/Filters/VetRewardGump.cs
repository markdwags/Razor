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

using System;

namespace Assistant.Filters
{
    public class VetRewardGumpFilter : Filter
    {
        // 1006046 = You have reward items available.  Click 'ok' below to get the selection menu or 'cancel' to be prompted upon your next login.
        private static string m_VetRewardStr = "{ xmfhtmlgump 52 35 420 55 1006046 1 1 }";

        public static void Initialize()
        {
            Filter.Register(new VetRewardGumpFilter());
        }

        private VetRewardGumpFilter()
        {
        }

        public override byte[] PacketIDs
        {
            get { return new byte[] {0xB0, 0xDD}; }
        }

        public override LocString Name
        {
            get { return LocString.VetRewardGump; }
        }

        public override void OnFilter(PacketReader p, PacketHandlerEventArgs args)
        {
            // completely skip this filter if we've been connected for more thn 1 minute
            if (Client.Instance.ConnectionStart + TimeSpan.FromMinutes(1.0) < DateTime.UtcNow)
                return;

            try
            {
                p.Seek(0, System.IO.SeekOrigin.Begin);
                byte packetID = p.ReadByte();

                p.MoveToData();

                uint ser = p.ReadUInt32();
                uint tid = p.ReadUInt32();
                int x = p.ReadInt32();
                int y = p.ReadInt32();
                string layout = null;

                if (packetID == 0xDD)
                {
                    layout = p.GetCompressedReader().ReadString();
                }
                else
                {
                    ushort layoutLength = p.ReadUInt16();
                    layout = p.ReadString(layoutLength);
                }

                if (layout != null && layout.IndexOf(m_VetRewardStr) != -1)
                    args.Block = true;
            }
            catch
            {
            }
        }
    }
}