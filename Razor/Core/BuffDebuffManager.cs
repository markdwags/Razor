#region license
// Razor: An Ultima Online Assistant
// Copyright (c) 2022 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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
using System.Linq;
using Assistant.Agents;
using Assistant.Gumps.Internal;

namespace Assistant.Core
{
    public enum BuffIconType : ushort
    {
        DismountPrevention = 0x3E9,
        NoRearm = 0x3EA,
        
        //Currently, no 0x3EB or 0x3EC
        NightSight = 0x3ED, 
        DeathStrike,
        EvilOmen,
        HonoredDebuff,
        AchievePerfection,
        DivineFury,         
        EnemyOfOne,         
        HidingAndOrStealth,
        ActiveMeditation,
        BloodOathCaster,
        BloodOathCurse,
        CorpseSkin,
        Mindrot,
        PainSpike,
        Strangle,
        GiftOfRenewal,
        AttuneWeapon,
        Thunderstorm,
        EssenceOfWind,
        EtherealVoyage,
        GiftOfLife,
        ArcaneEmpowerment,
        MortalStrike,
        ReactiveArmor,
        Protection,
        ArchProtection,
        MagicReflection,
        Incognito,
        Disguised,
        AnimalForm,
        Polymorph,
        Invisibility,
        Paralyze, 
        Poison,
        Bleed,
        Clumsy, 
        FeebleMind,
        Weaken,  
        Curse,   
        MassCurse,
        Agility, 
        Cunning, 
        Strength, 
        Bless,   
        Sleep,
        StoneForm,
        SpellPlague,
        Berserk,
        MassSleep,
        Fly,
        Inspire,
        Invigorate,
        Resilience,
        Perseverance,
        TribulationTarget,
        DespairTarget,
        FishPie = 0x426,
        HitLowerAttack,
        HitLowerDefense,
        DualWield,
        Block,
        DefenseMastery,
        DespairCaster,
        Healing,
        SpellFocusingBuff,
        SpellFocusingDebuff,
        RageFocusingDebuff,
        RageFocusingBuff,
        Warding,
        TribulationCaster,
        ForceArrow,
        Disarm,
        Surge,
        Feint,
        TalonStrike,
        PsychicAttack,
        ConsecrateWeapon,
        GrapesOfWrath,
        EnemyOfOneDebuff,
        HorrificBeast,
        LichForm,
        VampiricEmbrace,
        CurseWeapon,
        ReaperForm,
        ImmolatingWeapon,
        Enchant,
        HonorableExecution,
        Confidence,
        Evasion,
        CounterAttack,
        LightningStrike,
        MomentumStrike,
        OrangePetals,
        RoseOfTrinsic,
        PoisonImmunity,
        Veterinary,
        Perfection,
        Honored,
        ManaPhase,
        FanDancerFanFire,
        Rage,
        Webbing,
        MedusaStone,
        TrueFear,
        AuraOfNausea,
        HowlOfCacophony,
        GazeDespair,
        HiryuPhysicalResistance,
        RuneBeetleCorruption,
        BloodwormAnemia,
        RotwormBloodDisease,
        SkillUseDelay,
        FactionStatLoss,
        HeatOfBattleStatus,
        CriminalStatus,
        ArmorPierce,
        SplinteringEffect,
        SwingSpeedDebuff,
        WraithForm,
        CityTradeDeal = 0x466,
        HumilityDebuff = 0x467,
        Spirituality,
        Humility,
        // Skill Masteries
        Rampage,
        Stagger, // Debuff
        Toughness,
        Thrust,
        Pierce, // Debuff
        PlayingTheOdds,
        FocusedEye,
        Onslaught, // Debuff
        ElementalFury,
        ElementalFuryDebuff, // Debuff
        CalledShot,
        Knockout,
        SavingThrow,
        Conduit,
        EtherealBurst,
        MysticWeapon,
        ManaShield,
        AnticipateHit,
        Warcry,
        Shadow,
        WhiteTigerForm,
        Bodyguard,
        HeightenedSenses,
        Tolerance,
        DeathRay,
        DeathRayDebuff,
        Intuition,
        EnchantedSummoning,
        ShieldBash,
        Whispering,
        CombatTraining,
        InjectedStrikeDebuff,
        InjectedStrike,
        UnknownTomato,
        PlayingTheOddsDebuff,
        DragonTurtleDebuff,
        Boarding,
        Potency,
        ThrustDebuff,
        FistsOfFury,
        BarrabHemolymphConcentrate,
        JukariBurnPoiltice,
        KurakAmbushersEssence,
        BarakoDraftOfMight,
        UraliTranceTonic,
        SakkhraProphylaxis,
        Sparks,
        Swarm,
        BoneBreaker,
        Unknown2,
        SwarmImmune,
        BoneBreakerImmune,
        UnknownGoblin,
        UnknownRedDrop,
        UnknownStar,
        FeintDebuff,
        CaddelliteInfused,
        PotionGloriousFortune,
        MysticalPolymorphTotem,
        UnknownDebuff
    }

    public class BuffDebuff
    {
        public int IconNumber { get; set; }
        public int IconId { get; set; }
        public int Duration { get; set; }
        public string ClilocMessage1 { get; set; }
        public string ClilocMessage2 { get; set; }
        public BuffIconType BuffIconType { get; set; }
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            return ClilocMessage1;
        }
    }

    public static class BuffDebuffManager
    {
        private static Timer BuffDebuffTimer { get; set; } = new InternalTimer();

        private static List<string> _buffDebuffFilter = new List<string>();

        private static BuffGump _gump;

        public static ushort[] BuffTable { get; } =
        {
            0x754C,
            0x754A,
            0x0000,
            0x0000,
            0x755E,
            0x7549,
            0x7551,
            0x7556,
            0x753A,
            0x754D,
            0x754E,
            0x7565,
            0x753B,
            0x7543,
            0x7544,
            0x7546,
            0x755C,
            0x755F,
            0x7566,
            0x7554,
            0x7540,
            0x7568,
            0x754F,
            0x7550,
            0x7553,
            0x753E,
            0x755D,
            0x7563,
            0x7562,
            0x753F,
            0x7559,
            0x7557,
            0x754B,
            0x753D,
            0x7561,
            0x7558,
            0x755B,
            0x7560,
            0x7541,
            0x7545,
            0x7552,
            0x7569,
            0x7548,
            0x755A,
            0x753C,
            0x7547,
            0x7567,
            0x7542,
            0x758A,
            0x758B,
            0x758C,
            0x758D,
            0x0000,
            0x758E,
            0x094B,
            0x094C,
            0x094D,
            0x094E,
            0x094F,
            0x0950,
            0x753E,
            0x5011,
            0x7590,
            0x7591,
            0x7592,
            0x7593,
            0x7594,
            0x7595,
            0x7596,
            0x7598,
            0x7599,
            0x759B,
            0x759C,
            0x759E,
            0x759F,
            0x75A0,
            0x75A1,
            0x75A3,
            0x75A4,
            0x75A5,
            0x75A6,
            0x75A7,
            0x75C0,
            0x75C1,
            0x75C2,
            0x75C3,
            0x75C4,
            0x75F2,
            0x75F3,
            0x75F4,
            0x75F5,
            0x75F6,
            0x75F7,
            0x75F8,
            0x75F9,
            0x75FA,
            0x75FB,
            0x75FC,
            0x75FD,
            0x75FE,
            0x75FF,
            0x7600,
            0x7601,
            0x7602,
            0x7603,
            0x7604,
            0x7605,
            0x7606,
            0x7607,
            0x7608,
            0x7609,
            0x760A,
            0x760B,
            0x760C,
            0x760D,
            0x760E,
            0x760F,
            0x7610,
            0x7611,
            0x7612,
            0x7613,
            0x7614,
            0x7615,
            0x75C5,
            0x75F6,
            0x761B,
            // skill masteries
            0x9bc9,
            0x9bb5,
            0x9bdd,
            0x9bc6,
            0x9bcc,
            0x9bbe,
            0x9bbd,
            0x9bcb,
            0x9bc8,
            0x9bbf,
            0x9bcd,
            0x9bc0,
            0x9bce,
            0x9bc1,
            0x9bc7,
            0x9bc2,
            0x9bb7,
            0x9bca,
            0x9bb6,
            0x9bb8,
            0x9bb9,
            0x9bba,
            0x9bbb,
            0x9bbc,
            0x9bc3,
            0x9bc4,
            0x9bc5,
            0x9bd2,
            0x9bd3,
            0x9bd4,
            0x9bd5,
            0x9bd1,
            0x9bd6,
            0x9bd7,
            0x9bcf,
            0x9bd8,
            0x9bd9,
            0x9bdb,
            0x9bdc,
            0x9bda,
            0x9bd0,
            0x9bde,
            0x9bdf,

            0xC349,
            0xC34D,
            0xC34E,
            0xC34C,
            0xC34B,
            0xC34A,
            0xC343,
            0xC345,
            0xC346,
            0xC347,
            0xC348,

            0x9CDE,

            0x5DE1,
            0x5DDF,
            0x5DE3,
            0x5DE5,
            0x5DE4,
            0x5DE6,
            0x5D51,

            0x0951
        };

        public static void DisplayOverheadBuff(BuffDebuff buff, bool ignoreAction = false)
        {
            if (Config.GetBool("ShowBuffDebuffOverhead") && !IsFiltered(buff.ClilocMessage1))
            {
                TimeSpan diff = DateTime.UtcNow - buff.Timestamp;
                int timeLeft = buff.Duration - (int)diff.TotalSeconds;

                if (Config.GetBool("OverrideBuffDebuffFormat"))
                {
                    string format = Config.GetString("BuffDebuffFormat");

                    if (string.IsNullOrEmpty(format))
                    {
                        format = "[{action}{name} {duration}]";
                    }
                    
                    World.Player.OverheadMessage(Config.GetInt("BuffHue"), format.Replace("{action}", ignoreAction ? string.Empty : "+").Replace("{name}", buff.ClilocMessage1)
                            .Replace("{duration}", timeLeft < 1 ? string.Empty : $"({timeLeft}s)"));
                }
                else
                {
                    World.Player.OverheadMessage(Config.GetInt("BuffHue"), timeLeft < 1 ? $"[+{buff.ClilocMessage1}]" : $"[+{buff.ClilocMessage1} ({timeLeft}s)]");
                }
            }
        }

        public static void DisplayOverheadDebuff(BuffIconType debuffIconType)
        {
            if (Config.GetBool("ShowBuffDebuffOverhead"))
            {
                BuffDebuff debuff = World.Player.BuffsDebuffs.FirstOrDefault(b => b.BuffIconType == debuffIconType);

                if (debuff == null)
                    return;

                if (IsFiltered(debuff.ClilocMessage1))
                    return;

                if (Config.GetBool("OverrideBuffDebuffFormat"))
                {
                    string format = Config.GetString("BuffDebuffFormat");

                    if (string.IsNullOrEmpty(format))
                    {
                        format = "[{action}{name} {duration}]";
                    }
                    
                    World.Player.OverheadMessage(Config.GetInt("DebuffHue"),
                        format.Replace("{action}", "-").Replace("{name}", debuff.ClilocMessage1)
                            .Replace("{duration}", string.Empty));
                }
                else
                {
                    World.Player.OverheadMessage(Config.GetInt("DebuffHue"), $"[-{debuff.ClilocMessage1}]");
                }
            }
        }

        public static void ReloadFilter()
        {
            _buffDebuffFilter.Clear();

            foreach (string filter in Config.GetString("BuffDebuffFilter").Split(','))
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    _buffDebuffFilter.Add(filter);
                }
            }
        }

        private static bool IsFiltered(string name)
        {
            if (string.IsNullOrEmpty(Config.GetString("BuffDebuffFilter")))
                return false;

            if (string.IsNullOrEmpty(name))
                return false;

            foreach (string filter in _buffDebuffFilter)
            {
                if (name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) != -1)
                    return true;
            }

            return false;
        }
        
        public static bool Running => BuffDebuffTimer.Running;

        public static void Start()
        {
            if (BuffDebuffTimer.Running)
                BuffDebuffTimer.Stop();
            
            BuffDebuffTimer.Start();

            Client.Instance.RequestTitlebarUpdate();
        }

        public static void Stop()
        {
            BuffDebuffTimer.Stop();
            
            _gump?.CloseGump();
            
            Client.Instance.RequestTitlebarUpdate();
        }

        private class InternalTimer : Timer
        {
            public InternalTimer() : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
            {
            }

            protected override void OnTick()
            {
                if (World.Player == null)
                {
                    BuffDebuffTimer.Stop();
                    return;
                }

                Client.Instance.RequestTitlebarUpdate();
                
                _gump?.CloseGump();
                
                if (World.Player.BuffsDebuffs.Count > 0)
                {
                    _gump = new BuffGump();
                    _gump.SendGump();    
                }

                if (!Config.GetBool("BuffDebuffEveryXSeconds") || !Config.GetBool("ShowBuffDebuffOverhead"))
                    return;

                foreach (BuffDebuff buffsDebuff in World.Player.BuffsDebuffs)
                {
                    TimeSpan diff = DateTime.UtcNow - buffsDebuff.Timestamp;
                    int timeLeft = buffsDebuff.Duration - (int)diff.TotalSeconds;

                    if (timeLeft % Config.GetInt("BuffDebuffSeconds") == 0)
                    {
                        BuffDebuffManager.DisplayOverheadBuff(buffsDebuff, true);
                    }
                }
            }
        }
    }
}