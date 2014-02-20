using System;
using System.IO;
using System.Collections;
using System.Threading;

namespace Assistant
{
	public class MinHeap
	{
		private ArrayList m_List;
		private int m_Size;

		public MinHeap() : this( 1 )
		{
		}

		public MinHeap( int capacity ) 
		{
			m_List = new ArrayList( capacity + 1 );
			m_Size = 0;
			m_List.Add( null ); // 0th index is never used, always null
		}

		public MinHeap( ICollection c ) : this( c.Count )
		{
			foreach ( IComparable o in c )
				m_List.Add( o );
			m_Size = c.Count;
			Heapify();
		}

		public void Heapify()
		{
			for (int i = m_Size / 2; i > 0; i--)
				PercolateDown( i );
		}

		private void PercolateDown( int hole )
		{
			IComparable tmp = (IComparable)m_List[hole];
			int child;

			for ( ; hole * 2 <= m_Size; hole = child )
			{
				child = hole * 2;
				if ( child != m_Size && ((IComparable)m_List[child+1]).CompareTo( m_List[child] ) < 0 )
					child++;

				if ( tmp.CompareTo( m_List[child] ) >= 0 )
					m_List[hole] = m_List[child];
				else
					break;
			}

			m_List[hole] = tmp;
		}

		public IComparable Peek()
		{
			return m_List[1] as IComparable;
		}

		public IComparable Pop()
		{
			IComparable top = Peek();

			m_List[1] = m_List[m_Size--];
			PercolateDown( 1 );

			return top;
		}

		public void Remove( IComparable o )
		{
			for ( int i = 1 ; i <= m_Size; i++ )
			{
				if ( m_List[i] == o )
				{
					m_List[i] = m_List[m_Size--];
					PercolateDown( i );
					// TODO: Do we ever need to shrink?
					return;
				}
			}
		}
		
		public void Clear()
		{
			int capacity = m_List.Count / 2;
			if ( capacity < 2 )
				capacity = 2;
			m_Size = 0;
			m_List = new ArrayList( capacity );
			m_List.Add( null );
		}

		public void Add( IComparable o )
		{
			// PercolateUp
			int hole = ++m_Size;

			// Grow the list if needed
			while ( m_List.Count <= m_Size )
				m_List.Add( null );

			for( ; hole > 1 && o.CompareTo( m_List[ hole / 2 ] ) < 0; hole /= 2 )
				m_List[ hole ] = m_List[ hole / 2 ];
			m_List[ hole ] = o;
		}

		public void AddMultiple( ICollection col )
		{
			if ( col != null && col.Count > 0 )
			{
				foreach ( object o in col )
				{
					int hole = ++m_Size;

					// Grow the list as needed
					while ( m_List.Count <= m_Size )
						m_List.Add( null );

					m_List[hole] = o;
				}

				Heapify();
			}
		}
				
		public int Count { get { return m_Size; } }

		public bool IsEmpty { get { return Count <= 0; } }

		public ArrayList GetRawList()
		{
			ArrayList copy = new ArrayList( m_Size );
			for (int i = 1; i <= m_Size; i++)
				copy.Add( m_List[i] );
			return copy;
		}
	}

	public delegate void TimerCallback();
	public delegate void TimerCallbackState( object state );

	public abstract class Timer : IComparable
	{
		private DateTime m_Next;
		private TimeSpan m_Delay;
		private TimeSpan m_Interval;
		private bool m_Running;
		private int m_Index, m_Count;

		protected abstract void OnTick();

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
				m_Index = 0;
				m_Next = DateTime.Now + m_Delay;
				m_Running = true;
				m_Heap.Add( this );
				ChangedNextTick( true );
			}
		}

		public void Stop()
		{
			if ( m_Running )
			{
				m_Running = false;
				m_Heap.Remove( this );
				//ChangedNextTick();
			}
		}

		public int CompareTo(object obj)
		{
			if ( obj is Timer )
				return this.TimeUntilTick.CompareTo( ((Timer)obj).TimeUntilTick );
			else
				return -1;
		}

		public TimeSpan TimeUntilTick
		{
			get { return m_Running ? m_Next - DateTime.Now : TimeSpan.MaxValue; }
		}
		
		public bool Running { get { return m_Running; } }

		public TimeSpan Delay
		{
			get { return m_Delay; }
			set	{ m_Delay = value; }
		}

		public TimeSpan Interval
		{
			get { return m_Interval; }
			set { m_Interval = value; }
		}

		private static MinHeap m_Heap = new MinHeap();
		private static System.Windows.Forms.Timer m_Ctrl;
		
		public static System.Windows.Forms.Timer Control
		{
			get { return m_Ctrl; }
			set 
			{
				if ( m_Ctrl != value )
				{
					if ( m_Ctrl != null )
						m_Ctrl.Stop();
					m_Ctrl = value;
					ChangedNextTick();
				}
			}
		}

		private static void ChangedNextTick()
		{
			ChangedNextTick( false );
		}

		private static void ChangedNextTick( bool allowImmediate )
		{
			if ( m_Ctrl == null )
				return;

			m_Ctrl.Stop();

			if ( !m_Heap.IsEmpty )
			{
				int interval = (int)Math.Round( ((Timer)m_Heap.Peek()).TimeUntilTick.TotalMilliseconds );
				if ( allowImmediate && interval <= 0 )
				{
					Slice();
				}
				else
				{
					if ( interval <= 0 )
						interval = 1;
				
					m_Ctrl.Interval = interval;
					m_Ctrl.Start();
				}
			}
		}

		public static void Slice()
		{
			int breakCount = 100;
			ArrayList readd = new ArrayList();

			while ( !m_Heap.IsEmpty && ((Timer)m_Heap.Peek()).TimeUntilTick < TimeSpan.Zero )
			{
				if ( breakCount-- <= 0 )
					break;

				Timer t = (Timer)m_Heap.Pop();

				if ( t != null && t.Running )
				{
					t.OnTick();

					if ( t.Running && ( t.m_Count == 0 || (++t.m_Index) < t.m_Count ) )
					{
						t.m_Next = DateTime.Now + t.m_Interval;
						readd.Add( t );
					}
					else
					{
						t.Stop();
					}
				}
			}

			m_Heap.AddMultiple( readd );

			ChangedNextTick();
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
			}
		}

		public static Timer DelayedCallback( TimeSpan delay, TimerCallback call )
		{
			return new OneTimeTimer( delay, call );
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
			}
		}

		public static Timer DelayedCallbackState( TimeSpan delay, TimerCallbackState call, object state )
		{
			return new OneTimeTimerState( delay, call, state );
		}
	}
}
