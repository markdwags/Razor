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
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Assistant.Agents;
using Assistant.Core;
using Assistant.Gumps;
using Assistant.Macros;
using Assistant.UI;

namespace Assistant
{
    public enum LockType : byte
    {
        Up = 0,
        Down = 1,
        Locked = 2
    }

    public enum MsgLevel
    {
        Debug = 0,
        Info = 0,
        Warning = 1,
        Error = 2,
        Force = 3,
        Friend = 4
    }

    public class Skill
    {
        private LockType _lock;
        private ushort _value;
        private ushort _base;
        private ushort _cap;
        private short _delta;
        private int _index;

        public Skill(int idx)
        {
            _index = idx;
        }

        public int Index
        {
            get { return _index; }
        }

        public LockType Lock
        {
            get { return _lock; }
            set { _lock = value; }
        }

        public ushort FixedValue
        {
            get { return _value; }
            set { _value = value; }
        }

        public ushort FixedBase
        {
            get { return _base; }
            set
            {
                _delta += (short) (value - _base);
                _base = value;
            }
        }

        public ushort FixedCap
        {
            get { return _cap; }
            set { _cap = value; }
        }

        public double Value
        {
            get { return _value / 10.0; }
            set { _value = (ushort) (value * 10.0); }
        }

        public double Base
        {
            get { return _base / 10.0; }
            set { _base = (ushort) (value * 10.0); }
        }

        public double Cap
        {
            get { return _cap / 10.0; }
            set { _cap = (ushort) (value * 10.0); }
        }

        public double Delta
        {
            get { return _delta / 10.0; }
            set { _delta = (short) (value * 10); }
        }
    }

    public enum MaleSounds
    {
        Ah = 0x419,
        Ahha = 0x41A,
        Applaud = 0x41B,
        Blownose = 0x41C,
        Burp = 0x41D,
        Cheer = 0x41E,
        Clear_Throat = 0x41F,
        Cough = 0x420,
        Coughbs = 0x421,
        Cry = 0x422,
        Death_01 = 0x423,
        Death_02 = 0x424,
        Death_03 = 0x425,
        Death_04 = 0x426,
        Death_05 = 0x427,
        Fart = 0x428,
        Gasp = 0x429,
        Giggle = 0x42A,
        Groan = 0x42B,
        Growl = 0x42C,
        Hey = 0x42D,
        Hiccup = 0x42E,
        Huh = 0x42F,
        Kiss = 0x430,
        Laugh = 0x431,
        No = 0x432,
        Oh = 0x433,
        Oomph_01 = 0x434,
        Oomph_02 = 0x435,
        Oomph_03 = 0x436,
        Oomph_04 = 0x437,
        Oomph_05 = 0x438,
        Oomph_06 = 0x439,
        Oomph_07 = 0x43A,
        Oomph_08 = 0x43B,
        Oomph_09 = 0x43C,
        Oooh = 0x43D,
        Oops = 0x43E,
        Puke = 0x43F,
        Scream = 0x440,
        Shush = 0x441,
        Sigh = 0x442,
        Sneeze = 0x443,
        Sniff = 0x444,
        Snore = 0x445,
        Spit = 0x446,
        Whistle = 0x447,
        Yawn = 0x448,
        Yea = 0x449,
        Yell = 0x44A
    }

    public enum FemaleSounds
    {
        Ah = 0x30A,
        Ahha = 0x30B,
        Applaud = 0x30C,
        Blownose = 0x30D,
        Burp = 0x30E,
        Cheer = 0x30F,
        Clear_Throat = 0x310,
        Cough = 0x311,
        Coughbs = 0x312,
        Cry = 0x313,
        Death_01 = 0x314,
        Death_02 = 0x315,
        Death_03 = 0x316,
        Death_04 = 0x317,
        Fart = 0x318,
        Gasp = 0x319,
        Giggle = 0x31A,
        Groan = 0x31B,
        Growl = 0x31C,
        Hey = 0x31D,
        Hiccup = 0x31E,
        Huh = 0x31F,
        Kiss = 0x320,
        Laugh = 0x321,
        No = 0x322,
        Oh = 0x323,
        Oomph_01 = 0x324,
        Oomph_02 = 0x325,
        Oomph_03 = 0x326,
        Oomph_04 = 0x327,
        Oomph_05 = 0x328,
        Oomph_06 = 0x329,
        Oomph_07 = 0x32A,
        Oooh = 0x32B,
        Oops = 0x32C,
        Puke = 0x32D,
        Scream = 0x32E,
        Shush = 0x32F,
        Sigh = 0x330,
        Sneeze = 0x331,
        Sniff = 0x332,
        Snore = 0x333,
        Spit = 0x334,
        Whistle = 0x335,
        Yawn = 0x336,
        Yea = 0x337,
        Yell = 0x338
    }

    public class PlayerData : Mobile
    {
        public int VisRange = 18;

        public int MultiVisRange
        {
            get { return VisRange + 5; }
        }

        private int _maxWeight = -1;

        private short _fireResist, _coldResist, _poisonResist, _energyResist, _luck;
        private ushort _damageMin, _damageMax;

        private ushort _str, _dex, _int;
        private LockType _strLock, _dexLock, _intLock;
        private uint _gold;
        private ushort _weight;
        private Skill[] _skills;
        private ushort _ar;
        private ushort _statCap;
        private byte _followers;
        private byte _followersMax;
        private int _tithe;
        private sbyte _localLight;
        private byte _globalLight;
        private uint _features;
        private byte _season;
        private byte _defaultSeason;
        private int[] _papPatches = new int[10];


        private bool _skillsSent;
        private Timer _criminalTime;
        private DateTime _criminalStart = DateTime.MinValue;

        private List<BuffDebuff> _buffsDebuffs = new List<BuffDebuff>();

        internal List<BuffDebuff> BuffsDebuffs
        {
            get { return _buffsDebuffs; }
        }

        private List<uint> _openedCorpses = new List<uint>();

        public List<uint> OpenedCorpses
        {
            get { return _openedCorpses; }
        }

        public PlayerData(Serial serial) : base(serial)
        {
            _skills = new Skill[Ultima.Skills.TotalSkills()];
            for (int i = 0; i < _skills.Length; i++)
                _skills[i] = new Skill(i);
        }

        public ushort Str
        {
            get { return _str; }
            set { _str = value; }
        }

        public ushort Dex
        {
            get { return _dex; }
            set { _dex = value; }
        }

        public ushort Int
        {
            get { return _int; }
            set { _int = value; }
        }

        public uint Gold
        {
            get { return _gold; }
            set { _gold = value; }
        }

        public ushort Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        public ushort MaxWeight
        {
            get
            {
                if (_maxWeight == -1)
                    return (ushort) ((_str * 3.5) + 40);
                else
                    return (ushort) _maxWeight;
            }
            set { _maxWeight = value; }
        }

        public short FireResistance
        {
            get { return _fireResist; }
            set { _fireResist = value; }
        }

        public short ColdResistance
        {
            get { return _coldResist; }
            set { _coldResist = value; }
        }

        public short PoisonResistance
        {
            get { return _poisonResist; }
            set { _poisonResist = value; }
        }

        public short EnergyResistance
        {
            get { return _energyResist; }
            set { _energyResist = value; }
        }

        public short Luck
        {
            get { return _luck; }
            set { _luck = value; }
        }

        public ushort DamageMin
        {
            get { return _damageMin; }
            set { _damageMin = value; }
        }

        public ushort DamageMax
        {
            get { return _damageMax; }
            set { _damageMax = value; }
        }

        public LockType StrLock
        {
            get { return _strLock; }
            set { _strLock = value; }
        }

        public LockType DexLock
        {
            get { return _dexLock; }
            set { _dexLock = value; }
        }

        public LockType IntLock
        {
            get { return _intLock; }
            set { _intLock = value; }
        }

        public ushort StatCap
        {
            get { return _statCap; }
            set { _statCap = value; }
        }

        public ushort AR
        {
            get { return _ar; }
            set { _ar = value; }
        }

        public byte Followers
        {
            get { return _followers; }
            set { _followers = value; }
        }

        public byte FollowersMax
        {
            get { return _followersMax; }
            set { _followersMax = value; }
        }

        public int Tithe
        {
            get { return _tithe; }
            set { _tithe = value; }
        }

        public Skill[] Skills
        {
            get { return _skills; }
        }

        public bool SkillsSent
        {
            get { return _skillsSent; }
            set { _skillsSent = value; }
        }

        public int CriminalTime
        {
            get
            {
                if (_criminalStart != DateTime.MinValue)
                {
                    int sec = (int) (DateTime.UtcNow - _criminalStart).TotalSeconds;
                    if (sec > 300)
                    {
                        if (_criminalTime != null)
                            _criminalTime.Stop();
                        _criminalStart = DateTime.MinValue;
                        return 0;
                    }
                    else
                    {
                        return sec;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        private class DoorOpenTimer : Timer
        {
            // Fire off once in 5ms
            public DoorOpenTimer() : base(TimeSpan.FromMilliseconds(5), 1)
            {
            }

            protected override void OnTick()
            {
                Client.Instance.SendToServer(new OpenDoorMacro());
            }
        }

        private readonly DoorOpenTimer _doorTimer = new DoorOpenTimer();

        private void AutoOpenDoors(bool onDirChange)
        {
            if (!Visible && !Config.GetBool("AutoOpenDoorWhenHidden"))
                return;

            if (Body != 0x03DB && !IsGhost && ((int) (Direction & Direction.Mask)) % 2 == 0 &&
                Config.GetBool("AutoOpenDoors") &&
                Client.Instance.AllowBit(FeatureBit.AutoOpenDoors))
            {
                int x = Position.X, y = Position.Y, z = Position.Z;

                /* Check if one more tile in the direction we just moved is a door */
                Utility.Offset(Direction, ref x, ref y);

                if (World.Items.Values.Any(s =>
                    s.IsDoor && s.Position.X == x && s.Position.Y == y && s.Position.Z - 15 <= z &&
                    s.Position.Z + 15 >= z))
                {
                    if (Client.IsOSI)
                    {
                        Client.Instance.SendToServer(new OpenDoorMacro());
                    }
                    else
                    {
                        // ClassicUO requires a slight pause before attempting to
                        // open a door after a direction change
                        if (onDirChange)
                        {
                            _doorTimer.Start();
                        }
                        else
                        {
                            Client.Instance.SendToServer(new OpenDoorMacro());
                        }
                    }
                }
            }
        }

        public override void OnPositionChanging(Point3D oldPos)
        {
            if (!IsGhost)
                StealthSteps.OnMove();

            AutoOpenDoors(false);

            List<Mobile> mlist = new List<Mobile>(World.Mobiles.Values);
            for (int i = 0; i < mlist.Count; i++)
            {
                Mobile m = mlist[i];
                if (m != this)
                {
                    if (!Utility.InRange(m.Position, Position, VisRange))
                        m.Remove();
                    else
                        Targeting.CheckLastTargetRange(m);
                }
            }

            mlist = null;


            List<Item> ilist = new List<Item>(World.Items.Values);
            ScavengerAgent s = ScavengerAgent.Instance;
            for (int i = 0; i < ilist.Count; i++)
            {
                Item item = ilist[i];
                if (item.Deleted || item.Container != null)
                    continue;

                int dist = Utility.Distance(item.GetWorldPosition(), Position);
                if (item != DragDropManager.Holding && (dist > MultiVisRange || (!item.IsMulti && dist > VisRange)))
                    item.Remove();
                else if (!IsGhost && Visible && dist <= 2 && s.Enabled && item.Movable)
                    s.Scavenge(item);
            }

            ilist = null;

            if (Engine.MainWindow != null && Engine.MainWindow.MapWindow != null)
                Engine.MainWindow.SafeAction(f => f.MapWindow.PlayerMoved());

            base.OnPositionChanging(oldPos);
        }

        public override void OnDirectionChanging(Direction oldDir)
        {
            AutoOpenDoors(true);
        }

        public override void OnMapChange(byte old, byte cur)
        {
            List<Mobile> list = new List<Mobile>(World.Mobiles.Values);
            for (int i = 0; i < list.Count; i++)
            {
                Mobile m = list[i];
                if (m != this && m.Map != cur)
                    m.Remove();
            }

            list = null;

            World.Items.Clear();
            Counter.Reset();
            for (int i = 0; i < Contains.Count; i++)
            {
                Item item = (Item) Contains[i];
                World.AddItem(item);
                item.Contains.Clear();
            }

            if (Config.GetBool("AutoSearch") && Backpack != null)
                PlayerData.DoubleClick(Backpack);

            UOAssist.PostMapChange(cur);

            if (Engine.MainWindow != null && Engine.MainWindow.MapWindow != null)
                Engine.MainWindow.SafeAction(s => s.MapWindow.PlayerMoved());
        }

        /*public override void OnMapChange( byte old, byte cur )
        {
             World.Mobiles.Clear();
             World.Items.Clear();
             Counter.Reset();

             Contains.Clear();

             World.AddMobile( this );

             UOAssist.PostMapChange( cur );
        }*/

        protected override void OnNotoChange(byte old, byte cur)
        {
            if ((old == 3 || old == 4) && (cur != 3 && cur != 4))
            {
                // grey is turning off
                // SendMessage( "You are no longer a criminal." );
                if (_criminalTime != null)
                    _criminalTime.Stop();
                _criminalStart = DateTime.MinValue;
                Client.Instance.RequestTitlebarUpdate();
            }
            else if ((cur == 3 || cur == 4) && (old != 3 && old != 4 && old != 0))
            {
                // grey is turning on
                ResetCriminalTimer();
            }
        }

        public void ResetCriminalTimer()
        {
            if (_criminalStart == DateTime.MinValue || DateTime.UtcNow - _criminalStart >= TimeSpan.FromSeconds(1))
            {
                _criminalStart = DateTime.UtcNow;
                if (_criminalTime == null)
                    _criminalTime = new CriminalTimer(this);
                _criminalTime.Start();
                Client.Instance.RequestTitlebarUpdate();
            }
        }

        private class CriminalTimer : Timer
        {
            private PlayerData _player;

            public CriminalTimer(PlayerData player) : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
            {
                _player = player;
            }

            protected override void OnTick()
            {
                Client.Instance.RequestTitlebarUpdate();
            }
        }

        internal void SendMessage(MsgLevel lvl, LocString loc, params object[] args)
        {
            SendMessage(lvl, Language.Format(loc, args));
        }

        internal void SendMessage(MsgLevel lvl, LocString loc)
        {
            SendMessage(lvl, Language.GetString(loc));
        }

        internal void SendMessage(LocString loc, params object[] args)
        {
            SendMessage(MsgLevel.Info, Language.Format(loc, args));
        }

        internal void SendMessage(LocString loc)
        {
            SendMessage(MsgLevel.Info, Language.GetString(loc));
        }

        internal void SendMessage(int hue, string text)
        {
            Client.Instance.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, hue, 3,
                Language.CliLocName, "System", text));
        }

        internal void SendMessage(MsgLevel lvl, string format, params object[] args)
        {
            SendMessage(lvl, String.Format(format, args));
        }

        internal void SendMessage(string format, params object[] args)
        {
            SendMessage(MsgLevel.Info, String.Format(format, args));
        }

        internal void SendMessage(string text)
        {
            SendMessage(MsgLevel.Info, text);
        }

        internal void SendMessage(MsgLevel lvl, string text)
        {
            if (lvl >= (MsgLevel) Config.GetInt("MessageLevel") && text.Length > 0)
            {
                int hue;
                switch (lvl)
                {
                    case MsgLevel.Error:
                    case MsgLevel.Warning:
                        hue = Config.GetInt("WarningColor");
                        break;
                    case MsgLevel.Friend:
                        hue = 63;
                        break;
                    default:
                        hue = Config.GetInt("SysColor");
                        break;
                }

                SystemMessages.Add(text);

                if (Config.GetBool("FilterRazorMessages"))
                {
                    if (!MessageQueue.Enqueue(0xFFFFFFFF, null, 0, MessageType.Regular, (ushort) hue, 3,
                        Language.CliLocName, "System", text))
                    {
                        return;
                    }
                }

                Client.Instance.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, hue, 3,
                    Language.CliLocName, "System", text));
            }
        }

        internal void Say(int hue, string msg)
        {
            List<ushort> keywords = EncodedSpeech.GetKeywords(msg);

            if (keywords.Count == 1 && (int) keywords[0] == 0)
            {
                Client.Instance.SendToServer(new ClientUniEncodedCommandMessage(MessageType.Regular, hue, 3,
                    Language.CliLocName, keywords, msg));
            }
            else
            {
                Client.Instance.SendToServer(new ClientUniEncodedCommandMessage(MessageType.Encoded, hue, 3,
                    Language.CliLocName, keywords, msg));
            }
        }

        internal void Say(string msg)
        {
            Say(Config.GetInt("SpeechHue"), msg);
        }

        internal void Whisper(string msg, int hue)
        {
            Client.Instance.SendToServer(new ClientUniMessage(MessageType.Whisper, hue, 3,
                Language.CliLocName, new ArrayList(), msg));
        }

        internal void Yell(string msg, int hue)
        {
            Client.Instance.SendToServer(new ClientUniMessage(MessageType.Yell, hue, 3,
                Language.CliLocName, new ArrayList(), msg));
        }

        internal void Emote(string msg, int hue)
        {
            msg = $"*{msg}*";

            Client.Instance.SendToServer(new ClientUniMessage(MessageType.Emote, hue, 3,
                Language.CliLocName, new ArrayList(), msg));
        }

        internal void Guild(string msg, int hue)
        {
            Client.Instance.SendToServer(new ClientUniMessage(MessageType.Guild, hue, 3,
                Language.CliLocName, new ArrayList(), msg));
        }

        internal void Alliance(string msg, int hue)
        {
            Client.Instance.SendToServer(new ClientUniMessage(MessageType.Alliance, hue, 3,
                Language.CliLocName, new ArrayList(), msg));
        }

        public uint CurrentGumpS, CurrentGumpI;
        public GumpResponseAction LastGumpResponseAction;
        public bool HasGump;
        public bool HasCompressedGump;
        public List<string> CurrentGumpStrings = new List<string>(); 
        public string CurrentGumpRawData;
        public uint CurrentMenuS;
        public ushort CurrentMenuI;
        public bool HasMenu;

        public bool HasPrompt;
        public uint PromptSenderSerial;
        public uint PromptID;
        public uint PromptType;
        public string PromptInputText;

        public Dictionary<uint, GumpInfo> GumpList { get; set; } = new Dictionary<uint, GumpInfo>();

        public class GumpInfo
        {
            private uint _serial;
            private uint _gumpId;
            List<string> _gumpContext;

            public uint GumpSerial
            {
                set => _serial = value;
                get => _serial;
            }

            public uint GumpId
            {
                set => _gumpId = value;
                get => _gumpId;
            }

            public List<string> GumpContext
            {
                set => _gumpContext = value;
                get => _gumpContext;
            }

            public GumpInfo(uint serial, uint gumpId, List<string> gumpContext)
            {
                _serial = serial;
                _gumpId = gumpId;
                _gumpContext = gumpContext;
            }

            public GumpInfo(uint serial, uint gumpId)
            {
                _serial = serial;
                _gumpId = gumpId;
            }
        }

        public GumpCollection InternalGumps { get; set; } = new GumpCollection();

        public void CancelPrompt()
        {
            Client.Instance.SendToServer(new PromptResponse(World.Player.PromptSenderSerial, World.Player.PromptID, 0,
                Language.CliLocName, string.Empty));
            World.Player.HasPrompt = false;
        }

        public void ResponsePrompt(string text)
        {
            Client.Instance.SendToServer(new PromptResponse(World.Player.PromptSenderSerial, World.Player.PromptID, 1,
                Language.CliLocName, text));

            PromptInputText = text;
            World.Player.HasPrompt = false;
        }

        private ushort _speechHue;

        public ushort SpeechHue
        {
            get { return _speechHue; }
            set { _speechHue = value; }
        }

        public sbyte LocalLightLevel
        {
            get { return _localLight; }
            set { _localLight = value; }
        }

        public byte GlobalLightLevel
        {
            get { return _globalLight; }
            set { _globalLight = value; }
        }

        public enum SeasonFlag
        {
            Spring,
            Summer,
            Fall,
            Winter,
            Desolation
        }

        public byte Season
        {
            get { return _season; }
            set { _season = value; }
        }

        public byte DefaultSeason
        {
            get { return _defaultSeason; }
            set { _defaultSeason = value; }
        }

        /// <summary>
        /// Sets the player's season, set a default to revert back if required
        /// </summary>
        /// <param name="defaultSeason"></param>
        public void SetSeason(byte defaultSeason = 0)
        {
            if (Config.GetInt("Season") < 5)
            {
                byte season = (byte) Config.GetInt("Season");

                World.Player.Season = season;
                World.Player.DefaultSeason = defaultSeason;

                if (!_seasonTimer.Running)
                    _seasonTimer.Start();
            }
            else
            {
                World.Player.Season = defaultSeason;
                World.Player.DefaultSeason = defaultSeason;
            }
        }

        public static Timer _seasonTimer = new SeasonTimer();

        private class SeasonTimer : Timer
        {
            public SeasonTimer() : base(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3))
            {
            }

            protected override void OnTick()
            {
                if (World.Player != null && Client.Instance != null)
                {
                    Client.Instance.ForceSendToClient(new SeasonChange(World.Player.Season, true));
                    _seasonTimer.Stop();
                }
            }
        }

        public uint Features
        {
            get { return _features; }
            set { _features = value; }
        }

        public int[] MapPatches
        {
            get { return _papPatches; }
            set { _papPatches = value; }
        }

        private int _lstSkill = -1;

        public int LastSkill
        {
            get { return _lstSkill; }
            set { _lstSkill = value; }
        }

        private Serial _lastObj = Serial.Zero;

        public Serial LastObject
        {
            get { return _lastObj; }
            set { _lastObj = value; }
        }

        private int _lastSpell = -1;

        public int LastSpell
        {
            get { return _lastSpell; }
            set { _lastSpell = value; }
        }

        public bool UseItem(Item cont, ushort find)
        {
            if (!Client.Instance.AllowBit(FeatureBit.PotionHotkeys))
                return false;

            for (int i = 0; i < cont.Contains.Count; i++)
            {
                Item item = (Item)cont.Contains[i];

                if (item.ItemID == find)
                {
                    DoubleClick(item);
                    return true;
                }

                if (item.Contains != null && item.Contains.Count > 0)
                {
                    if (UseItem(item, find))
                        return true;
                }
            }

            return false;
        }

        public static bool DoubleClick(object clicked)
        {
            return DoubleClick(clicked, true);
        }

        public static bool DoubleClick(object clicked, bool silent)
        {
            Serial s;
            if (clicked is Mobile)
                s = ((Mobile) clicked).Serial.Value;
            else if (clicked is Item)
                s = ((Item) clicked).Serial.Value;
            else if (clicked is Serial)
                s = ((Serial) clicked).Value;
            else
                s = Serial.Zero;

            if (s != Serial.Zero)
            {
                Item free = null, pack = World.Player.Backpack;
                if (s.IsItem && pack != null && Config.GetBool("PotionEquip") &&
                    Client.Instance.AllowBit(FeatureBit.AutoPotionEquip))
                {
                    Item i = World.FindItem(s);
                    if (i != null && i.IsPotion && i.ItemID != 3853) // dont unequip for exploison potions
                    {
                        // dont worry about uneqipping RuneBooks or SpellBooks
                        Item left = World.Player.GetItemOnLayer(Layer.LeftHand);
                        Item right = World.Player.GetItemOnLayer(Layer.RightHand);

                        if (left != null && (right != null || left.IsTwoHanded))
                            free = left;
                        else if (right != null && right.IsTwoHanded)
                            free = right;

                        if (free != null)
                        {
                            if (DragDropManager.HasDragFor(free.Serial))
                                free = null;
                            else
                                DragDropManager.DragDrop(free, pack);
                        }
                    }
                }

                ActionQueue.DoubleClick(silent, s);

                if (free != null && Config.GetBool("PotionReequip"))
                    DragDropManager.DragDrop(free, World.Player, free.Layer, true);

                if (s.IsItem)
                    World.Player._lastObj = s;
            }

            return false;
        }

        public enum InvokeVirtues
        {
            Honor = 0x01,
            Sacrifice = 0x02,
            Valor = 0x03
        }

        public static void InvokeVirtue(InvokeVirtues virtue)
        {
            Client.Instance.SendToServer(new VirtueRequest((byte) virtue));
        }

        public static void RenameMobile(Serial serial, string newName)
        {
            Client.Instance.SendToServer(new RenamePacket(serial.Value, newName));
        }
    }
}