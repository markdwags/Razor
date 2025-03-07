#region license
// Razor: An Ultima Online Assistant
// Copyright (c) 2025 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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
using Assistant.Core;
using Assistant.Filters;
using Assistant.Scripts;

namespace Assistant
{
    public partial class Targeting
    {
        private enum TargetSelectionType
        {
            Closest,
            Random
        }

        private enum TargetFilterType
        {
            Any,
            Humanoid,
            Monster,
            Friend
        }
        
        /// <summary>
        /// Checks if the mobile passes all filters based on the type and notoriety
        /// </summary>
        /// <param name="m"></param>
        /// <param name="filterType"></param>
        /// <param name="notorietyTypes"></param>
        /// <returns></returns>
        private static bool PassesFilters(Mobile m, TargetFilterType filterType, int[] notorietyTypes)
        {
            // Common exclusion criteria for all target types
            if (m.Blessed || m.IsGhost || m.Serial == World.Player.Serial || 
                TargetFilterManager.IsFilteredTarget(m.Serial) ||
                !Utility.InRange(World.Player.Position, m.Position, Config.GetInt("LTRange")))
            {
                return false;
            }

            // Body type filtering if humanoid filter is specified
            if (filterType == TargetFilterType.Humanoid && 
                (m.Body != 0x0190 && m.Body != 0x0191 && m.Body != 0x025D && m.Body != 0x025E))
            {
                return false;
            }

            if (filterType == TargetFilterType.Monster && !m.IsMonster)
            {
                return false;
            }

            // Friend filter handling is special
            if (filterType == TargetFilterType.Friend)
            {
                return FriendsManager.IsFriend(m.Serial);
            }
            
            // For non-friend targets, check if they're NOT in friends list (unless notoriety[0] is 0)
            if (FriendsManager.IsFriend(m.Serial) && (notorietyTypes.Length == 0 || notorietyTypes[0] != 0))
            {
                return false;
            }

            // If no notoriety filters specified, include all targets that passed previous filters
            if (notorietyTypes.Length == 0)
            {
                return true;
            }

            // Check if mobile matches any of the specified notoriety types
            for (var i = 0; i < notorietyTypes.Length; i++)
            {
                if (notorietyTypes[i] == m.Notoriety)
                {
                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Find a random target that matches the criteria
        /// </summary>
        /// <param name="filterType"></param>
        /// <param name="notorietyTypes"></param>
        /// <returns></returns>
        private static Mobile FindRandomTarget(TargetFilterType filterType, int[] notorietyTypes)
        {
            var list = new List<Mobile>();
            
            foreach (var m in World.MobilesInRange(12))
            {
                if (PassesFilters(m, filterType, notorietyTypes))
                {
                    list.Add(m);
                }
            }
            
            return list.Count > 0 ? list[Utility.Random(list.Count)] : null;
        }

        /// <summary>
        /// Find the closest target that matches the criteria
        /// </summary>
        /// <param name="filterType"></param>
        /// <param name="notorietyTypes"></param>
        /// <returns></returns>
        private static Mobile FindClosestTarget(TargetFilterType filterType, int[] notorietyTypes)
        {
            Mobile closest = null;
            var closestDistSquared = double.MaxValue;
            
            foreach (var m in World.MobilesInRange(12))
            {
                if (PassesFilters(m, filterType, notorietyTypes))
                {
                    float xDelta = Math.Abs(m.Position.X - World.Player.Position.X);
                    float yDelta = Math.Abs(m.Position.Y - World.Player.Position.Y);
                    var distSquared = xDelta * xDelta + yDelta * yDelta;
            
                    if (closest == null || distSquared < closestDistSquared)
                    {
                        closestDistSquared = distSquared;
                        closest = m;
                    }
                }
            }
            
            return closest;
        }
        
        /// <summary>
        /// Determine the appropriate flagType based on target category
        /// </summary>
        /// <param name="notorietyTypes"></param>
        /// <returns></returns>
        private static byte DetermineFlagType(int[] notorietyTypes)
        {
            if (!IsSmartTargetingEnabled() || !Config.GetBool("SmartClosestRandom"))
            {
                return 0;
            }
            
            if (notorietyTypes.Length == 0)
            {
                return 0;
            }
            
            // Non-friendly targets (attackable, criminal, enemy, murderer) should use flag 1 (harmful)
            foreach (var noto in notorietyTypes)
            {
                switch (noto)
                {
                    case (int)TargetType.Attackable:
                    case (int)TargetType.Criminal:
                    case (int)TargetType.Enemy:
                    case (int)TargetType.Murderer:
                        return 1; // Harmful target
                    
                    case (int)TargetType.Innocent:
                    case (int)TargetType.GuildAlly:
                    case (int)TargetType.Invalid:
                        return 2; // Beneficial target
                }
            }
            
            return 0; // Default - sets both targets
        }

        /// <summary>
        /// Main targeting method that handles all targeting scenarios
        /// </summary>
        /// <param name="selectionType"></param>
        /// <param name="filterType"></param>
        /// <param name="notorietyTypes"></param>
        private static void ProcessTargeting(TargetSelectionType selectionType, TargetFilterType filterType, params int[] notorietyTypes)
        {
            int requiredFeature = selectionType == TargetSelectionType.Closest ? 
                FeatureBit.ClosestTargets : FeatureBit.RandomTargets;
                
            if (!Client.Instance.AllowBit(requiredFeature))
            {
                World.Player.SendMessage("This feature is not enabled on this server.");
                return;
            }

            Mobile selected = selectionType == TargetSelectionType.Closest
                ? FindClosestTarget(filterType, notorietyTypes)
                : FindRandomTarget(filterType, notorietyTypes);

            if (selected != null)
            {
                byte flagType = DetermineFlagType(notorietyTypes);

                if (filterType == TargetFilterType.Friend)
                {
                    flagType = 2;
                }
                
                SetLastTargetTo(selected, flagType);
                ScriptManager.TargetFound = true;
            }
            else
            {
                World.Player.SendMessage(MsgLevel.Warning, LocString.TargNoOne);
            }
        }
        
        public static void RandomTarget(params int[] noto)
        {
            ProcessTargeting(TargetSelectionType.Random, TargetFilterType.Any, noto);
        }

        public static void RandomHumanoidTarget(params int[] noto)
        {
            ProcessTargeting(TargetSelectionType.Random, TargetFilterType.Humanoid, noto);
        }

        public static void RandomMonsterTarget(params int[] noto)
        {
            ProcessTargeting(TargetSelectionType.Random, TargetFilterType.Monster, noto);
        }

        public static void RandomFriendTarget()
        {
            ProcessTargeting(TargetSelectionType.Random, TargetFilterType.Friend);
        }

        public static void ClosestTarget(params int[] noto)
        {
            ProcessTargeting(TargetSelectionType.Closest, TargetFilterType.Any, noto);
        }

        public static void ClosestHumanoidTarget(params int[] noto)
        {
            ProcessTargeting(TargetSelectionType.Closest, TargetFilterType.Humanoid, noto);
        }

        public static void ClosestMonsterTarget(params int[] noto)
        {
            ProcessTargeting(TargetSelectionType.Closest, TargetFilterType.Monster, noto);
        }

        public static void ClosestFriendTarget()
        {
            ProcessTargeting(TargetSelectionType.Closest, TargetFilterType.Friend);
        }
    }
}