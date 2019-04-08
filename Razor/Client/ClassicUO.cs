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

    public class ClassicUOClient : Client
    {
        public override DateTime ConnectionStart { get; }
        public override IPAddress LastConnection { get; }
        public override Process ClientProcess { get; }

        public override bool ClientRunning { get; }

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

        public override bool InstallHooks(IntPtr mainWindow)
        {
            return false;
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

        public override void RequestTitlebarUpdate()
        {

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

        }

        public override void SendToServer(PacketReader pr)
        {

        }

        public override void SendToClient(Packet p)
        {

        }

        public override void ForceSendToClient(Packet p)
        {

        }

        public override void SetPosition(uint x, uint y, uint z, byte dir)
        {

        }

        public override void KeyPress(int keyCode)
        {

        }

        public override string GetClientVersion()
        {
            return "";
        }

        public override IntPtr GetWindowHandle()
        {
            return IntPtr.Zero;
        }

        public override uint TotalDataIn()
        {
            return 0;
        }

        public override uint TotalDataOut()
        {
            return 0;
        }
    }
}