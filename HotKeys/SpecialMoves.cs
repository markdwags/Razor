using System;


namespace Assistant
{
	public enum AOSAbility
	{
		Clear,
		ArmorIgnore,
		BleedAttack,
		ConcussionBlow,
		CrushingBlow,
		Disarm,
		Dismount,
		DoubleStrike,
		InfectiousStrike,
		MortalStrike,
		MovingShot,
		ParalyzingBlow,
		ShadowStrike,
		WhirlwindAttack,
		Invalid
	}

	public class SpecialMoves
	{
		private class AbilityInfo
		{
			private AOSAbility m_Ability;
			private int[][] m_Items;
			public AbilityInfo( AOSAbility ab, params int[][] items )
			{
				m_Ability = (AOSAbility)ab;
				m_Items = items;
			}

			public AOSAbility Ability{ get{ return m_Ability; } }
			
			public bool HasItem( int item )
			{
				for(int a=0;a<m_Items.Length;a++)
				{
					for(int b=0;b<m_Items[a].Length;b++)
					{
						if ( m_Items[a][b] == item )
							return true;
					}
				}

				return false;
			}
		}

		private static DateTime m_LastToggle = DateTime.MinValue;

		private static int[] HatchetID = new int[]{ 0xF43, 0xF44 };
		private static int[] LongSwordID = new int[]{ 0xF60, 0xF61 };
		private static int[] BroadswordID = new int[]{ 0xF5E, 0xF5F };
		private static int[] KatanaID = new int[]{ 0x13FE, 0x13FF };
		private static int[] BladedStaffID = new int[]{ 0x26BD, 0x26C7 };
		private static int[] HammerPickID = new int[]{ 0x143C, 0x143D };
		private static int[] WarAxeID = new int[]{ 0x13AF, 0x13B0 };
		private static int[] KryssID = new int[]{ 0x1400, 0x1401 };
		private static int[] SpearID = new int[]{ 0xF62, 0xF63 };
		private static int[] CompositeBowID = new int[]{ 0x26C2, 0x26CC };
		private static int[] CleaverID = new int[]{ 0xEC2, 0xEC3 };
		private static int[] LargeBattleAxeID = new int[]{ 0x13FA, 0x13FB };
		private static int[] BattleAxeID = new int[]{ 0xF47, 0xF48 };
		private static int[] ExecAxeID = new int[]{ 0xF45, 0xF46 };
		private static int[] CutlassID = new int[]{ 0x1440, 0x1441 };
		private static int[] ScytheID = new int[]{ 0x26BA, 0x26C4 };
		private static int[] WarMaceID = new int[]{ 0x1406, 0x1407 };
		private static int[] PitchforkID = new int[]{ 0xE87, 0xE88 };
		private static int[] WarForkID = new int[]{ 0x1404, 0x1405 };
		private static int[] HalberdID = new int[]{ 0x143E, 0x143F };
		private static int[] MaulID = new int[]{ 0x143A, 0x143B };
		private static int[] MaceID = new int[]{ 0xF5C, 0x45D };
		private static int[] GnarledStaffID = new int[]{ 0x13F8, 0x13F9 };
		private static int[] QuarterStaffID = new int[]{ 0xE89, 0xE8A };
		private static int[] LanceID = new int[]{ 0x26C0, 0x26CA };
		private static int[] CrossbowID = new int[]{ 0xF4F, 0xF50 };
		private static int[] VikingSwordID = new int[]{ 0x13B9, 0x13BA };
		private static int[] AxeID = new int[]{ 0xF49, 0xF4A };
		private static int[] ShepherdsCrookID = new int[]{ 0xE81, 0xE82 };
		private static int[] SmithsHammerID = new int[]{ 0x13EC, 0x13E4 };
		private static int[] WarHammerID = new int[]{ 0x1438, 0x1439 };
		private static int[] ScepterID = new int[]{ 0x26BC, 0x26C6 };
		private static int[] SledgeHammerID = new int[]{ 0xFB4, 0xFB5 };
		private static int[] ButcherKnifeID = new int[]{ 0x13F6, 0x13F7 };
		private static int[] PickaxeID = new int[]{ 0xE85, 0xE86 };
		private static int[] SkinningKnifeID = new int[]{ 0xEC4, 0xEC5 };
		private static int[] WandID = new int[]{ 0xDF2, 0xDF3, 0xDF4, 0xDF5 };
		private static int[] BardicheID = new int[]{ 0xF4D, 0xF4E };
		private static int[] ClubID = new int[]{ 0x13B3, 0x13B4 };
		private static int[] ScimitarID = new int[]{ 0x13B5, 0x13B6 };
		private static int[] HeavyCrossbowID = new int[]{ 0x13FC, 0x13FD };
		private static int[] TwoHandedAxeID = new int[]{ 0x1442, 0x1443 };
		private static int[] DoubleAxeID = new int[]{ 0xF4B, 0xF4C };
		private static int[] CrescentBladeID = new int[]{ 0x26C1, 0x26C2 };
		private static int[] DoubleBladedStaffID = new int[]{ 0x26BF, 0x26C9 };
		private static int[] RepeatingCrossbowID = new int[]{ 0x26C3, 0x26CD };
		private static int[] DaggerID = new int[]{ 0xF51, 0xF52 };
		private static int[] PikeID = new int[]{ 0x26BE, 0x26C8 };
		private static int[] BoneHarvesterID = new int[]{ 0x26BB, 0x26C5 };
		private static int[] ShortSpearID = new int[]{ 0x1402, 0x1403 };
		private static int[] BowID = new int[]{ 0x13B1, 0x13B2 };
		private static int[] BlackStaffID = new int[]{ 0xDF0, 0xDF1 };
		private static int[] FistsID = new int[]{ 0 };

		private static AbilityInfo[] m_Primary = new AbilityInfo[]
		{
			new AbilityInfo( AOSAbility.ArmorIgnore, HatchetID, LongSwordID, BladedStaffID, HammerPickID, WarAxeID, KryssID, SpearID, CompositeBowID ),
			new AbilityInfo( AOSAbility.BleedAttack,  CleaverID, BattleAxeID, ExecAxeID, CutlassID, ScytheID, PitchforkID, WarForkID ),
			new AbilityInfo( AOSAbility.ConcussionBlow, MaceID, GnarledStaffID, CrossbowID ),
			new AbilityInfo( AOSAbility.CrushingBlow, VikingSwordID, AxeID, BroadswordID, ShepherdsCrookID, SmithsHammerID, MaulID, WarMaceID, ScepterID, SledgeHammerID ),
			new AbilityInfo( AOSAbility.Disarm, FistsID ), 
			new AbilityInfo( AOSAbility.Dismount, WandID, LanceID ),
			new AbilityInfo( AOSAbility.DoubleStrike, PickaxeID, TwoHandedAxeID, DoubleAxeID, ScimitarID, KatanaID, CrescentBladeID, QuarterStaffID, DoubleBladedStaffID, RepeatingCrossbowID ),
			new AbilityInfo( AOSAbility.InfectiousStrike, ButcherKnifeID, DaggerID ),
			//new AbilityInfo( AOSAbility.MortalStrike ), // not primary for anything
			new AbilityInfo( AOSAbility.MovingShot, HeavyCrossbowID ),
			new AbilityInfo( AOSAbility.ParalyzingBlow, BardicheID, BoneHarvesterID, PikeID, BowID ),
			new AbilityInfo( AOSAbility.ShadowStrike, SkinningKnifeID, ClubID, ShortSpearID ),
			new AbilityInfo( AOSAbility.WhirlwindAttack, LargeBattleAxeID, HalberdID, WarHammerID, BlackStaffID )
		};

		private static AbilityInfo[] m_Secondary = new AbilityInfo[]
		{
			new AbilityInfo( AOSAbility.ArmorIgnore, LargeBattleAxeID, BroadswordID, KatanaID ),
			new AbilityInfo( AOSAbility.BleedAttack, WarMaceID, WarAxeID ),
			new AbilityInfo( AOSAbility.ConcussionBlow, LongSwordID, BattleAxeID, HalberdID, MaulID, QuarterStaffID, LanceID ),
			new AbilityInfo( AOSAbility.CrushingBlow, WarHammerID ),
			new AbilityInfo( AOSAbility.Disarm, ButcherKnifeID, PickaxeID, SkinningKnifeID, HatchetID, WandID, ShepherdsCrookID, MaceID, WarForkID ), 
			new AbilityInfo( AOSAbility.Dismount, BardicheID, AxeID, BladedStaffID, ClubID, PitchforkID, HeavyCrossbowID ),
			//new AbilityInfo( AOSAbility.DoubleStrike ), // secondary on none
			new AbilityInfo( AOSAbility.InfectiousStrike, CleaverID, PikeID, KryssID, DoubleBladedStaffID ),
			new AbilityInfo( AOSAbility.MortalStrike, ExecAxeID, BoneHarvesterID, CrescentBladeID, HammerPickID, ScepterID, ShortSpearID, CrossbowID, BowID ),
			new AbilityInfo( AOSAbility.MovingShot, CompositeBowID, RepeatingCrossbowID ),
			new AbilityInfo( AOSAbility.ParalyzingBlow, VikingSwordID, ScimitarID, ScytheID, GnarledStaffID, BlackStaffID, SpearID, FistsID ),
			new AbilityInfo( AOSAbility.ShadowStrike, TwoHandedAxeID, CutlassID, SmithsHammerID, DaggerID, SledgeHammerID ),
			new AbilityInfo( AOSAbility.WhirlwindAttack, DoubleAxeID )
		};

		public static void Initialize()
		{
			HotKey.Add( HKCategory.Misc, HKSubCat.None, LocString.ToggleWarPeace, new HotKeyCallback( ToggleWarPeace ) );

			HotKey.Add( HKCategory.Misc, HKSubCat.SpecialMoves, LocString.ClearAbility, new HotKeyCallback( ClearAbilities ) );

			HotKey.Add( HKCategory.Misc, HKSubCat.SpecialMoves, LocString.SetPrimAb, new HotKeyCallback( SetPrimaryAbility ) );
			HotKey.Add( HKCategory.Misc, HKSubCat.SpecialMoves, LocString.SetSecAb, new HotKeyCallback( SetSecondaryAbility ) );

			HotKey.Add( HKCategory.Misc, HKSubCat.SpecialMoves, LocString.ToggleStun, new HotKeyCallback( OnStun ) );
			HotKey.Add( HKCategory.Misc, HKSubCat.SpecialMoves, LocString.ToggleDisarm, new HotKeyCallback( OnDisarm ) );
		}

		private static void ToggleWarPeace()
		{
			ClientCommunication.SendToServer( new SetWarMode( !World.Player.Warmode ) );
		}

		private static void OnStun()
		{
			if ( m_LastToggle+TimeSpan.FromSeconds( 0.5 ) < DateTime.Now )
			{
				m_LastToggle = DateTime.Now;
				ClientCommunication.SendToServer( new StunRequest() );
			}
		}

		private static void OnDisarm()
		{
			if ( m_LastToggle+TimeSpan.FromSeconds( 0.5 ) < DateTime.Now )
			{
				m_LastToggle = DateTime.Now;
				ClientCommunication.SendToServer( new DisarmRequest() );
			}
		}

		private static AOSAbility GetAbility( int item, AbilityInfo[] list )
		{
			for (int a=0;a<list.Length;a++)
			{
				if ( list[a].HasItem( item ) )
					return list[a].Ability;
			}

			return AOSAbility.Invalid;
		}

        public static void SetPrimaryAbility()
		{
			Item right = World.Player.GetItemOnLayer( Layer.RightHand );
			Item left = World.Player.GetItemOnLayer( Layer.LeftHand ); 

			AOSAbility a = AOSAbility.Invalid;
			if ( right != null )
				a = GetAbility( right.ItemID.Value, m_Primary );

			if ( a == AOSAbility.Invalid && left != null )
				a = GetAbility( left.ItemID.Value, m_Primary );

			if ( a == AOSAbility.Invalid )
				a = GetAbility( FistsID[0], m_Primary );

			if ( a != AOSAbility.Invalid )
			{
				ClientCommunication.SendToServer( new UseAbility( a ) );
				ClientCommunication.SendToClient( ClearAbility.Instance );
				World.Player.SendMessage( LocString.SettingAOSAb, a );
			}
		}

		public static void SetSecondaryAbility()
		{
			Item right = World.Player.GetItemOnLayer( Layer.RightHand );
			Item left = World.Player.GetItemOnLayer( Layer.LeftHand ); 

			AOSAbility a = AOSAbility.Invalid;
			if ( right != null )
				a = GetAbility( right.ItemID.Value, m_Secondary );

			if ( a == AOSAbility.Invalid && left != null )
				a = GetAbility( left.ItemID.Value, m_Secondary );

			if ( a == AOSAbility.Invalid )
				a = GetAbility( FistsID[0], m_Secondary );

			if ( a != AOSAbility.Invalid )
			{
				ClientCommunication.SendToServer( new UseAbility( a ) );
				ClientCommunication.SendToClient( ClearAbility.Instance );
				World.Player.SendMessage( LocString.SettingAOSAb, a );
			}
		}

		public static void ClearAbilities()
		{
			ClientCommunication.SendToServer( new UseAbility( AOSAbility.Clear ) );
			ClientCommunication.SendToClient( ClearAbility.Instance );
			World.Player.SendMessage( LocString.AOSAbCleared );
		}
	}
}

