using System;
using System.Collections;

namespace Assistant
{
	public class MessageQueue
	{
		private class MsgInfo
		{
			public MsgInfo( Serial ser, ushort body, MessageType type, ushort hue, ushort font, string lang, string name )
			{
				Serial = ser; Body = body; Type = type; Hue = hue; Font = font; Lang = lang; Name = name;
			}

			public TimeSpan Delay;
			public DateTime NextSend;
			public int Count;
			
			//ser, body, type, hue, font, lang, name
			public Serial Serial;
			public ushort Body, Hue, Font;
			public MessageType Type;
			public string Lang, Name;
		}

		private class MessageTimer : Timer
		{
			public MessageTimer() : base( TimeSpan.FromSeconds( 0.1 ), TimeSpan.FromSeconds( 0.1 ) )
			{
			}

			protected override void OnTick()
			{
				if ( m_Table.Count <= 0 )
					return;
				
				ArrayList list = new ArrayList( m_Table );
				foreach ( DictionaryEntry de in list )
				{
					string txt = de.Key.ToString();
					MsgInfo msg = (MsgInfo)de.Value;

					if ( msg.NextSend <= DateTime.Now )
					{
						if ( msg.Count > 0 )
						{
							if ( msg.Lang == "A" )
								ClientCommunication.SendToClient( new AsciiMessage( msg.Serial, msg.Body, msg.Type, msg.Hue, msg.Font, msg.Name, msg.Count > 1 ? String.Format( "{0} [{1}]", txt, msg.Count ) : txt ) );
							else
								ClientCommunication.SendToClient( new UnicodeMessage( msg.Serial, msg.Body, msg.Type, msg.Hue, msg.Font, msg.Lang, msg.Name, msg.Count > 1 ? String.Format( "{0} [{1}]", txt, msg.Count ) : txt ) );
							msg.Count = 0;
							msg.NextSend = DateTime.Now + msg.Delay;
						}
						else
						{
							m_Table.Remove( de.Key );
						}
					}
				}
			}
		}

		private static Timer m_Timer = new MessageTimer();
		private static Hashtable m_Table = new Hashtable();

		static MessageQueue()
		{
			m_Timer.Start();
		}

		public static bool Enqueue( Serial ser, ushort body, MessageType type, ushort hue, ushort font, string lang, string name, string text )
		{
			MsgInfo m = m_Table[text] as MsgInfo;
			if ( m == null )
			{
				m_Table[text] = m = new MsgInfo( ser, body, type, hue, font, lang, name );

				m.Count = 0;

				m.Delay = TimeSpan.FromSeconds( (((int)(text.Length / 50))+1) * 3.5 );

				m.NextSend = DateTime.Now + m.Delay;

				return true;
			}
			else
			{
				m.Count++;

				return false;
			}
		}


	}
}
