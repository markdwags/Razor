using System;
using System.Collections;

namespace Assistant
{
	public class World
	{
		private static Hashtable m_Items;
		private static Hashtable m_Mobiles;
		private static PlayerData m_Player;
		private static string m_ShardName, m_PlayerName, m_AccountName;
		private static Hashtable m_Servers;

		static World()
		{
			m_Servers = new Hashtable();
			m_Items = Hashtable.Synchronized( new Hashtable() );
			m_Mobiles = Hashtable.Synchronized( new Hashtable() );
			m_ShardName = "[None]";
		}

		internal static Hashtable Servers { get{ return m_Servers; } }
		internal static Hashtable Items { get{ return m_Items; } }
		internal static Hashtable Mobiles { get{ return m_Mobiles; } }

		internal static Item FindItem( Serial serial )
		{
			return m_Items[serial] as Item;
		}

		internal static Mobile FindMobile( Serial serial )
		{
			return m_Mobiles[serial] as Mobile;
		}

		internal static ArrayList MobilesInRange( int range )
		{
			ArrayList list = new ArrayList();

			if ( World.Player == null )
				return list;

			foreach ( Mobile m in World.Mobiles.Values )
			{
				if ( Utility.InRange( World.Player.Position, m.Position, World.Player.VisRange ) )
					list.Add( m );
			}

			return list;
		}

		internal static ArrayList MobilesInRange()
		{
			if ( Player == null )
				return MobilesInRange( 18 );
			else
				return MobilesInRange( Player.VisRange );
		}

		internal static void AddItem( Item item )
		{
			m_Items[item.Serial] = item;
		}

		internal static void AddMobile( Mobile mob )
		{
			m_Mobiles[mob.Serial] = mob;
		}

		internal static void RemoveMobile( Mobile mob )
		{
			m_Mobiles.Remove( mob.Serial );
		}

		internal static void RemoveItem( Item item )
		{
			m_Items.Remove( item.Serial );
		}

		internal static PlayerData Player
		{
			get{ return m_Player; }
			set{ m_Player = value; }
		}

		internal static string ShardName
		{
			get{ return m_ShardName; }
			set{ m_ShardName = value; }
		}

		internal static string OrigPlayerName
		{
			get { return m_PlayerName; }
			set { m_PlayerName = value; }
		}

		internal static string AccountName
		{
			get { return m_AccountName; }
			set { m_AccountName = value; }
		}
	}
}

