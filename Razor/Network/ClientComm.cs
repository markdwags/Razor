using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Net;
using Assistant.Core;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Assistant
{
	public class FeatureBit
	{
		public static readonly uint WeatherFilter	=  0;
		public static readonly uint LightFilter		=  1;
		public static readonly uint SmartLT			=  2;	
		public static readonly uint RangeCheckLT	=  3;
		public static readonly uint AutoOpenDoors	=  4;
		public static readonly uint UnequipBeforeCast= 5;
		public static readonly uint AutoPotionEquip	=  6;
		public static readonly uint BlockHealPoisoned= 7;
		public static readonly uint LoopingMacros	=  8; // includes fors and macros running macros
		public static readonly uint UseOnceAgent	=  9;
		public static readonly uint RestockAgent	= 10;
		public static readonly uint SellAgent		= 11;
		public static readonly uint BuyAgent		= 12;
		public static readonly uint PotionHotkeys	= 13;
		public static readonly uint RandomTargets	= 14;
		public static readonly uint ClosestTargets	= 15;
		public static readonly uint OverheadHealth	= 16;

		public static readonly uint MaxBit			= 16;
	}


    public class JsonApi
    {
        public string Name { get; set; }
        public string Shard { get; set; }
        public int CrimeTime { get; set; }
        public int Str { get; set; }
        public int Dex { get; set; }
        public int Int { get; set; }
        public int HP { get; set; }
        public int Stamina { get; set; }
        public int MaxHp { get; set; }
        public int MaxStam { get; set; }
        public int MaxInt { get; set; }
        public int MaxMana { get; set; }
        public int Mana { get; set; }
        public int AR { get; set; }
        public string Tithe { get; set; }
        public int Weight { get; set; }
        public int MaxWeight { get; set; }
        public int Followers { get; set; }
        public int MaxFollowers { get; set; }
        public int Gold { get; set; }
        public double GPS { get; set; }
        public double GPM { get; set; }
        public double GPH { get; set; }
        public int GoldTotal { get; set; }
        public double GoldTotalMin { get; set; }

        public int BandageTimer { get; set; }
        public int SkillTimer { get; set; }
        public int GateTimer { get; set; }

        public int StealthSteps { get; set; }

        public string BuffsDebuffs { get; set; }
    }

    public unsafe sealed class ClientCommunication
	{
		public enum UONetMessage
		{
			Send = 1,
			Recv = 2,
			Ready = 3,
			NotReady = 4,
			Connect = 5,
			Disconnect = 6,
			KeyDown = 7,
			Mouse = 8,
			Activate = 9,
			Focus = 10,
			Close = 11,
			StatBar = 12,
			NotoHue = 13,
			DLL_Error = 14,
			DeathMsg = 15,
			OpenRPV = 18,
			SetGameSize = 19,
			FindData = 20,
			SmartCPU = 21,
			Negotiate = 22,
			SetMapHWnd = 23
		}

		public const int WM_USER = 0x400;
		public enum UOAMessage
		{
			First = REGISTER,

			//in comming:
			REGISTER = WM_USER+200,
			COUNT_RESOURCES,
			GET_COORDS,
			GET_SKILL,
			GET_STAT,
			SET_MACRO,
			PLAY_MACRO,
			DISPLAY_TEXT,
			REQUEST_MULTIS,
			ADD_CMD,
			GET_UID,
			GET_SHARDNAME,
			ADD_USER_2_PARTY,
			GET_UO_HWND,
			GET_POISON,
			SET_SKILL_LOCK,
			GET_ACCT_ID,

			//out going:
			RES_COUNT_DONE = WM_USER+301,
			CAST_SPELL,
			LOGIN,
			MAGERY_LEVEL,
			INT_STATUS,
			SKILL_LEVEL,
			MACRO_DONE,
			LOGOUT,
			STR_STATUS,
			DEX_STATUS,
			ADD_MULTI,
			REM_MULTI,
			MAP_INFO,
			POWERHOUR,

			Last = POWERHOUR
		}

		private class WndCmd
		{
			public WndCmd( uint msg, IntPtr handle, string cmd )
			{
				Msg = msg;
				hWnd = handle;
				Command.Register( cmd, new CommandCallback( MyCallback ) );
			}
			private uint Msg;
			private IntPtr hWnd;
			private void MyCallback( string[] args )
			{
				StringBuilder sb = new StringBuilder();
				for(int i=0;i<args.Length;i++)
				{
					if ( i != 0 )
						sb.Append( ' ' );
					sb.Append( args[i] );
				}
				string str = sb.ToString();
				ushort atom = 0;
				if ( str != null && str.Length > 0 )
					atom = GlobalAddAtom( str );
				PostMessage( hWnd, Msg, (IntPtr)atom, IntPtr.Zero );
			}
		}

		private class WndRegEnt
		{
			private int m_Handle;
			private int m_Type; // 1 = get multi notifcations

			public int Handle { get { return m_Handle; } }
			public int Type { get{ return m_Type; } }

			public WndRegEnt( int hWnd, int type )
			{
				m_Handle = hWnd;
				m_Type = type;
			}
		}

		private static uint m_NextCmdID = WM_USER+401;

		public static int OnUOAMessage( MainForm razor, int Msg, int wParam, int lParam )
		{
			switch ( (UOAMessage)Msg )
			{
				case UOAMessage.REGISTER:
				{
					for(int i=0;i<m_WndReg.Count;i++)
					{
						if ( ((WndRegEnt)m_WndReg[i]).Handle == wParam )
						{
							m_WndReg.RemoveAt( i );
							return 2;
						}
					}

					m_WndReg.Add( new WndRegEnt( wParam, lParam == 1 ? 1 : 0 ) );
					
					if ( lParam == 1 && World.Items != null )
					{
						foreach ( Item item in World.Items.Values )
						{
							if ( item.ItemID >= 0x4000 )
								PostMessage( (IntPtr)wParam, (uint)UOAMessage.ADD_MULTI, (IntPtr)((int)((item.Position.X&0xFFFF)|((item.Position.Y&0xFFFF)<<16))), (IntPtr)item.ItemID.Value );
						}
					}
					
					return 1;
				}
				case UOAMessage.COUNT_RESOURCES:
				{
					Counter.FullRecount();
					return 0;
				}
				case UOAMessage.GET_COORDS:
				{
					if ( World.Player == null )
						return 0;
					return (World.Player.Position.X&0xFFFF)|((World.Player.Position.Y&0xFFFF)<<16);
				}
				case UOAMessage.GET_SKILL:
				{
					if ( World.Player == null || lParam > 3 || wParam < 0 || World.Player.Skills == null || wParam > World.Player.Skills.Length || lParam < 0 )
						return 0;

					switch ( lParam )
					{
						case 3:
						{
							try
							{
								return GlobalAddAtom( ((SkillName)wParam).ToString() );
							}
							catch
							{
								return 0;
							}
						}
						case 2: return (int)(World.Player.Skills[wParam].Lock);
						case 1: return World.Player.Skills[wParam].FixedBase;
						case 0: return World.Player.Skills[wParam].FixedValue;
					}

					return 0;
				}
				case UOAMessage.GET_STAT:
				{
					if ( World.Player == null || wParam < 0 || wParam > 5 )
						return 0;
					
					switch ( wParam )
					{
						case 0: return World.Player.Str;
						case 1: return World.Player.Int;
						case 2: return World.Player.Dex;
						case 3: return World.Player.Weight;
						case 4: return World.Player.HitsMax;
						case 5: return World.Player.Tithe;
					}
					return 0;
				}
				case UOAMessage.SET_MACRO:
				{
					try
					{
						//if ( wParam >= 0 && wParam < Engine.MainWindow.macroList.Items.Count )
						//	Engine.MainWindow.macroList.SelectedIndex = wParam;
					}
					catch
					{
					}
					return 0;
				}
				case UOAMessage.PLAY_MACRO:
				{
					if ( razor != null )
						razor.playMacro_Click( razor, new EventArgs() );
					return Macros.MacroManager.Playing ? 1 : 0;
				}
				case UOAMessage.DISPLAY_TEXT:
				{
					if ( World.Player == null )
						return 0;

					int hue = wParam&0xFFFF;
					StringBuilder sb = new StringBuilder( 256 );
					if ( GlobalGetAtomName( (ushort)lParam, sb, 256 ) == 0 )
						return 0;

					if ( (wParam&0x00010000) != 0 )
						ClientCommunication.SendToClient( new UnicodeMessage( 0xFFFFFFFF, -1, MessageType.Regular, hue, 3, Language.CliLocName, "System", sb.ToString() ) );
					else
						World.Player.OverheadMessage( hue, sb.ToString() );
					GlobalDeleteAtom( (ushort)lParam );
					return 1;
				}
				case UOAMessage.REQUEST_MULTIS:
				{
					return World.Player != null ? 1 : 0;
				}
				case UOAMessage.ADD_CMD:
				{
					StringBuilder sb = new StringBuilder( 256 );
					if ( GlobalGetAtomName( (ushort)lParam, sb, 256 ) == 0 )
						return 0;

					if ( wParam == 0 )
					{
						Command.RemoveCommand( sb.ToString() );
						return 0;
					}
					else
					{
						new WndCmd( m_NextCmdID, (IntPtr)wParam, sb.ToString() );
						return (int)(m_NextCmdID++);
					}
				}
				case UOAMessage.GET_UID:
				{
					return World.Player != null ? (int)World.Player.Serial.Value : 0;
				}
				case UOAMessage.GET_SHARDNAME:
				{
					if ( World.ShardName != null && World.ShardName.Length > 0 )
						return GlobalAddAtom( World.ShardName );
					else
						return 0;
				}
				case UOAMessage.ADD_USER_2_PARTY:
				{
					return 1; // not supported, return error
				}
				case UOAMessage.GET_UO_HWND:
				{
					return FindUOWindow().ToInt32();
				}
				case UOAMessage.GET_POISON:
				{
					return World.Player != null && World.Player.Poisoned ? 1 : 0;
				}
				case UOAMessage.SET_SKILL_LOCK:
				{
					if ( World.Player == null || wParam < 0 || wParam > World.Player.Skills.Length || lParam < 0 || lParam >= 3 )
						return 0;
					SendToServer( new SetSkillLock( wParam, (LockType)lParam ) );
					return 1;
				}
				case UOAMessage.GET_ACCT_ID:
				{
					// free shards don't use account ids... so just return the player's serial number
					return World.Player == null ? 0 : (int)World.Player.Serial.Value;
				}
				default:
				{
					return 0;
				}
			}
		}

		public static void PostCounterUpdate( int counter, int count )
		{
			PostToWndReg( (uint)UOAMessage.RES_COUNT_DONE, (IntPtr)counter, (IntPtr)count );
		}

		public static void PostSpellCast( int spell )
		{
			PostToWndReg( (uint)UOAMessage.CAST_SPELL, (IntPtr)spell, IntPtr.Zero );
		}

		public static void PostLogin( int serial )
		{
			PostToWndReg( (uint)UOAMessage.LOGIN, (IntPtr)serial, IntPtr.Zero );
		}

		public static void PostMacroStop()
		{
			PostToWndReg( (uint)UOAMessage.MACRO_DONE, IntPtr.Zero, IntPtr.Zero );
		}

		public static void PostMapChange( int map )
		{
			PostToWndReg( (uint)UOAMessage.MAP_INFO, (IntPtr)map, IntPtr.Zero );
		}

		public static void PostSkillUpdate( int skill, int val )
		{
			PostToWndReg( (uint)UOAMessage.SKILL_LEVEL, (IntPtr)skill, (IntPtr)val );
			if ( skill == (int)SkillName.Magery )
				PostToWndReg( (uint)UOAMessage.MAGERY_LEVEL, (IntPtr)((int)(val/10)), (IntPtr)(val%10) );
		}

		public static void PostRemoveMulti( Item item )
		{
			if ( item == null )
				return;

			IntPtr pos = (IntPtr)((int)((item.Position.X&0xFFFF)|((item.Position.Y&0xFFFF)<<16)));

			if ( pos == IntPtr.Zero )
				return;

			for(int i=0;i<m_WndReg.Count;i++)
			{
				WndRegEnt wnd = (WndRegEnt)m_WndReg[i];
				if ( wnd.Type == 1 )
					PostMessage( (IntPtr)wnd.Handle, (uint)UOAMessage.REM_MULTI, pos, (IntPtr)item.ItemID.Value );
			}
		}

		public static void PostAddMulti( ItemID iid, Point3D Position )
		{
			IntPtr pos = (IntPtr)((int)((Position.X&0xFFFF)|((Position.Y&0xFFFF)<<16)));

			if ( pos == IntPtr.Zero )
				return;

			for(int i=0;i<m_WndReg.Count;i++)
			{
				WndRegEnt wnd = (WndRegEnt)m_WndReg[i];
				if ( wnd.Type == 1 )
					PostMessage( (IntPtr)wnd.Handle, (uint)UOAMessage.ADD_MULTI, pos, (IntPtr)iid.Value );
			}
		}

		public static void PostHitsUpdate()
		{
			if ( World.Player != null )
				PostToWndReg( (uint)UOAMessage.STR_STATUS, (IntPtr)World.Player.HitsMax, (IntPtr)World.Player.Hits );
		}

		public static void PostManaUpdate()
		{
			if ( World.Player != null )
				PostToWndReg( (uint)UOAMessage.INT_STATUS, (IntPtr)World.Player.ManaMax, (IntPtr)World.Player.Mana );
		}

		public static void PostStamUpdate()
		{
			if ( World.Player != null )
				PostToWndReg( (uint)UOAMessage.DEX_STATUS, (IntPtr)World.Player.StamMax, (IntPtr)World.Player.Stam );	
		}

		private static void PostToWndReg( uint Msg, IntPtr wParam, IntPtr lParam )
		{
			ArrayList rem = null;
			for(int i=0;i<m_WndReg.Count;i++)
			{
				if ( PostMessage( (IntPtr)((WndRegEnt)m_WndReg[i]).Handle, Msg, wParam, lParam ) == 0 )
				{
					if ( rem == null )
						rem = new ArrayList( 1 );
					rem.Add( m_WndReg[i] );
				}
			}

			if ( rem != null )
			{
				for(int i=0;i<rem.Count;i++)
					m_WndReg.Remove( rem[i] );
			}
		}

		public const int WM_UONETEVENT = WM_USER+1;
		private const int WM_CUSTOMTITLE = WM_USER+2;
		// uoa = user+3

		private enum InitError
		{
			SUCCESS,
			NO_UOWND,
			NO_TID,
			NO_HOOK,
			NO_SHAREMEM,
			LIB_DISABLED,
			NO_PATCH,
			NO_MEMCOPY,
			INVALID_PARAMS,

			UNKNOWN
		}

		private const int SHARED_BUFF_SIZE = 524288; // 262144; // 250k

		[StructLayout( LayoutKind.Explicit, Size=8+SHARED_BUFF_SIZE )]
		private struct Buffer
		{
			[FieldOffset( 0 )] public int Length;
			[FieldOffset( 4 )] public int Start;
			[FieldOffset( 8 )] public byte Buff0;
		}

		[DllImport( "Crypt.dll" )]
		private static unsafe extern int InstallLibrary( IntPtr thisWnd, int procid, int features );
		[DllImport( "Crypt.dll" )]
		private static unsafe extern void Shutdown( bool closeClient );
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern IntPtr FindUOWindow();
		[DllImport( "Crypt.dll" )]
		private static unsafe extern IntPtr GetSharedAddress(); 
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern int GetPacketLength( byte *data, int bufLen );//GetPacketLength( [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] byte[] data, int bufLen );
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern bool IsDynLength(byte packetId);
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern int GetUOProcId();
		[DllImport( "Crypt.dll" )]
		private static unsafe extern void SetCustomTitle( string title );
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern int InitializeLibrary(string version);
		[DllImport( "Crypt.dll" )]
		private static unsafe extern IntPtr GetCommMutex();
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern uint TotalIn();
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern uint TotalOut();
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern IntPtr CaptureScreen(bool isFullScreen, string msgStr);
		[DllImport( "Crypt.dll" )]
		private static unsafe extern void WaitForWindow( int pid );
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern void SetDataPath(string path);
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern void SetDeathMsg(string msg);
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern void CalibratePosition( int x, int y, int z );
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern bool IsCalibrated();
		[DllImport( "Crypt.dll" )]
		private static unsafe extern bool GetPosition( int *x, int *y, int *z );
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern void BringToFront( IntPtr hWnd );
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern void DoFeatures( int features );
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern bool AllowBit( uint bit );
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern void SetAllowDisconn( bool allowed );
        /*[DllImport( "Crypt.dll" )]
		private static unsafe extern void TranslateSetup( IntPtr setupFunc );
		[DllImport( "Crypt.dll" )]
		private static unsafe extern void TranslateLogin( IntPtr loginFunc, string name, string shard );
		[DllImport( "Crypt.dll" )]
		private static unsafe extern void TranslateDo( IntPtr translateFunc, string inText, StringBuilder outText, ref uint outLen );*/
        [DllImport( "Crypt.dll" )]
		private static unsafe extern void SetServer( uint ip, ushort port );
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern int HandleNegotiate( ulong word );
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern string GetUOVersion();

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

		[DllImport( "Loader.dll" )]
		private static unsafe extern uint Load( string exe, string dll, string func, void *dllData, int dataLen, out uint pid );

		[DllImport( "msvcrt.dll" )]
		internal static unsafe extern void memcpy(void* to, void* from, int len);

		[DllImport( "user32.dll" )]
		internal static extern uint PostMessage( IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam );
        //[DllImport("user32.dll")]
        //internal static extern uint PostMessageA(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport( "user32.dll" )]
		internal static extern bool SetForegroundWindow( IntPtr hWnd );

		[DllImport( "kernel32.dll" )]
		private static extern ushort GlobalAddAtom( string str );
		[DllImport( "kernel32.dll" )]
		private static extern ushort GlobalDeleteAtom( ushort atom );
		[DllImport( "kernel32.dll" )]
		private static extern uint GlobalGetAtomName( ushort atom, StringBuilder buff, int bufLen );
        
		[DllImport( "Advapi32.dll" )]
		private static extern int GetUserNameA( StringBuilder buff, int *len );
	
		public static string GetWindowsUserName()
		{
			int len = 1024;
			StringBuilder sb = new StringBuilder( len );
			if ( GetUserNameA( sb, &len ) != 0 )
				return sb.ToString();
			else
				return "";
		}

		private static Queue<Packet> m_SendQueue = new Queue<Packet>();
        private static Queue<Packet> m_RecvQueue = new Queue<Packet>();

        private static bool m_QueueRecv;
		private static bool m_QueueSend;

		private static Buffer *m_InRecv;
		private static Buffer *m_OutRecv;
		private static Buffer *m_InSend;
		private static Buffer *m_OutSend;
		private static byte *m_TitleStr;
		private static Mutex CommMutex;
		private static Process ClientProc;

		private static bool m_Ready = false;
		private static string m_LastStr = "";
		private static DateTime m_ConnStart;
		private static Timer m_TBTimer;
		private static IPAddress m_LastConnection;

		private static ArrayList m_WndReg;

		public static int NotificationCount { get { return m_WndReg.Count; } }

		public static DateTime ConnectionStart { get{ return m_ConnStart; } }
		public static IPAddress LastConnection{ get{ return m_LastConnection; } }
		public static Process ClientProcess{ get{ return ClientProc; } }

		public static bool ClientRunning
		{ 
			get
			{ 
				try
				{
					return ClientProc != null && !ClientProc.HasExited;
				}
				catch
				{
					return ClientProc != null && FindUOWindow() != IntPtr.Zero;
				}
			}
		}

		static ClientCommunication()
		{
			m_WndReg = new ArrayList();
		}
		
		public static void SetMapWndHandle( Form mapWnd )
		{
			PostMessage( FindUOWindow(), WM_UONETEVENT, (IntPtr)UONetMessage.SetMapHWnd, mapWnd.Handle );
		}

		public static void RequestStatbarPatch( bool preAOS )
		{
			PostMessage( FindUOWindow(), WM_UONETEVENT, (IntPtr)UONetMessage.StatBar, preAOS ? (IntPtr)1 : IntPtr.Zero );
		}

		public static void SetCustomNotoHue( int hue )
		{
			PostMessage( FindUOWindow(), WM_UONETEVENT, (IntPtr)UONetMessage.NotoHue, (IntPtr)hue );
		}

	    public static void SetSmartCPU(bool enabled)
	    {
	        if (enabled)
	            try { ClientCommunication.ClientProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.Normal; } catch { }

	        PostMessage(FindUOWindow(), WM_UONETEVENT, (IntPtr)UONetMessage.SmartCPU, (IntPtr)(enabled ? 1 : 0));
        }

		public static void SetGameSize( int x, int y )
		{
			PostMessage( FindUOWindow(), WM_UONETEVENT, (IntPtr)UONetMessage.SetGameSize, (IntPtr)((x&0xFFFF)|((y&0xFFFF)<<16)) );
            //PostMessageA(FindUOWindow(), WM_UONETEVENT, (IntPtr)UONetMessage.SetGameSize, (IntPtr)((x & 0xFFFF) | ((y & 0xFFFF) << 16)));
        }

		public static Loader_Error LaunchClient( string client )
		{
			/*string dir = Directory.GetCurrentDirectory();
			Directory.SetCurrentDirectory( Path.GetDirectoryName( client ) );
			Directory.SetCurrentDirectory( dir );

			try
			{
				ProcessStartInfo psi = new ProcessStartInfo( client );
				psi.WorkingDirectory = Path.GetDirectoryName( client );

				ClientProc = Process.Start( psi );

				if ( ClientProc != null && !Config.GetBool( "SmartCPU" ) )
					ClientProc.PriorityClass = (ProcessPriorityClass)Enum.Parse( typeof(ProcessPriorityClass), Config.GetString( "ClientPrio" ), true );
			}
			catch
			{
			}*/

			string dll = Path.Combine( Config.GetInstallDirectory(), "Crypt.dll" );
			uint pid = 0;
			Loader_Error err = (Loader_Error)Load( client, dll, "OnAttach", null, 0, out pid );

			if ( err == Loader_Error.SUCCESS )
			{
				try
				{
					ClientProc = Process.GetProcessById( (int)pid );

                    /*if ( ClientProc != null && !Config.GetBool( "SmartCPU" ) )
						ClientProc.PriorityClass = (ProcessPriorityClass)Enum.Parse( typeof(ProcessPriorityClass), Config.GetString( "ClientPrio" ), true );*/
				}
				catch
				{
				}
			}

			if ( ClientProc == null )
				return Loader_Error.UNKNOWN_ERROR;
			else
				return err;
		}

		private static bool m_ClientEnc = false;
		internal static bool ClientEncrypted { get { return m_ClientEnc; } set { m_ClientEnc = value; } }

		private static bool m_ServerEnc = false;
		internal static bool ServerEncrypted { get { return m_ServerEnc; } set { m_ServerEnc = value; } }

		internal static bool InstallHooks( IntPtr mainWindow )
		{
			InitError error;
			int flags = 0;

			if ( Config.GetBool( "Negotiate" ) )
				flags |= 0x04;

			if ( ClientEncrypted )
				flags |= 0x08;

			if ( ServerEncrypted )
				flags |= 0x10;

			//ClientProc.WaitForInputIdle();
			WaitForWindow( ClientProc.Id );

			error = (InitError)InstallLibrary( mainWindow, ClientProc.Id, flags );

			if ( error != InitError.SUCCESS )
			{
				FatalInit( error );
				return false;
			}

			byte *baseAddr = (byte*)GetSharedAddress().ToPointer();

			m_InRecv = (Buffer*)baseAddr;
			m_OutRecv = (Buffer*)(baseAddr+sizeof(Buffer));
			m_InSend = (Buffer*)(baseAddr+sizeof(Buffer)*2);
			m_OutSend = (Buffer*)(baseAddr+sizeof(Buffer)*3);
			m_TitleStr = (byte*)(baseAddr+sizeof(Buffer)*4);

			SetServer( m_ServerIP, m_ServerPort );

			CommMutex = new Mutex();
#pragma warning disable 618
			CommMutex.Handle = GetCommMutex();
#pragma warning restore 618

			try
			{
				string path = Ultima.Files.GetFilePath("art.mul");
				if ( path != null && path != string.Empty )
					SetDataPath( Path.GetDirectoryName( path ) );
				else
					SetDataPath(Path.GetDirectoryName(Ultima.Files.Directory));
			}
			catch
			{
				SetDataPath( "" );
			}

			if ( Config.GetBool( "OldStatBar" ) )
				ClientCommunication.RequestStatbarPatch( true );

			return true;
		}
		
		private static uint m_ServerIP;
		private static ushort m_ServerPort;
		
		internal static void SetConnectionInfo( IPAddress addr, int port )
		{
#pragma warning disable 618
			m_ServerIP = (uint)addr.Address;
#pragma warning restore 618
			m_ServerPort = (ushort)port;
		}
		
		public static void SetNegotiate( bool negotiate )
		{
			PostMessage( FindUOWindow(), WM_UONETEVENT, (IntPtr)UONetMessage.Negotiate, (IntPtr)(negotiate ? 1 : 0) );
		}

		public static bool Attach( int pid )
		{
			ClientProc = null;
			ClientProc = Process.GetProcessById( pid );
			return ClientProc != null;
		}

		public static void Close()
		{
			Shutdown( true );
			if ( ClientProc != null && !ClientProc.HasExited )
				ClientProc.CloseMainWindow();
			ClientProc = null;
		}

		public static string EncodeColorStat( int val, int max )
		{
			double perc = ((double)val)/((double)max);

			if ( perc <= 0.25 )
				return String.Format( "~#FF0000{0}~#~", val );
			else if ( perc <= 0.75 )
				return String.Format( "~#FFFF00{0}~#~", val );
			else
				return val.ToString();
		}

		public static void RequestTitlebarUpdate()
		{
			// throttle updates, since things like counters might request 1000000 million updates/sec
			if ( m_TBTimer == null )
				m_TBTimer = new TitleBarThrottle();

			if ( !m_TBTimer.Running )
				m_TBTimer.Start();
		}

		private class TitleBarThrottle : Timer
		{
			public TitleBarThrottle() : base( TimeSpan.FromSeconds( 0.25 )  )
			{
			}

			protected override void OnTick()
			{
				UpdateTitleBar();
			}
		}


		//private static int m_TitleCapacity = 0;
		private static StringBuilder m_TBBuilder = new StringBuilder();
		private static string m_LastPlayerName = "";

	    private static void UpdateTitleBar()
	    {
	        if (!m_Ready)
	            return;

	        if (World.Player != null && Config.GetBool("TitleBarDisplay"))
	        {
	            if (PacketPlayer.Playing)
	            {
	                SetTitleStr($"UO - Razor \"Video\" Playback in Progress... ({PacketPlayer.ElapsedString})");
	                return;
	            }

	            // reuse the same sb each time for less damn allocations
	            m_TBBuilder.Remove(0, m_TBBuilder.Length);
	            m_TBBuilder.Insert(0, Config.GetString("TitleBarText"));
	            StringBuilder sb = m_TBBuilder;
	            //StringBuilder sb = new StringBuilder( Config.GetString( "TitleBarText" ) ); // m_TitleCapacity 

	            PlayerData p = World.Player;

	            if (p.Name != m_LastPlayerName)
	            {
	                m_LastPlayerName = p.Name;

	                Engine.MainWindow.UpdateTitle();
	            }

	            sb.Replace(@"{char}",
	                Config.GetBool("ShowNotoHue") ? $"~#{p.GetNotorietyColor() & 0x00FFFFFF:X6}{p.Name}~#~" : p.Name);

	            sb.Replace(@"{shard}", World.ShardName);

	            sb.Replace(@"{crimtime}", p.CriminalTime != 0 ? $"~^C0C0C0{p.CriminalTime}~#~" : "-");

	            sb.Replace(@"{str}", p.Str.ToString());
	            sb.Replace(@"{hpmax}", p.HitsMax.ToString());

	            sb.Replace(@"{hp}", p.Poisoned ? $"~#FF8000{p.Hits}~#~" : EncodeColorStat(p.Hits, p.HitsMax));

	            sb.Replace(@"{dex}", World.Player.Dex.ToString());
	            sb.Replace(@"{stammax}", World.Player.StamMax.ToString());
	            sb.Replace(@"{stam}", EncodeColorStat(p.Stam, p.StamMax));
	            sb.Replace(@"{int}", World.Player.Int.ToString());
	            sb.Replace(@"{manamax}", World.Player.ManaMax.ToString());
	            sb.Replace(@"{mana}", EncodeColorStat(p.Mana, p.ManaMax));

	            sb.Replace(@"{ar}", p.AR.ToString());
	            sb.Replace(@"{tithe}", p.Tithe.ToString());

	            sb.Replace(@"{physresist}", p.AR.ToString());
	            sb.Replace(@"{fireresist}", p.FireResistance.ToString());
	            sb.Replace(@"{coldresist}", p.ColdResistance.ToString());
	            sb.Replace(@"{poisonresist}", p.PoisonResistance.ToString());
	            sb.Replace(@"{energyresist}", p.EnergyResistance.ToString());

	            sb.Replace(@"{luck}", p.Luck.ToString());

	            sb.Replace(@"{damage}", String.Format("{0}-{1}", p.DamageMin, p.DamageMax));

	            sb.Replace(@"{weight}",
	                World.Player.Weight >= World.Player.MaxWeight
	                    ? $"~#FF0000{World.Player.Weight}~#~"
	                    : World.Player.Weight.ToString());

	            sb.Replace(@"{maxweight}", World.Player.MaxWeight.ToString());

	            sb.Replace(@"{followers}", World.Player.Followers.ToString());
	            sb.Replace(@"{followersmax}", World.Player.FollowersMax.ToString());

	            sb.Replace(@"{gold}", World.Player.Gold.ToString());

	            sb.Replace(@"{gps}", GoldPerHourTimer.Running ? $"{GoldPerHourTimer.GoldPerSecond:N2}" : "-");
	            sb.Replace(@"{gpm}", GoldPerHourTimer.Running ? $"{GoldPerHourTimer.GoldPerMinute:N2}" : "-");
	            sb.Replace(@"{gph}", GoldPerHourTimer.Running ? $"{GoldPerHourTimer.GoldPerHour:N2}" : "-");
	            sb.Replace(@"{goldtotal}", GoldPerHourTimer.Running ? $"{GoldPerHourTimer.GoldSinceStart}" : "-");
                 sb.Replace(@"{goldtotalmin}", GoldPerHourTimer.Running ? $"{GoldPerHourTimer.TotalMinutes:N2} min" : "-");

                 sb.Replace(@"{bandage}", BandageTimer.Running ? $"~#FF8000{BandageTimer.Count}~#~" : "-");

	            sb.Replace(@"{skill}", SkillTimer.Running ? $"{SkillTimer.Count}" : "-");
	            sb.Replace(@"{gate}", GateTimer.Running ? $"{GateTimer.Count}" : "-");

	            sb.Replace(@"{stealthsteps}", StealthSteps.Counting ? StealthSteps.Count.ToString() : "-");

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
	                        timeLeft = buff.Duration - (int)diff.TotalSeconds;
                         }

	                    buffs.Append(timeLeft <= 0
	                        ? $"{buff.ClilocMessage1}, "
	                        : $"{buff.ClilocMessage1} ({timeLeft}), ");
	                }

	                buffs.Length = buffs.Length - 2;
                     buffList = buffs.ToString();
                     sb.Replace(@"{buffsdebuffs}", buffList);                     

                 }
	            else
	            {
	                sb.Replace(@"{buffsdebuffs}", "-");
                     buffList = string.Empty;
                 }

                 //if (Config.GetBool("JsonApi"))
                 //{
                 //   JsonApi json = new JsonApi();
                 //   json.Name = p.Name;
                 //   json.Shard = World.ShardName;
                 //   json.CrimeTime = p.CriminalTime != 0 ? p.CriminalTime : 0;
                 //   json.Str = p.Str;
                 //   json.Dex = p.Dex;
                 //   json.Int = p.Int;
                 //   json.HP = p.Hits;
                 //   json.Stamina = p.Stam;
                 //   json.MaxHp = p.HitsMax;
                 //   json.MaxStam = p.StamMax;
                 //   json.MaxInt = p.Int;
                 //   json.MaxMana = p.ManaMax;
                 //   json.Mana = p.Mana;
                 //   json.AR = p.AR;
                 //   json.Tithe = p.Tithe.ToString();
                 //   json.Weight = p.Weight;
                 //   json.MaxWeight = p.MaxWeight;
                 //   json.Followers = p.Followers;
                 //   json.MaxFollowers = p.FollowersMax;

                 //   json.Gold = (int) p.Gold;
                 //   json.GPS = GoldPerHourTimer.Running ? GoldPerHourTimer.GoldPerSecond : 0;
                 //   json.GPM = GoldPerHourTimer.Running ? GoldPerHourTimer.GoldPerMinute : 0;
                 //   json.GPH = GoldPerHourTimer.Running ? GoldPerHourTimer.GoldPerHour: 0;
                 //   json.GoldTotal = GoldPerHourTimer.Running ? GoldPerHourTimer.GoldSinceStart : 0;
                 //   json.GoldTotalMin = GoldPerHourTimer.Running ? GoldPerHourTimer.TotalMinutes : 0;

                 //   json.BandageTimer = BandageTimer.Running ? BandageTimer.Count : 0;
                 //   json.SkillTimer = SkillTimer.Running ? SkillTimer.Count :0;
                 //   json.GateTimer = GateTimer.Running ? GateTimer.Count : 0;


                 //   json.StealthSteps = StealthSteps.Counting ? StealthSteps.Count : 0;
                 //   json.BuffsDebuffs = !string.IsNullOrEmpty(buffList) ? buffList : "-";

                 //   // Don't stop Razor because this file isn't available
                 //   try
                 //   {
                 //       File.WriteAllText($"{Config.GetInstallDirectory()}\\{p.Name}_{p.Serial}_{World.ShardName}.json", JsonConvert.SerializeObject(json));
                 //   }
                 //   catch { }
                 //}

                 string statStr = String.Format("{0}{1:X2}{2:X2}{3:X2}",
	                (int) (p.GetStatusCode()),
	                (int) (World.Player.HitsMax == 0 ? 0 : (double) World.Player.Hits / World.Player.HitsMax * 99),
	                (int) (World.Player.ManaMax == 0 ? 0 : (double) World.Player.Mana / World.Player.ManaMax * 99),
	                (int) (World.Player.StamMax == 0 ? 0 : (double) World.Player.Stam / World.Player.StamMax * 99));

	            sb.Replace(@"{statbar}", $"~SR{statStr}");
	            sb.Replace(@"{mediumstatbar}", $"~SL{statStr}");
	            sb.Replace(@"{largestatbar}", $"~SX{statStr}");

	            bool dispImg = Config.GetBool("TitlebarImages");
	            for (int i = 0; i < Counter.List.Count; i++)
	            {
	                Counter c = Counter.List[i];
	                if (c.Enabled)
	                    sb.Replace($"{{{c.Format}}}", c.GetTitlebarString(dispImg && c.DisplayImage));
	            }

	            SetTitleStr(sb.ToString());
	        }
	        else
	        {
	            SetTitleStr("");
	        }
	    }

		private static void SetTitleStr( string str )
		{
			if ( m_LastStr == str )
				return;

			m_LastStr = str;
			byte[] copy = System.Text.Encoding.ASCII.GetBytes( str );
			int clen = copy.Length;
			if ( clen >= 512 )
				clen = 511;

			CommMutex.WaitOne();
			if ( clen > 0 )
			{
				fixed( byte *array = copy )
					memcpy( m_TitleStr, array, clen );
			}
			*(m_TitleStr+clen) = 0;
			CommMutex.ReleaseMutex();

			PostMessage( FindUOWindow(), WM_CUSTOMTITLE, IntPtr.Zero, IntPtr.Zero );
		}

		public static int GetZ( int x, int y, int z )
		{
			if ( IsCalibrated() )
			{
				if ( GetPosition( null, null, &z ) )
					return z;
			}
				
			return Map.ZTop( World.Player.Map, x, y, z );
		}

		private static void CalibrateNow()
		{
			m_CalTimer = null;

			if ( World.Player == null )
				return;

			PlayerData.ExternalZ = false;

			Point3D pos = World.Player.Position;

			if ( pos != Point3D.Zero && m_CalPos == pos )
			{
				CalibratePosition( pos.X, pos.Y, pos.Z );
				System.Threading.Thread.Sleep( TimeSpan.FromSeconds( 0.1 ) );
			}

			m_CalPos = Point2D.Zero;

			PlayerData.ExternalZ = true;
		}

		public static Timer m_CalTimer = null;
		private static TimerCallback m_CalibrateNow = new TimerCallback( CalibrateNow );
		private static Point2D m_CalPos = Point2D.Zero;

		public static void BeginCalibratePosition()
		{
			if ( World.Player == null || IsCalibrated() )
				return;

			if ( m_CalTimer != null )
				m_CalTimer.Stop();

			m_CalPos = new Point2D( World.Player.Position );
			
			m_CalTimer = Timer.DelayedCallback( TimeSpan.FromSeconds( 0.5 ), m_CalibrateNow );
			m_CalTimer.Start();
		}

		private static void FatalInit( InitError error )
		{
			StringBuilder sb = new StringBuilder( Language.GetString( LocString.InitError ) );
			sb.AppendFormat( "{0}\n", error );
			sb.Append( Language.GetString( (int)(LocString.InitError + (int)error) ) );

			MessageBox.Show( Engine.ActiveWindow, sb.ToString(), "Init Error", MessageBoxButtons.OK, MessageBoxIcon.Stop );
		}

		public static void OnLogout()
		{
			OnLogout( true );
		}

		private static void OnLogout( bool fake )
		{
			if ( !fake )
			{
				PacketHandlers.Party.Clear();

				SetTitleStr( "" );
				Engine.MainWindow.UpdateTitle();
				for(int i=0;i<m_WndReg.Count;i++)
					PostMessage( (IntPtr)((WndRegEnt)m_WndReg[i]).Handle, (uint)UOAMessage.LOGOUT, IntPtr.Zero, IntPtr.Zero );
				m_ConnStart = DateTime.MinValue;
			}

			PlayerData.ExternalZ = false;
			World.Player = null;
			PlayerData.FastWalkKey = 0;
			World.Items.Clear();
			World.Mobiles.Clear();
			Macros.MacroManager.Stop();
			ActionQueue.Stop();
			Counter.Reset();
               GoldPerHourTimer.Stop();
               BandageTimer.Stop();
               GateTimer.Stop();
               BuffsTimer.Stop();
			StealthSteps.Unhide();
			Engine.MainWindow.OnLogout();
			if( Engine.MainWindow.MapWindow != null )
				Engine.MainWindow.MapWindow.Close();
            PacketHandlers.Party.Clear();
			PacketHandlers.IgnoreGumps.Clear();
			Config.Save();

			//TranslateEnabled = false;
		}

		//private static DateTime m_LastActivate;
		internal static bool OnMessage( MainForm razor, uint wParam, int lParam )
		{
			bool retVal = true;
			
			switch ( (UONetMessage)(wParam&0xFFFF) )
			{
				case UONetMessage.OpenRPV:
				{
					if ( Engine.MainWindow != null )
					{
						StringBuilder sb = new StringBuilder( 256 );
						if ( GlobalGetAtomName( (ushort)lParam, sb, 256 ) == 0 )
							return false;
						BringToFront( FindUOWindow() );
						PacketPlayer.Open( sb.ToString() );
						Engine.MainWindow.ShowMe();
						Engine.MainWindow.SwitchToVidTab();
					}
					break;
				}
				case UONetMessage.Ready: //Patch status
					if ( lParam == (int)InitError.NO_MEMCOPY )
					{
						if ( MessageBox.Show( Engine.ActiveWindow, Language.GetString( LocString.NoMemCpy ), "No Client MemCopy", MessageBoxButtons.YesNo, MessageBoxIcon.Warning ) == DialogResult.No )
						{
							m_Ready = false;
							ClientProc = null;
							Engine.MainWindow.CanClose = true;
							Engine.MainWindow.Close();
							break;
						}
					}

					try
					{
						SetDataPath(Ultima.Files.Directory);
					}
					catch
					{
						SetDataPath( "" );
					}

					m_Ready = true;
					break;

				case UONetMessage.NotReady:
					m_Ready = false;
					FatalInit( (InitError)lParam );
					ClientProc = null;
					Engine.MainWindow.CanClose = true;
					Engine.MainWindow.Close();
					break;

					// Network events
				case UONetMessage.Recv:
					OnRecv();
					break;
				case UONetMessage.Send:
					OnSend();
					break;
				case UONetMessage.Connect:
					m_ConnStart = DateTime.UtcNow;
					try
					{
						m_LastConnection = new IPAddress( (uint)lParam );
					}
					catch
					{
					}
					break;
				case UONetMessage.Disconnect:
					OnLogout( false );
					break;
				case UONetMessage.Close:
					OnLogout();
					ClientProc = null;
					try { PacketPlayer.Stop(); } catch {}
					try { AVIRec.Stop(); } catch {}
					Engine.MainWindow.CanClose = true;
					Engine.MainWindow.Close();
					break;

					// Hot Keys
				case UONetMessage.Mouse:
					HotKey.OnMouse( (ushort)(lParam&0xFFFF), (short)(lParam>>16) );
					break;
				case UONetMessage.KeyDown:
					retVal = HotKey.OnKeyDown( lParam );
					break;
					
					// Activation Tracking
				case UONetMessage.Activate:
					/*if ( Config.GetBool( "AlwaysOnTop" ) )
					{
						if ( (lParam&0x0000FFFF) == 0 && (lParam&0xFFFF0000) != 0 && razor.WindowState != FormWindowState.Minimized && razor.Visible )
						{// if uo is deactivating and minimized and we are not minimized
							if ( !razor.ShowInTaskbar && razor.Visible )
								razor.Hide();
							razor.WindowState = FormWindowState.Minimized;
							m_LastActivate = DateTime.UtcNow;
						}
						else if ( (lParam&0x0000FFFF) != 0 && (lParam&0xFFFF0000) != 0 && razor.WindowState != FormWindowState.Normal )
						{ // is UO is activating and minimized and we are minimized
							if ( m_LastActivate+TimeSpan.FromSeconds( 0.2 ) < DateTime.UtcNow )
							{
								if ( !razor.ShowInTaskbar && !razor.Visible )
									razor.Show();
								razor.WindowState = FormWindowState.Normal;
								//SetForegroundWindow( FindUOWindow() );
							}
							m_LastActivate = DateTime.UtcNow;
						}
					}*/
					break;

				case UONetMessage.Focus:
					if ( Config.GetBool( "AlwaysOnTop" ) )
					{
						if ( lParam != 0 && !razor.TopMost )
						{
							razor.TopMost = true;
							SetForegroundWindow( FindUOWindow() );
						}
						else if ( lParam == 0 && razor.TopMost )
						{
							razor.TopMost = false;
							razor.SendToBack();
						}
					}

					// always use smartness for the map window
					if ( razor.MapWindow != null && razor.MapWindow.Visible )
					{
						if ( lParam != 0 && !razor.MapWindow.TopMost )
						{
							razor.MapWindow.TopMost = true;
							SetForegroundWindow( FindUOWindow() );
						}
						else if ( lParam == 0 && razor.MapWindow.TopMost )
						{
							razor.MapWindow.TopMost = false;
							razor.MapWindow.SendToBack();
						}
					}

                    break;

				case UONetMessage.DLL_Error:
				{
					string error = "Unknown";
					switch ( (UONetMessage)lParam )
					{
						case UONetMessage.StatBar:
							error = "Unable to patch status bar.";
							break;
					}

					MessageBox.Show( Engine.ActiveWindow, "An Error has occured : \n" + error, "Error Reported", MessageBoxButtons.OK, MessageBoxIcon.Warning );
					break;
				}

				case UONetMessage.FindData:
					FindData.Message( (wParam&0xFFFF0000)>>16, lParam );
					break;

				// Unknown
				default:
					MessageBox.Show( Engine.ActiveWindow, "Unknown message from uo client\n" + ((int)wParam).ToString(), "Error?" );
					break;
			}

			return retVal;
		}

		internal static void SendToServer( Packet p )
		{
			if ( !m_Ready || PacketPlayer.Playing )
				return;

			if ( !m_QueueSend )
			{
				if ( PacketPlayer.Recording )
					PacketPlayer.ClientPacket( p );

				ForceSendToServer( p );
			}
			else
			{
				m_SendQueue.Enqueue( p );
			}
		}

		internal static void SendToServer( PacketReader pr )
		{
			if ( !m_Ready || PacketPlayer.Playing )
				return;

			SendToServer( MakePacketFrom( pr ) );
		}

		internal static void SendToClient( Packet p )
		{
			if ( !m_Ready || PacketPlayer.Playing || p.Length <= 0 )
				return;

			if ( !m_QueueRecv )
			{
				if ( PacketPlayer.Recording )
					PacketPlayer.ServerPacket( p );

				ForceSendToClient( p );
			}
			else
			{
				m_RecvQueue.Enqueue( p );
			}
		}

		internal static void SendToClient( PacketReader pr )
		{
			if ( !m_Ready || PacketPlayer.Playing )
				return;

			SendToClient( MakePacketFrom( pr ) );
		}

		internal static void ForceSendToClient( Packet p )
		{
			byte[] data = p.Compile();

			CommMutex.WaitOne();
			fixed ( byte *ptr = data )
			{
				Packet.Log( PacketPath.RazorToClient, ptr, data.Length );
				CopyToBuffer( m_OutRecv, ptr, data.Length );
			}
			CommMutex.ReleaseMutex();
		}

		internal static void ForceSendToServer( Packet p )
		{
			if ( p == null || p.Length <= 0 )
				return;

			byte[] data = p.Compile();

			CommMutex.WaitOne();
			InitSendFlush();
			fixed ( byte *ptr = data )
			{
				Packet.Log( PacketPath.RazorToServer, ptr, data.Length );
				CopyToBuffer( m_OutSend, ptr, data.Length );
			}
			CommMutex.ReleaseMutex();
		}

		private static void InitSendFlush()
		{
			if ( m_OutSend->Length == 0 )
				PostMessage( FindUOWindow(), WM_UONETEVENT, (IntPtr)UONetMessage.Send, IntPtr.Zero );
		}

		private static void CopyToBuffer( Buffer *buffer, byte *data, int len )
		{
			//if ( buffer->Length + buffer->Start + len >= SHARED_BUFF_SIZE )
			//	throw new NullReferenceException( String.Format( "Buffer OVERFLOW in CopyToBuffer [{0} + {1}] <- {2}", buffer->Start, buffer->Length, len ) );
			
			memcpy( (&buffer->Buff0) + buffer->Start + buffer->Length, data, len );
			buffer->Length += len;
		}

		internal static Packet MakePacketFrom( PacketReader pr )
		{
			byte[] data = pr.CopyBytes( 0, pr.Length );
			return new Packet( data, pr.Length, pr.DynamicLength );	
		}

		private static void HandleComm( Buffer *inBuff, Buffer *outBuff, Queue<Packet> queue, PacketPath path )
		{
			CommMutex.WaitOne();
			while ( inBuff->Length > 0 )
			{
				byte *buff = (&inBuff->Buff0) + inBuff->Start;

				int len = GetPacketLength( buff, inBuff->Length );
				if ( len > inBuff->Length || len <= 0 )
					break;

				inBuff->Start += len;
				inBuff->Length -= len;

				bool viewer = false;
				bool filter = false;

				switch ( path )
				{
					case PacketPath.ClientToServer:
						viewer = PacketHandler.HasClientViewer( buff[0] );
						filter = PacketHandler.HasClientFilter( buff[0] );
						break;
					case PacketPath.ServerToClient:
						viewer = PacketHandler.HasServerViewer( buff[0] );
						filter = PacketHandler.HasServerFilter( buff[0] );
						break;
				}

				Packet p = null;
				PacketReader pr = null;
				if ( viewer )
				{
					pr = new PacketReader( buff, len, IsDynLength( buff[0] ) );
					if ( filter )
						p = MakePacketFrom( pr );
				}
				else if ( filter )
				{
					byte[] temp = new byte[len];
					fixed ( byte *ptr = temp )
						memcpy( ptr, buff, len );
					p = new Packet( temp, len, IsDynLength( buff[0] ) );
				}

				bool blocked = false;
				switch ( path )
				{
						// yes it should be this way
					case PacketPath.ClientToServer:
					{
						blocked = !PacketPlayer.ClientPacket( p );
						if ( !blocked )
							blocked = PacketHandler.OnClientPacket( buff[0], pr, p );
						break;
					}
					case PacketPath.ServerToClient:
					{
						if ( !PacketPlayer.Playing )
						{
							blocked = PacketHandler.OnServerPacket( buff[0], pr, p );
						}
						else
						{
							blocked = true;
							if ( p != null && p.PacketID == 0x1C )
							{
								// 0, 1, 2
								Serial serial = p.ReadUInt32(); // 3, 4, 5, 6
								ushort body = p.ReadUInt16(); // 7, 8
								MessageType type = (MessageType)p.ReadByte(); // 9
								ushort hue = p.ReadUInt16(); // 10, 11
								ushort font = p.ReadUInt16();
								string name = p.ReadStringSafe( 30 );
								string text = p.ReadStringSafe();

								if ( World.Player != null && serial == Serial.Zero && body == 0 && type == MessageType.Regular && hue == 0xFFFF && font == 0xFFFF && name == "SYSTEM" )
								{
									p.Seek( 3, SeekOrigin.Begin );
									p.WriteAsciiFixed( "", (int)p.Length-3 );

									// CHEAT UO.exe 1/2 251--
									// 1 = 2d
									// 2 = 3d!
									
									DoFeatures( World.Player.Features );
								}
							}
						}

						if ( !blocked )
							blocked = !PacketPlayer.ServerPacket( p );
						break;
					}
				}

				if ( filter )
				{
					byte[] data = p.Compile();
					fixed ( byte *ptr = data )
					{
						Packet.Log( path, ptr, data.Length, blocked );
						if ( !blocked )
							CopyToBuffer( outBuff, ptr, data.Length );
					}
				}
				else
				{
					Packet.Log( path, buff, len, blocked );
					if ( !blocked )
						CopyToBuffer( outBuff, buff, len );
				}

				if ( !PacketPlayer.Playing )
				{
					while ( queue.Count > 0 )
					{
						p = (Packet)queue.Dequeue();
						if ( PacketPlayer.Recording )
						{
							switch ( path )
							{
								case PacketPath.ClientToServer:
									PacketPlayer.ClientPacket( p );
									break;
								case PacketPath.ServerToClient:
									PacketPlayer.ServerPacket( p );
									break;
							}
						}

						byte[] data = p.Compile();
						fixed ( byte *ptr = data )
						{
							CopyToBuffer( outBuff, ptr, data.Length );
							Packet.Log( (PacketPath)(((int)path)+1), ptr, data.Length );
						}
					}
				}
				else
				{
					queue.Clear();
				}
			}
			CommMutex.ReleaseMutex();
		}

		private static void OnRecv()
		{
			m_QueueRecv = true;
			HandleComm( m_InRecv, m_OutRecv, m_RecvQueue, PacketPath.ServerToClient );
			m_QueueRecv = false;
		}

		private static void OnSend()
		{
			m_QueueSend = true;
			HandleComm( m_InSend, m_OutSend, m_SendQueue, PacketPath.ClientToServer );
			m_QueueSend = false;
		}

		public static void ProcessPlaybackData( BinaryReader reader )
		{
			byte[] buff = reader.ReadBytes( 3 );
			reader.BaseStream.Seek( -3, SeekOrigin.Current );

			int maxLen = (int)(reader.BaseStream.Length - reader.BaseStream.Position);
			int len;
			fixed ( byte *temp = buff )
				len = GetPacketLength( temp, maxLen );

			if ( len > maxLen || len <= 0 )
				return;

			buff = reader.ReadBytes( len );

			// too lazy to make proper...
			if ( buff[0] == 0x6E && buff.Length >= 14 ) // mobile anim packet
			{
				double scalar = PacketPlayer.SpeedScalar();

				if ( scalar != 1 )
				{
					if ( buff[13] == 0 )
						buff[13] = 1;
					buff[13] = (byte)(buff[13] * scalar);
				}
			}
			else if ( buff[0] == 0xBF && buff.Length >= 5 )
			{
				if ( buff[4] == 0x10 && World.Player != null && World.Player.Features <= 3 ) 
					return;// Object Property List
				else if ( buff[4] == 0x06 ) 
					return;// Party Packets
			}
			else if ( buff[0] == 0x6C || buff[0] == 0xBA || buff[0] == 0xB2 || buff[0] == 0xFF )
			{
				return;
			}

			bool viewer = PacketHandler.HasServerViewer( buff[0] );
			bool filter = PacketHandler.HasServerFilter( buff[0] );
	
			if ( buff[0] == 0x25 )
				buff = PacketHandlers.HandleRPVContainerContentUpdate( new Packet( buff, buff.Length, IsDynLength( buff[0] ) ) );
			else if ( buff[1] == 0x3C )
				buff = PacketHandlers.HandleRPVContainerContent( new Packet( buff, buff.Length, IsDynLength( buff[0] ) ) );

			Packet p = null;
			PacketReader pr = null;
			if ( viewer )
			{
				pr = new PacketReader( buff, IsDynLength( buff[0] ) );
				if ( filter )
					p = MakePacketFrom( pr );
			}
			else if ( filter )
			{
				p = new Packet( buff, buff.Length, IsDynLength( buff[0] ) );
			}

			// prevent razor's default handlers from sending any data,
			// we just want the handlers to have these packets so we can maintain internal info
			// about mobs & items (we dont really want razor do do anything, just to know whats going on)
			m_QueueRecv = true;
			m_QueueSend = true;
			PacketHandler.OnServerPacket( buff[0], pr, p );
			m_QueueRecv = false;
			m_QueueSend = false;

			m_RecvQueue.Clear();
			m_SendQueue.Clear();

			CommMutex.WaitOne();
			fixed ( byte *ptr = buff )
			{
				while ( m_OutRecv->Start + m_OutRecv->Length + buff.Length >= SHARED_BUFF_SIZE )
				{
					CommMutex.ReleaseMutex();
					System.Threading.Thread.Sleep( 1 );
					CommMutex.WaitOne();
				}

				Packet.Log( PacketPath.PacketVideo, ptr, buff.Length );
				CopyToBuffer( m_OutRecv, ptr, buff.Length );
			}
			CommMutex.ReleaseMutex();
		}
	}

}

