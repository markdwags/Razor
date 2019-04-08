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
	public unsafe sealed class UOAssist
	{
		public const int WM_USER = 0x400;
		public enum UOAMessage
		{
			First = REGISTER,

			//incoming:
			REGISTER = WM_USER + 200,
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

			//outgoing:
			RES_COUNT_DONE = WM_USER + 301,
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
			public WndCmd(uint msg, IntPtr handle, string cmd)
			{
				Msg = msg;
				hWnd = handle;
				Command.Register(cmd, new CommandCallback(MyCallback));
			}
			private uint Msg;
			private IntPtr hWnd;
			private void MyCallback(string[] args)
			{
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < args.Length; i++)
				{
					if (i != 0)
						sb.Append(' ');
					sb.Append(args[i]);
				}
				string str = sb.ToString();
				ushort atom = 0;
				if (str != null && str.Length > 0)
					atom = GlobalAddAtom(str);
				PostMessage(hWnd, Msg, (IntPtr)atom, IntPtr.Zero);
			}
		}

		private class WndRegEnt
		{
			private int m_Handle;
			private int m_Type; // 1 = get multi notifcations

			public int Handle { get { return m_Handle; } }
			public int Type { get { return m_Type; } }

			public WndRegEnt(int hWnd, int type)
			{
				m_Handle = hWnd;
				m_Type = type;
			}
		}

		private static uint m_NextCmdID = WM_USER + 401;
		private static ArrayList m_WndReg;

		public static int NotificationCount { get { return m_WndReg.Count; } }

		static UOAssist()
		{
			m_WndReg = new ArrayList();
		}

		public static int OnUOAMessage(MainForm razor, int Msg, int wParam, int lParam)
		{
			switch ((UOAMessage)Msg)
			{
				case UOAMessage.REGISTER:
					{
						for (int i = 0; i < m_WndReg.Count; i++)
						{
							if (((WndRegEnt)m_WndReg[i]).Handle == wParam)
							{
								m_WndReg.RemoveAt(i);
								return 2;
							}
						}

						m_WndReg.Add(new WndRegEnt(wParam, lParam == 1 ? 1 : 0));

						if (lParam == 1 && World.Items != null)
						{
							foreach (Item item in World.Items.Values)
							{
								if (item.ItemID >= 0x4000)
									PostMessage((IntPtr)wParam, (uint)UOAMessage.ADD_MULTI, (IntPtr)((int)((item.Position.X & 0xFFFF) | ((item.Position.Y & 0xFFFF) << 16))), (IntPtr)item.ItemID.Value);
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
						if (World.Player == null)
							return 0;
						return (World.Player.Position.X & 0xFFFF) | ((World.Player.Position.Y & 0xFFFF) << 16);
					}
				case UOAMessage.GET_SKILL:
					{
						if (World.Player == null || lParam > 3 || wParam < 0 || World.Player.Skills == null || wParam > World.Player.Skills.Length || lParam < 0)
							return 0;

						switch (lParam)
						{
							case 3:
								{
									try
									{
										return GlobalAddAtom(((SkillName)wParam).ToString());
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
						if (World.Player == null || wParam < 0 || wParam > 5)
							return 0;

						switch (wParam)
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
						if (razor != null)
							razor.playMacro_Click(razor, new EventArgs());
						return Macros.MacroManager.Playing ? 1 : 0;
					}
				case UOAMessage.DISPLAY_TEXT:
					{
						if (World.Player == null)
							return 0;

						int hue = wParam & 0xFFFF;
						StringBuilder sb = new StringBuilder(256);
						if (GlobalGetAtomName((ushort)lParam, sb, 256) == 0)
							return 0;

						if ((wParam & 0x00010000) != 0)
							Client.Instance.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, hue, 3, Language.CliLocName, "System", sb.ToString()));
						else
							World.Player.OverheadMessage(hue, sb.ToString());
						GlobalDeleteAtom((ushort)lParam);
						return 1;
					}
				case UOAMessage.REQUEST_MULTIS:
					{
						return World.Player != null ? 1 : 0;
					}
				case UOAMessage.ADD_CMD:
					{
						StringBuilder sb = new StringBuilder(256);
						if (GlobalGetAtomName((ushort)lParam, sb, 256) == 0)
							return 0;

						if (wParam == 0)
						{
							Command.RemoveCommand(sb.ToString());
							return 0;
						}
						else
						{
							new WndCmd(m_NextCmdID, (IntPtr)wParam, sb.ToString());
							return (int)(m_NextCmdID++);
						}
					}
				case UOAMessage.GET_UID:
					{
						return World.Player != null ? (int)World.Player.Serial.Value : 0;
					}
				case UOAMessage.GET_SHARDNAME:
					{
						if (World.ShardName != null && World.ShardName.Length > 0)
							return GlobalAddAtom(World.ShardName);
						else
							return 0;
					}
				case UOAMessage.ADD_USER_2_PARTY:
					{
						return 1; // not supported, return error
					}
				case UOAMessage.GET_UO_HWND:
					{
						return Client.Instance.GetWindowHandle().ToInt32();
					}
				case UOAMessage.GET_POISON:
					{
						return World.Player != null && World.Player.Poisoned ? 1 : 0;
					}
				case UOAMessage.SET_SKILL_LOCK:
					{
						if (World.Player == null || wParam < 0 || wParam > World.Player.Skills.Length || lParam < 0 || lParam >= 3)
							return 0;
						Client.Instance.SendToServer(new SetSkillLock(wParam, (LockType)lParam));
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

		public static void PostCounterUpdate(int counter, int count)
		{
			PostToWndReg((uint)UOAMessage.RES_COUNT_DONE, (IntPtr)counter, (IntPtr)count);
		}

		public static void PostSpellCast(int spell)
		{
			PostToWndReg((uint)UOAMessage.CAST_SPELL, (IntPtr)spell, IntPtr.Zero);
		}

		public static void PostLogin(int serial)
		{
			PostToWndReg((uint)UOAMessage.LOGIN, (IntPtr)serial, IntPtr.Zero);
		}

		public static void PostLogout()
		{
			for (int i = 0; i < m_WndReg.Count; i++)
				PostMessage((IntPtr)((WndRegEnt)m_WndReg[i]).Handle, (uint)UOAMessage.LOGOUT, IntPtr.Zero, IntPtr.Zero);
		}

		public static void PostMacroStop()
		{
			PostToWndReg((uint)UOAMessage.MACRO_DONE, IntPtr.Zero, IntPtr.Zero);
		}

		public static void PostMapChange(int map)
		{
			PostToWndReg((uint)UOAMessage.MAP_INFO, (IntPtr)map, IntPtr.Zero);
		}

		public static void PostSkillUpdate(int skill, int val)
		{
			PostToWndReg((uint)UOAMessage.SKILL_LEVEL, (IntPtr)skill, (IntPtr)val);
			if (skill == (int)SkillName.Magery)
				PostToWndReg((uint)UOAMessage.MAGERY_LEVEL, (IntPtr)((int)(val / 10)), (IntPtr)(val % 10));
		}

		public static void PostRemoveMulti(Item item)
		{
			if (item == null)
				return;

			IntPtr pos = (IntPtr)((int)((item.Position.X & 0xFFFF) | ((item.Position.Y & 0xFFFF) << 16)));

			if (pos == IntPtr.Zero)
				return;

			for (int i = 0; i < m_WndReg.Count; i++)
			{
				WndRegEnt wnd = (WndRegEnt)m_WndReg[i];
				if (wnd.Type == 1)
					PostMessage((IntPtr)wnd.Handle, (uint)UOAMessage.REM_MULTI, pos, (IntPtr)item.ItemID.Value);
			}
		}

		public static void PostAddMulti(ItemID iid, Point3D Position)
		{
			IntPtr pos = (IntPtr)((int)((Position.X & 0xFFFF) | ((Position.Y & 0xFFFF) << 16)));

			if (pos == IntPtr.Zero)
				return;

			for (int i = 0; i < m_WndReg.Count; i++)
			{
				WndRegEnt wnd = (WndRegEnt)m_WndReg[i];
				if (wnd.Type == 1)
					PostMessage((IntPtr)wnd.Handle, (uint)UOAMessage.ADD_MULTI, pos, (IntPtr)iid.Value);
			}
		}

		public static void PostHitsUpdate()
		{
			if (World.Player != null)
				PostToWndReg((uint)UOAMessage.STR_STATUS, (IntPtr)World.Player.HitsMax, (IntPtr)World.Player.Hits);
		}

		public static void PostManaUpdate()
		{
			if (World.Player != null)
				PostToWndReg((uint)UOAMessage.INT_STATUS, (IntPtr)World.Player.ManaMax, (IntPtr)World.Player.Mana);
		}

		public static void PostStamUpdate()
		{
			if (World.Player != null)
				PostToWndReg((uint)UOAMessage.DEX_STATUS, (IntPtr)World.Player.StamMax, (IntPtr)World.Player.Stam);
		}

		private static void PostToWndReg(uint Msg, IntPtr wParam, IntPtr lParam)
		{
			ArrayList rem = null;
			for (int i = 0; i < m_WndReg.Count; i++)
			{
				if (PostMessage((IntPtr)((WndRegEnt)m_WndReg[i]).Handle, Msg, wParam, lParam) == 0)
				{
					if (rem == null)
						rem = new ArrayList(1);
					rem.Add(m_WndReg[i]);
				}
			}

			if (rem != null)
			{
				for (int i = 0; i < rem.Count; i++)
					m_WndReg.Remove(rem[i]);
			}
		}

		[DllImport("user32.dll")]
		internal static extern uint PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
		[DllImport("kernel32.dll")]
		private static extern ushort GlobalAddAtom(string str);
		[DllImport("kernel32.dll")]
		private static extern ushort GlobalDeleteAtom(ushort atom);
		[DllImport("kernel32.dll")]
		private static extern uint GlobalGetAtomName(ushort atom, StringBuilder buff, int bufLen);
	}
}

