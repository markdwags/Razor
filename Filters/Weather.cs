using System;
using Assistant;

namespace Assistant.Filters
{
	public class WeatherFilter : Filter
	{
		public static void Initialize()
		{
			Filter.Register( new WeatherFilter() );
		}

		private WeatherFilter()
		{
		}

		public override byte[] PacketIDs{ get{ return new byte[]{ 0x65 }; } }

		public override LocString Name{ get{ return LocString.Weather; } }

		public override void OnFilter( PacketReader p, PacketHandlerEventArgs args )
		{
			if ( ClientCommunication.AllowBit( FeatureBit.WeatherFilter ) )
				args.Block = true;
		}
	}
}

