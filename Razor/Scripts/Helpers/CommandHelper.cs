using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistant.Scripts.Engine;

namespace Assistant.Scripts.Helpers
{
    public static class CommandHelper
    {
        /// <summary>
        /// Common logic for dclicktype and targettype to find items by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="backpack"></param>
        /// <param name="inRange"></param>
        /// <returns></returns>
        public static List<Item> GetItemsByName(string name, bool backpack, bool inRange)
        {
            List<Item> items = new List<Item>();

            if (backpack && World.Player.Backpack != null) // search backpack only
            {
                Item i = World.Player.Backpack.FindItemByName(name, true);

                if (i != null)
                    items.Add(i);
            }
            else if (inRange) // inrange includes both backpack and within 2 tiles
            {
                items.AddRange(World.FindItemsByName(name).Where(item =>
                    !item.IsInBank && (Utility.InRange(World.Player.Position, item.Position, 2) ||
                                       item.RootContainer == World.Player)).ToList());
            }
            else
            {
                items.AddRange(World.FindItemsByName(name).Where(item => !item.IsInBank).ToList());
            }

            return items;
        }

        /// <summary>
        /// Common logic for dclicktype and targettype to find items by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="backpack"></param>
        /// <param name="inRange"></param>
        /// <returns></returns>
        public static List<Item> GetItemsById(ushort id, bool backpack, bool inRange)
        {
            List<Item> items = new List<Item>();

            if (backpack && World.Player.Backpack != null)
            {
                Item i = World.Player.Backpack.FindItemByID(id);

                if (i != null)
                    items.Add(i);
            } 
            else
            {
                foreach (Item i in World.Items.Values)
                {
                    if (i.ItemID == id && i.RootContainer == null)
                    {
                        if (inRange)
                        {
                            if (Utility.InRange(World.Player.Position, i.Position, 2))
                                items.Add(i);
                        }
                        else
                        {
                            items.Add(i);
                        }
                    }
                }

                if (items.Count == 0)
                {
                    foreach (Item i in World.Items.Values)
                    {
                        if (i.ItemID == id && !i.IsInBank)
                        {
                            if (inRange)
                            {
                                if (Utility.InRange(World.Player.Position, i.Position, 2))
                                    items.Add(i);
                            }
                            else
                            {
                                items.Add(i);
                            }
                        }
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// Common logic for dclicktype and targettype to find mobiles by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="inRange"></param>
        /// <returns></returns>
        public static List<Mobile> GetMobilesByName(string name, bool inRange)
        {
            List<Mobile> mobiles;

            if (inRange)
            {
                mobiles = World.FindMobilesByName(name)
                    .Where(m => Utility.InRange(World.Player.Position, m.Position, 2)).ToList();
            }
            else
            {
                mobiles = World.FindMobilesByName(name);
            }

            return mobiles;
        }

        /// <summary>
        /// Common logic for dclicktype and targettype to find mobiles by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="inRange"></param>
        /// <returns></returns>
        public static List<Mobile> GetMobilesById(ushort id, bool inRange)
        {
            List<Mobile> mobiles;

            if (inRange)
            {
                mobiles = World.MobilesInRange()
                    .Where(m => Utility.InRange(World.Player.Position, m.Position, 2) && m.Body == id).ToList();
            }
            else
            {
                mobiles = World.MobilesInRange().Where(m => m.Body == id).ToList();
            }

            return mobiles;
        }

        /// <summary>
        /// Parse the script input to target the correct mobile
        /// </summary>
        /// <param name="args"></param>
        /// <param name="closest"></param>
        /// <param name="random"></param>
        /// <param name="next"></param>
        /// <param name="prev"></param>
        public static void SetTarget(Argument[] args, bool closest, bool random = false, bool next = false, bool prev = false)
        {
            ScriptManager.TargetFound = false;

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
                string[] notoList = args[1].AsString().Split(',');

                foreach (string noto in notoList)
                {
                    if (ScriptManager.TargetFound)
                    {
                        break;
                    }

                    switch (noto)
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
}
