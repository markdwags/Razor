using System;

namespace Assistant
{
	public class BandageTimer
	{
		private static int m_Count;
		private static Timer m_Timer;

		private static int[] m_ClilocNums = new int[]
			{
				500955,
				500962,
				500963,
				500964,
				500965,
				500966,
				500967,
				500968,
				500969,
				503252,
				503253,
				503254,
				503255,
				503256,
				503257,
				503258,
				503259,
				503260,
				503261,
				1010058,
				1010648,
				1010650,
				1060088,
				1060167,
			};
		
		static BandageTimer()
		{
			m_Timer = new InternalTimer();
		}

		public static void OnLocalizedMessage( int num )
		{
			if ( Running )
			{
				if ( num == 500955 || ( num >= 500962 && num <= 500969 ) || ( num >= 503252 && num <= 503261 ) || num == 1010058 || num == 1010648 || num == 1010650 || num == 1060088 || num == 1060167 )
					Stop();
			}
		}

		public static void OnAsciiMessage( string msg )
		{
			if ( Running )
			{
				for (int i=0;i<m_ClilocNums.Length;i++)
				{
					if ( Language.GetCliloc( m_ClilocNums[i] ) == msg )
					{
						Stop();
						break;
					}
				}
			}
		}

		public static int Count
		{
			get
			{
				return m_Count;
			}
		}

		public static bool Running
		{
			get
			{
				return m_Timer.Running;
			}
		}

		public static void Start()
		{
			m_Count = 0;

			if ( m_Timer.Running )
				m_Timer.Stop();
			m_Timer.Start();
			ClientCommunication.RequestTitlebarUpdate();
		}

		public static void Stop()
		{
			m_Timer.Stop();
			ClientCommunication.RequestTitlebarUpdate();
		}

		private class InternalTimer : Timer
		{
			public InternalTimer() : base( TimeSpan.FromSeconds( 1 ), TimeSpan.FromSeconds( 1 ) )
			{
			}

			protected override void OnTick()
			{
				m_Count++;
				if ( m_Count > 30 )
					Stop();
				ClientCommunication.RequestTitlebarUpdate();
			}
		}
	}
}
