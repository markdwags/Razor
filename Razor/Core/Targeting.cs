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
using Assistant.Agents;
using Assistant.Core;
using Assistant.Macros;
using Assistant.Scripts;

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

    public partial class Targeting
    {
        public const uint LocalTargID = 0x7FFFFFFF; // uid for target sent from razor

        public delegate void TargetResponseCallback(bool location, Serial serial, Point3D p, ushort gfxid);

        public delegate void CancelTargetCallback();

        private static CancelTargetCallback _onCancel;
        private static TargetResponseCallback _onTarget;

        private static bool _intercept;
        private static bool _hasTarget;
        private static bool _clientTarget;
        private static TargetInfo _lastTarget { get; set; }
        private static TargetInfo _lastGroundTarget;
        private static TargetInfo _lastBeneTarget;
        private static TargetInfo _lastHarmTarget;


        private static bool _fromGrabHotKey;

        private static bool _allowGround;
        private static uint _currentID;
        private static byte _curFlags;

        private static uint _previousID;
        private static bool _previousGround;
        private static byte _prevFlags;

        private static Serial _lastCombatant;

        private delegate bool QueueTarget();

        private static readonly QueueTarget TargetSelfAction = new QueueTarget(DoTargetSelf);
        private static readonly QueueTarget LastTargetAction = new QueueTarget(DoLastTarget);
        private static QueueTarget _queueTarget;


        public static uint SpellTargetID { get; set; } = 0;

        private static List<uint> _filterCancel = new List<uint>();

        public static bool HasTarget
        {
            get { return _hasTarget; }
        }

        public static short CursorType => _curFlags;

        public static TargetInfo LastTargetInfo
        {
            get { return _lastTarget; }
        }

        public static bool FromGrabHotKey
        {
            get { return _fromGrabHotKey; }
        }

        private static List<ushort> _monsterIds = new List<ushort>()
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

        public enum TargetType
        {
            Invalid, // invalid/across server line
            Innocent, //Blue
            GuildAlly, //Green,
            Attackable, //Attackable but not criminal (gray)
            Criminal, //gray
            Enemy, //orange
            Murderer, //red

            // Razor specfic
            NonFriendly, //Attackable, Criminal, Enemy, Murderer
            Friendly, //Innocent, Guild/Ally 
            Red, //Murderer
            Blue, //Innocent
            //Friend, //Friend list
            Gray, //Attackable, Criminal
            Grey, //Attackable, Criminal
            Green, //GuildAlly
            Guild, //GuildAlly
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


        private static void CombatantChange(PacketReader p, PacketHandlerEventArgs e)
        {
            Serial ser = p.ReadUInt32();
            if (ser.IsMobile && ser != World.Player.Serial && ser != Serial.Zero && ser != Serial.MinusOne)
                _lastCombatant = ser;
        }

        private static void AttackLastComb()
        {
            if (_lastCombatant.IsMobile)
            {
                Client.Instance.SendToServer(new AttackReq(_lastCombatant));
                ShowAttackOverhead(_lastCombatant);
            }
                
        }

        public static void AttackLastTarg()
        {
            TargetInfo targ;
            if (IsSmartTargetingEnabled())
            {
                // If Smart Targetting is being used we'll assume that the user would like to attack the harmful target.
                targ = _lastHarmTarget;

                // If there is no last harmful target, then we'll attack the last target.
                if (targ == null)
                    targ = _lastTarget;
            }
            else
            {
                targ = _lastTarget;
            }

            if (targ != null && targ.Serial.IsMobile)
            {
                Client.Instance.SendToServer(new AttackReq(targ.Serial));
                ShowAttackOverhead(targ.Serial);
            }
        }

        private static Serial _lastOverheadMessageAttack;

        public static void ShowAttackOverhead(Serial serial)
        {
            if (!Config.GetBool("ShowAttackTargetOverhead"))
                return;

            if (Config.GetBool("ShowAttackTargetNewOnly") && serial == _lastOverheadMessageAttack)
                return;

            Mobile m = World.FindMobile(serial);
            if (m == null)
                return;

            World.Player.OverheadMessage(FriendsManager.IsFriend(m.Serial) ? 63 : m.GetNotorietyColorInt(),
                $"Attack: {m.Name}");

            _lastOverheadMessageAttack = serial;
        }

        public static void OnClearQueue()
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
            _fromGrabHotKey = fromGrab;

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
            if (_intercept && _onCancel != null)
            {
                _onCancel();
                CancelOneTimeTarget();
            }

            if (_hasTarget && _currentID != 0 && _currentID != LocalTargID)
            {
                _previousID = _currentID;
                _previousGround = _allowGround;
                _prevFlags = _curFlags;

                _filterCancel.Add(_previousID);
            }

            _intercept = true;
            _currentID = LocalTargID;
            _onTarget = onTarget;
            _onCancel = onCancel;

            _clientTarget = _hasTarget = true;
            Client.Instance.SendToClient(new Target(LocalTargID, ground));
            ClearQueue();
        }

        internal static void CancelOneTimeTarget()
        {
            _clientTarget = _hasTarget = _fromGrabHotKey = false;

            Client.Instance.SendToClient(new CancelTarget(LocalTargID));
            EndIntercept();
        }

        private static bool _lastTargetWasSet;

        public static void TargetSetLastTarget()
        {
            if (World.Player != null)
            {
                _lastTargetWasSet = false;
                OneTimeTarget(false, OnSetLastTarget, OnSetLastTargetCancel);
                World.Player.SendMessage(MsgLevel.Force, LocString.TargSetLT);
            }
        }

        public static void SetLastTarget(Mobile mobile)
        {
            if (World.Player != null)
            {
                OnSetLastTarget(false, mobile.Serial, mobile.Position, mobile.Body);
            }
        }

        public static void SetLastTarget(Item item)
        {
            if (World.Player != null)
            {
                OnSetLastTarget(false, item.Serial, item.Position, item.ItemID.Value);
            }
        }

        public static void SetLastTarget(Serial serial)
        {
            if (World.Player != null)
            {
                OnSetLastTarget(false, serial, new Point3D(0, 0, 0), 0);
            }
        }

        private static void OnSetLastTargetCancel()
        {
            if (_lastTarget != null)
                _lastTargetWasSet = true;
        }

        private static void OnSetLastTarget(bool location, Serial serial, Point3D p, ushort gfxid)
        {
            if (serial == World.Player.Serial)
            {
                OnSetLastTargetCancel();
                return;
            }

            _lastBeneTarget = _lastHarmTarget = _lastGroundTarget = _lastTarget = new TargetInfo();
            _lastTarget.Flags = 0;
            _lastTarget.Gfx = gfxid;
            _lastTarget.Serial = serial;
            _lastTarget.Type = (byte) (location ? 1 : 0);
            _lastTarget.X = p.X;
            _lastTarget.Y = p.Y;
            _lastTarget.Z = p.Z;

            _lastTargetWasSet = true;

            World.Player.SendMessage(MsgLevel.Force, LocString.LTSet);

            if (serial.IsMobile)
            {
                LastTargetChanged();
                Client.Instance.SendToClient(new ChangeCombatant(serial));
                _lastCombatant = serial;
            }
        }

        private static bool m_LTBeneWasSet;

        /// <summary>
        /// Sets the beneficial target
        /// </summary>
        private static void SetLastTargetBeneficial()
        {
            if (!IsSmartTargetingEnabled())
            {
                World.Player.SendMessage(MsgLevel.Error, "Smart Targeting is disabled");
                return;
            }

            if (World.Player != null)
            {
                m_LTBeneWasSet = false;
                OneTimeTarget(false, OnSetLastTargetBeneficial, OnSLTBeneficialCancel);
                World.Player.SendMessage(MsgLevel.Force, LocString.NewBeneficialTarget);
            }
        }

        private static void OnSLTBeneficialCancel()
        {
            if (_lastBeneTarget != null)
                m_LTBeneWasSet = true;
        }

        private static void OnSetLastTargetBeneficial(bool location, Serial serial, Point3D p, ushort gfxid)
        {
            if (serial == World.Player.Serial)
            {
                OnSLTBeneficialCancel();
                return;
            }

            _lastBeneTarget = new TargetInfo
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
            if (!IsSmartTargetingEnabled())
            {
                World.Player.SendMessage(MsgLevel.Error, "Smart Targeting is disabled");
                return;
            }

            if (World.Player != null)
            {
                OneTimeTarget(false, OnSetLastTargetHarmful, OnSLTHarmfulCancel);
                World.Player.SendMessage(MsgLevel.Force, LocString.NewHarmfulTarget);
            }
        }

        private static void OnSLTHarmfulCancel()
        {
            if (_lastTarget != null)
                m_LTHarmWasSet = true;
        }

        private static void OnSetLastTargetHarmful(bool location, Serial serial, Point3D p, ushort gfxid)
        {
            if (serial == World.Player.Serial)
            {
                OnSLTHarmfulCancel();
                return;
            }

            _lastHarmTarget = new TargetInfo
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

                if (IsSmartTargetingEnabled())
                {
                    if (_lastHarmTarget != null && _lastHarmTarget.Serial == m.Serial)
                    {
                        oplchanged = true;
                        m.ObjPropList.Add(Language.GetString(LocString.HarmfulTarget));
                    }

                    if (_lastBeneTarget != null && _lastBeneTarget.Serial == m.Serial)
                    {
                        oplchanged = true;
                        m.ObjPropList.Add(Language.GetString(LocString.BeneficialTarget));
                    }
                }

                if (!oplchanged && _lastTarget != null && _lastTarget.Serial == m.Serial)
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
            if (_lastTarget != null)
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

                if (_lastTarget.Serial.IsItem)
                {
                    AddTextFlags(World.FindItem(_lastTarget.Serial));
                }
                else
                {
                    Mobile m = World.FindMobile(_lastTarget.Serial);
                    if (m != null)
                    {
                        if (IsLastTarget(m) && lth)
                            Client.Instance.SendToClient(new MobileIncoming(m));

                        CheckLastTargetRange(m);

                        AddTextFlags(m);
                    }
                }

                m_OldLT = _lastTarget.Serial;
            }
        }

        private static void LastBeneficialTargetChanged()
        {
            if (_lastBeneTarget != null)
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

                if (_lastBeneTarget.Serial.IsItem)
                {
                    AddTextFlags(World.FindItem(_lastBeneTarget.Serial));
                }
                else
                {
                    Mobile m = World.FindMobile(_lastBeneTarget.Serial);
                    if (m != null)
                    {
                        CheckLastTargetRange(m);

                        AddTextFlags(m);
                    }
                }

                m_OldBeneficialLT = _lastBeneTarget.Serial;
            }
        }

        private static void LastHarmfulTargetChanged()
        {
            if (_lastHarmTarget != null)
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

                if (_lastHarmTarget.Serial.IsItem)
                {
                    AddTextFlags(World.FindItem(_lastHarmTarget.Serial));
                }
                else
                {
                    Mobile m = World.FindMobile(_lastHarmTarget.Serial);
                    if (m != null)
                    {
                        CheckLastTargetRange(m);

                        AddTextFlags(m);
                    }
                }

                m_OldHarmfulLT = _lastHarmTarget.Serial;
            }
        }


        public static bool LastTargetWasSet
        {
            get { return _lastTargetWasSet; }
        }


        public static void SetLastTargetTo(Mobile m)
        {
            SetLastTargetTo(m, 0);
        }

        public static void SetLastTargetTo(Mobile m, byte flagType)
        {
            TargetInfo targ = new TargetInfo();
            _lastGroundTarget = _lastTarget = targ;

            if ((_hasTarget && _curFlags == 1) || flagType == 1)
                _lastHarmTarget = targ;
            else if ((_hasTarget && _curFlags == 2) || flagType == 2)
                _lastBeneTarget = targ;
            else if (flagType == 0)
                _lastHarmTarget = _lastBeneTarget = targ;

            targ.Type = 0;
            if (_hasTarget)
                targ.Flags = _curFlags;
            else
                targ.Flags = flagType;

            targ.Gfx = m.Body;
            targ.Serial = m.Serial;
            targ.X = m.Position.X;
            targ.Y = m.Position.Y;
            targ.Z = m.Position.Z;

            Client.Instance.SendToClient(new ChangeCombatant(m));
            _lastCombatant = m.Serial;
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
            _intercept = false;
            _onTarget = null;
            _onCancel = null;
            _fromGrabHotKey = false;
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

            if (_hasTarget)
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

                _queueTarget = TargetSelfAction;
            }
        }

        public static bool DoTargetSelf()
        {
            if (World.Player == null)
                return false;

            if (CheckHealPoisonTarg(_currentID, World.Player.Serial))
                return false;

            CancelClientTarget();
            _hasTarget = false;
            _fromGrabHotKey = false;

            if (_intercept)
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
                Client.Instance.SendToServer(new TargetResponse(_currentID, World.Player));
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

            if (FromGrabHotKey)
                return;

            if (_hasTarget)
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

                _queueTarget = LastTargetAction;
            }
        }

        public static bool DoLastTarget()
        {
            if (FromGrabHotKey)
                return true;

            TargetInfo targ;
            if (IsSmartTargetingEnabled())
            {
                if (_allowGround && _lastGroundTarget != null)
                    targ = _lastGroundTarget;
                else if (_curFlags == 1)
                    targ = _lastHarmTarget;
                else if (_curFlags == 2)
                    targ = _lastBeneTarget;
                else
                    targ = _lastTarget;

                if (targ == null)
                    targ = _lastTarget;
            }
            else
            {
                if (_allowGround && _lastGroundTarget != null)
                    targ = _lastGroundTarget;
                else
                    targ = _lastTarget;
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
                if (!_allowGround && (targ.Serial == Serial.Zero || targ.Serial >= 0x80000000))
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
                    _queueTarget = LastTargetAction;
                World.Player.SendMessage(MsgLevel.Warning, LocString.LTOutOfRange);
                return false;
            }

            if (CheckHealPoisonTarg(_currentID, targ.Serial))
                return false;

            CancelClientTarget();
            _hasTarget = false;

            targ.TargID = _currentID;

            if (_intercept)
                OneTimeResponse(targ);
            else
                Client.Instance.SendToServer(new TargetResponse(targ));
            return true;
        }

        public static void ClearQueue()
        {
            _queueTarget = null;
        }

        private static TimerCallbackState m_OneTimeRespCallback = new TimerCallbackState(OneTimeResponse);

        private static void OneTimeResponse(object state)
        {
            TargetInfo info = state as TargetInfo;

            if (info != null)
            {
                if ((info.X == 0xFFFF && info.X == 0xFFFF) && (info.Serial == 0 || info.Serial >= 0x80000000))
                {
                    if (_onCancel != null)
                        _onCancel();
                }
                else
                {
                    if (Macros.MacroManager.AcceptActions)
                        MacroManager.Action(new AbsoluteTargetAction(info));

                    ScriptManager.AddToScript($"target {info.Serial}");

                    if (_onTarget != null)
                        _onTarget(info.Type == 1 ? true : false, info.Serial, new Point3D(info.X, info.Y, info.Z),
                            info.Gfx);
                }
            }

            EndIntercept();
        }

        public static void CancelTarget()
        {
            OnClearQueue();
            CancelClientTarget();

            _fromGrabHotKey = false;

            ScriptManager.SetVariableActive = false;

            if (_hasTarget)
            {
                Client.Instance.SendToServer(new TargetCancelResponse(_currentID));
                _hasTarget = false;
            }
        }

        private static void CancelClientTarget()
        {
            if (_clientTarget)
            {
                _filterCancel.Add((uint) _currentID);
                Client.Instance.SendToClient(new CancelTarget(_currentID));
                _clientTarget = false;
            }
        }

        public static void Target(TargetInfo info)
        {
            if (_intercept)
            {
                OneTimeResponse(info);
            }
            else if (_hasTarget)
            {
                info.TargID = _currentID;
                _lastGroundTarget = _lastTarget = info;
                Client.Instance.SendToServer(new TargetResponse(info));
            }

            CancelClientTarget();
            _hasTarget = false;
            _fromGrabHotKey = false;
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

        private static DateTime _lastFlagCheck = DateTime.UtcNow;
        private static Serial _lastFlagCheckSerial;

        public static void CheckTextFlags(Mobile m)
        {
            if (DateTime.UtcNow - _lastFlagCheck < TimeSpan.FromMilliseconds(250) && m.Serial == _lastFlagCheckSerial)
                return;

            if (IgnoreAgent.IsIgnored(m.Serial))
            {
                m.OverheadMessage(Config.GetInt("SysColor"), "[Ignored]");
            }

            if (IsSmartTargetingEnabled())
            {
                bool harm = _lastHarmTarget != null && _lastHarmTarget.Serial == m.Serial;
                bool bene = _lastBeneTarget != null && _lastBeneTarget.Serial == m.Serial;

                if (harm)
                    m.OverheadMessage(0x90, $"[{Language.GetString(LocString.HarmfulTarget)}]");
                if (bene)
                    m.OverheadMessage(0x3F, $"[{Language.GetString(LocString.BeneficialTarget)}]");
            }

            if (_lastTarget != null && _lastTarget.Serial == m.Serial)
                m.OverheadMessage(0x3B2, $"[{Language.GetString(LocString.LastTarget)}]");

            _lastFlagCheck = DateTime.UtcNow;
            _lastFlagCheckSerial = m.Serial;
        }

        public static bool IsLastTarget(Mobile m)
        {
            if (m != null)
            {
                if (IsSmartTargetingEnabled())
                {
                    if (_lastHarmTarget != null && _lastHarmTarget.Serial == m.Serial)
                        return true;
                }
                else
                {
                    if (_lastTarget != null && _lastTarget.Serial == m.Serial)
                        return true;
                }
            }

            return false;
        }

        public static bool IsBeneficialTarget(Mobile m)
        {
            if (m != null)
            {
                if (IsSmartTargetingEnabled())
                {
                    if (_lastBeneTarget != null && _lastBeneTarget.Serial == m.Serial)
                        return true;
                }
                else
                {
                    if (_lastTarget != null && _lastTarget.Serial == m.Serial)
                        return true;
                }
            }

            return false;
        }

        public static bool IsHarmfulTarget(Mobile m)
        {
            if (m != null)
            {
                if (IsSmartTargetingEnabled())
                {
                    if (_lastHarmTarget != null && _lastHarmTarget.Serial == m.Serial)
                        return true;
                }
                else
                {
                    if (_lastTarget != null && _lastTarget.Serial == m.Serial)
                        return true;
                }
            }

            return false;
        }

        public static void CheckLastTargetRange(Mobile m)
        {
            if (World.Player == null)
                return;

            if (_hasTarget && m != null && _lastTarget != null && m.Serial == _lastTarget.Serial &&
                _queueTarget == LastTargetAction)
            {
                if (Config.GetBool("RangeCheckLT") && Client.Instance.AllowBit(FeatureBit.RangeCheckLT))
                {
                    if (Utility.InRange(World.Player.Position, m.Position, Config.GetInt("LTRange")))
                    {
                        if (_queueTarget())
                            ClearQueue();
                    }
                }
            }
        }

        private static bool CheckHealPoisonTarg(uint targID, Serial ser)
        {
            if (World.Player == null)
                return false;

            if (targID == SpellTargetID && ser.IsMobile &&
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
            if (World.Player == null)
                return;

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

            _clientTarget = false;

            OverheadTargetMessage(info);

            // check for cancel
            if (info.X == 0xFFFF && info.X == 0xFFFF && (info.Serial <= 0 || info.Serial >= 0x80000000))
            {
                _hasTarget = false;
                _fromGrabHotKey = false;

                if (_intercept)
                {
                    args.Block = true;
                    Timer.DelayedCallbackState(TimeSpan.Zero, m_OneTimeRespCallback, info).Start();
                    EndIntercept();

                    if (_previousID != 0)
                    {
                        _currentID = _previousID;
                        _allowGround = _previousGround;
                        _curFlags = _prevFlags;

                        _previousID = 0;

                        ResendTarget();
                    }
                }
                else if (_filterCancel.Contains((uint) info.TargID) || info.TargID == LocalTargID)
                {
                    args.Block = true;
                }

                _filterCancel.Clear();
                return;
            }

            ClearQueue();

            if (_intercept)
            {
                if (info.TargID == LocalTargID)
                {
                    Timer.DelayedCallbackState(TimeSpan.Zero, m_OneTimeRespCallback, info).Start();

                    _hasTarget = false;
                    _fromGrabHotKey = false;
                    args.Block = true;

                    if (_previousID != 0)
                    {
                        _currentID = _previousID;
                        _allowGround = _previousGround;
                        _curFlags = _prevFlags;

                        _previousID = 0;

                        ResendTarget();
                    }

                    _filterCancel.Clear();

                    return;
                }
                else
                {
                    EndIntercept();
                }
            }

            _hasTarget = false;

            if (CheckHealPoisonTarg(_currentID, info.Serial))
            {
                ResendTarget();
                args.Block = true;
            }

            if (info.Serial != World.Player.Serial)
            {
                if (info.Serial.IsValid)
                {
                    // only let lasttarget be a non-ground target

                    _lastTarget = info;
                    if (info.Flags == 1)
                        _lastHarmTarget = info;
                    else if (info.Flags == 2)
                        _lastBeneTarget = info;

                    LastTargetChanged();
                    LastBeneficialTargetChanged();
                    LastHarmfulTargetChanged();
                }

                _lastGroundTarget = info; // ground target is the true last target

                if (Macros.MacroManager.AcceptActions)
                    MacroManager.Action(new AbsoluteTargetAction(info));

                ScriptManager.AddToScript(info.Serial == Serial.Zero
                    ? $"target 0x0 {info.X} {info.Y} {info.Z}"
                    : $"target {info.Serial}");


                if (ScriptManager.Recording)
                {
                    if (info.Serial == Serial.Zero)
                    {
                    }
                    else
                    {
                    }
                }
            }
            else
            {
                KeyData hk = HotKey.Get((int)LocString.TargetSelf);

                if (MacroManager.AcceptActions)
                {
                    if (hk != null)
                    {
                        MacroManager.Action(new HotKeyAction(hk));
                    }
                    else
                    {
                        MacroManager.Action(new AbsoluteTargetAction(info));
                    }
                }

                ScriptManager.AddToScript(hk != null ? "target 'self'" : $"target {info.Serial}");
            }

            if (World.Player.LastSpell == 52 && !GateTimer.Running)
            {
                GateTimer.Start();
            }

            _filterCancel.Clear();
        }

        private static void NewTarget(PacketReader p, PacketHandlerEventArgs args)
        {
            bool prevAllowGround = _allowGround;
            uint prevID = _currentID;
            byte prevFlags = _curFlags;
            bool prevClientTarget = _clientTarget;

            _allowGround = p.ReadBoolean(); // allow ground
            _currentID = p.ReadUInt32(); // target uid
            _curFlags = p.ReadByte(); // flags
            // the rest of the packet is 0s

            // check for a server cancel command
            if (!_allowGround && _currentID == 0 && _curFlags == 3)
            {
                _hasTarget = false;
                _fromGrabHotKey = false;

                _clientTarget = false;
                if (_intercept)
                {
                    EndIntercept();
                    World.Player.SendMessage(MsgLevel.Error, LocString.OTTCancel);
                }

                return;
            }

            if (Spell.LastCastTime + TimeSpan.FromSeconds(3.0) > DateTime.UtcNow &&
                Spell.LastCastTime + TimeSpan.FromSeconds(0.5) <= DateTime.UtcNow && SpellTargetID == 0)
                SpellTargetID = _currentID;

            _hasTarget = true;
            _clientTarget = false;

            if (_queueTarget == null && Macros.MacroManager.AcceptActions &&
                MacroManager.Action(new WaitForTargetAction()))
            {
                args.Block = true;
            }
            else if (_queueTarget == null && ScriptManager.AddToScript("waitfortarget"))
            {
                //args.Block = true;
            }
            else if (_queueTarget != null && _queueTarget())
            {
                ClearQueue();
                args.Block = true;
            }

            if (args.Block)
            {
                if (prevClientTarget)
                {
                    _allowGround = prevAllowGround;
                    _currentID = prevID;
                    _curFlags = prevFlags;

                    _clientTarget = true;

                    if (!_intercept)
                        CancelClientTarget();
                }
            }
            else
            {
                _clientTarget = true;

                if (_intercept)
                {
                    if (_onCancel != null)
                        _onCancel();
                    EndIntercept();
                    World.Player.SendMessage(MsgLevel.Error, LocString.OTTCancel);

                    _filterCancel.Add((uint) prevID);
                }
            }
        }

        public static void ResendTarget()
        {
            if (!_clientTarget || !_hasTarget)
            {
                CancelClientTarget();
                _clientTarget = _hasTarget = true;
                Client.Instance.SendToClient(new Target(_currentID, _allowGround, _curFlags));
            }
        }

        private static TargetInfo _lastOverheadMessageTarget = new TargetInfo();

        public static void OverheadTargetMessage(TargetInfo info)
        {
            if (info == null)
                return;

            if (Config.GetBool("ShowAttackTargetNewOnly") && info.Serial == _lastOverheadMessageTarget.Serial)
                return;

            Mobile m = null;

            if (Config.GetBool("ShowAttackTargetOverhead") && info.Serial != 0 && info.Serial.IsMobile)
            {
                m = World.FindMobile(info.Serial);

                if (m == null)
                    return;

                World.Player.OverheadMessage(FriendsManager.IsFriend(m.Serial) ? 63 : m.GetNotorietyColorInt(),
                    $"Target: {m.Name}");
            }

            if (Config.GetBool("ShowTextTargetIndicator") && info.Serial != 0 && info.Serial.IsMobile)
            {
                // lets not look it up again they had the previous feature enabled
                if (m == null)
                    m = World.FindMobile(info.Serial);

                if (m == null)
                    return;

                m.OverheadMessage(Config.GetInt("TargetIndicatorHue"),
                    Config.GetString("TargetIndicatorFormat").Replace("{name}", m.Name));
            }

            _lastOverheadMessageTarget = info;
        }

        private static bool IsSmartTargetingEnabled()
        {
            return Config.GetBool("SmartLastTarget") && Client.Instance.AllowBit(FeatureBit.SmartLT);
        }
    }
}