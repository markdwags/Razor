using System;
using Assistant;

namespace Assistant.Filters
{
	public class LightFilter : Filter
	{
		public static void Initialize()
		{
			Filter.Register( new LightFilter() );
		}

		private LightFilter()
		{
		}

		public override byte[] PacketIDs{ get{ return new byte[]{ 0x4E, 0x4F }; } }

		public override LocString Name{ get{ return LocString.LightFilter; } }

		public override void OnFilter( PacketReader p, PacketHandlerEventArgs args )
		{
			if ( ClientCommunication.AllowBit( FeatureBit.LightFilter ) )
			{
				args.Block = true;
				if ( World.Player != null )
				{
					World.Player.LocalLightLevel = 0;
					World.Player.GlobalLightLevel = 0;
				}
			}
		}

		public override void OnEnable()
		{
			base.OnEnable ();

			if ( ClientCommunication.AllowBit( FeatureBit.LightFilter ) && World.Player != null )
			{
				World.Player.LocalLightLevel = 0;
				World.Player.GlobalLightLevel = 0;

				ClientCommunication.SendToClient( new GlobalLightLevel( 0 ) );
				ClientCommunication.SendToClient( new PersonalLightLevel( World.Player ) );
			}
		}

	}
}
