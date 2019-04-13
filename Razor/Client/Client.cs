using Assistant.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Assistant
{
    public class FeatureBit
    {
        public static readonly int WeatherFilter        =  0;
        public static readonly int LightFilter          =  1;
        public static readonly int SmartLT              =  2;
        public static readonly int RangeCheckLT         =  3;
        public static readonly int AutoOpenDoors        =  4;
        public static readonly int UnequipBeforeCast    = 5;
        public static readonly int AutoPotionEquip      =  6;
        public static readonly int BlockHealPoisoned    = 7;
        public static readonly int LoopingMacros        =  8; // includes fors and macros running macros
        public static readonly int UseOnceAgent         =  9;
        public static readonly int RestockAgent         = 10;
        public static readonly int SellAgent            = 11;
        public static readonly int BuyAgent             = 12;
        public static readonly int PotionHotkeys        = 13;
        public static readonly int RandomTargets        = 14;
        public static readonly int ClosestTargets       = 15;
        public static readonly int OverheadHealth       = 16;
        public static readonly int AutolootAgent        = 17;
        public static readonly int BoneCutterAgent      = 18;
        public static readonly int AdvancedMacros       = 19;
        public static readonly int AutoRemount          = 20;
        public static readonly int AutoBandage          = 21;
        public static readonly int EnemyTargetShare     = 22;
        public static readonly int FilterSeason         = 23;
        public static readonly int SpellTargetShare     = 24;
        public static readonly int HumanoidHealthChecks = 25;
        public static readonly int SpeechJournalChecks  = 26;

        public static readonly int MaxBit = 26;
    }

    public abstract class Client
    {
        public static Client Instance;

        internal static void Init(bool isOSI)
        {
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

        public abstract bool ClientEncrypted { get; set;  }

        public abstract bool ServerEncrypted { get; set; }

        public abstract bool InstallHooks(IntPtr mainWindow);

        public abstract void SetConnectionInfo(IPAddress addr, int port);

        public abstract void SetNegotiate(bool negotiate);

        public abstract bool Attach(int pid);

        public abstract void Close();

        public abstract void RequestTitlebarUpdate();

        public abstract bool OnMessage(MainForm razor, uint wParam, int lParam);

        public abstract bool OnCopyData(IntPtr wparam, IntPtr lparam);

        public abstract void SendToServer(Packet p);

        public abstract void SendToServer(PacketReader pr);

        public abstract void SendToClient(Packet p);

        public abstract void ForceSendToClient(Packet p);

        public abstract void SetPosition(uint x, uint y, uint z, byte dir);

        public abstract void KeyPress(int keyCode);

        public abstract string GetClientVersion();

        public abstract IntPtr GetWindowHandle();

        public abstract uint TotalDataIn();

        public abstract uint TotalDataOut();

    }

}

