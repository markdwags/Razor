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

using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Ultima
{
    public static class NativeMethods
    {
        [DllImport("User32")]
        public static extern int IsWindow(ClientWindowHandle window);

        [DllImport("User32")]
        public static extern int GetWindowThreadProcessId(ClientWindowHandle window, ref ClientProcessHandle processID);

        [DllImport("Kernel32")]
        public unsafe static extern int _lread(SafeFileHandle hFile, void* lpBuffer, int wBytes);

        [DllImport("Kernel32")]
        public static extern ClientProcessHandle OpenProcess(int desiredAccess, int inheritClientHandle,
            ClientProcessHandle processID);

        [DllImport("Kernel32")]
        public static extern int CloseHandle(ClientProcessHandle handle);

        [DllImport("Kernel32")]
        public static unsafe extern int ReadProcessMemory(ClientProcessHandle process, int baseAddress, void* buffer,
            int size, ref int op);

        [DllImport("Kernel32")]
        public static unsafe extern int WriteProcessMemory(ClientProcessHandle process, int baseAddress, void* buffer,
            int size, int nullMe);

        [DllImport("User32")]
        public static extern int SetForegroundWindow(ClientWindowHandle hWnd);

        [DllImport("User32")]
        public static extern int SendMessage(ClientWindowHandle hWnd, int wMsg, int wParam, int lParam);

        [DllImport("User32")]
        public static extern bool PostMessage(ClientWindowHandle hWnd, int wMsg, int wParam, int lParam);

        [DllImport("User32")]
        public static extern int OemKeyScan(int wOemChar);

        [DllImport("user32")]
        public static extern ClientWindowHandle FindWindowA(string lpClassName, string lpWindowName);

        /// <summary>
        /// Swaps from Big to LittleEndian and vise versa
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static short SwapEndian(short x)
        {
            ushort y = (ushort) x;
            return (short) ((y >> 8) | (y << 8));
        }

        private static byte[] m_StringBuffer;

        public unsafe static string ReadNameString(byte* buffer, int len)
        {
            if ((m_StringBuffer == null) || (m_StringBuffer.Length < len))
                m_StringBuffer = new byte[20];
            int count;
            for (count = 0; count < len && *buffer != 0; ++count)
                m_StringBuffer[count] = *buffer++;

            return System.Text.Encoding.Default.GetString(m_StringBuffer, 0, count);
        }

        public unsafe static string ReadNameString(byte[] buffer, int len)
        {
            int count;
            for (count = 0; count < 20 && buffer[count] != 0; ++count) ;
            return System.Text.Encoding.Default.GetString(buffer, 0, count);
        }
    }
}