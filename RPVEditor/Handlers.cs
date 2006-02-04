using System;
using System.IO;
using System.Collections;

namespace Assistant
{
	public delegate void PacketHandler( PacketReader reader );

	public class PacketHandlers
	{
		static PacketHandlers()
		{
			//Server -> Client handlers
			Register( 0x11, new PacketHandler( MobileStatus ) );
			Register( 0x1A, new PacketHandler( WorldItem ) );
			Register( 0x1D, new PacketHandler( RemoveObject ) );
			Register( 0x20, new PacketHandler( MobileUpdate ) );
			Register( 0x2D, new PacketHandler( MobileStatInfo ) );
			Register( 0x2E, new PacketHandler( EquipmentUpdate ) );
			Register( 0x3A, new PacketHandler( Skills ) );
			Register( 0x4E, new PacketHandler( PersonalLight ) );	
			Register( 0x4F, new PacketHandler( GlobalLight ) );
			Register( 0x72, new PacketHandler( ServerSetWarMode ) );
			Register( 0x76, new PacketHandler( ServerChange ) );
			Register( 0x77, new PacketHandler( MobileMoving ) );
			Register( 0x78, new PacketHandler( MobileIncoming ) );
			Register( 0x97, new PacketHandler( MovementDemand ) );
			Register( 0xA1, new PacketHandler( HitsUpdate ) );		
			Register( 0xA2, new PacketHandler( ManaUpdate ) );	
			Register( 0xA3, new PacketHandler( StamUpdate ) );	
			Register( 0xB9, new PacketHandler( Features ) );
			Register( 0xBC, new PacketHandler( ChangeSeason ) );
			Register( 0xBF, new PacketHandler( ExtendedPacket ) );
		}

		private static Hashtable m_Table = new Hashtable();
		private static void Register( byte PacketID, PacketHandler callback )
		{
			m_Table[PacketID] = callback;
		}

		public static void ProcessPacket( byte[] data )
		{
			PacketHandler call = m_Table[data[0]] as PacketHandler;

			if ( call != null )
			{
				PacketReader pvSrc = new PacketReader( data, RPVEditor.PacketInfo.IsDyn( data[0] ) );
				pvSrc.MoveToData();
				call( pvSrc );
			}
		}

		private static void EquipmentUpdate( PacketReader p )
		{
			Serial serial = p.ReadUInt32();

			Item i = World.FindItem( serial );
			if ( i == null )
			{
				World.AddItem( i=new Item( serial ) );
				Item.UpdateContainers();
			}

			if ( World.Player != null && World.Player.Holding == i )
				World.Player.Holding = null;

			ushort iid = p.ReadUInt16();
			i.ItemID = (ushort)(iid + p.ReadSByte()); // signed, itemID offset
			i.Layer = p.ReadByte();
			Serial ser = p.ReadUInt32();// cont must be set after hue (for counters)
			i.Hue = p.ReadUInt16();

			i.Container = ser;
		}

		private static void Skills( PacketReader p )
		{
			if ( World.Player == null )
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

							skill.FixedValue = p.ReadUInt16();
							skill.FixedBase = p.ReadUInt16();
							skill.Lock = (LockType)p.ReadByte();
							skill.FixedCap = p.ReadUInt16();
						}
						else
						{
							p.Seek( 7, SeekOrigin.Current );
						}
					}
					break;
				}

				case 0xDF://change (with cap, new clients)
				{
					int i = p.ReadUInt16();

					if ( i >= 0 && i < Skill.Count )
					{
						Skill skill = World.Player.Skills[i];

						skill.FixedValue = p.ReadUInt16();
						skill.FixedBase = p.ReadUInt16();
						skill.Lock = (LockType)p.ReadByte();
						skill.FixedCap = p.ReadUInt16();
					}
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

							skill.FixedValue = p.ReadUInt16();
							skill.FixedBase = p.ReadUInt16();
							skill.Lock = (LockType)p.ReadByte();
							skill.FixedCap = 100;//p.ReadUInt16();
						}
						else
						{
							p.Seek( 5, SeekOrigin.Current );
						}
					}
					break;
				}

				case 0xFF://change (without cap, older clients)
				{
					int i = p.ReadUInt16();

					if ( i >= 0 && i < Skill.Count )
					{
						Skill skill = World.Player.Skills[i];

						skill.FixedValue = p.ReadUInt16();
						skill.FixedBase = p.ReadUInt16();
						skill.Lock = (LockType)p.ReadByte();
						skill.FixedCap = 100;
					}
					break;
				}
			}
		}

		private static void MobileMoving( PacketReader p )
		{
			Mobile m = World.FindMobile( p.ReadUInt32() );

			if ( m != null )
			{
				m.Body = p.ReadUInt16();
				m.Position = new Point3D( p.ReadUInt16(), p.ReadUInt16(), p.ReadSByte() );
				
				if ( Utility.Distance( World.Player.Position, m.Position ) > 18 )
				{
					World.RemoveMobile( m );
					return;
				}

				m.Direction = (Direction)p.ReadByte();
				m.Hue = p.ReadUInt16();
				bool wasPoisoned = m.Poisoned;
				m.ProcessPacketFlags( p.ReadByte() );
				byte oldNoto = m.Notoriety;
				m.Notoriety = p.ReadByte();
			}
		}

		private static void HitsUpdate( PacketReader p )
		{
			Mobile m = World.FindMobile( p.ReadUInt32() );

			if ( m != null )
			{
				m.HitsMax = p.ReadUInt16();
				m.Hits = p.ReadUInt16();
			}
		}

		private static void StamUpdate( PacketReader p )
		{
			Mobile m = World.FindMobile( p.ReadUInt32() );

			if ( m != null && m == World.Player )
			{
				World.Player.StamMax = p.ReadUInt16();
				World.Player.Stam = p.ReadUInt16();
			}
		}

		private static void ManaUpdate( PacketReader p )
		{
			Mobile m = World.FindMobile( p.ReadUInt32() );

			if ( m != null && m == World.Player )
			{
				World.Player.ManaMax = p.ReadUInt16();
				World.Player.Mana = p.ReadUInt16();
			}
		}

		private static void MobileStatInfo( PacketReader pvSrc )
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
		}

		private static void MobileStatus( PacketReader p )
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

				player.Str = p.ReadUInt16();
				player.Dex = p.ReadUInt16();
				player.Int = p.ReadUInt16();

				player.Stam = p.ReadUInt16();
				player.StamMax = p.ReadUInt16();
				player.Mana = p.ReadUInt16();
				player.ManaMax = p.ReadUInt16();

				player.Gold = p.ReadUInt32();
				player.AR = p.ReadUInt16(); // ar / physical resist
				player.Weight = p.ReadUInt16();

				if ( type == 0x03 || type == 0x04 )
				{
					player.StatCap = p.ReadUInt16();
					player.Followers = p.ReadByte();
					player.FollowersMax = p.ReadByte();

					if ( type == 0x04 )
					{
						/*m_Stream.Write( (short) beheld.FireResistance ); // Fire
						m_Stream.Write( (short) beheld.ColdResistance ); // Cold
						m_Stream.Write( (short) beheld.PoisonResistance ); // Poison
						m_Stream.Write( (short) beheld.EnergyResistance ); // Energy
						m_Stream.Write( (short) 0 ); // Luck
						m_Stream.Write( (short) 0 ); // Damage min
						m_Stream.Write( (short) 0 ); // Damage max*/
						p.Seek( 14, SeekOrigin.Current );
						player.Tithe = p.ReadInt32();
					}
				}
			}
		}
		
		private static void MobileUpdate( PacketReader p )
		{
			if ( World.Player == null )
				return;

			Serial serial = p.ReadUInt32();
			Mobile m = World.FindMobile( serial );
			if ( m == null )
				World.AddMobile( m = new Mobile( serial ) );
			m.Body = (ushort)(p.ReadUInt16() + p.ReadSByte());
			m.Hue = p.ReadUInt16();
			bool wasPoisoned = m.Poisoned;
			m.ProcessPacketFlags( p.ReadByte() );

			ushort x = p.ReadUInt16();
			ushort y = p.ReadUInt16();
			p.ReadUInt16(); //always 0?
			m.Direction = (Direction)p.ReadByte();
			m.Position = new Point3D( x, y, p.ReadSByte() );

			Item.UpdateContainers();
		}

		private static void MobileIncoming( PacketReader p )
		{
			Serial serial = p.ReadUInt32();
			ushort body =  p.ReadUInt16();
			Point3D position = new Point3D( p.ReadUInt16(), p.ReadUInt16(), p.ReadSByte() );

			if ( Utility.Distance( World.Player.Position, position ) > 18 )
				return;

			Mobile m = World.FindMobile( serial );
			if ( m == null )
				World.AddMobile( m = new Mobile( serial ) );

			bool wasHidden = !m.Visible;

			m.Body = body;
			m.Position = position;
			m.Direction = (Direction)p.ReadByte();
			m.Hue = p.ReadUInt16();
			bool wasPoisoned = m.Poisoned;
			m.ProcessPacketFlags( p.ReadByte() );
			byte oldNoto = m.Notoriety;
			m.Notoriety = p.ReadByte();

			while ( true ) 
			{
				serial = p.ReadUInt32();
				if ( !serial.IsItem )
					break;
				
				Item item = World.FindItem( serial );
				if ( item == null )
					World.AddItem( item = new Item( serial ) );

				if ( World.Player.Holding == item )
					World.Player.Holding = null;

				ushort id = p.ReadUInt16();
				item.ItemID = (ushort)(id & 0x3FFF);
				item.Layer = p.ReadByte();

				if ( (id & 0x8000) != 0 )
					item.Hue = p.ReadUInt16();
				else
					item.Hue = 0;
					
				item.Container = m;
			}
			Item.UpdateContainers();
		}

		private static void RemoveObject( PacketReader p )
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
					i.Remove();
			}
		}

		private static void ServerChange( PacketReader p )
		{
			World.Player.Position = new Point3D( p.ReadUInt16(), p.ReadUInt16(), p.ReadInt16() );
		}

		private static void WorldItem( PacketReader p )
		{
			Item item;
			uint serial = p.ReadUInt32();
			item = World.FindItem( serial&0x7FFFFFFF );
			if ( item == null )
			{
				World.AddItem( item=new Item( serial&0x7FFFFFFF ) );
			}

			item.Container = null;
			if ( World.Player.Holding == item )
				World.Player.Holding = null;

			ushort itemID = p.ReadUInt16();
			item.ItemID = (ushort)(itemID&0x7FFF);

			if ( (serial & 0x80000000) != 0 )
				item.Amount = p.ReadUInt16();
			else
				item.Amount = 0;

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

			Item.UpdateContainers();
		}

		private static void ChangeSeason( PacketReader p )
		{
			World.Player.Season = p.ReadByte();
		}

		private static void ExtendedPacket( PacketReader p )
		{
			ushort type = p.ReadUInt16();

			switch ( type )
			{
				case 0x04: // close gump
				{
					// int serial, int tid
					World.Player.HasGump = false;
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
				case 0x18: // map patches
				{
					int count = p.ReadInt32() * 2;
					for(int i=0;i<count;i++)
						World.Player.MapPatches[i] = p.ReadInt32();
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
			}
		}

		private static void Features( PacketReader p )
		{
			if ( World.Player != null )
				World.Player.Features = p.ReadUInt16();
		}

		private static void PersonalLight( PacketReader p )
		{
			p.ReadUInt32(); // serial
			World.Player.LocalLightLevel = p.ReadSByte();
		}

		private static void GlobalLight( PacketReader p )
		{
			World.Player.GlobalLightLevel = p.ReadByte();
		}

		private static void MovementDemand( PacketReader p )
		{
			World.Player.ProcessMove( (Direction)p.ReadByte() );
		}

		private static void ServerSetWarMode( PacketReader p )
		{
			World.Player.Warmode = p.ReadBoolean();
		}
	}
}
