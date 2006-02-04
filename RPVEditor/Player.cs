using System;
using System.IO;
using System.Reflection;
using System.Collections;
using Ultima;

namespace Assistant
{
	public enum LockType : byte
	{
		Up = 0,
		Down = 1,
		Locked = 2,
	}
	
	public enum MessageType
	{
		Regular = 0x00,
		System = 0x01,
		Emote = 0x02,
		Label = 0x06,
		Focus = 0x07,
		Whisper = 0x08,
		Yell = 0x09,
		Spell = 0x0A,
		Encoded = 0xC0
	}

	public class Skill
	{
		public static int Count = 54;

		private LockType m_Lock;
		private ushort m_Value;
		private ushort m_Base;
		private ushort m_Cap;
		private short m_Delta;
		private int m_Idx;

		public Skill( int idx )
		{
			m_Idx = idx;
		}

		public int Index { get { return m_Idx; } }

		public LockType Lock
		{
			get{ return m_Lock; }
			set{ m_Lock = value; }
		}

		public ushort FixedValue
		{
			get{ return m_Value; }
			set{ m_Value = value; }
		}

		public ushort FixedBase
		{
			get{ return m_Base; }
			set
			{ 
				m_Delta += (short)(value - m_Base);
				m_Base = value; 
			}
		}

		public ushort FixedCap
		{
			get{ return m_Cap; }
			set{ m_Cap = value; }
		}

		public double Value
		{
			get{ return m_Value / 10.0; }
			set{ m_Value = (ushort)(value*10.0); }
		}

		public double Base
		{
			get{ return m_Base / 10.0; }
			set{ m_Base = (ushort)(value*10.0); }
		}

		public double Cap
		{
			get{ return m_Cap / 10.0; }
			set{ m_Cap = (ushort)(value*10.0); }
		}

		public double Delta 
		{ 
			get{ return m_Delta / 10.0; }
			set{ m_Delta = (short)(value*10); }
		}
	}

	public enum SkillName
	{
		Alchemy = 0,
		Anatomy = 1,
		AnimalLore = 2,
		ItemID = 3,
		ArmsLore = 4,
		Parry = 5,
		Begging = 6,
		Blacksmith = 7,
		Fletching = 8,
		Peacemaking = 9,
		Camping = 10,
		Carpentry = 11,
		Cartography = 12,
		Cooking = 13,
		DetectHidden = 14,
		Discordance = 15,
		EvalInt = 16,
		Healing = 17,
		Fishing = 18,
		Forensics = 19,
		Herding = 20,
		Hiding = 21,
		Provocation = 22,
		Inscribe = 23,
		Lockpicking = 24,
		Magery = 25,
		MagicResist = 26,
		Tactics = 27,
		Snooping = 28,
		Musicianship = 29,
		Poisoning = 30,
		Archery = 31,
		SpiritSpeak = 32,
		Stealing = 33,
		Tailoring = 34,
		AnimalTaming = 35,
		TasteID = 36,
		Tinkering = 37,
		Tracking = 38,
		Veterinary = 39,
		Swords = 40,
		Macing = 41,
		Fencing = 42,
		Wrestling = 43,
		Lumberjacking = 44,
		Mining = 45,
		Meditation = 46,
		Stealth = 47,
		RemoveTrap = 48,
		Necromancy = 49,
		Focus = 50,
		Chivalry = 51
	}

	public class PlayerData : Mobile
	{
		public class MoveEntry
		{
			//public byte Seq;
			public Direction Dir;
			//public int x;
			//public int y;
			public Point3D Position;
			public bool IsStep;
		}
		
		private ushort m_Str, m_Dex, m_Int;
		private ushort m_StamMax, m_Stam, m_ManaMax, m_Mana;
		private LockType m_StrLock, m_DexLock, m_IntLock;
		private uint m_Gold;
		private ushort m_Weight;
		private Skill[] m_Skills;
		private ushort m_AR;
		private ushort m_StatCap;
		private byte m_Followers;
		private byte m_FollowersMax;
		private int m_Tithe;
		private sbyte m_LocalLight;
		private byte m_GlobalLight;
		private ushort m_Features;
		private byte m_Season;
		private int[] m_MapPatches = new int[8];

		private bool m_SkillsSent;
		private Item m_Holding;
		private ushort m_HoldAmt;
		private Hashtable m_MoveInfo;
		private DateTime m_CriminalStart = DateTime.MinValue;
		private byte m_WalkSeq;

		public override void SaveState( BinaryWriter writer )
		{
			base.SaveState (writer);

			writer.Write( m_Str );
			writer.Write( m_Dex );
			writer.Write( m_Int );
			writer.Write( m_StamMax );
			writer.Write( m_Stam );
			writer.Write( m_ManaMax );
			writer.Write( m_Mana );
			writer.Write( (byte)m_StrLock );
			writer.Write( (byte)m_DexLock );
			writer.Write( (byte)m_IntLock );
			writer.Write( m_Gold );
			writer.Write( m_Weight );
			
			writer.Write( (byte)Skill.Count );
			for(int i=0;i<Skill.Count;i++)
			{
				writer.Write( m_Skills[i].FixedBase );
				writer.Write( m_Skills[i].FixedCap );
				writer.Write( m_Skills[i].FixedValue );
				writer.Write( (byte)m_Skills[i].Lock );
			}

			writer.Write( m_AR );
			writer.Write( m_StatCap );
			writer.Write( m_Followers );
			writer.Write( m_FollowersMax );
			writer.Write( m_Tithe );

			writer.Write( m_LocalLight );
			writer.Write( m_GlobalLight );
			writer.Write( m_Features );
			writer.Write( m_Season );
			
			writer.Write( (byte) m_MapPatches.Length );
			for(int i=0;i<m_MapPatches.Length;i++)
				writer.Write( (int)m_MapPatches[i] );
		}

		public PlayerData( BinaryReader reader, int version ) : base( reader, version )
		{
			int c;
			m_Str = reader.ReadUInt16();
			m_Dex = reader.ReadUInt16();
			m_Int = reader.ReadUInt16();
			m_StamMax = reader.ReadUInt16();
			m_Stam = reader.ReadUInt16();
			m_ManaMax = reader.ReadUInt16();
			m_Mana = reader.ReadUInt16();
			m_StrLock = (LockType)reader.ReadByte();
			m_DexLock = (LockType)reader.ReadByte();
			m_IntLock = (LockType)reader.ReadByte();
			m_Gold = reader.ReadUInt32();
			m_Weight = reader.ReadUInt16();

			m_MoveInfo = new Hashtable( 256 );

			if ( version >= 4 )
			{
				Skill.Count = c = reader.ReadByte();
			}
			else if ( version == 3 )
			{
				long skillStart = reader.BaseStream.Position;
				c = 0;
				reader.BaseStream.Seek( 7*49, SeekOrigin.Current );
				for(int i=48;i<60;i++)
				{
					ushort Base,Cap,Val;
					byte Lock;

					Base = reader.ReadUInt16();
					Cap = reader.ReadUInt16();
					Val = reader.ReadUInt16();
					Lock = reader.ReadByte();

					if ( Base > 2000 || Cap > 2000 || Val > 2000 || Lock > 2 )
					{
						c = i;
						break;
					}
				}

				if ( c == 0 )
					c = 52;
				else if ( c > 54 )
					c = 54;

				Skill.Count = c;

				reader.BaseStream.Seek( skillStart, SeekOrigin.Begin );
			}
			else
			{
				Skill.Count = c = 52;
			}

			m_Skills = new Skill[c];
			for (int i=0;i<c;i++)
			{
				m_Skills[i] = new Skill(i);
				m_Skills[i].FixedBase = reader.ReadUInt16();
				m_Skills[i].FixedCap = reader.ReadUInt16();
				m_Skills[i].FixedValue = reader.ReadUInt16();
				m_Skills[i].Lock = (LockType)reader.ReadByte();
			}

			m_AR = reader.ReadUInt16();
			m_StatCap = reader.ReadUInt16();
			m_Followers = reader.ReadByte();
			m_FollowersMax = reader.ReadByte();
			m_Tithe = reader.ReadInt32();

			m_LocalLight = reader.ReadSByte();
			m_GlobalLight = reader.ReadByte();
			m_Features = reader.ReadUInt16();
			m_Season = reader.ReadByte();

			if ( version >= 4 )
				c = reader.ReadByte();
			else
				c = 8;
			m_MapPatches = new int[c];
			for(int i=0;i<c;i++)
				m_MapPatches[i] = reader.ReadInt32();
		}

		public ushort Str
		{
			get{ return m_Str; }
			set{ m_Str = value; }
		}

		public ushort Dex
		{
			get{ return m_Dex; }
			set{ m_Dex = value; }
		}

		public ushort Stam
		{
			get{ return m_Stam; }
			set{ m_Stam = value; }
		}

		public ushort StamMax
		{
			get{ return m_StamMax; }
			set{ m_StamMax = value; }
		}

		public ushort Int
		{
			get{ return m_Int; }
			set{ m_Int = value; }
		}

		public ushort Mana
		{
			get{ return m_Mana; }
			set{ m_Mana = value; }
		}

		public ushort ManaMax
		{
			get{ return m_ManaMax; }
			set{ m_ManaMax = value; }
		}

		public uint Gold
		{
			get{ return m_Gold; }
			set{ m_Gold = value; }
		}

		public ushort Weight
		{
			get{ return m_Weight; }
			set{ m_Weight = value; }
		}

		public ushort MaxWeight
		{
			get{ return (ushort)((m_Str * 3.5) + 40); }
		}

		public LockType StrLock
		{
			get{ return m_StrLock; }
			set{ m_StrLock = value; }
		}

		public LockType DexLock
		{
			get{ return m_DexLock; }
			set{ m_DexLock = value; }
		}

		public LockType IntLock
		{
			get{ return m_IntLock; }
			set{ m_IntLock = value; }
		}

		public ushort StatCap
		{
			get{ return m_StatCap; }
			set{ m_StatCap = value; }
		}

		public ushort AR
		{
			get{ return m_AR; }
			set{ m_AR = value; }
		}

		public byte Followers
		{
			get{ return m_Followers; }
			set{ m_Followers = value; }
		}

		public byte FollowersMax
		{
			get{ return m_FollowersMax; }
			set{ m_FollowersMax = value; }
		}

		public int Tithe
		{
			get{ return m_Tithe; }
			set{ m_Tithe = value; }
		}

		public Skill []Skills { get{ return m_Skills; } }

		public bool SkillsSent
		{
			get{ return m_SkillsSent; }
			set{ m_SkillsSent = value; }
		}

		public byte WalkSequence{ get{ return m_WalkSeq; } }

		public void Resync()
		{
			m_WalkSeq = 0;
			m_MoveInfo.Clear();
		}

		public MoveEntry GetMoveEntry( byte seq )
		{
			return (MoveEntry)m_MoveInfo[seq];
		}

		public void MoveReq( Direction dir, byte seq )
		{
			MoveEntry e = new MoveEntry();
			m_MoveInfo[seq] = e;

			e.IsStep = (dir & Direction.Mask) == (Direction & Direction.Mask);
			e.Dir = dir;

			ProcessMove( dir );

			e.Position = Position;

			m_WalkSeq = (byte)(seq >= 255 ? 1 : seq+1);
		}

		public void ProcessMove( Direction dir )
		{
			if ( (dir & Direction.Mask) == (Direction & Direction.Mask) )
			{
				int x = Position.X, y = Position.Y;

				Utility.Offset( dir&Direction.Mask, ref x, ref y );
				
				int newZ = Position.Z;
				try { newZ = Assistant.Map.ZTop( Map, x, y, newZ ); } 
				catch { }
				Position = new Point3D( x, y, newZ );
			}
			Direction = dir;
		}

		public bool HasWalkEntry( byte seq )
		{
			return m_MoveInfo[seq] != null;
		}

		public void MoveRej( byte seq, Direction dir, Point3D pos )
		{
			Direction = dir;
			Position = pos;
			Resync();
		}

		public void MoveAck( byte seq )
		{
		}

		public static int GetZ( int fallBack )
		{
			return fallBack;
		}

		public override void OnPositionChanging( Point3D newPos )
		{
			int visRange = 18;//Configuration.GetInt( "VisRange" );

			ArrayList list = new ArrayList( World.Mobiles.Values );
			for (int i=0;i<list.Count;i++)
			{
				Mobile m = (Mobile)list[i];
				if ( m != this && Utility.Distance( m.Position, newPos ) > visRange )
					m.Remove();
			}

			list = new ArrayList( World.Items.Values );
			for (int i=0;i<list.Count;i++)
			{
				Item item = (Item)list[i];
				if ( item.Deleted || item.Container != null )
					continue;

				int dist = Utility.Distance( item.GetWorldPosition(), newPos );
				if ( item != this.Holding && dist > visRange )
					item.Remove();
			}

			base.OnPositionChanging( newPos );
		}

		public override void OnMapChange( byte old, byte cur )
		{
			ArrayList list = new ArrayList( World.Mobiles.Values );
			for (int i=0;i<list.Count;i++)
			{
				Mobile m = (Mobile)list[i];
				if ( m != this && m.Map != cur )
					m.Remove();
			}

			World.Items.Clear();
			for(int i=0;i<Contains.Count;i++)
			{
				Item item = (Item)Contains[i];
				World.AddItem( item );
				item.Contains.Clear();
			}	
		}

		public Item Holding
		{
			get{ return m_Holding; }
			set{ m_Holding = value; }
		}
		
		public ushort HoldAmount
		{
			get{ return m_HoldAmt; }
			set{ m_HoldAmt = value; }
		}

		public uint CurrentGumpS, CurrentGumpI;
		public bool HasGump;
		public uint CurrentMenuS;
		public ushort CurrentMenuI;
		public bool HasMenu;

		private ushort m_SpeechHue;
		public ushort SpeechHue { get { return m_SpeechHue; } set { m_SpeechHue = value; } }

		public sbyte LocalLightLevel { get { return m_LocalLight; } set { m_LocalLight = value; } }
		public byte GlobalLightLevel { get { return m_GlobalLight; } set { m_GlobalLight = value; } }
		public byte Season { get { return m_Season; } set { m_Season = value; } }
		public ushort Features { get { return m_Features; } set { m_Features = value; } }
		public int[] MapPatches { get { return m_MapPatches; } }
	}
}
