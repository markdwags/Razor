using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Assistant
{
    internal static unsafe class NativeMethods
    {
        [DllImport( "Crypt.dll" )]
        internal static unsafe extern int InstallLibrary( IntPtr thisWnd, int procid, int features );
        [DllImport( "Crypt.dll" )]
        internal static unsafe extern void Shutdown( bool closeClient );
        [DllImport( "Crypt.dll" )]
        internal static unsafe extern IntPtr FindUOWindow();
        [DllImport( "Crypt.dll" )]
        internal static unsafe extern IntPtr GetSharedAddress();
        [DllImport( "Crypt.dll" )]
        internal static unsafe extern IntPtr GetCommMutex();
        [DllImport( "Crypt.dll" )]
        internal static unsafe extern uint TotalIn();
        [DllImport( "Crypt.dll" )]
        internal static unsafe extern uint TotalOut();
        [DllImport( "Crypt.dll" )]
        internal static unsafe extern void WaitForWindow( int pid );
        [DllImport( "Crypt.dll" )]
        internal static unsafe extern void SetDataPath( string path );
        [DllImport( "Crypt.dll" )]
        internal static unsafe extern void CalibratePosition( uint x, uint y, uint z, byte dir );
        [DllImport( "Crypt.dll" )]
        internal static unsafe extern void SetServer( uint ip, ushort port );
        [DllImport( "Crypt.dll" )]
        internal static unsafe extern string GetUOVersion();

        [DllImport( "WinUtil.dll" )]
        internal static unsafe extern IntPtr CaptureScreen( IntPtr handle, bool isFullScreen, string msgStr );
        [DllImport( "WinUtil.dll" )]
        internal static unsafe extern void BringToFront( IntPtr hWnd );
        [DllImport( "WinUtil.dll" )]
        internal static unsafe extern int HandleNegotiate( ulong word );
        [DllImport( "WinUtil.dll" )]
        internal static unsafe extern bool AllowBit( ulong bit );

        [DllImport( "Loader.dll" )]
        internal static unsafe extern uint Load( string exe, string dll, string func, void* dllData, int dataLen, out uint pid );

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
    }
}
