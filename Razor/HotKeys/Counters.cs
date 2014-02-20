using System;
using Assistant;

namespace Assistant.HotKeys
{
	public class CounterHotKeys
	{
		public static void Initialize()
		{
			HotKey.Add( HKCategory.Misc, LocString.DispCounters, new HotKeyCallback( DispCounters ) );
			HotKey.Add( HKCategory.Misc, LocString.RecountCounters, new HotKeyCallback( Recount ) );
		}

		private static void Recount()
		{
			Counter.FullRecount();
		}

		private static void DispCounters()
		{
			for (int i=0;i<Counter.List.Count;i++)
			{
				Counter c = (Counter)Counter.List[i];

				if ( c.Enabled )
					World.Player.SendMessage( MsgLevel.Force, "{0}: {1}", c.Name, c.Amount );
			}
		}
	}
}

