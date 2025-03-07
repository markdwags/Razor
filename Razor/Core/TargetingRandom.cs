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


        public static void TargetRandNonFriendly()
        {
            RandomTarget((int) TargetType.Attackable, (int) TargetType.Criminal, (int) TargetType.Enemy,
                (int) TargetType.Murderer);
        }

        public static void TargetRandNonFriendlyHumanoid()
        {
            RandomHumanoidTarget((int) TargetType.Attackable, (int) TargetType.Criminal, (int) TargetType.Enemy,
                (int) TargetType.Murderer);
        }

        public static void TargetRandNonFriendlyMonster()
        {
            RandomMonsterTarget((int) TargetType.Attackable, (int) TargetType.Criminal, (int) TargetType.Enemy,
                (int) TargetType.Murderer);
        }

        public static void TargetRandFriendly()
        {
            RandomTarget((int) TargetType.Invalid, (int) TargetType.Innocent, (int) TargetType.GuildAlly);
        }

        public static void TargetRandFriendlyHumanoid()
        {
            RandomHumanoidTarget((int) TargetType.Invalid, (int) TargetType.Innocent, (int) TargetType.GuildAlly);
        }

        public static void TargetRandFriendlyMonster()
        {
            RandomMonsterTarget((int) TargetType.Invalid, (int) TargetType.Innocent, (int) TargetType.GuildAlly);
        }

        public static void TargetRandEnemy()
        {
            RandomTarget((int) TargetType.Enemy);
        }

        public static void TargetRandEnemyMonster()
        {
            RandomMonsterTarget((int) TargetType.Enemy);
        }

        public static void TargetRandEnemyHumanoid()
        {
            RandomHumanoidTarget((int) TargetType.Enemy);
        }

        public static void TargetRandRed()
        {
            RandomTarget((int) TargetType.Murderer);
        }

        public static void TargetRandRedHumanoid()
        {
            RandomHumanoidTarget((int) TargetType.Murderer);
        }

        public static void TargetRandRedMonster()
        {
            RandomMonsterTarget((int) TargetType.Murderer);
        }

        public static void TargetRandGrey()
        {
            RandomTarget((int) TargetType.Attackable, (int) TargetType.Criminal);
        }

        public static void TargetRandGreyMonster()
        {
            RandomMonsterTarget((int) TargetType.Attackable, (int) TargetType.Criminal);
        }

        public static void TargetRandGreyHumanoid()
        {
            RandomHumanoidTarget((int) TargetType.Attackable, (int) TargetType.Criminal);
        }

        public static void TargetRandCriminal()
        {
            RandomTarget((int) TargetType.Criminal);
        }

        public static void TargetRandCriminalHumanoid()
        {
            RandomHumanoidTarget((int) TargetType.Criminal);
        }

        public static void TargetRandCriminalMonster()
        {
            RandomMonsterTarget((int) TargetType.Criminal);
        }

        public static void TargetRandInnocent()
        {
            RandomTarget((int) TargetType.Innocent);
        }

        public static void TargetRandInnocentHumanoid()
        {
            RandomHumanoidTarget((int) TargetType.Innocent);
        }

        public static void TargetRandInnocentMonster()
        {
            RandomMonsterTarget((int) TargetType.Innocent);
        }

        public static void TargetRandFriend()
        {
            RandomFriendTarget();
        }

        public static void TargetRandAnyone()
        {
            RandomTarget();
        }
    }
}