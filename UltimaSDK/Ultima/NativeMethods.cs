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
		public static extern ClientProcessHandle OpenProcess(int desiredAccess, int inheritClientHandle, ClientProcessHandle processID);

		[DllImport("Kernel32")]
		public static extern int CloseHandle(ClientProcessHandle handle);

		[DllImport("Kernel32")]
		public static unsafe extern int ReadProcessMemory(ClientProcessHandle process, int baseAddress, void* buffer, int size, ref int op);

		[DllImport("Kernel32")]
		public static unsafe extern int WriteProcessMemory(ClientProcessHandle process, int baseAddress, void* buffer, int size, int nullMe);

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
			ushort y = (ushort)x;
			return (short)((y >> 8) | (y << 8));
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
