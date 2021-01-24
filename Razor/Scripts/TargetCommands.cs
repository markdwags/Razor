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

using System;
using System.Collections.Generic;
using Assistant.Scripts.Engine;
using Assistant.Scripts.Helpers;

namespace Assistant.Scripts
{
    public static class TargetCommands
    {
        public static void Register()
        {
            // Targets
            Interpreter.RegisterCommandHandler("target", Target); //Absolute Target

            Interpreter.RegisterCommandHandler("targettype", TargetType); //TargetTypeAction
            Interpreter.RegisterCommandHandler("targetrelloc", TargetRelLoc); //TargetRelLocAction
            Interpreter.RegisterCommandHandler("targetloc", TargetLocation);

            Interpreter.RegisterCommandHandler("waitfortarget", WaitForTarget); //WaitForTargetAction
            Interpreter.RegisterCommandHandler("wft", WaitForTarget); //WaitForTargetAction
        }
        
        private static bool Target(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: target (serial) OR target (closest/random/next/prev [type]");
            }

            switch (args[0].AsString())
            {
                #region Closest
                case "closest":

                    if (args.Length == 1)
                    {
                        Targeting.TargetClosest();
                    }
                    else if (args.Length > 1)
                    {
                        switch (args[1].AsString())
                        {
                            case "enemy":
                                if (args.Length == 3)
                                {
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetCloseEnemyHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetCloseEnemyMonster();
                                    }
                                }
                                else
                                {
                                    Targeting.TargetCloseEnemy();
                                }

                                break;
                            case "friend":
                                Targeting.TargetClosestFriend();
                                break;
                            case "friendly":
                                if (args.Length == 3)
                                {
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetCloseFriendlyHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetCloseFriendlyMonster();
                                    }
                                }
                                else
                                {
                                    Targeting.TargetCloseFriendly();
                                }

                                break;
                            case "gray":
                            case "grey":
                                if (args.Length == 3)
                                {
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetCloseGreyHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetCloseGreyMonster();
                                    }
                                }
                                else
                                {
                                    Targeting.TargetCloseGrey();
                                }

                                break;
                            case "criminal":
                                if (args.Length == 3)
                                {
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetCloseCriminalHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetCloseCriminalMonster();
                                    }
                                }
                                else
                                {
                                    Targeting.TargetCloseCriminal();
                                }

                                break;
                            case "blue":
                            case "innocent":
                                if (args.Length == 3)
                                {
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetCloseInnocentHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetCloseInnocentMonster();
                                    }
                                }
                                else
                                {
                                    Targeting.TargetCloseInnocent();
                                }

                                break;
                            case "red":
                            case "murderer":
                                if (args.Length == 3)
                                {
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetCloseRedHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetCloseRedMonster();
                                    }
                                }
                                else
                                {
                                    Targeting.TargetCloseRed();
                                }

                                break;
                            case "nonfriendly":
                                if (args.Length == 3)
                                {
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetCloseNonFriendlyHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetCloseNonFriendlyMonster();
                                    }
                                }
                                else
                                {
                                    Targeting.TargetCloseNonFriendly();
                                }

                                break;
                            default:
                                throw new RunTimeError(null, $"Unknown target type: '{args[1].AsString()}'");
                        }
                    }

                    break;
                #endregion

                #region Random
                case "random":

                    if (args.Length == 1)
                    {
                        Targeting.TargetRandAnyone();
                    }
                    else if (args.Length > 1)
                    {
                        switch (args[1].AsString())
                        {
                            case "enemy":
                                if (args.Length == 3)
                                {
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetRandEnemyHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetRandEnemyMonster();
                                    }
                                }
                                else
                                {
                                    Targeting.TargetRandEnemy();
                                }

                                break;
                            case "friend":
                                Targeting.TargetRandFriend();
                                break;
                            case "friendly":
                                if (args.Length == 3)
                                {
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetRandFriendlyHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetRandFriendlyMonster();
                                    }
                                }
                                else
                                {
                                    Targeting.TargetRandFriendly();
                                }

                                break;
                            case "gray":
                            case "grey":
                                if (args.Length == 3)
                                {
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetRandGreyHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetRandGreyMonster();
                                    }
                                }
                                else
                                {
                                    Targeting.TargetRandGrey();
                                }

                                break;
                            case "criminal":
                                if (args.Length == 3)
                                {
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetRandCriminalHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetRandCriminalMonster();
                                    }
                                }
                                else
                                {
                                    Targeting.TargetRandCriminal();
                                }

                                break;
                            case "blue":
                            case "innocent":
                                if (args.Length == 3)
                                {
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetRandInnocentHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetRandInnocentMonster();
                                    }
                                }
                                else
                                {
                                    Targeting.TargetRandInnocent();
                                }

                                break;
                            case "red":
                            case "murderer":
                                if (args.Length == 3)
                                {
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetRandRedHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetRandRedMonster();
                                    }
                                }
                                else
                                {
                                    Targeting.TargetRandRed();
                                }

                                break;
                            case "nonfriendly":
                                if (args.Length == 3)
                                {
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetRandNonFriendlyHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.TargetRandNonFriendlyMonster();
                                    }
                                }
                                else
                                {
                                    Targeting.TargetRandNonFriendly();
                                }

                                break;
                        }
                    }

                    break;
                #endregion

                #region Next
                case "next":
                    switch (args.Length)
                    {
                        case 1:
                            Targeting.NextTarget();
                            break;
                        case 2:
                            switch (args[1].AsString())
                            {
                                case "human":
                                case "humanoid":
                                    Targeting.NextTargetHumanoid();

                                    break;
                                case "monster":
                                    Targeting.NextTargetMonster();

                                    break;
                                case "friend":
                                    Targeting.NextTargetFriend();
                                    break;
                                case "nonfriendly":
                                    Targeting.NextTargetNonFriend();
                                    break;
                                default:
                                    throw new RunTimeError(null, $"Unknown target type: '{args[1].AsString()}' - Missing type? (human/monster)");
                            }

                            break;
                        case 3:
                            switch (args[1].AsString())
                            {
                                case "enemy":
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.NextTargetEnemyHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.NextTargetEnemyMonster();
                                    }

                                    break;
                                case "friendly":
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.NextTargetFriendlyHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.NextTargetFriendlyMonster();
                                    }

                                    break;
                                case "gray":
                                case "grey":
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.NextTargetGreyHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.NextTargetGreyMonster();
                                    }

                                    break;
                                case "criminal":
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.NextTargetCriminalHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.NextTargetCriminalMonster();
                                    }

                                    break;
                                case "blue":
                                case "innocent":
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.NextTargetInnocentHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.NextTargetInnocentMonster();
                                    }

                                    break;
                                case "red":
                                case "murderer":
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.NextTargetMurdererHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.NextTargetMurdererMonster();
                                    }

                                    break;
                                case "nonfriendly":
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.NextTargetNonFriendlyHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.NextTargetNonFriendlyMonster();
                                    }

                                    break;
                                default:
                                    throw new RunTimeError(null, $"Unknown target type: '{args[1].AsString()}'");
                            }

                            break;
                    }

                    break;
                #endregion

                #region Previous
                case "prev":
                case "previous":
                    switch (args.Length)
                    {
                        case 1:
                            Targeting.PrevTarget();
                            break;
                        case 2:
                            switch (args[1].AsString())
                            {
                                case "human":
                                case "humanoid":
                                    Targeting.PrevTargetHumanoid();

                                    break;
                                case "monster":
                                    Targeting.PrevTargetMonster();

                                    break;
                                case "friend":
                                    Targeting.PrevTargetFriend();
                                    break;
                                case "nonfriendly":
                                    Targeting.PrevTargetNonFriend();
                                    break;
                                default:
                                    throw new RunTimeError(null, $"Unknown target type: '{args[1].AsString()}' - Missing type? (human/monster)");
                            }

                            break;
                        case 3:
                            switch (args[1].AsString())
                            {
                                case "enemy":
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.PrevTargetEnemyHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.PrevTargetEnemyMonster();
                                    }

                                    break;
                                case "friendly":
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.PrevTargetFriendlyHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.PrevTargetFriendlyMonster();
                                    }

                                    break;
                                case "gray":
                                case "grey":
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.PrevTargetGreyHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.PrevTargetGreyMonster();
                                    }

                                    break;
                                case "criminal":
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.PrevTargetCriminalHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.PrevTargetCriminalMonster();
                                    }

                                    break;
                                case "blue":
                                case "innocent":
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.PrevTargetInnocentHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.PrevTargetInnocentMonster();
                                    }

                                    break;
                                case "red":
                                case "murderer":
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.PrevTargetMurdererHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.PrevTargetMurdererMonster();
                                    }

                                    break;
                                case "nonfriendly":
                                    if (args[2].AsString().IndexOf("human",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.PrevTargetNonFriendlyHumanoid();
                                    }
                                    else if (args[2].AsString().IndexOf("monster",
                                        StringComparison.InvariantCultureIgnoreCase) != -1)
                                    {
                                        Targeting.PrevTargetNonFriendlyMonster();
                                    }

                                    break;
                                default:
                                    throw new RunTimeError(null, $"Unknown target type: '{args[1].AsString()}'");
                            }

                            break;
                    }
                    break;

                #endregion

                default:
                    Serial serial = args[0].AsSerial();

                    if (serial != Serial.Zero) // Target a specific item or mobile
                    {
                        Item item = World.FindItem(serial);

                        if (item != null)
                        {
                            Targeting.Target(item);
                            return true;
                        }

                        Mobile mobile = World.FindMobile(serial);

                        if (mobile != null)
                        {
                            Targeting.Target(mobile);
                        }
                    }

                    break;
            }

            return true;
        }
        
        private static bool TargetType(string command, Argument[] args, bool quiet, bool force)
        {
            if (Targeting.FromGrabHotKey)
                return false;

            if (args.Length < 1)
            {
                throw new RunTimeError(null, "Usage: targettype (graphic) OR ('name of item or mobile type') [inrangecheck/backpack]");
            }

            string gfxStr = args[0].AsString();
            Serial gfx = Utility.ToUInt16(gfxStr, 0);
            List<Item> items;
            List<Mobile> mobiles = new List<Mobile>();

            bool inRangeCheck = false;
            bool backpack = false;

            if (args.Length == 2)
            {
                if (args[1].AsString().IndexOf("pack", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    backpack = true;
                }
                else
                {
                    inRangeCheck = args[1].AsBool();
                }
            }

            // No graphic id, maybe searching by name?
            if (gfx == 0)
            {
                items = CommandHelper.GetItemsByName(gfxStr, backpack, inRangeCheck);

                if (items.Count == 0) // no item found, search mobile by name
                {
                    mobiles = CommandHelper.GetMobilesByName(gfxStr, inRangeCheck);
                }
            }
            else // Provided graphic id for type, check backpack first (same behavior as DoubleClickAction in macros
            {
                ushort id = Utility.ToUInt16(gfxStr, 0);

                items = CommandHelper.GetItemsById(id, backpack, inRangeCheck);

                // Still no item? Mobile check!
                if (items.Count == 0)
                {
                    mobiles = CommandHelper.GetMobilesById(id, inRangeCheck);
                }
            }

            if (items.Count > 0)
            {
                Targeting.Target(items[Utility.Random(items.Count)]);
            }
            else if (mobiles.Count > 0)
            {
                Targeting.Target(mobiles[Utility.Random(mobiles.Count)]);
            }
            else
            {
                World.Player.SendMessage(MsgLevel.Force, $"{command}: Item or mobile type '{gfxStr}' not found");
            }

            return true;
        }

        private static bool TargetRelLoc(string command, Argument[] args, bool quiet, bool force)
        {
            if (Targeting.FromGrabHotKey)
                return false;

            if (args.Length < 2)
            {
                throw new RunTimeError(null, "Usage: targetrelloc (x-offset) (y-offset)");
            }

            int xoffset = Utility.ToInt32(args[0].AsString(), 0);
            int yoffset = Utility.ToInt32(args[1].AsString(), 0);

            ushort x = (ushort)(World.Player.Position.X + xoffset);
            ushort y = (ushort)(World.Player.Position.Y + yoffset);
            short z = (short)World.Player.Position.Z;

            try
            {
                Ultima.HuedTile tile = Map.GetTileNear(World.Player.Map, x, y, z);
                Targeting.Target(new Point3D(x, y, tile.Z), tile.ID);
            }
            catch (Exception e)
            {
                throw new RunTimeError(null, $"Error Executing TargetRelLoc: {e.Message}");
            }

            return true;
        }

        private static bool TargetLocation(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 3)
            {
                throw new RunTimeError(null, "Usage: targetloc (x) (y) (z)");
            }

            Targeting.Target(new TargetInfo
            {
                Type = 1,
                Flags = 0,
                Serial = Serial.Zero,
                X = args[0].AsInt(),
                Y = args[1].AsInt(),
                Z = args[2].AsInt(),
                Gfx = 0
            });

            return true;
        }

        private static bool WaitForTarget(string command, Argument[] args, bool quiet, bool force)
        {
            if (Targeting.HasTarget)
            {
                Interpreter.ClearTimeout();
                return true;
            }

            Interpreter.Timeout(args.Length > 0 ? args[0].AsUInt() : 30000, () => { return true; });

            return false;
        }
    }
}
