using System;
using Assistant;

namespace Assistant.Filters
{
	public class StaffItemFilter : Filter
	{
		public static void Initialize()
		{
			Filter.Register( new StaffItemFilter() );
		}

		private StaffItemFilter()
		{
		}

		public override byte[] PacketIDs{ get{ return new byte[]{ 0x1A }; } }

		public override LocString Name{ get{ return LocString.StaffOnlyItems; } }

		private static bool IsStaffItem( ItemID itemID )
		{
			return ( 
				itemID == 0x36FF || // LOS blocker
				itemID == 0x1183 // Movement blocker
				);
		}

		private static bool IsStaffItem( Item i )
		{
			return i.OnGround && ( IsStaffItem( i.ItemID ) || !i.Visible );
		}

		public override void OnFilter( PacketReader p, PacketHandlerEventArgs args )
		{
			uint serial = p.ReadUInt32();
			ushort itemID = p.ReadUInt16();

			if ( (serial & 0x80000000) != 0 )
				p.ReadUInt16(); // amount

			if ( (itemID & 0x8000) != 0 )
				itemID = (ushort)((itemID&0x7FFF) + p.ReadSByte()); // itemID offset

			ushort x = p.ReadUInt16();
			ushort y = p.ReadUInt16();

			if ( (x & 0x8000) != 0 )
				p.ReadByte(); // direction

			short z = p.ReadSByte();

			if ( ( y & 0x8000 ) != 0 )
				p.ReadUInt16(); // hue

			bool visable = true;
			if ( ( y & 0x4000 ) != 0 )
			{
				int flags = p.ReadByte();

				visable = ( (flags&0x80) == 0 );
			}
			
			if ( IsStaffItem( itemID ) || !visable )
				args.Block = true;
		}

		public override void OnEnable()
		{
			base.OnEnable ();

			if ( World.Player != null )
			{
				foreach ( Item i in World.Items.Values )
				{
					if ( IsStaffItem( i ) )
						ClientCommunication.SendToClient( new RemoveObject( i ) );
				}
			}
		}

		public override void OnDisable()
		{
			base.OnDisable ();

			if ( World.Player != null )
			{
				foreach ( Item i in World.Items.Values )
				{
					if ( IsStaffItem( i ) )
						ClientCommunication.SendToClient( new WorldItem( i ) );
				}
			}
		}
	}
}
