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
        internal static void BringToFront( IntPtr window )
        {

        }
    }
    internal static unsafe class Platform
    {
        internal static ushort GetAsyncKeyState( int key )
        {
            if ( Environment.OSVersion.Platform == PlatformID.Win32NT )
                return Win32Platform.GetAsyncKeyState( key );
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
