using System;
using System.Collections;
using Ultima;

namespace Assistant
{
	public class MultiTileEntry
	{
		public ushort m_ItemID;
		public short m_OffsetX;
		public short m_OffsetY;
		public short m_OffsetZ;
	}

	public class Map
	{
		public static Ultima.Map GetMap( int mapNum )
		{
			switch ( mapNum )
			{
				case 1: return Ultima.Map.Trammel; 
				case 2: return Ultima.Map.Ilshenar;
				case 3: return Ultima.Map.Malas; 
				case 4: return Ultima.Map.Tokuno;
				case 0: 
				default:return Ultima.Map.Felucca;
			}
		}

		public static int Parse( string name )
		{
			if ( name == null || name == "" )
				return 0;

			name = name.ToLower();

			if ( name == "felucca" )
				return 0;
			else if ( name == "trammel" )
				return 1;
			else if ( name == "ilshenar" )
				return 2;
			else if ( name == "malas" )
				return 3;
			else if ( name == "samurai" || name == "tokuno" )
				return 4;
			else
				return 0;
		}

		public static HuedTile GetTileNear( int mapNum, int x, int y, int z )
		{
			try
			{
				Ultima.Map map = GetMap( mapNum );
			
				HuedTile[] tiles = map.Tiles.GetStaticTiles( x, y );
				if ( tiles != null && tiles.Length > 0 )
				{
					for ( int i=0;i<tiles.Length;i++ )
					{
						if ( tiles[i].Z >= z-5 && tiles[i].Z <= z+5 )
							return tiles[i];
					}
				}
			}
			catch
			{
			}
			
			return new HuedTile( 0, 0, (sbyte)z );
		}

		private static void GetAverageZ( Ultima.Map map, int x, int y, ref int z, ref int avg, ref int top )
		{
			try
			{
				int zTop = map.Tiles.GetLandTile( x, y ).Z;
				int zLeft = map.Tiles.GetLandTile( x, y + 1 ).Z;
				int zRight = map.Tiles.GetLandTile( x + 1, y ).Z;
				int zBottom = map.Tiles.GetLandTile( x + 1, y + 1 ).Z;

				z = zTop;
				if ( zLeft < z )
					z = zLeft;
				if ( zRight < z )
					z = zRight;
				if ( zBottom < z )
					z = zBottom;

				top = zTop;
				if ( zLeft > top )
					top = zLeft;
				if ( zRight > top )
					top = zRight;
				if ( zBottom > top )
					top = zBottom;

				if ( Math.Abs( zTop - zBottom ) > Math.Abs( zLeft - zRight) )
					avg = (int)Math.Floor( (zLeft + zRight) / 2.0 );
				else
					avg = (int)Math.Floor( (zTop + zBottom) / 2.0 );
			}
			catch
			{
			}
		}

		public static sbyte ZTop( int mapNum, int xCheck, int yCheck, int oldZ )
		{
			try
			{
				Ultima.Map map = GetMap( mapNum );

				Tile landTile = map.Tiles.GetLandTile( xCheck, yCheck );
				int landZ = 0, landCenter = 0, zTop = 0;
			
				GetAverageZ( map, xCheck, yCheck, ref landZ, ref landCenter, ref zTop );

				if ( zTop > oldZ )
					oldZ = zTop;

				bool isSet = false;
				HuedTile[] staticTiles = map.Tiles.GetStaticTiles( xCheck, yCheck );
				for ( int i = 0; i < staticTiles.Length; ++i )
				{
					HuedTile tile = staticTiles[i];
					ItemData id = TileData.ItemTable[tile.ID & 0x3FFF];

					int calcTop = (tile.Z + id.CalcHeight);

					if ( calcTop <= oldZ+5 && ( !isSet || calcTop > zTop ) && ( (id.Flags & TileFlag.Surface) != 0 || (id.Flags&TileFlag.Wet) != 0 ) )
					{
						zTop = calcTop;
						isSet = true;
					}
				}

				return (sbyte)zTop;
			}
			catch
			{
				return (sbyte)oldZ;
			}
		}
	}
}

