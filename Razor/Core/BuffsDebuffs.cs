﻿#region license

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

namespace Assistant.Core
{
    public enum BuffIcon : ushort
    {
        DismountPrevention = 0x3E9,
        NoRearm = 0x3EA,

        //Currently, no 0x3EB or 0x3EC
        NightSight = 0x3ED,

        DeathStrike,
        EvilOmen,
        UnknownStandingSwirl,
        UnknownKneelingSword,
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
        Disguised, //*
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
        Sleep = 1049,
        StoneForm = 1050,
        SpellPlague = 1051,
        GargoyleBerserk = 1052,
        GargoyleFly = 1054,
        Inspire = 1055,
        Invigorate = 1056,
        Resilience = 1057,
        Perseverance = 1058,
        TribulationTarget = 1059,
        DespairTarget = 1060,
        ArcaneEmpowermentNew = 1061,
        MagicFish = 1062,
        HitLowerAttack = 1063,
        HitLowerDefense = 1064,
        HitDualwield = 1065,
        Block = 1066,
        DefenseMastery = 1067,
        Despair = 1068,
        HealingSkill = 1069,
        SpellFocusing = 1070,
        SpellFocusingTarget = 1071,
        RageFocusingTarget = 1072,
        RageFocusing = 1073,
        Warding = 1074,
        Tribulation = 1075,
        ForceArrow = 1076,
        DisarmNew = 1077,
        Surge = 1078,
        Feint = 1079,
        TalonStrike = 1080,
        PsychicAttack = 1081,
        ConsecrateWeapon = 1082,
        EnemyOfOneNew = 1084,
        HorrificBeast = 1085,
        LichForm = 1086,
        VampiricEmbrace = 1087,
        CurseWeapon = 1088,
        ReaperForm = 1089,
        ImmolatingWeapon = 1090,
        Enchant = 1091,
        HonorableExecution = 1092,
        Confidence = 1093,
        Evasion = 1094,
        CounterAttack = 1095,
        LightningStrike = 1096,
        MomentumStrike = 1097,
        OrangePetals = 1098,
        RoseOfTrinsic = 1099,
        PoisonResistanceImmunity = 1100,
        Veterinary = 1101,
        Perfection = 1102,
        Honored = 1103,
        ManaPhase = 1104,
        FanDancerFanFire = 1105,
        Rage = 1106,
        Webbing = 1107,
        MedusaStone = 1108,
        DragonSlasherFear = 1109,
        AuraOfNausea = 1110,
        HowlOfCacophony = 1111,
        GazeDespair = 1112,
        HiryuPhysicalResistance = 1113,
        RuneBeetleCorruption = 1114,
        BloodwormAnemia = 1115,
        RotwormBloodDisease = 1116,
        SkillUseDelay = 1117,
        FactionLoss = 1118,
        HeatOfBattleStatus = 1119,
        CriminalStatus = 1120,
        ArmorPierce = 1121,
        SplinteringEffect = 1122,
        SwingSpeed = 1123,
        WraithForm = 1124,
        CityTradeDeal = 1126
    }

    public class BuffsDebuffs
    {
        public int IconNumber { get; set; }
        public int Duration { get; set; }
        public string ClilocMessage1 { get; set; }
        public string ClilocMessage2 { get; set; }
        public BuffIcon BuffIcon { get; set; }
        public DateTime Timestamp { get; set; }
    }
}