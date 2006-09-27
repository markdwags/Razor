using System;
using System.IO;
using System.Collections;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Text;

namespace Assistant
{
	public class PacketPlayer
	{
		public static bool Playing { get { return m_Playing; } }
		public static bool Recording { get { return m_Recording; } }

		private static bool m_Recording = false;
		private static bool m_Playing = false;

		private static BinaryWriter m_TempWriter;
		private static GZBlockIn m_GZIn;
		private static GZBlockOut m_GZOut;
		private static DateTime m_LastTime, m_StartTime;
		private static Hashtable m_HouseDataWritten = new Hashtable();

		private static TimerCallback m_SendNext = new TimerCallback( SendNextPacket );
		private static TimerCallback m_BeginPlay = new TimerCallback( BeginPlayback );
		private static TimerCallback m_EndPlay = new TimerCallback( EndPlayback );

		private static TimeSpan FadeDelay = TimeSpan.FromSeconds( 3.0 );
		private const byte PlayerVersion = 4;

		private static Label lblPlay;
		private static TrackBar tbPos;
		private static Button btnRec, btnPlay, btnStop, btnClose;

		private static Timer m_PlayTimer, m_ScrollTimer;
		private static int m_StartPos;
		private static TimeSpan m_Elapsed;
		private static int m_PlaySpeed = 2;
		private static byte m_Version;

		private static string m_RPVInfo;
		public static string CurrentOpenedInfo { get { return m_RPVInfo; } }

		public static void SetControls( Label play, Button bRec, Button bPlay, Button stop, Button close, TrackBar pos )
		{
			lblPlay = play;
			btnRec = bRec;
			btnPlay = bPlay;
			btnStop = stop;
			btnClose = close;
			tbPos = pos;
		}

		public static void SetSpeed( int speed )
		{
			m_PlaySpeed = speed;
		}

		public static void Stop()
		{
			if ( m_Recording )
			{
				byte[] hash;

				// the final timestamp
				m_GZOut.Compressed.Write( (int)((DateTime.Now-m_LastTime).TotalMilliseconds) );
				m_GZOut.Compressed.Write( (byte)0xFF );

				m_GZOut.ForceFlush();
				m_GZOut.BufferAll = true;

				m_GZOut.RawStream.Seek( 1+16+8, SeekOrigin.Begin );
				m_GZOut.Raw.Write( (int)((DateTime.Now-m_StartTime).TotalMilliseconds) );

				m_GZOut.RawStream.Seek( 1+16, SeekOrigin.Begin );
				using ( MD5 md5 = MD5.Create() )
					hash = md5.ComputeHash( m_GZOut.RawStream );

				m_GZOut.RawStream.Seek( 1, SeekOrigin.Begin );
				m_GZOut.Raw.Write( hash );
				
				m_GZOut.RawStream.Flush();
				m_GZOut.Close();
				m_GZOut = null;

				m_Recording = false;
				btnRec.Text = "Record PacketVideo";
				btnPlay.Enabled = btnStop.Enabled = true;
			}
			else if ( Playing )
			{
				ClientCommunication.SetDeathMsg( Language.GetString( LocString.PacketPlayerStop + Utility.Random( 10 ) ) ); 
				ClientCommunication.ForceSendToClient( new DeathStatus( true ) );

				RemoveAll();

				if ( m_PlayTimer != null && m_PlayTimer.Running )
					m_PlayTimer.Stop();

				if ( m_ScrollTimer != null )
					m_ScrollTimer.Stop();

				m_PlayTimer = Timer.DelayedCallback( FadeDelay, m_EndPlay );
				m_PlayTimer.Start();
				
				btnPlay.Text = "Play";
				btnClose.Enabled = tbPos.Enabled = btnPlay.Enabled = btnStop.Enabled = false;
			}
		}

		public static void Record()
		{
			if ( m_Recording || Playing || World.Player == null )
				return;

			btnRec.Text = "Stop Recording (PV)";
			btnPlay.Enabled = btnStop.Enabled = false;

			m_HouseDataWritten.Clear();

			string filename;
			string name = "Unknown";
			string path = Config.GetString( "RecFolder" );
				
			if ( World.Player != null )
				name = World.Player.Name;
			if ( name == null || name.Trim() == "" || name.IndexOfAny( Path.InvalidPathChars ) != -1 )
				name = "Unknown";

			name = String.Format( "{0}_{1}", name, DateTime.Now.ToString( "M-d_HH.mm" ) );
			try
			{
				Engine.EnsureDirectory( path );
			}
			catch
			{
				try 
				{
					path = Engine.GetDirectory( "Videos" );
					Config.SetProperty( "RecFolder", path );
				}
				catch
				{
					path = "";
				}
			}
			int count  = 0;
			do
			{
				filename = Path.Combine( path, String.Format( "{0}{1}.rpv", name, count != 0 ? count.ToString() : ""  ) );
				count--; // cause a - to be put in front of the number 
			}
			while ( File.Exists( filename ) );

			m_Recording = true;
			m_StartTime = m_LastTime = DateTime.Now;

			try
			{
				//m_OutStream = new BinaryWriter( new FileStream( filename, FileMode.Create, FileAccess.ReadWrite, FileShare.None ) );
				m_GZOut = new GZBlockOut( filename, 2048 );
			
				m_GZOut.Raw.Write( (byte) PlayerVersion );
				m_GZOut.Raw.Seek( 16, SeekOrigin.Current ); // md5
				m_GZOut.Raw.Write( (long)m_StartTime.ToFileTime() );
				m_GZOut.Raw.Write( (int)(0) ); // length

				m_GZOut.BufferAll = true;
				m_GZOut.Compressed.Write( World.Player.Name );
				m_GZOut.Compressed.Write( World.ShardName );
				byte[] addr;
				try
				{
					addr = ClientCommunication.LastConnection.GetAddressBytes();
				}
				catch
				{
					addr = new byte[4]{0,0,0,0};
				}
				m_GZOut.Compressed.Write( addr, 0, 4 );
				SaveWorldState();
				m_GZOut.BufferAll = false;
				m_GZOut.Flush();
			}
			catch  ( Exception e )
			{
				MessageBox.Show( Engine.MainWindow, Language.GetString( LocString.RecError ), "Rec Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
				Engine.LogCrash( e );
			}
		}

		private static void SaveWorldState()
		{
			long start = m_GZOut.Position;
			m_GZOut.Compressed.Write( (int)0 ); // len

			World.Player.SaveState( m_GZOut.Compressed );
			foreach ( Mobile m in World.Mobiles.Values )
			{
				if ( m != World.Player )
				{
					m_GZOut.Compressed.Write( (byte)1 );
					m.SaveState( m_GZOut.Compressed );
				}
			}

			foreach ( Item i in World.Items.Values )
			{
				if ( !(i.Container is Item) )
				{
					m_GZOut.Compressed.Write( (byte)0 );
					i.SaveState( m_GZOut.Compressed );
					m_HouseDataWritten[i.Serial] = true;
				}
			}

			long end = m_GZOut.Position;

			m_GZOut.Seek( (int)start, SeekOrigin.Begin );
			m_GZOut.Compressed.Write( (int)( end - (start+4) ) );
			m_GZOut.Seek( 0, SeekOrigin.End );
		}

		public static bool ServerPacket( Packet p )
		{
			if ( Playing )
			{
				return false; // dont allow the client to recv server packets.
			}
			else if ( m_Recording && p != null )
			{
				if ( World.Player == null )
				{
					Stop();
					return true;
				}

				switch ( p.PacketID )
				{
					case 0x21: // Movement Reject
					case 0x27: // Lift Reject
					case 0x6C: // Target
					case 0x7C: // Menu
					case 0x88: // Display paper doll
					case 0xB2: // Chat Message
					case 0xBA: // Quest Arrow
						return true; 

					case 0x22: // Movement Ack
					{
						// replace movement ack with a force walk
						byte seq = p.ReadByte();
						if ( World.Player.HasWalkEntry( seq ) )
						{ 
							WritePacket( new ForceWalk( World.Player.GetMoveEntry( seq ).Dir&Direction.Mask ) );
							//WritePacket( new MobileUpdate( World.Player ) );
						}
						return true; 
					}

					case 0xBF:
					{
						short type = p.ReadInt16();
						switch ( type )
						{
							case 0x06:// party stuff
							{
								byte subType = p.ReadByte();

								if ( subType == 0x03 || subType == 0x04 )
								{
									Mobile from = World.FindMobile( p.ReadUInt32() );
									string msg = p.ReadUnicodeStringSafe();

									string send = String.Format( "[{0}]: {1}", from != null && from.Name != null && from.Name.Length > 0 ? from.Name : "Party", msg );
							
									WritePacket( new UnicodeMessage( Serial.MinusOne, 0, MessageType.System, 0x3b2, 3, "ENU", "Party", send ) );
								}
								return true;
							}
							case 0x1D: // House Revision info
							{
								Item i = World.FindItem( p.ReadUInt32() );
								if ( i != null )
								{
									i.HouseRevision = p.ReadInt32();

									if ( m_HouseDataWritten[i.Serial] == null )
									{
										if ( i.HousePacket == null )
											i.MakeHousePacket();
										if ( i.HousePacket != null )
										{
											m_HouseDataWritten[i.Serial] = true;
											// WritePacket( p );
											WritePacket( new Packet( i.HousePacket, i.HousePacket.Length, true ) );
											return true;
										}
									}
								}

								break;
							}
						}
						break;
					}
					case 0xD8: // Custom House data
					{
						p.ReadByte(); // Compression
						p.ReadByte(); // Unknown
						Serial s = p.ReadUInt32();
							
						m_HouseDataWritten[s] = true;
						break;
					}
				}

				WritePacket( p );
				return true;
			}
			else
			{
				return true;
			}
		}

		private static void WritePacket( Packet p )
		{
			int delay = (int)((DateTime.Now - m_LastTime).TotalMilliseconds);
			m_LastTime = DateTime.Now;

			m_GZOut.Compressed.Write( delay );
			m_GZOut.Compressed.Write( p.Compile() );
		}

		public static bool ClientPacket( Packet p )
		{
			if ( Playing )
			{
				if ( p == null || World.Player == null )
					return false;
				switch ( p.PacketID )
				{
					case 0x02: // Movement Req
					{
						Direction dir = (Direction)p.ReadByte();
						byte seq = p.ReadByte();

						ClientCommunication.ForceSendToClient( new MoveReject( seq, World.Player ) );
						World.Player.Resync();
						break;
					}
					case 0x06: // Double Click
					{
						Serial s = p.ReadUInt32();
						
						Mobile m;
						if ( (s.Value & 0x80000000) != 0 || s == World.Player.Serial )
							m = World.Player;
						else 
							m = World.FindMobile( s );

						if ( m != null )
						{
							string name = m.Name;
							if ( name == null || name == "" )
								name = "<No Data>";
							ClientCommunication.ForceSendToClient( new DisplayPaperdoll( m, name ) );
						}
						break;
					}
					case 0x07: // Lift Req
					{
						ClientCommunication.ForceSendToClient( new LiftRej( 5 ) ); //inspecific
						break;
					}
					case 0x09: // single click
					{
						Serial s = p.ReadUInt32();

						if ( s.IsMobile )
						{
							Mobile m = World.FindMobile( s );
							if ( m != null && m.Name != null && m.Name != "" )
							{
								int hue;
								switch ( m.Notoriety )
								{
									case 1: hue = 0x059; break;
									case 2: hue = 0x03F; break;
									case 3:
									case 4: hue = 0x3B2; break;
									case 5: hue = 0x090; break;
									case 6: hue = 0x022; break;
									case 7: hue = 0x035; break;
									default:hue = 0x481; break;
								}
								ClientCommunication.ForceSendToClient( new UnicodeMessage( s, m.Body, MessageType.Label, hue, 3, "ENU", "", m.Name ) ); 
							}
						}
						else if ( s.IsItem )
						{
							Item i = World.FindItem( s );
							if ( i != null && i.Name != null && i.Name != "" )
								ClientCommunication.ForceSendToClient( new UnicodeMessage( s, i.ItemID, MessageType.Label, 0x3B2, 3, "ENU", "", i.Name ) ); 
						}
						break;
					}
					case 0x34: // Mobile Info Query
					{
						p.ReadInt32(); // 0xEDEDEDED
						int type = p.ReadByte();
						Mobile m = World.FindMobile( p.ReadUInt32() );
						if ( m == null )
							break;

						switch ( type )
						{
							case 0x04: // Stats
							{
								if ( m == World.Player )
								{
									ClientCommunication.ForceSendToClient( new MobileStatusExtended( World.Player ) );
									ClientCommunication.ForceSendToClient( new StatLockInfo( World.Player ) );
								}
								else 
								{
									if ( m.Hits == 0 && ( m.HitsMax == 0 || m.HitsMax == 1 ) )
									{
										m.HitsMax = 1;
										if ( m.Name == null || m.Name == "" )
											m.Name = "<No Data>";
									}
									else if ( m.Name == null || m.Name == "" || m.Name == "<No Data>" )
									{
										m.Name = "<No Name>";
									}
									
									ClientCommunication.ForceSendToClient( new MobileStatusCompact( m ) );
								}
								break;
							}
							case 0x05:
							{
								ClientCommunication.ForceSendToClient( new SkillsList() );
								break;
							}
						}
						
						break;
					}
					case 0x6C: // target
					{
						p.ReadByte(); // type
						uint tid = p.ReadUInt32();
						p.ReadByte(); // flags
						Serial s = p.ReadUInt32();

						if ( tid == Targeting.LocalTargID )
							ClientCommunication.ForceSendToClient( new UnicodeMessage( 0xFFFFFFFF, -1, MessageType.Regular, 0x25, 3, Language.CliLocName, "System", String.Format( "Serial Number is {0}", s ) ) );
						break;
					}
					case 0xAD: // speech
					{
						PacketReader pvSrc = new PacketReader( p.Compile(), true );
						pvSrc.MoveToData();
						Command.OnSpeech( pvSrc, new PacketHandlerEventArgs() );
						break;
					}
					case 0xBF: // Extended Packet
					{
						ushort subType = p.ReadUInt16();

						switch ( subType )
						{
							case 0x1E: // QueryDesignDetails (custom house)
							{
								Item i = World.FindItem( p.ReadUInt32() );
								if ( i != null && i.HousePacket != null )
									ClientCommunication.ForceSendToClient( new Packet( i.HousePacket, i.HousePacket.Length, true ) );
								break;
							}
						}
						break;
					}
				}

				return false;
			}
			else if ( m_Recording )
			{
				if ( p == null )
					return true;

				switch ( p.PacketID )
				{
					case 0xB1: // gump response
						p.ReadInt32(); // skip serial

						WritePacket( new CloseGump( p.ReadUInt32() ) );
						break;
				}

				return true;
			}
			else 
			{
				return true;
			}
		}

		public static double SpeedScalar()
		{
			switch ( m_PlaySpeed )
			{
				case 2:  return 0.25;
				case 1:  return 0.50;
				case -1: return 2.00;
				case -2: return 4.00;
				default:
				case 0:  return 1.00;
			}
		}

		private static void SendNextPacket()
		{
			if ( !Playing )
				return;

			if ( World.Player == null )
			{
				btnPlay.Text = "Play";
				tbPos.Enabled = btnClose.Enabled = btnPlay.Enabled = btnStop.Enabled = btnRec.Enabled = true;
				tbPos.Value = tbPos.Minimum;

				if ( m_PlayTimer != null && m_PlayTimer.Running )
					m_PlayTimer.Stop();

				if ( m_ScrollTimer != null && m_ScrollTimer.Running )
					m_ScrollTimer.Stop();
				return;
			}

			int delay = 0;
			int totalDelay = 0;

			do 
			{
				// peek ahead 1 byte... and no, BinaryReader does not have a peek function.
				if ( m_GZIn.Compressed.ReadByte() == 0xFF ) 
					break;
				m_GZIn.Seek( -1, SeekOrigin.Current );

				ClientCommunication.ProcessPlaybackData( m_GZIn.Compressed );
				
				if ( !m_GZIn.EndOfFile )
					totalDelay += delay = m_GZIn.Compressed.ReadInt32();
			} while ( totalDelay*SpeedScalar() <= 1 && !m_GZIn.EndOfFile );

			m_Elapsed += TimeSpan.FromMilliseconds( totalDelay );
			//tbPos.Value = (int)m_Elapsed.TotalSeconds;

			if ( !m_GZIn.EndOfFile )
			{
				m_PlayTimer = Timer.DelayedCallback( TimeSpan.FromMilliseconds( delay*SpeedScalar() * 0.75  ), m_SendNext );
				m_PlayTimer.Start();
			}
			else
			{
				Stop();
			}
		}

		public static void OnScroll()
		{
			TimeSpan delay = TimeSpan.Zero;
			TimeSpan target = TimeSpan.FromSeconds( tbPos.Value );

			try
			{
				if ( !Playing )
				{
					tbPos.Value = tbPos.Minimum;
					return;
				}
				else if ( target <= m_Elapsed )
				{
					try
					{
						tbPos.Value = (int)m_Elapsed.TotalSeconds;
					}
					catch
					{
					}
					return;
				}
			}
			catch
			{
				return;
			}

			PlayerData.ExternalZ = false;

			int sleepCount = 0;
			while ( m_Elapsed < target && !m_GZIn.EndOfFile )
			{
				// peek ahead 1 byte... and no, BinaryReader doesnt have a peek function.
				byte peek = m_GZIn.Compressed.ReadByte();
				if ( peek == 0xFF ) 
					break;
				m_GZIn.Seek( -1, SeekOrigin.Current );

				ClientCommunication.ProcessPlaybackData( m_GZIn.Compressed );
				
				if ( !m_GZIn.EndOfFile )
				{
					delay = TimeSpan.FromMilliseconds( m_GZIn.Compressed.ReadInt32() );
					m_Elapsed += delay;

					if ( ((++sleepCount) % 5) == 0 )
						System.Threading.Thread.Sleep( TimeSpan.FromMilliseconds( 1 ) );
				}
			}

			try
			{
				tbPos.Value = (int)m_Elapsed.TotalSeconds;
			}
			catch
			{
			}

			ClientCommunication.ForceSendToClient( new MobileUpdate( World.Player ) );
			ClientCommunication.ForceSendToClient( new MobileIncoming( World.Player ) );

			if ( m_PlayTimer.Running ) // paused?
			{
				m_PlayTimer.Stop();
				if ( !m_GZIn.EndOfFile )
				{
					m_PlayTimer = Timer.DelayedCallback( delay, m_SendNext );
					m_PlayTimer.Start();
				}
				else
				{
					Stop();
				}
			}

			ClientCommunication.BeginCalibratePosition();
		}

		public static void Open( string filename )
		{
			if ( Playing )
				return;

			btnPlay.Enabled = btnStop.Enabled = false;
			if ( m_GZIn != null )
				m_GZIn.Close();
			try
			{
				m_GZIn = new GZBlockIn( filename );//new BinaryReader( new FileStream( filename, FileMode.Open, FileAccess.Read, FileShare.Read ) );

				m_Version = m_GZIn.Raw.ReadByte();
				
				if ( m_Version > PlayerVersion )
				{
					m_GZIn.Close();
					m_GZIn = null;
					MessageBox.Show( Engine.MainWindow, Language.GetString( LocString.WrongVer ), "Version Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Stop );
					return;
				}

				m_GZIn.IsCompressed = m_Version > 1;

				byte[] filehash = m_GZIn.Raw.ReadBytes( 16 );				
				DateTime created = DateTime.FromFileTime( m_GZIn.Raw.ReadInt64() );
				TimeSpan len = TimeSpan.FromMilliseconds( m_GZIn.Raw.ReadInt32() );

				string player = m_GZIn.Compressed.ReadString();
				string shard = m_GZIn.Compressed.ReadString();
				System.Net.IPAddress ip = System.Net.IPAddress.Any;
				try
				{
					if ( m_Version > 1 )
						ip = new System.Net.IPAddress( (long)m_GZIn.Compressed.ReadUInt32() );
				}
				catch
				{
				}

				m_StartPos = (int)m_GZIn.Position;
				
				long rawPos = m_GZIn.RawStream.Position;
				m_GZIn.RawStream.Seek( 1+16, SeekOrigin.Begin );
				using ( MD5 md5 = MD5.Create() )
				{
					byte[] check = md5.ComputeHash( m_GZIn.RawStream );
					for(int i=0;i<check.Length;i++)
					{
						if ( check[i] != filehash[i] )
						{
							m_GZIn.Close();
							m_GZIn = null;
							MessageBox.Show( Engine.MainWindow, Language.GetString( LocString.VideoCorrupt ), "Damaged File", MessageBoxButtons.OK, MessageBoxIcon.Error );
							return;
						}
					}
				}
				m_GZIn.RawStream.Seek( rawPos, SeekOrigin.Begin );

				m_RPVInfo = lblPlay.Text = String.Format( "File: {0}\nLength: {1} ({2})\nDate: {3}\nRecorded by \"{4}\" on \"{5}\" ({6})\n", Path.GetFileName( filename ), Utility.FormatTime( (int)len.TotalSeconds ), Utility.FormatSize( m_GZIn.RawStream.Length ), created.ToString( "M-dd-yy @ h:mmtt" ), player, shard, ip );
				btnClose.Enabled = btnPlay.Enabled = btnStop.Enabled = true;
				tbPos.Maximum = (int)len.TotalSeconds;
				tbPos.Minimum = 0;
			}
			catch ( Exception e )
			{
				if ( e is FileNotFoundException )
				{
					MessageBox.Show( Engine.MainWindow, e.Message, "File not found.", MessageBoxButtons.OK, MessageBoxIcon.Error );
				}
				else
				{
					Engine.LogCrash( e );
					MessageBox.Show( Engine.MainWindow, Language.GetString( LocString.ReadError ), "Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
				}
				m_GZIn.Close();
				m_GZIn = null;
				return;
			}
		}

		public static void Close()
		{
			if ( Playing )
				return;

			btnClose.Enabled = btnPlay.Enabled = btnStop.Enabled = false;
			if ( m_GZIn != null )
				m_GZIn.Close();
			m_GZIn = null;
			tbPos.Value = tbPos.Minimum;
			lblPlay.Text = "";
			m_RPVInfo = null;
		}

		public static void Play()
		{
			if ( m_Recording || Playing || m_GZIn == null )
				return;

			if ( World.Player == null )
			{
				MessageBox.Show( Engine.MainWindow, "You must be logged in to ANY shard to play a packet video.", "Must Log in", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			btnPlay.Enabled = btnStop.Enabled = btnClose.Enabled = btnRec.Enabled = false;
			btnPlay.Text = "Pause";
			
			// save the real player data
			m_TempWriter = new BinaryWriter( new MemoryStream() );
			World.Player.SaveState( m_TempWriter );

			//m_InStream = new BinaryReader( new FileStream( fileName, FileMode.Open, FileAccess.Read, FileShare.Read ) );

			m_Playing = true;
			ClientCommunication.SetAllowDisconn( false );

			ClientCommunication.BringToFront( ClientCommunication.FindUOWindow() );

			ClientCommunication.SetDeathMsg( "Playing..." );
			ClientCommunication.ForceSendToClient( new DeathStatus( true ) );

			RemoveAll();

			m_GZIn.Seek( m_StartPos, SeekOrigin.Begin );

			LoadWorldState();
			
			m_PlayTimer = Timer.DelayedCallback( FadeDelay, m_BeginPlay );
			m_PlayTimer.Start();
			tbPos.Value = tbPos.Minimum;
			m_Elapsed = TimeSpan.Zero;

			ClientCommunication.RequestTitlebarUpdate();
		}

		private static void LoadWorldState()
		{
			int end = m_GZIn.Compressed.ReadInt32();
			end += (int)m_GZIn.Position;

			try
			{
				World.AddMobile( World.Player = new PlayerData( m_GZIn.Compressed, m_Version ) );
				while ( m_GZIn.Position < end )
				{
					byte type = m_GZIn.Compressed.ReadByte();
					if ( type == 1 )
						World.AddMobile( new Mobile( m_GZIn.Compressed, m_Version ) );
					else if ( type == 0 )
						World.AddItem( new Item( m_GZIn.Compressed, m_Version ) );
				}
			}
			catch ( Exception e )
			{
				new MessageDialog( "Error Reading PacketVideo", true, e.ToString() ).ShowDialog( Engine.ActiveWindow );
			}

			foreach ( Mobile m in World.Mobiles.Values )
				m.AfterLoad();

			foreach ( Item i in World.Items.Values )
				i.AfterLoad();
		}

		public static void Pause()
		{
			if ( !Playing )
				return;

			if ( !m_PlayTimer.Running )
			{
				SendNextPacket();
				btnPlay.Text = "Pause";
			}
			else
			{
				m_PlayTimer.Stop();
				btnPlay.Text = "Play";
			}
		}

		private static void BeginPlayback()
		{
			DoLogin( World.Player );

			ClientCommunication.SetDeathMsg( "You are dead." );
			ClientCommunication.BringToFront( ClientCommunication.FindUOWindow() );
			
			TimeSpan delay = TimeSpan.FromMilliseconds( m_GZIn.Compressed.ReadInt32() );
			m_PlayTimer = Timer.DelayedCallback( delay, m_SendNext );
			m_PlayTimer.Start();
			if ( m_ScrollTimer == null )
				m_ScrollTimer = new ScrollTimer();
			m_ScrollTimer.Start();
			m_StartTime = DateTime.Now;
			m_Elapsed = delay;

			btnPlay.Enabled = btnStop.Enabled = true;
		}

		private static void EndPlayback()
		{
			m_PlayTimer = null;
			
			m_Playing = false;
			ClientCommunication.SetAllowDisconn( true );
			ClientCommunication.SetDeathMsg( "You are dead." );

			PlayerData player;
			using ( BinaryReader reader = new BinaryReader( m_TempWriter.BaseStream ) )
			{
				reader.BaseStream.Seek( 0, SeekOrigin.Begin );
				player = World.Player = new PlayerData( reader, PlayerVersion );
			}
			m_TempWriter.Close();

			player.Contains.Clear();
			World.AddMobile( player );

			DoLogin( player );

			tbPos.Enabled = btnClose.Enabled = btnPlay.Enabled = btnStop.Enabled = btnRec.Enabled = true;
			tbPos.Value = tbPos.Minimum;
			
			ClientCommunication.SendToClient( new MoveReject( World.Player.WalkSequence, World.Player ) );
			ClientCommunication.SendToServer( new ResyncReq() );
			World.Player.Resync();
			ClientCommunication.RequestTitlebarUpdate();
		}

		private static void RemoveAll()
		{
			/*foreach ( Mobile m in World.Mobiles.Values )
				ClientCommunication.ForceSendToClient( new RemoveObject( m ) );
			foreach ( Item i in World.Items.Values )
				ClientCommunication.ForceSendToClient( new RemoveObject( i ) );*/

			World.Mobiles.Clear();
			World.Items.Clear();

			ClientCommunication.OnLogout();
		}

		private static void DoLogin( PlayerData player )
		{
			PlayerData.ExternalZ = false;

			ClientCommunication.ForceSendToClient( new LoginConfirm( player ) );
			ClientCommunication.ForceSendToClient( new MapChange( player.Map ) );
			ClientCommunication.ForceSendToClient( new MapPatches( player.MapPatches ) );
			ClientCommunication.ForceSendToClient( new SeasonChange( player.Season, true ) );
			ClientCommunication.ForceSendToClient( new SupportedFeatures( player.Features ) );
			ClientCommunication.ForceSendToClient( new MobileUpdate( player ) );
			ClientCommunication.ForceSendToClient( new MobileUpdate( player ) );

			ClientCommunication.ForceSendToClient( new GlobalLightLevel( player.GlobalLightLevel ) );
			ClientCommunication.ForceSendToClient( new PersonalLightLevel( player ) );
			
			ClientCommunication.ForceSendToClient( new MobileUpdate( player ) );
			ClientCommunication.ForceSendToClient( new MobileIncoming( player ) );
			ClientCommunication.ForceSendToClient( new MobileAttributes( player ) );
			ClientCommunication.ForceSendToClient( new SetWarMode( player.Warmode ) );

			foreach ( Item i in World.Items.Values )
			{
				if ( i.Container == null )
				{
					ClientCommunication.ForceSendToClient( new WorldItem( i ) );
					if ( i.HouseRevision != 0 )
						ClientCommunication.ForceSendToClient( new DesignStateGeneral( i ) );
				}
			}

			foreach ( Mobile m in World.Mobiles.Values )
				ClientCommunication.ForceSendToClient( new MobileIncoming( m ) );

			ClientCommunication.ForceSendToClient( new SupportedFeatures( player.Features ) );

			ClientCommunication.ForceSendToClient( new MobileUpdate( player ) );
			ClientCommunication.ForceSendToClient( new MobileIncoming( player ) );
			ClientCommunication.ForceSendToClient( new MobileAttributes( player ) );
			ClientCommunication.ForceSendToClient( new SetWarMode( player.Warmode ) );
			ClientCommunication.ForceSendToClient( new MobileIncoming( player ) );

			ClientCommunication.ForceSendToClient( new LoginComplete() );
			ClientCommunication.ForceSendToClient( new CurrentTime() );

			ClientCommunication.ForceSendToClient( new SeasonChange( player.Season, true ) );
			ClientCommunication.ForceSendToClient( new MapChange( player.Map ) );
			ClientCommunication.ForceSendToClient( new MobileUpdate( player ) );
			ClientCommunication.ForceSendToClient( new MobileIncoming( player ) );

			PacketHandlers.PlayCharTime = DateTime.Now;
			
			ClientCommunication.BeginCalibratePosition();
		}

		private class ScrollTimer : Timer
		{
			private DateTime m_LastPing;
			public ScrollTimer() : base( TimeSpan.Zero, TimeSpan.FromSeconds( 1 ) )
			{
			}

			protected override void OnTick()
			{
				if ( Playing )
				{
					int val = (int)m_Elapsed.TotalSeconds + (int)(m_PlayTimer.Delay - m_PlayTimer.TimeUntilTick).TotalSeconds;
					if ( val > tbPos.Maximum )
						val = tbPos.Maximum;
					else if ( val < tbPos.Minimum )
						val = tbPos.Minimum;
					
					tbPos.Value = val;

					if ( (DateTime.Now-m_LastPing) >= TimeSpan.FromMinutes( 1 ) )
					{
						ClientCommunication.ForceSendToServer( new PingPacket( 0 ) );
						m_LastPing = DateTime.Now;
					}
				}
				else
				{
					Stop();
				}
			}
		}
	}
}
