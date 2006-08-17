using System;
using System.Collections;
using Assistant.Macros;

namespace Assistant
{
	public class TargetInfo
	{
		public byte Type;
		public uint TargID;
		public byte Flags;
		public Serial Serial;
		public int X, Y;
		public int Z;
		public ushort Gfx;
	}

	public class Targeting
	{
		public const uint LocalTargID = 0x7FFFFFFF; // uid for target sent from razor

		public delegate void TargetResponseCallback( bool location, Serial serial, Point3D p, ushort gfxid );
		public delegate void CancelTargetCallback();

		private static CancelTargetCallback m_OnCancel;
		private static TargetResponseCallback m_OnTarget;
		
		private static bool m_Intercept;
		private static bool m_HasTarget;
		private static bool m_ClientTarget;
		private static TargetInfo m_LastTarget;
		private static TargetInfo m_LastBeneTarg;
		private static TargetInfo m_LastHarmTarg;

		private static bool m_AllowGround;
		private static uint m_CurrentID;
		private static byte m_CurFlags;

		private static uint m_PreviousID;
		private static bool m_PreviousGround;
		private static byte m_PrevFlags;

		private static Serial m_LastCombatant;

		private delegate bool QueueTarget();
		private static QueueTarget TargetSelfAction = new QueueTarget( DoTargetSelf );
		private static QueueTarget LastTargetAction = new QueueTarget( DoLastTarget );
		private static QueueTarget m_QueueTarget;

		
		private static uint m_SpellTargID = 0;
		public static uint SpellTargetID { get { return m_SpellTargID; } set { m_SpellTargID = value; } }

		private static ArrayList m_FilterCancel = new ArrayList();

		public static bool HasTarget { get{ return m_HasTarget; } }

		public static void Initialize()
		{
			PacketHandler.RegisterClientToServerViewer( 0x6C, new PacketViewerCallback( TargetResponse ) );
			PacketHandler.RegisterServerToClientViewer( 0x6C, new PacketViewerCallback( NewTarget ) );
			PacketHandler.RegisterServerToClientViewer( 0xAA, new PacketViewerCallback( CombatantChange ) );

			HotKey.Add( HKCategory.Targets, LocString.LastTarget, new HotKeyCallback( LastTarget ) );
			HotKey.Add( HKCategory.Targets, LocString.TargetSelf, new HotKeyCallback( TargetSelf ) );
			HotKey.Add( HKCategory.Targets, LocString.ClearTargQueue, new HotKeyCallback( OnClearQueue ) );
			HotKey.Add( HKCategory.Targets, LocString.SetLT, new HotKeyCallback( TargetSetLastTarget ) );
			HotKey.Add( HKCategory.Targets, LocString.TargRed, new HotKeyCallback( TargetRed ) );
			HotKey.Add( HKCategory.Targets, LocString.TargNFriend, new HotKeyCallback( TargetNonFriendly ) );
			HotKey.Add( HKCategory.Targets, LocString.TargFriend, new HotKeyCallback( TargetFriendly ) );
			HotKey.Add( HKCategory.Targets, LocString.TargBlue, new HotKeyCallback( TargetInnocent ) );
			HotKey.Add( HKCategory.Targets, LocString.TargGrey, new HotKeyCallback( TargetGrey ) );
			HotKey.Add( HKCategory.Targets, LocString.TargEnemy, new HotKeyCallback( TargetEnemy ) );
			HotKey.Add( HKCategory.Targets, LocString.TargCriminal, new HotKeyCallback( TargetCriminal ) );

			HotKey.Add( HKCategory.Targets, LocString.TargEnemyHuman, new HotKeyCallback( TargetEnemyHuman ) );
			HotKey.Add( HKCategory.Targets, LocString.TargGreyHuman, new HotKeyCallback( TargetGreyHuman ) );
			HotKey.Add( HKCategory.Targets, LocString.TargInnocentHuman, new HotKeyCallback( TargetInnocentHuman ) );
			HotKey.Add( HKCategory.Targets, LocString.TargCriminalHuman, new HotKeyCallback( TargetCriminalHuman ) );

			HotKey.Add( HKCategory.Targets, LocString.AttackLastComb, new HotKeyCallback( AttackLastComb ) );
			HotKey.Add( HKCategory.Targets, LocString.AttackLastTarg, new HotKeyCallback( AttackLastTarg ) );
			HotKey.Add( HKCategory.Targets, LocString.CancelTarget, new HotKeyCallback( CancelTarget ) );

			HotKey.Add( HKCategory.Targets, LocString.NextTarget, new HotKeyCallback( NextTarget ) );
		}

		private static void CombatantChange( PacketReader p, PacketHandlerEventArgs e )
		{
			Serial ser = p.ReadUInt32();
			if ( ser.IsMobile && ser != World.Player.Serial && ser != Serial.Zero && ser != Serial.MinusOne )
				m_LastCombatant = ser;
		}

		private static void AttackLastComb()
		{
			if ( m_LastCombatant.IsMobile )
				ClientCommunication.SendToServer( new AttackReq( m_LastCombatant ) );
		}

		private static void AttackLastTarg()
		{
			if ( m_LastTarget != null && m_LastTarget.Serial.IsMobile )
				ClientCommunication.SendToServer( new AttackReq( m_LastTarget.Serial ) );
		}

		private static void OnClearQueue()
		{
			Targeting.ClearQueue();
			World.Player.OverheadMessage( LocString.TQCleared );
		}

		internal static void OneTimeTarget( TargetResponseCallback onTarget )
		{
			OneTimeTarget( false, onTarget, null );
		}

		internal static void OneTimeTarget( bool ground, TargetResponseCallback onTarget )
		{
			OneTimeTarget( ground, onTarget, null );
		}

		internal static void OneTimeTarget( TargetResponseCallback onTarget, CancelTargetCallback onCancel )
		{
			OneTimeTarget( false, onTarget, onCancel );
		}

		internal static void OneTimeTarget( bool ground, TargetResponseCallback onTarget, CancelTargetCallback onCancel )
		{
			if ( m_Intercept && m_OnCancel != null )
			{
				m_OnCancel();
				CancelOneTimeTarget();
			}

			if ( m_HasTarget && m_CurrentID != 0 )
			{
				m_PreviousID = m_CurrentID;
				m_PreviousGround = m_AllowGround;
				m_PrevFlags = m_CurFlags;

				m_FilterCancel.Add( m_PreviousID );
			}

			m_Intercept = true;
			m_CurrentID = LocalTargID;
			m_OnTarget = onTarget;
			m_OnCancel = onCancel;

			m_ClientTarget = m_HasTarget = true;
			ClientCommunication.SendToClient( new Target( LocalTargID, ground ) );
			ClearQueue();
		}

		internal static void CancelOneTimeTarget()
		{
			m_ClientTarget = m_HasTarget = false;

			ClientCommunication.SendToClient( new CancelTarget( LocalTargID ) );
			EndIntercept();
		}

		private static bool m_LTWasSet;
		public static void TargetSetLastTarget()
		{
			if ( World.Player != null )
			{
				m_LTWasSet = false;
				OneTimeTarget( false, new TargetResponseCallback( OnSetLastTarget ), new CancelTargetCallback( OnSLTCancel ) );
				World.Player.SendMessage( MsgLevel.Force, LocString.TargSetLT );
			}
		}

		private static void OnSLTCancel()
		{
			if ( m_LastTarget != null )
				m_LTWasSet = true;
		}

		private static void OnSetLastTarget( bool location, Serial serial, Point3D p, ushort gfxid )
		{
			if ( serial == World.Player.Serial )
			{
				OnSLTCancel();
				return;
			}

			m_LastBeneTarg = m_LastHarmTarg = m_LastTarget = new TargetInfo();
			m_LastTarget.Flags = 0;
			m_LastTarget.Gfx = gfxid;
			m_LastTarget.Serial = serial;
			m_LastTarget.Type = (byte)(location ? 1 : 0);
			m_LastTarget.X = p.X;
			m_LastTarget.Y = p.Y;
			m_LastTarget.Z = p.Z;

			m_LTWasSet = true;

			World.Player.SendMessage( MsgLevel.Force, LocString.LTSet );
			if ( serial.IsMobile )
			{
				LastTargetChanged();
				ClientCommunication.SendToClient( new ChangeCombatant( serial ) );
				m_LastCombatant = serial;
			}
		}

		private static Serial m_OldLT = Serial.Zero;

		private static void LastTargetChanged()
		{
			if ( m_LastTarget != null )
			{
				bool lth = Config.GetInt( "LTHilight" ) != 0;
				Mobile m = World.FindMobile( m_LastTarget.Serial );

				if ( m != null )
				{
					if ( IsLastTarget( m ) && lth )
						ClientCommunication.SendToClient( new MobileIncoming( m ) );
					CheckLastTargetRange( m );
				}

				if ( lth )
				{
					m = World.FindMobile( m_OldLT );
					if ( m != null )
						ClientCommunication.SendToClient( new MobileIncoming( m ) );
				}

				m_OldLT = m_LastTarget.Serial;
			}
		}

		public static bool LTWasSet{ get{ return m_LTWasSet; } }

		public static void TargetNonFriendly()
		{
			RandomTarget( 3, 4, 5, 6 );
		}

		public static void TargetFriendly()
		{
			RandomTarget( 0, 1, 2 );
		}

		public static void TargetEnemy()
		{
			RandomTarget( 5 );
		}

		public static void TargetEnemyHuman()
		{
			RandomHumanTarget( 5 );
		}

		public static void TargetRed()
		{
			RandomTarget( 6 );
		}

		public static void TargetGrey()
		{
			RandomTarget( 3, 4 );
		}

		public static void TargetGreyHuman()
		{
			RandomHumanTarget( 3, 4 );
		}

		public static void TargetCriminal()
		{
			RandomTarget( 4 );
		}

		public static void TargetCriminalHuman()
		{
			RandomHumanTarget( 4 );
		}

		public static void TargetInnocent()
		{
			RandomTarget( 1 );
		}

		public static void TargetInnocentHuman()
		{
			RandomHumanTarget( 1 );
		}

		public static void TargetAnyone()
		{
			RandomTarget();
		}

		public static void RandomTarget( params int[] noto )
		{
			if ( !ClientCommunication.AllowBit( FeatureBit.RandomTargets ) )
				return;

			ArrayList list = new ArrayList();
			foreach ( Mobile m in World.MobilesInRange( 12 ) )
			{
				if ( ( !FriendsAgent.IsFriend( m ) || ( noto.Length > 0 && noto[0] == 0 ) ) && 
					!m.Blessed && !m.IsGhost && m.Serial != World.Player.Serial &&
					Utility.InRange( World.Player.Position, m.Position, Config.GetInt( "LTRange" ) ) )
				{
					for(int i=0;i<noto.Length;i++)
					{
						if ( noto[i] == m.Notoriety )
						{
							list.Add( m );
							break;
						}
					}

					if ( noto.Length == 0 )
						list.Add( m );
				}
			}

			if ( list.Count > 0 )
				SetLastTargetTo( (Mobile)list[Utility.Random( list.Count )] );
			else
				World.Player.SendMessage( MsgLevel.Warning, LocString.TargNoOne );
		}

		public static void RandomHumanTarget( params int[] noto )
		{
			if ( !ClientCommunication.AllowBit( FeatureBit.RandomTargets ) )
				return;

			ArrayList list = new ArrayList();
			foreach ( Mobile m in World.MobilesInRange( 12 ) )
			{
				if ( m.Body < 0x0190 || m.Body > 0x0193 )
					continue;

				if ( ( !FriendsAgent.IsFriend( m ) || ( noto.Length > 0 && noto[0] == 0 ) ) && 
					!m.Blessed && !m.IsGhost && m.Serial != World.Player.Serial &&
					Utility.InRange( World.Player.Position, m.Position, Config.GetInt( "LTRange" ) ) )
				{
					for(int i=0;i<noto.Length;i++)
					{
						if ( noto[i] == m.Notoriety )
						{
							list.Add( m );
							break;
						}
					}

					if ( noto.Length == 0 )
						list.Add( m );
				}
			}

			if ( list.Count > 0 )
				SetLastTargetTo( (Mobile)list[Utility.Random( list.Count )] );
			else
				World.Player.SendMessage( MsgLevel.Warning, LocString.TargNoOne );
		}

		public static void SetLastTargetTo( Mobile m )
		{
			SetLastTargetTo( m, 0 );
		}

		public static void SetLastTargetTo( Mobile m, byte flagType )
		{
			TargetInfo targ = new TargetInfo();
			m_LastTarget = targ;

			if ( ( m_HasTarget && m_CurFlags == 1 ) || flagType == 1 )
				m_LastHarmTarg = targ;
			else if ( ( m_HasTarget && m_CurFlags == 2 ) || flagType == 2 )
				m_LastBeneTarg = targ;
			else if ( flagType == 0 )
				m_LastHarmTarg = m_LastBeneTarg = targ;

			targ.Type = 0;
			if ( m_HasTarget )
				targ.Flags = m_CurFlags;
			else
				targ.Flags = flagType;

			targ.Gfx = m.Body;
			targ.Serial = m.Serial;
			targ.X = m.Position.X;
			targ.Y = m.Position.Y;
			targ.Z = m.Position.Z;

			ClientCommunication.SendToClient( new ChangeCombatant( m ) );
			m_LastCombatant = m.Serial;
			World.Player.SendMessage( MsgLevel.Force, LocString.NewTargSet );
			
			bool wasSmart = Config.GetBool( "SmartLastTarget" );
			if ( wasSmart )
				Config.SetProperty( "SmartLastTarget", false );
			LastTarget();
			if ( wasSmart )
				Config.SetProperty( "SmartLastTarget", true );
			LastTargetChanged();
		}

		private static void EndIntercept()
		{
			m_Intercept = false;
			m_OnTarget = null;
			m_OnCancel = null;
		}

		public static void TargetSelf()
		{
			TargetSelf( false );
		}

		public static void TargetSelf( bool forceQ )
		{
			if ( World.Player == null )
				return;

			//if ( Macros.MacroManager.AcceptActions )
			//	MacroManager.Action( new TargetSelfAction() );

			if ( m_HasTarget )
			{
				if ( !DoTargetSelf() )
					ResendTarget();
			}
			else if ( forceQ || Config.GetBool( "QueueTargets" ) )
			{
				if ( !forceQ )
					World.Player.OverheadMessage( LocString.QueuedTS );
				m_QueueTarget = TargetSelfAction;
			}
		}

		public static bool DoTargetSelf()
		{
			if ( World.Player == null )
				return false;

			if ( CheckHealPoisonTarg( m_CurrentID, World.Player.Serial ) )
				return false;

			CancelClientTarget();
			m_HasTarget = false;

			if ( m_Intercept )
			{
				TargetInfo targ = new TargetInfo();
				targ.Serial = World.Player.Serial;
				targ.Gfx = World.Player.Body;
				targ.Type = 0;
				targ.X = World.Player.Position.X;
				targ.Y = World.Player.Position.Y;
				targ.Z = World.Player.Position.Z;
				targ.TargID = LocalTargID;
				targ.Flags = 0;
			
				OneTimeResponse( targ );
			}
			else
			{
				ClientCommunication.SendToServer( new TargetResponse( m_CurrentID, World.Player ) );
			}

			return true;
		}

		public static void LastTarget()
		{
			LastTarget( false );
		}
		
		public static void LastTarget( bool forceQ )
		{
			//if ( Macros.MacroManager.AcceptActions )
			//	MacroManager.Action( new LastTargetAction() );

			if ( m_HasTarget )
			{
				if ( !DoLastTarget() )
					ResendTarget();
			}
			else if ( forceQ || Config.GetBool( "QueueTargets" ) )
			{
				if ( !forceQ )
					World.Player.OverheadMessage( LocString.QueuedLT );
				m_QueueTarget = LastTargetAction;
			}
		}

		public static bool DoLastTarget()
		{
			TargetInfo targ;
			if ( Config.GetBool( "SmartLastTarget" ) && ClientCommunication.AllowBit( FeatureBit.SmartLT ) )
			{
				if ( m_CurFlags == 1 )
					targ = m_LastHarmTarg;
				else if ( m_CurFlags == 2 )
					targ = m_LastBeneTarg;
				else
					targ = m_LastTarget;

				if ( targ == null )
					targ = m_LastTarget;
			}
			else
			{
				targ = m_LastTarget;
			}

			if ( targ == null )
				return false;

			Point3D pos = Point3D.Zero;
			if ( targ.Serial.IsMobile )
			{
				Mobile m = World.FindMobile( targ.Serial );
				if ( m != null )
				{
					pos = m.Position;

					targ.X = pos.X;
					targ.Y = pos.Y;
					targ.Z = pos.Z;
				}
				else
				{
					pos = Point3D.Zero;
				}
			}
			else if ( targ.Serial.IsItem )
			{
				Item i = World.FindItem( targ.Serial );
				if ( i != null )
				{
					pos = i.GetWorldPosition();
						
					targ.X = i.Position.X;
					targ.Y = i.Position.Y;
					targ.Z = i.Position.Z;
				}
				else
				{
					pos = Point3D.Zero;
				}
			}
			else
			{
				if ( !m_AllowGround && ( targ.Serial == Serial.Zero || targ.Serial >= 0x80000000 ) )
				{
					World.Player.SendMessage( MsgLevel.Warning, LocString.LTGround );
					return false;
				}
				else
				{
					pos = new Point3D( targ.X, targ.Y, targ.Z );
				}
			}

			if ( Config.GetBool( "RangeCheckLT" ) && ClientCommunication.AllowBit( FeatureBit.RangeCheckLT ) && ( pos == Point3D.Zero || !Utility.InRange( World.Player.Position, pos, Config.GetInt( "LTRange" ) ) ) )
			{
				if ( Config.GetBool( "QueueTargets" ) )
					m_QueueTarget = LastTargetAction;
				World.Player.SendMessage( MsgLevel.Warning, LocString.LTOutOfRange );
				return false;
			}

			if ( CheckHealPoisonTarg( m_CurrentID, targ.Serial ) )
				return false;
			
			CancelClientTarget();
			m_HasTarget = false;

			targ.TargID = m_CurrentID;

			if ( m_Intercept )
				OneTimeResponse( targ );
			else
				ClientCommunication.SendToServer( new TargetResponse( targ ) );
			return true;
		}

		public static void ClearQueue()
		{
			m_QueueTarget = null;
		}

		private static TimerCallbackState m_OneTimeRespCallback = new TimerCallbackState( OneTimeResponse );

		private static void OneTimeResponse( object state )
		{
			TargetInfo info = state as TargetInfo;

			if ( info != null )
			{
				if ( ( info.X == 0xFFFF && info.X == 0xFFFF ) && ( info.Serial == 0 || info.Serial >= 0x80000000 ) )
				{
					if ( m_OnCancel != null )
						m_OnCancel();
				}
				else
				{	
					if ( Macros.MacroManager.AcceptActions )
						MacroManager.Action( new AbsoluteTargetAction( info ) );

					if ( m_OnTarget != null )
						m_OnTarget( info.Type == 1 ? true : false, info.Serial, new Point3D( info.X, info.Y, info.Z ), info.Gfx );
				}
			}

			EndIntercept();
		}

		private static void CancelTarget()
		{
			OnClearQueue();
			if ( m_ClientTarget )
				CancelClientTarget();
			
			if ( m_HasTarget )
			{
				ClientCommunication.SendToServer( new TargetCancelResponse( m_CurrentID ) );
				m_HasTarget = false;
			}
		}

		private static void CancelClientTarget()
		{
			if ( m_ClientTarget )
			{
				m_FilterCancel.Add( (uint)m_CurrentID );
				ClientCommunication.SendToClient( new CancelTarget( m_CurrentID ) );
				m_ClientTarget = false;
			}
		}

		public static void Target( TargetInfo info )
		{
			m_HasTarget = false;
			if ( m_Intercept )
			{
				OneTimeResponse( info );
			}
			else
			{
				info.TargID = m_CurrentID;
				m_LastTarget = info;
				ClientCommunication.SendToServer( new TargetResponse( info ) );
			}
		}

		public static void Target( Point3D pt )
		{
			TargetInfo info = new TargetInfo();
			info.Type = 1;
			info.Flags = 0;
			info.Serial = 0;
			info.X = pt.X;
			info.Y = pt.Y;
			info.Z = pt.Z;
			info.Gfx = 0;

			Target( info );
		}

		public static void Target( Point3D pt, ushort gfx )
		{
			TargetInfo info = new TargetInfo();
			info.Type = 1;
			info.Flags = 0;
			info.Serial = 0;
			info.X = pt.X;
			info.Y = pt.Y;
			info.Z = pt.Z;
			info.Gfx = gfx;

			Target( info );
		}

		public static void Target( Serial s )
		{
			TargetInfo info = new TargetInfo();
			info.Type = 0;
			info.Flags = 0;
			info.Serial = s;

			if ( s.IsItem )
			{
				Item item = World.FindItem( s );
				if ( item != null )
				{
					info.X = item.Position.X;
					info.Y = item.Position.Y;
					info.Z = item.Position.Z;
					info.Gfx = item.ItemID;
				}
			}
			else if ( s.IsMobile )
			{
				Mobile m = World.FindMobile( s );
				if ( m != null )
				{
					info.X = m.Position.X;
					info.Y = m.Position.Y;
					info.Z = m.Position.Z;
					info.Gfx = m.Body;
				}
			}

			Target( info );
		}

		public static void Target( object o )
		{
			if ( o is Item )
			{
				Item item = (Item)o;
				TargetInfo info = new TargetInfo();
				info.Type = 0;
				info.Flags = 0;
				info.Serial = item.Serial;
				info.X = item.Position.X;
				info.Y = item.Position.Y;
				info.Z = item.Position.Z;
				info.Gfx = item.ItemID;
				Target( info );
			}
			else if ( o is Mobile )
			{
				Mobile m = (Mobile)o;
				TargetInfo info = new TargetInfo();
				info.Type = 0;
				info.Flags = 0;
				info.Serial = m.Serial;
				info.X = m.Position.X;
				info.Y = m.Position.Y;
				info.Z = m.Position.Z;
				info.Gfx = m.Body;
				Target( info );
			}
			else if ( o is Serial )
			{
				Target( (Serial)o );
			}
			else if ( o is TargetInfo )
			{
				Target( (TargetInfo)o );
			}
		}

		public static void CheckTextFlags( Mobile m )
		{
			if ( Config.GetBool( "SmartLastTarget" ) && ClientCommunication.AllowBit( FeatureBit.SmartLT ) )
			{
				bool harm = m_LastHarmTarg != null && m_LastHarmTarg.Serial == m.Serial;
				bool bene = m_LastBeneTarg != null && m_LastBeneTarg.Serial == m.Serial;
				if ( harm )
					m.OverheadMessage( 0x90, "[Harmful Target]" );
				if ( bene )
					m.OverheadMessage( 0x3F, "[Beneficial Target]" );
			}

			if ( m_LastTarget != null && m_LastTarget.Serial == m.Serial )
				m.OverheadMessage( 0x3B2, "[Current Target]" );
		}

		public static bool IsLastTarget( Mobile m )
		{
			if ( m != null )
			{
				if ( Config.GetBool( "SmartLastTarget" ) && ClientCommunication.AllowBit( FeatureBit.SmartLT ) )
				{
					if ( m_LastHarmTarg != null && m_LastHarmTarg.Serial == m.Serial )
						return true;
				}
				else
				{
					if ( m_LastTarget != null && m_LastTarget.Serial == m.Serial )
						return true;
				}
			}

			return false;
		}

		private static int m_NextTargIdx = 0;
		public static void NextTarget()
		{
			ArrayList list = World.MobilesInRange( 12 );
			TargetInfo targ = new TargetInfo();
			Mobile m = null, old = World.FindMobile( m_LastTarget == null ? Serial.Zero : m_LastTarget.Serial );

			if ( list.Count <= 1 )
			{
				World.Player.SendMessage( MsgLevel.Warning, LocString.TargNoOne );
				return;
			}
			
			for (int i=0;i<3;i++)
			{
				m_NextTargIdx++;

				if ( m_NextTargIdx >= list.Count )
					m_NextTargIdx = 0;

				m = (Mobile)list[m_NextTargIdx];

				if ( m != null && m != World.Player && m != old )
					break;
				else
					m = null;
			}

			if ( m == null )
			{
				World.Player.SendMessage( MsgLevel.Warning, LocString.TargNoOne );
				return;
			}

			m_LastTarget = targ;

			m_LastHarmTarg = m_LastBeneTarg = targ;

			if ( m_HasTarget )
				targ.Flags = m_CurFlags;
			else
				targ.Type = 0;

			targ.Gfx = m.Body;
			targ.Serial = m.Serial;
			targ.X = m.Position.X;
			targ.Y = m.Position.Y;
			targ.Z = m.Position.Z;

			ClientCommunication.SendToClient( new ChangeCombatant( m ) );
			m_LastCombatant = m.Serial;
			World.Player.SendMessage( MsgLevel.Force, LocString.NewTargSet );

			/*if ( m_HasTarget )
			{
				DoLastTarget();
				ClearQueue();
			}*/
		}

		public static void CheckLastTargetRange( Mobile m )
		{
			if ( m_HasTarget && m != null && m_LastTarget != null && m.Serial == m_LastTarget.Serial && m_QueueTarget == LastTargetAction )
			{
				if ( Config.GetBool( "RangeCheckLT" ) && ClientCommunication.AllowBit( FeatureBit.RangeCheckLT ) )
				{
					if ( Utility.InRange( World.Player.Position, m.Position, Config.GetInt( "LTRange" ) ) )
					{
						if ( m_QueueTarget() )
							ClearQueue();
					}
				}
			}
		}
		
		private static void TargetResponse( PacketReader p, PacketHandlerEventArgs args )
		{
			TargetInfo info = new TargetInfo();
			info.Type = p.ReadByte();
			info.TargID = p.ReadUInt32();
			info.Flags = p.ReadByte();
			info.Serial = p.ReadUInt32();
			info.X = p.ReadUInt16();
			info.Y = p.ReadUInt16();
			info.Z = p.ReadInt16();
			info.Gfx = p.ReadUInt16();

			m_ClientTarget = false;

			//if ( !Config.GetBool( "SmartLastTarget" ) || m_LastTarget == null || ( info.Flags != 0 && info.Flags == m_LastTarget.Flags ) )
				ClearQueue();

			if ( m_Intercept )
			{
				if ( info.TargID == LocalTargID )
				{
					Timer.DelayedCallbackState( TimeSpan.Zero, m_OneTimeRespCallback, info ).Start();

					m_HasTarget = false;
					args.Block = true;
					
					if ( m_PreviousID != 0 )
					{
						m_CurrentID = m_PreviousID;
						m_AllowGround = m_PreviousGround;
						m_CurFlags = m_PrevFlags;

						m_PreviousID = 0;

						ResendTarget();
					}

					m_FilterCancel.Clear();
					
					return;
				}
				else
				{
					EndIntercept();
				}
			}

			if ( info.X != 0xFFFF || info.X != 0xFFFF || info.Serial.IsValid ) 
			{
				m_HasTarget = false;

				if ( CheckHealPoisonTarg( m_CurrentID, info.Serial ) )
				{
					ResendTarget();
					args.Block = true;
				}
				
				if ( info.Serial != World.Player.Serial )
				{
					m_LastTarget = info;
					if ( info.Flags == 1 )
						m_LastHarmTarg = info;
					else if ( info.Flags == 2 )
						m_LastBeneTarg = info;

					LastTargetChanged();

					if ( Macros.MacroManager.AcceptActions )
						MacroManager.Action( new AbsoluteTargetAction( info ) );
				}
				else 
				{
					if ( Macros.MacroManager.AcceptActions )
					{
						KeyData hk = HotKey.Get( (int)LocString.TargetSelf );
						if ( hk != null )
							MacroManager.Action( new HotKeyAction( hk ) );
						else
							MacroManager.Action( new AbsoluteTargetAction( info ) );
					}
				}
			}
			else
			{
				if ( m_FilterCancel.Contains( (uint)info.TargID ) || info.TargID == LocalTargID )
					args.Block = true;
				else
					m_ClientTarget = m_HasTarget = false;
			}

			m_FilterCancel.Clear();
		}

		private static bool CheckHealPoisonTarg( uint targID, Serial ser )
		{
			if ( targID == m_SpellTargID && ser.IsMobile && ( World.Player.LastSpell == Spell.ToID( 1, 4 ) || World.Player.LastSpell == Spell.ToID( 4, 5 ) ) && Config.GetBool( "BlockHealPoison" ) && ClientCommunication.AllowBit( FeatureBit.BlockHealPoisoned ) )
			{
				Mobile m = World.FindMobile( ser );

				if ( m != null && m.Poisoned )
				{
					World.Player.SendMessage( MsgLevel.Warning, LocString.HealPoisonBlocked );
					return true;
				}
			}

			return false;
		}

		private static void NewTarget( PacketReader p, PacketHandlerEventArgs args )
		{
			bool prevAllowGround = m_AllowGround;
			uint prevID = m_CurrentID;
			byte prevFlags = m_CurFlags;
			bool prevClientTarget = m_ClientTarget;

			m_AllowGround = p.ReadBoolean(); // allow ground
			m_CurrentID = p.ReadUInt32(); // target uid
			m_CurFlags = p.ReadByte(); // flags
			// the rest of the packet is 0s

			if ( Spell.LastCastTime + TimeSpan.FromSeconds( 3.0 ) > DateTime.Now && Spell.LastCastTime + TimeSpan.FromSeconds( 0.5 ) <= DateTime.Now && m_SpellTargID == 0 )
				m_SpellTargID = m_CurrentID;

			m_HasTarget = true;
			m_ClientTarget = false;
			
			if ( m_QueueTarget == null && Macros.MacroManager.AcceptActions && MacroManager.Action( new WaitForTargetAction() ) )
			{
				args.Block = true;
			}
			else if ( m_QueueTarget != null && m_QueueTarget() )
			{
				ClearQueue();
				args.Block = true;
			}

			if ( args.Block )
			{
				if ( prevClientTarget )
				{
					m_AllowGround = prevAllowGround;
					m_CurrentID = prevID;
					m_CurFlags = prevFlags;

					m_ClientTarget = true;

					if ( !m_Intercept )
						CancelClientTarget();
				}
			}
			else
			{
				m_ClientTarget = true;

				if ( m_Intercept )
				{
					if ( m_OnCancel != null )
						m_OnCancel();
					EndIntercept();
					World.Player.SendMessage( MsgLevel.Error, LocString.OTTCancel );

					m_FilterCancel.Add( (uint)prevID );
				}
			}
		}

		public static void ResendTarget()
		{
			if ( !m_ClientTarget || !m_HasTarget )
			{
				CancelClientTarget();
				m_ClientTarget = m_HasTarget = true;
				ClientCommunication.SendToClient( new Target( m_CurrentID, m_AllowGround, m_CurFlags ) );
			}
		}
	}
}
