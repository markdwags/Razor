using System;
using System.Runtime.InteropServices;

namespace Assistant
{
	public class MemoryHelperThinggie : Timer
	{
		private static TimeSpan Frequency = TimeSpan.FromMinutes( 2.5 );

		public static readonly MemoryHelperThinggie Instance = new MemoryHelperThinggie();

		public static void Initialize()
		{
			Instance.Start();
		}

		private MemoryHelperThinggie() : base( TimeSpan.Zero, Frequency )
		{
		}

		[System.Runtime.InteropServices.DllImport( "Kernel32" )]
		private static extern uint SetProcessWorkingSetSize( IntPtr hProc, int minSize, int maxSize );

		protected override void OnTick()
		{
			SetProcessWorkingSetSize( System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1 );
		}
	}
}
