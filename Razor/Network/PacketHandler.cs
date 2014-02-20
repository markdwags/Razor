using System;
using System.Collections;

namespace Assistant
{
	public delegate void PacketViewerCallback( PacketReader p, PacketHandlerEventArgs args );
	public delegate void PacketFilterCallback( Packet p, PacketHandlerEventArgs args );

	public class PacketHandlerEventArgs
	{
		private bool m_Block;
		public bool Block
		{
			get{ return m_Block; }
			set{ m_Block = value; }
		}

		public PacketHandlerEventArgs()
		{
			Reinit();
		}

		public void Reinit()
		{
			m_Block = false;
		}
	}

	public class PacketHandler
	{
		private static Hashtable m_ClientViewers;
		private static Hashtable m_ServerViewers;

		private static Hashtable m_ClientFilters;
		private static Hashtable m_ServerFilters;

		static PacketHandler()
		{
			m_ClientViewers = new Hashtable();
			m_ServerViewers = new Hashtable();

			m_ClientFilters = new Hashtable();
			m_ServerFilters = new Hashtable();
		}

		internal static void RegisterClientToServerViewer( int packetID, PacketViewerCallback callback )
		{
			ArrayList list = (ArrayList)m_ClientViewers[packetID];
			if ( list == null )
				m_ClientViewers[packetID] = list = new ArrayList();
			list.Add( callback );
		}

		internal static void RegisterServerToClientViewer( int packetID, PacketViewerCallback callback )
		{
			ArrayList list = (ArrayList)m_ServerViewers[packetID];
			if ( list == null )
				m_ServerViewers[packetID] = list = new ArrayList();
			list.Add( callback );
		}

		internal static void RemoveClientToServerViewer( int packetID, PacketViewerCallback callback )
		{
			ArrayList list = (ArrayList)m_ClientViewers[packetID];
			if ( list != null )
				list.Remove( callback );
		}

		internal static void RemoveServerToClientViewer( int packetID, PacketViewerCallback callback )
		{
			ArrayList list = (ArrayList)m_ServerViewers[packetID];
			if ( list != null )
				list.Remove( callback );
		}

		internal static void RegisterClientToServerFilter( int packetID, PacketFilterCallback callback )
		{
			ArrayList list = (ArrayList)m_ClientFilters[packetID];
			if ( list == null )
				m_ClientFilters[packetID] = list = new ArrayList();
			list.Add( callback );
		}

		internal static void RegisterServerToClientFilter( int packetID, PacketFilterCallback callback )
		{
			ArrayList list = (ArrayList)m_ServerFilters[packetID];
			if ( list == null )
				m_ServerFilters[packetID] = list = new ArrayList();
			list.Add( callback );
		}

		internal static void RemoveClientToServerFilter( int packetID, PacketFilterCallback callback )
		{
			ArrayList list = (ArrayList)m_ClientFilters[packetID];
			if ( list != null )
				list.Remove( callback );
		}

		internal static void RemoveServerToClientFilter( int packetID, PacketFilterCallback callback )
		{
			ArrayList list = (ArrayList)m_ServerFilters[packetID];
			if ( list != null )
				list.Remove( callback );
		}

		public static bool OnServerPacket( int id, PacketReader pr, Packet p )
		{
			bool result = false;
			if ( pr != null )
			{
				ArrayList list = (ArrayList)m_ServerViewers[id];
				if ( list != null && list.Count > 0 )
					result = ProcessViewers( list, pr );
			}

			if ( p != null )
			{
				ArrayList list = (ArrayList)m_ServerFilters[id];
				if ( list != null && list.Count > 0 )
					result |= ProcessFilters( list, p );
			}

			return result;
		}

		public static bool OnClientPacket( int id, PacketReader pr, Packet p )
		{
			bool result = false;
			if ( pr != null )
			{
				ArrayList list = (ArrayList)m_ClientViewers[id];
				if ( list != null && list.Count > 0 )
					result = ProcessViewers( list, pr );
			}

			if ( p != null )
			{
				ArrayList list = (ArrayList)m_ClientFilters[id];
				if ( list != null && list.Count > 0 )
					result |= ProcessFilters( list, p );
			}

			return result;
		}

		public static bool HasClientViewer( int packetID )
		{
			ArrayList list = (ArrayList)m_ClientViewers[packetID];
			return list != null && list.Count > 0;
		}

		public static bool HasServerViewer( int packetID )
		{
			ArrayList list = (ArrayList)m_ServerViewers[packetID];
			return list != null && list.Count > 0;
		}

		public static bool HasClientFilter( int packetID )
		{
			ArrayList list = (ArrayList)m_ClientFilters[packetID];
			return ( list != null && list.Count > 0 ) || PacketPlayer.Recording || PacketPlayer.Playing;
		}

		public static bool HasServerFilter( int packetID )
		{
			ArrayList list = (ArrayList)m_ServerFilters[packetID];
			return ( list != null && list.Count > 0 ) || PacketPlayer.Recording;
		}

		private static PacketHandlerEventArgs m_Args = new PacketHandlerEventArgs();
		private static bool ProcessViewers( ArrayList list, PacketReader p )
		{
			m_Args.Reinit();

			if ( list != null )
			{
				for (int i=0;i<list.Count;i++)
				{
					p.MoveToData();

					try
					{
						((PacketViewerCallback)list[i])( p, m_Args );
					}
					catch ( Exception e )
					{
						Engine.LogCrash( e );
						new MessageDialog( "WARNING: Packet viewer exception!", true, e.ToString() ).Show();
					}
				}
			}

			return m_Args.Block;
		}

		private static bool ProcessFilters( ArrayList list, Packet p )
		{
			m_Args.Reinit();

			if ( list != null )
			{
				for (int i=0;i<list.Count;i++)
				{
					p.MoveToData();

					try
					{
						((PacketFilterCallback)list[i])( p, m_Args );
					}
					catch ( Exception e )
					{
						Engine.LogCrash( e );
						new MessageDialog( "WARNING: Packet filter exception!", true, e.ToString() ).Show();
					}
				}
			}

			return m_Args.Block;
		}
	}
}
