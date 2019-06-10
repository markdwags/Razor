using System;
using System.Collections.Generic;
using System.Linq;
using Assistant.Macros;

namespace Assistant
{
    public class TargetInfo
    {
        public byte Type;
        public uint TargID;
        public byte Flags;
        public Serial Serial;
        public int X, Y;
        public int Z;
        public ushort Gfx;
    }

    public class Targeting
    {
        public const uint LocalTargID = 0x7FFFFFFF; // uid for target sent from razor

        public delegate void TargetResponseCallback(bool location, Serial serial, Point3D p, ushort gfxid);

        public delegate void CancelTargetCallback();

        private static CancelTargetCallback m_OnCancel;
        private static TargetResponseCallback m_OnTarget;

        private static bool m_Intercept;
        private static bool m_HasTarget;
        private static bool m_ClientTarget;
        private static TargetInfo m_LastTarget;
        private static TargetInfo m_LastGroundTarg;
        private static TargetInfo m_LastBeneTarg;
        private static TargetInfo m_LastHarmTarg;


        private static bool m_FromGrabHotKey;

        private static bool m_AllowGround;
        private static uint m_CurrentID;
        private static byte m_CurFlags;

        private static uint m_PreviousID;
        private static bool m_PreviousGround;
        private static byte m_PrevFlags;

        private static Serial m_LastCombatant;

        private delegate bool QueueTarget();

        private static QueueTarget TargetSelfAction = new QueueTarget(DoTargetSelf);
        private static QueueTarget LastTargetAction = new QueueTarget(DoLastTarget);
        private static QueueTarget m_QueueTarget;


        private static uint m_SpellTargID = 0;

        public static uint SpellTargetID
        {
            get { return m_SpellTargID; }
            set { m_SpellTargID = value; }
        }

        private static List<uint> m_FilterCancel = new List<uint>();

        public static bool HasTarget
        {
            get { return m_HasTarget; }
        }

        public static bool FromGrabHotKey
        {
            get { return m_FromGrabHotKey; }
        }

        private static List<ushort> m_MonsterIds = new List<ushort>()
        {
            0x1, 0x2, 0x3, 0x4, 0x7, 0x8, 0x9, 0xC, 0xD, 0xE, 0xF,
            0x10, 0x11, 0x12, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C,
            0x1E, 0x1F, 0x21, 0x23, 0x24, 0x25, 0x27, 0x29, 0x2A, 0x2C,
            0x2D, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38,
            0x39, 0x3B, 0x3C, 0x3D, 0x42, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49,
            0x4B, 0x4F, 0x50, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x59, 0x5A,
            0x5B, 0x5C, 0x5D, 0x5E, 0x60, 0x61, 0x62, 0x69, 0x6A, 0x6B, 0x6C,
            0x6D, 0x6E, 0x6F, 0x70, 0x71, 0x72, 0x73, 0x74, 0x87, 0x88, 0x89,
            0x8A, 0x8B, 0x8C, 0x8E, 0x8F, 0x91, 0x93, 0x96, 0x99, 0x9B, 0x9E,
            0x9F, 0xA0, 0xA1, 0xA2, 0xA3, 0xA4, 0xB4, 0x4C, 0x4D, 0x3D
        };

        private enum TargetType
        {
            Invalid, // invalid/across server line
            Innocent, //Blue
            GuildAlly, //Green,
            Attackable, //Attackable but not criminal (gray)
            Criminal, //gray
            Enemy, //orange
            Murderer //red
        }


        public static void Initialize()
        {
            PacketHandler.RegisterClientToServerViewer(0x6C, TargetResponse);
            PacketHandler.RegisterServerToClientViewer(0x6C, NewTarget);
            PacketHandler.RegisterServerToClientViewer(0xAA, CombatantChange);

            HotKey.Add(HKCategory.Targets, LocString.LastTarget, LastTarget);
            HotKey.Add(HKCategory.Targets, LocString.TargetSelf, TargetSelf);
            HotKey.Add(HKCategory.Targets, LocString.ClearTargQueue, OnClearQueue);

            HotKey.Add(HKCategory.Targets, LocString.SetLT, TargetSetLastTarget);
            HotKey.Add(HKCategory.Targets, LocString.SetLastBeneficial, SetLastTargetBeneficial);
            HotKey.Add(HKCategory.Targets, LocString.SetLastHarmful, SetLastTargetHarmful);

            HotKey.Add(HKCategory.Targets, LocString.AttackLastComb, AttackLastComb);
            HotKey.Add(HKCategory.Targets, LocString.AttackLastTarg, AttackLastTarg);
            HotKey.Add(HKCategory.Targets, LocString.CancelTarget, CancelTarget);

            InitRandomTarget();
            InitNextPrevTargets();
            InitClosestTargets();
        }

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

        private static void InitNextPrevTargets()
        {
            //Next targets
            HotKey.Add(HKCategory.Targets, LocString.NextTarget, NextTarget);

            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetEnemy, LocString.NextTargetEnemyHumanoid,
                NextTargetEnemyHumanoid);
            HotKey.Add(HKCategory.Targets, LocString.NextTargetHumanoid, NextTargetHumanoid);
            HotKey.Add(HKCategory.Targets, LocString.NextTargetMonster, NextTargetMonster);
            HotKey.Add(HKCategory.Targets, LocString.NextTargetFriend, NextTargetFriend);

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

            //Previous targets
            HotKey.Add(HKCategory.Targets, LocString.PrevTarget, PrevTarget);

            HotKey.Add(HKCategory.Targets, HKSubCat.SubTargetEnemy, LocString.PrevTargetEnemyHumanoid,
                PrevTargetEnemyHumanoid);
            HotKey.Add(HKCategory.Targets, LocString.PrevTargetHumanoid, PrevTargetHumanoid);
            HotKey.Add(HKCategory.Targets, LocString.PrevTargetMonster, PrevTargetMonster);
            HotKey.Add(HKCategory.Targets, LocString.PrevTargetFriend, PrevTargetFriend);

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
        }

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

        private static void CombatantChange(PacketReader p, PacketHandlerEventArgs e)
        {
            Serial ser = p.ReadUInt32();
            if (ser.IsMobile && ser != World.Player.Serial && ser != Serial.Zero && ser != Serial.MinusOne)
                m_LastCombatant = ser;
        }

        private static void AttackLastComb()
        {
            if (m_LastCombatant.IsMobile)
                Client.Instance.SendToServer(new AttackReq(m_LastCombatant));
        }

        private static void AttackLastTarg()
        {
            TargetInfo targ;
            if (Config.GetBool("SmartLastTarget") && Client.Instance.AllowBit(FeatureBit.SmartLT))
            {
                // If Smart Targetting is being used we'll assume that the user would like to attack the harmful target.
                targ = m_LastHarmTarg;

                // If there is no last harmful target, then we'll attack the last target.
                if (targ == null)
                    targ = m_LastTarget;
            }
            else
            {
                targ = m_LastTarget;
            }

            if (targ != null && targ.Serial.IsMobile)
                Client.Instance.SendToServer(new AttackReq(targ.Serial));
        }

        private static void OnClearQueue()
        {
            ClearQueue();

            if (Config.GetBool("ShowTargetSelfLastClearOverhead"))
            {
                World.Player.OverheadMessage(LocString.TQCleared);
            }
            else
            {
                World.Player.SendMessage(MsgLevel.Force, LocString.TQCleared);
            }
        }

        internal static void OneTimeTarget(TargetResponseCallback onTarget)
        {
            OneTimeTarget(false, onTarget, null);
        }

        internal static void OneTimeTarget(TargetResponseCallback onTarget, bool fromGrab)
        {
            m_FromGrabHotKey = fromGrab;

            OneTimeTarget(false, onTarget, null);
        }

        internal static void OneTimeTarget(bool ground, TargetResponseCallback onTarget)
        {
            OneTimeTarget(ground, onTarget, null);
        }

        internal static void OneTimeTarget(TargetResponseCallback onTarget, CancelTargetCallback onCancel)
        {
            OneTimeTarget(false, onTarget, onCancel);
        }

        internal static void OneTimeTarget(bool ground, TargetResponseCallback onTarget, CancelTargetCallback onCancel)
        {
            if (m_Intercept && m_OnCancel != null)
            {
                m_OnCancel();
                CancelOneTimeTarget();
            }

            if (m_HasTarget && m_CurrentID != 0 && m_CurrentID != LocalTargID)
            {
                m_PreviousID = m_CurrentID;
                m_PreviousGround = m_AllowGround;
                m_PrevFlags = m_CurFlags;

                m_FilterCancel.Add(m_PreviousID);
            }

            m_Intercept = true;
            m_CurrentID = LocalTargID;
            m_OnTarget = onTarget;
            m_OnCancel = onCancel;

            m_ClientTarget = m_HasTarget = true;
            Client.Instance.SendToClient(new Target(LocalTargID, ground));
            ClearQueue();
        }

        internal static void CancelOneTimeTarget()
        {
            m_ClientTarget = m_HasTarget = m_FromGrabHotKey = false;

            Client.Instance.SendToClient(new CancelTarget(LocalTargID));
            EndIntercept();
        }

        private static bool m_LTWasSet;

        public static void TargetSetLastTarget()
        {
            if (World.Player != null)
            {
                m_LTWasSet = false;
                OneTimeTarget(false, new TargetResponseCallback(OnSetLastTarget),
                    new CancelTargetCallback(OnSLTCancel));
                World.Player.SendMessage(MsgLevel.Force, LocString.TargSetLT);
            }
        }

        private static void OnSLTCancel()
        {
            if (m_LastTarget != null)
                m_LTWasSet = true;
        }

        private static void OnSetLastTarget(bool location, Serial serial, Point3D p, ushort gfxid)
        {
            if (serial == World.Player.Serial)
            {
                OnSLTCancel();
                return;
            }

            m_LastBeneTarg = m_LastHarmTarg = m_LastGroundTarg = m_LastTarget = new TargetInfo();
            m_LastTarget.Flags = 0;
            m_LastTarget.Gfx = gfxid;
            m_LastTarget.Serial = serial;
            m_LastTarget.Type = (byte) (location ? 1 : 0);
            m_LastTarget.X = p.X;
            m_LastTarget.Y = p.Y;
            m_LastTarget.Z = p.Z;

            m_LTWasSet = true;

            World.Player.SendMessage(MsgLevel.Force, LocString.LTSet);

            if (serial.IsMobile)
            {
                LastTargetChanged();
                Client.Instance.SendToClient(new ChangeCombatant(serial));
                m_LastCombatant = serial;
            }
        }

        private static bool m_LTBeneWasSet;

        /// <summary>
        /// Sets the beneficial target
        /// </summary>
        private static void SetLastTargetBeneficial()
        {
            if (!Config.GetBool("SmartLastTarget"))
            {
                World.Player.SendMessage(MsgLevel.Error, "Smart Targeting is disabled");
                return;
            }

            if (World.Player != null)
            {
                m_LTBeneWasSet = false;
                OneTimeTarget(false, OnSetLastTargetBeneficial, OnSLTBeneficialCancel);
                World.Player.SendMessage(MsgLevel.Force, LocString.TargSetLT);
            }
        }

        private static void OnSLTBeneficialCancel()
        {
            if (m_LastBeneTarg != null)
                m_LTBeneWasSet = true;
        }

        private static void OnSetLastTargetBeneficial(bool location, Serial serial, Point3D p, ushort gfxid)
        {
            if (serial == World.Player.Serial)
            {
                OnSLTBeneficialCancel();
                return;
            }

            m_LastBeneTarg = new TargetInfo
            {
                Flags = 0,
                Gfx = gfxid,
                Serial = serial,
                Type = (byte) (location ? 1 : 0),
                X = p.X,
                Y = p.Y,
                Z = p.Z
            };

            m_LTBeneWasSet = true;

            World.Player.SendMessage(MsgLevel.Force, LocString.SetLTBene);

            if (serial.IsMobile)
            {
                LastBeneficialTargetChanged();
            }
        }

        private static bool m_LTHarmWasSet;

        /// <summary>
        /// Sets the harmful target
        /// </summary>
        private static void SetLastTargetHarmful()
        {
            if (!Config.GetBool("SmartLastTarget"))
            {
                World.Player.SendMessage(MsgLevel.Error, "Smart Targeting is disabled");
                return;
            }

            if (World.Player != null)
            {
                m_LTHarmWasSet = false;
                OneTimeTarget(false, OnSetLastTargetHarmful, OnSLTHarmfulCancel);
                World.Player.SendMessage(MsgLevel.Force, LocString.TargSetLT);
            }
        }

        private static void OnSLTHarmfulCancel()
        {
            if (m_LastTarget != null)
                m_LTHarmWasSet = true;
        }

        private static void OnSetLastTargetHarmful(bool location, Serial serial, Point3D p, ushort gfxid)
        {
            if (serial == World.Player.Serial)
            {
                OnSLTHarmfulCancel();
                return;
            }

            m_LastHarmTarg = new TargetInfo
            {
                Flags = 0,
                Gfx = gfxid,
                Serial = serial,
                Type = (byte) (location ? 1 : 0),
                X = p.X,
                Y = p.Y,
                Z = p.Z
            };

            m_LTHarmWasSet = true;

            World.Player.SendMessage(MsgLevel.Force, LocString.SetLTHarm);

            if (serial.IsMobile)
            {
                LastHarmfulTargetChanged();
            }
        }

        private static Serial m_OldLT = Serial.Zero;
        private static Serial m_OldBeneficialLT = Serial.Zero;
        private static Serial m_OldHarmfulLT = Serial.Zero;

        private static void RemoveTextFlags(UOEntity m)
        {
            if (m != null)
            {
                bool oplchanged = false;

                oplchanged |= m.ObjPropList.Remove(Language.GetString(LocString.LastTarget));
                oplchanged |= m.ObjPropList.Remove(Language.GetString(LocString.HarmfulTarget));
                oplchanged |= m.ObjPropList.Remove(Language.GetString(LocString.BeneficialTarget));

                if (oplchanged)
                    m.OPLChanged();
            }
        }

        private static void AddTextFlags(UOEntity m)
        {
            if (m != null)
            {
                bool oplchanged = false;

                if (Config.GetBool("SmartLastTarget"))
                {
                    if (m_LastHarmTarg != null && m_LastHarmTarg.Serial == m.Serial)
                    {
                        oplchanged = true;
                        m.ObjPropList.Add(Language.GetString(LocString.HarmfulTarget));
                    }

                    if (m_LastBeneTarg != null && m_LastBeneTarg.Serial == m.Serial)
                    {
                        oplchanged = true;
                        m.ObjPropList.Add(Language.GetString(LocString.BeneficialTarget));
                    }
                }

                if (!oplchanged && m_LastTarget != null && m_LastTarget.Serial == m.Serial)
                {
                    oplchanged = true;
                    m.ObjPropList.Add(Language.GetString(LocString.LastTarget));
                }

                if (oplchanged)
                    m.OPLChanged();
            }
        }

        private static void LastTargetChanged()
        {
            if (m_LastTarget != null)
            {
                bool lth = Config.GetInt("LTHilight") != 0;

                if (m_OldLT.IsItem)
                {
                    RemoveTextFlags(World.FindItem(m_OldLT));
                }
                else
                {
                    Mobile m = World.FindMobile(m_OldLT);
                    if (m != null)
                    {
                        if (lth)
                            Client.Instance.SendToClient(new MobileIncoming(m));

                        RemoveTextFlags(m);
                    }
                }

                if (m_LastTarget.Serial.IsItem)
                {
                    AddTextFlags(World.FindItem(m_LastTarget.Serial));
                }
                else
                {
                    Mobile m = World.FindMobile(m_LastTarget.Serial);
                    if (m != null)
                    {
                        if (IsLastTarget(m) && lth)
                            Client.Instance.SendToClient(new MobileIncoming(m));

                        CheckLastTargetRange(m);

                        AddTextFlags(m);
                    }
                }

                m_OldLT = m_LastTarget.Serial;
            }
        }

        private static void LastBeneficialTargetChanged()
        {
            if (m_LastBeneTarg != null)
            {
                if (m_OldBeneficialLT.IsItem)
                {
                    RemoveTextFlags(World.FindItem(m_OldBeneficialLT));
                }
                else
                {
                    Mobile m = World.FindMobile(m_OldBeneficialLT);
                    if (m != null)
                    {
                        RemoveTextFlags(m);
                    }
                }

                if (m_LastBeneTarg.Serial.IsItem)
                {
                    AddTextFlags(World.FindItem(m_LastBeneTarg.Serial));
                }
                else
                {
                    Mobile m = World.FindMobile(m_LastBeneTarg.Serial);
                    if (m != null)
                    {
                        CheckLastTargetRange(m);

                        AddTextFlags(m);
                    }
                }

                m_OldBeneficialLT = m_LastBeneTarg.Serial;
            }
        }

        private static void LastHarmfulTargetChanged()
        {
            if (m_LastHarmTarg != null)
            {
                if (m_OldHarmfulLT.IsItem)
                {
                    RemoveTextFlags(World.FindItem(m_OldHarmfulLT));
                }
                else
                {
                    Mobile m = World.FindMobile(m_OldHarmfulLT);
                    if (m != null)
                    {
                        RemoveTextFlags(m);
                    }
                }

                if (m_LastHarmTarg.Serial.IsItem)
                {
                    AddTextFlags(World.FindItem(m_LastHarmTarg.Serial));
                }
                else
                {
                    Mobile m = World.FindMobile(m_LastHarmTarg.Serial);
                    if (m != null)
                    {
                        CheckLastTargetRange(m);

                        AddTextFlags(m);
                    }
                }

                m_OldHarmfulLT = m_LastHarmTarg.Serial;
            }
        }


        public static bool LTWasSet
        {
            get { return m_LTWasSet; }
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

        public static void RandomTarget(params int[] noto)
        {
            if (!Client.Instance.AllowBit(FeatureBit.RandomTargets))
                return;

            List<Mobile> list = new List<Mobile>();
            foreach (Mobile m in World.MobilesInRange(12))
            {
                if ((!FriendsAgent.IsFriend(m) || (noto.Length > 0 && noto[0] == 0)) &&
                    !m.Blessed && !m.IsGhost && m.Serial != World.Player.Serial &&
                    Utility.InRange(World.Player.Position, m.Position, Config.GetInt("LTRange")))
                {
                    for (int i = 0; i < noto.Length; i++)
                    {
                        if (noto[i] == m.Notoriety)
                        {
                            list.Add(m);
                            break;
                        }
                    }

                    if (noto.Length == 0)
                        list.Add(m);
                }
            }

            if (list.Count > 0)
                SetLastTargetTo((Mobile) list[Utility.Random(list.Count)]);
            else
                World.Player.SendMessage(MsgLevel.Warning, LocString.TargNoOne);
        }

        public static void RandomHumanoidTarget(params int[] noto)
        {
            if (!Client.Instance.AllowBit(FeatureBit.RandomTargets))
                return;

            List<Mobile> list = new List<Mobile>();
            foreach (Mobile m in World.MobilesInRange(12))
            {
                if (m.Body != 0x0190 && m.Body != 0x0191 && m.Body != 0x025D && m.Body != 0x025E)
                    continue;

                if ((!FriendsAgent.IsFriend(m) || (noto.Length > 0 && noto[0] == 0)) &&
                    !m.Blessed && !m.IsGhost && m.Serial != World.Player.Serial &&
                    Utility.InRange(World.Player.Position, m.Position, Config.GetInt("LTRange")))
                {
                    for (int i = 0; i < noto.Length; i++)
                    {
                        if (noto[i] == m.Notoriety)
                        {
                            list.Add(m);
                            break;
                        }
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

        public static void RandomMonsterTarget(params int[] noto)
        {
            if (!Client.Instance.AllowBit(FeatureBit.RandomTargets))
                return;

            List<Mobile> list = new List<Mobile>();
            foreach (Mobile m in World.MobilesInRange(12))
            {
                if (!m.IsMonster)
                    continue;

                if ((!FriendsAgent.IsFriend(m) || (noto.Length > 0 && noto[0] == 0)) &&
                    !m.Blessed && !m.IsGhost && m.Serial != World.Player.Serial &&
                    Utility.InRange(World.Player.Position, m.Position, Config.GetInt("LTRange")))
                {
                    for (int i = 0; i < noto.Length; i++)
                    {
                        if (noto[i] == m.Notoriety)
                        {
                            list.Add(m);
                            break;
                        }
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

        public static void RandomFriendTarget()
        {
            if (!Client.Instance.AllowBit(FeatureBit.RandomTargets))
                return;

            List<Mobile> list = new List<Mobile>();
            foreach (Mobile m in World.MobilesInRange(12))
            {
                if ((FriendsAgent.IsFriend(m) &&
                     !m.Blessed && !m.IsGhost && m.Serial != World.Player.Serial &&
                     Utility.InRange(World.Player.Position, m.Position, Config.GetInt("LTRange"))))
                {
                    list.Add(m);
                }
            }

            if (list.Count > 0)
                SetLastTargetTo((Mobile) list[Utility.Random(list.Count)]);
            else
                World.Player.SendMessage(MsgLevel.Warning, LocString.TargNoOne);
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
            ClosestTarget((int) TargetType.Invalid, (int) TargetType.Innocent, (int) TargetType.GuildAlly);
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

        public static void ClosestTarget(params int[] noto)
        {
            if (!Client.Instance.AllowBit(FeatureBit.ClosestTargets))
                return;

            List<Mobile> list = new List<Mobile>();
            foreach (Mobile m in World.MobilesInRange(12))
            {
                if ((!FriendsAgent.IsFriend(m) || (noto.Length > 0 && noto[0] == 0)) &&
                    !m.Blessed && !m.IsGhost && m.Serial != World.Player.Serial &&
                    Utility.InRange(World.Player.Position, m.Position, Config.GetInt("LTRange")))
                {
                    for (int i = 0; i < noto.Length; i++)
                    {
                        if (noto[i] == m.Notoriety)
                        {
                            list.Add(m);
                            break;
                        }
                    }

                    if (noto.Length == 0)
                        list.Add(m);
                }
            }

            Mobile closest = null;
            double closestDist = double.MaxValue;

            foreach (Mobile m in list)
            {
                double dist = Utility.DistanceSqrt(m.Position, World.Player.Position);

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

        public static void ClosestHumanoidTarget(params int[] noto)
        {
            if (!Client.Instance.AllowBit(FeatureBit.ClosestTargets))
                return;

            List<Mobile> list = new List<Mobile>();
            foreach (Mobile m in World.MobilesInRange(12))
            {
                if (m.Body != 0x0190 && m.Body != 0x0191 && m.Body != 0x025D && m.Body != 0x025E)
                    continue;

                if ((!FriendsAgent.IsFriend(m) || (noto.Length > 0 && noto[0] == 0)) &&
                    !m.Blessed && !m.IsGhost && m.Serial != World.Player.Serial &&
                    Utility.InRange(World.Player.Position, m.Position, Config.GetInt("LTRange")))
                {
                    for (int i = 0; i < noto.Length; i++)
                    {
                        if (noto[i] == m.Notoriety)
                        {
                            list.Add(m);
                            break;
                        }
                    }

                    if (noto.Length == 0)
                        list.Add(m);
                }
            }

            Mobile closest = null;
            double closestDist = double.MaxValue;

            foreach (Mobile m in list)
            {
                double dist = Utility.DistanceSqrt(m.Position, World.Player.Position);

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

        public static void ClosestMonsterTarget(params int[] noto)
        {
            if (!Client.Instance.AllowBit(FeatureBit.ClosestTargets))
                return;

            List<Mobile> list = new List<Mobile>();
            foreach (Mobile m in World.MobilesInRange(12))
            {
                if (!m.IsMonster)
                    continue;

                if ((!FriendsAgent.IsFriend(m) || (noto.Length > 0 && noto[0] == 0)) &&
                    !m.Blessed && !m.IsGhost && m.Serial != World.Player.Serial &&
                    Utility.InRange(World.Player.Position, m.Position, Config.GetInt("LTRange")))
                {
                    for (int i = 0; i < noto.Length; i++)
                    {
                        if (noto[i] == m.Notoriety)
                        {
                            list.Add(m);
                            break;
                        }
                    }

                    if (noto.Length == 0)
                        list.Add(m);
                }
            }

            Mobile closest = null;
            double closestDist = double.MaxValue;

            foreach (Mobile m in list)
            {
                double dist = Utility.DistanceSqrt(m.Position, World.Player.Position);

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

        public static void ClosestFriendTarget()
        {
            if (!Client.Instance.AllowBit(FeatureBit.ClosestTargets))
                return;

            List<Mobile> list = new List<Mobile>();
            foreach (Mobile m in World.MobilesInRange(12))
            {
                if (FriendsAgent.IsFriend(m) && !m.Blessed && !m.IsGhost && m.Serial != World.Player.Serial &&
                    Utility.InRange(World.Player.Position, m.Position, Config.GetInt("LTRange")))
                {
                    list.Add(m);
                }
            }

            Mobile closest = null;
            double closestDist = double.MaxValue;

            foreach (Mobile m in list)
            {
                double dist = Utility.DistanceSqrt(m.Position, World.Player.Position);

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

        public static void SetLastTargetTo(Mobile m)
        {
            SetLastTargetTo(m, 0);
        }

        public static void SetLastTargetTo(Mobile m, byte flagType)
        {
            TargetInfo targ = new TargetInfo();
            m_LastGroundTarg = m_LastTarget = targ;

            if ((m_HasTarget && m_CurFlags == 1) || flagType == 1)
                m_LastHarmTarg = targ;
            else if ((m_HasTarget && m_CurFlags == 2) || flagType == 2)
                m_LastBeneTarg = targ;
            else if (flagType == 0)
                m_LastHarmTarg = m_LastBeneTarg = targ;

            targ.Type = 0;
            if (m_HasTarget)
                targ.Flags = m_CurFlags;
            else
                targ.Flags = flagType;

            targ.Gfx = m.Body;
            targ.Serial = m.Serial;
            targ.X = m.Position.X;
            targ.Y = m.Position.Y;
            targ.Z = m.Position.Z;

            Client.Instance.SendToClient(new ChangeCombatant(m));
            m_LastCombatant = m.Serial;
            World.Player.SendMessage(MsgLevel.Force, LocString.NewTargSet);

            OverheadTargetMessage(targ);

            bool wasSmart = Config.GetBool("SmartLastTarget");
            if (wasSmart)
                Config.SetProperty("SmartLastTarget", false);
            LastTarget();
            if (wasSmart)
                Config.SetProperty("SmartLastTarget", true);
            LastTargetChanged();
        }

        private static void EndIntercept()
        {
            m_Intercept = false;
            m_OnTarget = null;
            m_OnCancel = null;
            m_FromGrabHotKey = false;
        }

        public static void TargetSelf()
        {
            TargetSelf(false);
        }

        public static void TargetSelf(bool forceQ)
        {
            if (World.Player == null)
                return;

            //if ( Macros.MacroManager.AcceptActions )
            //	MacroManager.Action( new TargetSelfAction() );

            if (m_HasTarget)
            {
                if (!DoTargetSelf())
                    ResendTarget();
            }
            else if (forceQ || Config.GetBool("QueueTargets"))
            {
                if (!forceQ)
                {
                    if (Config.GetBool("ShowTargetSelfLastClearOverhead"))
                    {
                        World.Player.OverheadMessage(LocString.QueuedTS);
                    }
                    else
                    {
                        World.Player.SendMessage(MsgLevel.Force, LocString.QueuedTS);
                    }
                }

                m_QueueTarget = TargetSelfAction;
            }
        }

        public static bool DoTargetSelf()
        {
            if (World.Player == null)
                return false;

            if (CheckHealPoisonTarg(m_CurrentID, World.Player.Serial))
                return false;

            CancelClientTarget();
            m_HasTarget = false;
            m_FromGrabHotKey = false;

            if (m_Intercept)
            {
                TargetInfo targ = new TargetInfo();
                targ.Serial = World.Player.Serial;
                targ.Gfx = World.Player.Body;
                targ.Type = 0;
                targ.X = World.Player.Position.X;
                targ.Y = World.Player.Position.Y;
                targ.Z = World.Player.Position.Z;
                targ.TargID = LocalTargID;
                targ.Flags = 0;

                OneTimeResponse(targ);
            }
            else
            {
                Client.Instance.SendToServer(new TargetResponse(m_CurrentID, World.Player));
            }

            return true;
        }

        public static void LastTarget()
        {
            LastTarget(false);
        }

        public static void LastTarget(bool forceQ)
        {
            //if ( Macros.MacroManager.AcceptActions )
            //	MacroManager.Action( new LastTargetAction() );

            if (m_HasTarget)
            {
                if (!DoLastTarget())
                    ResendTarget();
            }
            else if (forceQ || Config.GetBool("QueueTargets"))
            {
                if (!forceQ)
                {
                    if (Config.GetBool("ShowTargetSelfLastClearOverhead"))
                    {
                        World.Player.OverheadMessage(LocString.QueuedLT);
                    }
                    else
                    {
                        World.Player.SendMessage(MsgLevel.Force, LocString.QueuedLT);
                    }
                }

                m_QueueTarget = LastTargetAction;
            }
        }

        public static bool DoLastTarget()
        {
            TargetInfo targ;
            if (Config.GetBool("SmartLastTarget") && Client.Instance.AllowBit(FeatureBit.SmartLT))
            {
                if (m_AllowGround && m_LastGroundTarg != null)
                    targ = m_LastGroundTarg;
                else if (m_CurFlags == 1)
                    targ = m_LastHarmTarg;
                else if (m_CurFlags == 2)
                    targ = m_LastBeneTarg;
                else
                    targ = m_LastTarget;

                if (targ == null)
                    targ = m_LastTarget;
            }
            else
            {
                if (m_AllowGround && m_LastGroundTarg != null)
                    targ = m_LastGroundTarg;
                else
                    targ = m_LastTarget;
            }

            if (targ == null)
                return false;

            Point3D pos = Point3D.Zero;
            if (targ.Serial.IsMobile)
            {
                Mobile m = World.FindMobile(targ.Serial);
                if (m != null)
                {
                    pos = m.Position;

                    targ.X = pos.X;
                    targ.Y = pos.Y;
                    targ.Z = pos.Z;
                }
                else
                {
                    pos = Point3D.Zero;
                }
            }
            else if (targ.Serial.IsItem)
            {
                Item i = World.FindItem(targ.Serial);
                if (i != null)
                {
                    pos = i.GetWorldPosition();

                    targ.X = i.Position.X;
                    targ.Y = i.Position.Y;
                    targ.Z = i.Position.Z;
                }
                else
                {
                    pos = Point3D.Zero;
                    targ.X = targ.Y = targ.Z = 0;
                }
            }
            else
            {
                if (!m_AllowGround && (targ.Serial == Serial.Zero || targ.Serial >= 0x80000000))
                {
                    World.Player.SendMessage(MsgLevel.Warning, LocString.LTGround);
                    return false;
                }
                else
                {
                    pos = new Point3D(targ.X, targ.Y, targ.Z);
                }
            }

            if (Config.GetBool("RangeCheckLT") && Client.Instance.AllowBit(FeatureBit.RangeCheckLT) &&
                (pos == Point3D.Zero || !Utility.InRange(World.Player.Position, pos, Config.GetInt("LTRange"))))
            {
                if (Config.GetBool("QueueTargets"))
                    m_QueueTarget = LastTargetAction;
                World.Player.SendMessage(MsgLevel.Warning, LocString.LTOutOfRange);
                return false;
            }

            if (CheckHealPoisonTarg(m_CurrentID, targ.Serial))
                return false;

            CancelClientTarget();
            m_HasTarget = false;

            targ.TargID = m_CurrentID;

            if (m_Intercept)
                OneTimeResponse(targ);
            else
                Client.Instance.SendToServer(new TargetResponse(targ));
            return true;
        }

        public static void ClearQueue()
        {
            m_QueueTarget = null;
        }

        private static TimerCallbackState m_OneTimeRespCallback = new TimerCallbackState(OneTimeResponse);

        private static void OneTimeResponse(object state)
        {
            TargetInfo info = state as TargetInfo;

            if (info != null)
            {
                if ((info.X == 0xFFFF && info.X == 0xFFFF) && (info.Serial == 0 || info.Serial >= 0x80000000))
                {
                    if (m_OnCancel != null)
                        m_OnCancel();
                }
                else
                {
                    if (Macros.MacroManager.AcceptActions)
                        MacroManager.Action(new AbsoluteTargetAction(info));

                    if (m_OnTarget != null)
                        m_OnTarget(info.Type == 1 ? true : false, info.Serial, new Point3D(info.X, info.Y, info.Z),
                            info.Gfx);
                }
            }

            EndIntercept();
        }

        private static void CancelTarget()
        {
            OnClearQueue();
            CancelClientTarget();

            m_FromGrabHotKey = false;

            if (m_HasTarget)
            {
                Client.Instance.SendToServer(new TargetCancelResponse(m_CurrentID));
                m_HasTarget = false;
            }
        }

        private static void CancelClientTarget()
        {
            if (m_ClientTarget)
            {
                m_FilterCancel.Add((uint) m_CurrentID);
                Client.Instance.SendToClient(new CancelTarget(m_CurrentID));
                m_ClientTarget = false;
            }
        }

        public static void Target(TargetInfo info)
        {
            if (m_Intercept)
            {
                OneTimeResponse(info);
            }
            else if (m_HasTarget)
            {
                info.TargID = m_CurrentID;
                m_LastGroundTarg = m_LastTarget = info;
                Client.Instance.SendToServer(new TargetResponse(info));
            }

            CancelClientTarget();
            m_HasTarget = false;
            m_FromGrabHotKey = false;
        }

        public static void Target(Point3D pt)
        {
            TargetInfo info = new TargetInfo();
            info.Type = 1;
            info.Flags = 0;
            info.Serial = 0;
            info.X = pt.X;
            info.Y = pt.Y;
            info.Z = pt.Z;
            info.Gfx = 0;

            Target(info);
        }

        public static void Target(Point3D pt, int gfx)
        {
            TargetInfo info = new TargetInfo();
            info.Type = 1;
            info.Flags = 0;
            info.Serial = 0;
            info.X = pt.X;
            info.Y = pt.Y;
            info.Z = pt.Z;
            info.Gfx = (ushort) (gfx & 0x3FFF);

            Target(info);
        }

        public static void Target(Serial s)
        {
            TargetInfo info = new TargetInfo();
            info.Type = 0;
            info.Flags = 0;
            info.Serial = s;

            if (s.IsItem)
            {
                Item item = World.FindItem(s);
                if (item != null)
                {
                    info.X = item.Position.X;
                    info.Y = item.Position.Y;
                    info.Z = item.Position.Z;
                    info.Gfx = item.ItemID;
                }
            }
            else if (s.IsMobile)
            {
                Mobile m = World.FindMobile(s);
                if (m != null)
                {
                    info.X = m.Position.X;
                    info.Y = m.Position.Y;
                    info.Z = m.Position.Z;
                    info.Gfx = m.Body;
                }
            }

            Target(info);
        }

        public static void Target(object o)
        {
            if (o is Item)
            {
                Item item = (Item) o;
                TargetInfo info = new TargetInfo();
                info.Type = 0;
                info.Flags = 0;
                info.Serial = item.Serial;
                info.X = item.Position.X;
                info.Y = item.Position.Y;
                info.Z = item.Position.Z;
                info.Gfx = item.ItemID;
                Target(info);
            }
            else if (o is Mobile)
            {
                Mobile m = (Mobile) o;
                TargetInfo info = new TargetInfo();
                info.Type = 0;
                info.Flags = 0;
                info.Serial = m.Serial;
                info.X = m.Position.X;
                info.Y = m.Position.Y;
                info.Z = m.Position.Z;
                info.Gfx = m.Body;
                Target(info);
            }
            else if (o is Serial)
            {
                Target((Serial) o);
            }
            else if (o is TargetInfo)
            {
                Target((TargetInfo) o);
            }
        }

        public static void CheckTextFlags(Mobile m)
        {
            if (IgnoreAgent.IsIgnored(m.Serial))
            {
                m.OverheadMessage(Config.GetInt("SysColor"), "[Ignored]");
            }

            if (Config.GetBool("ShowFriendOverhead") && FriendsAgent.IsFriend(m))
            {
                m.OverheadMessage(0x03F, $"[{Language.GetString(LocString.Friend)}]");
            }

            if (Config.GetBool("SmartLastTarget") && Client.Instance.AllowBit(FeatureBit.SmartLT))
            {
                bool harm = m_LastHarmTarg != null && m_LastHarmTarg.Serial == m.Serial;
                bool bene = m_LastBeneTarg != null && m_LastBeneTarg.Serial == m.Serial;

                if (harm)
                    m.OverheadMessage(0x90, $"[{Language.GetString(LocString.HarmfulTarget)}]");
                if (bene)
                    m.OverheadMessage(0x3F, $"[{Language.GetString(LocString.BeneficialTarget)}]");
            }

            if (m_LastTarget != null && m_LastTarget.Serial == m.Serial)
                m.OverheadMessage(0x3B2, $"[{Language.GetString(LocString.LastTarget)}]");
        }

        public static bool IsLastTarget(Mobile m)
        {
            if (m != null)
            {
                if (Config.GetBool("SmartLastTarget") && Client.Instance.AllowBit(FeatureBit.SmartLT))
                {
                    if (m_LastHarmTarg != null && m_LastHarmTarg.Serial == m.Serial)
                        return true;
                }
                else
                {
                    if (m_LastTarget != null && m_LastTarget.Serial == m.Serial)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Index used to keep track of the current Next/Prev target
        /// </summary>
        private static int _nextPrevTargetIndex;

        private static bool IsNextPrevFriend(Mobile mobile)
        {
            return Config.GetBool("NextPrevTargetIgnoresFriends") && FriendsAgent.IsFriend(mobile);
        }

        /// <summary>
        /// Handles the common Next/Prev logic based on a list of targets passed in already filtered by the calling
        /// functions conditions.
        /// </summary>
        /// <param name="targets">The list of targets (already filtered)</param>
        /// <param name="nextTarget">next target true, previous target false</param>
        /// <param name="removeFriends"></param>
        public static void NextPrevTarget(List<Mobile> targets, bool nextTarget)
        {
            if (targets.Count <= 0)
            {
                World.Player.SendMessage(MsgLevel.Warning, LocString.TargNoOne);
                return;
            }


            Mobile mobile = null, old = World.FindMobile(m_LastTarget?.Serial ?? Serial.Zero);
            TargetInfo target = new TargetInfo();


            // Loop through 3 times and break out if you can't get a target for some reason
            for (int i = 0; i < 3; i++)
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

                if (mobile != null && mobile != World.Player && mobile != old)
                {
                    break;
                }

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

            m_LastHarmTarg = m_LastBeneTarg = target;

            if (m_HasTarget)
                target.Flags = m_CurFlags;
            else
                target.Type = 0;

            target.Gfx = mobile.Body;
            target.Serial = mobile.Serial;
            target.X = mobile.Position.X;
            target.Y = mobile.Position.Y;
            target.Z = mobile.Position.Z;

            Client.Instance.SendToClient(new ChangeCombatant(mobile));

            m_LastCombatant = mobile.Serial;
            World.Player.SendMessage(MsgLevel.Force, LocString.NewTargSet);

            OverheadTargetMessage(target);
        }

        public static void NextTarget()
        {
            List<Mobile> mobiles = World.MobilesInRange();

            NextPrevTarget(mobiles, true);
        }

        public static void NextTargetHumanoid()
        {
            List<Mobile> mobiles = World.MobilesInRange().Where(x =>
                    x.IsHuman && !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial)
                .ToList();

            NextPrevTarget(mobiles, true);
        }

        public static void NextTargetEnemyHumanoid()
        {
            List<Mobile> mobiles = World.MobilesInRange().Where(x =>
                    x.IsHuman && x.Notoriety == (int) TargetType.Enemy && !IsNextPrevFriend(x) && !x.Blessed &&
                    !x.IsGhost && x.Serial != World.Player.Serial)
                .ToList();

            NextPrevTarget(mobiles, true);
        }

        public static void NextTargetMonster()
        {
            List<Mobile> mobiles = World.MobilesInRange().Where(x =>
                    x.IsMonster && !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial)
                .ToList();

            NextPrevTarget(mobiles, true);
        }

        public static void NextTargetFriend()
        {
            List<Mobile> mobiles = World.MobilesInRange().Where(x =>
                IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial).ToList();

            NextPrevTarget(mobiles, true);
        }

        public static void PrevTarget()
        {
            List<Mobile> mobiles = World.MobilesInRange();

            NextPrevTarget(mobiles, false);
        }

        public static void PrevTargetHumanoid()
        {
            List<Mobile> mobiles = World.MobilesInRange().Where(x =>
                    x.IsHuman && !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial)
                .ToList();

            NextPrevTarget(mobiles, false);
        }

        public static void PrevTargetEnemyHumanoid()
        {
            List<Mobile> mobiles = World.MobilesInRange().Where(x =>
                    x.IsHuman && x.Notoriety == (int) TargetType.Enemy && !IsNextPrevFriend(x) && !x.Blessed &&
                    !x.IsGhost && x.Serial != World.Player.Serial)
                .ToList();

            NextPrevTarget(mobiles, false);
        }

        public static void PrevTargetMonster()
        {
            List<Mobile> mobiles = World.MobilesInRange().Where(x =>
                    x.IsMonster && !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial)
                .ToList();

            NextPrevTarget(mobiles, false);
        }

        public static void PrevTargetFriend()
        {
            List<Mobile> mobiles = World.MobilesInRange().Where(x =>
                IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost &&
                x.Serial != World.Player.Serial).ToList();

            NextPrevTarget(mobiles, false);
        }

        public static void NextTargetCriminalHumanoid()
        {
            List<Mobile> mobiles = World.MobilesInRange().Where(x =>
                x.IsHuman && x.Notoriety == (int) TargetType.Criminal && !IsNextPrevFriend(x) && !x.Blessed &&
                !x.IsGhost && x.Serial != World.Player.Serial).ToList();

            NextPrevTarget(mobiles, true);
        }

        public static void NextTargetMurdererHumanoid()
        {
            List<Mobile> mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && x.Notoriety == (int) TargetType.Murderer && !IsNextPrevFriend(x) &&
                            !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial).ToList();

            NextPrevTarget(mobiles, true);
        }

        public static void NextTargetInnocentHumanoid()
        {
            List<Mobile> mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && x.Notoriety == (int) TargetType.Innocent && !IsNextPrevFriend(x) &&
                            !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial).ToList();

            NextPrevTarget(mobiles, true);
        }

        public static void PrevTargetCriminalHumanoid()
        {
            List<Mobile> mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && x.Notoriety == (int) TargetType.Criminal && !IsNextPrevFriend(x) &&
                            !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial).ToList();

            NextPrevTarget(mobiles, false);
        }

        public static void PrevTargetMurdererHumanoid()
        {
            List<Mobile> mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && x.Notoriety == (int) TargetType.Murderer && !IsNextPrevFriend(x) &&
                            !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial).ToList();

            NextPrevTarget(mobiles, false);
        }

        public static void PrevTargetInnocentHumanoid()
        {
            List<Mobile> mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && x.Notoriety == (int) TargetType.Innocent && !IsNextPrevFriend(x) &&
                            !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial).ToList();

            NextPrevTarget(mobiles, false);
        }

        public static void NextTargetFriendlyHumanoid()
        {
            List<Mobile> mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && (x.Notoriety == (int) TargetType.Innocent ||
                                          x.Notoriety == (int) TargetType.Invalid ||
                                          x.Notoriety == (int) TargetType.GuildAlly &&
                                          !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial)).ToList();

            NextPrevTarget(mobiles, true);
        }

        public static void NextTargetGreyHumanoid()
        {
            List<Mobile> mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && (x.Notoriety == (int) TargetType.Attackable ||
                                          x.Notoriety == (int) TargetType.Criminal && 
                                          !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial)).ToList();

            NextPrevTarget(mobiles, true);
        }

        public static void NextTargetNonFriendlyHumanoid()
        {
            List<Mobile> mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && (x.Notoriety == (int) TargetType.Attackable ||
                                          x.Notoriety == (int) TargetType.Criminal ||
                                          x.Notoriety == (int) TargetType.Enemy ||
                                          x.Notoriety == (int) TargetType.Murderer &&
                                          !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial
                                          )).ToList();

            NextPrevTarget(mobiles, true);
        }

        public static void PrevTargetFriendlyHumanoid()
        {
            List<Mobile> mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && (x.Notoriety == (int) TargetType.Innocent ||
                                          x.Notoriety == (int) TargetType.Invalid ||
                                          x.Notoriety == (int) TargetType.GuildAlly &&
                                          !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial)).ToList();

            NextPrevTarget(mobiles, false);
        }

        public static void PrevTargetGreyHumanoid()
        {
            List<Mobile> mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && (x.Notoriety == (int) TargetType.Attackable ||
                                          x.Notoriety == (int) TargetType.Criminal && 
                                          !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial)).ToList();

            NextPrevTarget(mobiles, false);
        }

        public static void PrevTargetNonFriendlyHumanoid()
        {
            List<Mobile> mobiles = World.MobilesInRange()
                .Where(x => x.IsHuman && (x.Notoriety == (int) TargetType.Attackable ||
                                          x.Notoriety == (int) TargetType.Criminal ||
                                          x.Notoriety == (int) TargetType.Enemy ||
                                          x.Notoriety == (int) TargetType.Murderer &&
                                          !IsNextPrevFriend(x) && !x.Blessed && !x.IsGhost && x.Serial != World.Player.Serial)).ToList();

            NextPrevTarget(mobiles, false);
        }

        public static void CheckLastTargetRange(Mobile m)
        {
            if (World.Player == null)
                return;

            if (m_HasTarget && m != null && m_LastTarget != null && m.Serial == m_LastTarget.Serial &&
                m_QueueTarget == LastTargetAction)
            {
                if (Config.GetBool("RangeCheckLT") && Client.Instance.AllowBit(FeatureBit.RangeCheckLT))
                {
                    if (Utility.InRange(World.Player.Position, m.Position, Config.GetInt("LTRange")))
                    {
                        if (m_QueueTarget())
                            ClearQueue();
                    }
                }
            }
        }

        private static bool CheckHealPoisonTarg(uint targID, Serial ser)
        {
            if (World.Player == null)
                return false;

            if (targID == m_SpellTargID && ser.IsMobile &&
                (World.Player.LastSpell == Spell.ToID(1, 4) || World.Player.LastSpell == Spell.ToID(4, 5)) &&
                Config.GetBool("BlockHealPoison") && Client.Instance.AllowBit(FeatureBit.BlockHealPoisoned))
            {
                Mobile m = World.FindMobile(ser);

                if (m != null && m.Poisoned)
                {
                    World.Player.SendMessage(MsgLevel.Warning, LocString.HealPoisonBlocked);
                    return true;
                }
            }

            return false;
        }

        private static void TargetResponse(PacketReader p, PacketHandlerEventArgs args)
        {
            TargetInfo info = new TargetInfo
            {
                Type = p.ReadByte(),
                TargID = p.ReadUInt32(),
                Flags = p.ReadByte(),
                Serial = p.ReadUInt32(),
                X = p.ReadUInt16(),
                Y = p.ReadUInt16(),
                Z = p.ReadInt16(),
                Gfx = p.ReadUInt16()
            };

            m_ClientTarget = false;

            OverheadTargetMessage(info);

            // check for cancel
            if (info.X == 0xFFFF && info.X == 0xFFFF && (info.Serial <= 0 || info.Serial >= 0x80000000))
            {
                m_HasTarget = false;
                m_FromGrabHotKey = false;

                if (m_Intercept)
                {
                    args.Block = true;
                    Timer.DelayedCallbackState(TimeSpan.Zero, m_OneTimeRespCallback, info).Start();
                    EndIntercept();

                    if (m_PreviousID != 0)
                    {
                        m_CurrentID = m_PreviousID;
                        m_AllowGround = m_PreviousGround;
                        m_CurFlags = m_PrevFlags;

                        m_PreviousID = 0;

                        ResendTarget();
                    }
                }
                else if (m_FilterCancel.Contains((uint) info.TargID) || info.TargID == LocalTargID)
                {
                    args.Block = true;
                }

                m_FilterCancel.Clear();
                return;
            }

            ClearQueue();

            if (m_Intercept)
            {
                if (info.TargID == LocalTargID)
                {
                    Timer.DelayedCallbackState(TimeSpan.Zero, m_OneTimeRespCallback, info).Start();

                    m_HasTarget = false;
                    m_FromGrabHotKey = false;
                    args.Block = true;

                    if (m_PreviousID != 0)
                    {
                        m_CurrentID = m_PreviousID;
                        m_AllowGround = m_PreviousGround;
                        m_CurFlags = m_PrevFlags;

                        m_PreviousID = 0;

                        ResendTarget();
                    }

                    m_FilterCancel.Clear();

                    return;
                }
                else
                {
                    EndIntercept();
                }
            }

            m_HasTarget = false;

            if (CheckHealPoisonTarg(m_CurrentID, info.Serial))
            {
                ResendTarget();
                args.Block = true;
            }

            if (info.Serial != World.Player.Serial)
            {
                if (info.Serial.IsValid)
                {
                    // only let lasttarget be a non-ground target

                    m_LastTarget = info;
                    if (info.Flags == 1)
                        m_LastHarmTarg = info;
                    else if (info.Flags == 2)
                        m_LastBeneTarg = info;

                    LastTargetChanged();
                    LastBeneficialTargetChanged();
                    LastHarmfulTargetChanged();
                }

                m_LastGroundTarg = info; // ground target is the true last target

                if (Macros.MacroManager.AcceptActions)
                    MacroManager.Action(new AbsoluteTargetAction(info));
            }
            else
            {
                if (Macros.MacroManager.AcceptActions)
                {
                    KeyData hk = HotKey.Get((int) LocString.TargetSelf);
                    if (hk != null)
                        MacroManager.Action(new HotKeyAction(hk));
                    else
                        MacroManager.Action(new AbsoluteTargetAction(info));
                }
            }

            if (World.Player.LastSpell == 52 && !GateTimer.Running)
            {
                GateTimer.Start();
            }

            m_FilterCancel.Clear();
        }

        private static void NewTarget(PacketReader p, PacketHandlerEventArgs args)
        {
            bool prevAllowGround = m_AllowGround;
            uint prevID = m_CurrentID;
            byte prevFlags = m_CurFlags;
            bool prevClientTarget = m_ClientTarget;

            m_AllowGround = p.ReadBoolean(); // allow ground
            m_CurrentID = p.ReadUInt32(); // target uid
            m_CurFlags = p.ReadByte(); // flags
            // the rest of the packet is 0s

            // check for a server cancel command
            if (!m_AllowGround && m_CurrentID == 0 && m_CurFlags == 3)
            {
                m_HasTarget = false;
                m_FromGrabHotKey = false;

                m_ClientTarget = false;
                if (m_Intercept)
                {
                    EndIntercept();
                    World.Player.SendMessage(MsgLevel.Error, LocString.OTTCancel);
                }

                return;
            }

            if (Spell.LastCastTime + TimeSpan.FromSeconds(3.0) > DateTime.UtcNow &&
                Spell.LastCastTime + TimeSpan.FromSeconds(0.5) <= DateTime.UtcNow && m_SpellTargID == 0)
                m_SpellTargID = m_CurrentID;

            m_HasTarget = true;
            m_ClientTarget = false;

            if (m_QueueTarget == null && Macros.MacroManager.AcceptActions &&
                MacroManager.Action(new WaitForTargetAction()))
            {
                args.Block = true;
            }
            else if (m_QueueTarget != null && m_QueueTarget())
            {
                ClearQueue();
                args.Block = true;
            }

            if (args.Block)
            {
                if (prevClientTarget)
                {
                    m_AllowGround = prevAllowGround;
                    m_CurrentID = prevID;
                    m_CurFlags = prevFlags;

                    m_ClientTarget = true;

                    if (!m_Intercept)
                        CancelClientTarget();
                }
            }
            else
            {
                m_ClientTarget = true;

                if (m_Intercept)
                {
                    if (m_OnCancel != null)
                        m_OnCancel();
                    EndIntercept();
                    World.Player.SendMessage(MsgLevel.Error, LocString.OTTCancel);

                    m_FilterCancel.Add((uint) prevID);
                }
            }
        }

        public static void ResendTarget()
        {
            if (!m_ClientTarget || !m_HasTarget)
            {
                CancelClientTarget();
                m_ClientTarget = m_HasTarget = true;
                Client.Instance.SendToClient(new Target(m_CurrentID, m_AllowGround, m_CurFlags));
            }
        }

        private static TargetInfo _lastTarget = new TargetInfo();

        public static void OverheadTargetMessage(TargetInfo info)
        {
            if (info == null)
                return;

            if (Config.GetBool("ShowAttackTargetNewOnly") && info.Serial == _lastTarget.Serial)
                return;

            Mobile m = null;

            if (Config.GetBool("ShowAttackTargetOverhead") && info.Serial != 0 && info.Serial.IsMobile)
            {
                m = World.FindMobile(info.Serial);

                if (m == null)
                    return;

                World.Player.OverheadMessage(FriendsAgent.IsFriend(m) ? 63 : m.GetNotorietyColorInt(),
                    $"Target: {m.Name}");
            }

            if (Config.GetBool("ShowTextTargetIndicator") && info.Serial != 0 && info.Serial.IsMobile)
            {
                // lets not look it up again they had the previous feature enabled
                if (m == null)
                    m = World.FindMobile(info.Serial);

                if (m == null)
                    return;

                m.OverheadMessage(10, Config.GetString("TargetIndicatorFormat"));
            }

            _lastTarget = info;
        }
    }
}