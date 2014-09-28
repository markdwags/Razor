//#define LOG_CONTROL_TEXT

using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using Ultima;

namespace Assistant
{
	#region Localization enum
	//1044060 = Alchemy in UO cliloc
	public enum LocString : int
	{
		Null = 0,

		__Start = 1000,
		DeathStatus,
		BardMusic,
		DogSounds,
		CatSounds,
		HorseSounds,
		SheepSounds,
		SS_Sound,
		FizzleSound,
		Weather,
		DispCounters,
		RecountCounters,
		ClearAbility,
		SetPrimAb,
		SetSecAb,
		ToggleStun,
		ToggleDisarm,
		SettingAOSAb,
		AOSAbCleared,
		UndressAll,
		UndressHands,
		UndressLeft,
		UndressRight,
		UndressHat,
		UndressJewels,
		BandageSelf,
		BandageLT,
		UseBandage,
		DrinkHeal,
		DrinkCure,
		DrinkRef,
		DrinkNS,
		DrinkExp,
		DrinkStr,
		DrinkAg,
		NoBandages,
		NoItemOfType,
		DClickA1,
		ReTarget,
		Conv2DCT,
		SelTargAct,
		ProfileLoadEx,
		MacroItemOutRange,
		LiftA10,
		ConvLiftByType,
		MacroNoHold,
		EquipTo,
		DropA2,
		ConvRelLoc,
        DropRelA3,
		GumpRespB,//1050
		CloseGump,
		MenuRespA1,
		MacroNoTarg,
		AbsTarg,
		ConvLT,
		ConvTargType,
		TargRelLocA3,
		LastTarget,
		TargetSelf,
		SetLT,
		TargRandRed,
		TargRandGrey,
		TargRandBlue,
		TargRandEnemy,
		TargRandFriend,
		TargRandNFriend,
		SayQA1,
		UseSkillA1,
		CastSpellA1,
		SetAbilityA1,
		DressA1,
		UndressA1,
		UndressLayerA1,
		WaitAnyMenu,
		WaitMenuA1,
		Edit,
		WaitAnyGump,
		WaitGumpA1,
		WaitTarg,
		PauseA1,
		WaitA3,
		LoadingA1,
		StopCurrent,
		PlayA1,
		PlayingA1,
		MacroFinished,
		InitError,
		IE_1,
		IE_2,
		IE_3,
		IE_4,
		IE_5,
		IE_6, // InitError.NO_PATCH
		IE_7,
		NoMemCpy,
		SkillChanged,
		InvalidAbrev,
		InvalidIID,
		InvalidHue,
		SelItem2Count, // 1100
		ProfileCorrupt,
		NoProp,
		CounterFux,
		NoAutoCount,
		CountLow,
		SelHue,
		TakeSS,
		TitleBarTip,
		RazorStatus1,
		SetSLUp,
		SetSLDown,
		SetSLLocked,
		ConfirmDelCounter,
		ProfLoadQ,
		ProfLoadE,
		NoDelete,
		EnterProfileName,
		ProfExists,
		DressName,
		DelDressQ,
		OutOfRangeA1,
		DelDressItemQ,
		TargUndressBag,
		UB_Set,
		ItemNotFound,
		KeyUsed,
		SaveOK,
		PacketLogWarn,
		Conv2Type,
		NewMacro,
		InvalidChars,
		MacroExists,
		MacroConfRec,
		DelConf,
		InsWait,
		InsLT,
		MoveUp,
		MoveDown,
		RemAct,
		BeginRec,
		FileNotFoundA1,
		Confirm,
		FileDelError,
		NextRestart,
		PWWarn,
		NeedPort,
		QueueIgnore,
		ActQueued,
		QueueFinished,
		UseOnceAgent, // 1150
		AddTarg,
		AddContTarg,
		RemoveTarg,
		ClearList,
		TargItemAdd,
		TargCont,
		TargItemRem,
		ItemAdded,
		ItemRemoved,
		ItemsAdded,
		UseOnceEmpty,
		UseOnceStatus,
		UseOnceError,
		SellTotals,
		Remove,
		ContSet,
		Clear,
		PushDisable,
		PushEnable,
		SetHB,
		ClearHB,
		OrganizerAgent,
		Organizer,
		OrganizeNow,
		ContNotSet,
		NoBackpack,
		OrgQueued,
		OrgNoItems,
		ItemExists,
		AutoSearchEx,
		BuyTotals,
		BuyLowGold,
		EnterAmount,
		PrioSet,
		CurTime,
		CurLoc,
		UndressBagRange,
		UndressQueued,
		DressQueued,
		AlreadyDressed,
		ItemsNotFound,
		NoSpells,
		StealthSteps,
		StealthStart,
		ClearTargQueue,
		AttackLastComb,
		TQCleared,
		TargSetLT,
		LTSet,
		NewTargSet,// 1200
		TargNoOne,
		QueuedTS,
		QueuedLT,
		OTTCancel,
		TargByType,
		Scavenger,
		DeleteConfirm,
		SelSSFolder,
		HotKeys,

		// 1210 - 1248 reserved for hotkey categories
		HKSubOffset = 1249,
		// 1252 - 1300 reserved for hotkey sub-categories

		Sell = 1301,
		Buy,
		InputReq,
		CommandList,
		UseHand,
		MustDisarm,
		ArmDisarmRight,
		ArmDisarmLeft,
		DropCur,
		SpellWeaving,
		ToggleHKEnable,
		HKEnabledPress,
		HKDisabledPress,
		HKEnabled,
		HKDisabled,
		WalkA1,
		AddTargType,
		LTOutOfRange,
		SellAmount,
		SnoopFilter,
		PackSound,
		InsIF,
		InsELSE,
		InsENDIF,
		SetAmt,
		Restock,
		RestockNow,
		RestockAgent,
		RestockTarget,
		InvalidCont,
		RestockDone,
		CancelTarget,
		NewerVersion,
		Reload,
		Save,
		LTGround,
		Resync,
		DismountBlocked,
		RecStart,
		VidStop,
		RecError,
		WrongVer,
		VideoCorrupt,
		ReadError,
		CatName,
		CantDelDir,
		CanCreateDir,
		CantMoveMacro,
		Friends,

		// 1350 to 1359 reserved for PacketPlayer stop messages
		PacketPlayerStop = 1350,
		PacketPlayerStopLast = 1359,

		TargFriendAdd,
		TargFriendRem,
		FriendAdded,
		FriendRemoved,
		Constructs,
		InsFOR,
		InsENDFOR,
		NumIter,
		ClearScavCache,
		LastSpell,
		LastSkill,
		LastObj,
		AllNames,
		UseOnce,
		TargRandEnemyHuman,
		TargRandGreyHuman,
		TargRandInnocentHuman,
		TargRandCriminalHuman,
		TargRandCriminal,
		ForceEndHolding,
		RestartClient,
		RelogRequired,
		LiftQueued,
		RestockQueued,
		StrChanged,
		DexChanged,
		IntChanged,
		LightFilter,
		HealPoisonBlocked,
		ClearDragDropQueue,
		StopNow,
		HealOrCureSelf,
		NextTarget,
		SetRestockHB,
		AddUseOnce,
		AttackLastTarg,
		EditTimeout,
		NoHold,
		NotAllowed,
		Allowed,
		// 1400 to 1465 reserved for negotiation features
		NegotiateTitle = 1400,
		AllFeaturesEnabled,
		FeatureDescBase,

		NextCliloc = 1466,
		FeatureDisabled = NextCliloc,
		FeatureDisabledText,
		InsComment,
		AddFriend,
		RemoveFriend,
		MiniHealOrCureSelf,
		NoPatchWarning,
		Dismount,
		ToggleMap,
		TooFar,
		DragDropQueueFull,
		ToggleWarPeace,
		VetRewardGump,
		ForceSizeBad,
		ScavengerHB,
		RestockHBA1,
		UseOnceHBA1,
		SellHB,
		OrganizerHBA1,
		BadServerAddr,
		PartyAccept,
		PartyDecline,
		HarmfulTarget,
		BeneficialTarget,
		RazorFriend,
		PlayFromHere,
		Initializing,
		LoadingLastProfile,
		LoadingClient,
		WaitingForClient,
		RememberDonate,
		Welcome,
		Auto2D,
		Auto3D,
		AutoDetect, // 1500
		OpacityA1,
		NewTimeout,
		NotAssigned,
		ChangeTimeout,
		EnterAName,
		Invalid,
		StaffOnlyItems,
		sStatsA1,
		DrinkApple,

		TargCloseRed,
		TargCloseGrey,
		TargCloseBlue,
		TargCloseEnemy,
		TargCloseFriend,
		TargCloseNFriend,
		TargCloseEnemyHuman,
		TargCloseGreyHuman,
		TargCloseInnocentHuman,
		TargCloseCriminalHuman,
		TargCloseCriminal,

		ProfileInUse,
		WaitingTimeout,
		NoCliLocMsg,
		NoCliLoc,
		BOD,
		LaunchBODAgent,

		NextTargetHumanoid,

		__End
	}
	#endregion

	public class Language
	{
		private static Hashtable m_Controls;
		private static Hashtable m_Strings;
		private static Ultima.StringList m_CliLoc = null;
		private static bool m_Loaded = false;
		private static string m_Current;
		private static string m_CliLocName = "ENU";

		public static bool Loaded { get{ return m_Loaded; } }
		public static string Current { get{ return m_Current; } }
		public static string CliLocName { get{ return m_CliLocName; } }

		public static string GetControlText( string name )
		{
			name = String.Format( "{0}::Text", name );
			if ( m_Controls.ContainsKey( name ) )
				return m_Controls[name] as string;
			else
				return null;
		}

		static Language()
		{
			m_Controls = new Hashtable(32, 1.0f, StringComparer.OrdinalIgnoreCase);
			m_Strings = new Hashtable( (int)(LocString.__End - LocString.__Start) + 1, 1.0f );
		}

		public static string GetString( LocString key )
		{
			string value = m_Strings[key] as string;
			if ( value == null )
				value = String.Format( "LanguageString \"{0}\" not found!", key );//throw new MissingFieldException( String.Format( "Razor requested Language Pack string '{0}', but it does not exist in the current language pack.", key ) );
			return value;
		}

		public static string GetString( int key )
		{
			string value = null;
			if (key > (uint)LocString.__Start && key < (uint)LocString.__End)
				value = m_Strings[(LocString)key] as string;
			else if (m_CliLoc != null)
				value = m_CliLoc.GetString(key);
			
			if ( value == null )
				value = String.Format( "LanguageString \"{0}\" not found!", key );
			return value;
		}

		public static string Format( int key, params object[] args )
		{
			return String.Format( GetString( key ), args );
		}

		public static string Format( LocString key, params object[] args )
		{
			return String.Format( GetString( key ), args );
		}

		public static string Skill2Str( SkillName sk )
		{
			return Skill2Str( (int)sk );
		}

		public static string Skill2Str( int skill )
		{
			string value = null;
			if (m_CliLoc != null)
				value = m_CliLoc.GetString(1044060 + skill);
			if ( value == null )
				value = String.Format( "LanguageString \"{0}\" not found!", 1044060+skill );
			return value;
		}

		public static string[] GetPackNames()
		{
			string path = Config.GetInstallDirectory( "Language" );
			string[] names = Directory.GetFiles( path, "Razor_lang.*" );
			for(int i=0;i<names.Length;i++)
				names[i] = Path.GetExtension( names[i] ).ToUpper().Substring( 1 );
			return names;
		}
		
		public static bool Load( string lang )
		{
			lang = lang.ToUpper();
			if ( m_Current != null && m_Current == lang )
				return true;

			m_CliLocName = "enu";
			string filename = Path.Combine( Config.GetInstallDirectory( "Language" ), String.Format( "Razor_lang.{0}", lang ) );
			if ( !File.Exists( filename ) )
				return false;
			m_Current = lang;
			ArrayList errors = new ArrayList();
			Encoding encoding = Encoding.ASCII;

			using ( StreamReader reader = new StreamReader( filename ) )
			{
				string line;
				while ( (line=reader.ReadLine()) != null )
				{
					line = line.Trim();
					string lower = line.ToLower();

					if ( line == "" || line[0] == '#' || line[0] == ';' || ( line.Length >= 2 && line[0] == '/' && line[1] == '/' ) )
						continue;
					else if ( lower == "[controls]" || lower == "[strings]" )
						break;
					
					if ( lower.StartsWith( "::encoding" ) )
					{
						try
						{
							int idx = lower.IndexOf( '=' )+1;
							if ( idx > 0 && idx < lower.Length )
								encoding = Encoding.GetEncoding( line.Substring( idx ).Trim() );
						}
						catch
						{
							encoding = null;
						}

						if ( encoding == null )
						{
							MessageBox.Show( "Error: The encoding specified in the language file was not valid.  Using ASCII.", "Invalid Encoding", MessageBoxButtons.OK, MessageBoxIcon.Error );
							encoding = Encoding.ASCII;
						}
						break;
					}
				}
			}

			using ( StreamReader reader = new StreamReader( filename, encoding ) )
			{
				//m_Dict.Clear(); // just overwrite the old lang, rather than erasing it (this way if this lang is missing something, it'll appear in the old one
				int lineNum = 0;
				string line;
				bool controls = true;
				int idx = 0;

				while ( (line=reader.ReadLine()) != null )
				{
					try
					{
						line = line.Trim();
						lineNum++;
						if ( line == "" || line[0] == '#' || line[0] == ';' || ( line.Length >= 2 && line[0] == '/' && line[1] == '/' ) )
							continue;
						string lower = line.ToLower();
						if ( lower == "[controls]" )
						{
							controls = true;
							continue;
						}
						else if ( lower == "[strings]" )
						{
							controls = false;
							continue;
						}
						else if ( lower.StartsWith( "::cliloc" ) )
						{
							idx = lower.IndexOf( '=' )+1;
							if ( idx > 0 && idx < lower.Length )
								m_CliLocName = lower.Substring( idx ).Trim().ToUpper();
							continue;
						}
						else if ( lower.StartsWith( "::encoding" ) )
						{
							continue;
						}

						idx = line.IndexOf( '=' );
						if ( idx < 0 )
						{
							errors.Add( lineNum );
							continue;
						}

						string key = line.Substring( 0, idx ).Trim();
						string value = line.Substring( idx+1 ).Trim().Replace( "\\n", "\n" );

						if ( controls )
							m_Controls[key] = value;
						else
							m_Strings[(LocString)Convert.ToInt32( key )] = value;
					}
					catch
					{
						errors.Add( lineNum );
					}
				}//while
			}//using

			if ( errors.Count > 0 )
			{
				StringBuilder sb = new StringBuilder();

				sb.AppendFormat( "Razor enountered errors on the following lines while loading the file '{0}'\r\n", filename );
				for(int i=0;i<errors.Count;i++)
					sb.AppendFormat( "Line {0}\r\n", errors[i] );
				
				new MessageDialog( "Language Pack Load Errors", true, sb.ToString() ).Show();
			}

			LoadCliLoc();

			m_Loaded = true;
			return true;
		}

		public static void LoadCliLoc()
		{
			if ( m_CliLocName == null || m_CliLocName.Length <= 0 )
				m_CliLocName = "enu";
            
			try
			{
				m_CliLoc = new Ultima.StringList( m_CliLocName.ToLower() );
			}
			catch ( Exception e )
			{
				string fileName = "[CliLoc]";
				try
				{
					fileName = Ultima.Files.GetFilePath( String.Format( "cliloc.{0}", m_CliLocName ) );
				}
				catch{}

				new MessageDialog( "Error loading CliLoc", true, "There was an exception while attempting to load '{0}':\n{1}", fileName, e ).ShowDialog( Engine.ActiveWindow );
			}

			if ( m_CliLoc == null || m_CliLoc.Entries == null || m_CliLoc.Entries.Count < 10 )	
			{
				m_CliLoc = null;
				if ( m_CliLocName != "enu" )
				{
					m_CliLocName = "enu";
					LoadCliLoc();
					return;
				}
				else
				{
					MessageBox.Show( Engine.ActiveWindow, Language.GetString( LocString.NoCliLocMsg ), Language.GetString( LocString.NoCliLoc ), MessageBoxButtons.OK, MessageBoxIcon.Warning );
				}
			}
		}

		public static string GetCliloc( int num )
		{
			if (m_CliLoc == null)
				return String.Empty;

			StringEntry se = m_CliLoc.GetEntry(num);

			if ( se != null )
				return se.Format();
			else
				return string.Empty;
		}

		public static string ClilocFormat( int num, string argstr )
		{
			if (m_CliLoc == null)
				return String.Empty;

			StringEntry se = m_CliLoc.GetEntry(num);

			if (se != null)
				return se.SplitFormat(argstr);
			else
				return string.Empty;
		}

		public static string ClilocFormat( int num, params object[] args )
		{
			if (m_CliLoc == null)
				return String.Empty;

			StringEntry se = m_CliLoc.GetEntry(num);

			if (se != null)
				return se.Format(args);
			else
				return string.Empty;
		}

		private static void LoadControls( string name, System.Windows.Forms.Control.ControlCollection controls )
		{
			if ( controls == null )
				return;

			for(int i=0;i<controls.Count;i++)
			{
				string find = String.Format( "{0}::{1}", name, controls[i].Name );
				string str = m_Controls[find] as string;
				if ( str != null )
					controls[i].Text = str;

				if ( controls[i] is ListView )
				{
					foreach ( ColumnHeader ch in ((ListView)controls[i]).Columns )
					{
						find = String.Format( "{0}::{1}::{2}", name, controls[i].Name, ch.Index );
						str = m_Controls[find] as string;
						if ( str != null )
							ch.Text = str;
					}
				}

				LoadControls( name, controls[i].Controls );
			}
		}

		public static void LoadControlNames( System.Windows.Forms.Form form )
		{
#if LOG_CONTROL_TEXT
			DumpControls( form );
#endif

			LoadControls( form.Name, form.Controls );
			string text = m_Controls[String.Format( "{0}::Text", form.Name )] as string;
			if ( text != null )
				form.Text = text;

			if ( form is MainForm )
				((MainForm)form).UpdateTitle();
		}

#if LOG_CONTROL_TEXT
		public static void DumpControls( System.Windows.Forms.Form form )
		{
			using ( StreamWriter w = new StreamWriter( form.Name+".controls.txt" ) )
			{
				w.WriteLine( "{0}::Text={1}", form.Name, form.Text );
				Dump( form.Name, form.Controls, w );
			}
		}

		private static void Dump( string name, System.Windows.Forms.Control.ControlCollection ctrls, StreamWriter w )
		{
			for(int i=0;ctrls != null && i<ctrls.Count;i++)
			{
				if ( !(ctrls[i] is System.Windows.Forms.TextBox) && !(ctrls[i] is System.Windows.Forms.ComboBox) )
				{
					if ( ctrls[i].Text.Length > 0 )
						w.WriteLine( "{0}::{1}={2}", name, ctrls[i].Name, ctrls[i].Text );
					if ( ctrls[i] is ListView )
					{
						foreach ( ColumnHeader ch in ((ListView)ctrls[i]).Columns )
							w.WriteLine( "{0}::{1}::{2}={3}", name, ctrls[i].Name, ch.Index, ch.Text );
					}
				}

				Dump( name, ctrls[i].Controls, w );
			}
		}
#endif
	}
}

