using Assistant.UI;
using CUO_API;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Assistant
{
    public partial class Engine
    {
        public static unsafe void Install(PluginHeader* plugin)
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

            if (!(Client.Instance as ClassicUOClient).Install(plugin))
            {
                Process.GetCurrentProcess().Kill();
                return;
            }

            /* Load localization files */
            if (!Language.Load("ENU"))
            {
                SplashScreen.End();
                MessageBox.Show(
                    String.Format(
                        "WARNING: Razor was unable to load the file Language/Razor_lang.ENU\n."),
                        "Language Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            m_Running = true;

            string clientPath = ((OnGetUOFilePath)Marshal.GetDelegateForFunctionPointer(plugin->GetUOFilePath, typeof(OnGetUOFilePath)))();

            Ultima.Files.SetMulPath(clientPath);
            Ultima.Multis.PostHSFormat = UsePostHSChanges;
            Client.Instance.ClientEncrypted = false;
            Client.Instance.ServerEncrypted = false;

            Language.LoadCliLoc();

            /* Initialize engine */
            SplashScreen.Message = LocString.Initializing;
            Initialize(typeof(Engine).Assembly);

            /* Load Profile */
            SplashScreen.Message = LocString.LoadingLastProfile;
            Config.LoadCharList();
            if (!Config.LoadLastProfile())
                MessageBox.Show(
                    "The selected profile could not be loaded, using default instead.", "Profile Load Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

            SplashScreen.Message = LocString.WaitingForClient;

            SplashScreen.End();

            Thread t = new Thread(() =>
            {
                RunUI();
            });
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
        
        private static OnPacketSendRecv _sendToClient, _sendToServer, _recv, _send;
        private static OnGetPacketLength _getPacketLength;
        private static OnGetPlayerPosition _getPlayerPosition;
        private static OnCastSpell _castSpell;
        private static OnGetStaticImage _getStaticImage;
        private static OnTick _tick;
        private static RequestMove _requestMove;
        private static OnSetTitle _setTitle;
        private static OnGetUOFilePath _uoFilePath;


        private static OnHotkey _onHotkeyPressed;
        private static OnMouse _onMouse;
        private static OnUpdatePlayerPosition _onUpdatePlayerPosition;
        private static OnClientClose _onClientClose;
        private static OnInitialize _onInitialize;
        private static OnConnected _onConnected;
        private static OnDisconnected _onDisconnected;
        private static OnFocusGained _onFocusGained;
        private static OnFocusLost _onFocusLost;
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

        public unsafe bool Install( PluginHeader* header)
        {
            _sendToClient = (OnPacketSendRecv)Marshal.GetDelegateForFunctionPointer( header->Recv, typeof( OnPacketSendRecv ) );
            _sendToServer = (OnPacketSendRecv)Marshal.GetDelegateForFunctionPointer( header->Send, typeof( OnPacketSendRecv ) );
            _getPacketLength = (OnGetPacketLength)Marshal.GetDelegateForFunctionPointer( header->GetPacketLength, typeof( OnGetPacketLength ) );
            _getPlayerPosition = (OnGetPlayerPosition)Marshal.GetDelegateForFunctionPointer( header->GetPlayerPosition, typeof( OnGetPlayerPosition ) );
            _castSpell = (OnCastSpell)Marshal.GetDelegateForFunctionPointer( header->CastSpell, typeof( OnCastSpell ) );
            _getStaticImage = (OnGetStaticImage)Marshal.GetDelegateForFunctionPointer( header->GetStaticImage, typeof( OnGetStaticImage ) );
            _requestMove = (RequestMove)Marshal.GetDelegateForFunctionPointer( header->RequestMove, typeof( RequestMove ) );
            _setTitle = (OnSetTitle)Marshal.GetDelegateForFunctionPointer( header->SetTitle, typeof( OnSetTitle ) );
            _uoFilePath = (OnGetUOFilePath)Marshal.GetDelegateForFunctionPointer(header->GetUOFilePath, typeof(OnGetUOFilePath));
            m_ClientVersion = new Version( (byte)(header->ClientVersion >> 24), (byte)(header->ClientVersion >> 16), (byte)(header->ClientVersion >> 8), (byte)header->ClientVersion ).ToString();
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
            header->Tick = Marshal.GetFunctionPointerForDelegate( _tick );
            header->OnRecv = Marshal.GetFunctionPointerForDelegate( _recv );
            header->OnSend = Marshal.GetFunctionPointerForDelegate( _send );
            header->OnHotkeyPressed = Marshal.GetFunctionPointerForDelegate( _onHotkeyPressed );
            header->OnMouse = Marshal.GetFunctionPointerForDelegate( _onMouse );
            header->OnPlayerPositionChanged = Marshal.GetFunctionPointerForDelegate( _onUpdatePlayerPosition );
            header->OnClientClosing = Marshal.GetFunctionPointerForDelegate( _onClientClose );
            header->OnInitialize = Marshal.GetFunctionPointerForDelegate( _onInitialize );
            header->OnConnected = Marshal.GetFunctionPointerForDelegate( _onConnected );
            header->OnDisconnected = Marshal.GetFunctionPointerForDelegate( _onDisconnected );
            header->OnFocusGained = Marshal.GetFunctionPointerForDelegate( _onFocusGained );
            header->OnFocusLost = Marshal.GetFunctionPointerForDelegate( _onFocusLost );

            return true;
        }
        public unsafe override bool InstallHooks(IntPtr pluginPtr)
        {
            Engine.MainWindow.SafeAction((s) => {
                Engine.MainWindow.MainForm_EndLoad();
            });
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

        private unsafe bool OnRecv(ref byte[] data, ref int length)
        {
            m_In += (uint)length;
            fixed (byte* ptr = data)
            {
                bool result = true;
                byte id = data[0];

                PacketReader reader = null;
                Packet packet = null;
                bool isView = PacketHandler.HasServerViewer(id);
                bool isFilter = PacketHandler.HasServerFilter(id);

                if (isView)
                {
                    reader = new PacketReader(ptr, length, PacketsTable.IsDynLength(id));
                    result = !PacketHandler.OnServerPacket(id, reader, packet);
                }
                else if (isFilter)
                {
                    packet = new Packet(data, length, PacketsTable.IsDynLength(id));
                    result = !PacketHandler.OnServerPacket(id, reader, packet);

                    data = packet.Compile();
                    length = (int) packet.Length;
                }

                return result;
            }
        }

        private unsafe bool OnSend(ref byte[] data, ref int length)
        {
            m_Out += (uint)length;
            fixed (byte* ptr = data)
            {
                bool result = true;
                byte id = data[0];

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
                    packet = new Packet(data, length, PacketsTable.IsDynLength(id));
                    result = !PacketHandler.OnClientPacket(id, reader, packet);

                    data = packet.Compile();
                    length = (int)packet.Length;
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
        private bool OnHotKeyHandler(int key, int mod, bool ispressed)
        {
            int keycode = key;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                keycode = Win32Platform.MapKey(key);

            if (ispressed)
            {
                return HotKey.OnKeyDown((int)(keycode | mod));;
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
            ActionQueue.Stop();
            Counter.Reset();
            GoldPerHourTimer.Stop();
            BandageTimer.Stop();
            GateTimer.Stop();
            BuffsTimer.Stop();
            StealthSteps.Unhide();
            Engine.MainWindow.OnLogout();
            if (Engine.MainWindow.MapWindow != null)
                Engine.MainWindow.MapWindow.Close();
            PacketHandlers.Party.Clear();
            PacketHandlers.IgnoreGumps.Clear();
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

                TitleBarBuilder.Replace(@"{crimtime}", World.Player.CriminalTime != 0 ? $"{World.Player.CriminalTime}" : "-");

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


        public override void SetTitleStr( string str )
        {
            _setTitle( str );
        }

        public override bool OnMessage(MainForm razor, uint wParam, int lParam)
        {
            return false;
        }

        public override bool OnCopyData(IntPtr wparam, IntPtr lparam)
        {
            return false;
        }

        public override void SendToServer(Packet p)
        {
            byte[] data = p.Compile();
            int length = (int) p.Length;
            _sendToServer(ref data, ref length);
        }

        public override void SendToServer(PacketReader pr)
        {
            SendToServer(MakePacketFrom(pr));
        }

        public override void SendToClient(Packet p)
        {
            byte[] data = p.Compile();
            int length = (int)p.Length;

            _sendToClient(ref data, ref length);

        }

        public override void ForceSendToClient(Packet p)
        {
            byte[] data = p.Compile();
            int length = (int)p.Length;

            _sendToClient(ref data, ref length);
        }

        public override void ForceSendToServer(Packet p)
        {
            byte[] data = p.Compile();
            int length = (int)p.Length;

            _sendToServer(ref data, ref length);
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

        internal override void RequestMove( Direction m_Dir )
        {
            _requestMove( (int)m_Dir, true );
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
