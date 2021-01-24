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
                case "close":
                case "closest":
                    SetTarget(args, true);

                    break;

                case "rand":
                case "random":
                    SetTarget(args, false, true);

                    break;

                case "next":
                    SetTarget(args, false, false, true);

                    break;

                case "prev":
                case "previous":
                    SetTarget(args, false, false, false, true);

                    break;

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
                throw new RunTimeError(null,
                    "Usage: targettype (graphic) OR ('name of item or mobile type') [inrangecheck/backpack]");
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

            ushort x = (ushort) (World.Player.Position.X + xoffset);
            ushort y = (ushort) (World.Player.Position.Y + yoffset);
            short z = (short) World.Player.Position.Z;

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

        /// <summary>
        /// Parse the script input to target the correct mobile
        /// </summary>
        /// <param name="args"></param>
        /// <param name="closest"></param>
        /// <param name="random"></param>
        /// <param name="next"></param>
        /// <param name="prev"></param>
        private static void SetTarget(Argument[] args, bool closest, bool random = false, bool next = false,
            bool prev = false)
        {
            if (args.Length == 1)
            {
                if (closest)
                {
                    Targeting.TargetClosest();
                }
                else if (random)
                {
                    Targeting.TargetRandAnyone();
                }
                else if (next)
                {
                    Targeting.NextTarget();
                }
                else
                {
                    Targeting.PrevTarget();
                }
            }
            else if ((next || prev) && args.Length == 2)
            {
                switch (args[1].AsString())
                {
                    case "human":
                    case "humanoid":
                        if (next)
                        {
                            Targeting.NextTargetHumanoid();
                        }
                        else
                        {
                            Targeting.PrevTargetHumanoid();
                        }

                        break;
                    case "monster":
                        if (next)
                        {
                            Targeting.NextTargetMonster();
                        }
                        else
                        {
                            Targeting.PrevTargetMonster();
                        }

                        break;
                    case "friend":
                        if (next)
                        {
                            Targeting.NextTargetFriend();
                        }
                        else
                        {
                            Targeting.PrevTargetFriend();
                        }

                        break;
                    case "nonfriendly":
                        if (next)
                        {
                            Targeting.NextTargetNonFriend();
                        }
                        else
                        {
                            Targeting.PrevTargetNonFriend();
                        }

                        break;
                    default:
                        throw new RunTimeError(null,
                            $"Unknown target type: '{args[1].AsString()}' - Missing type? (human/monster)");
                }
            }
            else if (args.Length > 1)
            {
                switch (args[1].AsString())
                {
                    case "enemy":
                        if (args.Length == 3)
                        {
                            if (args[2].AsString().IndexOf("human", StringComparison.InvariantCultureIgnoreCase) != -1)
                            {
                                if (closest)
                                {
                                    Targeting.TargetCloseEnemyHumanoid();
                                }
                                else if (random)
                                {
                                    Targeting.TargetRandEnemyHumanoid();
                                }
                                else if (next)
                                {
                                    Targeting.NextTargetEnemyHumanoid();
                                }
                                else
                                {
                                    Targeting.PrevTargetEnemyHumanoid();
                                }
                            }
                            else if (args[2].AsString()
                                .IndexOf("monster", StringComparison.InvariantCultureIgnoreCase) != -1)
                            {
                                if (closest)
                                {
                                    Targeting.TargetCloseEnemyMonster();
                                }
                                else if (random)
                                {
                                    Targeting.TargetRandEnemyMonster();
                                }
                                else if (next)
                                {
                                    Targeting.NextTargetEnemyMonster();
                                }
                                else
                                {
                                    Targeting.PrevTargetEnemyMonster();
                                }
                            }
                        }
                        else
                        {
                            if (closest)
                            {
                                Targeting.TargetCloseEnemy();
                            }
                            else if (random)
                            {
                                Targeting.TargetRandEnemy();
                            }
                        }

                        break;
                    case "friend":
                        if (closest)
                        {
                            Targeting.TargetClosestFriend();
                        }
                        else if (random)
                        {
                            Targeting.TargetRandFriend();
                        }
                        else if (next)
                        {
                            Targeting.NextTargetFriend();
                        }
                        else
                        {
                            Targeting.PrevTargetFriend();
                        }

                        break;
                    case "friendly":
                        if (args.Length == 3)
                        {
                            if (args[2].AsString().IndexOf("human", StringComparison.InvariantCultureIgnoreCase) != -1)
                            {
                                if (closest)
                                {
                                    Targeting.TargetCloseFriendlyHumanoid();
                                }
                                else if (random)
                                {
                                    Targeting.TargetRandFriendlyHumanoid();
                                }
                                else if (next)
                                {
                                    Targeting.NextTargetFriendlyHumanoid();
                                }
                                else
                                {
                                    Targeting.PrevTargetFriendlyHumanoid();
                                }
                            }
                            else if (args[2].AsString()
                                .IndexOf("monster", StringComparison.InvariantCultureIgnoreCase) != -1)
                            {
                                if (closest)
                                {
                                    Targeting.TargetCloseFriendlyMonster();
                                }
                                else if (random)
                                {
                                    Targeting.TargetRandFriendlyMonster();
                                }
                                else if (next)
                                {
                                    Targeting.NextTargetFriendlyMonster();
                                }
                                else
                                {
                                    Targeting.PrevTargetFriendlyMonster();
                                }
                            }
                        }
                        else
                        {
                            if (closest)
                            {
                                Targeting.TargetCloseFriendly();
                            }
                            else if (random)
                            {
                                Targeting.TargetRandFriendly();
                            }
                        }

                        break;
                    case "gray":
                    case "grey":
                        if (args.Length == 3)
                        {
                            if (args[2].AsString().IndexOf("human", StringComparison.InvariantCultureIgnoreCase) != -1)
                            {
                                if (closest)
                                {
                                    Targeting.TargetCloseGreyHumanoid();
                                }
                                else if (random)
                                {
                                    Targeting.TargetRandGreyHumanoid();
                                }
                                else if (next)
                                {
                                    Targeting.NextTargetGreyHumanoid();
                                }
                                else
                                {
                                    Targeting.PrevTargetGreyHumanoid();
                                }
                            }
                            else if (args[2].AsString()
                                .IndexOf("monster", StringComparison.InvariantCultureIgnoreCase) != -1)
                            {
                                if (closest)
                                {
                                    Targeting.TargetCloseGreyMonster();
                                }
                                else if (random)
                                {
                                    Targeting.TargetRandGreyMonster();
                                }
                                else if (next)
                                {
                                    Targeting.NextTargetGreyMonster();
                                }
                                else
                                {
                                    Targeting.PrevTargetGreyMonster();
                                }
                            }
                        }
                        else
                        {
                            if (closest)
                            {
                                Targeting.TargetCloseGrey();
                            }
                            else if (random)
                            {
                                Targeting.TargetRandGrey();
                            }
                        }

                        break;
                    case "criminal":
                        if (args.Length == 3)
                        {
                            if (args[2].AsString().IndexOf("human", StringComparison.InvariantCultureIgnoreCase) != -1)
                            {
                                if (closest)
                                {
                                    Targeting.TargetCloseCriminalHumanoid();
                                }
                                else if (random)
                                {
                                    Targeting.TargetRandCriminalHumanoid();
                                }
                                else if (next)
                                {
                                    Targeting.NextTargetCriminalHumanoid();
                                }
                                else
                                {
                                    Targeting.PrevTargetCriminalHumanoid();
                                }
                            }
                            else if (args[2].AsString()
                                .IndexOf("monster", StringComparison.InvariantCultureIgnoreCase) != -1)
                            {
                                if (closest)
                                {
                                    Targeting.TargetCloseCriminalMonster();
                                }
                                else if (random)
                                {
                                    Targeting.TargetRandCriminalMonster();
                                }
                                else if (next)
                                {
                                    Targeting.NextTargetCriminalMonster();
                                }
                                else
                                {
                                    Targeting.PrevTargetCriminalMonster();
                                }
                            }
                        }
                        else
                        {
                            if (closest)
                            {
                                Targeting.TargetCloseCriminal();
                            }
                            else if (random)
                            {
                                Targeting.TargetRandCriminal();
                            }
                        }

                        break;
                    case "blue":
                    case "innocent":
                        if (args.Length == 3)
                        {
                            if (args[2].AsString().IndexOf("human", StringComparison.InvariantCultureIgnoreCase) != -1)
                            {
                                if (closest)
                                {
                                    Targeting.TargetCloseInnocentHumanoid();
                                }
                                else if (random)
                                {
                                    Targeting.TargetRandInnocentHumanoid();
                                }
                                else if (next)
                                {
                                    Targeting.NextTargetInnocentHumanoid();
                                }
                                else
                                {
                                    Targeting.PrevTargetInnocentHumanoid();
                                }
                            }
                            else if (args[2].AsString()
                                .IndexOf("monster", StringComparison.InvariantCultureIgnoreCase) != -1)
                            {
                                if (closest)
                                {
                                    Targeting.TargetCloseInnocentMonster();
                                }
                                else if (random)
                                {
                                    Targeting.TargetRandInnocentMonster();
                                }
                                else if (next)
                                {
                                    Targeting.NextTargetInnocentMonster();
                                }
                                else
                                {
                                    Targeting.PrevTargetInnocentMonster();
                                }
                            }
                        }
                        else
                        {
                            if (closest)
                            {
                                Targeting.TargetCloseInnocent();
                            }
                            else if (random)
                            {
                                Targeting.TargetRandInnocent();
                            }
                        }

                        break;
                    case "red":
                    case "murderer":
                        if (args.Length == 3)
                        {
                            if (args[2].AsString().IndexOf("human", StringComparison.InvariantCultureIgnoreCase) != -1)
                            {
                                if (closest)
                                {
                                    Targeting.TargetCloseRedHumanoid();
                                }
                                else if (random)
                                {
                                    Targeting.TargetRandRedHumanoid();
                                }
                                else if (next)
                                {
                                    Targeting.NextTargetMurdererHumanoid();
                                }
                                else
                                {
                                    Targeting.PrevTargetMurdererHumanoid();
                                }
                            }
                            else if (args[2].AsString()
                                .IndexOf("monster", StringComparison.InvariantCultureIgnoreCase) != -1)
                            {
                                if (closest)
                                {
                                    Targeting.TargetCloseRedMonster();
                                }
                                else if (random)
                                {
                                    Targeting.TargetRandRedMonster();
                                }
                                else if (next)
                                {
                                    Targeting.NextTargetMurdererMonster();
                                }
                                else
                                {
                                    Targeting.PrevTargetMurdererMonster();
                                }
                            }
                        }
                        else
                        {
                            if (closest)
                            {
                                Targeting.TargetCloseRed();
                            }
                            else if (random)
                            {
                                Targeting.TargetRandRed();
                            }
                        }

                        break;
                    case "nonfriendly":
                        if (args.Length == 3)
                        {
                            if (args[2].AsString().IndexOf("human", StringComparison.InvariantCultureIgnoreCase) != -1)
                            {
                                if (closest)
                                {
                                    Targeting.TargetCloseNonFriendlyHumanoid();
                                }
                                else if (random)
                                {
                                    Targeting.TargetRandNonFriendlyHumanoid();
                                }
                                else if (next)
                                {
                                    Targeting.NextTargetNonFriendlyHumanoid();
                                }
                                else
                                {
                                    Targeting.PrevTargetNonFriendlyHumanoid();
                                }
                            }
                            else if (args[2].AsString()
                                .IndexOf("monster", StringComparison.InvariantCultureIgnoreCase) != -1)
                            {
                                if (closest)
                                {
                                    Targeting.TargetCloseNonFriendlyMonster();
                                }
                                else if (random)
                                {
                                    Targeting.TargetRandNonFriendlyMonster();
                                }
                                else if (next)
                                {
                                    Targeting.NextTargetNonFriendlyMonster();
                                }
                                else
                                {
                                    Targeting.PrevTargetNonFriendlyMonster();
                                }
                            }
                        }
                        else
                        {
                            if (closest)
                            {
                                Targeting.TargetCloseNonFriendly();
                            }
                            else if (random)
                            {
                                Targeting.TargetRandNonFriendly();
                            }
                        }

                        break;
                    default:
                        throw new RunTimeError(null, $"Unknown target type: '{args[1].AsString()}'");
                }
            }
        }
    }
}