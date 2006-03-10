using System;
using System.IO;
using System.Collections;
using System.Threading;

namespace Assistant
{
	public delegate void TimerCallback();
	public delegate void TimerCallbackState( object state );

	public abstract class Timer
	{
		private DateTime m_Next;
		private TimeSpan m_Delay;
		private TimeSpan m_Interval;
		private bool m_Running;
		private int m_Index, m_Count;

		public TimeSpan TimeUntilTick
		{
			get
			{
				return m_Running && m_Next > DateTime.Now ? m_Next - DateTime.Now : TimeSpan.Zero;
			}
		}

		public TimeSpan Delay
		{
			get
			{
				return m_Delay;
			}
			set
			{
				m_Delay = value;
			}
		}

		public TimeSpan Interval
		{
			get
			{
				return m_Interval;
			}
			set
			{
				m_Interval = value;
			}
		}

		public void HardSetInterval( TimeSpan newInterval )
		{
			if ( newInterval != m_Interval )
			{
				if ( m_Running )
					m_Next = m_Next - m_Interval + newInterval;

				m_Interval = newInterval;
			}
		}

		public bool Running
		{
			get
			{
				return m_Running;
			}
		}

		public class TimerThread
		{
			private static Queue m_AddQueue = Queue.Synchronized( new Queue() );
			private static Queue m_RemoveQueue = Queue.Synchronized( new Queue() );

			private static ArrayList m_Timers = new ArrayList();

			public static void AddTimer( Timer t )
			{
				m_AddQueue.Enqueue( t );
			}

			public static void RemoveTimer( Timer t )
			{
				m_RemoveQueue.Enqueue( t );
			}

			private static void ProcessAddQueue()
			{
				while ( m_AddQueue.Count != 0 )
				{
					Timer t = (Timer)m_AddQueue.Dequeue();
					t.m_Next = DateTime.Now + t.m_Delay;
					t.m_Index = 0;
					m_Timers.Add( t );
				}//while !empty
			}

			private static void ProcessRemoveQueue()
			{
				while ( m_RemoveQueue.Count != 0 )
				{
					Timer t = (Timer)m_RemoveQueue.Dequeue();
					m_Timers.Remove( t );
				}//while !empty
			}

			public static void TimerMain()
			{
				DateTime now;
				int j;
				try
				{
					while ( Engine.Running )
					{
						Thread.Sleep( 5 );
						
						ProcessAddQueue();
						ProcessRemoveQueue();

						for (j=0;j<m_Timers.Count;j++)
						{
							now = DateTime.Now;
							Timer t = (Timer) m_Timers[j];

							if ( !t.m_Queued && t.m_Running && now >= t.m_Next )
							{
								t.m_Queued = true;
								lock ( m_Queue )	
									m_Queue.Enqueue( t );
								
								if ( t.m_Count != 0 && (++t.m_Index >= t.m_Count) )
									t.Stop();
								else
									t.m_Next = now + t.m_Interval;
							}
						}//for timers.Count

					}//while (running)
				}
				catch
				{
				}
			}//TimerMain
		}

		private static Queue m_Queue = new Queue();
		private static int m_BreakCount = 20000;

		public static int BreakCount{ get{ return m_BreakCount; } set{ m_BreakCount = value; } }

		public static Queue Queue
		{
			get
			{
				return m_Queue;
			}
		}

		private bool m_Queued;

		public static void Slice()
		{
			lock ( m_Queue )
			{
				int index = 0;

				while ( index < m_BreakCount && m_Queue.Count != 0 )
				{
					Timer t = (Timer)m_Queue.Dequeue();

					t.OnTick();
					t.m_Queued = false;
					++index;
				}//while !empty
			}
		}

		public Timer( TimeSpan delay ) : this( delay, TimeSpan.Zero, 1 )
		{
		}

		public Timer( TimeSpan interval, int count ) : this( interval, interval, count )
		{
		}

		public Timer( TimeSpan delay, TimeSpan interval ) : this( delay, interval, 0 )
		{
		}

		public Timer( TimeSpan delay, TimeSpan interval, int count )
		{
			m_Delay = delay;
			m_Interval = interval;
			m_Count = count;
		}

		public void Start()
		{
			if ( !m_Running )
			{
				m_Running = true;
				TimerThread.AddTimer( this );
			}
		}

		public void Stop()
		{
			if ( m_Running )
			{
				m_Running = false;
				TimerThread.RemoveTimer( this );
			}
		}

		private class OneTimeTimer : Timer
		{
			private TimerCallback m_Call;

			public OneTimeTimer( TimeSpan d, TimerCallback call ) : base( d )
			{
				m_Call = call;
			}

			protected override void OnTick()
			{
				m_Call();
				Stop();
			}
		}

		private class OneTimeTimerState : Timer
		{
			private TimerCallbackState m_Call;
			private object m_State;

			public OneTimeTimerState( TimeSpan d, TimerCallbackState call, object state ) : base( d )
			{
				m_Call = call;
				m_State = state;
			}

			protected override void OnTick()
			{
				m_Call( m_State );
				Stop();
			}
		}

		public static Timer DelayedCallback( TimeSpan delay, TimerCallback call )
		{
			Timer t = new OneTimeTimer( delay, call );
			//t.Start();
			return t;
		}

		public static Timer DelayedCallbackState( TimeSpan delay, TimerCallbackState call, object state )
		{
			Timer t = new OneTimeTimerState( delay, call, state );
			//t.Start();
			return t;
		}

		protected abstract void OnTick();
	}
}
