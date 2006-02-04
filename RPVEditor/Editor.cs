using System;
using System.IO;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using Assistant;

namespace RPVEditor
{
	/// <summary>
	/// Summary description for Editor.
	/// </summary>
	public class Editor : System.Windows.Forms.Form
	{
		public static void Main( string[] args )
		{
			Application.Run( new Editor() );
		}

		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuFile;
		private System.Windows.Forms.MenuItem fileOpen;
		private System.Windows.Forms.MenuItem fileClose;
		private System.Windows.Forms.MenuItem sep1;
		private System.Windows.Forms.MenuItem fileSave;
		private System.Windows.Forms.MenuItem sep2;
		private System.Windows.Forms.MenuItem fileQuit;
		private System.Windows.Forms.ListBox evtList;
		private System.Windows.Forms.Label rpvInfo;
		private System.Windows.Forms.Button setBeg;
		private System.Windows.Forms.Button setEnd;
		private System.Windows.Forms.TrackBar begPos;
		private System.Windows.Forms.TrackBar endPos;
		private System.Windows.Forms.MenuItem fileSaveAs;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Editor()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Editor));
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.menuFile = new System.Windows.Forms.MenuItem();
			this.fileOpen = new System.Windows.Forms.MenuItem();
			this.fileClose = new System.Windows.Forms.MenuItem();
			this.sep1 = new System.Windows.Forms.MenuItem();
			this.fileSave = new System.Windows.Forms.MenuItem();
			this.fileSaveAs = new System.Windows.Forms.MenuItem();
			this.sep2 = new System.Windows.Forms.MenuItem();
			this.fileQuit = new System.Windows.Forms.MenuItem();
			this.evtList = new System.Windows.Forms.ListBox();
			this.rpvInfo = new System.Windows.Forms.Label();
			this.setBeg = new System.Windows.Forms.Button();
			this.setEnd = new System.Windows.Forms.Button();
			this.begPos = new System.Windows.Forms.TrackBar();
			this.endPos = new System.Windows.Forms.TrackBar();
			((System.ComponentModel.ISupportInitialize)(this.begPos)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.endPos)).BeginInit();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuFile});
			// 
			// menuFile
			// 
			this.menuFile.Index = 0;
			this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.fileOpen,
																					 this.fileClose,
																					 this.sep1,
																					 this.fileSave,
																					 this.fileSaveAs,
																					 this.sep2,
																					 this.fileQuit});
			this.menuFile.Text = "File";
			// 
			// fileOpen
			// 
			this.fileOpen.Index = 0;
			this.fileOpen.Text = "Open";
			this.fileOpen.Click += new System.EventHandler(this.fileOpen_Click);
			// 
			// fileClose
			// 
			this.fileClose.Index = 1;
			this.fileClose.Text = "Close";
			this.fileClose.Click += new System.EventHandler(this.fileClose_Click);
			// 
			// sep1
			// 
			this.sep1.Index = 2;
			this.sep1.Text = "-";
			// 
			// fileSave
			// 
			this.fileSave.Index = 3;
			this.fileSave.Text = "Save";
			this.fileSave.Click += new System.EventHandler(this.fileSave_Click);
			// 
			// fileSaveAs
			// 
			this.fileSaveAs.Index = 4;
			this.fileSaveAs.Text = "Save As...";
			this.fileSaveAs.Click += new System.EventHandler(this.fileSaveAs_Click);
			// 
			// sep2
			// 
			this.sep2.Index = 5;
			this.sep2.Text = "-";
			// 
			// fileQuit
			// 
			this.fileQuit.Index = 6;
			this.fileQuit.Text = "Quit";
			this.fileQuit.Click += new System.EventHandler(this.fileQuit_Click);
			// 
			// evtList
			// 
			this.evtList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.evtList.HorizontalScrollbar = true;
			this.evtList.IntegralHeight = false;
			this.evtList.Location = new System.Drawing.Point(4, 84);
			this.evtList.Name = "evtList";
			this.evtList.Size = new System.Drawing.Size(432, 172);
			this.evtList.TabIndex = 0;
			// 
			// rpvInfo
			// 
			this.rpvInfo.Location = new System.Drawing.Point(4, 4);
			this.rpvInfo.Name = "rpvInfo";
			this.rpvInfo.Size = new System.Drawing.Size(252, 76);
			this.rpvInfo.TabIndex = 2;
			// 
			// setBeg
			// 
			this.setBeg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.setBeg.Location = new System.Drawing.Point(8, 264);
			this.setBeg.Name = "setBeg";
			this.setBeg.Size = new System.Drawing.Size(75, 20);
			this.setBeg.TabIndex = 3;
			this.setBeg.Text = "Begining:";
			this.setBeg.Click += new System.EventHandler(this.setBeg_Click);
			// 
			// setEnd
			// 
			this.setEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.setEnd.Location = new System.Drawing.Point(8, 292);
			this.setEnd.Name = "setEnd";
			this.setEnd.Size = new System.Drawing.Size(75, 20);
			this.setEnd.TabIndex = 4;
			this.setEnd.Text = "End:";
			this.setEnd.Click += new System.EventHandler(this.setEnd_Click);
			// 
			// begPos
			// 
			this.begPos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.begPos.AutoSize = false;
			this.begPos.LargeChange = 5000;
			this.begPos.Location = new System.Drawing.Point(84, 264);
			this.begPos.Maximum = 1;
			this.begPos.Name = "begPos";
			this.begPos.Size = new System.Drawing.Size(352, 20);
			this.begPos.SmallChange = 100;
			this.begPos.TabIndex = 6;
			this.begPos.TickStyle = System.Windows.Forms.TickStyle.None;
			this.begPos.Scroll += new System.EventHandler(this.begPos_Scroll);
			// 
			// endPos
			// 
			this.endPos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.endPos.AutoSize = false;
			this.endPos.LargeChange = 5000;
			this.endPos.Location = new System.Drawing.Point(84, 292);
			this.endPos.Maximum = 1;
			this.endPos.Name = "endPos";
			this.endPos.Size = new System.Drawing.Size(352, 20);
			this.endPos.SmallChange = 100;
			this.endPos.TabIndex = 7;
			this.endPos.TickStyle = System.Windows.Forms.TickStyle.None;
			this.endPos.Value = 1;
			this.endPos.Scroll += new System.EventHandler(this.endPos_Scroll);
			// 
			// Editor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(440, 317);
			this.Controls.Add(this.endPos);
			this.Controls.Add(this.begPos);
			this.Controls.Add(this.setEnd);
			this.Controls.Add(this.setBeg);
			this.Controls.Add(this.rpvInfo);
			this.Controls.Add(this.evtList);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
			this.Name = "Editor";
			this.Text = "RPV Editor";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Editor_Closing);
			this.Load += new System.EventHandler(this.Editor_Load);
			((System.ComponentModel.ISupportInitialize)(this.begPos)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.endPos)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private ArrayList m_Packets = new ArrayList();
		private string m_FileName;

		private class Packet
		{
			public int Time;
			public int ListIdx = -1;
			public byte[] Data;

			public bool IsDisplayed
			{
				get
				{
					return Data[0] == 0xAE || Data[0] == 0x1C || Data[0] == 0x97;
				}
			}

			private static string ReadAsciiFixed( byte[] data, int offset, int maxLen )
			{
				int count = 0;
				while ( count < maxLen && data[offset+count] != 0 )
					count ++;
				return Encoding.ASCII.GetString( data, offset, count );
			}

			private static string ReadUniNull( byte[] data, int offset )
			{
				StringBuilder sb = new StringBuilder();
				int count = 0;
				while ( offset+count+1 < data.Length )
				{
					char ch = (char)(data[offset+count]<<8 | data[offset+count+1]);
					if ( ch == 0 )
						break;
					sb.Append( ch );
					count+=2;
				}
				return sb.ToString();
			}

			public override string ToString()
			{
				StringBuilder sb = new StringBuilder();
				sb.Append( Utility.FormatTimeMS( Time ) );
				sb.Append( ": " );

				switch ( Data[0] )
				{
					case 0x1C:
					{
						MessageType type = (MessageType)Data[9]; // 9
						string name = ReadAsciiFixed( Data, 14, 30 );
						string text = Encoding.ASCII.GetString( Data, 44, Data.Length - 44 ); // 45+

						switch( type )
						{
							case MessageType.Label:
								sb.Append( "You See: " );
								sb.Append( text );
								break;
							case MessageType.Spell:
								sb.Append( "Spell: " );
								sb.Append( name );
								sb.Append( ": " );
								sb.Append( text );
								break;
							case MessageType.Emote:
								sb.Append( "Emote: " );
								sb.Append( name );
								sb.Append( ": " );
								sb.Append( text );
								break;
							default:
								sb.Append( "Speech: " );
								sb.Append( name );
								sb.Append( ": " );
								sb.Append( text );
								break;
						}
						break;
					}
					case 0xAE:
					{
						MessageType type = (MessageType)Data[9]; // 9
						string name = ReadAsciiFixed( Data, 18, 30 );
						string text = ReadUniNull( Data, 48 );

						switch( type )
						{
							case MessageType.Label:
								sb.Append( "You See: " );
								sb.Append( text );
								break;
							case MessageType.Spell:
								sb.Append( "Spell: " );
								sb.Append( name );
								sb.Append( ": " );
								sb.Append( text );
								break;
							case MessageType.Emote:
								sb.Append( "Emote: " );
								sb.Append( name );
								sb.Append( ": " );
								sb.Append( text );
								break;
							default:
								sb.Append( "Speech: " );
								sb.Append( name );
								sb.Append( ": " );
								sb.Append( text );
								break;
						}
						break;
					}
					case 0x97:
					{
						sb.Append( "Walk " );
						switch ( ((Direction)Data[1])& Direction.Mask )
						{
							case Direction.Up:
								sb.Append( "NorthWest" );
								break;
							case Direction.Right:
								sb.Append( "NorthEast" );
								break;
							case Direction.Down:
								sb.Append( "SouthEast" );
								break;
							case Direction.Left:
								sb.Append( "SouthWest" );
								break;
							default:
								sb.Append( (((Direction)Data[1])& Direction.Mask).ToString() );
								break;
						}
						break;
					}
					default:
					{
						sb.AppendFormat( "PacketID 0x{0:X2}", Data[0] );
						break;
					}
				}

				return sb.ToString();
			}
		}

		private object m_Beg, m_End;

		private System.Net.IPAddress m_IP;
		private string m_PlayerName;
		private DateTime m_Date;
		private bool m_NeedSave = false;

		public bool NeedSave
		{
			get
			{
				return m_NeedSave;
			}
			set
			{
				m_NeedSave = value;

				if ( m_FileName != null )
					this.Text = String.Format( "RPVEditor - {0}{1}", Path.GetFileName( m_FileName ), m_NeedSave ? " *" : "" );
				else
					this.Text = "RPVEditor";
			}
		}

		private const int PlayerVersion = 4;

		private void LoadRPV( string filename )
		{
			m_FileName = filename;

			GZBlockIn gzin = new GZBlockIn( filename );//new BinaryReader( new FileStream( filename, FileMode.Open, FileAccess.Read, FileShare.Read ) );
			byte ver;

			try
			{
				ver = gzin.Raw.ReadByte();
				
				if ( ver > PlayerVersion )
				{
					gzin.Close();
					gzin = null;
					MessageBox.Show( this, "This file is not compatible with this version of RPVEditor.  Make sure you have the latest Razor version.", "Version Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Stop );
					return;
				}

				gzin.IsCompressed = ver > 1;

				byte[] filehash = gzin.Raw.ReadBytes( 16 );				
				m_Date = DateTime.FromFileTime( gzin.Raw.ReadInt64() );
				TimeSpan len = TimeSpan.FromMilliseconds( gzin.Raw.ReadInt32() );

				m_PlayerName = gzin.Compressed.ReadString();
				World.ShardName = gzin.Compressed.ReadString();
				try
				{
					if ( ver > 1 )
						m_IP = new System.Net.IPAddress( (long)gzin.Compressed.ReadUInt32() );
					else
						m_IP = System.Net.IPAddress.Any;
				}
				catch
				{
				}

				int m_StartPos = (int)gzin.Position;
				
				long rawPos = gzin.RawStream.Position;
				gzin.RawStream.Seek( 1+16, SeekOrigin.Begin );
				using ( MD5 md5 = MD5.Create() )
				{
					byte[] check = md5.ComputeHash( gzin.RawStream );
					for(int i=0;i<check.Length;i++)
					{
						if ( check[i] != filehash[i] )
						{
							gzin.Close();
							gzin = null;
							MessageBox.Show( this, "This video file is corrupt.\nIt may be incomplete, or it may have been tampered with.\nThe file cannot be played.", "Damaged File", MessageBoxButtons.OK, MessageBoxIcon.Error );
							return;
						}
					}
				}
				gzin.RawStream.Seek( rawPos, SeekOrigin.Begin );

				rpvInfo.Text = String.Format( "File: {0}\nLength: {1} ({2})\nDate: {3}\nRecorded by \"{4}\" on \"{5}\" ({6})\n", Path.GetFileName( m_FileName ), Utility.FormatTime( (int)len.TotalSeconds ), Utility.FormatSize( gzin.RawStream.Length ), m_Date.ToString( "M-dd-yy @ h:mmtt" ), m_PlayerName, World.ShardName, m_IP );
			}
			catch ( Exception ex )
			{
				MessageBox.Show( this, "There was a problem reading the file.  It may be damaged or incomplete.\n" + ex.Message, "Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
				gzin.Close();
				gzin = null;
				return;
			}

			int end = gzin.Compressed.ReadInt32();
			end += (int)gzin.Position;

			World.AddMobile( World.Player = new PlayerData( gzin.Compressed, ver ) );
			while ( gzin.Position < end )
			{
				byte type = gzin.Compressed.ReadByte();
				if ( type == 1 )
					World.AddMobile( new Mobile( gzin.Compressed, ver ) );
				else if ( type == 0 )
					World.AddItem( new Item( gzin.Compressed, ver ) );
			}

			foreach ( Mobile m in World.Mobiles.Values )
				m.AfterLoad();

			foreach ( Item i in World.Items.Values )
				i.AfterLoad();

			byte[] buff = new byte[32767];
			int curTime = 0;
			int lastTime = 0;
			while ( !gzin.EndOfFile )
			{
				lastTime = curTime;
				curTime += gzin.Compressed.ReadInt32();

				gzin.Compressed.Read( buff, 0, 1 );
				if ( buff[0] == 0xFF )
					break;

				int len;
				int idx = 1;
				if ( PacketInfo.IsDyn( buff[0] ) )
				{
					gzin.Compressed.Read( buff, 1, 2 );
					idx += 2;
				}
				len = PacketInfo.GetLength( buff );

				gzin.Compressed.Read( buff, idx, len-idx );

				Packet p = new Packet();
				p.Time = curTime;
				p.Data = new byte[len];
				Array.Copy( buff, p.Data, len );

				m_Packets.Add( p );
			}

			gzin.Close();
			gzin = null;

			lastTime = curTime - lastTime;
			if ( lastTime <= 500 )
			{
				lastTime = 500;
				curTime += lastTime;
			}
			begPos.Maximum = endPos.Maximum = curTime;
			endPos.Value = curTime;
			begPos.Value = 0;

			evtList.BeginUpdate();
			for(int i=0;i<m_Packets.Count;i++)
			{
				Packet p = (Packet)m_Packets[i];
				if ( p.IsDisplayed )
				{
					p.ListIdx = evtList.Items.Count;
					evtList.Items.Add( p );
				}

			}
			UpdateBeg();
			UpdateEnd();
			evtList.EndUpdate();

			NeedSave = false;
		}

		private void SaveRPV( string filename )
		{
			int curTime = 0;
			int i = 0;
			while ( i < m_Packets.Count )
			{
				Packet p = (Packet)m_Packets[i];
				curTime = p.Time;
				if ( curTime < begPos.Value )
					PacketHandlers.ProcessPacket( p.Data );
				else
					break;
				i++;
			}

			m_Date += TimeSpan.FromMilliseconds( curTime );

			GZBlockOut gzout;
			
			try
			{
				gzout = new GZBlockOut( filename, 2048 );
			}
			catch ( Exception ex )
			{
				MessageBox.Show( this, ex.Message, "Error Saving RPV!", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}
			
			gzout.Raw.Write( (byte) PlayerVersion );
			gzout.Raw.Seek( 16, SeekOrigin.Current ); // md5
			gzout.Raw.Write( (long)m_Date.ToFileTime() );
			gzout.Raw.Write( (int)(endPos.Value - begPos.Value) ); // length

			gzout.BufferAll = true;
			gzout.Compressed.Write( World.Player.Name );
			gzout.Compressed.Write( World.ShardName );
			gzout.Compressed.Write( m_IP.GetAddressBytes(), 0, 4 );
			
			long start = gzout.Position;
			gzout.Compressed.Write( (int)0 ); // len

			World.Player.SaveState( gzout.Compressed );
			foreach ( Mobile m in World.Mobiles.Values )
			{
				if ( m != World.Player )
				{
					gzout.Compressed.Write( (byte)1 );
					m.SaveState( gzout.Compressed );
				}
			}

			foreach ( Item item in World.Items.Values )
			{
				if ( !(item.Container is Item) )
				{
					gzout.Compressed.Write( (byte)0 );
					item.SaveState( gzout.Compressed );
				}
			}

			long end = gzout.Position;

			gzout.Seek( (int)start, SeekOrigin.Begin );
			gzout.Compressed.Write( (int)( end - (start+4) ) );
			gzout.Seek( 0, SeekOrigin.End );

			gzout.BufferAll = false;
			gzout.Flush();

			int lastTime = curTime;
			while ( i < m_Packets.Count )
			{
				Packet p = (Packet)m_Packets[i++];
				curTime = p.Time;
				if ( curTime <= endPos.Value )
				{
					gzout.Compressed.Write( (int)(p.Time - curTime) );
					gzout.Compressed.Write( p.Data );
				}
				else
				{
					break;
				}
			}

			gzout.Compressed.Write( (int)(endPos.Value - curTime) );
			gzout.Compressed.Write( (byte) 0xFF );

			gzout.ForceFlush();
			gzout.BufferAll = true;

			gzout.RawStream.Seek( 1+16, SeekOrigin.Begin );
			byte[] hash;
			using ( MD5 md5 = MD5.Create() )
				hash = md5.ComputeHash( gzout.RawStream );

			gzout.RawStream.Seek( 1, SeekOrigin.Begin );
			gzout.Raw.Write( hash );
				
			gzout.RawStream.Flush();
			gzout.Close();
			gzout = null;

			NeedSave = false;

			CloseRPV();
		}
		
		private void CloseRPV()
		{
			if ( m_FileName == null )
			{
				NeedSave = false;
				return;
			}

			if ( NeedSave )
			{
				if ( MessageBox.Show( this, String.Format( "{0} has changed.\nDo you wish to save the changes?", m_FileName ), "Save Changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes )
				{
					SaveFileDialog sfd = new SaveFileDialog();
					sfd.FileName = m_FileName;
					sfd.AddExtension = false;
					sfd.CheckFileExists = false;
					sfd.CheckPathExists = true;
					sfd.DefaultExt = "rpv";
					sfd.DereferenceLinks = true;
					sfd.Filter = "Razor PacketVideo (*.rpv)|*.rpv|All Files (*.*)|*.*";
					sfd.FilterIndex = 0;
					sfd.RestoreDirectory = true;
					sfd.ShowHelp = false;
					sfd.Title = "Select a Video File...";
					sfd.ValidateNames = true;

					if ( sfd.ShowDialog( this ) == DialogResult.OK )
						SaveRPV( sfd.FileName );
				}
			}

			rpvInfo.Text = "";
			begPos.Value = 0;
			endPos.Value = 1;
			begPos.Maximum = endPos.Maximum = 1;
			evtList.Items.Clear();
			m_Packets.Clear();

			World.Mobiles.Clear();
			World.Items.Clear();
			World.Player = null;
			World.ShardName = "";

			m_PlayerName = null;
			m_IP = System.Net.IPAddress.Any;
			m_Date = DateTime.MinValue;

			m_FileName = null;
			NeedSave = false;
		}

		private void begPos_Scroll(object sender, System.EventArgs e)
		{
			if ( begPos.Value >= endPos.Value || m_FileName == null )
			{
				begPos.Value = endPos.Value - 1;
				return;
			}

			UpdateBeg();
			NeedSave = true;
		}

		private void endPos_Scroll(object sender, System.EventArgs e)
		{
			if ( endPos.Value <= begPos.Value || m_FileName == null )
			{
				endPos.Value = begPos.Value + 1;
				return;
			}

			UpdateEnd();
			NeedSave = true;
		}

		private void UpdateBeg()
		{
			evtList.BeginUpdate();
			if ( m_Beg != null )
				evtList.Items.Remove( m_Beg );
			
			m_Beg = String.Format( "*** Begining : {0} ***", Utility.FormatTimeMS( begPos.Value ) );
			int pos = evtList.Items.Count;
			for (int i=0;i<evtList.Items.Count;i++)
			{
				Packet p = evtList.Items[i] as Packet;
				if ( p == null || !p.IsDisplayed || p.ListIdx == -1 )
					continue;

				if ( p.Time >= begPos.Value )
				{
					pos = p.ListIdx;
					break;
				}
			}

			evtList.Items.Insert( pos, m_Beg );
			evtList.SelectedIndex = pos;
			evtList.EndUpdate();
		}

		private void UpdateEnd()
		{
			evtList.BeginUpdate();
			if ( m_End != null )
				evtList.Items.Remove( m_End );
			
			m_End = String.Format( "*** End : {0} ***", Utility.FormatTimeMS( endPos.Value ) );
			int pos = 0;
			for (int i=evtList.Items.Count-1;i>=0;i--)
			{
				Packet p = evtList.Items[i] as Packet;
				if ( p == null || !p.IsDisplayed || p.ListIdx == -1 )
					continue;

				if ( p.Time <= endPos.Value )
				{
					pos = p.ListIdx+2; // +1 for begining, +1 for being after it
					break;
				}
			}

			if ( pos >= evtList.Items.Count )
				evtList.Items.Add( m_End );
			else
				evtList.Items.Insert( pos, m_End );
			evtList.SelectedIndex = pos;
			evtList.EndUpdate();
		}

		private void setBeg_Click(object sender, System.EventArgs e)
		{
			Packet p = evtList.SelectedItem as Packet;
			if ( p == null )
				return;

			if ( p.Time >= endPos.Value )
				begPos.Value = endPos.Value - 1;
			else
				begPos.Value = p.Time;
			UpdateBeg();
			NeedSave = true;
		}

		private void setEnd_Click(object sender, System.EventArgs e)
		{
			Packet p = evtList.SelectedItem as Packet;
			if ( p == null )
				return;

			if ( p.Time <= begPos.Value )
				endPos.Value = begPos.Value + 1;
			else
				endPos.Value = p.Time;
			UpdateEnd();
			NeedSave = true;
		}

		private void fileOpen_Click(object sender, System.EventArgs e)
		{
			CloseRPV();

			OpenFileDialog ofd = new OpenFileDialog();
			ofd.AddExtension = false;
			ofd.CheckFileExists = true;
			ofd.CheckPathExists = true;
			ofd.DefaultExt = "rpv";
			ofd.DereferenceLinks = true;
			ofd.Filter = "Razor PacketVideo (*.rpv)|*.rpv|All Files (*.*)|*.*";
			ofd.FilterIndex = 0;
			ofd.Multiselect = false;
			ofd.RestoreDirectory = true;
			ofd.ShowHelp = ofd.ShowReadOnly = false;
			ofd.Title = "Select a Video File...";
			ofd.ValidateNames = true;

			if ( ofd.ShowDialog( this ) != DialogResult.OK )
				return;
			
			LoadRPV( ofd.FileName );
		}

		private void fileClose_Click(object sender, System.EventArgs e)
		{
			CloseRPV();
		}

		private void fileSave_Click(object sender, System.EventArgs e)
		{
			if ( m_FileName == null )
				return;

			string file = m_FileName;
			SaveRPV( file );
			CloseRPV();
			LoadRPV( file );
		}

		private void fileSaveAs_Click(object sender, System.EventArgs e)
		{
			if ( m_FileName == null )
				return;

			SaveFileDialog sfd = new SaveFileDialog();
			sfd.FileName = m_FileName;
			sfd.AddExtension = false;
			sfd.CheckFileExists = false;
			sfd.CheckPathExists = true;
			sfd.DefaultExt = "rpv";
			sfd.DereferenceLinks = true;
			sfd.Filter = "Razor PacketVideo (*.rpv)|*.rpv|All Files (*.*)|*.*";
			sfd.FilterIndex = 0;
			sfd.RestoreDirectory = true;
			sfd.ShowHelp = false;
			sfd.Title = "Select a Video File...";
			sfd.ValidateNames = true;

			if ( sfd.ShowDialog( this ) != DialogResult.OK )
				return;

			SaveRPV( sfd.FileName );
			CloseRPV();
			LoadRPV( sfd.FileName );
		}

		private void fileQuit_Click(object sender, System.EventArgs e)
		{
			CloseRPV();
			this.Close();
		}

		private void Editor_Load(object sender, System.EventArgs e)
		{
			fileOpen_Click(sender,e);
		}

		private void Editor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			CloseRPV();
		}
	}
}
