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

	public unsafe sealed class Client
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
			SetGameSize = 19,
			FindData = 20,
			SmartCPU = 21,
			Negotiate = 22,
			SetMapHWnd = 23
		}

		public enum UONetMessageCopyData
		{
			Position = 1,
		}

		public const int WM_USER = 0x400;

		public const int WM_COPYDATA = 0x4A;
		public const int WM_UONETEVENT = WM_USER+1;
		private const int WM_CUSTOMTITLE = WM_USER+2;

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
		private static unsafe extern IntPtr GetCommMutex();
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern uint TotalIn();
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern uint TotalOut();
		[DllImport( "Crypt.dll" )]
		private static unsafe extern void WaitForWindow( int pid );
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern void SetDataPath(string path);
		[DllImport( "Crypt.dll" )]
		internal static unsafe extern void CalibratePosition( uint x, uint y, uint z, byte dir );
		[DllImport( "Crypt.dll" )]
		private static unsafe extern void SetServer( uint ip, ushort port );
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

		public static void SetMapWndHandle( Form mapWnd )
		{
			Platform.PostMessage( FindUOWindow(), WM_UONETEVENT, (IntPtr)UONetMessage.SetMapHWnd, mapWnd.Handle );
		}

		public static void RequestStatbarPatch( bool preAOS )
		{
            Platform.PostMessage( FindUOWindow(), WM_UONETEVENT, (IntPtr)UONetMessage.StatBar, preAOS ? (IntPtr)1 : IntPtr.Zero );
		}

		public static void SetCustomNotoHue( int hue )
		{
            Platform.PostMessage( FindUOWindow(), WM_UONETEVENT, (IntPtr)UONetMessage.NotoHue, (IntPtr)hue );
		}

		public static void SetSmartCPU(bool enabled)
		{
			if (enabled)
				try { Client.ClientProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.Normal; } catch { }

            Platform.PostMessage(FindUOWindow(), WM_UONETEVENT, (IntPtr)UONetMessage.SmartCPU, (IntPtr)(enabled ? 1 : 0));
		}

		public static void SetGameSize( int x, int y )
		{
            Platform.PostMessage( FindUOWindow(), WM_UONETEVENT, (IntPtr)UONetMessage.SetGameSize, (IntPtr)((x&0xFFFF)|((y&0xFFFF)<<16)) );
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

			if (m_Ready)
				return false; // double init

			if ( Config.GetBool( "Negotiate" ) )
				flags |= 0x04;

			if ( ClientEncrypted )
				flags |= 0x08;

			if ( ServerEncrypted )
				flags |= 0x10;

            WaitForWindow( ClientProc.Id );

			error = (InitError)InstallLibrary( mainWindow, ClientProc.Id, flags );
			if ( error != InitError.SUCCESS )
			{
				FatalInit( error );
				return false;
			}

			// When InstallLibrary finishes, we get a UONETEVENT saying it's ready.
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
            Platform.PostMessage( FindUOWindow(), WM_UONETEVENT, (IntPtr)UONetMessage.Negotiate, (IntPtr)(negotiate ? 1 : 0) );
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

		private static string EncodeColorStat( int val, int max )
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
				//Client.ConnectionStart != DateTime.MinValue )
				//time = (int)((DateTime.UtcNow - Client.ConnectionStart).TotalSeconds);
				 sb.Replace(@"{uptime}", ConnectionStart != DateTime.MinValue ? Utility.FormatTime((int)((DateTime.UtcNow - ConnectionStart).TotalSeconds)) : "-");

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
                    Platform.memcpy( m_TitleStr, array, clen );
			}
			*(m_TitleStr+clen) = 0;
			CommMutex.ReleaseMutex();

            Platform.PostMessage( FindUOWindow(), WM_CUSTOMTITLE, IntPtr.Zero, IntPtr.Zero );
		}

		private static void FatalInit( InitError error )
		{
			StringBuilder sb = new StringBuilder( Language.GetString( LocString.InitError ) );
			sb.AppendFormat( "{0}\n", error );
			sb.Append( Language.GetString( (int)(LocString.InitError + (int)error) ) );

			MessageBox.Show( Engine.ActiveWindow, sb.ToString(), "Init Error", MessageBoxButtons.OK, MessageBoxIcon.Stop );
		}

		private static void OnLogout( bool fake )
		{
			if ( !fake )
			{
				PacketHandlers.Party.Clear();

				SetTitleStr( "" );
				Engine.MainWindow.UpdateTitle();
				UOAssist.PostLogout();
				m_ConnStart = DateTime.MinValue;
			}

			World.Player = null;
			World.Items.Clear();
			World.Mobiles.Clear();
			Macros.MacroManager.Stop();
			ActionQueue.Stop();
			Counter.Reset();
			   GoldPerHourTimer.Stop();
			DamageTracker.Stop();
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

					byte* baseAddr = (byte*)GetSharedAddress().ToPointer();

					m_InRecv = (Buffer*)baseAddr;
					m_OutRecv = (Buffer*)(baseAddr + sizeof(Buffer));
					m_InSend = (Buffer*)(baseAddr + sizeof(Buffer) * 2);
					m_OutSend = (Buffer*)(baseAddr + sizeof(Buffer) * 3);
					m_TitleStr = (byte*)(baseAddr + sizeof(Buffer) * 4);

                    SetServer(m_ServerIP, m_ServerPort);

					CommMutex = new Mutex();
#pragma warning disable 618
					CommMutex.Handle = GetCommMutex();
#pragma warning restore 618

					try
					{
						string path = Ultima.Files.GetFilePath("art.mul");
						if (path != null && path != string.Empty)
                            SetDataPath(Path.GetDirectoryName(path));
						else
                            SetDataPath(Path.GetDirectoryName(Ultima.Files.Directory));
					}
					catch
					{
                        SetDataPath("");
					}

					if (Config.GetBool("OldStatBar"))
						Client.RequestStatbarPatch(true);

					m_Ready = true;
					Engine.MainWindow.MainForm_EndLoad();
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
					OnLogout(true);
					ClientProc = null;
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
							Platform.SetForegroundWindow( FindUOWindow() );
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
                            Platform.SetForegroundWindow( FindUOWindow() );
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

		[StructLayout(LayoutKind.Sequential, Pack=1)]
		private struct CopyData
		{
			public int dwData;
			public int cbDAta;
			public IntPtr lpData;
		};

		[StructLayout(LayoutKind.Sequential, Pack=1)]
		private struct Position
		{
			public ushort x;
			public ushort y;
			public ushort z;
		};

		internal static unsafe bool OnCopyData(IntPtr wparam, IntPtr lparam)
		{
			CopyData copydata = (CopyData)Marshal.PtrToStructure(lparam, typeof(CopyData));

			switch ((UONetMessageCopyData)copydata.dwData)
			{
				case UONetMessageCopyData.Position:
					if (World.Player != null)
					{
						Position pos = (Position)Marshal.PtrToStructure(copydata.lpData, typeof(Position));
						Point3D pt = new Point3D();

						pt.X = pos.x;
						pt.Y = pos.y;
						pt.Z = pos.z;

						World.Player.Position = pt;
					}
					return true;
			}

			return false;
		}

		internal static void SendToServer( Packet p )
		{
			if ( !m_Ready )
				return;

			if ( !m_QueueSend )
			{
				ForceSendToServer( p );
			}
			else
			{
				m_SendQueue.Enqueue( p );
			}
		}

		internal static void SendToServer( PacketReader pr )
		{
			if ( !m_Ready )
				return;

			SendToServer( MakePacketFrom( pr ) );
		}

		internal static void SendToClient( Packet p )
		{
			if ( !m_Ready || p.Length <= 0 )
				return;

			if ( !m_QueueRecv )
			{
				ForceSendToClient( p );
			}
			else
			{
				m_RecvQueue.Enqueue( p );
			}
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

		private static void ForceSendToServer( Packet p )
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
                Platform.PostMessage( FindUOWindow(), WM_UONETEVENT, (IntPtr)UONetMessage.Send, IntPtr.Zero );
		}

		private static void CopyToBuffer( Buffer *buffer, byte *data, int len )
		{
            //if ( buffer->Length + buffer->Start + len >= SHARED_BUFF_SIZE )
            //	throw new NullReferenceException( String.Format( "Buffer OVERFLOW in CopyToBuffer [{0} + {1}] <- {2}", buffer->Start, buffer->Length, len ) );

            Platform.memcpy( (&buffer->Buff0) + buffer->Start + buffer->Length, data, len );
			buffer->Length += len;
		}

		private static Packet MakePacketFrom( PacketReader pr )
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

				short len = PacketsTable.GetPacketLength( buff, inBuff->Length );
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
					pr = new PacketReader( buff, len, PacketsTable.IsDynLength(buff[0]) );
					if ( filter )
						p = MakePacketFrom( pr );
				}
				else if ( filter )
				{
					byte[] temp = new byte[len];
					fixed ( byte *ptr = temp )
                        Platform.memcpy( ptr, buff, len );
					p = new Packet( temp, len, PacketsTable.IsDynLength(buff[0]) );
				}

				bool blocked = false;
				switch ( path )
				{
						// yes it should be this way
					case PacketPath.ClientToServer:
					{
						blocked = PacketHandler.OnClientPacket( buff[0], pr, p );
						break;
					}
					case PacketPath.ServerToClient:
					{
						blocked = PacketHandler.OnServerPacket(buff[0], pr, p);
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

				while (queue.Count > 0)
				{
					p = (Packet)queue.Dequeue();
					byte[] data = p.Compile();
					fixed (byte* ptr = data)
					{
						CopyToBuffer(outBuff, ptr, data.Length);
						Packet.Log((PacketPath)(((int)path) + 1), ptr, data.Length);
					}
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
	}

}

