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
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Assistant
{
    public unsafe class OSIClient : Client
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
            SmartCPU = 21,
            Negotiate = 22,
            SetMapHWnd = 23,
            OnTick = 24,
        }

        public enum UONetMessageCopyData
        {
            Position = 1,
        }

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

        [StructLayout(LayoutKind.Explicit, Size = 8 + SHARED_BUFF_SIZE)]
        private struct Buffer
        {
            [FieldOffset(0)] public int Length;
            [FieldOffset(4)] public int Start;
            [FieldOffset(8)] public byte Buff0;
        }

        [DllImport("Crypt.dll")]
        private static unsafe extern int InstallLibrary(IntPtr thisWnd, int procid, int features);

        [DllImport("Crypt.dll")]
        private static unsafe extern void Shutdown(bool closeClient);

        [DllImport("Crypt.dll")]
        internal static unsafe extern IntPtr FindUOWindow();

        [DllImport("Crypt.dll")]
        private static unsafe extern IntPtr GetSharedAddress();

        [DllImport("Crypt.dll")]
        private static unsafe extern IntPtr GetCommMutex();

        [DllImport("Crypt.dll")]
        internal static unsafe extern uint TotalIn();

        [DllImport("Crypt.dll")]
        internal static unsafe extern uint TotalOut();

        [DllImport("Crypt.dll")]
        private static unsafe extern void WaitForWindow(int pid);

        [DllImport("Crypt.dll")]
        internal static unsafe extern void SetDataPath(string path);

        [DllImport("Crypt.dll")]
        internal static unsafe extern void CalibratePosition(uint x, uint y, uint z, byte dir);

        [DllImport("Crypt.dll")]
        private static unsafe extern void SetServer(uint ip, ushort port);

        [DllImport("Crypt.dll")]
        internal static unsafe extern string GetUOVersion();

        [DllImport("Loader.dll")]
        private static unsafe extern uint Load(string exe, string dll, string func, void* dllData, int dataLen,
            out uint pid);

        private Queue<Packet> m_SendQueue = new Queue<Packet>();
        private Queue<Packet> m_RecvQueue = new Queue<Packet>();

        private bool m_QueueRecv;
        private bool m_QueueSend;

        private Buffer* m_InRecv;
        private Buffer* m_OutRecv;
        private Buffer* m_InSend;
        private Buffer* m_OutSend;
        private byte* m_TitleStr;
        private Mutex CommMutex;
        private Process ClientProc;

        private bool m_Ready = false;
        private string m_LastStr = "";
        private DateTime m_ConnStart;
        private IPAddress m_LastConnection;

        public override DateTime ConnectionStart
        {
            get { return m_ConnStart; }
        }

        public override IPAddress LastConnection
        {
            get { return m_LastConnection; }
        }

        public override Process ClientProcess
        {
            get { return ClientProc; }
        }

        public override bool ClientRunning
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

        public override void SetMapWndHandle(Form mapWnd)
        {
            Platform.PostMessage(FindUOWindow(), WM_UONETEVENT, (IntPtr) UONetMessage.SetMapHWnd, mapWnd.Handle);
        }

        public override void RequestStatbarPatch(bool preAOS)
        {
            Platform.PostMessage(FindUOWindow(), WM_UONETEVENT, (IntPtr) UONetMessage.StatBar,
                preAOS ? (IntPtr) 1 : IntPtr.Zero);
        }

        public override void SetCustomNotoHue(int hue)
        {
            Platform.PostMessage(FindUOWindow(), WM_UONETEVENT, (IntPtr) UONetMessage.NotoHue, (IntPtr) hue);
        }

        public override void SetSmartCPU(bool enabled)
        {
            if (enabled)
                try
                {
                    ClientProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.Normal;
                }
                catch
                {
                }

            Platform.PostMessage(FindUOWindow(), WM_UONETEVENT, (IntPtr) UONetMessage.SmartCPU,
                (IntPtr) (enabled ? 1 : 0));
        }

        public override void SetGameSize(int x, int y)
        {
            Platform.PostMessage(FindUOWindow(), WM_UONETEVENT, (IntPtr) UONetMessage.SetGameSize,
                (IntPtr) ((x & 0xFFFF) | ((y & 0xFFFF) << 16)));
        }

        public override Loader_Error LaunchClient(string client)
        {
            string dll = Path.Combine(Config.GetInstallDirectory(), "Crypt.dll");
            uint pid = 0;
            Loader_Error err = (Loader_Error) Load(client, dll, "OnAttach", null, 0, out pid);

            if (err == Loader_Error.SUCCESS)
            {
                try
                {
                    ClientProc = Process.GetProcessById((int) pid);
                }
                catch
                {
                    // ignore
                }
            }

            if (ClientProc == null)
                return Loader_Error.UNKNOWN_ERROR;
            else
                return err;
        }

        private bool m_ClientEnc = false;

        public override bool ClientEncrypted
        {
            get { return m_ClientEnc; }
            set { m_ClientEnc = value; }
        }

        private bool m_ServerEnc = false;

        public override bool ServerEncrypted
        {
            get { return m_ServerEnc; }
            set { m_ServerEnc = value; }
        }

        public override bool InstallHooks(IntPtr mainWindow)
        {
            InitError error;
            int flags = 0;

            if (m_Ready)
                return false; // double init

            if (Config.GetBool("Negotiate"))
                flags |= 0x04;

            if (ClientEncrypted)
                flags |= 0x08;

            if (ServerEncrypted)
                flags |= 0x10;

            WaitForWindow(ClientProc.Id);

            error = (InitError) InstallLibrary(mainWindow, ClientProc.Id, flags);
            if (error != InitError.SUCCESS)
            {
                FatalInit(error);
                return false;
            }

            // When InstallLibrary finishes, we get a UONETEVENT saying it's ready.
            return true;
        }

        private uint m_ServerIP;
        private ushort m_ServerPort;

        public override void SetConnectionInfo(IPAddress addr, int port)
        {
#pragma warning disable 618
            m_ServerIP = (uint) addr.Address;
#pragma warning restore 618
            m_ServerPort = (ushort) port;
        }

        public override void SetNegotiate(bool negotiate)
        {
            Platform.PostMessage(FindUOWindow(), WM_UONETEVENT, (IntPtr) UONetMessage.Negotiate,
                (IntPtr) (negotiate ? 1 : 0));
        }

        public override bool Attach(int pid)
        {
            ClientProc = null;
            ClientProc = Process.GetProcessById(pid);
            return ClientProc != null;
        }

        public override void Close()
        {
            Shutdown(true);
            if (ClientProc != null && !ClientProc.HasExited)
                ClientProc.CloseMainWindow();
            ClientProc = null;
        }

        private string EncodeColorStat(int val, int max)
        {
            double perc = ((double) val) / ((double) max);

            if (perc <= 0.25)
                return String.Format("~#FF0000{0}~#~", val);

            if (perc <= 0.75)
                return String.Format("~#FFFF00{0}~#~", val);

            return val.ToString();
        }

        public override void UpdateTitleBar()
        {
            if (!ClientRunning)
                return;

            if (World.Player != null && Config.GetBool("TitleBarDisplay"))
            {
                ResetTitleBarBuilder();

                TitleBarBuilder.Replace(@"{char}",
                    Config.GetBool("ShowNotoHue")
                        ? $"~#{World.Player.GetNotorietyColor() & 0x00FFFFFF:X6}{World.Player.Name}~#~"
                        : World.Player.Name);

                TitleBarBuilder.Replace(@"{crimtime}",
                    World.Player.CriminalTime != 0 ? $"~^C0C0C0{World.Player.CriminalTime}~#~" : "-");

                TitleBarBuilder.Replace(@"{hp}",
                    World.Player.Poisoned
                        ? $"~#FF8000{World.Player.Hits}~#~"
                        : EncodeColorStat(World.Player.Hits, World.Player.HitsMax));
                TitleBarBuilder.Replace(@"{mana}", EncodeColorStat(World.Player.Mana, World.Player.ManaMax));
                TitleBarBuilder.Replace(@"{stam}", EncodeColorStat(World.Player.Stam, World.Player.StamMax));

                TitleBarBuilder.Replace(@"{weight}",
                    World.Player.Weight >= World.Player.MaxWeight
                        ? $"~#FF0000{World.Player.Weight}~#~"
                        : World.Player.Weight.ToString());

                TitleBarBuilder.Replace(@"{bandage}", BandageTimer.Running ? $"~#FF8000{BandageTimer.Count}~#~" : "-");

                string statStr = String.Format("{0}{1:X2}{2:X2}{3:X2}",
                    (int) (World.Player.GetStatusCode()),
                    (int) (World.Player.HitsMax == 0 ? 0 : (double) World.Player.Hits / World.Player.HitsMax * 99),
                    (int) (World.Player.ManaMax == 0 ? 0 : (double) World.Player.Mana / World.Player.ManaMax * 99),
                    (int) (World.Player.StamMax == 0 ? 0 : (double) World.Player.Stam / World.Player.StamMax * 99));

                TitleBarBuilder.Replace(@"{statbar}", $"~SR{statStr}");
                TitleBarBuilder.Replace(@"{mediumstatbar}", $"~SL{statStr}");
                TitleBarBuilder.Replace(@"{largestatbar}", $"~SX{statStr}");

                bool dispImg = Config.GetBool("TitlebarImages");
                for (int i = 0; i < Counter.List.Count; i++)
                {
                    Counter c = Counter.List[i];
                    if (c.Enabled)
                        TitleBarBuilder.Replace($"{{{c.Format}}}",
                            c.GetTitlebarString(dispImg && c.DisplayImage, true));
                }

                base.UpdateTitleBar();
            }
            else
            {
                SetTitleStr(string.Empty);
            }
        }

        private const int WM_CUSTOMTITLE = Client.WM_USER + 2;

        public override void SetTitleStr(string str)
        {
            if (m_LastStr == str)
                return;

            m_LastStr = str;
            byte[] copy = System.Text.Encoding.ASCII.GetBytes(str);
            int clen = copy.Length;
            if (clen >= 512)
                clen = 511;

            CommMutex.WaitOne();
            if (clen > 0)
            {
                fixed (byte* array = copy)
                    Platform.memcpy(m_TitleStr, array, clen);
            }

            *(m_TitleStr + clen) = 0;
            CommMutex.ReleaseMutex();

            Platform.PostMessage(FindUOWindow(), WM_CUSTOMTITLE, IntPtr.Zero, IntPtr.Zero);
        }

        private void FatalInit(InitError error)
        {
            StringBuilder sb = new StringBuilder(Language.GetString(LocString.InitError));
            sb.AppendFormat("{0}\n", error);
            sb.Append(Language.GetString((int) (LocString.InitError + (int) error)));

            MessageBox.Show(Engine.ActiveWindow, sb.ToString(), "Init Error", MessageBoxButtons.OK,
                MessageBoxIcon.Stop);
        }

        private void OnLogout(bool fake)
        {
            if (!fake)
            {
                PacketHandlers.Party.Clear();

                SetTitleStr("");
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

            //TranslateEnabled = false;
        }

        //private DateTime m_LastActivate;
        public override bool OnMessage(MainForm razor, uint wParam, int lParam)
        {
            bool retVal = true;

            switch ((UONetMessage) (wParam & 0xFFFF))
            {
                case UONetMessage.Ready: //Patch status
                    if (lParam == (int) InitError.NO_MEMCOPY)
                    {
                        if (MessageBox.Show(Engine.ActiveWindow, Language.GetString(LocString.NoMemCpy),
                                "No Client MemCopy", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==
                            DialogResult.No)
                        {
                            m_Ready = false;
                            ClientProc = null;
                            Engine.MainWindow.CanClose = true;
                            Engine.MainWindow.Close();
                            break;
                        }
                    }

                    byte* baseAddr = (byte*) GetSharedAddress().ToPointer();

                    m_InRecv = (Buffer*) baseAddr;
                    m_OutRecv = (Buffer*) (baseAddr + sizeof(Buffer));
                    m_InSend = (Buffer*) (baseAddr + sizeof(Buffer) * 2);
                    m_OutSend = (Buffer*) (baseAddr + sizeof(Buffer) * 3);
                    m_TitleStr = (byte*) (baseAddr + sizeof(Buffer) * 4);

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
                        RequestStatbarPatch(true);

                    m_Ready = true;
                    Engine.MainWindow.MainForm_EndLoad();
                    break;

                case UONetMessage.NotReady:
                    m_Ready = false;
                    FatalInit((InitError) lParam);
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
                        m_LastConnection = new IPAddress((uint) lParam);
                    }
                    catch
                    {
                    }

                    break;
                case UONetMessage.Disconnect:
                    OnLogout(false);
                    break;
                case UONetMessage.Close:
                    OnLogout(true);
                    ClientProc = null;
                    Engine.MainWindow.CanClose = true;
                    Engine.MainWindow.Close();
                    break;

                // Hot Keys
                case UONetMessage.Mouse:
                    HotKey.OnMouse((ushort) (lParam & 0xFFFF), (short) (lParam >> 16));
                    break;
                case UONetMessage.KeyDown:
                    retVal = HotKey.OnKeyDown(lParam, ModKeys.None);
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
                    if (Config.GetBool("AlwaysOnTop"))
                    {
                        if (lParam != 0 && !razor.TopMost)
                        {
                            razor.TopMost = true;
                            Platform.SetForegroundWindow(FindUOWindow());
                        }
                        else if (lParam == 0 && razor.TopMost)
                        {
                            razor.TopMost = false;
                            razor.SendToBack();
                        }
                    }

                    // always use smartness for the map window
                    if (razor.MapWindow != null && razor.MapWindow.Visible)
                    {
                        if (lParam != 0 && !razor.MapWindow.TopMost)
                        {
                            razor.MapWindow.TopMost = true;
                            Platform.SetForegroundWindow(FindUOWindow());
                        }
                        else if (lParam == 0 && razor.MapWindow.TopMost)
                        {
                            razor.MapWindow.TopMost = false;
                            razor.MapWindow.SendToBack();
                        }
                    }

                    break;

                case UONetMessage.DLL_Error:
                {
                    string error = "Unknown";
                    switch ((UONetMessage) lParam)
                    {
                        case UONetMessage.StatBar:
                            error = "Unable to patch status bar.";
                            break;
                    }

                    MessageBox.Show(Engine.ActiveWindow, "An Error has occured : \n" + error, "Error Reported",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }

                case UONetMessage.OnTick:
                    // Game engine tick
                    Timer.Slice();
                    break;

                // Unknown
                default:
                    MessageBox.Show(Engine.ActiveWindow, "Unknown message from uo client\n" + ((int) wParam).ToString(),
                        "Error?");
                    break;
            }

            return retVal;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct CopyData
        {
            public int dwData;
            public int cbDAta;
            public IntPtr lpData;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct Position
        {
            public ushort x;
            public ushort y;
            public ushort z;
        };

        public override bool OnCopyData(IntPtr wparam, IntPtr lparam)
        {
            CopyData copydata = (CopyData) Marshal.PtrToStructure(lparam, typeof(CopyData));

            switch ((UONetMessageCopyData) copydata.dwData)
            {
                case UONetMessageCopyData.Position:
                    if (World.Player != null)
                    {
                        Position pos = (Position) Marshal.PtrToStructure(copydata.lpData, typeof(Position));
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

        public override void SendToServer(Packet p)
        {
            if (!m_Ready)
                return;

            if (!m_QueueSend)
            {
                ForceSendToServer(p);
            }
            else
            {
                m_SendQueue.Enqueue(p);
            }
        }

        public override void SendToServer(PacketReader pr)
        {
            if (!m_Ready)
                return;

            SendToServer(MakePacketFrom(pr));
        }

        public override void SendToClient(Packet p)
        {
            if (!m_Ready || p.Length <= 0)
                return;

            if (!m_QueueRecv)
            {
                ForceSendToClient(p);
            }
            else
            {
                m_RecvQueue.Enqueue(p);
            }
        }

        public override void SendPacketToClient(byte[] packet, int length)
        {
            //_sendToClient(ref packet, ref length);
        }

        public override void ForceSendToClient(Packet p)
        {
            byte[] data = p.Compile();

            CommMutex.WaitOne();
            fixed (byte* ptr = data)
            {
                Packet.Log(PacketPath.RazorToClient, ptr, data.Length);
                CopyToBuffer(m_OutRecv, ptr, data.Length);
            }

            CommMutex.ReleaseMutex();
        }

        public override void ForceSendToServer(Packet p)
        {
            if (p == null || p.Length <= 0)
                return;

            byte[] data = p.Compile();

            CommMutex.WaitOne();
            InitSendFlush();
            fixed (byte* ptr = data)
            {
                Packet.Log(PacketPath.RazorToServer, ptr, data.Length);
                CopyToBuffer(m_OutSend, ptr, data.Length);
            }

            CommMutex.ReleaseMutex();
        }

        private void InitSendFlush()
        {
            if (m_OutSend->Length == 0)
                Platform.PostMessage(FindUOWindow(), WM_UONETEVENT, (IntPtr) UONetMessage.Send, IntPtr.Zero);
        }

        private void CopyToBuffer(Buffer* buffer, byte* data, int len)
        {
            //if ( buffer->Length + buffer->Start + len >= SHARED_BUFF_SIZE )
            //	throw new NullReferenceException( String.Format( "Buffer OVERFLOW in CopyToBuffer [{0} + {1}] <- {2}", buffer->Start, buffer->Length, len ) );

            Platform.memcpy((&buffer->Buff0) + buffer->Start + buffer->Length, data, len);
            buffer->Length += len;
        }

        private void HandleComm(Buffer* inBuff, Buffer* outBuff, Queue<Packet> queue, PacketPath path)
        {
            CommMutex.WaitOne();
            while (inBuff->Length > 0)
            {
                byte* buff = (&inBuff->Buff0) + inBuff->Start;

                short len = PacketsTable.GetPacketLength(buff, inBuff->Length);
                if (len > inBuff->Length || len <= 0)
                    break;

                inBuff->Start += len;
                inBuff->Length -= len;

                bool viewer = false;
                bool filter = false;

                switch (path)
                {
                    case PacketPath.ClientToServer:
                        viewer = PacketHandler.HasClientViewer(buff[0]);
                        filter = PacketHandler.HasClientFilter(buff[0]);
                        break;
                    case PacketPath.ServerToClient:
                        viewer = PacketHandler.HasServerViewer(buff[0]);
                        filter = PacketHandler.HasServerFilter(buff[0]);
                        break;
                }

                Packet p = null;
                PacketReader pr = null;
                if (viewer)
                {
                    pr = new PacketReader(buff, len, PacketsTable.IsDynLength(buff[0]));
                    if (filter)
                        p = MakePacketFrom(pr);
                }
                else if (filter)
                {
                    byte[] temp = new byte[len];
                    fixed (byte* ptr = temp)
                        Platform.memcpy(ptr, buff, len);
                    p = new Packet(temp, len, PacketsTable.IsDynLength(buff[0]));
                }

                bool blocked = false;
                switch (path)
                {
                    // yes it should be this way
                    case PacketPath.ClientToServer:
                    {
                        blocked = PacketHandler.OnClientPacket(buff[0], pr, p);
                        break;
                    }

                    case PacketPath.ServerToClient:
                    {
                        blocked = PacketHandler.OnServerPacket(buff[0], pr, p);
                        break;
                    }
                }

                if (filter)
                {
                    byte[] data = p.Compile();
                    fixed (byte* ptr = data)
                    {
                        Packet.Log(path, ptr, data.Length, blocked);
                        if (!blocked)
                            CopyToBuffer(outBuff, ptr, data.Length);
                    }
                }
                else
                {
                    Packet.Log(path, buff, len, blocked);
                    if (!blocked)
                        CopyToBuffer(outBuff, buff, len);
                }

                while (queue.Count > 0)
                {
                    p = (Packet) queue.Dequeue();
                    byte[] data = p.Compile();
                    fixed (byte* ptr = data)
                    {
                        CopyToBuffer(outBuff, ptr, data.Length);
                        Packet.Log((PacketPath) (((int) path) + 1), ptr, data.Length);
                    }
                }
            }

            CommMutex.ReleaseMutex();
        }

        private void OnRecv()
        {
            m_QueueRecv = true;
            HandleComm(m_InRecv, m_OutRecv, m_RecvQueue, PacketPath.ServerToClient);
            m_QueueRecv = false;
        }

        private void OnSend()
        {
            m_QueueSend = true;
            HandleComm(m_InSend, m_OutSend, m_SendQueue, PacketPath.ClientToServer);
            m_QueueSend = false;
        }

        public override void SetPosition(uint x, uint y, uint z, byte dir)
        {
            CalibratePosition(x, y, z, dir);
        }

        private static uint WM_KEYDOWN = 0x100, WM_KEYUP = 0x101;

        public void KeyPress(int keyCode)
        {
            Platform.SendMessage(FindUOWindow(), WM_KEYDOWN, (IntPtr) keyCode, (IntPtr) 1);
        }

        public override string GetClientVersion()
        {
            return GetUOVersion();
        }

        public override string GetUoFilePath()
        {
            return ConfigurationManager.AppSettings["UODataDir"];
        }

        public override IntPtr GetWindowHandle()
        {
            return FindUOWindow();
        }

        public override uint TotalDataIn()
        {
            return TotalIn();
        }

        public override uint TotalDataOut()
        {
            return TotalOut();
        }

        private enum KeyboardDir
        {
            North = 0x21, //page up
            Right = 0x27, // right
            East = 0x22, // page down
            Down = 0x28, // down
            South = 0x23, // end
            Left = 0x25, // left
            West = 0x24, // home
            Up = 0x26, // up
        }

        internal override void RequestMove(Direction m_Dir)
        {
            int direction;

            switch (m_Dir)
            {
                case Direction.Down:
                    direction = (int) KeyboardDir.Down;
                    break;
                case Direction.East:
                    direction = (int) KeyboardDir.East;
                    break;
                case Direction.Left:
                    direction = (int) KeyboardDir.Left;
                    break;
                case Direction.North:
                    direction = (int) KeyboardDir.North;
                    break;
                case Direction.Right:
                    direction = (int) KeyboardDir.Right;
                    break;
                case Direction.South:
                    direction = (int) KeyboardDir.South;
                    break;
                case Direction.Up:
                    direction = (int) KeyboardDir.Up;
                    break;
                case Direction.West:
                    direction = (int) KeyboardDir.West;
                    break;
                default:
                    direction = (int) KeyboardDir.Up;
                    break;
            }

            KeyPress(direction);
        }
    }
};