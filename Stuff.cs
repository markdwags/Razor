//#define STUFF

using System;
using System.Text;

namespace Assistant
{
	public class FindData
	{
#if STUFF
#warning Extra special leet stuff is enabled.  Don't release this build.

		public static void Initialize()
		{
			Command.Register( "ResetFind", new CommandCallback( ResetFind ) );
			Command.Register( "Find", new CommandCallback( Find ) );
			Command.Register( "Walk", new CommandCallback( Walk ) );
			//Command.Register( "pwn", new CommandCallback( Pwnx0r ) );
			Command.Register( "VisRange", new CommandCallback( VisRange ) );
		}

		public static void VisRange( string[] args )
		{
			int range = 18;

			try
			{
				range = Convert.ToInt32( args[0] );
			}
			catch
			{
				range = 18;
			}

			World.Player.VisRange = range;
			ClientCommunication.SendToClient( new SetUpdateRange( range ) );
			World.Player.SendMessage( "Set VisRange to {0}", range );
		}

		public static void ResetFind( string[] args )
		{
			uint wParam = ((uint)ClientCommunication.UONetMessage.FindData)|0xFFFF0000;
			World.Player.SendMessage( MsgLevel.Force, "Clearing addr list." );
			ClientCommunication.PostMessage( ClientCommunication.FindUOWindow(), ClientCommunication.WM_UONETEVENT, (IntPtr)wParam, IntPtr.Zero );
		}

		public static void Walk( string[] args )
		{
			Point3D loc = new Point3D( Point3D.Zero );

			try
			{
				loc.X = Utility.ToInt32( args[0], 0 );
				loc.Y = Utility.ToInt32( args[1], 0 );
				loc.Z = Utility.ToInt32( args[2], 0 );
				
				ClientCommunication.SendToClient( new PathFindTo( loc ) );
				World.Player.SendMessage( "Going... {0}", loc );
			}
			catch
			{
			}
		}

		public static void Pwnx0r( string[] args )
		{
			int count = 0x7FFFFFFF;
			try
			{
				count = Utility.ToInt32( args[0], 0x7FFFFFFF );
			}
			catch
			{
			}

			World.Player.SendMessage( MsgLevel.Force, "Pwning... {0}", count );

			for(int i=0;i<count;i++)
				ClientCommunication.SendToServer( new ResyncReq() );

			World.Player.SendMessage( MsgLevel.Force, "Done." );
		}

		public static void Find( string[] args )
		{
			try
			{
				uint val = Convert.ToUInt32( args[0], 16 );
				int size = 4;

				try
				{
					size = Utility.ToInt32( args[1], 4 );
				}
				catch
				{
					size = 4;
				}

				World.Player.SendMessage( MsgLevel.Force, "Finding 0x{0:X8} ({1})...", val, size );

				ClientCommunication.PostMessage( ClientCommunication.FindUOWindow(), ClientCommunication.WM_UONETEVENT, (IntPtr)(((uint)ClientCommunication.UONetMessage.FindData)|(((uint)size)<<16)), (IntPtr)((int)val) );
			}
			catch ( Exception e )
			{
				World.Player.SendMessage( MsgLevel.Force, e.Message );
				World.Player.SendMessage( MsgLevel.Force, "Usage: Find <hex value> [size = 4]" );
			}
		}
#endif
		public static void Message( uint c, int a )
		{
			if ( World.Player != null )
			{
				if ( c == 0 )
					World.Player.SendMessage( MsgLevel.Force, "{0} Values found!", a );
				else
					World.Player.SendMessage( MsgLevel.Force, "{0} @ {1:X8}", c, a );
			}
		}
	}

#if STUFF
	public class Stuff
	{
		public static void Initialize()
		{
			Command.Register( "AutoTarget", new CommandCallback( AutoTargetCallback ) );
			//Command.Register( "pwn", new CommandCallback( Pwn ) );
			PacketHandler.RegisterServerToClientViewer( 0x77, new PacketViewerCallback( MobileMoving ) );	
			PacketHandler.RegisterServerToClientViewer( 0x78, new PacketViewerCallback( MobileIncoming ) );
		}

		private static void Pwn( string[] args )
		{
			if ( World.Player == null )
				return;

			int count = 1000;
			if ( args.Length > 0 )
				count = Utility.ToInt32( args[0], 1000 );
			Packet p = new Packet( 0x22, 3*count+3 );
			p.Write( (short)0 );
			for(int i=0;i<count;i++)
			{
				p.Write( (byte)0x22 );
				p.Write( (short)0 );
			}

			ClientCommunication.SendToServer( p );
		}

		private enum AutoTargType
		{
			none,
			blue, green, grey, grey2, orange, red, yellow
		}

		private static AutoTargType m_Type;

		private static void AutoTargetCallback( string[] args )
		{
			if ( args.Length <= 0 )
			{
				World.Player.SendMessage( MsgLevel.Force, "Syntax: -autotarget TYPE" );
				World.Player.SendMessage( MsgLevel.Force, "Where TYPE Can be: blue, red, grey, grey2, green, orange, yellow, or none" );
				World.Player.SendMessage( MsgLevel.Force, "Auto Targ Aq currently set to: {0}", m_Type );
				return;
			}

			try
			{
				m_Type = (AutoTargType)Enum.Parse( typeof( AutoTargType ), args[0], true );
			}
			catch
			{
				World.Player.SendMessage( MsgLevel.Force, "Invalid type specified." );
				return;
			}

			World.Player.SendMessage( MsgLevel.Force, "Auto Target Aq set to: {0}", m_Type );
		}

		private static void MobileMoving( PacketReader p, PacketHandlerEventArgs args )
		{
			Mobile m = World.FindMobile( p.ReadUInt32() );

			if ( m != null && m.Notoriety == (byte)m_Type && m_Type != AutoTargType.none )
			{
				Point3D oldPos = m.Position;
				Point3D newPos = new Point3D( p.ReadUInt16(), p.ReadUInt16(), p.ReadSByte() );

				int dist = Utility.Distance( World.Player.Position, newPos );
				int oldDist = Utility.Distance( World.Player.Position, oldPos );
				int range = 15;
				if ( Config.GetBool( "RangeCheckLT" ) )
					range = Config.GetInt( "LTRange" );

				if ( oldDist > dist && oldDist > range && dist <= range )
				{
					Targeting.SetLastTargetTo( m );
					World.Player.SendMessage( MsgLevel.Force, "New target acquired." );
				}
			}
		}

		private static void MobileIncoming( PacketReader p, PacketHandlerEventArgs args )
		{
			Mobile m = World.FindMobile( p.ReadUInt32() );

			if ( m != null && m.Notoriety == (byte)m_Type && m_Type != AutoTargType.none )
			{
				Targeting.SetLastTargetTo( m );
				World.Player.SendMessage( MsgLevel.Force, "New target acquired." );
			}
		}
	}
#endif // STUFFs
}
