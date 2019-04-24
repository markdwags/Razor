using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Assistant
{

    internal static unsafe class Win32Platform
    {
        [DllImport( "Platform.dll" )]
        internal static unsafe extern IntPtr CaptureScreen( IntPtr handle, bool isFullScreen, string msgStr );
        [DllImport( "Platform.dll" )]
        internal static unsafe extern void BringToFront( IntPtr hWnd );
        [DllImport( "user32.dll" )]
        internal static extern ushort GetAsyncKeyState( int key );
    }
    internal static unsafe class LinuxPlatform
    {
        [DllImport( "libX11" )]
        private static extern IntPtr XOpenDisplay( IntPtr display );
        [DllImport( "libX11" )]
        private static extern IntPtr XCloseDisplay( IntPtr display );
        [DllImport( "libX11" )]
        private static extern int XRaiseWindow( IntPtr display, IntPtr window );
        [DllImport( "libX11" )]
        private static extern int XGetInputFocus( IntPtr display, IntPtr window, IntPtr focus_return );
        [DllImport( "libX11" )]
        private static extern int XQueryKeymap( IntPtr display, byte[] keys );
        [DllImport( "libX11" )]
        private static extern int XKeysymToKeycode( IntPtr display, int key );

        private static IntPtr m_Display;
        private static IntPtr Display
        {
            get
            {
                if ( m_Display == IntPtr.Zero )
                    m_Display = XOpenDisplay( IntPtr.Zero );
                return m_Display;
            }
        }
        internal static void BringToFront( IntPtr window )
        {
            XRaiseWindow( Display, window );
        }

        internal static ushort GetAsyncKeyState( int key )
        {
            try
            {
                var szKey = new byte[32];
                int res = XQueryKeymap( Display, szKey );
                //foreach(var xx in szKey)
                //Console.WriteLine(xx + "-");
                int code = XKeysymToKeycode( Display, (int)key );
                bool pressed = ( szKey[code >> 3] & ( 1 << ( code & 7 ) ) ) == 0;
                var r = szKey[code / 8];
                var s = ( 1 << ( code % 8 ) );
                var x = r & s;
                return r == s ? (ushort)0xFF00 : (ushort)0 ;
            }
            catch
            {
                return 0;
            }
        }
    }
    internal static unsafe class Platform
    {
        internal static ushort GetAsyncKeyState( int key )
        {
            if ( Environment.OSVersion.Platform == PlatformID.Win32NT )
                return Win32Platform.GetAsyncKeyState( key );
            else if ( Environment.OSVersion.Platform == PlatformID.Unix )
                return LinuxPlatform.GetAsyncKeyState( key );
            else
                return 0;
        }
        internal static IntPtr CaptureScreen( IntPtr handle, bool isFullScreen, string msgStr )
        {
            if ( Environment.OSVersion.Platform == PlatformID.Win32NT )
                return Win32Platform.CaptureScreen( handle, isFullScreen, msgStr );
            else
                return IntPtr.Zero;
        }
        internal static void BringToFront( IntPtr window )
        {
            if ( Environment.OSVersion.Platform == PlatformID.Win32NT )
                Win32Platform.BringToFront( window );
            else
                LinuxPlatform.BringToFront( window );
        }

        [DllImport( "User32.dll" )]
        private static extern IntPtr GetSystemMenu( IntPtr wnd, bool reset );

        [DllImport( "User32.dll" )]
        private static extern IntPtr EnableMenuItem( IntPtr menu, uint item, uint options );


        [DllImport( "msvcrt.dll" )]
        internal static unsafe extern void memcpy( void* to, void* from, int len );

        [DllImport( "user32.dll" )]
        internal static extern uint PostMessage( IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam );
        [DllImport( "user32.dll" )]
        internal static extern bool SetForegroundWindow( IntPtr hWnd );

        [DllImport( "kernel32.dll" )]
        internal static extern uint GlobalGetAtomName( ushort atom, StringBuilder buff, int bufLen );

        [DllImport( "Advapi32.dll" )]
        internal static extern int GetUserNameA( StringBuilder buff, int* len );

        [DllImport( "user32.dll" )]
        internal static extern IntPtr SendMessage( IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam );
        

        public static string GetWindowsUserName()
        {
            int len = 1024;
            StringBuilder sb = new StringBuilder( len );
            if ( GetUserNameA( sb, &len ) != 0 )
                return sb.ToString();
            else
                return "";
        }

        internal static void DisableCloseButton( IntPtr handle )
        {
            if ( Environment.OSVersion.Platform == PlatformID.Win32NT )
            {
                IntPtr menu = GetSystemMenu( handle, false );
                EnableMenuItem( menu, 0xF060, 0x00000002 ); //menu, SC_CLOSE, MF_BYCOMMAND|MF_GRAYED
            }         
        }
    }
}
