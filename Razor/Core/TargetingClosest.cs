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

using System.Collections.Generic;
using Assistant.Core;
using Assistant.Filters;

namespace Assistant
{
    public partial class Targeting
    {
        private static void InitClosestTargets()
        {
            HotKey.Add(HKCategory.Targets, LocString.TargClosest, TargetClosest);

            HotKey.Add(HKCategory.Targets, LocString.TargClosestFriend, TargetClosestFriend);

            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetMurderer, LocString.TargCloseRed,
                TargetCloseRed);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetMurderer, LocString.TargCloseRedHumanoid,
                TargetCloseRedHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetMurderer, LocString.TargCloseRedMonster,
                TargetCloseRedMonster);

            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetNonFriendly, LocString.TargCloseNFriend,
                TargetCloseNonFriendly);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetNonFriendly, LocString.TargClosestNFriendlyHuman,
                TargetCloseNonFriendlyHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetNonFriendly, LocString.TargClosestNFriendlyMonster,
                TargetCloseNonFriendlyMonster);

            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetFriendly, LocString.TargCloseFriend,
                TargetCloseFriendly);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetFriendly, LocString.TargClosestFriendlyHuman,
                TargetCloseFriendlyHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetFriendly, LocString.TargClosestFriendlyMonster,
                TargetCloseFriendlyMonster);

            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetInnocent, LocString.TargCloseBlue,
                TargetCloseInnocent);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetInnocent, LocString.TargCloseInnocentHuman,
                TargetCloseInnocentHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetInnocent, LocString.TargClosestInnocentMonster,
                TargetCloseInnocentMonster);

            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetGrey, LocString.TargCloseGrey,
                TargetCloseGrey);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetGrey, LocString.TargCloseGreyHuman,
                TargetCloseGreyHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetGrey, LocString.TargCloseGreyMonster,
                TargetCloseGreyMonster);

            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetCriminal, LocString.TargCloseCriminal,
                TargetCloseCriminal);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetCriminal, LocString.TargCloseCriminalHuman,
                TargetCloseCriminalHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetCriminal, LocString.TargClosestCriminalMonster,
                TargetCloseCriminalMonster);

            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetEnemy, LocString.TargCloseEnemy,
                TargetCloseEnemy);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetEnemy, LocString.TargCloseEnemyHuman,
                TargetCloseEnemyHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetEnemy, LocString.TargCloseEnemyMonster,
                TargetCloseEnemyMonster);
        }


        public static void TargetCloseNonFriendly()
        {
            ClosestTarget((int) TargetType.Attackable, (int) TargetType.Criminal, (int) TargetType.Enemy,
                (int) TargetType.Murderer);
        }

        public static void TargetCloseNonFriendlyHumanoid()
        {
            ClosestHumanoidTarget((int) TargetType.Attackable, (int) TargetType.Criminal, (int) TargetType.Enemy,
                (int) TargetType.Murderer);
        }

        public static void TargetCloseNonFriendlyMonster()
        {
            ClosestMonsterTarget((int) TargetType.Attackable, (int) TargetType.Criminal, (int) TargetType.Enemy,
                (int) TargetType.Murderer);
        }

        public static void TargetCloseFriendly()
        {
            ClosestTarget((int) TargetType.Invalid, (int) TargetType.Innocent, (int) TargetType.GuildAlly);
        }

        public static void TargetCloseFriendlyHumanoid()
        {
            ClosestHumanoidTarget((int) TargetType.Invalid, (int) TargetType.Innocent, (int) TargetType.GuildAlly);
        }

        public static void TargetCloseFriendlyMonster()
        {
            ClosestTarget((int) TargetType.Invalid, (int) TargetType.Innocent, (int) TargetType.GuildAlly);
        }

        public static void TargetCloseEnemy()
        {
            ClosestTarget((int) TargetType.Enemy);
        }

        public static void TargetCloseEnemyHumanoid()
        {
            ClosestHumanoidTarget((int) TargetType.Enemy);
        }

        public static void TargetCloseEnemyMonster()
        {
            ClosestMonsterTarget((int) TargetType.Enemy);
        }

        public static void TargetCloseRed()
        {
            ClosestTarget((int) TargetType.Murderer);
        }

        public static void TargetCloseRedMonster()
        {
            ClosestMonsterTarget((int) TargetType.Murderer);
        }

        public static void TargetCloseRedHumanoid()
        {
            ClosestHumanoidTarget((int) TargetType.Murderer);
        }

        public static void TargetCloseGrey()
        {
            ClosestTarget((int) TargetType.Attackable, (int) TargetType.Criminal);
        }

        public static void TargetCloseGreyMonster()
        {
            ClosestMonsterTarget((int) TargetType.Attackable, (int) TargetType.Criminal);
        }

        public static void TargetCloseGreyHumanoid()
        {
            ClosestHumanoidTarget((int) TargetType.Attackable, (int) TargetType.Criminal);
        }

        public static void TargetCloseCriminal()
        {
            ClosestTarget((int) TargetType.Criminal);
        }

        public static void TargetCloseCriminalHumanoid()
        {
            ClosestHumanoidTarget((int) TargetType.Criminal);
        }

        public static void TargetCloseCriminalMonster()
        {
            ClosestHumanoidTarget((int) TargetType.Criminal);
        }

        public static void TargetCloseInnocent()
        {
            ClosestTarget((int) TargetType.Innocent);
        }

        public static void TargetCloseInnocentHumanoid()
        {
            ClosestHumanoidTarget((int) TargetType.Innocent);
        }

        public static void TargetCloseInnocentMonster()
        {
            ClosestMonsterTarget((int) TargetType.Innocent);
        }

        public static void TargetClosestFriend()
        {
            ClosestFriendTarget();
        }

        public static void TargetClosest()
        {
            ClosestTarget();
        }

        private static void ClosestTarget(params int[] noto)
        {
            if (!Client.Instance.AllowBit(FeatureBit.ClosestTargets))
                return;

            var list = new List<Mobile>();
            foreach (var m in World.MobilesInRange(12))
                if ((!FriendsManager.IsFriend(m.Serial) || noto.Length > 0 && noto[0] == 0) &&
                    !m.Blessed && !m.IsGhost && m.Serial != World.Player.Serial &&
                    !TargetFilterManager.IsFilteredTarget(m.Serial) &&
                    Utility.InRange(World.Player.Position, m.Position, Config.GetInt("LTRange")))
                {
                    for (var i = 0; i < noto.Length; i++)
                        if (noto[i] == m.Notoriety)
                        {
                            list.Add(m);
                            break;
                        }

                    if (noto.Length == 0)
                        list.Add(m);
                }

            Mobile closest = null;
            var closestDist = double.MaxValue;

            foreach (var m in list)
            {
                var dist = Utility.DistanceSqrt(m.Position, World.Player.Position);

                if (dist < closestDist || closest == null)
                {
                    closestDist = dist;
                    closest = m;
                }
            }

            if (closest != null)
                SetLastTargetTo(closest);
            else
                World.Player.SendMessage(MsgLevel.Warning, LocString.TargNoOne);
        }

        private static void ClosestHumanoidTarget(params int[] noto)
        {
            if (!Client.Instance.AllowBit(FeatureBit.ClosestTargets))
                return;

            var list = new List<Mobile>();
            foreach (var m in World.MobilesInRange(12))
            {
                if (m.Body != 0x0190 && m.Body != 0x0191 && m.Body != 0x025D && m.Body != 0x025E)
                    continue;

                if ((!FriendsManager.IsFriend(m.Serial) || noto.Length > 0 && noto[0] == 0) &&
                    !m.Blessed && !m.IsGhost && m.Serial != World.Player.Serial &&
                    !TargetFilterManager.IsFilteredTarget(m.Serial) &&
                    Utility.InRange(World.Player.Position, m.Position, Config.GetInt("LTRange")))
                {
                    for (var i = 0; i < noto.Length; i++)
                        if (noto[i] == m.Notoriety)
                        {
                            list.Add(m);
                            break;
                        }

                    if (noto.Length == 0)
                        list.Add(m);
                }
            }

            Mobile closest = null;
            var closestDist = double.MaxValue;

            foreach (var m in list)
            {
                var dist = Utility.DistanceSqrt(m.Position, World.Player.Position);

                if (dist < closestDist || closest == null)
                {
                    closestDist = dist;
                    closest = m;
                }
            }

            if (closest != null)
                SetLastTargetTo(closest);
            else
                World.Player.SendMessage(MsgLevel.Warning, LocString.TargNoOne);
        }

        private static void ClosestMonsterTarget(params int[] noto)
        {
            if (!Client.Instance.AllowBit(FeatureBit.ClosestTargets))
                return;

            var list = new List<Mobile>();
            foreach (var m in World.MobilesInRange(12))
            {
                if (!m.IsMonster)
                    continue;

                if ((!FriendsManager.IsFriend(m.Serial) || noto.Length > 0 && noto[0] == 0) &&
                    !m.Blessed && !m.IsGhost && m.Serial != World.Player.Serial &&
                    !TargetFilterManager.IsFilteredTarget(m.Serial) &&
                    Utility.InRange(World.Player.Position, m.Position, Config.GetInt("LTRange")))
                {
                    for (var i = 0; i < noto.Length; i++)
                        if (noto[i] == m.Notoriety)
                        {
                            list.Add(m);
                            break;
                        }

                    if (noto.Length == 0)
                        list.Add(m);
                }
            }

            Mobile closest = null;
            var closestDist = double.MaxValue;

            foreach (var m in list)
            {
                var dist = Utility.DistanceSqrt(m.Position, World.Player.Position);

                if (dist < closestDist || closest == null)
                {
                    closestDist = dist;
                    closest = m;
                }
            }

            if (closest != null)
                SetLastTargetTo(closest);
            else
                World.Player.SendMessage(MsgLevel.Warning, LocString.TargNoOne);
        }

        private static void ClosestFriendTarget()
        {
            if (!Client.Instance.AllowBit(FeatureBit.ClosestTargets))
                return;

            var list = new List<Mobile>();
            foreach (var m in World.MobilesInRange(12))
                if (FriendsManager.IsFriend(m.Serial) && !m.Blessed && !m.IsGhost && m.Serial != World.Player.Serial &&
                    !TargetFilterManager.IsFilteredTarget(m.Serial) &&
                    Utility.InRange(World.Player.Position, m.Position, Config.GetInt("LTRange")))
                    list.Add(m);

            Mobile closest = null;
            var closestDist = double.MaxValue;

            foreach (var m in list)
            {
                var dist = Utility.DistanceSqrt(m.Position, World.Player.Position);

                if (dist < closestDist || closest == null)
                {
                    closestDist = dist;
                    closest = m;
                }
            }

            if (closest != null)
                SetLastTargetTo(closest);
            else
                World.Player.SendMessage(MsgLevel.Warning, LocString.TargNoOne);
        }
    }
}