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

using Assistant.Core;
using Assistant.Network;
using Assistant.UI;

namespace Assistant.Filters
{
    public class WallStaticFilter
    {
        const ushort WallStaticID = 0x28A8;
        const ushort WallStaticIDStone = 0x0750;

        internal enum WallColor : ushort
        {
            Stone = 0x3B1,
            Fire = 0x0845,
            Poison = 0x016A,
            Paralyze = 0x00DA,
            Energy = 0x0125
        }

        internal static bool MakeWallStatic(Item wall)
        {
            switch (wall.ItemID)
            {
                case 0x0080:
                case 0x0082:
                    wall.ItemID = WallStaticIDStone;
                    wall.Hue = (ushort) WallColor.Stone;
                    Client.Client.Instance.SendToClient(new WorldItem(wall));
                    if (Config.GetBool("ShowStaticWallLabels"))
                        Client.Client.Instance.SendToClient(new UnicodeMessage(wall.Serial, wall.ItemID, MessageType.Regular,
                            (ushort) WallColor.Stone, 3, Language.CliLocName, wall.Name, "[Wall Of Stone]"));
                    return true;
                case 0x3996:
                case 0x398C:
                    wall.ItemID = WallStaticID;
                    wall.Hue = (ushort) WallColor.Fire;
                    Client.Client.Instance.SendToClient(new WorldItem(wall));
                    if (Config.GetBool("ShowStaticWallLabels"))
                        Client.Client.Instance.SendToClient(new UnicodeMessage(wall.Serial, wall.ItemID, MessageType.Regular,
                            (ushort) WallColor.Fire, 3, Language.CliLocName, wall.Name, "[Fire Field]"));
                    return true;
                case 0x3915:
                case 0x3920:
                case 0x3922:
                    wall.ItemID = WallStaticID;
                    wall.Hue = (ushort) WallColor.Poison;
                    Client.Client.Instance.SendToClient(new WorldItem(wall));
                    if (Config.GetBool("ShowStaticWallLabels"))
                        Client.Client.Instance.SendToClient(new UnicodeMessage(wall.Serial, wall.ItemID, MessageType.Regular,
                            (ushort) WallColor.Poison, 3, Language.CliLocName, wall.Name, "[Poison Field]"));
                    return true;
                case 0x3967:
                case 0x3979:
                    wall.ItemID = WallStaticID;
                    wall.Hue = (ushort) WallColor.Paralyze;
                    Client.Client.Instance.SendToClient(new WorldItem(wall));
                    if (Config.GetBool("ShowStaticWallLabels"))
                        Client.Client.Instance.SendToClient(new UnicodeMessage(wall.Serial, wall.ItemID, MessageType.Regular,
                            (ushort) WallColor.Paralyze, 3, Language.CliLocName, wall.Name, "[Paralyze Field]"));
                    return true;
                case 0x3946:
                case 0x3956:
                    wall.ItemID = WallStaticID;
                    wall.Hue = (ushort) WallColor.Energy;
                    Client.Client.Instance.SendToClient(new WorldItem(wall));
                    if (Config.GetBool("ShowStaticWallLabels"))
                        Client.Client.Instance.SendToClient(new UnicodeMessage(wall.Serial, wall.ItemID, MessageType.Regular,
                            (ushort) WallColor.Energy, 3, Language.CliLocName, wall.Name, "[Energy Field]"));
                    return true;
                default:
                    return false;
            }
        }
    }
}