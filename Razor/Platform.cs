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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Assistant
{
    internal static unsafe class Win32Platform
    {
        static Dictionary<int, int> m_KeyMap = new Dictionary<int, int>()
        {
            {1073741979, 3},
            {8, 8},
            {9, 9},
            {1073741980, 12},
            {13, 13},
            {1073741942, 165},
            {1073741896, 19},
            {1073741881, 20},
            {27, 27},
            {1073742081, 31},
            {32, 32},
            {1073741899, 33},
            {1073741902, 34},
            {1073741901, 35},
            {1073741898, 36},
            {1073741904, 37},
            {1073741906, 38},
            {1073741903, 39},
            {1073741905, 40},
            {1073741943, 41},
            {1073741940, 43},
            {1073741897, 45},
            {127, 46},
            {1073741941, 47},
            {97, 65},
            {98, 66},
            {99, 67},
            {100, 68},
            {101, 69},
            {102, 70},
            {103, 71},
            {104, 72},
            {105, 73},
            {106, 74},
            {107, 75},
            {108, 76},
            {109, 77},
            {110, 78},
            {111, 79},
            {112, 80},
            {113, 81},
            {114, 82},
            {115, 83},
            {116, 84},
            {117, 85},
            {118, 86},
            {119, 87},
            {120, 88},
            {121, 89},
            {122, 90},
            {1073742051, 91},
            {1073742055, 92},
            {1073741925, 93},
            {1073742106, 95},
            {1073741922, 96},
            {1073741913, 97},
            {1073741914, 98},
            {1073741915, 99},
            {1073741916, 100},
            {1073741917, 101},
            {1073741918, 102},
            {1073741919, 103},
            {1073741920, 104},
            {1073741921, 105},
            {1073741909, 106},
            {1073741911, 107},
            {1073741910, 109},
            {1073741923, 110},
            {1073741908, 111},
            {1073741882, 112},
            {1073741883, 113},
            {1073741884, 114},
            {1073741885, 115},
            {1073741886, 116},
            {1073741887, 117},
            {1073741888, 118},
            {1073741889, 119},
            {1073741890, 120},
            {1073741891, 121},
            {1073741892, 122},
            {1073741893, 123},
            {1073741928, 124},
            {1073741929, 125},
            {1073741930, 126},
            {1073741931, 127},
            {1073741932, 128},
            {1073741933, 129},
            {1073741934, 130},
            {1073741935, 131},
            {1073741936, 132},
            {1073741937, 133},
            {1073741938, 134},
            {1073741939, 135},
            {1073741907, 144},
            {1073741895, 145},
            {1073742086, 173},
            {1073741953, 174},
            {1073741952, 175},
            {1073742082, 176},
            {1073742083, 177},
            {1073742084, 178},
            {1073742085, 179},
            {1073742089, 180},
            {1073742087, 181},
            {48, 48},
            {49, 49},
            {50, 50},
            {51, 51},
            {52, 52},
            {53, 53},
            {54, 54},
            {55, 55},
            {56, 56},
            {57, 57},
        };

        internal static int MapKey(int key)
        {
            int keycode;

            if (!m_KeyMap.TryGetValue(key, out keycode))
            {
                keycode = 0;
            }

            return keycode;
        }

        static Dictionary<int, int> m_KeyModMap = new Dictionary<int, int>()
        {
            {3, 512},
            {192, 1024},
            {195, 1536},
            {768, 2048},
            {771, 2560},
            {960, 3072},
            {963, 3584},
        };

        internal static int MapKeyMod(int key)
        {
            int keycode;

            if (!m_KeyModMap.TryGetValue(key, out keycode))
            {
                keycode = 0;
            }

            return keycode;
        }

        [DllImport("Platform.dll")]
        internal static unsafe extern IntPtr CaptureScreen(IntPtr handle, bool isFullScreen, string msgStr);

        [DllImport("Platform.dll")]
        internal static unsafe extern void BringToFront(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern ushort GetAsyncKeyState(int key);

        [DllImport("user32.dll")]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);
    }

    internal static unsafe class LinuxPlatform
    {
        [DllImport("libX11")]
        private static extern IntPtr XOpenDisplay(IntPtr display);

        [DllImport("libX11")]
        private static extern IntPtr XCloseDisplay(IntPtr display);

        [DllImport("libX11")]
        private static extern int XRaiseWindow(IntPtr display, IntPtr window);

        [DllImport("libX11")]
        private static extern int XGetInputFocus(IntPtr display, IntPtr window, IntPtr focus_return);

        [DllImport("libX11")]
        private static extern int XQueryKeymap(IntPtr display, byte[] keys);

        [DllImport("libX11")]
        private static extern int XKeysymToKeycode(IntPtr display, int key);

        private static IntPtr m_Display;

        private static IntPtr Display
        {
            get
            {
                if (m_Display == IntPtr.Zero)
                    m_Display = XOpenDisplay(IntPtr.Zero);
                return m_Display;
            }
        }

        internal static void BringToFront(IntPtr window)
        {
            XRaiseWindow(Display, window);
        }

        internal static ushort GetAsyncKeyState(int key)
        {
            try
            {
                var szKey = new byte[32];
                int res = XQueryKeymap(Display, szKey);
                //foreach(var xx in szKey)
                //Console.WriteLine(xx + "-");
                int code = XKeysymToKeycode(Display, (int) key);
                bool pressed = (szKey[code >> 3] & (1 << (code & 7))) == 0;
                var r = szKey[code / 8];
                var s = (1 << (code % 8));
                var x = r & s;
                return r == s ? (ushort) 0xFF00 : (ushort) 0;
            }
            catch
            {
                return 0;
            }
        }

        internal static bool SetForegroundWindow(IntPtr hWnd)
        {
            XRaiseWindow(Display, hWnd);
            return true;
        }
    }

    internal static unsafe class Platform
    {
        internal static ushort GetAsyncKeyState(int key)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                return Win32Platform.GetAsyncKeyState(key);
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
                return LinuxPlatform.GetAsyncKeyState(key);
            else
                return 0;
        }

        internal static IntPtr CaptureScreen(IntPtr handle, bool isFullScreen, string msgStr)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                return Win32Platform.CaptureScreen(handle, isFullScreen, msgStr);
            else
                return IntPtr.Zero;
        }

        internal static void BringToFront(IntPtr window)
        {
            try
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    Win32Platform.BringToFront(window);
                else
                    LinuxPlatform.BringToFront(window);
            }
            catch
            {
            }
        }

        internal static bool SetForegroundWindow(IntPtr hWnd)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                return Win32Platform.SetForegroundWindow(hWnd);
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
                return LinuxPlatform.SetForegroundWindow(hWnd);
            else
                return false;
        }

        [DllImport("User32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr wnd, bool reset);

        [DllImport("User32.dll")]
        private static extern IntPtr EnableMenuItem(IntPtr menu, uint item, uint options);


        [DllImport("msvcrt.dll")]
        internal static unsafe extern void memcpy(void* to, void* from, int len);

        [DllImport("user32.dll")]
        internal static extern uint PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);


        [DllImport("kernel32.dll")]
        internal static extern uint GlobalGetAtomName(ushort atom, StringBuilder buff, int bufLen);

        [DllImport("Advapi32.dll")]
        internal static extern int GetUserNameA(StringBuilder buff, int* len);

        [DllImport("user32.dll")]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);


        public static string GetWindowsUserName()
        {
            int len = 1024;
            StringBuilder sb = new StringBuilder(len);
            if (GetUserNameA(sb, &len) != 0)
                return sb.ToString();
            else
                return "";
        }

        internal static void DisableCloseButton(IntPtr handle)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                IntPtr menu = GetSystemMenu(handle, false);
                EnableMenuItem(menu, 0xF060, 0x00000002); //menu, SC_CLOSE, MF_BYCOMMAND|MF_GRAYED
            }
        }
    }
}