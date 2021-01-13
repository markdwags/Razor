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
        private static void InitRandomTarget()
        {
            HotKey.Add(HKCategory.Targets, LocString.TargetRandom, TargetRandAnyone);

            HotKey.Add(HKCategory.Targets, LocString.TargRandomFriend, TargetRandFriend);

            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetMurderer, LocString.TargRandRed,
                TargetRandRed);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetMurderer, LocString.TargRandomRedHuman,
                TargetRandRedHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetMurderer, LocString.TargRandomRedMonster,
                TargetRandRedMonster);

            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetNonFriendly, LocString.TargRandNFriend,
                TargetRandNonFriendly);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetNonFriendly, LocString.TargRandomNFriendlyHuman,
                TargetRandNonFriendlyHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetNonFriendly, LocString.TargRandomNFriendlyMonster,
                TargetRandNonFriendlyMonster);

            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetFriendly, LocString.TargRandFriend,
                TargetRandFriendly);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetFriendly, LocString.TargRandomFriendlyHuman,
                TargetRandFriendlyHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetFriendly, LocString.TargRandomFriendlyMonster,
                TargetRandFriendlyMonster);

            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetInnocent, LocString.TargRandBlue,
                TargetRandInnocent);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetInnocent, LocString.TargRandInnocentHuman,
                TargetRandInnocentHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetInnocent, LocString.TargRandomInnocentMonster,
                TargetRandInnocentMonster);

            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetGrey, LocString.TargRandGrey, TargetRandGrey);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetGrey, LocString.TargRandGreyHuman,
                TargetRandGreyHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetGrey, LocString.TargRandGreyMonster,
                TargetRandGreyMonster);

            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetEnemy, LocString.TargRandEnemy,
                TargetRandEnemy);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetEnemy, LocString.TargRandEnemyHuman,
                TargetRandEnemyHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetEnemy, LocString.TargRandEnemyMonster,
                TargetRandEnemyMonster);

            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetCriminal, LocString.TargRandCriminal,
                TargetRandCriminal);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetCriminal, LocString.TargRandCriminalHuman,
                TargetRandCriminalHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetCriminal, LocString.TargRandomCriminalMonster,
                TargetRandCriminalMonster);
        }


        private static void TargetRandNonFriendly()
        {
            RandomTarget((int) TargetType.Attackable, (int) TargetType.Criminal, (int) TargetType.Enemy,
                (int) TargetType.Murderer);
        }

        private static void TargetRandNonFriendlyHumanoid()
        {
            RandomHumanoidTarget((int) TargetType.Attackable, (int) TargetType.Criminal, (int) TargetType.Enemy,
                (int) TargetType.Murderer);
        }

        private static void TargetRandNonFriendlyMonster()
        {
            RandomMonsterTarget((int) TargetType.Attackable, (int) TargetType.Criminal, (int) TargetType.Enemy,
                (int) TargetType.Murderer);
        }

        private static void TargetRandFriendly()
        {
            RandomTarget((int) TargetType.Invalid, (int) TargetType.Innocent, (int) TargetType.GuildAlly);
        }

        private static void TargetRandFriendlyHumanoid()
        {
            RandomHumanoidTarget((int) TargetType.Invalid, (int) TargetType.Innocent, (int) TargetType.GuildAlly);
        }

        private static void TargetRandFriendlyMonster()
        {
            RandomMonsterTarget((int) TargetType.Invalid, (int) TargetType.Innocent, (int) TargetType.GuildAlly);
        }

        private static void TargetRandEnemy()
        {
            RandomTarget((int) TargetType.Enemy);
        }

        private static void TargetRandEnemyMonster()
        {
            RandomMonsterTarget((int) TargetType.Enemy);
        }

        private static void TargetRandEnemyHumanoid()
        {
            RandomHumanoidTarget((int) TargetType.Enemy);
        }

        private static void TargetRandRed()
        {
            RandomTarget((int) TargetType.Murderer);
        }

        private static void TargetRandRedHumanoid()
        {
            RandomHumanoidTarget((int) TargetType.Murderer);
        }

        private static void TargetRandRedMonster()
        {
            RandomMonsterTarget((int) TargetType.Murderer);
        }

        private static void TargetRandGrey()
        {
            RandomTarget((int) TargetType.Attackable, (int) TargetType.Criminal);
        }

        private static void TargetRandGreyMonster()
        {
            RandomMonsterTarget((int) TargetType.Attackable, (int) TargetType.Criminal);
        }

        private static void TargetRandGreyHumanoid()
        {
            RandomHumanoidTarget((int) TargetType.Attackable, (int) TargetType.Criminal);
        }

        private static void TargetRandCriminal()
        {
            RandomTarget((int) TargetType.Criminal);
        }

        private static void TargetRandCriminalHumanoid()
        {
            RandomHumanoidTarget((int) TargetType.Criminal);
        }

        private static void TargetRandCriminalMonster()
        {
            RandomMonsterTarget((int) TargetType.Criminal);
        }

        private static void TargetRandInnocent()
        {
            RandomTarget((int) TargetType.Innocent);
        }

        private static void TargetRandInnocentHumanoid()
        {
            RandomHumanoidTarget((int) TargetType.Innocent);
        }

        private static void TargetRandInnocentMonster()
        {
            RandomMonsterTarget((int) TargetType.Innocent);
        }

        private static void TargetRandFriend()
        {
            RandomFriendTarget();
        }

        private static void TargetRandAnyone()
        {
            RandomTarget();
        }

        private static void RandomTarget(params int[] noto)
        {
            if (!Client.Instance.AllowBit(FeatureBit.RandomTargets))
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

            if (list.Count > 0)
                SetLastTargetTo(list[Utility.Random(list.Count)]);
            else
                World.Player.SendMessage(MsgLevel.Warning, LocString.TargNoOne);
        }

        private static void RandomHumanoidTarget(params int[] noto)
        {
            if (!Client.Instance.AllowBit(FeatureBit.RandomTargets))
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

            if (list.Count > 0)
                SetLastTargetTo(list[Utility.Random(list.Count)]);
            else
                World.Player.SendMessage(MsgLevel.Warning, LocString.TargNoOne);
        }

        private static void RandomMonsterTarget(params int[] noto)
        {
            if (!Client.Instance.AllowBit(FeatureBit.RandomTargets))
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

            if (list.Count > 0)
                SetLastTargetTo(list[Utility.Random(list.Count)]);
            else
                World.Player.SendMessage(MsgLevel.Warning, LocString.TargNoOne);
        }

        private static void RandomFriendTarget()
        {
            if (!Client.Instance.AllowBit(FeatureBit.RandomTargets))
                return;

            var list = new List<Mobile>();
            foreach (var m in World.MobilesInRange(12))
                if (FriendsManager.IsFriend(m.Serial) &&
                    !m.Blessed && !m.IsGhost && m.Serial != World.Player.Serial &&
                    !TargetFilterManager.IsFilteredTarget(m.Serial) &&
                    Utility.InRange(World.Player.Position, m.Position, Config.GetInt("LTRange")))
                    list.Add(m);

            if (list.Count > 0)
                SetLastTargetTo(list[Utility.Random(list.Count)]);
            else
                World.Player.SendMessage(MsgLevel.Warning, LocString.TargNoOne);
        }
    }
}