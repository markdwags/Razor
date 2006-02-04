using System;
using System.Runtime.InteropServices;

namespace Assistant
{
	public unsafe class UOGLite2
	{
		public enum ClientType : uint
		{
			Regular = 0,
			ThirdDawn = 1
		}

		[DllImport( "uog.dll", EntryPoint="UOG_Client_Launch" )]//, ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
		public static unsafe extern int Launch( int client );

		[DllImport( "uog.dll", EntryPoint="UOG_Client_Launch" )]//, ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
		public static unsafe extern int Launch( string clientPath );

		[DllImport( "uog.dll", EntryPoint="UOG_Client_Terminate" )]//, ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
		public static unsafe extern int Terminate();

		[DllImport( "uog.dll", EntryPoint="UOG_Client_Patch" )]//, ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
		public static unsafe extern int Patch();

		[DllImport( "uog.dll", EntryPoint="UOG_Client_Resume" )]//, ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
		public static unsafe extern int Resume();
	}
}

