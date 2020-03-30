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

using Assistant.Core;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Assistant
{
    public class FeatureBit
    {
        public static readonly int WeatherFilter = 0;
        public static readonly int LightFilter = 1;
        public static readonly int SmartLT = 2;
        public static readonly int RangeCheckLT = 3;
        public static readonly int AutoOpenDoors = 4;
        public static readonly int UnequipBeforeCast = 5;
        public static readonly int AutoPotionEquip = 6;
        public static readonly int BlockHealPoisoned = 7;
        public static readonly int LoopingMacros = 8; // includes fors and macros running macros
        public static readonly int UseOnceAgent = 9;
        public static readonly int RestockAgent = 10;
        public static readonly int SellAgent = 11;
        public static readonly int BuyAgent = 12;
        public static readonly int PotionHotkeys = 13;
        public static readonly int RandomTargets = 14;
        public static readonly int ClosestTargets = 15;
        public static readonly int OverheadHealth = 16;
        public static readonly int AutolootAgent = 17;
        public static readonly int BoneCutterAgent = 18;
        public static readonly int AdvancedMacros = 19;
        public static readonly int AutoRemount = 20;
        public static readonly int AutoBandage = 21;
        public static readonly int EnemyTargetShare = 22;
        public static readonly int FilterSeason = 23;
        public static readonly int SpellTargetShare = 24;
        public static readonly int HumanoidHealthChecks = 25;
        public static readonly int SpeechJournalChecks = 26;

        public static readonly int MaxBit = 26;
    }

    public abstract class Client
    {
        public static Client Instance;
        public static bool IsOSI;

        internal static void Init(bool isOSI)
        {
            IsOSI = isOSI;

            if (isOSI)
                Instance = new OSIClient();
            else
                Instance = new ClassicUOClient();
        }

        public const int WM_USER = 0x400;

        public const int WM_COPYDATA = 0x4A;
        public const int WM_UONETEVENT = WM_USER + 1;

        private ulong m_Features = 0;

        public bool AllowBit(int bit)
        {
            return (m_Features & (1U << bit)) == 0;
        }

        public void SetFeatures(ulong features)
        {
            m_Features = features;
        }

        public abstract DateTime ConnectionStart { get; }
        public abstract IPAddress LastConnection { get; }
        public abstract Process ClientProcess { get; }

        public abstract bool ClientRunning { get; }

        public abstract void SetMapWndHandle(Form mapWnd);

        public abstract void RequestStatbarPatch(bool preAOS);

        public abstract void SetCustomNotoHue(int hue);

        public abstract void SetSmartCPU(bool enabled);

        public abstract void SetGameSize(int x, int y);

        public enum Loader_Error
        {
            SUCCESS = 0,
            NO_OPEN_EXE,
            NO_MAP_EXE,
            NO_READ_EXE_DATA,

            NO_RUN_EXE,
            NO_ALLOC_MEM,

            NO_WRITE,
            NO_VPROTECT,
            NO_READ,

            UNKNOWN_ERROR = 99
        };

        public abstract Loader_Error LaunchClient(string client);

        public abstract bool ClientEncrypted { get; set; }

        public abstract bool ServerEncrypted { get; set; }

        public abstract bool InstallHooks(IntPtr mainWindow);

        public abstract void SetConnectionInfo(IPAddress addr, int port);

        public abstract void SetNegotiate(bool negotiate);

        public abstract bool Attach(int pid);

        public abstract void Close();

        public abstract void SetTitleStr(string str);

        public abstract bool OnMessage(MainForm razor, uint wParam, int lParam);

        public abstract bool OnCopyData(IntPtr wparam, IntPtr lparam);

        public abstract void SendToServer(Packet p);

        public abstract void SendToServer(PacketReader pr);

        public abstract void SendToClient(Packet p);

        public abstract void ForceSendToClient(Packet p);

        public abstract void ForceSendToServer(Packet p);

        public abstract void SetPosition(uint x, uint y, uint z, byte dir);

        public abstract string GetClientVersion();

        public abstract string GetUoFilePath();

        public abstract IntPtr GetWindowHandle();

        public abstract uint TotalDataIn();

        public abstract uint TotalDataOut();
        internal abstract void RequestMove(Direction m_Dir);


        public void RequestTitlebarUpdate()
        {
            // throttle updates, since things like counters might request 1000000 million updates/sec
            if (m_TBTimer == null)
                m_TBTimer = new TitleBarThrottle();

            if (!m_TBTimer.Running)
                m_TBTimer.Start();
        }

        private class TitleBarThrottle : Timer
        {
            public TitleBarThrottle() : base(TimeSpan.FromSeconds(0.25))
            {
            }

            protected override void OnTick()
            {
                Instance.UpdateTitleBar();
            }
        }

        private Timer m_TBTimer;
        public StringBuilder TitleBarBuilder = new StringBuilder();
        private string m_LastPlayerName = "";

        public void ResetTitleBarBuilder()
        {
            // reuse the same sb each time for less damn allocations
            TitleBarBuilder.Remove(0, TitleBarBuilder.Length);
            TitleBarBuilder.Insert(0, $"{Config.GetString("TitleBarText")}");
        }

        public virtual void UpdateTitleBar()
        {
            if (!ClientRunning || World.Player == null)
                return;

            StringBuilder sb = TitleBarBuilder;

            PlayerData p = World.Player;

            if (p.Name != m_LastPlayerName)
            {
                m_LastPlayerName = p.Name;

                Engine.MainWindow.UpdateTitle();
            }

            sb.Replace(@"{shard}", World.ShardName);

            sb.Replace(@"{str}", p.Str.ToString());
            sb.Replace(@"{hpmax}", p.HitsMax.ToString());

            sb.Replace(@"{dex}", World.Player.Dex.ToString());
            sb.Replace(@"{stammax}", World.Player.StamMax.ToString());

            sb.Replace(@"{int}", World.Player.Int.ToString());
            sb.Replace(@"{manamax}", World.Player.ManaMax.ToString());

            sb.Replace(@"{ar}", p.AR.ToString());
            sb.Replace(@"{tithe}", p.Tithe.ToString());

            sb.Replace(@"{physresist}", p.AR.ToString());
            sb.Replace(@"{fireresist}", p.FireResistance.ToString());
            sb.Replace(@"{coldresist}", p.ColdResistance.ToString());
            sb.Replace(@"{poisonresist}", p.PoisonResistance.ToString());
            sb.Replace(@"{energyresist}", p.EnergyResistance.ToString());

            sb.Replace(@"{luck}", p.Luck.ToString());

            sb.Replace(@"{damage}", String.Format("{0}-{1}", p.DamageMin, p.DamageMax));

            sb.Replace(@"{maxweight}", World.Player.MaxWeight.ToString());

            sb.Replace(@"{followers}", World.Player.Followers.ToString());
            sb.Replace(@"{followersmax}", World.Player.FollowersMax.ToString());

            sb.Replace(@"{gold}", World.Player.Gold.ToString());

            sb.Replace(@"{gps}", GoldPerHourTimer.Running ? $"{GoldPerHourTimer.GoldPerSecond:N2}" : "-");
            sb.Replace(@"{gpm}", GoldPerHourTimer.Running ? $"{GoldPerHourTimer.GoldPerMinute:N2}" : "-");
            sb.Replace(@"{gph}", GoldPerHourTimer.Running ? $"{GoldPerHourTimer.GoldPerHour:N2}" : "-");
            sb.Replace(@"{goldtotal}", GoldPerHourTimer.Running ? $"{GoldPerHourTimer.GoldSinceStart}" : "-");
            sb.Replace(@"{goldtotalmin}", GoldPerHourTimer.Running ? $"{GoldPerHourTimer.TotalMinutes:N2} min" : "-");

            sb.Replace(@"{skill}", SkillTimer.Running ? $"{SkillTimer.Count}" : "-");
            sb.Replace(@"{gate}", GateTimer.Running ? $"{GateTimer.Count}" : "-");

            sb.Replace(@"{stealthsteps}", StealthSteps.Counting ? StealthSteps.Count.ToString() : "-");
            //Client.ConnectionStart != DateTime.MinValue )
            //time = (int)((DateTime.UtcNow - Client.ConnectionStart).TotalSeconds);
            sb.Replace(@"{uptime}",
                ConnectionStart != DateTime.MinValue
                    ? Utility.FormatTime((int) ((DateTime.UtcNow - ConnectionStart).TotalSeconds))
                    : "-");

            sb.Replace(@"{dps}", DamageTracker.Running ? $"{DamageTracker.DamagePerSecond:N2}" : "-");
            sb.Replace(@"{maxdps}", DamageTracker.Running ? $"{DamageTracker.MaxDamagePerSecond:N2}" : "-");
            sb.Replace(@"{maxdamagedealt}", DamageTracker.Running ? $"{DamageTracker.MaxSingleDamageDealt}" : "-");
            sb.Replace(@"{maxdamagetaken}", DamageTracker.Running ? $"{DamageTracker.MaxSingleDamageTaken}" : "-");
            sb.Replace(@"{totaldamagedealt}", DamageTracker.Running ? $"{DamageTracker.TotalDamageDealt}" : "-");
            sb.Replace(@"{totaldamagetaken}", DamageTracker.Running ? $"{DamageTracker.TotalDamageTaken}" : "-");


            string buffList = string.Empty;

            if (BuffsTimer.Running)
            {
                StringBuilder buffs = new StringBuilder();
                foreach (BuffsDebuffs buff in World.Player.BuffsDebuffs)
                {
                    int timeLeft = 0;

                    if (buff.Duration > 0)
                    {
                        TimeSpan diff = DateTime.UtcNow - buff.Timestamp;
                        timeLeft = buff.Duration - (int) diff.TotalSeconds;
                    }

                    buffs.Append(timeLeft <= 0
                        ? $"{buff.ClilocMessage1}, "
                        : $"{buff.ClilocMessage1} ({timeLeft}), ");
                }

                buffs.Length = Math.Max(0, buffs.Length - 2);
                buffList = buffs.ToString();
                sb.Replace(@"{buffsdebuffs}", buffList);
            }
            else
            {
                sb.Replace(@"{buffsdebuffs}", "-");
            }

            SetTitleStr(sb.ToString());
        }

        public Packet MakePacketFrom(PacketReader pr)
        {
            byte[] data = pr.CopyBytes(0, pr.Length);
            return new Packet(data, pr.Length, pr.DynamicLength);
        }
    }
}