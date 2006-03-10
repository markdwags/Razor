using System;
using System.IO;
using System.Text;
using System.Collections;
using Assistant.Macros;

namespace Assistant
{
	public class PacketHandlers
	{
		private static ArrayList m_IgnoreGumps = new ArrayList();
		public static ArrayList IgnoreGumps{ get{ return m_IgnoreGumps; } }

		public static void Initialize()
		{
			//Client -> Server handlers
			PacketHandler.RegisterClientToServerViewer( 0x00, new PacketViewerCallback( CreateCharacter ) );
			PacketHandler.RegisterClientToServerViewer( 0x02, new PacketViewerCallback( MovementRequest ) );
			PacketHandler.RegisterClientToServerViewer( 0x06, new PacketViewerCallback( ClientDoubleClick ) );
			PacketHandler.RegisterClientToServerViewer( 0x07, new PacketViewerCallback( LiftRequest ) );
			PacketHandler.RegisterClientToServerViewer( 0x08, new PacketViewerCallback( DropRequest ) );
			PacketHandler.RegisterClientToServerViewer( 0x09, new PacketViewerCallback( ClientSingleClick ) );
			PacketHandler.RegisterClientToServerViewer( 0x12, new PacketViewerCallback( ClientTextCommand ) );
			PacketHandler.RegisterClientToServerViewer( 0x13, new PacketViewerCallback( EquipRequest ) );
			PacketHandler.RegisterClientToServerViewer( 0x22, new PacketViewerCallback( ResyncRequest ) );
			PacketHandler.RegisterClientToServerViewer( 0x3A, new PacketViewerCallback( SetSkillLock ) );
			PacketHandler.RegisterClientToServerViewer( 0x5D, new PacketViewerCallback( PlayCharacter ) );
			PacketHandler.RegisterClientToServerViewer( 0x7D, new PacketViewerCallback( MenuResponse ) );
			PacketHandler.RegisterClientToServerFilter( 0x80, new PacketFilterCallback( ServerListLogin ) );
			PacketHandler.RegisterClientToServerFilter( 0x91, new PacketFilterCallback( GameLogin ) );
			PacketHandler.RegisterClientToServerViewer( 0x95, new PacketViewerCallback( HueResponse ) );
			PacketHandler.RegisterClientToServerViewer( 0xA0, new PacketViewerCallback( PlayServer ) );
			PacketHandler.RegisterClientToServerViewer( 0xB1, new PacketViewerCallback( ClientGumpResponse ) );
			PacketHandler.RegisterClientToServerFilter( 0xBF, new PacketFilterCallback( ExtendedClientCommand ) );
			PacketHandler.RegisterClientToServerViewer( 0xD7, new PacketViewerCallback( ClientEncodedPacket ) );
			
			//Server -> Client handlers
			PacketHandler.RegisterServerToClientViewer( 0x11, new PacketViewerCallback( MobileStatus ) );
			PacketHandler.RegisterServerToClientViewer( 0x17, new PacketViewerCallback( NewMobileStatus ) );
			PacketHandler.RegisterServerToClientViewer( 0x1A, new PacketViewerCallback( WorldItem ) );
			PacketHandler.RegisterServerToClientViewer( 0x1B, new PacketViewerCallback( LoginConfirm ) );
			PacketHandler.RegisterServerToClientFilter( 0x1C, new PacketFilterCallback( AsciiSpeech ) );
			PacketHandler.RegisterServerToClientViewer( 0x1D, new PacketViewerCallback( RemoveObject ) );
			PacketHandler.RegisterServerToClientFilter( 0x20, new PacketFilterCallback( MobileUpdate ) );
			PacketHandler.RegisterServerToClientViewer( 0x21, new PacketViewerCallback( MovementRej ) );
			PacketHandler.RegisterServerToClientViewer( 0x22, new PacketViewerCallback( MovementAck ) );
			PacketHandler.RegisterServerToClientViewer( 0x24, new PacketViewerCallback( BeginContainerContent ) );
			PacketHandler.RegisterServerToClientFilter( 0x25, new PacketFilterCallback( ContainerContentUpdate ) );
			PacketHandler.RegisterServerToClientViewer( 0x27, new PacketViewerCallback( LiftReject ) );
			PacketHandler.RegisterServerToClientViewer( 0x2D, new PacketViewerCallback( MobileStatInfo ) );
			PacketHandler.RegisterServerToClientFilter( 0x2E, new PacketFilterCallback( EquipmentUpdate ) );
			PacketHandler.RegisterServerToClientViewer( 0x3A, new PacketViewerCallback( Skills ) );
			PacketHandler.RegisterServerToClientFilter( 0x3C, new PacketFilterCallback( ContainerContent ) );
			PacketHandler.RegisterServerToClientViewer( 0x4E, new PacketViewerCallback( PersonalLight ) );	
			PacketHandler.RegisterServerToClientViewer( 0x4F, new PacketViewerCallback( GlobalLight ) );
			PacketHandler.RegisterServerToClientViewer( 0x72, new PacketViewerCallback( ServerSetWarMode ) );
			PacketHandler.RegisterServerToClientViewer( 0x73, new PacketViewerCallback( PingResponse ) );
			PacketHandler.RegisterServerToClientViewer( 0x76, new PacketViewerCallback( ServerChange ) );
			PacketHandler.RegisterServerToClientFilter( 0x77, new PacketFilterCallback( MobileMoving ) );
			PacketHandler.RegisterServerToClientFilter( 0x78, new PacketFilterCallback( MobileIncoming ) );
			PacketHandler.RegisterServerToClientViewer( 0x7C, new PacketViewerCallback( SendMenu ) );
			PacketHandler.RegisterServerToClientFilter( 0x8C, new PacketFilterCallback( ServerAddress ) );
			PacketHandler.RegisterServerToClientViewer( 0x97, new PacketViewerCallback( MovementDemand ) );
			PacketHandler.RegisterServerToClientViewer( 0xA1, new PacketViewerCallback( HitsUpdate ) );		
			PacketHandler.RegisterServerToClientViewer( 0xA2, new PacketViewerCallback( ManaUpdate ) );	
			PacketHandler.RegisterServerToClientViewer( 0xA3, new PacketViewerCallback( StamUpdate ) );					
			PacketHandler.RegisterServerToClientViewer( 0xA8, new PacketViewerCallback( ServerList ) );
			PacketHandler.RegisterServerToClientViewer( 0xAF, new PacketViewerCallback( DeathAnimation ) );
			PacketHandler.RegisterServerToClientFilter( 0xAE, new PacketFilterCallback( UnicodeSpeech ) );
			PacketHandler.RegisterServerToClientViewer( 0xB0, new PacketViewerCallback( SendGump ) );
			PacketHandler.RegisterServerToClientViewer( 0xB9, new PacketViewerCallback( Features ) );
			PacketHandler.RegisterServerToClientViewer( 0xBC, new PacketViewerCallback( ChangeSeason ) );
			PacketHandler.RegisterServerToClientViewer( 0xBF, new PacketViewerCallback( ExtendedPacket ) );
			PacketHandler.RegisterServerToClientFilter( 0xC1, new PacketFilterCallback( OnLocalizedMessage ) );
			PacketHandler.RegisterServerToClientFilter( 0xC8, new PacketFilterCallback( SetUpdateRange ) );
			PacketHandler.RegisterServerToClientFilter( 0xCC, new PacketFilterCallback( OnLocalizedMessageAffix ) );
			PacketHandler.RegisterServerToClientViewer( 0xD6, new PacketViewerCallback( EncodedPacket ) );//0xD6 "encoded" packets
			PacketHandler.RegisterServerToClientViewer( 0xD8, new PacketViewerCallback( CustomHouseInfo ) );
			PacketHandler.RegisterServerToClientViewer( 0xDD, new PacketViewerCallback( CompressedGump ) );
		}

		private static void SetUpdateRange( Packet p, PacketHandlerEventArgs args )
		{
			if ( World.Player != null )
				World.Player.VisRange = p.ReadByte();
		}

		private static void EncodedPacket( PacketReader p, PacketHandlerEventArgs args )
		{
			ushort id = p.ReadUInt16();

			switch ( id )
			{
				case 1: // object property list
					//ObjectPropertyList.Read( p );
					break;
			}
		}

		private static void ClientSingleClick( PacketReader p, PacketHandlerEventArgs args )
		{
			// if you modify this, don't forget to modify the allnames hotkey
			if ( Config.GetBool( "LastTargTextFlags" ) )
			{
				Mobile m = World.FindMobile( p.ReadUInt32() );
				if ( m != null )
					Targeting.CheckTextFlags( m );
			}
		}

		private static void ClientDoubleClick( PacketReader p, PacketHandlerEventArgs args )
		{
			Serial ser = p.ReadUInt32();
			if ( Config.GetBool( "BlockDismount" ) && World.Player != null && ser == World.Player.Serial && World.Player.Warmode && World.Player.GetItemOnLayer( Layer.Mount ) != null )
			{ // mount layer = 0x19
				World.Player.SendMessage( LocString.DismountBlocked );
				args.Block = true;
				return;
			}

			if ( Config.GetBool( "QueueActions" ) )
				args.Block = !PlayerData.DoubleClick( ser, false );
			
			if ( Macros.MacroManager.AcceptActions )
			{
				ushort gfx = 0;
				if ( ser.IsItem )
				{
					Item i = World.FindItem( ser );
					if ( i != null )
						gfx = i.ItemID;
				}
				else 
				{
					Mobile m = World.FindMobile( ser );
					if ( m != null )
						gfx = m.Body;
				}
				
				if ( gfx != 0 )
					MacroManager.Action( new DoubleClickAction( ser, gfx ) );
			}
		}

		private static void DeathAnimation( PacketReader p, PacketHandlerEventArgs args )
		{
			Serial killed = p.ReadUInt32();
			if ( Config.GetBool( "AutoCap" ) )
			{
				Mobile m = World.FindMobile( killed );
				if ( m != null && m.Body >= 0x0190 && m.Body <= 0x0193 && Utility.Distance( World.Player.Position, m.Position ) <= 12 )
					ScreenCapManager.DeathCapture();
			}
		}

		private static void ExtendedClientCommand( Packet p, PacketHandlerEventArgs args )
		{
			ushort ext = p.ReadUInt16();
			switch ( ext )
			{
				case 0x15: // context menu response
				{
					UOEntity ent = null;
					Serial ser = p.ReadUInt32();
					ushort idx = p.ReadUInt16();

					if ( ser.IsMobile )
						ent = World.FindMobile( ser );
					else if ( ser.IsItem )
						ent = World.FindItem( ser );

					if ( ent != null && ent.ContextMenu != null && ent.ContextMenu.ContainsKey( idx ) )
					{
						ushort menu = (ushort)ent.ContextMenu[idx];

						if ( menu != 0 && MacroManager.AcceptActions )
							MacroManager.Action( new ContextMenuAction( ent, idx, menu ) );
					}
					break;
				}
				case 0x1C:// cast spell
				{
					Serial ser = Serial.MinusOne;
					if ( p.ReadUInt16() == 1 )
						ser = p.ReadUInt32();
					ushort sid = p.ReadUInt16();
					Spell s = Spell.Get( sid );
					if ( s != null )
					{
						s.OnCast( p );
						args.Block = true;

						if ( Macros.MacroManager.AcceptActions )
							MacroManager.Action( new ExtCastSpellAction( s, ser ) );
					}
					break;
				}
				case 0x24:
				{
					// for the cheatx0r part 2...  anything outside this range indicates some haxing, just hide it with 0x30s
					byte b = p.ReadByte();
					if ( b < 0x25 || b >= 0x5E+0x25 )
					{
						p.Seek( -1, SeekOrigin.Current );
						p.Write( (byte)0x30 );
					}
					//using ( StreamWriter w = new StreamWriter( "bf24.txt", true ) )
					//	w.WriteLine( "{0} : 0x{1:X2}", DateTime.Now.ToString( "HH:mm:ss.ffff" ), b );
					break;
				}
			}
		}

		private static void ClientTextCommand( PacketReader p, PacketHandlerEventArgs args )
		{
			int type = p.ReadByte();
			string command = p.ReadString();

			switch ( type )
			{
				case 0x24: // Use skill
				{
					int skillIndex;

					try{ skillIndex = Convert.ToInt32( command.Split( ' ' )[0] ); }
					catch{ break; }

					if ( World.Player != null )
						World.Player.LastSkill = skillIndex;

					if ( Macros.MacroManager.AcceptActions )
						MacroManager.Action( new UseSkillAction( skillIndex ) );

					if ( skillIndex == (int)SkillName.Stealth && !World.Player.Visible )
						StealthSteps.Hide();
					break;
				}
				case 0x27: // Cast spell from book
				{
					try
					{
						string[] split = command.Split( ' ' );

						if ( split.Length > 0 )
						{
							ushort spellID = Convert.ToUInt16( split[0] );
							Serial serial = Convert.ToUInt32( split.Length > 1 ? Utility.ToInt32( split[1], -1 ) : -1 );
							Spell s = Spell.Get( spellID );
							if ( s != null )
							{
								s.OnCast( p );
								args.Block = true;
								if ( Macros.MacroManager.AcceptActions )
									MacroManager.Action( new BookCastSpellAction( s, serial ) );
							}
						}
					}
					catch
					{
					}
					break;
				}
				case 0x56: // Cast spell from macro
				{
					try
					{
						ushort spellID = Convert.ToUInt16( command );
						Spell s = Spell.Get( spellID );
						if ( s != null )
						{
							s.OnCast( p );
							args.Block = true;
							if ( Macros.MacroManager.AcceptActions )
								MacroManager.Action( new MacroCastSpellAction( s ) );
						}
					}
					catch
					{
					}
					break;
				}
			}
		}

		public static DateTime PlayCharTime = DateTime.MinValue;

		private static void CreateCharacter( PacketReader p, PacketHandlerEventArgs args )
		{
			p.Seek( 1+4+4+1, SeekOrigin.Begin ); // skip begining crap
			World.OrigPlayerName = p.ReadStringSafe( 30 );

			PlayCharTime = DateTime.Now;

			if ( Engine.MainWindow != null )
				Engine.MainWindow.OnLogin();
		}

		private static void PlayCharacter( PacketReader p, PacketHandlerEventArgs args )
		{
			p.ReadUInt32(); //0xedededed
			World.OrigPlayerName = p.ReadStringSafe( 30 );

			PlayCharTime = DateTime.Now;

			if ( Engine.MainWindow != null )
				Engine.MainWindow.OnLogin();
		}

		private static void ServerList( PacketReader p, PacketHandlerEventArgs args )
		{
			p.ReadByte(); //unknown
			ushort numServers = p.ReadUInt16();

			for ( int i = 0; i < numServers; ++i )
			{
				ushort num = p.ReadUInt16();
				World.Servers[num] = p.ReadString( 32 );
				p.ReadByte(); // full %
				p.ReadSByte(); // time zone
				p.ReadUInt32(); // ip
			}
		}

		private static void PlayServer( PacketReader p, PacketHandlerEventArgs args )
		{
			ushort index = p.ReadUInt16();

			World.ShardName = World.Servers[index] as string;
			if ( World.ShardName == null )
				World.ShardName = "[Unknown]";
		}

		private static void LiftRequest( PacketReader p, PacketHandlerEventArgs args )
		{
			Serial serial = p.ReadUInt32();
			ushort amount = p.ReadUInt16();

			Item item = World.FindItem( serial );
			ushort iid = 0;
			if ( item != null )
			{
				iid = item.ItemID.Value;

				if ( Config.GetBool( "QueueActions" ) )
				{
					DragDropManager.Drag( item, amount, true );
					args.Block = true;
				}
			}

			if ( Macros.MacroManager.AcceptActions )
			{
				MacroManager.Action( new LiftAction( serial, amount, iid ) ); 
				//MacroManager.Action( new PauseAction( TimeSpan.FromMilliseconds( Config.GetInt( "ObjectDelay" ) ) ) );
			}
		}

		private static void LiftReject( PacketReader p, PacketHandlerEventArgs args )
		{
			/*
			if ( ActionQueue.FilterLiftReject() )
				args.Block = true;
			*/
			if ( !DragDropManager.LiftReject() )
				args.Block = true;
			//MacroManager.PlayError( MacroError.LiftRej );
		}

		private static void EquipRequest( PacketReader p, PacketHandlerEventArgs args )
		{
			Serial iser = p.ReadUInt32(); // item being dropped serial
			Layer layer = (Layer)p.ReadByte();
			Serial mser = p.ReadUInt32();

			Item item = World.FindItem( iser );

			if ( MacroManager.AcceptActions )
			{
				if ( layer == Layer.Invalid || layer > Layer.LastValid )
				{
					if ( item != null )
					{
						layer = item.Layer;
						if ( layer == Layer.Invalid || layer > Layer.LastValid )
							layer = (Layer)item.ItemID.ItemData.Quality;
					}
				}
			
				if ( layer > Layer.Invalid && layer <= Layer.LastUserValid )
					MacroManager.Action( new DropAction( mser, Point3D.Zero, layer ) );
			}

			if ( item == null )
				return;

			Mobile m = World.FindMobile( mser );
			if ( m == null )
				return;

			if ( Config.GetBool( "QueueActions" ) )
			{
				DragDropManager.Drop( item, m, layer );
				args.Block = true;
			}
		}

		private static void DropRequest( PacketReader p, PacketHandlerEventArgs args )
		{
			Serial iser = p.ReadUInt32();
			int x = p.ReadInt16();
			int y = p.ReadInt16();
			int z = p.ReadSByte();
			Point3D newPos = new Point3D( x, y, z );
			Serial dser = p.ReadUInt32();
			
			if ( Macros.MacroManager.AcceptActions )
				MacroManager.Action( new DropAction( dser, newPos ) );

			Item i = World.FindItem( iser );
			if ( i == null )
				return;

			Item dest = World.FindItem( dser );
			if ( dest != null && dest.IsContainer && World.Player != null && World.Player.Backpack != null && dest.IsChildOf( World.Player.Backpack ) )
                i.IsNew = true;

			if ( Config.GetBool( "QueueActions" ) )
			{
				DragDropManager.Drop( i, dser, newPos );
				args.Block = true;
			}
		}

		private static void MovementRej( PacketReader p, PacketHandlerEventArgs args )
		{
			if ( World.Player != null )
			{
				byte seq = p.ReadByte();
				int x = p.ReadUInt16();
				int y = p.ReadUInt16();
				Direction dir = (Direction)p.ReadByte();
				sbyte z = p.ReadSByte();

				if ( WalkAction.IsMacroWalk( seq ) )
					args.Block = true;
				World.Player.MoveRej( seq, dir, new Point3D( x, y, z ) );
			}
		}

		private static void MovementAck( PacketReader p, PacketHandlerEventArgs args )
		{
			if ( World.Player != null )
			{
				byte oldNoto = World.Player.Notoriety;

				byte seq = p.ReadByte();
				World.Player.Notoriety = p.ReadByte();

				if ( WalkAction.IsMacroWalk( seq ) )
					args.Block = true;

				args.Block |= !World.Player.MoveAck( seq );

				if ( oldNoto != World.Player.Notoriety && Config.GetBool( "ShowNotoHue" ) )
					ClientCommunication.RequestTitlebarUpdate();
			}
		}

		private static void MovementRequest( PacketReader p, PacketHandlerEventArgs args )
		{
			if ( World.Player != null )
			{
				Direction dir = (Direction)p.ReadByte();
				byte seq = p.ReadByte();
				
				World.Player.MoveReq( dir, seq );

				WalkAction.LastWalkTime = DateTime.Now;
				if ( MacroManager.AcceptActions )
					MacroManager.Action( new WalkAction( dir ) );
			}
		}

		private static void ContainerContentUpdate( Packet p, PacketHandlerEventArgs args )
		{
			// This function will ignore the item if the container item has not been sent to the client yet.
			// We can do this because we can't really count on getting all of the container info anyway.
			// (So we'd need to request the container be updated, so why bother with the extra stuff required to find the container once its been sent?)
			Serial serial = p.ReadUInt32();
			ushort itemid = p.ReadUInt16();
			itemid = (ushort)(itemid + p.ReadSByte()); // signed, itemID offset
			ushort amount = p.ReadUInt16();
			if ( amount == 0 )
				amount = 1;
			Point3D pos = new Point3D( p.ReadUInt16(), p.ReadUInt16(), 0 );
			Serial cser = p.ReadUInt32();
			ushort hue = p.ReadUInt16();

			Item i = World.FindItem( serial );
			if ( i == null  )
			{
				if ( !serial.IsItem )
					return;

				World.AddItem( i = new Item( serial ) );
				i.IsNew = i.AutoStack = true;
			}
			else
			{
				i.CancelRemove();
			}

			if ( !DragDropManager.EndHolding( serial ) )
				return;

			i.ItemID = itemid;
			i.Amount = amount;
			i.Position = pos;
			i.Hue = hue;

			if ( SearchExemptionAgent.Contains( i ) )
			{
				p.Seek( -2, System.IO.SeekOrigin.Current );
				p.Write( (short)Config.GetInt( "ExemptColor" ) );
			}

			i.Container = cser;
			if ( i.IsNew )
				Item.UpdateContainers();
			if ( !SearchExemptionAgent.IsExempt( i ) && i.IsChildOf( World.Player.Backpack ) )
				Counter.Count( i );
		}

		private static void BeginContainerContent( PacketReader p, PacketHandlerEventArgs args )
		{
			Serial ser = p.ReadUInt32();
			if ( !ser.IsItem )
				return;
			Item item = World.FindItem( ser );
			if ( item != null )
			{
				if ( m_IgnoreGumps.Contains( item ) )
				{
					m_IgnoreGumps.Remove( item );
					args.Block = true;
				}
			}
			else
			{
				World.AddItem( new Item( ser ) );
				Item.UpdateContainers();
			}
		}

		private static void ContainerContent( Packet p, PacketHandlerEventArgs args )
		{
			int count = p.ReadUInt16();

			for (int i=0;i<count;i++)
			{
				Serial serial = p.ReadUInt32();
				// serial is purposely not checked to be valid, sometimes buy lists dont have "valid" item serials (and we are okay with that).
				Item item = World.FindItem( serial );
				if ( item == null )
				{
					World.AddItem( item = new Item( serial ) );
					item.IsNew = true;
					item.AutoStack = false;
				}
				else
				{
					item.CancelRemove();
				}

				if ( !DragDropManager.EndHolding( serial ) )
					continue;

				item.ItemID = p.ReadUInt16();
				item.ItemID = (ushort)(item.ItemID + p.ReadSByte());// signed, itemID offset
				item.Amount = p.ReadUInt16();
				if ( item.Amount == 0 )
					item.Amount = 1;
				item.Position = new Point3D( p.ReadUInt16(), p.ReadUInt16(), 0 );
				Serial cont = p.ReadUInt32();
				item.Hue = p.ReadUInt16();
				if ( SearchExemptionAgent.Contains( item ) )
				{
					p.Seek( -2, System.IO.SeekOrigin.Current );
					p.Write( (short)Config.GetInt( "ExemptColor" ) );
				}

				item.Container = cont; // must be done after hue is set (for counters)
				if ( !SearchExemptionAgent.IsExempt( item ) && item.IsChildOf( World.Player.Backpack ) )
					Counter.Count( item );
			}
			Item.UpdateContainers();
		}

		private static void EquipmentUpdate( Packet p, PacketHandlerEventArgs args )
		{
			Serial serial = p.ReadUInt32();

			Item i = World.FindItem( serial );
			bool isNew = false;
			if ( i == null )
			{
				World.AddItem( i=new Item( serial ) );
				isNew = true;
				Item.UpdateContainers();
			}
			else
			{
				i.CancelRemove();
			}

			if ( !DragDropManager.EndHolding( serial ) )
				return;

			ushort iid = p.ReadUInt16();
			i.ItemID = (ushort)(iid + p.ReadSByte()); // signed, itemID offset
			i.Layer = (Layer)p.ReadByte();
			Serial ser = p.ReadUInt32();// cont must be set after hue (for counters)
			i.Hue = p.ReadUInt16();

			i.Container = ser;

			int ltHue = Config.GetInt( "LTHilight" );
			if ( ltHue != 0 && Targeting.IsLastTarget( i.Container as Mobile ) )
			{
				p.Seek( -2, SeekOrigin.Current );
				p.Write( (ushort)(ltHue&0x3FFF) );
			}

			if ( i.Layer == Layer.Backpack && isNew && Config.GetBool( "AutoSearch" ) && ser == World.Player.Serial )
			{
				m_IgnoreGumps.Add( i );
				PlayerData.DoubleClick( i );
			}
		}

		private static void SetSkillLock( PacketReader p, PacketHandlerEventArgs args )
		{
			int i = p.ReadUInt16();

			if ( i >= 0 && i < Skill.Count )
			{
				Skill skill = World.Player.Skills[i];

				skill.Lock = (LockType)p.ReadByte();
				Engine.MainWindow.UpdateSkill( skill );
			}
		}

		private static void Skills( PacketReader p, PacketHandlerEventArgs args )
		{
			if ( World.Player == null || World.Player.Skills == null || Engine.MainWindow == null )
				return;

			byte type = p.ReadByte();

			switch ( type )
			{
				case 0x02://list (with caps, 3.0.8 and up)
				{
					int i;
					while ( (i = p.ReadUInt16()) > 0 )
					{
						if ( i>0 && i <= Skill.Count )
						{
							Skill skill = World.Player.Skills[i-1];

							if ( skill == null )
								continue;

							skill.FixedValue = p.ReadUInt16();
							skill.FixedBase = p.ReadUInt16();
							skill.Lock = (LockType)p.ReadByte();
							skill.FixedCap = p.ReadUInt16();
							if ( !World.Player.SkillsSent )
								skill.Delta = 0;
							ClientCommunication.PostSkillUpdate( i-1, skill.FixedBase );
						}
						else
						{
							p.Seek( 7, SeekOrigin.Current );
						}
					}

					World.Player.SkillsSent = true;
					Engine.MainWindow.RedrawSkills();
					break;
				}

				case 0x00: // list (without caps, older clients)
				{
					int i;
					while ( (i = p.ReadUInt16()) > 0 )
					{
						if ( i>0 && i <= Skill.Count )
						{
							Skill skill = World.Player.Skills[i-1];

							if ( skill == null )
								continue;

							skill.FixedValue = p.ReadUInt16();
							skill.FixedBase = p.ReadUInt16();
							skill.Lock = (LockType)p.ReadByte();
							skill.FixedCap = 100;//p.ReadUInt16();
							if ( !World.Player.SkillsSent )
								skill.Delta = 0;
							
							ClientCommunication.PostSkillUpdate( i-1, skill.FixedBase );
						}
						else
						{
							p.Seek( 5, SeekOrigin.Current );
						}
					}

					World.Player.SkillsSent = true;
					Engine.MainWindow.RedrawSkills();
					break;
				}

				case 0xDF: //change (with cap, new clients)
				{
					int i = p.ReadUInt16();

					if ( i >= 0 && i < Skill.Count )
					{
						Skill skill = World.Player.Skills[i];

						if ( skill == null )
							break;

						ushort old = skill.FixedBase;
						skill.FixedValue = p.ReadUInt16();
						skill.FixedBase = p.ReadUInt16();
						skill.Lock = (LockType)p.ReadByte();
						skill.FixedCap = p.ReadUInt16();
						Engine.MainWindow.UpdateSkill( skill );
						
						if ( Config.GetBool( "DisplaySkillChanges" ) && skill.FixedBase != old )
							World.Player.SendMessage( MsgLevel.Force, LocString.SkillChanged, (SkillName)i, skill.Delta > 0 ? "+" : "", skill.Delta, skill.Value, skill.FixedBase - old > 0 ? "+" : "", ((double)( skill.FixedBase - old )) / 10.0 );
						ClientCommunication.PostSkillUpdate( i, skill.FixedBase );
					}
					break;
				}

				case 0xFF: //change (without cap, older clients)
				{
					int i = p.ReadUInt16();

					if ( i >= 0 && i < Skill.Count )
					{
						Skill skill = World.Player.Skills[i];

						if ( skill == null )
							break;
						
						ushort old = skill.FixedBase;
						skill.FixedValue = p.ReadUInt16();
						skill.FixedBase = p.ReadUInt16();
						skill.Lock = (LockType)p.ReadByte();
						skill.FixedCap = 100;
						Engine.MainWindow.UpdateSkill( skill );
						if ( Config.GetBool( "DisplaySkillChanges" ) && skill.FixedBase != old )
							World.Player.SendMessage( MsgLevel.Force, LocString.SkillChanged, (SkillName)i, skill.Delta > 0 ? "+" : "", skill.Delta, skill.Value, ((double)( skill.FixedBase - old )) / 10.0, skill.FixedBase - old > 0 ? "+" : "" );
						ClientCommunication.PostSkillUpdate( i, skill.FixedBase );
					}
					break;
				}
			}
		}

		private static void LoginConfirm( PacketReader p, PacketHandlerEventArgs args )
		{
			World.Items.Clear();
			World.Mobiles.Clear();

			Serial serial = p.ReadUInt32();

			PlayerData m = new PlayerData( serial );
			m.Name = World.OrigPlayerName;

			Mobile test = World.FindMobile( serial );
			if ( test != null )
				test.Remove();

			World.AddMobile( World.Player = m );
			Config.LoadProfileFor( World.Player );

			PlayerData.ExternalZ = false;

			p.ReadUInt32(); // always 0?
			m.Body = p.ReadUInt16();
			m.Position = new Point3D( p.ReadUInt16(), p.ReadUInt16(), p.ReadInt16() );
			m.Direction = (Direction)p.ReadByte();
			m.Resync();

			//ClientCommunication.SendToServer( new SkillsQuery( m ) );
			//ClientCommunication.SendToServer( new StatusQuery( m ) );
			
			ClientCommunication.RequestTitlebarUpdate();
			ClientCommunication.PostLogin( (int)serial.Value );
			Engine.MainWindow.UpdateTitle(); // update player name & shard name
			/*
			//the rest of the packet: (total length: 37)
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (int) -1 );

			m_Stream.Write( (short) 0 );
			m_Stream.Write( (short) 0 );
			m_Stream.Write( (short) (map==null?6144:map.Width) );
			m_Stream.Write( (short) (map==null?4096:map.Height) );

			Stream.Fill();
			*/

			ClientCommunication.BeginCalibratePosition();
		}
		
		private static void MobileMoving( Packet p, PacketHandlerEventArgs args )
		{
			Mobile m = World.FindMobile( p.ReadUInt32() );

			if ( m != null )
			{
				m.Body = p.ReadUInt16();
				m.Position = new Point3D( p.ReadUInt16(), p.ReadUInt16(), p.ReadSByte() );
				
				if ( !Utility.InRange( World.Player.Position, m.Position, World.Player.VisRange ) )
				{
					m.Remove();
					return;
				}

				Targeting.CheckLastTargetRange( m );

				m.Direction = (Direction)p.ReadByte();
				m.Hue = p.ReadUInt16();
				int ltHue = Config.GetInt( "LTHilight" );
				if ( ltHue != 0 && Targeting.IsLastTarget( m ) )
				{
					p.Seek( -2, SeekOrigin.Current );
					p.Write( (short)(ltHue|0x8000) );
				}

				bool wasPoisoned = m.Poisoned;
				m.ProcessPacketFlags( p.ReadByte() );
				byte oldNoto = m.Notoriety;
				m.Notoriety = p.ReadByte();

				if ( m == World.Player )
				{
					ClientCommunication.BeginCalibratePosition();

					if ( wasPoisoned != m.Poisoned || ( oldNoto != m.Notoriety && Config.GetBool( "ShowNotoHue" ) ) )
						ClientCommunication.RequestTitlebarUpdate();
				}
			}
		}

		private static void HitsUpdate( PacketReader p, PacketHandlerEventArgs args )
		{
			Mobile m = World.FindMobile( p.ReadUInt32() );

			if ( m != null )
			{
				m.HitsMax = p.ReadUInt16();
				m.Hits = p.ReadUInt16();

				if ( m == World.Player )
				{
					ClientCommunication.RequestTitlebarUpdate();
					ClientCommunication.PostHitsUpdate();
				}
			}
		}

		private static void StamUpdate( PacketReader p, PacketHandlerEventArgs args )
		{
			Mobile m = World.FindMobile( p.ReadUInt32() );

			if ( m != null && m == World.Player )
			{
				World.Player.StamMax = p.ReadUInt16();
				World.Player.Stam = p.ReadUInt16();

				ClientCommunication.RequestTitlebarUpdate();
				ClientCommunication.PostStamUpdate();
			}
		}

		private static void ManaUpdate( PacketReader p, PacketHandlerEventArgs args )
		{
			Mobile m = World.FindMobile( p.ReadUInt32() );

			if ( m != null && m == World.Player )
			{
				World.Player.ManaMax = p.ReadUInt16();
				World.Player.Mana = p.ReadUInt16();

				ClientCommunication.RequestTitlebarUpdate();
				ClientCommunication.PostManaUpdate();
			}
		}

		private static void MobileStatInfo( PacketReader pvSrc, PacketHandlerEventArgs args )
		{
			Serial serial = pvSrc.ReadUInt32();
			if ( World.Player == null || serial != World.Player.Serial )
				return;
			PlayerData p = World.Player;

			p.HitsMax = pvSrc.ReadUInt16();
			p.Hits = pvSrc.ReadUInt16();

			p.ManaMax = pvSrc.ReadUInt16();
			p.Mana = pvSrc.ReadUInt16();

			p.StamMax = pvSrc.ReadUInt16();
			p.Stam = pvSrc.ReadUInt16();

			ClientCommunication.RequestTitlebarUpdate();
			ClientCommunication.PostHitsUpdate();
			ClientCommunication.PostStamUpdate();
			ClientCommunication.PostManaUpdate();
		}

		private static void NewMobileStatus( PacketReader p, PacketHandlerEventArgs args )
		{
			Mobile m = World.FindMobile( (Serial)p.ReadUInt32() );

			if ( m == null )
				return;

			// 00 01 00 01
			p.ReadUInt16();
			p.ReadUInt16();

			byte poison = p.ReadByte();

			bool wasPoisoned = m.Poisoned;
			m.Poisoned = ( poison != 0 );

			if ( m == World.Player && wasPoisoned != m.Poisoned )// || ( oldNoto != m.Notoriety && Config.GetBool( "ShowNotoHue" ) ) )
				ClientCommunication.RequestTitlebarUpdate();
		}

		private static void MobileStatus( PacketReader p, PacketHandlerEventArgs args )
		{
			Serial serial = p.ReadUInt32();
			Mobile m = World.FindMobile( serial );
			if ( m == null )
				World.AddMobile( m = new Mobile( serial ) );

			m.Name = p.ReadString( 30 );

			m.Hits = p.ReadUInt16();
			m.HitsMax = p.ReadUInt16();

			p.ReadBoolean();//CanBeRenamed

			byte type = p.ReadByte();

			if ( m == World.Player && type != 0x00 )
			{
				PlayerData player = (PlayerData)m;

				player.Female = p.ReadBoolean();

				int oStr = player.Str, oDex = player.Dex, oInt = player.Int;

				player.Str = p.ReadUInt16();
				player.Dex = p.ReadUInt16();
				player.Int = p.ReadUInt16();

				if ( player.Str != oStr && oStr != 0 && Config.GetBool( "DisplaySkillChanges" ) )
					World.Player.SendMessage( MsgLevel.Force, LocString.StrChanged, player.Str - oStr > 0 ? "+" : "", player.Str - oStr, player.Str );

				if ( player.Dex != oDex && oDex != 0 && Config.GetBool( "DisplaySkillChanges" ) )
					World.Player.SendMessage( MsgLevel.Force, LocString.DexChanged, player.Dex - oDex > 0 ? "+" : "", player.Dex - oDex, player.Dex );

				if ( player.Int != oInt && oInt != 0 && Config.GetBool( "DisplaySkillChanges" ) )
					World.Player.SendMessage( MsgLevel.Force, LocString.IntChanged, player.Int - oInt > 0 ? "+" : "", player.Int - oInt, player.Int );

				player.Stam = p.ReadUInt16();
				player.StamMax = p.ReadUInt16();
				player.Mana = p.ReadUInt16();
				player.ManaMax = p.ReadUInt16();

				player.Gold = p.ReadUInt32();
				player.AR = p.ReadUInt16(); // ar / physical resist
				player.Weight = p.ReadUInt16();

				if ( type >= 0x03 )
				{
					if ( type > 0x04 )
					{
						player.MaxWeight = p.ReadUInt16();

						p.ReadByte(); // race?
					}

					player.StatCap = p.ReadUInt16();

					if ( type > 0x03 )
					{
						player.Followers = p.ReadByte();
						player.FollowersMax = p.ReadByte();

						player.FireResistance = p.ReadInt16();
						player.ColdResistance = p.ReadInt16();
						player.PoisonResistance = p.ReadInt16();
						player.EnergyResistance = p.ReadInt16();
						
						player.Luck = p.ReadInt16();
						
						player.DamageMin = p.ReadUInt16();
						player.DamageMax = p.ReadUInt16();

						player.Tithe = p.ReadInt32();
					}
				}

				ClientCommunication.RequestTitlebarUpdate();
				
				ClientCommunication.PostHitsUpdate();
				ClientCommunication.PostStamUpdate();
				ClientCommunication.PostManaUpdate();

				Engine.MainWindow.UpdateTitle(); // update player name
			}
		}
		
		private static void MobileUpdate( Packet p, PacketHandlerEventArgs args )
		{
			if ( World.Player == null )
				return;

			Serial serial = p.ReadUInt32();
			Mobile m = World.FindMobile( serial );
			if ( m == null )
				World.AddMobile( m = new Mobile( serial ) );

			bool wasHidden = !m.Visible;

			m.Body = (ushort)(p.ReadUInt16() + p.ReadSByte());
			m.Hue = p.ReadUInt16();
			int ltHue = Config.GetInt( "LTHilight" );
			if ( ltHue != 0 && Targeting.IsLastTarget( m ) )
			{
				p.Seek( -2, SeekOrigin.Current );
				p.Write( (ushort)(ltHue|0x8000) );
			}

			bool wasPoisoned = m.Poisoned;
			m.ProcessPacketFlags( p.ReadByte() );

			if ( m == World.Player )
			{
				ClientCommunication.BeginCalibratePosition();

				World.Player.Resync();

				if ( !wasHidden && !m.Visible )
				{
					if ( Config.GetBool( "AlwaysStealth" ) )
						StealthSteps.Hide();
				}
				else if ( wasHidden && m.Visible )
				{
					StealthSteps.Unhide();
				}

				if ( wasPoisoned != m.Poisoned )
					ClientCommunication.RequestTitlebarUpdate();
			}

			ushort x = p.ReadUInt16();
			ushort y = p.ReadUInt16();
			p.ReadUInt16(); //always 0?
			m.Direction = (Direction)p.ReadByte();
			m.Position = new Point3D( x, y, p.ReadSByte() );

			Item.UpdateContainers();
		}

		private static void MobileIncoming( Packet p, PacketHandlerEventArgs args )
		{
			if ( World.Player == null )
				return;

			Serial serial = p.ReadUInt32();
			ushort body =  p.ReadUInt16();
			Point3D position = new Point3D( p.ReadUInt16(), p.ReadUInt16(), p.ReadSByte() );

			if ( World.Player.Position != Point3D.Zero && !Utility.InRange( World.Player.Position, position, World.Player.VisRange ) )
				return;

			Mobile m = World.FindMobile( serial );
			if ( m == null )
				World.AddMobile( m = new Mobile( serial ) );

			bool wasHidden = !m.Visible;

			if ( m != World.Player && Config.GetBool( "ShowMobNames" ) )
				ClientCommunication.SendToServer( new SingleClick( m ) );
			if ( Config.GetBool( "LastTargTextFlags" ) )
				Targeting.CheckTextFlags( m );

			int ltHue = Config.GetInt( "LTHilight" );
			bool isLT;
			if ( ltHue != 0 )
				isLT = Targeting.IsLastTarget( m );
			else
				isLT = false;

			m.Body = body;
			if ( m != World.Player || World.Player.OutstandingMoveReqs == 0 )
				m.Position = position;
			m.Direction = (Direction)p.ReadByte();
			m.Hue = p.ReadUInt16();
			if ( isLT )
			{
				p.Seek( -2, SeekOrigin.Current );
				p.Write( (short)(ltHue|0x8000) );
			}

			bool wasPoisoned = m.Poisoned;
			m.ProcessPacketFlags( p.ReadByte() );
			byte oldNoto = m.Notoriety;
			m.Notoriety = p.ReadByte();
			
			if ( m == World.Player )
			{
				ClientCommunication.BeginCalibratePosition();

				if ( !wasHidden && !m.Visible )
				{
					if ( Config.GetBool( "AlwaysStealth" ) )
						StealthSteps.Hide();
				}
				else if ( wasHidden && m.Visible )
				{
					StealthSteps.Unhide();
				}

				if ( wasPoisoned != m.Poisoned || ( oldNoto != m.Notoriety && Config.GetBool( "ShowNotoHue" ) ) )
					ClientCommunication.RequestTitlebarUpdate();
			}

			while ( true ) 
			{
				serial = p.ReadUInt32();
				if ( !serial.IsItem )
					break;
				
				Item item = World.FindItem( serial );
				bool isNew = false;
				if ( item == null )
				{
					isNew = true;
					World.AddItem( item = new Item( serial ) );
				}

				if ( !DragDropManager.EndHolding( serial ) )
					continue;

				item.Container = m;

				ushort id = p.ReadUInt16();
				item.ItemID = (ushort)(id & 0x3FFF);
				item.Layer = (Layer)p.ReadByte();

				if ( (id & 0x8000) != 0 )
				{
					item.Hue = p.ReadUInt16();
					if ( isLT )
					{
						p.Seek( -2, SeekOrigin.Current );
						p.Write( (short)(ltHue&0x3FFF) );
					}
				}
				else
				{
					item.Hue = 0;
					if ( isLT )
						ClientCommunication.SendToClient( new EquipmentItem( item, (ushort)(ltHue&0x3FFF), m.Serial ) );
				}

				if ( item.Layer == Layer.Backpack && isNew && Config.GetBool( "AutoSearch" ) && m == World.Player && m != null )
				{
					m_IgnoreGumps.Add( item );
					PlayerData.DoubleClick( item );
				}
			}

			Item.UpdateContainers();
		}

		private static void RemoveObject( PacketReader p, PacketHandlerEventArgs args )
		{
			Serial serial = p.ReadUInt32();

			if ( serial.IsMobile )
			{
				Mobile m = World.FindMobile( serial );
				if ( m != null && m != World.Player )
					m.Remove();
			}
			else if ( serial.IsItem )
			{
				Item i = World.FindItem( serial );
				if ( i != null )
				{
					if ( DragDropManager.Holding == i )
					{
						Counter.SupressWarnings = true;
						i.Container = null;
						Counter.SupressWarnings = false;
					}
					else
					{
						i.RemoveRequest();
					}
				}
			}
		}

		private static void ServerChange( PacketReader p, PacketHandlerEventArgs args )
		{
			World.Player.Position = new Point3D( p.ReadUInt16(), p.ReadUInt16(), p.ReadInt16() );
		}

		private static void WorldItem( PacketReader p, PacketHandlerEventArgs args )
		{
			Item item;
			uint serial = p.ReadUInt32();
			item = World.FindItem( serial&0x7FFFFFFF );
			bool isNew = false;
			if ( item == null )
			{
				World.AddItem( item=new Item( serial&0x7FFFFFFF ) );
				isNew = true;
			}
			else
			{
				item.CancelRemove();
			}

			if ( !DragDropManager.EndHolding( serial ) )
				return;

			item.Container = null;
			Counter.Uncount( item );

			ushort itemID = p.ReadUInt16();
			item.ItemID = (ushort)(itemID&0x7FFF);

			if ( (serial & 0x80000000) != 0 )
				item.Amount = p.ReadUInt16();
			else
				item.Amount = 1;

			if ( (itemID & 0x8000) != 0 )
				item.ItemID = (ushort)(item.ItemID + p.ReadSByte());

			ushort x = p.ReadUInt16();
			ushort y = p.ReadUInt16();

			if ( (x & 0x8000) != 0 )
				item.Direction = p.ReadByte();
			else
				item.Direction = 0;

			short z = p.ReadSByte();

			item.Position = new Point3D( x&0x3FFF, y&0x3FFF, z );

			if ( ( y & 0x8000 ) != 0 )
				item.Hue = p.ReadUInt16();
			else
				item.Hue = 0;

			byte flags = 0;
			if ( ( y & 0x4000 ) != 0 )
				flags = p.ReadByte();

			item.ProcessPacketFlags( flags );
			
			if ( isNew && World.Player != null )
			{
				if ( item.ItemID == 0x2006 )// corpse itemid = 0x2006
				{
					if ( Config.GetBool( "ShowCorpseNames" ) )
						ClientCommunication.SendToServer( new SingleClick( item ) );
					if ( Config.GetBool( "AutoOpenCorpses" ) && Utility.InRange( item.Position, World.Player.Position, Config.GetInt( "CorpseRange" ) ) && World.Player != null && World.Player.Visible )
						PlayerData.DoubleClick( item ) ;
				}
				else if ( item.IsMulti )
				{
					ClientCommunication.PostAddMulti( item.ItemID, item.Position );
				}
				else
				{
					ScavengerAgent s = ScavengerAgent.Instance;
					int dist = Utility.Distance( item.GetWorldPosition(), World.Player.Position );
					if ( !World.Player.IsGhost && World.Player.Visible && dist <= 2 && s.Enabled && item.Movable )
						s.Scavenge( item );
				}
			}

			Item.UpdateContainers();
		}

		public static ArrayList SysMessages = new ArrayList( 21 );

		public static void HandleSpeech( Packet p, PacketHandlerEventArgs args, Serial ser, ushort body, MessageType type, ushort hue, ushort font, string lang, string name, string text )
		{
			if ( World.Player == null )
				return;

			if ( !ser.IsValid || ser == World.Player.Serial || ser.IsItem )
			{
				SysMessages.Add( text.ToLower() );

				if ( SysMessages.Count >= 25 )
					SysMessages.RemoveRange( 0, 10 );
			}

			if ( type == MessageType.Spell )
			{
				Spell s = Spell.Get( text.Trim() );
				bool replaced = false; 
				if ( s != null )
				{
					System.Text.StringBuilder sb = new System.Text.StringBuilder( Config.GetString( "SpellFormat" ) );
					sb.Replace( @"{power}", s.WordsOfPower );
					string spell = Language.GetString( s.Name );
					sb.Replace( @"{spell}", spell );
					sb.Replace( @"{name}", spell );
					
					string newText = sb.ToString();

					if ( newText != null && newText != "" && newText != text )
					{
						ClientCommunication.SendToClient( new AsciiMessage( ser, body, MessageType.Spell, s.GetHue( hue ), font, name, newText ) );
						//ClientCommunication.SendToClient( new UnicodeMessage( ser, body, MessageType.Spell, s.GetHue( hue ), font, Language.CliLocName, name, newText ) );
						replaced = true;
						args.Block = true;
					}
				}

				if ( !replaced && Config.GetBool( "ForceSpellHue" ) )
				{
					p.Seek( 10, SeekOrigin.Begin );
					if ( s != null )
						p.Write( (ushort)s.GetHue( hue ) );
					else
						p.Write( (ushort)Config.GetInt( "NeutralSpellHue" ) );
				}
			}
			else if ( ser.IsMobile && type == MessageType.Label )
			{
				Mobile m = World.FindMobile( ser );
				if ( m != null /*&& ( m.Name == null || m.Name == "" )*/ && m != World.Player && !( text.StartsWith( "(" )  && text.EndsWith( ")" ) ) )
					m.Name = text;
			}
			/*else if ( Spell.Get( text.Trim() ) != null )
			{ // send fake spells to bottom left
				p.Seek( 3, SeekOrigin.Begin );
				p.Write( (uint)0xFFFFFFFF );
			}*/
			else 
			{
				if ( ser == Serial.MinusOne && name == "System" )
				{
					if ( Config.GetBool( "FilterSnoopMsg" ) && text.IndexOf( World.Player.Name ) == -1 && text.StartsWith( "You notice" ) && text.IndexOf( "attempting to peek into" ) != -1 && text.IndexOf( "belongings" ) != -1 )
					{
						args.Block = true;
						return;
					}
					else if ( text.IndexOf( "You've committed a criminal act!" ) != -1 || text.IndexOf( "You are now a criminal" ) != -1 )
					{
						World.Player.ResetCriminalTimer();
					}
				}

				if ( ( type == MessageType.Emote || type == MessageType.Regular || type == MessageType.Whisper || type == MessageType.Yell ) && ser.IsMobile && ser != World.Player.Serial )
				{
					if ( Config.GetBool( "ForceSpeechHue" ) )
					{
						p.Seek( 10, SeekOrigin.Begin );
						p.Write( (ushort)Config.GetInt( "SpeechHue" ) );
					}
				}

				if ( Config.GetBool( "FilterSpam" ) && ( ser == Serial.MinusOne || ser == Serial.Zero ) )
				{
					if ( !MessageQueue.Enqueue( ser, body, type, hue, font, lang, name, text ) )
					{
						args.Block = true;
						return;
					}
				}
			}
		}

		public static void AsciiSpeech( Packet p, PacketHandlerEventArgs args )
		{
			// 0, 1, 2
			Serial serial = p.ReadUInt32(); // 3, 4, 5, 6
			ushort body = p.ReadUInt16(); // 7, 8
			MessageType type = (MessageType)p.ReadByte(); // 9
			ushort hue = p.ReadUInt16(); // 10, 11
			ushort font = p.ReadUInt16();
			string name = p.ReadStringSafe( 30 );
			string text = p.ReadStringSafe();

			if (  World.Player != null && serial == Serial.Zero && body == 0 && type == MessageType.Regular && hue == 0xFFFF && font == 0xFFFF && name == "SYSTEM" )
			{
				args.Block = true;

				p.Seek( 3, SeekOrigin.Begin );
				p.WriteAsciiFixed( "", (int)p.Length-3 );

				// CHEAT UO.exe 1 251--
				int features = 0;
				if ( (World.Player.Features & 0x8000) == 0 )
				{
					if ( (World.Player.Features & 1) != 0 )
						features = 2;
					if ( (World.Player.Features & 2) != 0 )
						features |= 8;
					features &= 0xFFFF;
				}
				else
				{
					features = World.Player.Features & 0x7FFF;
				}

				ClientCommunication.DoFeatures( features ) ;
			}
			else
			{
				HandleSpeech( p, args, serial, body, type, hue, font, "A", name, text );
			}
		}

		public static void UnicodeSpeech( Packet p, PacketHandlerEventArgs args )
		{
			// 0, 1, 2
			Serial serial = p.ReadUInt32(); // 3, 4, 5, 6
			ushort body = p.ReadUInt16(); // 7, 8
			MessageType type = (MessageType)p.ReadByte(); // 9
			ushort hue = p.ReadUInt16(); // 10, 11
			ushort font = p.ReadUInt16();
			string lang = p.ReadStringSafe( 4 );
			string name = p.ReadStringSafe( 30 );
			string text = p.ReadUnicodeStringSafe();

			HandleSpeech( p, args, serial, body, type, hue, font, lang, name, text );
		}

		private static void OnLocalizedMessage( Packet p, PacketHandlerEventArgs args )
		{
			// 0, 1, 2
			Serial serial = p.ReadUInt32(); // 3, 4, 5, 6
			ushort body = p.ReadUInt16(); // 7, 8
			MessageType type = (MessageType)p.ReadByte(); // 9
			ushort hue = p.ReadUInt16(); // 10, 11
			ushort font = p.ReadUInt16();
			int num = p.ReadInt32();
			string name = p.ReadStringSafe( 30 );
			string ext_str = p.ReadUnicodeStringLESafe();

			if ( ( num >= 3002011 && num < 3002011+64 ) || // reg spells
				( num >= 1060509 && num < 1060509+16 ) || // necro
				( num >= 1060585 && num < 1060585+10 ) || // chiv
				( num >= 1060493 && num < 1060493+10 ) || // chiv
				( num >= 1060595 && num < 1060595+6 ) || // bush
				( num >= 1060610 && num < 1060610+8 ) ) // ninj
			{
				type = MessageType.Spell;
			}

			try
			{
				string text = Language.ClilocFormat( num, ext_str );
				HandleSpeech( p, args, serial, body, type, hue, font, Language.CliLocName.ToUpper(), name, text );
			}
			catch ( Exception e )
			{
				Engine.LogCrash( new Exception( String.Format( "Exception in Ultima.dll cliloc: {0}, {1}", num, ext_str ), e ) );
			}
		}

		private static void OnLocalizedMessageAffix( Packet p, PacketHandlerEventArgs phea )
		{
			// 0, 1, 2
			Serial serial = p.ReadUInt32(); // 3, 4, 5, 6
			ushort body = p.ReadUInt16(); // 7, 8
			MessageType type = (MessageType)p.ReadByte(); // 9
			ushort hue = p.ReadUInt16(); // 10, 11
			ushort font = p.ReadUInt16();
			int num = p.ReadInt32();
			byte affixType = p.ReadByte();
			string name = p.ReadStringSafe( 30 );
			string affix = p.ReadStringSafe();
			string args = p.ReadUnicodeStringSafe();

			if (( num >= 3002011 && num < 3002011+64 ) || // reg spells
				( num >= 1060509 && num < 1060509+16 ) || // necro
				( num >= 1060585 && num < 1060585+10 ) || // chiv
				( num >= 1060493 && num < 1060493+10 ) || // chiv
				( num >= 1060595 && num < 1060595+6 )  || // bush
				( num >= 1060610 && num < 1060610+8 )     // ninj
				)
			{
				type = MessageType.Spell;
			}

			string text;
			if ( (affixType&1)!=0 ) // prepend
				text = String.Format( "{0}{1}", affix, Language.ClilocFormat( num, args ) );
			else // 0 == append, 2 = system
				text = String.Format( "{0}{1}", Language.ClilocFormat( num, args ), affix );
			HandleSpeech( p, phea, serial, body, type, hue, font, Language.CliLocName.ToUpper(), name, text );
		}

		private static void SendGump( PacketReader p, PacketHandlerEventArgs args )
		{ 
			World.Player.CurrentGumpS = p.ReadUInt32();
			World.Player.CurrentGumpI = p.ReadUInt32();
			World.Player.HasGump = true;
			//byte[] data = p.CopyBytes( 11, p.Length - 11 );

			if ( Macros.MacroManager.AcceptActions && MacroManager.Action( new WaitForGumpAction( World.Player.CurrentGumpI ) ) )
				args.Block = true;
		}

		private static void ClientGumpResponse( PacketReader p, PacketHandlerEventArgs args )
		{
			Serial ser = p.ReadUInt32();
			uint tid = p.ReadUInt32();
			int bid = p.ReadInt32();

			World.Player.HasGump = false;

			int sc = p.ReadInt32();
			if ( sc < 0 || sc > 2000 )
				return;
			int[] switches = new int[sc];
			for(int i=0;i<sc;i++)
				switches[i] = p.ReadInt32();

			int ec = p.ReadInt32();
			if ( ec < 0 || ec > 2000 )
				return;
			GumpTextEntry[] entries = new GumpTextEntry[ec];
			for(int i=0;i<ec;i++)
			{
				ushort id = p.ReadUInt16();
				ushort len = p.ReadUInt16();
				if ( len >= 240 )
					return;
				string text = p.ReadUnicodeStringSafe( len );
				entries[i] = new GumpTextEntry( id, text );
			}

			if ( Macros.MacroManager.AcceptActions )
				MacroManager.Action( new GumpResponseAction( bid, switches, entries ) );
		}

		private static void ChangeSeason( PacketReader p, PacketHandlerEventArgs args )
		{
			if ( World.Player != null )
				World.Player.Season = p.ReadByte();
		}

		private static void ExtendedPacket( PacketReader p, PacketHandlerEventArgs args )
		{
			ushort type = p.ReadUInt16();

			switch ( type )
			{
				case 0x04: // close gump
				{
					// int serial, int tid
					if ( World.Player != null )
						World.Player.HasGump = false;
					break;
				}
				case 0x06: // party messages
				{
					OnPartyMessage( p, args );
					break;
				}
				case 0x08: // map change
				{
					if ( World.Player != null )
						World.Player.Map = p.ReadByte();
					break;
				}				
				case 0x10: // object property list info
				{
					//args.Block = true;
					//ClientCommunication.SendToServer( new OPLInfoPacket( p.ReadUInt32(), p.ReadInt32() ) );
					break;
				}
				case 0x14: // context menu
				{
					p.ReadInt16(); // 0x01
					UOEntity ent = null;
					Serial ser = p.ReadUInt32();
					if ( ser.IsMobile )
						ent = World.FindMobile( ser );
					else if ( ser.IsItem )
						ent = World.FindItem( ser );

					if ( ent != null )
					{
						byte count = p.ReadByte();

						try
						{
							ent.ContextMenu.Clear();

							for(int i=0;i<count;i++)
							{
								ushort idx = p.ReadUInt16();
								ushort num = p.ReadUInt16();
								ushort flags = p.ReadUInt16();
								ushort color = 0;

								if ( (flags&0x02) != 0 )
									color = p.ReadUInt16();

								ent.ContextMenu.Add( idx, num );
							}
						}
						catch
						{
						}
					}
					break;
				}
				case 0x18: // map patches
				{
					if ( World.Player != null )
					{
						int count = p.ReadInt32() * 2;
						try
						{
							World.Player.MapPatches = new int[count];
							for(int i=0;i<count;i++)
								World.Player.MapPatches[i] = p.ReadInt32();
						}
						catch
						{
						}
					}
					break;
				}
				case 0x19: //  stat locks
				{
					if ( p.ReadByte() == 0x02 )
					{
						Mobile m = World.FindMobile( p.ReadUInt32() );
						if ( World.Player == m && m != null )
						{
							p.ReadByte();// 0?

							byte locks = p.ReadByte();

							World.Player.StrLock = (LockType)((locks>>4) & 3);
							World.Player.DexLock = (LockType)((locks>>2) & 3);
							World.Player.IntLock = (LockType)(locks & 3);
						}
					}
					break;
				}
				case 0x1D: // Custom House "General Info"
				{
					Item i = World.FindItem( p.ReadUInt32() );
					if ( i != null )
						i.HouseRevision = p.ReadInt32();
					break;
				}
			}
		}

		private static ArrayList m_Party = new ArrayList();
		public static ArrayList Party { get { return m_Party; } }
		private static void OnPartyMessage( PacketReader p, PacketHandlerEventArgs args )
		{
			switch ( p.ReadByte() )
			{
				case 0x01: // List
				{
					m_Party.Clear();

					int count = p.ReadByte();
					for(int i=0;i<count;i++)
					{
						Serial s = p.ReadUInt32();
						if ( World.Player == null || s != World.Player.Serial )
							m_Party.Add( s );
					}
					break;
				}
				case 0x02: // Remove Member/Re-list
				{
					m_Party.Clear();
					int count = p.ReadByte();
					p.ReadUInt32(); // the serial of who was removed
					for(int i=0;i<count;i++)
					{
						Serial s = p.ReadUInt32();
						if ( World.Player == null || s != World.Player.Serial )
							m_Party.Add( s );
					}
					break;
				}
				/*case 0x03: // text message
				case 0x04: // 3 = private, 4 = public
				{
					Serial from = p.ReadUInt32();
					string text = p.ReadUnicodeStringSafe();
					break;
				}
				case 0x07: // party invite
				{
					Serial leader = p.ReadUInt32();
					break;
				}*/
			}
		}

		private static void PingResponse( PacketReader p, PacketHandlerEventArgs args )
		{
			if ( Ping.Response( p.ReadByte() ) )
				args.Block = true;
		}

		private static void ClientEncodedPacket( PacketReader p, PacketHandlerEventArgs args )
		{
			Serial serial = p.ReadUInt32();
			ushort packetID = p.ReadUInt16();
			switch ( packetID )
			{
				case 0x19: // set ability
				{
					int ability = 0;
					if ( p.ReadByte() == 0 )
						ability = p.ReadInt32();
					
					if ( ability >= 0 && ability < (int)AOSAbility.Invalid && Macros.MacroManager.AcceptActions )
						MacroManager.Action( new SetAbilityAction( (AOSAbility)ability ) );
					break;
				}
			}
		}

		private static string m_LastPW = "";
		private static void ServerListLogin( Packet p, PacketHandlerEventArgs args )
		{
			m_LastPW = "";
			if ( !Config.GetBool( "RememberPwds" ) )
				return;

			World.AccountName = p.ReadStringSafe( 30 );
			string pass = p.ReadStringSafe( 30 );

			if ( pass == "" )
			{
				pass = PasswordMemory.Find( World.AccountName, ClientCommunication.LastConnection );
				if ( pass != null && pass != "" )
				{
					p.Seek( 31, SeekOrigin.Begin );
					p.WriteAsciiFixed( pass, 30 );
					m_LastPW = pass;
				}
			}
			else
			{
				PasswordMemory.Add( World.AccountName, pass, ClientCommunication.LastConnection );
			}
		}

		private static void GameLogin( Packet p, PacketHandlerEventArgs args )
		{
			int authID = p.ReadInt32();

			World.AccountName = p.ReadString( 30 );
			string password = p.ReadString( 30 );

			if ( password == "" && m_LastPW != "" && Config.GetBool( "RememberPwds" ) )
			{
				p.Seek( 35, SeekOrigin.Begin );
				p.WriteAsciiFixed( m_LastPW, 30 );
				m_LastPW = "";
			}
		}

		private static void MenuResponse( PacketReader pvSrc, PacketHandlerEventArgs args )
		{
			uint serial = pvSrc.ReadUInt32();
			ushort menuID = pvSrc.ReadUInt16();
			ushort index  = pvSrc.ReadUInt16();
			ushort itemID = pvSrc.ReadUInt16();
			ushort hue    = pvSrc.ReadUInt16();

			World.Player.HasMenu = false;
			if ( MacroManager.AcceptActions )
				MacroManager.Action( new MenuResponseAction( index, itemID, hue ) );
		}

		private static void SendMenu( PacketReader p, PacketHandlerEventArgs args )
		{
			World.Player.CurrentMenuS = p.ReadUInt32();
			World.Player.CurrentMenuI = p.ReadUInt16();
			World.Player.HasMenu = true;
			if ( MacroManager.AcceptActions && MacroManager.Action( new WaitForMenuAction( World.Player.CurrentMenuI ) ) )
				args.Block = true;
		}

		private static void HueResponse( PacketReader p, PacketHandlerEventArgs args )
		{
			Serial serial = p.ReadUInt32();
			ushort iid = p.ReadUInt16();
			ushort hue = p .ReadUInt16();

			if ( serial == Serial.MinusOne )
			{
				if ( HueEntry.Callback != null )
					HueEntry.Callback( hue );				
				args.Block = true;
			}
		}

		private static void ResyncRequest( PacketReader p, PacketHandlerEventArgs args )
		{
			if ( World.Player != null )
				World.Player.Resync();
		}

		private static void ServerAddress( Packet p, PacketHandlerEventArgs args )
		{
			int port = Config.GetInt( "ForcePort" ) ;
			if ( port != 0 )
			{
				try
				{
					string[] parts = Config.GetString( "ForceIP" ).Split( '.' );
					p.Write( (byte) Convert.ToInt16( parts[0] ) );
					p.Write( (byte) Convert.ToInt16( parts[1] ) );
					p.Write( (byte) Convert.ToInt16( parts[2] ) );
					p.Write( (byte) Convert.ToInt16( parts[3] ) );

					p.Write( (ushort)port );
				}
				catch
				{
					System.Windows.Forms.MessageBox.Show( Engine.MainWindow, "Error parsing Proxy Settings.", "Force Proxy Error." );
				}
			}
		}

		private static void Features( PacketReader p, PacketHandlerEventArgs args )
		{
			if ( World.Player != null )
				World.Player.Features = p.ReadUInt16();
		}

		private static void PersonalLight( PacketReader p, PacketHandlerEventArgs args )
		{
			if ( World.Player != null )
			{
				p.ReadUInt32(); // serial
				World.Player.LocalLightLevel = p.ReadSByte();
			}
		}

		private static void GlobalLight( PacketReader p, PacketHandlerEventArgs args )
		{
			if ( World.Player != null )
				World.Player.GlobalLightLevel = p.ReadByte();
		}

		private static void MovementDemand( PacketReader p, PacketHandlerEventArgs args )
		{
			if ( PacketPlayer.Playing )
				ClientCommunication.ForceSendToClient( new MobileUpdate( World.Player ) );

			World.Player.ProcessMove( (Direction)p.ReadByte() );
		}

		private static void ServerSetWarMode( PacketReader p, PacketHandlerEventArgs args )
		{
			World.Player.Warmode = p.ReadBoolean();
		}

		private static void CustomHouseInfo( PacketReader p, PacketHandlerEventArgs args )
		{
			p.ReadByte(); // compression
			p.ReadByte(); // Unknown

			Item i = World.FindItem( p.ReadUInt32() );
			if ( i != null )
			{
				i.HouseRevision = p.ReadInt32();
				i.HousePacket = p.CopyBytes( 0, p.Length );
			}
		}

		private static void CompressedGump( PacketReader p, PacketHandlerEventArgs args )
		{
			if ( World.Player != null )
			{
				World.Player.CurrentGumpS = p.ReadUInt32();
				World.Player.CurrentGumpI = p.ReadUInt32();

				
				if ( Macros.MacroManager.AcceptActions && MacroManager.Action( new WaitForGumpAction( World.Player.CurrentGumpI ) ) )
					args.Block = true;
			}
		}
/*
				int serial  = pvSrc.ReadInt32(), dialog  = pvSrc.ReadInt32();
				int xOffset = pvSrc.ReadInt32(), yOffset = pvSrc.ReadInt32();
				string layout = GetCompressedReader( pvSrc ).ReadString();
				pvSrc = GetCompressedReader( pvSrc );
				ArrayList strings = Engine.GetDataStore();
				ushort length;
				while ( !pvSrc.Finished && (length = pvSrc.ReadUInt16()) > 0 )
					strings.Add( pvSrc.ReadUnicodeString( length ) );

			int packLength = pvSrc.ReadInt32();
			int fullLength = pvSrc.ReadInt32();
			byte[] buffer = pvSrc.ReadBytes( packLength );

packLength==0 || fullLength==0 just break out
osi is rage
when i saw they used int32's for those lengths
i just snapped
since packet lengths are int16's
*/
	}
}
