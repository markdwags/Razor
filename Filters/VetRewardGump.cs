using System;
using Assistant;

namespace Assistant.Filters
{
	public class VetRewardGumpFilter : Filter
	{
		// 1006046 = You have reward items available.  Click 'ok' below to get the selection menu or 'cancel' to be prompted upon your next login.
		private static string m_VetRewardStr = "{ xmfhtmlgump 52 35 420 55 1006046 1 1 }";

		public static void Initialize()
		{
			Filter.Register( new VetRewardGumpFilter() );
		}

		private VetRewardGumpFilter()
		{
		}

		public override byte[] PacketIDs{ get{ return new byte[]{ 0xB0, 0xDD }; } }

		public override LocString Name{ get{ return LocString.VetRewardGump; } }

		public override void OnFilter( PacketReader p, PacketHandlerEventArgs args )
		{
			// completely skip this filter if we've been connected for more thn 1 minute
			if ( ClientCommunication.ConnectionStart + TimeSpan.FromMinutes( 1.0 ) < DateTime.Now )
				return;

			try
			{
				p.Seek( 0, System.IO.SeekOrigin.Begin );
				byte packetID = p.ReadByte();

				p.MoveToData();

				uint ser = p.ReadUInt32();
				uint tid = p.ReadUInt32();
				int x = p.ReadInt32();
				int y = p.ReadInt32();
				string layout = null;

				if ( packetID == 0xDD )
				{
					layout = p.GetCompressedReader().ReadString();
				}
				else
				{
					ushort layoutLength = p.ReadUInt16();
					layout = p.ReadString( layoutLength );
				}

				if ( layout != null && layout.IndexOf( m_VetRewardStr ) != -1 )
					args.Block = true;
			}
			catch
			{
			}
		}
	}
}
