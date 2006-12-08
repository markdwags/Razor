using System;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using Microsoft.Win32;
using Assistant.Filters;

namespace Assistant
{
	public class Profile
	{
		private string m_Name;
		private Hashtable m_Props;

		public Profile( string name )
		{
			Name = name;
			m_Props = new Hashtable( 16, 1.0f, CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default );

			MakeDefault();
		}

		public void MakeDefault()
		{
			m_Props.Clear();

			AddProperty( "ShowMobNames", false );
			AddProperty( "ShowCorpseNames", false );
			AddProperty( "DisplaySkillChanges", false );
			AddProperty( "TitleBarText", @"UO - {char} {crimtime}- {mediumstatbar} {bp} {bm} {gl} {gs} {mr} {ns} {ss} {sa} {aids}" );
			AddProperty( "TitleBarDisplay", true );
			AddProperty( "AutoSearch", true );
			AddProperty( "NoSearchPouches", true );
			AddProperty( "CounterWarnAmount", (int)5 );
			AddProperty( "CounterWarn", true );
			AddProperty( "ObjectDelay", (int)600 );
			AddProperty( "AlwaysOnTop", false );
			AddProperty( "SortCounters", true );
			AddProperty( "QueueActions", true );
			AddProperty( "QueueTargets", true );
			AddProperty( "WindowX", (int)400 );
			AddProperty( "WindowY", (int)400 );
			AddProperty( "CountStealthSteps", true );
			AddProperty( "AlwaysStealth", false );

			AddProperty( "SysColor", (int)0x03B1 );
			AddProperty( "WarningColor", (int)0x0025 );
			AddProperty( "ExemptColor", (int)0x0480 );
			AddProperty( "SpeechHue", (int)0x03B1 );
			AddProperty( "BeneficialSpellHue", (int)0x0005 );
			AddProperty( "HarmfulSpellHue", (int)0x0025 );
			AddProperty( "NeutralSpellHue", (int)0x03B1 );
			AddProperty( "ForceSpeechHue", false );
			AddProperty( "ForceSpellHue", false );
			AddProperty( "SpellFormat", @"{power} [{spell}]" );

			AddProperty( "ShowNotoHue", true );
			AddProperty( "Opacity", (int)100 );

			AddProperty( "AutoOpenCorpses", false );
			AddProperty( "CorpseRange", (int)2 );

			AddProperty( "FilterSpam", false );
			AddProperty( "BlockDismount", false );

			AddProperty( "AutoCap", false );
			AddProperty( "CapFullScreen", false );
			AddProperty( "CapPath", Path.Combine( Engine.BaseDirectory, "ScreenShots" ) );
			AddProperty( "CapTimeStamp", true );
			AddProperty( "ImageFormat", "jpg" );

			AddProperty( "UndressConflicts", true );
			AddProperty( "HighlightReagents", true );
			AddProperty( "Systray", true );
			AddProperty( "TitlebarImages", true );

			AddProperty( "SellAgentMax", (int)99 );
			AddProperty( "SkillListCol", (int)-1 );
			AddProperty( "SkillListAsc", false );

			AddProperty( "AutoStack", false );
			AddProperty( "ActionStatusMsg", true );
			AddProperty( "RememberPwds", false );

			AddProperty( "SpellUnequip", false );
			AddProperty( "RangeCheckLT", true );
			AddProperty( "LTRange", (int)12 );

			AddProperty( "ClientPrio", "Normal" );
			AddProperty( "FilterSnoopMsg", true );
			AddProperty( "OldStatBar", false );

			AddProperty( "SmartLastTarget", false );
			AddProperty( "LastTargTextFlags", true );
			AddProperty( "SmartCPU", true );
			AddProperty( "LTHilight", (int)0 );

			AddProperty( "RecFolder", Path.Combine( Engine.BaseDirectory, "Videos" ) );
			AddProperty( "AviFPS", 15 );
			AddProperty( "AviRes", 1 );

			AddProperty( "AutoFriend", false );

			AddProperty( "AutoOpenDoors", false );

			AddProperty( "MessageLevel", 0 );

			AddProperty( "ForceIP", "" );
			AddProperty( "ForcePort", 0 );

			AddProperty( "ForceSizeEnabled", false );
			AddProperty( "ForceSizeX", 800 );
			AddProperty( "ForceSizeY", 600 );

			AddProperty( "FlipVidH", false );
			AddProperty( "FlipVidV", false );

			AddProperty( "PotionEquip", false );
			AddProperty( "BlockHealPoison", false );

			AddProperty( "SmoothWalk", false );

			AddProperty( "Negotiate", true );

			AddProperty( "MapX", 200 );
			AddProperty( "MapY", 200 );
			AddProperty( "MapW", 200 );
			AddProperty( "MapH", 200 );

			AddProperty( "LogPacketsByDefault", false );

			Counter.Default();
			Filter.DisableAll();
			DressList.ClearAll();
			HotKey.ClearAll();
			Agent.ClearAll();
			PasswordMemory.ClearAll();
		}

		public string Name
		{
			get 
			{ 
				return m_Name; 
			}
			set 
			{ 
				if ( value != null && value.Trim() != "" )
				{
					StringBuilder sb = new StringBuilder( value );
					sb.Replace( '\\', '_' );
					sb.Replace( '/', '_' );
					sb.Replace( '\"', '\'' );
					sb.Replace( ':', '_' );
					sb.Replace( '?', '_' );
					sb.Replace( '*', '_' );
					sb.Replace( '<', '(' );
					sb.Replace( '>', ')' );
					sb.Replace( '|', '_' );
					m_Name = sb.ToString();
				}
				else
				{
					m_Name = "[No Name]";
				}
			}
		}

		public bool Load()
		{
			if ( m_Name == null || m_Name.Trim() == "" )
				return false;

			string path = Engine.GetDirectory( "Profiles" );
			string file = Path.Combine( path, String.Format( "{0}.xml", m_Name ) );
			if ( !File.Exists( file ) )
				return false;

			XmlDocument doc = new XmlDocument();
			try
			{
				doc.Load( file );
			}
			catch
			{
				MessageBox.Show( Engine.ActiveWindow, Language.Format( LocString.ProfileCorrupt, file ), "Profile Load Error", MessageBoxButtons.OK, MessageBoxIcon.Stop );
				return false;
			}

			XmlElement root = doc["profile"];
			if ( root  == null )
				return false;

			Assembly exe = Assembly.GetCallingAssembly();
			if ( exe == null )
				return false;

			foreach( XmlElement el in root.GetElementsByTagName( "property" ) )
			{
				try
				{
					string name = el.GetAttribute( "name" );
					string typeStr = el.GetAttribute( "type" );
					string val = el.InnerText;

					if ( typeStr == "-null-" || name == "LimitSize" || name == "VisRange" )
					{
						//m_Props[name] = null;
						if ( m_Props.ContainsKey( name ) )
							m_Props.Remove( name );
					}
					else
					{
						Type type = Type.GetType( typeStr );
						if ( type == null )
							type = exe.GetType( typeStr );

						if ( m_Props.ContainsKey( name ) || name == "ForceSize" )
						{
							if ( type == null )
								m_Props.Remove( name );
							else
								m_Props[name] = GetObjectFromString( val, type );
						}
					}
				}
				catch ( Exception e )
				{
					MessageBox.Show( Engine.ActiveWindow, Language.Format( LocString.ProfileLoadEx, e.ToString() ), "Profile Load Exception", MessageBoxButtons.OK, MessageBoxIcon.Warning );
				}
			}

			Filter.Load( root["filters"] );
			Counter.LoadProfile( root["counters"] );
			Agent.LoadProfile( root["agents"] );
			DressList.Load( root["dresslists"] );
			HotKey.Load( root["hotkeys"] );
			PasswordMemory.Load( root["passwords"] );

			if ( m_Props.ContainsKey( "ForceSize" ) )
			{
				try
				{
					int x, y;
					switch ( (int)m_Props["ForceSize"] )
					{
						case 1: x = 960;	y = 600;	break;
						case 2: x = 1024;	y = 768;	break;
						case 3: x = 1152;	y = 864;	break;
						case 4: x = 1280;	y = 720;	break;
						case 5: x = 1280;	y = 768;	break;
						case 6: x = 1280;	y = 800;	break;
						case 7: x = 1280;	y = 960;	break;
						case 8: x = 1280;	y = 1024;	break;

						case 0:
						default:x = 800;	y = 600;	break;
					}

					SetProperty( "ForceSizeX", x );
					SetProperty( "ForceSizeY", y );

					if ( x != 800 || y != 600 )
						SetProperty( "ForceSizeEnabled", true );

					m_Props.Remove( "ForceSize" );
				}
				catch
				{
				}
			}

			//if ( !Language.Load( GetString( "Language" ) ) )
			//	MessageBox.Show( Engine.ActiveWindow, "Warning: Could not load language from profile, using current language instead.", "Language Error", MessageBoxButtons.OK, MessageBoxIcon.Warning );

			return true;
		}

		public void Save()
		{
			string profileDir = Engine.GetDirectory( "Profiles" );
			string file = Path.Combine( profileDir, String.Format( "{0}.xml", m_Name ) );

			XmlTextWriter xml;
			try
			{
				xml= new XmlTextWriter( file, Encoding.Default );
			}
			catch
			{
				return;
			}

			xml.Formatting = Formatting.Indented;
			xml.IndentChar = '\t';
			xml.Indentation = 1;

			xml.WriteStartDocument( true );
			xml.WriteStartElement( "profile" );
				
			foreach( DictionaryEntry de in m_Props )
			{
				xml.WriteStartElement( "property" );
				xml.WriteAttributeString( "name", (string)de.Key );
				if ( de.Value == null )
				{
					xml.WriteAttributeString( "type", "-null-" );
				}
				else
				{
					xml.WriteAttributeString( "type", de.Value.GetType().FullName );
					xml.WriteString( de.Value.ToString() );
				}
				xml.WriteEndElement();
			}

			xml.WriteStartElement( "filters" );
			Filter.Save( xml );
			xml.WriteEndElement();

			xml.WriteStartElement( "counters" );
			Counter.SaveProfile( xml );
			xml.WriteEndElement();

			xml.WriteStartElement( "agents" );
			Agent.SaveProfile( xml );
			xml.WriteEndElement();

			xml.WriteStartElement( "dresslists" );
			DressList.Save( xml );
			xml.WriteEndElement();

			xml.WriteStartElement( "hotkeys" );
			HotKey.Save( xml );
			xml.WriteEndElement();

			xml.WriteStartElement( "passwords" );
			PasswordMemory.Save( xml );
			xml.WriteEndElement();

			xml.WriteEndElement(); // end profile section

			xml.Close();
		}

		private static Type[] ctorTypes = new Type[]{ typeof( string ) };
		private object GetObjectFromString( string val, Type type )
		{
			if ( type == typeof( string ) )
			{
				return val;
			}
			else
			{
				try
				{
					ConstructorInfo ctor = type.GetConstructor( ctorTypes );
					if ( ctor != null )
						return ctor.Invoke( new object[]{ val } );
				}
				catch
				{
				}

				return Convert.ChangeType( val, type );
			}
		}

		public object GetProperty( string name )
		{
			if ( !m_Props.ContainsKey( name ) )
				throw new Exception( Language.Format( LocString.NoProp, name ) );
			return m_Props[name];
		}

		private Timer m_SaveTimer = null;

		public void SetProperty( string name, object val )
		{
			if ( !m_Props.ContainsKey( name ) )
				throw new Exception( Language.Format( LocString.NoProp, name ) );
			m_Props[name] = val;

			if ( m_SaveTimer == null )
				m_SaveTimer = Timer.DelayedCallback( TimeSpan.FromMinutes( 1.0 ), new TimerCallback( Save ) );
			if ( m_SaveTimer.Running )
				m_SaveTimer.Stop();
			m_SaveTimer.Start();
		}

		public void AddProperty( string name, object val )
		{
			if ( !m_Props.Contains( name ) )
			{
				m_Props.Add( name, val );
				ClientCommunication.AddProperty( name );
			}
		}
	}

	public class Config
	{
		private static Profile m_Current;
		private static Hashtable m_Chars;

		public static Profile CurrentProfile
		{
			get
			{
				if ( m_Current == null )
					LoadLastProfile();
				return m_Current;
			}
		}

		public static void Save()
		{
			if ( m_Current != null )
				m_Current.Save();
			SaveCharList();
		}

		public static bool LoadProfile( string name )
		{
			Profile p = new Profile( name );
			if ( p.Load() )
			{
				LastProfileName = p.Name;
				m_Current = p;
				return true;
			}
			else
			{
				return false;
			}
		}

		public static void NewProfile( string name )
		{
			m_Current = new Profile( name );
		}

		public static void LoadCharList()
		{
			if ( m_Chars == null )
				m_Chars = new Hashtable();
			else
				m_Chars.Clear();

			string file = Path.Combine( Engine.GetDirectory( "Profiles" ), "chars.lst" );
			if ( !File.Exists( file ) )
				return;

			using ( StreamReader reader = new StreamReader( file ) )
			{
				string line;
				while ( (line=reader.ReadLine()) != null )
				{
					if ( line.Length <= 0 || line[0] == ';' || line[0] == '#' )
						continue;
					string[] split = line.Split( '=' );
					try
					{
						m_Chars.Add( Serial.Parse( split[0] ), split[1] );
					}
					catch
					{
					}
				}
			}
		}

		public static void SaveCharList()
		{
			if ( m_Chars == null )
				m_Chars = new Hashtable();

			try
			{
				using ( StreamWriter writer = new StreamWriter( Path.Combine( Engine.GetDirectory( "Profiles" ), "chars.lst" ) ) )
				{
					foreach ( DictionaryEntry de in m_Chars )
					{
						writer.WriteLine( "{0}={1}", de.Key, de.Value );
					}
				}
			}
			catch
			{
			}
		}

		public static void LoadProfileFor( PlayerData player )
		{
			if ( m_Chars == null )
				m_Chars = new Hashtable();
			string prof = m_Chars[player.Serial] as string;
			if ( m_Current != null && prof != null && ( m_Current.Name == prof || m_Current.Name.Trim() == prof.Trim() ) )
				return;

			if ( prof != null )
			{
				Save();

				if ( !LoadProfile( prof ) )
				{
					if ( prof != "default" )
					{
						if ( !LoadProfile( "default" ) )
							m_Current.MakeDefault();
					}
					else
					{
						m_Current.MakeDefault();
					}
				}

				Engine.MainWindow.InitConfig();
			}
			else
			{
				m_Chars.Add( player.Serial, m_Current != null ? m_Current.Name : "default" );
			}
			Engine.MainWindow.SelectProfile( m_Current == null ? "default" : m_Current.Name );
		}

		public static void SetProfileFor( PlayerData player )
		{
			if ( m_Current != null )
				m_Chars[player.Serial] = m_Current.Name;
		}
		
		public static bool LoadLastProfile()
		{
			string name = LastProfileName;
			bool failed = true;
			Profile p = null;

			if ( name != null )
			{
				p = new Profile( name );
				failed = !p.Load();
			}
			
			if ( failed )
			{
				if ( p == null )
					p = new Profile( "default" );
				else
					p.Name = "default";

				if ( !p.Load() )
				{
					p.MakeDefault();
					p.Save();
				}
				LastProfileName = "default";
			}

			m_Current = p;
			return !failed;
		}

		public static void SetupProfilesList( ComboBox list, string selectName )
		{
			if ( list == null || list.Items == null )
				return;

			string[] files = Directory.GetFiles( Engine.GetDirectory( "Profiles" ), "*.xml" );
			string compare = String.Empty;
			if ( selectName != null )
				compare = selectName.ToLower();
			
			for(int i=0;i<files.Length;i++)
			{
				string name = Path.GetFileNameWithoutExtension( files[i] );
				if ( name == null || name.Length <= 0 )
					name = files[i];
				if ( name == null )
					continue;

				list.Items.Add( name );
				if ( name.ToLower() == compare )
					list.SelectedIndex = i;
			}
		}

		private const string RazorRegPath = @"SOFTWARE\Razor";
		public static string GetRegString( Microsoft.Win32.RegistryKey hkey, string vname )
		{
			try
			{
				RegistryKey key = hkey.OpenSubKey( RazorRegPath ) ;
				if ( key == null )
				{
					key = hkey.CreateSubKey( RazorRegPath );
					if ( key == null )
						return null;
				}

				string v = key.GetValue( vname ) as string;

				if ( v == null )
					return null;
				return v.Trim();
			}
			catch
			{
				return null;
			}
		}

		public static object GetProperty( string name )
		{
			return CurrentProfile.GetProperty( name );
		}

		public static void SetProperty( string name, object val )
		{
			CurrentProfile.SetProperty( name, val );
		}

		public static void AddProperty( string name, object val )
		{
			CurrentProfile.AddProperty( name, val );
		}

		public static bool GetBool( string name )
		{
			return (bool)CurrentProfile.GetProperty( name );
		}

		public static string GetString( string name )
		{
			return (string)CurrentProfile.GetProperty( name );
		}

		public static int GetInt( string name )
		{
			return (int)CurrentProfile.GetProperty( name );
		}

		public static bool SetRegString( RegistryKey hkey, string vname, string value )
		{
			if ( vname == null || value == null )
				return false;

			try
			{
				RegistryKey key = hkey.OpenSubKey( RazorRegPath, true );
				if ( key == null )
					key = hkey.CreateSubKey( RazorRegPath );

				key.SetValue( vname, value );

				key.Close();

				return true;
			}
			catch
			{
				if ( hkey != Registry.CurrentUser )
					return SetRegString( Registry.CurrentUser, vname, value );
				else
					return false;
			}
		}

		public static void DeleteRegValue( Microsoft.Win32.RegistryKey hkey, string vname )
		{
			using ( RegistryKey key = hkey.OpenSubKey( RazorRegPath, true ) )
			{
				key.DeleteValue( vname, false );
			}
		}

		public static string BaseDirectory
		{
			get
			{
				string inst = GetRegString( Microsoft.Win32.Registry.LocalMachine, "InstallDir" );

				try
				{
					if ( inst == null || inst == "" )
					{
						inst = Directory.GetCurrentDirectory();
						if ( inst != null )
							SetRegString( Microsoft.Win32.Registry.LocalMachine, "InstallDir", inst );
					}
				}
				catch
				{
				}
				
				return inst;
			}
		}

		public static string LastProfileName
		{
			get
			{
				return GetRegString( Microsoft.Win32.Registry.CurrentUser, "LastProfile" );
			}
			set
			{
				SetRegString( Microsoft.Win32.Registry.CurrentUser, "LastProfile", value );
			}
		}
	}
}

