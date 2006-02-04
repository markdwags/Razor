using System;

namespace Assistant
{
	public class BandageTimer
	{
		private static int m_Count;
		private static Timer m_Timer;
		
		static BandageTimer()
		{
			m_Timer = new InternalTimer();
			PacketHandler.RegisterServerToClientViewer( 0xC1, new PacketViewerCallback( OnLocalizedMessage ) );
		}

		private static void OnLocalizedMessage( PacketReader pvSrc, PacketHandlerEventArgs args )
		{
			if ( Running )
			{
				pvSrc.Seek( 1 + 2 + 4 + 2 + 1 + 2 + 2, System.IO.SeekOrigin.Begin );
				int num = pvSrc.ReadInt32(); // number

				if ( num == 500955 || ( num >= 500962 && num <= 500969 ) || ( num >= 503252 && num <= 503261 ) || num == 1010058 || num == 1010648 || num == 1010650 || num == 1060088 || num == 1060167 )
					Stop();
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
