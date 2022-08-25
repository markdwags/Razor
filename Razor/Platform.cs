#region license
// Razor: An Ultima Online Assistant
// Copyright (c) 2022 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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
        
        /* List of mappings between:
         * Windows 'Virtual Key Codes' (https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes)
         * and
         * Unix X11 Keysyms (https://cgit.freedesktop.org/xorg/proto/x11proto/tree/keysymdef.h)
         *
         * Testing was done on Gentoo Linux with an Unicomp New Model M, standard ANSI 104-key layout. (https://www.pckeyboard.com/page/product/NEW_M)
         * The keyboard used may affect the results for some of the more ambiguous keysyms and keycodes. Hopefully this works for most folks.
         */
        static Dictionary<int, int> m_MapX11Key = new Dictionary<int, int>()
        {
            // Utility keys.
            // For many of these, Windows lists one keycode while unix has a separate keysym for each symbol on the key.
            // In such cases, the primary symbol is used for the mapping.
            {0x08, 0xFF08}, // Backspace
            {0x09, 0xFF09}, // Tab
            {0x0C, 0xFF0B}, // Clear
            {0x0D, 0xFF0D}, // Return
            {0x20, 0x0020}, // Space
            {0xDE, 0x0027}, // Single/Double quote key
            {0xDB, 0x005B}, // Bracket left
            {0xDD, 0x005D}, // Bracket right
            {0xBC, 0x002C}, // Comma / Less than (<)
            {0xBE, 0x002E}, // Period / Greater than (>)
            {0xDC, 0x005C}, // Backslash
            {0xBB, 0x003D}, // Addition / Equals
            {0xBF, 0x002F}, // Forward slash
            {0xBD, 0x002D}, // Dash / Minus
            {0xC0, 0x0060}, // Grave / Tilde
            {0xBA, 0x003B}, // Semi-Colon / Colon
            {0x13, 0xFF13}, // Pause
            {0x1B, 0xFF1B}, // Escape
            {0x26, 0xFF52}, // Up arrow
            {0x28, 0xFF54}, // Down arrow
            {0x25, 0xFF51}, // Left arrow
            {0x27, 0xFF53}, // Right arrow
            {0x2D, 0xFF63}, // Insert
            {0x2E, 0xFFFF}, // Delete
            {0x24, 0xFF50}, // Home
            {0x23, 0xFF57}, // End
            {0x21, 0xFF55}, // Page Up
            {0x22, 0xFF56}, // Page Down
            {0x14, 0xFFE5}, // Caps Lock
            {0x91, 0xFF14}, // Scroll Lock
            // Same physical key as scroll lock, unlikely to be needed for UO
            //{0x91, 0xFF15}, // SysRq

            // Modifier keys
            // Unix has no combined keysym for these, handled separately in GetAsyncKeyState() below.
            //{0x10, 0xFF??}, // Shift
            //{0x11, 0xFF??}, // Control
            //{0x12, 0xFF??}, // Alt
            {0xA0, 0xFFE1}, // LSHIFT
            {0xA1, 0xFFE2}, // RSHIFT
            {0xA2, 0xFFE3}, // LCONTROL
            {0xA3, 0xFFE4}, // RCONTROL
            {0xA4, 0xFFE9}, // LALT
            {0xA5, 0xFFEA}, // RALT
            // These are pretty ambiguous and I don't have a windows machine handy to verify them.
            //{0x5B, 0x00EB}, // Super L / Windows key L
            //{0x5C, 0x00EC}, // Super R / Windows key R
            //{0x5D, 0x0067}, // Menu

            // Numeric Keypad
            {0x90, 0xFFBE}, // Num lock
            {0x60, 0xFFB0}, // Numpad0
            {0x61, 0xFFB1},
            {0x62, 0xFFB2},
            {0x63, 0xFFB3},
            {0x64, 0xFFB4},
            {0x65, 0xFFB5},
            {0x66, 0xFFB6},
            {0x67, 0xFFB7},
            {0x68, 0xFFB8},
            {0x69, 0xFFB0}, // Numpad9
            {0x6F, 0xFFAF}, // Divide
            {0x6A, 0xFFAA}, // Multiply
            {0x6D, 0xFFAD}, // Subtract
            {0x6B, 0xFFAB}, // Add
            {0x6C, 0xFFAE}, // Decimal
            // Windows has no separate keycode for these, so ignore them for now.
            // Should probably be handled similar to how Shift/Control/Alt are, below.
            //{0x, 0xFF95}, // KP_Home
            //{0x, 0xFF97}, // KP_Up
            //{0x, 0xFF9A}, // KP_PageUp
            //{0x, 0xFF96}, // KP_Left
            //{0x, 0xFF98}, // KP_Right
            //{0x, 0xFF9C}, // KP_End
            //{0x, 0xFF99}, // KP_Down
            //{0x, 0xFF9B}, // KP_PageDown
            //{0x, 0xFF8D}, // KP_Enter

            // F1-F24
            {0x70, 0xFFBE},
            {0x71, 0xFFBF},
            {0x72, 0xFFC0},
            {0x73, 0xFFC1},
            {0x74, 0xFFC2},
            {0x75, 0xFFC3},
            {0x76, 0xFFC4},
            {0x77, 0xFFC5},
            {0x78, 0xFFC6},
            {0x79, 0xFFC7},
            {0x7A, 0xFFC8},
            {0x7B, 0xFFC9},
            {0x7C, 0xFFCA},
            {0x7D, 0xFFCB},
            {0x7E, 0xFFCC},
            {0x7F, 0xFFCD},
            {0x80, 0xFFCE},
            {0x81, 0xFFCF},
            {0x82, 0xFFD0},
            {0x83, 0xFFD1},
            {0x84, 0xFFD2},
            {0x85, 0xFFD3},
            {0x86, 0xFFD4},
            {0x87, 0xFFD5},

            // 0-9 map directly
            {0x30, 0x0030},
            {0x31, 0x0031},
            {0x32, 0x0032},
            {0x33, 0x0033},
            {0x34, 0x0034},
            {0x35, 0x0035},
            {0x36, 0x0036},
            {0x37, 0x0037},
            {0x38, 0x0038},
            {0x39, 0x0039},

            // Captial A-Z map directly.
            {0x41, 0x0041},
            {0x42, 0x0042},
            {0x43, 0x0043},
            {0x44, 0x0044},
            {0x45, 0x0045},
            {0x46, 0x0046},
            {0x47, 0x0047},
            {0x48, 0x0048},
            {0x49, 0x0049},
            {0x4A, 0x004A},
            {0x4B, 0x004B},
            {0x4C, 0x004C},
            {0x4D, 0x004D},
            {0x4E, 0x004E},
            {0x4F, 0x004F},
            {0x50, 0x0050},
            {0x51, 0x0051},
            {0x52, 0x0052},
            {0x53, 0x0053},
            {0x54, 0x0054},
            {0x55, 0x0055},
            {0x56, 0x0056},
            {0x57, 0x0057},
            {0x58, 0x0058},
            {0x59, 0x0059},
            {0x5A, 0x005A},

            // Windows doesn't have lowercase A-Z keycodes.
        };
        
        internal static int MapX11Key(int key)
        {
            int keycode;

            // If no mapping is found, pass the unmapped code back out.
            // Can't be any worse than the way it used to work.
            if (!m_MapX11Key.TryGetValue(key, out keycode))
                keycode = key;

            return keycode;
        }

        // A linux-version of WinUser.dll's GetAsyncKeyState()
        // Take the (Windows) Virtual Key Code, convert it to an X11 KeySym, then back to the X11 Keycode, then check if it's pressed.
        internal static ushort GetAsyncKeyState(int winkey)
        {
            try
            {
                int key = 0; int key2 = 0;
                bool pressed = false; bool pressed2 = false;

                // Convert Windows Virtual Key Code to an X11 Keysym.
                // Code elsewhere distills Left/Right modifier keys to Window's single keycode. Un-distill them here
                if (winkey == 0x10)      { key = 0xFFE1; key2 = 0xFFE2; }               // Any shift key
                else if (winkey == 0x11) { key = 0xFFE3; key2 = 0xFFE4; }               // Any ctrl key
                else if (winkey == 0x12) { key = 0xFFE9; key2 = 0xFFEA; }               // Any alt key
                else                     { key = LinuxPlatform.MapX11Key (winkey); }    // Any other key, map the keycode to a keysym

                // Get physical keyboard state - every key being pressed is listed in the byte array
                var szKey = new byte[32];
                int res = XQueryKeymap(Display, szKey);

                // Convert X11 Keysym to X11 Keycode
                int code = XKeysymToKeycode(Display, (int) key);

                // Check if keycode is included in the keyboard state.
                var r = szKey[code / 8];
                var s = (1 << (code % 8));
                pressed = Convert.ToBoolean(r & s);

                // Check the second modifier key if required
                if (key2 > 0)
                {
                    int code2 = XKeysymToKeycode(Display, (int) key2);
                    var r2 = szKey[code2 / 8];
                    var s2 = (1 << (code2 % 8));
                    pressed2 = Convert.ToBoolean(r2 & s2);
                }

                if (pressed || pressed2)
                    return (ushort) 0xFF00;
                else
                    return (ushort) 0;
            }
            catch
            {
                return (ushort) 0;
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
