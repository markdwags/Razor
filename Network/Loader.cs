using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Assistant
{
	public class Loader
	{
		public enum ERROR_TYPE : int
		{
			SUCCESS = 0,
			NO_OPEN_EXE,
			NO_READ_EXE_DATA,

			NO_RUN_EXE,
			NO_ALLOC_MEM,

			NO_WRITE,
			NO_VPROTECT,
			NO_READ,
		};

		[DllImport( "Loader.dll" )]
		public static unsafe extern ERROR_TYPE Load( string exe, string dll, string funcName, ref DLLParameters data, int dataSize, out int pid );

		[Flags]
		public enum DLLFlags  : uint
		{
			None			= 0x00000000,

			OSICryptEnabled = 0x00000001,
			SmartCPU		= 0x00000002,
			Trees			= 0x00000004,
		};

		[StructLayout( LayoutKind.Sequential )]
		public struct DLLParameters
		{
			public DLLFlags Flags;
		}
	}
}