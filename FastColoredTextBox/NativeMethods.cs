using System;
using System.Runtime.InteropServices;

namespace FastColoredTextBoxNS
{
    public static class NativeMethodWrapper {
        public static void GetNativeSystemInfo(ref Win32NativeMethods.SYSTEM_INFO sysInfo)
        {
             Win32NativeMethods.GetNativeSystemInfo(ref sysInfo);
             return;
        }

        public static void GetSystemInfo(ref Win32NativeMethods.SYSTEM_INFO sysInfo)
        {
            Win32NativeMethods.GetSystemInfo(ref sysInfo);
            return;        
        }

        public static void ShowCaret(IntPtr handle)
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                Win32NativeMethods.ShowCaret(handle);
        }

        public static void SetCaretPos(int carX, int carY)
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                Win32NativeMethods.SetCaretPos(carX,carY);        }

        public static void CreateCaret(IntPtr handle, int i, int carWidth, int caretHeight)
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                Win32NativeMethods.CreateCaret(handle,i,carWidth,caretHeight);
        }

        public static void HideCaret(IntPtr handle)
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                Win32NativeMethods.HideCaret(handle);
        }

        public static void SendMessage(IntPtr handle, int wmSetredraw, int i, int i1)
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                Win32NativeMethods.SendMessage(handle,wmSetredraw,i,i1);
        }

        public static IntPtr ImmGetContext(IntPtr handle)
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                return Win32NativeMethods.ImmGetContext(handle);
            return IntPtr.Zero;
        }

        public static void CloseClipboard()
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                Win32NativeMethods.CloseClipboard();        
        }

        public static void ImmAssociateContext(IntPtr handle, IntPtr mHImc)
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                Win32NativeMethods.ImmAssociateContext(handle,mHImc); 
        }
    }
    
    public static class Win32NativeMethods
    {
        [DllImport("User32.dll")]
        public static extern bool CreateCaret(IntPtr hWnd, int hBitmap, int nWidth, int nHeight);

        [DllImport("User32.dll")]
        public static extern bool SetCaretPos(int x, int y);

        [DllImport("User32.dll")]
        public static extern bool DestroyCaret();

        [DllImport("User32.dll")]
        public static extern bool ShowCaret(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern bool HideCaret(IntPtr hWnd);
        
        [DllImport("Imm32.dll")]
        public static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport("Imm32.dll")]
        public static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);
        
        [DllImport("user32.dll")]
        public static extern IntPtr GetOpenClipboardWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr CloseClipboard();
        
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        
        [DllImport("kernel32.dll")]
        public static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll")]
        public static extern void GetSystemInfo(ref SYSTEM_INFO lpSystemInfo);
        
        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_INFO
        {
            public ushort wProcessorArchitecture;
            public ushort wReserved;
            public uint dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public UIntPtr dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public ushort wProcessorLevel;
            public ushort wProcessorRevision;
        };
    }
}