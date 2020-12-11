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

using Assistant.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Assistant.Core;
using Assistant.Scripts;
using CUO_API;

namespace Assistant
{
    public partial class Engine
    {
        public static unsafe void Install(void* ptr)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
            {
                string[] fields = e.Name.Split(',');
                string name = fields[0];
                string culture = fields[2];

                if (name.EndsWith(".resources") && !culture.EndsWith("neutral"))
                {
                    return null;
                }

                AssemblyName askedassembly = new AssemblyName(e.Name);

                bool isdll = File.Exists(Path.Combine(RootPath, askedassembly.Name + ".dll"));

                return Assembly.LoadFile(Path.Combine(RootPath, askedassembly.Name + (isdll ? ".dll" : ".exe")));
            };

            SplashScreen.Start();
            m_ActiveWnd = SplashScreen.Instance;

            Client.Init(false);

            if (!(Client.Instance as ClassicUOClient).Install(ptr))
            {
                Process.GetCurrentProcess().Kill();
                return;
            }

            // load ultimasdk before or the Language.Load will throw the cliloc not found warning every time you run cuo
            string clientPath =
                ((CUO_API.OnGetUOFilePath) Marshal.GetDelegateForFunctionPointer(((PluginHeader*) ptr)->GetUOFilePath, typeof(OnGetUOFilePath))
                )();

            // just replicating the static .ctor
            Ultima.Files.ReLoadDirectory();
            Ultima.Files.LoadMulPath();

            Ultima.Files.SetMulPath(clientPath);
            Ultima.Multis.PostHSFormat = UsePostHSChanges;
            Client.Instance.ClientEncrypted = false;
            Client.Instance.ServerEncrypted = false;


            /* Load localization files */
            if (!Language.Load("ENU"))
            {
                SplashScreen.End();
                MessageBox.Show(
                    "WARNING: Razor was unable to load the file Language/Razor_lang.ENU\n.",
                    "Language Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            m_Running = true;

            Language.LoadCliLoc();

            /* Initialize engine */
            SplashScreen.Message = LocString.Initializing;
            Initialize(typeof(Engine).Assembly);

            /* Load Profile */
            SplashScreen.Message = LocString.LoadingLastProfile;
            Config.LoadCharList();
            Overrides.Load();
            if (!Config.LoadLastProfile())
                MessageBox.Show(
                    "The selected profile could not be loaded, using default instead.", "Profile Load Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

            SplashScreen.Message = LocString.WaitingForClient;

            SplashScreen.End();

            Thread t = new Thread(() => { RunUI(); });
            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
        }
    }

    public class ClassicUOClient : Client
    {
        public override DateTime ConnectionStart => m_ConnectionStart;
        public override IPAddress LastConnection { get; }
        public override Process ClientProcess => m_ClientProcess;
        public override bool ClientRunning => m_ClientRunning;
        private uint m_In, m_Out;

        private DateTime m_ConnectionStart;
        private Process m_ClientProcess = null;
        private bool m_ClientRunning = false;
        private string m_ClientVersion;

        private static CUO_API.OnPacketSendRecv _sendToClient, _sendToServer;
        private static CUO_API.OnPacketSendRecv _recv, _send;
        private static CUO_API.OnGetPacketLength _getPacketLength;
        private static CUO_API.OnGetPlayerPosition _getPlayerPosition;
        private static CUO_API.OnCastSpell _castSpell;
        private static CUO_API.OnGetStaticImage _getStaticImage;
        private static CUO_API.OnTick _tick;
        private static CUO_API.RequestMove _requestMove;
        private static CUO_API.OnSetTitle _setTitle;
        private static CUO_API.OnGetUOFilePath _uoFilePath;


        private static CUO_API.OnHotkey _onHotkeyPressed;
        private static CUO_API.OnMouse _onMouse;
        private static CUO_API.OnUpdatePlayerPosition _onUpdatePlayerPosition;
        private static CUO_API.OnClientClose _onClientClose;
        private static CUO_API.OnInitialize _onInitialize;
        private static CUO_API.OnConnected _onConnected;
        private static CUO_API.OnDisconnected _onDisconnected;
        private static CUO_API.OnFocusGained _onFocusGained;
        private static CUO_API.OnFocusLost _onFocusLost;
        private IntPtr m_ClientWindow;

        public override void SetMapWndHandle(Form mapWnd)
        {
        }

        public override void RequestStatbarPatch(bool preAOS)
        {
        }

        public override void SetCustomNotoHue(int hue)
        {
        }

        public override void SetSmartCPU(bool enabled)
        {
        }

        public override void SetGameSize(int x, int y)
        {
        }

        public override Loader_Error LaunchClient(string client)
        {
            return Loader_Error.SUCCESS;
        }

        public override bool ClientEncrypted { get; set; }

        public override bool ServerEncrypted { get; set; }

        public unsafe bool Install(void* ptr)
        {
            CUO_API.PluginHeader* header = (CUO_API.PluginHeader*) ptr;

            _sendToClient =
                (CUO_API.OnPacketSendRecv) Marshal.GetDelegateForFunctionPointer(header->Recv, typeof(CUO_API.OnPacketSendRecv));
            _sendToServer =
                (CUO_API.OnPacketSendRecv) Marshal.GetDelegateForFunctionPointer(header->Send, typeof(CUO_API.OnPacketSendRecv));
            _getPacketLength =
                (CUO_API.OnGetPacketLength) Marshal.GetDelegateForFunctionPointer(header->GetPacketLength,
                                                                                  typeof(CUO_API.OnGetPacketLength));
            _getPlayerPosition =
                (CUO_API.OnGetPlayerPosition) Marshal.GetDelegateForFunctionPointer(header->GetPlayerPosition,
                                                                                    typeof(CUO_API.OnGetPlayerPosition));
            _castSpell = (CUO_API.OnCastSpell) Marshal.GetDelegateForFunctionPointer(header->CastSpell, typeof(CUO_API.OnCastSpell));
            _getStaticImage =
                (CUO_API.OnGetStaticImage) Marshal.GetDelegateForFunctionPointer(header->GetStaticImage,
                                                                                 typeof(CUO_API.OnGetStaticImage));
            _requestMove =
                (CUO_API.RequestMove) Marshal.GetDelegateForFunctionPointer(header->RequestMove, typeof(CUO_API.RequestMove));
            _setTitle = (CUO_API.OnSetTitle) Marshal.GetDelegateForFunctionPointer(header->SetTitle, typeof(CUO_API.OnSetTitle));
            _uoFilePath =
                (CUO_API.OnGetUOFilePath) Marshal.GetDelegateForFunctionPointer(header->GetUOFilePath, typeof(CUO_API.OnGetUOFilePath));
            m_ClientVersion = new Version((byte) (header->ClientVersion >> 24), (byte) (header->ClientVersion >> 16),
                (byte) (header->ClientVersion >> 8), (byte) header->ClientVersion).ToString();
            m_ClientRunning = true;
            m_ClientWindow = header->HWND;
            _tick = Tick;
            _recv = OnRecv;
            _send = OnSend;
            _onHotkeyPressed = OnHotKeyHandler;
            _onMouse = OnMouseHandler;
            _onUpdatePlayerPosition = OnPlayerPositionChanged;
            _onClientClose = OnClientClosing;
            _onInitialize = OnInitialize;
            _onConnected = OnConnected;
            _onDisconnected = OnDisconnected;
            _onFocusGained = OnFocusGained;
            _onFocusLost = OnFocusLost;
            header->Tick = Marshal.GetFunctionPointerForDelegate(_tick);
            header->OnRecv = Marshal.GetFunctionPointerForDelegate(_recv);
            header->OnSend = Marshal.GetFunctionPointerForDelegate(_send);
            header->OnHotkeyPressed = Marshal.GetFunctionPointerForDelegate(_onHotkeyPressed);
            header->OnMouse = Marshal.GetFunctionPointerForDelegate(_onMouse);
            header->OnPlayerPositionChanged = Marshal.GetFunctionPointerForDelegate(_onUpdatePlayerPosition);
            header->OnClientClosing = Marshal.GetFunctionPointerForDelegate(_onClientClose);
            header->OnInitialize = Marshal.GetFunctionPointerForDelegate(_onInitialize);
            header->OnConnected = Marshal.GetFunctionPointerForDelegate(_onConnected);
            header->OnDisconnected = Marshal.GetFunctionPointerForDelegate(_onDisconnected);
            header->OnFocusGained = Marshal.GetFunctionPointerForDelegate(_onFocusGained);
            header->OnFocusLost = Marshal.GetFunctionPointerForDelegate(_onFocusLost);

            return true;
        }

        public unsafe override bool InstallHooks(IntPtr pluginPtr)
        {
            Engine.MainWindow.SafeAction((s) => { Engine.MainWindow.MainForm_EndLoad(); });
            return true;
        }

        private void Tick()
        {
            Timer.Slice();
        }

        private void OnPlayerPositionChanged(int x, int y, int z)
        {
            World.Player.Position = new Point3D(x, y, z);
        }

        private unsafe bool OnRecv(ref byte[] buffer, ref int length)
        {
            m_In += (uint) length;
            fixed (byte* ptr = buffer)
            {
                bool result = true;
                byte id = ptr[0];

                PacketReader reader = null;
                Packet packet = null;
                bool isView = PacketHandler.HasServerViewer(id);
                bool isFilter = PacketHandler.HasServerFilter(id);
                
                if (isView)
                {
                    reader = new PacketReader(ptr, length, PacketsTable.IsDynLength(id));
                    result = !PacketHandler.OnServerPacket(id, reader, packet);
                }

                if (isFilter)
                {
                    byte[] data = new byte[length];

                    for (int i = 0; i < length; ++i)
                    {
                        data[i] = ptr[i];
                    }

                    packet = new Packet(data, length, PacketsTable.IsDynLength(id));
                    result = !PacketHandler.OnClientPacket(id, reader, packet);

                    data = packet.Compile();
                    length = (int)packet.Length;

                    for (int i = 0; i < length; ++i)
                    {
                        data[i] = ptr[i];
                    }
                }

                if (Packet.Logging)
                {
                    Packet.Log(PacketPath.ServerToClient, ptr, length, !result);
                }

                return result;
            }
        }

        private unsafe bool OnSend(ref byte[] buffer, ref int length)
        {
            m_Out += (uint) length;
            fixed (byte * ptr = buffer)
            {
                bool result = true;
                byte id = ptr[0];

                PacketReader reader = null;
                Packet packet = null;
                bool isView = PacketHandler.HasClientViewer(id);
                bool isFilter = PacketHandler.HasClientFilter(id);
                
                if (isView)
                {
                    reader = new PacketReader(ptr, length, PacketsTable.IsDynLength(id));
                    result = !PacketHandler.OnClientPacket(id, reader, packet);
                }
                else if (isFilter)
                {
                    byte[] data = new byte[length];

                    for (int i = 0; i < length; ++i)
                    {
                        data[i] = ptr[i];
                    }

                    packet = new Packet(data, length, PacketsTable.IsDynLength(id));
                    result = !PacketHandler.OnClientPacket(id, reader, packet);

                    data = packet.Compile();
                    length = (int) packet.Length;

                    for (int i = 0; i < length; ++i)
                    {
                        data[i] = ptr[i];
                    }
                }

                if (Packet.Logging)
                {
                    Packet.Log(PacketPath.ClientToServer, ptr, length, !result);
                }

                return result;
            }
        }

        private void OnMouseHandler(int button, int wheel)
        {
            if (button > 4)
                button = 3;
            else if (button > 3)
                button = 2;
            else if (button > 2)
                button = 2;
            else if (button > 1)
                button = 1;

            HotKey.OnMouse(button, wheel);
        }

        private enum SDL_Keymod
        {
            KMOD_NONE = 0x0000,
            KMOD_LSHIFT = 0x0001,
            KMOD_RSHIFT = 0x0002,
            KMOD_LCTRL = 0x0040,
            KMOD_RCTRL = 0x0080,
            KMOD_LALT = 0x0100,
            KMOD_RALT = 0x0200,
            KMOD_LGUI = 0x0400,
            KMOD_RGUI = 0x0800,
            KMOD_NUM = 0x1000,
            KMOD_CAPS = 0x2000,
            KMOD_MODE = 0x4000,
            KMOD_RESERVED = 0x8000
        }

        private enum SDL_Keycode_Ignore
        {
            SDLK_LCTRL = 1073742048,
            SDLK_LSHIFT = 1073742049,
            SDLK_LALT = 1073742050,
            SDLK_RCTRL = 1073742052,
            SDLK_RSHIFT = 1073742053,
            SDLK_RALT = 1073742054,
        }

        private bool OnHotKeyHandler(int key, int mod, bool ispressed)
        {
            if (ispressed && !Enum.IsDefined(typeof(SDL_Keycode_Ignore), key))
            {
                ModKeys cur = ModKeys.None;
                SDL_Keymod keymod = (SDL_Keymod) mod;
                if (keymod.HasFlag(SDL_Keymod.KMOD_LCTRL) || keymod.HasFlag(SDL_Keymod.KMOD_RCTRL))
                    cur |= ModKeys.Control;
                if (keymod.HasFlag(SDL_Keymod.KMOD_LALT) || keymod.HasFlag(SDL_Keymod.KMOD_RALT))
                    cur |= ModKeys.Alt;
                if (keymod.HasFlag(SDL_Keymod.KMOD_LSHIFT) || keymod.HasFlag(SDL_Keymod.KMOD_RSHIFT))
                    cur |= ModKeys.Shift;
                return HotKey.OnKeyDown(Win32Platform.MapKey(key), cur);
            }

            return true;
        }

        private void OnDisconnected()
        {
            PacketHandlers.Party.Clear();
            //TODO reset window title
            Engine.MainWindow.UpdateTitle();
            UOAssist.PostLogout();

            World.Player = null;
            World.Items.Clear();
            World.Mobiles.Clear();
            Macros.MacroManager.Stop();
            ScriptManager.OnLogout();
            ActionQueue.Stop();
            Counter.Reset();
            GoldPerHourTimer.Stop();
            BandageTimer.Stop();
            GateTimer.Stop();
            WaypointManager.StopTimer();
            BuffsTimer.Stop();
            StealthSteps.Unhide();
            Engine.MainWindow.OnLogout();
            if (Engine.MainWindow.MapWindow != null)
                Engine.MainWindow.MapWindow.Close();
            PacketHandlers.Party.Clear();
            PacketHandlers.IgnoreGumps.Clear();
            Agents.BuyAgent.OnDisconnected();
            Config.Save();
        }

        private void OnConnected()
        {
            m_ConnectionStart = DateTime.UtcNow;
        }

        private void OnClientClosing()
        {
            var last = Console.BackgroundColor;
            var lastFore = Console.ForegroundColor;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Closing Razor instance");
            Console.BackgroundColor = last;
            Console.ForegroundColor = lastFore;
            Engine.Close();
        }

        private void OnInitialize()
        {
            var last = Console.BackgroundColor;
            var lastFore = Console.ForegroundColor;
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Initialized Razor instance");
            Console.BackgroundColor = last;
            Console.ForegroundColor = lastFore;
        }

        public override void SetConnectionInfo(IPAddress addr, int port)
        {
        }

        public override void SetNegotiate(bool negotiate)
        {
        }

        public override bool Attach(int pid)
        {
            return false;
        }

        public override void Close()
        {
        }

        public override void UpdateTitleBar()
        {
            if (!ClientRunning)
                return;

            if (World.Player != null && Config.GetBool("TitleBarDisplay"))
            {
                ResetTitleBarBuilder();

                TitleBarBuilder.Replace(@"{char}", World.Player.Name);

                TitleBarBuilder.Replace(@"{crimtime}",
                    World.Player.CriminalTime != 0 ? $"{World.Player.CriminalTime}" : "-");

                TitleBarBuilder.Replace(@"{hp}", $"{World.Player.Hits}");
                TitleBarBuilder.Replace(@"{mana}", $"{World.Player.Mana}");
                TitleBarBuilder.Replace(@"{stam}", $"{World.Player.Stam}");

                TitleBarBuilder.Replace(@"{weight}", World.Player.Weight.ToString());

                TitleBarBuilder.Replace(@"{bandage}", BandageTimer.Running ? $"{BandageTimer.Count}" : "-");

                TitleBarBuilder.Replace(@"{statbar}", string.Empty);
                TitleBarBuilder.Replace(@"{mediumstatbar}", string.Empty);
                TitleBarBuilder.Replace(@"{largestatbar}", string.Empty);

                foreach (Counter c in Counter.List)
                {
                    if (c.Enabled)
                        TitleBarBuilder.Replace($"{{{c.Format}}}", c.GetTitlebarString(false, false));
                }

                base.UpdateTitleBar();
            }
            else
            {
                SetTitleStr(string.Empty);
            }
        }


        public override void SetTitleStr(string str)
        {
            _setTitle(str);
        }

        public override bool OnMessage(MainForm razor, uint wParam, int lParam)
        {
            return false;
        }

        public override bool OnCopyData(IntPtr wparam, IntPtr lparam)
        {
            return false;
        }

        public override unsafe void SendToServer(Packet p)
        {
            byte[] data = p.Compile();
            int length = (int) p.Length;

            _sendToServer(ref data, ref length);
        }

        public override void SendToServer(PacketReader pr)
        {
            SendToServer(MakePacketFrom(pr));
        }

        public override unsafe void SendToClient(Packet p)
        {
            byte[] data = p.Compile();
            int length = (int) p.Length;

            _sendToClient(ref data, ref length);
        }

        public override void SendPacketToClient(byte[] packet, int length)
        {
            _sendToClient(ref packet, ref length);
        }

        public override unsafe void ForceSendToClient(Packet p)
        {
            byte[] data = p.Compile();
            int length = (int) p.Length;

            fixed (byte* ptr = data)
            {
                _sendToClient(ref data, ref length);

                if (Packet.Logging)
                {
                    Packet.Log(PacketPath.RazorToClient, ptr, data.Length);
                }
            }
        }

        public override unsafe void ForceSendToServer(Packet p)
        {
            byte[] data = p.Compile();
            int length = (int) p.Length;

            fixed (byte* ptr = data)
            {
                _sendToServer(ref data, ref length);

                if (Packet.Logging)
                {
                    Packet.Log(PacketPath.RazorToServer, ptr, data.Length);
                }
            }
        }

        public override void SetPosition(uint x, uint y, uint z, byte dir)
        {
        }

        public override string GetClientVersion()
        {
            return m_ClientVersion;
        }

        public override string GetUoFilePath()
        {
            return _uoFilePath();
        }

        public override IntPtr GetWindowHandle()
        {
            return m_ClientWindow;
        }

        public override uint TotalDataIn()
        {
            return m_In;
        }

        public override uint TotalDataOut()
        {
            return m_Out;
        }

        internal override void RequestMove(Direction m_Dir)
        {
            _requestMove((int) m_Dir, false);
        }

        public void OnFocusGained()
        {
            if (Engine.MainWindow == null)
                return;

            if (Config.GetBool("AlwaysOnTop"))
            {
                if (!Engine.MainWindow.TopMost)
                {
                    Platform.SetForegroundWindow(GetWindowHandle());

                    Engine.MainWindow.SafeAction(s =>
                    {
                        s.TopMost = true;
                        s.BringToFront();
                    });
                }
            }

            // always use smartness for the map window
            if (Engine.MainWindow.MapWindow != null && Engine.MainWindow.MapWindow.Visible)
            {
                if (!Engine.MainWindow.MapWindow.TopMost)
                {
                    Engine.MainWindow.MapWindow.SafeAction(s =>
                    {
                        s.TopMost = true;
                        s.BringToFront();
                    });
                }
            }
        }

        public void OnFocusLost()
        {
            if (Engine.MainWindow == null)
                return;

            bool razorfocus = Form.ActiveForm == Engine.MainWindow || Form.ActiveForm == Engine.MainWindow.MapWindow;
            if (Config.GetBool("AlwaysOnTop"))
            {
                if (Engine.MainWindow.TopMost && !razorfocus)
                {
                    Engine.MainWindow.SafeAction(s =>
                    {
                        s.TopMost = false;
                        s.SendToBack();
                    });
                }
            }

            // always use smartness for the map window
            if (Engine.MainWindow.MapWindow != null && Engine.MainWindow.MapWindow.Visible && !razorfocus)
            {
                if (Engine.MainWindow.MapWindow.TopMost)
                {
                    Engine.MainWindow.MapWindow.SafeAction(s =>
                    {
                        s.TopMost = false;
                        s.SendToBack();
                    });
                }
            }
        }
    }
}