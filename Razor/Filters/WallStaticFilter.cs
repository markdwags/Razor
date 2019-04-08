using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        internal static bool MakeWallStatic(Assistant.Item wall)
        {
            switch (wall.ItemID)
            {
                case 0x0080:
                case 0x0082:
                    wall.ItemID = WallStaticIDStone;
                    wall.Hue = (ushort)WallColor.Stone;
                    Client.SendToClient(new WorldItem(wall));
                    if (Config.GetBool("ShowStaticWallLabels"))
                        Client.SendToClient(new UnicodeMessage(wall.Serial, wall.ItemID, MessageType.Regular, (ushort)WallColor.Stone, 3, Language.CliLocName, wall.Name, "[Wall Of Stone]"));
                    return true;
                case 0x3996:
                case 0x398C:
                    wall.ItemID = WallStaticID;
                    wall.Hue = (ushort)WallColor.Fire;
                    Client.SendToClient(new WorldItem(wall));
                    if (Config.GetBool("ShowStaticWallLabels"))
                        Client.SendToClient(new UnicodeMessage(wall.Serial, wall.ItemID, MessageType.Regular, (ushort)WallColor.Fire, 3, Language.CliLocName, wall.Name, "[Fire Field]"));
                    return true;
                case 0x3915:
                case 0x3920:
                case 0x3922:
                    wall.ItemID = WallStaticID;
                    wall.Hue = (ushort)WallColor.Poison;
                    Client.SendToClient(new WorldItem(wall));
                    if (Config.GetBool("ShowStaticWallLabels"))
                        Client.SendToClient(new UnicodeMessage(wall.Serial, wall.ItemID, MessageType.Regular, (ushort)WallColor.Poison, 3, Language.CliLocName, wall.Name, "[Poison Field]"));
                    return true;
                case 0x3967:
                case 0x3979:
                    wall.ItemID = WallStaticID;
                    wall.Hue = (ushort)WallColor.Paralyze;
                    Client.SendToClient(new WorldItem(wall));
                    if (Config.GetBool("ShowStaticWallLabels"))
                        Client.SendToClient(new UnicodeMessage(wall.Serial, wall.ItemID, MessageType.Regular, (ushort)WallColor.Paralyze, 3, Language.CliLocName, wall.Name, "[Paralyze Field]"));
                    return true;
                case 0x3946:
                case 0x3956:
                    wall.ItemID = WallStaticID;
                    wall.Hue = (ushort)WallColor.Energy;
                    Client.SendToClient(new WorldItem(wall));
                    if (Config.GetBool("ShowStaticWallLabels"))
                        Client.SendToClient(new UnicodeMessage(wall.Serial, wall.ItemID, MessageType.Regular, (ushort)WallColor.Energy, 3, Language.CliLocName, wall.Name, "[Energy Field]"));
                    return true;
                default:
                    return false;
            }
        }
    }
}
