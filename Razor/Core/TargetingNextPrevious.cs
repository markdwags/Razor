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

using System.Collections.Generic;
using System.Linq;
using Assistant.Core;
using Assistant.Filters;

namespace Assistant
{
    public partial class Targeting
    {
        /// <summary>
        ///     Index used to keep track of the current Next/Prev target
        /// </summary>
        private static int _nextPrevTargetIndex;

        private static void InitNextPrevTargets()
        {
            //Next targets
            HotKey.Add(HKCategory.Targets, LocString.NextTarget, NextTarget);
            HotKey.Add(HKCategory.Targets, LocString.NextTargetFriend, NextTargetFriend);

            // Next humanoids
            HotKey.Add(HKCategory.Targets, LocString.NextTargetHumanoid, NextTargetHumanoid);

            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetEnemy, LocString.NextTargetEnemyHumanoid,
                NextTargetEnemyHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetCriminal, LocString.NextTargetCriminalHumanoid,
                NextTargetCriminalHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetMurderer, LocString.NextTargetMurdererHumanoid,
                NextTargetMurdererHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetInnocent, LocString.NextTargetInnocentHumanoid,
                NextTargetInnocentHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetFriendly, LocString.NextTargetFriendlyHumanoid,
                NextTargetFriendlyHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetGrey, LocString.NextTargetGreyHumanoid,
                NextTargetGreyHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetNonFriendly, LocString.NextTargetNonFriendlyHumanoid,
                NextTargetNonFriendlyHumanoid);

            // Next monsters
            HotKey.Add(HKCategory.Targets, LocString.NextTargetMonster, NextTargetMonster);

            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetEnemy, LocString.NextTargetEnemyMonster,
                NextTargetEnemyMonster);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetCriminal, LocString.NextTargetCriminalMonster,
                NextTargetCriminalMonster);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetMurderer, LocString.NextTargetMurdererMonster,
                NextTargetMurdererMonster);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetInnocent, LocString.NextTargetInnocentMonster,
                NextTargetInnocentMonster);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetFriendly, LocString.NextTargetFriendlyMonster,
                NextTargetFriendlyMonster);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetGrey, LocString.NextTargetGreyMonster,
                NextTargetGreyMonster);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetNonFriendly, LocString.NextTargetNonFriendlyMonster,
                NextTargetNonFriendlyMonster);

            //Previous targets
            HotKey.Add(HKCategory.Targets, LocString.PrevTarget, PrevTarget);

            HotKey.Add(HKCategory.Targets, LocString.PrevTargetHumanoid, PrevTargetHumanoid);
            HotKey.Add(HKCategory.Targets, LocString.PrevTargetMonster, PrevTargetMonster);
            HotKey.Add(HKCategory.Targets, LocString.PrevTargetFriend, PrevTargetFriend);

            // Previous humanoids
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetEnemy, LocString.PrevTargetEnemyHumanoid,
                PrevTargetEnemyHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetFriendly, LocString.PrevTargetFriendlyHumanoid,
                PrevTargetFriendlyHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetGrey, LocString.PrevTargetGreyHumanoid,
                PrevTargetGreyHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetNonFriendly, LocString.PrevTargetNonFriendlyHumanoid,
                PrevTargetNonFriendlyHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetCriminal, LocString.PrevTargetCriminalHumanoid,
                PrevTargetCriminalHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetMurderer, LocString.PrevTargetMurdererHumanoid,
                PrevTargetMurdererHumanoid);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetInnocent, LocString.PrevTargetInnocentHumanoid,
                PrevTargetInnocentHumanoid);

            // Previous monsters
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetEnemy, LocString.PrevTargetEnemyMonster,
                PrevTargetEnemyMonster);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetFriendly, LocString.PrevTargetFriendlyMonster,
                PrevTargetFriendlyMonster);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetGrey, LocString.PrevTargetGreyMonster,
                PrevTargetGreyMonster);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetNonFriendly, LocString.PrevTargetNonFriendlyMonster,
                PrevTargetNonFriendlyMonster);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetCriminal, LocString.PrevTargetCriminalMonster,
                PrevTargetCriminalMonster);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetMurderer, LocString.PrevTargetMurdererMonster,
                PrevTargetMurdererMonster);
            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetInnocent, LocString.PrevTargetInnocentMonster,
                PrevTargetInnocentMonster);
        }

        private static bool IsNextPrevFriend(Mobile mobile)
        {
            return Config.GetBool("NextPrevTargetIgnoresFriends") && FriendsManager.IsFriend(mobile.Serial);
        }

        /// <summary>
        ///     Handles the common Next/Prev logic based on a list of targets passed in already filtered by the calling
        ///     functions conditions.
        /// </summary>
        /// <param name="targets">The list of targets (already filtered)</param>
        /// <param name="nextTarget">next target true, previous target false</param>
        /// <param name="isFriend">Indicates this was the Next/Prev Friend hot key</param>
        /// <param name="isFriendly"></param>
        /// <param name="isNonFriendly"></param>
        private static void NextPrevTarget(List<Mobile> targets, bool nextTarget, bool isFriend = false,
            bool isFriendly = false, bool isNonFriendly = false)
        {
            if (targets.Count <= 0)
            {
                World.Player.SendMessage(MsgLevel.Warning, LocString.TargNoOne);
                return;
            }

            Mobile mobile = null, old = World.FindMobile(m_LastTarget?.Serial ?? Serial.Zero);
            var target = new TargetInfo();

            // Loop through 3 times and break out if you can't get a target for some reason
            for (var i = 0; i < 3; i++)
            {
                if (nextTarget)
                {
                    _nextPrevTargetIndex++;

                    if (_nextPrevTargetIndex >= targets.Count)
                        _nextPrevTargetIndex = 0;
                }
                else
                {
                    _nextPrevTargetIndex--;

                    if (_nextPrevTargetIndex < 0)
                        _nextPrevTargetIndex = targets.Count - 1;
                }

                try
                {
                    mobile = targets[_nextPrevTargetIndex];
                }
                catch
                {
                    mobile = null;
                }

                if (mobile != null && mobile != World.Player && mobile != old) break;

                mobile = null;
            }

            if (mobile == null)
                mobile = old;

            if (mobile == null)
            {
                World.Player.SendMessage(MsgLevel.Warning, LocString.TargNoOne);
                return;
            }

            m_LastGroundTarg = m_LastTarget = target;

            if (IsSmartTargetingEnabled())
            {
                if ((Config.GetBool("OnlyNextPrevBeneficial") && isFriend) ||
                    (Config.GetBool("FriendlyBeneficialOnly") && isFriendly))
                {
                    m_LastBeneTarg = target;
                }
                else if ((Config.GetBool("OnlyNextPrevBeneficial") && !isFriend) ||
                         (Config.GetBool("NonFriendlyHarmfulOnly") && isNonFriendly))
                {
                    m_LastHarmTarg = target;
                }
                else
                {
                    m_LastBeneTarg = m_LastHarmTarg = target;
                }
            }
            else
            {
                m_LastBeneTarg = m_LastHarmTarg = target;
            }

            if (m_HasTarget)
                target.Flags = m_CurFlags;
            else
                target.Type = 0;

            target.Gfx = mobile.Body;
            target.Serial = mobile.Serial;
            target.X = mobile.Position.X;
            target.Y = mobile.Position.Y;
            target.Z = mobile.Position.Z;

            if (!isFriend || Config.GetBool("HighlightFriend"))
            {
                Client.Instance.SendToClient(new ChangeCombatant(mobile));
                m_LastCombatant = mobile.Serial;
            }

            World.Player.SendMessage(MsgLevel.Force, LocString.NewTargSet);

            OverheadTargetMessage(target);
        }

        private static void NextTarget()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => !IsNextPrevFriend(x) && !TargetFilterManager.IsFilteredTarget(x.Serial)).ToList();

            NextPrevTarget(mobiles, true);
        }

        private static void NextTargetHumanoid()
        {
            var mobiles = World.MobilesInRange().Where(x =>
                    x.IsHuman && !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                    !TargetFilterManager.IsFilteredTarget(x.Serial))
                .ToList();

            NextPrevTarget(mobiles, true);
        }

        private static void NextTargetEnemyHumanoid()
        {
            var mobiles = World.MobilesInRange().Where(x =>
                    x.IsHuman && x.Notoriety == (int) TargetType.Enemy && !IsNextPrevFriend(x) && !x.Blessed &&
                    !x.IsGhost && x.Serial != World.Player.Serial && !TargetFilterManager.IsFilteredTarget(x.Serial))
                .ToList();

            NextPrevTarget(mobiles, true);
        }

        private static void NextTargetEnemyMonster()
        {
            var mobiles = World.MobilesInRange().Where(x =>
                    x.IsMonster && x.Notoriety == (int) TargetType.Enemy && !IsNextPrevFriend(x) && !x.Blessed &&
                    !x.IsGhost && x.Serial != World.Player.Serial && !TargetFilterManager.IsFilteredTarget(x.Serial))
                .ToList();

            NextPrevTarget(mobiles, true);
        }

        private static void NextTargetMonster()
        {
            var mobiles = World.MobilesInRange().Where(x =>
                    x.IsMonster && !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost &&
                    x.Serial != World.Player.Serial && !TargetFilterManager.IsFilteredTarget(x.Serial))
                .ToList();

            NextPrevTarget(mobiles, true);
        }

        private static void NextTargetFriend()
        {
            var mobiles = World.MobilesInRange().Where(x =>
                    FriendsManager.IsFriend(x.Serial) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial)
                .ToList();

            NextPrevTarget(mobiles, true, true);
        }

        private static void PrevTarget()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => !IsNextPrevFriend(x) && !TargetFilterManager.IsFilteredTarget(x.Serial)).ToList();

            NextPrevTarget(mobiles, false);
        }

        private static void PrevTargetHumanoid()
        {
            var mobiles = World.MobilesInRange().Where(x =>
                    x.IsHuman && !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                    !TargetFilterManager.IsFilteredTarget(x.Serial))
                .ToList();

            NextPrevTarget(mobiles, false);
        }

        private static void PrevTargetEnemyHumanoid()
        {
            var mobiles = World.MobilesInRange().Where(x =>
                    x.IsHuman && x.Notoriety == (int) TargetType.Enemy && !IsNextPrevFriend(x) && !x.Blessed &&
                    !x.IsGhost && x.Serial != World.Player.Serial && !TargetFilterManager.IsFilteredTarget(x.Serial))
                .ToList();

            NextPrevTarget(mobiles, false);
        }

        private static void PrevTargetEnemyMonster()
        {
            var mobiles = World.MobilesInRange().Where(x =>
                    x.IsMonster && x.Notoriety == (int) TargetType.Enemy && !IsNextPrevFriend(x) && !x.Blessed &&
                    !x.IsGhost && x.Serial != World.Player.Serial && !TargetFilterManager.IsFilteredTarget(x.Serial))
                .ToList();

            NextPrevTarget(mobiles, false);
        }

        private static void PrevTargetMonster()
        {
            var mobiles = World.MobilesInRange().Where(x =>
                    x.IsMonster && !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost &&
                    x.Serial != World.Player.Serial && !TargetFilterManager.IsFilteredTarget(x.Serial))
                .ToList();

            NextPrevTarget(mobiles, false);
        }

        private static void PrevTargetFriend()
        {
            var mobiles = World.MobilesInRange().Where(x =>
                FriendsManager.IsFriend(x.Serial) && !x.Blessed && !x.IsGhost &&
                x.Serial != World.Player.Serial).ToList();

            NextPrevTarget(mobiles, false, true);
        }

        private static void NextTargetCriminalHumanoid()
        {
            var mobiles = World.MobilesInRange().Where(x =>
                    x.IsHuman && x.Notoriety == (int) TargetType.Criminal && !IsNextPrevFriend(x) && !x.Blessed &&
                    !x.IsGhost && x.Serial != World.Player.Serial && !TargetFilterManager.IsFilteredTarget(x.Serial))
                .ToList();

            NextPrevTarget(mobiles, true);
        }

        private static void NextTargetMurdererHumanoid()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && x.Notoriety == (int) TargetType.Murderer && !IsNextPrevFriend(x) &&
                            !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial)).ToList();

            NextPrevTarget(mobiles, true);
        }

        private static void NextTargetInnocentHumanoid()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && x.Notoriety == (int) TargetType.Innocent && !IsNextPrevFriend(x) &&
                            !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial)).ToList();

            NextPrevTarget(mobiles, true);
        }

        private static void NextTargetCriminalMonster()
        {
            var mobiles = World.MobilesInRange().Where(x =>
                    x.IsMonster && x.Notoriety == (int) TargetType.Criminal && !IsNextPrevFriend(x) && !x.Blessed &&
                    !x.IsGhost && x.Serial != World.Player.Serial && !TargetFilterManager.IsFilteredTarget(x.Serial))
                .ToList();

            NextPrevTarget(mobiles, true);
        }

        private static void NextTargetMurdererMonster()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsMonster && x.Notoriety == (int) TargetType.Murderer && !IsNextPrevFriend(x) &&
                            !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial)).ToList();

            NextPrevTarget(mobiles, true);
        }

        private static void NextTargetInnocentMonster()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsMonster && x.Notoriety == (int) TargetType.Innocent && !IsNextPrevFriend(x) &&
                            !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial)).ToList();

            NextPrevTarget(mobiles, true);
        }

        private static void PrevTargetCriminalHumanoid()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && x.Notoriety == (int) TargetType.Criminal && !IsNextPrevFriend(x) &&
                            !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial)).ToList();

            NextPrevTarget(mobiles, false);
        }

        private static void PrevTargetCriminalMonster()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsMonster && x.Notoriety == (int) TargetType.Criminal && !IsNextPrevFriend(x) &&
                            !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial)).ToList();

            NextPrevTarget(mobiles, false);
        }

        private static void PrevTargetMurdererHumanoid()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && x.Notoriety == (int) TargetType.Murderer && !IsNextPrevFriend(x) &&
                            !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial)).ToList();

            NextPrevTarget(mobiles, false);
        }

        private static void PrevTargetMurdererMonster()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsMonster && x.Notoriety == (int) TargetType.Murderer && !IsNextPrevFriend(x) &&
                            !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial)).ToList();

            NextPrevTarget(mobiles, false);
        }

        private static void PrevTargetInnocentHumanoid()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && x.Notoriety == (int) TargetType.Innocent && !IsNextPrevFriend(x) &&
                            !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial)).ToList();

            NextPrevTarget(mobiles, false);
        }

        private static void PrevTargetInnocentMonster()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsMonster && x.Notoriety == (int) TargetType.Innocent && !IsNextPrevFriend(x) &&
                            !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial)).ToList();

            NextPrevTarget(mobiles, false);
        }

        private static void NextTargetFriendlyHumanoid()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && (x.Notoriety == (int) TargetType.Innocent ||
                                          x.Notoriety == (int) TargetType.Invalid ||
                                          x.Notoriety == (int) TargetType.GuildAlly) &&
                            !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial))
                .ToList();

            NextPrevTarget(mobiles, true, false, true);
        }

        private static void NextTargetFriendlyMonster()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsMonster && (x.Notoriety == (int) TargetType.Innocent ||
                                            x.Notoriety == (int) TargetType.Invalid ||
                                            x.Notoriety == (int) TargetType.GuildAlly) &&
                            !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial))
                .ToList();

            NextPrevTarget(mobiles, true, false, true);
        }

        private static void NextTargetGreyHumanoid()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && (x.Notoriety == (int) TargetType.Attackable ||
                                          x.Notoriety == (int) TargetType.Criminal) &&
                            !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial))
                .ToList();

            NextPrevTarget(mobiles, true);
        }

        private static void NextTargetGreyMonster()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsMonster && (x.Notoriety == (int) TargetType.Attackable ||
                                            x.Notoriety == (int) TargetType.Criminal) &&
                            !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial))
                .ToList();

            NextPrevTarget(mobiles, true);
        }

        private static void NextTargetNonFriendlyHumanoid()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && (x.Notoriety == (int) TargetType.Attackable ||
                                          x.Notoriety == (int) TargetType.Criminal ||
                                          x.Notoriety == (int) TargetType.Enemy ||
                                          x.Notoriety == (int) TargetType.Murderer) &&
                            !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial)
                ).ToList();

            NextPrevTarget(mobiles, true, false, false, true);
        }

        private static void NextTargetNonFriendlyMonster()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsMonster && (x.Notoriety == (int) TargetType.Attackable ||
                                            x.Notoriety == (int) TargetType.Criminal ||
                                            x.Notoriety == (int) TargetType.Enemy ||
                                            x.Notoriety == (int) TargetType.Murderer) &&
                            !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial)
                ).ToList();

            NextPrevTarget(mobiles, true, false, false, true);
        }

        private static void PrevTargetFriendlyHumanoid()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && (x.Notoriety == (int) TargetType.Innocent ||
                                          x.Notoriety == (int) TargetType.Invalid ||
                                          x.Notoriety == (int) TargetType.GuildAlly) &&
                            !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial))
                .ToList();

            NextPrevTarget(mobiles, false, false, true);
        }

        private static void PrevTargetFriendlyMonster()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsMonster && (x.Notoriety == (int) TargetType.Innocent ||
                                            x.Notoriety == (int) TargetType.Invalid ||
                                            x.Notoriety == (int) TargetType.GuildAlly) &&
                            !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial))
                .ToList();

            NextPrevTarget(mobiles, false, false, true);
        }

        private static void PrevTargetGreyHumanoid()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && (x.Notoriety == (int) TargetType.Attackable ||
                                          x.Notoriety == (int) TargetType.Criminal) &&
                            !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial))
                .ToList();

            NextPrevTarget(mobiles, false);
        }

        private static void PrevTargetGreyMonster()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsMonster && (x.Notoriety == (int) TargetType.Attackable ||
                                            x.Notoriety == (int) TargetType.Criminal) &&
                            !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial))
                .ToList();

            NextPrevTarget(mobiles, false);
        }

        private static void PrevTargetNonFriendlyHumanoid()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && (x.Notoriety == (int) TargetType.Attackable ||
                                          x.Notoriety == (int) TargetType.Criminal ||
                                          x.Notoriety == (int) TargetType.Enemy ||
                                          x.Notoriety == (int) TargetType.Murderer) &&
                            !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial))
                .ToList();

            NextPrevTarget(mobiles, false, false, false, true);
        }

        private static void PrevTargetNonFriendlyMonster()
        {
            var mobiles = World.MobilesInRange()
                .Where(x => x.IsMonster && (x.Notoriety == (int) TargetType.Attackable ||
                                            x.Notoriety == (int) TargetType.Criminal ||
                                            x.Notoriety == (int) TargetType.Enemy ||
                                            x.Notoriety == (int) TargetType.Murderer) &&
                            !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial &&
                            !TargetFilterManager.IsFilteredTarget(x.Serial))
                .ToList();

            NextPrevTarget(mobiles, false, false, false, true);
        }
    }
}