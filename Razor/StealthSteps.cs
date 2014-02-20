using System;

namespace Assistant
{
	public class StealthSteps
	{
		private static int m_Count;
		private static bool m_Hidden = false;

		public static int Count{get{return m_Count;}}
		public static bool Counting{ get{ return m_Hidden; } }
		
		public static void OnMove()
		{
			if ( m_Hidden && Config.GetBool( "CountStealthSteps" ) && World.Player != null )
			{
				m_Count++;
				World.Player.SendMessage( MsgLevel.Error, LocString.StealthSteps, m_Count );
				if ( m_Count > 30 )
					Unhide();
			}
		}

		public static void Hide()
		{
			m_Hidden = true;
			m_Count = 0;
			if ( Config.GetBool( "CountStealthSteps" ) && World.Player != null )
				World.Player.SendMessage( MsgLevel.Error, LocString.StealthStart );
		}

		public static void Unhide()
		{
			m_Hidden = false;
			m_Count = 0;
		}
	}
}
