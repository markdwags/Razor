using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Assistant
{
	public abstract class Agent
	{
		private static ArrayList m_List = new ArrayList();
		public static ArrayList List { get{ return m_List; } }

		public delegate void ItemCreatedEventHandler( Item item );
		public delegate void MobileCreatedEventHandler( Mobile m );
		public static event ItemCreatedEventHandler OnItemCreated;
		public static event MobileCreatedEventHandler OnMobileCreated;

		public static void InvokeMobileCreated( Mobile m )
		{
			if ( OnMobileCreated != null )
				OnMobileCreated( m );
		}

		public static void InvokeItemCreated( Item i )
		{
			if ( OnItemCreated != null )
				OnItemCreated( i );
		}

		public static void Add( Agent a )
		{
			m_List.Add( a );
		}

		public static void ClearAll()
		{
			for (int i=0;i<m_List.Count;i++)
				((Agent)m_List[i]).Clear();
		}

		public static void SaveProfile( XmlTextWriter xml )
		{
			foreach( Agent a in m_List )
			{
				xml.WriteStartElement( a.Name );
				a.Save( xml );
				xml.WriteEndElement();
			}
		}	

		public static void LoadProfile( XmlElement xml )
		{
			ClearAll();

			if ( xml == null )
				return;

			for (int i=0;i<m_List.Count;i++)
			{
				try
				{
					Agent a = (Agent)m_List[i];
					XmlElement el = xml[a.Name];
					if ( el != null )
						a.Load( el );
				}
				catch
				{
				}
			}
		}

		public static void Redraw( ComboBox list, GroupBox gb, params Button[] buttons )
		{
			list.Visible = true;
			list.BeginUpdate();
			list.Items.Clear();
			list.SelectedIndex = -1;

			for(int i=0;i<buttons.Length;i++)
				buttons[i].Visible = false;

			for (int i=0;i<m_List.Count;i++)
				list.Items.Add( m_List[i] );
			list.EndUpdate();

			gb.Visible = false;
		}
		
		public static void Select( int idx, ComboBox agents, ListBox subList, GroupBox grp, params Button[] buttons )
		{
			for(int i=0;i<buttons.Length;i++)
			{
				buttons[i].Visible = false;
				buttons[i].Text = "";
				Engine.MainWindow.UnlockControl( buttons[i] );
			}
			grp.Visible = false;
			subList.Visible = false;
			Engine.MainWindow.UnlockControl( subList );

			Agent a = null;
			if ( idx >= 0 && idx < m_List.Count )
				a = m_List[idx] as Agent;
			
			if ( a != null )
			{
				grp.Visible = true;
				subList.Visible = true;
				grp.Text = a.Name;
				a.OnSelected( subList, buttons );
			}
		}

		public override string ToString()
		{
			return Name;
		}

		public abstract string Name{ get; }
		public abstract void Save( XmlTextWriter xml );
		public abstract void Load( XmlElement node );
		public abstract void Clear();
		public abstract void OnSelected( ListBox subList, params Button[] buttons );
		public abstract void OnButtonPress( int num );
	}
	
	public class UseOnceAgent : Agent
	{
		public static void Initialize()
		{
			Agent.Add( new UseOnceAgent() );
		}

		private ListBox m_SubList;
		private ArrayList m_Items;
		
		public UseOnceAgent()
		{
			m_Items = new ArrayList();
			PacketHandler.RegisterClientToServerViewer( 0x09, new PacketViewerCallback( OnSingleClick ) );
			HotKey.Add( HKCategory.Agents, LocString.UseOnceAgent, new HotKeyCallback( OnHotKey ) );
			HotKey.Add( HKCategory.Agents, LocString.AddUseOnce, new HotKeyCallback( OnAdd ) );

			Agent.OnItemCreated += new ItemCreatedEventHandler( CheckItemOPL );
		}
		
		public override void Clear()
		{
			m_Items.Clear();
		}

		private void CheckItemOPL( Item newItem )
		{
			for(int i=0;i<m_Items.Count;i++)
			{
				if ( m_Items[i] is Serial )
				{
					if ( newItem.Serial == (Serial)m_Items[i] )
					{
						m_Items[i] = newItem;
						newItem.ObjPropList.Add( Language.GetString( LocString.UseOnce ) );
						break;
					}
				}
			}
		}

		private void OnSingleClick( PacketReader pvSrc, PacketHandlerEventArgs args )
		{
			Serial serial = pvSrc.ReadUInt32();
			for(int i=0;i<m_Items.Count;i++)
			{
				Item item;
				if ( m_Items[i] is Serial )
				{
					item = World.FindItem( (Serial)m_Items[i] );
					if ( item != null )
						m_Items[i] = item;
				}

				item = m_Items[i] as Item;
				if ( item == null )
					continue;

				if ( item.Serial == serial )
				{
					ClientCommunication.SendToClient( new UnicodeMessage( item.Serial, item.ItemID, Assistant.MessageType.Label, 0x3B2, 3, Language.CliLocName, "", Language.Format( LocString.UseOnceHBA1, i+1 ) ) );
					break;
				}
			}
		}

		public override string Name{ get{ return Language.GetString( LocString.UseOnce ); } }
		public override void OnSelected( ListBox subList, params Button[] buttons )
		{
			m_SubList = subList;
			buttons[0].Text = Language.GetString( LocString.AddTarg );
			buttons[0].Visible = true;
			buttons[1].Text = Language.GetString( LocString.AddContTarg );
			buttons[1].Visible = true;
			buttons[2].Text = Language.GetString( LocString.RemoveTarg );
			buttons[2].Visible = true;
			buttons[3].Text = Language.GetString( LocString.ClearList );
			buttons[3].Visible = true;

			m_SubList.BeginUpdate();
			m_SubList.Items.Clear();
			
			for(int i=0;i<m_Items.Count;i++)
			{
				if ( m_Items[i] is Serial )
				{
					Item item = World.FindItem( (Serial)m_Items[i] );
					if ( item != null )
						m_Items[i] = item;
				}
				m_SubList.Items.Add( m_Items[i] );
			}
			m_SubList.EndUpdate();

			if ( !ClientCommunication.AllowBit( FeatureBit.UseOnceAgent ) && Engine.MainWindow != null )
			{
				for (int i=0;i<buttons.Length;i++)
					Engine.MainWindow.LockControl( buttons[i] );
				Engine.MainWindow.LockControl( subList );
			}
		}
		
		public override void OnButtonPress( int num )
		{
			switch ( num )
			{
				case 1:
					OnAdd();
					break;
				case 2:
					World.Player.SendMessage( MsgLevel.Force, LocString.TargCont );
					Targeting.OneTimeTarget( new Targeting.TargetResponseCallback( OnTargetBag ) );
					break;
				case 3:
					World.Player.SendMessage( MsgLevel.Force, LocString.TargItemRem );
					Targeting.OneTimeTarget( new Targeting.TargetResponseCallback( OnTargetRemove ) );
					break;
				case 4:
					for (int i = 0; i < m_Items.Count; i++)
					{
						if ( m_Items[i] is Item )
						{
							Item item = (Item)m_Items[i];

							item.ObjPropList.Remove( Language.GetString( LocString.UseOnce ) );
							item.OPLChanged();
						}
					}

					m_SubList.Items.Clear();
					m_Items.Clear();
					break;
			}
		}

		private void OnAdd()
		{
			World.Player.SendMessage( MsgLevel.Force, LocString.TargItemAdd );
			Targeting.OneTimeTarget( new Targeting.TargetResponseCallback( OnTarget ) );
		}
		
		private void OnTarget( bool location, Serial serial, Point3D loc, ushort gfx )
		{
            if ( Config.GetBool("AlwaysOnTop") )
                Engine.MainWindow.ShowMe();

			if ( !location && serial.IsItem )
			{
				Item item = World.FindItem( serial );
				if ( item == null )
				{
					World.Player.SendMessage( MsgLevel.Force, LocString.ItemNotFound );
					return;
				}

				item.ObjPropList.Add( Language.GetString( LocString.UseOnce ) );
				item.OPLChanged();

				m_Items.Add( item );
				if ( m_SubList != null )
					m_SubList.Items.Add( item );

				World.Player.SendMessage( MsgLevel.Force, LocString.ItemAdded );
			}
		}

		private void OnTargetRemove( bool location, Serial serial, Point3D loc, ushort gfx )
		{
			Engine.MainWindow.ShowMe();
			if ( !location && serial.IsItem )
			{
				for(int i=0;i<m_Items.Count;i++)
				{
					bool rem = false;
					if ( m_Items[i] is Item )
					{
						if ( ((Item)m_Items[i]).Serial == serial )
						{
							((Item)m_Items[i]).ObjPropList.Remove( Language.GetString( LocString.UseOnce ) );
							((Item)m_Items[i]).OPLChanged();

							rem = true;
						}
					}
					else if ( m_Items[i] is Serial )
					{
						if ( ((Serial)m_Items[i]) == serial )
							rem = true;
					}

					if ( rem )
					{
						m_Items.RemoveAt( i );
						m_SubList.Items.RemoveAt( i );
						World.Player.SendMessage( MsgLevel.Force, LocString.ItemRemoved );
						return;
					}
				}

				World.Player.SendMessage( MsgLevel.Force, LocString.ItemNotFound );
			}
		}

		private void OnTargetBag( bool location, Serial serial, Point3D loc, ushort gfx )
		{
			Engine.MainWindow.ShowMe();
			if ( !location && serial.IsItem )
			{
				Item i = World.FindItem( serial );
				if ( i != null && i.Contains.Count > 0 )
				{
					for ( int ci=0;ci<i.Contains.Count;ci++ )
					{
						Item toAdd = i.Contains[ci] as Item;

						if ( toAdd != null )
						{
							toAdd.ObjPropList.Add( Language.GetString( LocString.UseOnce ) );
							toAdd.OPLChanged();
							m_Items.Add( toAdd );
							m_SubList.Items.Add( toAdd );
						}
					}

					World.Player.SendMessage( MsgLevel.Force, LocString.ItemsAdded, i.Contains.Count );
				}
			}
		}
		
		public override void Save( XmlTextWriter xml )
		{
			for(int i=0;i<m_Items.Count;i++)
			{
				xml.WriteStartElement( "item" );
				if ( m_Items[i] is Item )
					xml.WriteAttributeString( "serial", ((Item)m_Items[i]).Serial.Value.ToString() );
				else
					xml.WriteAttributeString( "serial", ((Serial)m_Items[i]).Value.ToString() );
				xml.WriteEndElement();
			}
		}
		
		public override void Load( XmlElement node )
		{
			foreach ( XmlElement el in node.GetElementsByTagName( "item" ) )
			{
				try
				{
					string ser = el.GetAttribute( "serial" );
					m_Items.Add( (Serial)Convert.ToUInt32( ser )  );
				}
				catch
				{
				}
			}
		}
		
		public void OnHotKey()
		{
			if ( World.Player == null || !ClientCommunication.AllowBit( FeatureBit.UseOnceAgent ) )
				return;

			if ( m_Items.Count <= 0 )
			{
				World.Player.SendMessage( MsgLevel.Error, LocString.UseOnceEmpty );
			}
			else
			{
				Item item = null;
				if ( m_Items[0] is Item )
					item = (Item)m_Items[0];
				else if ( m_Items[0] is Serial )
					item = World.FindItem( (Serial)m_Items[0] );

				try
				{
					m_Items.RemoveAt( 0 );
					if ( m_SubList != null && m_SubList.Items.Count > 0 )
						m_SubList.Items.RemoveAt( 0 );
				}
				catch
				{
				}

				if ( item != null )
				{
					item.ObjPropList.Remove( Language.GetString( LocString.UseOnce ) );
					item.OPLChanged();

					World.Player.SendMessage( LocString.UseOnceStatus, item, m_Items.Count );
					PlayerData.DoubleClick( item );
				}
				else
				{
					World.Player.SendMessage( LocString.UseOnceError );
					OnHotKey();
				}
			}
		}
	}

	public class SellAgent : Agent
	{
		public static void Initialize()
		{
			Agent.Add( new SellAgent() );
		}

		private ListBox m_SubList;
		private Button m_EnableBTN;
		private Button m_HotBTN;
		private Button m_AmountButton;
		private ArrayList m_Items;
		private Serial m_HotBag;
		private bool m_Enabled;
		
		public SellAgent()
		{
			m_Items = new ArrayList();
			PacketHandler.RegisterServerToClientViewer( 0x9E, new PacketViewerCallback( OnVendorSell ) );
			PacketHandler.RegisterClientToServerViewer( 0x09, new PacketViewerCallback( OnSingleClick ) );

			Agent.OnItemCreated += new ItemCreatedEventHandler( CheckHBOPL );
		}

		private void CheckHBOPL( Item item )
		{
			if ( item.Serial == m_HotBag )
				item.ObjPropList.Add( Language.GetString( LocString.SellHB ) );
		}

		private void OnSingleClick( PacketReader pvSrc, PacketHandlerEventArgs args )
		{
			Serial serial = pvSrc.ReadUInt32();
			if ( m_HotBag == serial )
			{
				ushort gfx = 0;
				Item c = World.FindItem( m_HotBag );
				if ( c != null )
					gfx = c.ItemID.Value;
				ClientCommunication.SendToClient( new UnicodeMessage( m_HotBag, gfx, Assistant.MessageType.Label, 0x3B2, 3, Language.CliLocName, "", Language.GetString( LocString.SellHB ) ) );
			}
		}

		public override void Clear()
		{
			m_Items.Clear();
		}

		private void OnVendorSell( PacketReader pvSrc, PacketHandlerEventArgs args )
		{
			if ( !m_Enabled || !ClientCommunication.AllowBit( FeatureBit.SellAgent ) || ( m_Items.Count == 0 && m_HotBag == Serial.Zero ) )
				return;

			Item hb = null;
			if ( m_HotBag != Serial.Zero )
			{
				hb = World.FindItem( m_HotBag );
				if ( hb == null )
				{
					//m_HotBag = Serial.Zero;
					//SetHBText();
					World.Player.SendMessage( MsgLevel.Warning, "Sell Agent HotBag could not be found." );

					if ( m_Items.Count == 0 )
						return;
				}
			}

			int total = 0;

			uint serial = pvSrc.ReadUInt32();
			Mobile vendor = World.FindMobile( serial );
			if ( vendor == null )
				World.AddMobile( vendor=new Mobile( serial ) );
			int count = pvSrc.ReadUInt16();

			int maxSell = Config.GetInt( "SellAgentMax" );
			int sold = 0;
			ArrayList list = new ArrayList( count );
			for (int i=0;i<count && ( sold < maxSell || maxSell <= 0 );i++)
			{
				uint ser = pvSrc.ReadUInt32();
				ushort gfx = pvSrc.ReadUInt16();
				ushort hue = pvSrc.ReadUInt16();
				ushort amount = pvSrc.ReadUInt16();
				ushort price = pvSrc.ReadUInt16();

				pvSrc.ReadString( pvSrc.ReadUInt16() );//name

				Item item = World.FindItem( ser );

				if ( m_Items.Contains( gfx ) || ( item != null && item != hb && item.IsChildOf( hb ) ) )
				{
					if ( sold + amount > maxSell && maxSell > 0 )
						amount = (ushort)(maxSell - sold);
					list.Add( new SellListItem( ser, amount ) );
					total += amount * price;
					sold += amount;
				}
				//if ( sold >= maxSell && maxSell > 0 ) break;
			}
			
			if ( list.Count > 0 )
			{
				ClientCommunication.SendToServer( new VendorSellResponse( vendor, list ) );
				World.Player.SendMessage( MsgLevel.Force, LocString.SellTotals, sold, total );
				args.Block = true;
			}
		}
		
		public override string Name{ get{ return Language.GetString( LocString.Sell ); } }
		public override void OnSelected( ListBox subList, params Button[] buttons )
		{
			m_SubList = subList;
			m_EnableBTN = buttons[5];
			m_HotBTN = buttons[2];
			m_AmountButton = buttons[4];

			buttons[0].Text = Language.GetString( LocString.AddTarg );
			buttons[0].Visible = true;
			buttons[1].Text = Language.GetString( LocString.Remove );
			buttons[1].Visible = true;
			//button[2] = hotbutton
			buttons[2].Visible = true;
			buttons[3].Text = Language.GetString( LocString.Clear );
			buttons[3].Visible = true;
			m_AmountButton.Text = Language.Format( LocString.SellAmount, Config.GetInt( "SellAgentMax" ) );
			buttons[4].Visible = true;
			buttons[5].Text = Language.GetString( m_Enabled ? LocString.PushDisable : LocString.PushEnable );
			buttons[5].Visible = true;

			SetHBText();
			m_SubList.BeginUpdate();
			m_SubList.Items.Clear();
			for (int i=0;i<m_Items.Count;i++)
				m_SubList.Items.Add( (ItemID)((ushort)m_Items[i]) );
			m_SubList.EndUpdate();

			if ( !ClientCommunication.AllowBit( FeatureBit.SellAgent ) && Engine.MainWindow != null )
			{
				for (int i=0;i<buttons.Length;i++)
					Engine.MainWindow.LockControl( buttons[i] );
				Engine.MainWindow.LockControl( subList );
			}
		}
		
		public override void OnButtonPress( int num )
		{
			switch ( num )
			{
				case 1:
					World.Player.SendMessage( MsgLevel.Force, LocString.TargItemAdd );
					Targeting.OneTimeTarget( new Targeting.TargetResponseCallback( OnTarget ) );
					break;
				case 2:
					if ( m_SubList.SelectedIndex >= 0 )
					{
						m_Items.RemoveAt( m_SubList.SelectedIndex );
						m_SubList.Items.RemoveAt( m_SubList.SelectedIndex );
						m_SubList.SelectedIndex = -1;
					}
					break;
				case 3:
					if ( m_HotBag == Serial.Zero )
					{
						World.Player.SendMessage( MsgLevel.Force, LocString.TargCont );
						Targeting.OneTimeTarget( new Targeting.TargetResponseCallback( OnHBTarget ) );
					}
					else
					{
						Item hb = World.FindItem( m_HotBag );
						if ( hb != null )
						{
							if ( hb.ObjPropList.Remove( Language.GetString( LocString.SellHB ) ) )
								hb.OPLChanged();
						}
						m_HotBag = Serial.Zero;
						SetHBText();
					}
					break;
				case 4:
					m_SubList.Items.Clear();
					m_Items.Clear();
					break;
				case 5:
					if ( InputBox.Show( Language.GetString( LocString.EnterAmount ) ) )
						Config.SetProperty( "SellAgentMax", InputBox.GetInt( 100 ) );
					m_AmountButton.Text = Language.Format( LocString.SellAmount, Config.GetInt( "SellAgentMax" ) );
					break;
				case 6:
					m_Enabled = !m_Enabled;
					m_EnableBTN.Text = Language.GetString( m_Enabled ? LocString.PushDisable : LocString.PushEnable );
					break;
			}
		}

		private void SetHBText()
		{
			if ( m_HotBTN != null )
				m_HotBTN.Text = Language.GetString( m_HotBag == Serial.Zero ? LocString.SetHB : LocString.ClearHB );
		}
		
		private void OnTarget( bool location, Serial serial, Point3D loc, ushort gfx )
		{
			Engine.MainWindow.ShowMe();
			if ( !location && serial.IsItem )
			{
				m_Items.Add( gfx );
				m_SubList.Items.Add( (ItemID)gfx );
			}
		}

		private void OnHBTarget( bool location, Serial serial, Point3D loc, ushort gfx )
		{
			Engine.MainWindow.ShowMe();
			if ( !location && serial.IsItem )
			{
				m_HotBag = serial;
				SetHBText();

				Item hb = World.FindItem( m_HotBag );
				if ( hb != null )
				{
					hb.ObjPropList.Add( Language.GetString( LocString.SellHB ) );
					hb.OPLChanged();
				}
			}
		}

		public override void Save( XmlTextWriter xml )
		{
			if ( m_Items == null )
				return;

			xml.WriteAttributeString( "enabled", m_Enabled.ToString() );
			
			if ( m_HotBag != Serial.Zero )
			{
				xml.WriteStartElement( "hotbag" );
				xml.WriteString( m_HotBag.ToString() );
				xml.WriteEndElement();
			}

			foreach ( ushort iid in m_Items )
			{
				xml.WriteStartElement( "item" );
				xml.WriteAttributeString( "id", iid.ToString() );
				xml.WriteEndElement();
			}
		}
		
		public override void Load( XmlElement node )
		{
			try
			{
				m_Enabled = Boolean.Parse( node.GetAttribute( "enabled" ) );
			}
			catch
			{
				m_Enabled = false;
			}

			try
			{
				m_HotBag = Serial.Parse( node["hotbag"].InnerText );
			}
			catch
			{
				m_HotBag = Serial.Zero;
			}

			foreach ( XmlElement el in node.GetElementsByTagName( "item" ) )
			{
				try
				{
					string str = el.GetAttribute( "id" );
					m_Items.Add( Convert.ToUInt16( str ) );
				}
				catch
				{
				}
			}
		}
	}

	public class OrganizerAgent : Agent
	{
		public static void Initialize()
		{
			for(int i=1;i<=10;i++)
				Agent.Add( new OrganizerAgent( i ) );
		}

		private ListBox m_SubList;
		private Button m_BagBTN;
		private Button m_ArrBTN;
		private ArrayList m_Items;
		private uint m_Cont;
		private int m_Num;
		
		public OrganizerAgent( int num )
		{
			m_Items = new ArrayList();
			m_Num = num;
			HotKey.Add( HKCategory.Agents, HKSubCat.None, String.Format( "{0}-{1}", Language.GetString( LocString.OrganizerAgent ), m_Num ), new HotKeyCallback( Organize ) );
			HotKey.Add( HKCategory.Agents, HKSubCat.None, String.Format( "{0}-{1}: {2}", Language.GetString( LocString.OrganizerAgent ), m_Num, Language.GetString( LocString.SetHB ) ), new HotKeyCallback( SetHotBag ) );
			PacketHandler.RegisterClientToServerViewer( 0x09, new PacketViewerCallback( OnSingleClick ) );

			Agent.OnItemCreated += new ItemCreatedEventHandler( CheckContOPL );
		}

		public void CheckContOPL( Item item )
		{
			if ( item.Serial == m_Cont )
				item.ObjPropList.Add( Language.Format( LocString.OrganizerHBA1, m_Num ) );
		}

		private void OnSingleClick( PacketReader pvSrc, PacketHandlerEventArgs args )
		{
			uint serial = pvSrc.ReadUInt32();
			if ( m_Cont == serial )
			{
				ushort gfx = 0;
				Item c = World.FindItem( m_Cont );
				if ( c != null )
					gfx = c.ItemID.Value;
				ClientCommunication.SendToClient( new UnicodeMessage( m_Cont, gfx, Assistant.MessageType.Label, 0x3B2, 3, Language.CliLocName, "", Language.Format( LocString.OrganizerHBA1, m_Num ) ) );
			}
		}

		public override string Name{ get{ return String.Format( "{0}-{1}", Language.GetString( LocString.Organizer ), m_Num ); } }

		public override void OnSelected( ListBox subList, params Button[] buttons )
		{
			m_SubList = subList;
			buttons[0].Text = Language.GetString( LocString.AddTarg );
			buttons[0].Visible = true;
			m_BagBTN = buttons[1];
			m_ArrBTN = buttons[2];
			if ( m_Cont != 0 )
				buttons[1].Text = Language.GetString( LocString.ClearHB );
			else
				buttons[1].Text = Language.GetString( LocString.SetHB );
			buttons[1].Visible = true;
			buttons[2].Text = Language.GetString( LocString.OrganizeNow );
			buttons[2].Visible = true;
			buttons[3].Text = Language.GetString( LocString.Remove );
			buttons[3].Visible = true;
			buttons[4].Text = Language.GetString( LocString.Clear );
			buttons[4].Visible = true;
			buttons[5].Text = Language.GetString( LocString.StopNow );
			buttons[5].Visible = true;

			m_SubList.BeginUpdate();
			m_SubList.Items.Clear();
			for (int i=0;i<m_Items.Count;i++)
				m_SubList.Items.Add( (ItemID)((ushort)m_Items[i]) );
			m_SubList.EndUpdate();
		}

		public void SetHotBag()
		{
			World.Player.SendMessage( MsgLevel.Force, LocString.TargCont );
			Targeting.OneTimeTarget( new Targeting.TargetResponseCallback( OnTargetBag ) );
		}
		
		public override void OnButtonPress( int num )
		{
			switch ( num )
			{
				case 1:
					World.Player.SendMessage( MsgLevel.Force, LocString.TargItemAdd );
					Targeting.OneTimeTarget( new Targeting.TargetResponseCallback( OnTarget ) );
					break;
				case 2:
					SetHotBag();
					break;
				case 3:
					Organize();
					break;
				case 4:
					if ( m_SubList.SelectedIndex >= 0 && m_SubList.SelectedIndex < m_Items.Count )
					{
						m_Items.RemoveAt( m_SubList.SelectedIndex );
						m_SubList.Items.RemoveAt( m_SubList.SelectedIndex );
					}
					break;
				case 5:
				{
					Item bag = World.FindItem( m_Cont );
					if ( bag != null )
					{
						bag.ObjPropList.Remove( Language.Format( LocString.OrganizerHBA1, m_Num ) );
						bag.OPLChanged();
					}

					m_SubList.Items.Clear();
					m_Items.Clear();
					m_Cont = 0;
					m_BagBTN.Text = Language.GetString( LocString.SetHB );
					break;
				}
				case 6:
					DragDropManager.GracefulStop();
					break;
			}
		}

		private void Organize()
		{
			if ( m_Cont == 0 || m_Cont > 0x7FFFFF00 )
			{
				World.Player.SendMessage( MsgLevel.Force, LocString.ContNotSet );
				return;
			}

			Item pack = World.Player.Backpack;
			if ( pack == null )
			{
				World.Player.SendMessage( MsgLevel.Warning, LocString.NoBackpack );
				return;
			}

			int count = OrganizeChildren( pack );

			if ( count > 0 )
				World.Player.SendMessage( LocString.OrgQueued, count );
			else
				World.Player.SendMessage( LocString.OrgNoItems );
		}
		
		private int OrganizeChildren( Item container )
		{
			object dest = World.FindItem( m_Cont );
			if ( dest == null )
			{
				dest = World.FindMobile( m_Cont );
				if ( dest == null )
					return 0;
			}

			/*else if ( World.Player.Backpack != null && ((Item)dest).IsChildOf( World.Player ) && !((Item)dest).IsChildOf( World.Player.Backpack ) )
			{
				return 0;
			}*/

			return OrganizeChildren( container, dest );
		}

		private int OrganizeChildren( Item container, object dest )
		{	
			int count = 0;
			for(int i=0;i<container.Contains.Count;i++)
			{
				Item item = (Item)container.Contains[i];
				if ( item.Serial != m_Cont && !item.IsChildOf( dest ) )
				{
					count += OrganizeChildren( item, dest );
					if ( m_Items.Contains( item.ItemID.Value ) )
					{
						if ( dest is Item )
							DragDropManager.DragDrop( item, (Item)dest );
						else if ( dest is Mobile )
							DragDropManager.DragDrop( item, ((Mobile)dest).Serial );
						count++;
					}
				}
			}

			return count;
		}
		
		private void OnTarget( bool location, Serial serial, Point3D loc, ushort gfx )
		{
			if ( Engine.MainWindow != null )
				Engine.MainWindow.ShowMe();
			if ( !location && serial.IsItem && World.Player != null )
			{
				if ( m_Items != null && m_Items.Contains( gfx ) )
				{
					World.Player.SendMessage( MsgLevel.Force, LocString.ItemExists );
				}
				else
				{
					if ( m_Items != null )
						m_Items.Add( gfx );

					if ( m_SubList != null )
						m_SubList.Items.Add( (ItemID)gfx );

					World.Player.SendMessage( MsgLevel.Force, LocString.ItemAdded );
				}
			}
		}

		private void OnTargetBag( bool location, Serial serial, Point3D loc, ushort gfx )
		{
			if ( Engine.MainWindow != null )
				Engine.MainWindow.ShowMe();
			if ( !location && serial > 0 && serial <= 0x7FFFFF00 )
			{
				Item bag = World.FindItem( m_Cont );
				if ( bag != null && bag.ObjPropList != null )
				{
					bag.ObjPropList.Remove( Language.Format( LocString.OrganizerHBA1, m_Num ) );
					bag.OPLChanged();
				}

				m_Cont = serial;
				if ( m_BagBTN != null )
					m_BagBTN.Text = Language.GetString( LocString.ClearHB );
				if ( World.Player != null )
					World.Player.SendMessage( MsgLevel.Force, LocString.ContSet );

				bag = World.FindItem( m_Cont );
				if ( bag != null && bag.ObjPropList != null )
				{
					bag.ObjPropList.Add( Language.Format( LocString.OrganizerHBA1, m_Num ) );
					bag.OPLChanged();
				}
			}
		}
		
		public override void Clear()
		{
			m_Items.Clear();
			m_Cont = 0;
			if ( m_BagBTN != null )
				m_BagBTN.Text = Language.GetString( LocString.SetHB );
		}

		public override void Save( XmlTextWriter xml )
		{
			xml.WriteAttributeString( "hotbag", m_Cont.ToString() );
			for(int i=0;i<m_Items.Count;i++)
			{
				xml.WriteStartElement( "item" );
				xml.WriteAttributeString( "id", m_Items[i].ToString() );
				xml.WriteEndElement();
			}
		}
		
		public override void Load( XmlElement node )
		{
			try
			{
				m_Cont = Convert.ToUInt32( node.GetAttribute( "hotbag" ) );
			}
			catch
			{
			}

			if ( m_BagBTN != null )
				m_BagBTN.Text = Language.GetString( m_Cont != 0 ? LocString.ClearHB : LocString.SetHB );

			foreach ( XmlElement el in node.GetElementsByTagName( "item" ) )
			{
				try
				{
					string gfx = el.GetAttribute( "id" );
					m_Items.Add( Convert.ToUInt16( gfx ) );
				}
				catch
				{
				}
			}
		}
	}

	public class SearchExemptionAgent : Agent
	{
		private static SearchExemptionAgent m_Instance;
		public static int Count{ get{ return m_Instance.m_Items.Count; } }
		public static SearchExemptionAgent Instance{ get{ return m_Instance; } }

		public static void Initialize()	
		{ 
			Agent.Add( m_Instance = new SearchExemptionAgent() ); 
		}

		public static bool IsExempt( Item item )
		{
			if ( item == null || item.IsBagOfSending )
				return true;
			else
				return m_Instance == null ? false : m_Instance.CheckExempt( item );
		}

		public static bool Contains( Item item )
		{
			return m_Instance == null ? false : m_Instance.m_Items.Contains( item.Serial ) || m_Instance.m_Items.Contains( item.ItemID );
		}

		private ListBox m_SubList;
		private ArrayList m_Items;
		
		public SearchExemptionAgent()
		{
			m_Items = new ArrayList();
		}

		public override void Clear()
		{
			m_Items.Clear();
		}

		private bool CheckExempt( Item item )
		{
			if ( m_Items.Count > 0 )
			{
				if ( m_Items.Contains( item.Serial ) )
					return true;
				else if ( m_Items.Contains( item.ItemID ) )
					return true;
				else if ( item.Container != null && item.Container is Item )
					return CheckExempt( (Item)item.Container );
			}
			return false;
		}

		public override string Name{ get{ return Language.GetString( LocString.AutoSearchEx ); } }
		public override void OnSelected( ListBox subList, params Button[] buttons )
		{
			m_SubList = subList;

			buttons[0].Text = Language.GetString( LocString.AddTarg );
			buttons[0].Visible = true;
			buttons[1].Text = Language.GetString( LocString.AddTargType );
			buttons[1].Visible = true;
			buttons[2].Text = Language.GetString( LocString.Remove );
			buttons[2].Visible = true;
			buttons[3].Text = Language.GetString( LocString.RemoveTarg );
			buttons[3].Visible = true;
			buttons[4].Text = Language.GetString( LocString.ClearList );
			buttons[4].Visible = true;

			m_SubList.BeginUpdate();
			m_SubList.Items.Clear();
			
			for(int i=0;i<m_Items.Count;i++)
			{
				Item item = null;
				if ( m_Items[i] is Serial )
					item = World.FindItem( (Serial)m_Items[i] );
				if ( item != null )
					m_SubList.Items.Add( item.ToString() );
				else
					m_SubList.Items.Add( m_Items[i].ToString() );
			}
			m_SubList.EndUpdate();
		}
		
		public override void OnButtonPress( int num )
		{
			switch ( num )
			{
				case 1:
					World.Player.SendMessage( MsgLevel.Force, LocString.TargItemAdd );
					Targeting.OneTimeTarget( new Targeting.TargetResponseCallback( OnTarget ) );
					break;
				case 2:
					World.Player.SendMessage( MsgLevel.Force, LocString.TargItemAdd );
					Targeting.OneTimeTarget( new Targeting.TargetResponseCallback( OnTargetType ) );
					break;
				case 3:
					if ( m_SubList.SelectedIndex >= 0 && m_SubList.SelectedIndex < m_Items.Count )
					{
						m_Items.RemoveAt( m_SubList.SelectedIndex );
						m_SubList.Items.RemoveAt( m_SubList.SelectedIndex );
					}
					break;
				case 4:
					World.Player.SendMessage( MsgLevel.Force, LocString.TargItemRem );
					Targeting.OneTimeTarget( new Targeting.TargetResponseCallback( OnTargetRemove ) );
					break;
				case 5:
					m_SubList.Items.Clear();
					m_Items.Clear();
					break;
			}
		}
		
		private void OnTarget( bool location, Serial serial, Point3D loc, ushort gfx )
		{
			Engine.MainWindow.ShowMe();
			if ( !location && serial.IsItem )
			{
				m_Items.Add( serial );

				Item item = World.FindItem( serial );
				if ( item != null )
				{
					ClientCommunication.SendToClient( new ContainerItem( item ) );
					m_SubList.Items.Add( item.ToString() );
				}
				else
				{
					m_SubList.Items.Add( serial.ToString() );
				}

				World.Player.SendMessage( MsgLevel.Force, LocString.ItemAdded );
			}
		}

		private void OnTargetType( bool location, Serial serial, Point3D loc, ushort gfx )
		{
			Engine.MainWindow.ShowMe();

			if ( !serial.IsItem ) return;

			m_Items.Add( (ItemID)gfx );
			m_SubList.Items.Add( ((ItemID)gfx).ToString() );
			World.Player.SendMessage( MsgLevel.Force, LocString.ItemAdded );
		}

		private void OnTargetRemove( bool location, Serial serial, Point3D loc, ushort gfx )
		{
			Engine.MainWindow.ShowMe();
			if ( !location && serial.IsItem )
			{
				for(int i=0;i<m_Items.Count;i++)
				{
					if ( m_Items[i] is Serial && (Serial)m_Items[i] == serial )
					{
						m_Items.RemoveAt( i );
						m_SubList.Items.RemoveAt( i );
						World.Player.SendMessage( MsgLevel.Force, LocString.ItemRemoved );

						Item item = World.FindItem( serial );
						if ( item != null )
							ClientCommunication.SendToClient( new ContainerItem( item ) );
						return;
					}
				}

				World.Player.SendMessage( MsgLevel.Force, LocString.ItemNotFound );
			}
		}
		
		public override void Save( XmlTextWriter xml )
		{
			for(int i=0;i<m_Items.Count;i++)
			{
				xml.WriteStartElement( "item" );
				if ( m_Items[i] is Serial )
					xml.WriteAttributeString( "serial", ((Serial)m_Items[i]).Value.ToString() );
				else
					xml.WriteAttributeString( "id", ((ItemID)m_Items[i]).Value.ToString() );
				xml.WriteEndElement();
			}
		}
		
		public override void Load( XmlElement node )
		{
			foreach ( XmlElement el in node.GetElementsByTagName( "item" ) )
			{
				try
				{
					string ser = el.GetAttribute( "serial" );
					string iid = el.GetAttribute( "id" );
					if ( ser != null )
						m_Items.Add( (Serial)Convert.ToUInt32( ser ) );
					else if ( iid != null )
						m_Items.Add( (ItemID)Convert.ToUInt16( iid ) );
				}
				catch
				{
				}
			}
		}
	}

	public class ScavengerAgent : Agent
	{
		private static ScavengerAgent m_Instance = new ScavengerAgent();
		public static ScavengerAgent Instance{ get{ return m_Instance; } }

		public static bool Debug = false;

		public static void Initialize()	
		{ 
			Agent.Add( m_Instance ); 
		}

		private bool m_Enabled;
		private Serial m_Bag;
		private ListBox m_SubList;
		private Button m_EnButton;
		private ArrayList m_Items;

		private ArrayList m_Cache;
		private Item m_BagRef;
		
		public ScavengerAgent()
		{
			m_Items = new ArrayList();

			HotKey.Add( HKCategory.Agents, LocString.ClearScavCache, new HotKeyCallback( ClearCache ) );
			PacketHandler.RegisterClientToServerViewer( 0x09, new PacketViewerCallback( OnSingleClick ) );

			Agent.OnItemCreated += new ItemCreatedEventHandler( CheckBagOPL );
		}

		private void CheckBagOPL( Item item )
		{
			if ( item.Serial == m_Bag )
				item.ObjPropList.Add( Language.GetString( LocString.ScavengerHB ) );
		}

		private void OnSingleClick( PacketReader pvSrc, PacketHandlerEventArgs args )
		{
			Serial serial = pvSrc.ReadUInt32();
			if ( m_Bag == serial )
			{
				ushort gfx = 0;
				Item c = World.FindItem( m_Bag );
				if ( c != null )
					gfx = c.ItemID.Value;
				ClientCommunication.SendToClient( new UnicodeMessage( m_Bag, gfx, Assistant.MessageType.Label, 0x3B2, 3, Language.CliLocName, "", Language.GetString( LocString.ScavengerHB ) ) );
			}
		}

		public override void Clear()
		{
			m_Items.Clear();
			m_BagRef = null;
		}

		public bool Enabled { get{ return m_Enabled; } }
		public override string Name{ get{ return Language.GetString( LocString.Scavenger ); } }
		public override void OnSelected( ListBox subList, params Button[] buttons )
		{
			buttons[0].Text = Language.GetString( LocString.AddTarg );
			buttons[0].Visible = true;
			buttons[1].Text = Language.GetString( LocString.Remove );
			buttons[1].Visible = true;
			buttons[2].Text = Language.GetString( LocString.SetHB );
			buttons[2].Visible = true;
			buttons[3].Text = Language.GetString( LocString.ClearList );
			buttons[3].Visible = true;
			buttons[4].Text = Language.GetString( LocString.ClearScavCache );
			buttons[4].Visible = true;
			m_EnButton = buttons[5];
			m_EnButton.Visible = true;
			UpdateEnableButton();

			m_SubList = subList;
			subList.BeginUpdate();
			subList.Items.Clear();
			
			for(int i=0;i<m_Items.Count;i++)
				subList.Items.Add( m_Items[i] );
			subList.EndUpdate();
		}

		private void UpdateEnableButton()
		{
			m_EnButton.Text = Language.GetString( m_Enabled ? LocString.PushDisable : LocString.PushEnable );
		}
		
		public override void OnButtonPress( int num )
		{
			DebugLog( "User pressed button {0}", num );
			switch ( num )
			{
				case 1:
					World.Player.SendMessage( MsgLevel.Force, LocString.TargItemAdd );
					Targeting.OneTimeTarget( new Targeting.TargetResponseCallback( OnTarget ) );
					break;
				case 2:
					if ( m_SubList.SelectedIndex >= 0 && m_SubList.SelectedIndex < m_Items.Count )
					{
						m_Items.RemoveAt( m_SubList.SelectedIndex );
						m_SubList.Items.RemoveAt( m_SubList.SelectedIndex );
					}
					break;
				case 3:
					World.Player.SendMessage( LocString.TargCont );
					Targeting.OneTimeTarget( new Targeting.TargetResponseCallback( OnTargetBag ) );
					break;
				case 4:
					m_SubList.Items.Clear();
					m_Items.Clear();
					break;
				case 5:
					ClearCache();
					break;
				case 6:
					m_Enabled = !m_Enabled;
					UpdateEnableButton();
					break;
			}
		}

		private void ClearCache()
		{
			DebugLog( "Clearing Cache of {0} items", m_Cache == null ? -1 : m_Cache.Count );
			if ( m_Cache != null )
				m_Cache.Clear();
			if ( World.Player != null )
				World.Player.SendMessage( MsgLevel.Force, "Scavenger agent item cache cleared." );
		}
		
		private void OnTarget( bool location, Serial serial, Point3D loc, ushort gfx )
		{
			Engine.MainWindow.ShowMe();

			if ( location || !serial.IsItem )
				return;

			Item item = World.FindItem( serial );
			if ( item == null )
				return;

			m_Items.Add( item.ItemID );
			m_SubList.Items.Add( item.ItemID );

			DebugLog( "Added item {0}", item );
				
			World.Player.SendMessage( MsgLevel.Force, LocString.ItemAdded );
		}

		private void OnTargetBag( bool location, Serial serial, Point3D loc, ushort gfx )
		{
			Engine.MainWindow.ShowMe();

			if ( location || !serial.IsItem )
				return;

			if ( m_BagRef == null )
				m_BagRef = World.FindItem( m_Bag );
			if ( m_BagRef != null )
			{
				m_BagRef.ObjPropList.Remove( Language.GetString( LocString.ScavengerHB ) );
				m_BagRef.OPLChanged();
			}

			DebugLog( "Set bag to {0}", serial );
			m_Bag = serial;
			m_BagRef = World.FindItem( m_Bag );
			if ( m_BagRef != null )
			{
				m_BagRef.ObjPropList.Add( Language.GetString( LocString.ScavengerHB ) );
				m_BagRef.OPLChanged();
			}

			World.Player.SendMessage( MsgLevel.Force, LocString.ContSet, m_Bag );
		}

		public void Uncache( Serial s )
		{
			if ( m_Cache != null )
				m_Cache.Remove( s );
		}
		
		public void Scavenge( Item item )
		{
			DebugLog( "Checking WorldItem {0} ...", item );
			if ( !m_Enabled || !m_Items.Contains( item.ItemID ) || World.Player.Backpack == null || World.Player.IsGhost || World.Player.Weight >= World.Player.MaxWeight )
			{
				DebugLog( "... skipped." );
				return;
			}

			if ( m_Cache == null )
				m_Cache = new ArrayList( 200 );
			else if ( m_Cache.Count >= 190 )
				m_Cache.RemoveRange( 0, 50 );
			
			if ( m_Cache.Contains( item.Serial ) )
			{
				DebugLog( "Item was cached." );
				return;
			}

			Item bag = m_BagRef;
			if ( bag == null || bag.Deleted )
				bag = m_BagRef = World.FindItem( m_Bag );
			if ( bag == null || bag.Deleted || !bag.IsChildOf( World.Player.Backpack ) )
				bag = World.Player.Backpack;

			m_Cache.Add( item.Serial );
			DragDropManager.DragDrop( item, bag );
			DebugLog( "Dragging to {0}!", bag );
		}

		private static void DebugLog( string str, params object[] args )
		{
			if ( Debug )
			{
				using ( System.IO.StreamWriter w = new System.IO.StreamWriter( "Scavenger.log", true ) )
				{
					w.Write( DateTime.Now.ToString( "HH:mm:ss.fff" ) );
					w.Write( ":: " );
					w.WriteLine( str, args );
					w.Flush();
				}
			}
		}

		public override void Save( XmlTextWriter xml )
		{
			xml.WriteAttributeString( "enabled", m_Enabled.ToString() );

			if ( m_Bag != Serial.Zero )
			{
				xml.WriteStartElement( "bag" );
				xml.WriteAttributeString( "serial", m_Bag.ToString() );
				xml.WriteEndElement();
			}

			for(int i=0;i<m_Items.Count;i++)
			{
				xml.WriteStartElement( "item" );
				xml.WriteAttributeString( "id", ((ItemID)m_Items[i]).Value.ToString() );
				xml.WriteEndElement();
			}
		}
		
		public override void Load( XmlElement node )
		{
			try
			{
				m_Enabled = Boolean.Parse( node.GetAttribute( "enabled" ) );
			}
			catch
			{
				m_Enabled = false;
			}

			try
			{
				m_Bag = Serial.Parse( node["bag"].GetAttribute( "serial" ) );
			}
			catch
			{
				m_Bag = Serial.Zero;
			}

			foreach ( XmlElement el in node.GetElementsByTagName( "item" ) )
			{
				try
				{
					string iid = el.GetAttribute( "id" );
					m_Items.Add( (ItemID)Convert.ToUInt16( iid )  );
				}
				catch
				{
				}
			}
		}
	}

	public class BuyAgent : Agent
	{
		private class BuyEntry
		{
			public BuyEntry( ushort id, ushort amount )
			{
				Id = id;
				Amount = amount;
			}

			public ushort Id;
			public ushort Amount;
			public ItemID ItemID{ get{ return (ItemID)Id; } }

			public override string ToString()
			{
				return String.Format( "{0}\t{1}", ItemID, Amount );
			}
		}

		private class ItemXYComparer : IComparer
		{
			public static readonly ItemXYComparer Instance = new ItemXYComparer();

			private ItemXYComparer()
			{
			}

			public int Compare(object x, object y)
			{
				if ( !(x is Item) )
					return 1;
				else if ( !(y is Item ) )
					return -1;
				
				int xsum = ((Item)x).Position.X + ((Item)x).Position.Y*200;
				int ysum = ((Item)y).Position.X + ((Item)y).Position.Y*200;

				return xsum.CompareTo( ysum );
			}
		}

		private static ArrayList m_Instances = new ArrayList();

		public static void Initialize()
		{
			PacketHandler.RegisterServerToClientViewer( 0x74, new PacketViewerCallback( ExtBuyInfo ) );
			PacketHandler.RegisterServerToClientViewer( 0x24, new PacketViewerCallback( DisplayBuy ) );

			for(int i=1;i<=5;i++)
			{
				BuyAgent b = new BuyAgent( i );
				m_Instances.Add( b );
				Agent.Add( b );
			}
		}
		
		private ListBox m_SubList;
		private Button m_EnableBTN;
		private ArrayList m_Items;
		private bool m_Enabled;
		private int m_Num;

		public BuyAgent( int num )
		{
			m_Num = num;
			m_Items = new ArrayList();
		}
		
		private static void DisplayBuy( PacketReader p, PacketHandlerEventArgs args )
		{
			Serial serial = p.ReadUInt32();
			ushort gump = p.ReadUInt16();

			if ( gump != 0x30 || !serial.IsMobile || !ClientCommunication.AllowBit( FeatureBit.BuyAgent ) || World.Player == null )
				return;

			Mobile vendor = World.FindMobile( serial );
			if ( vendor == null )
				return;

			Item pack = vendor.GetItemOnLayer( Layer.ShopBuy );
			if ( pack == null || pack.Contains == null || pack.Contains.Count <= 0 )
				return;

			pack.Contains.Sort( ItemXYComparer.Instance );

			int total = 0;
			int cost = 0;
			ArrayList buyList = new ArrayList();
			Hashtable found = new Hashtable();
			bool lowGoldWarn = false;
			for (int i=0;i<pack.Contains.Count;i++)
			{
				Item item = (Item)pack.Contains[i];
				if ( item == null )
					continue;

				foreach ( BuyAgent ba in m_Instances )
				{
					if ( ba == null || ba.m_Items == null || !ba.m_Enabled )
						continue;

					for(int a=0;a<ba.m_Items.Count;a++)
					{
						BuyEntry b = (BuyEntry)ba.m_Items[a];
						if ( b == null )
							continue;

						bool dupe = false;
						foreach ( VendorBuyItem vbi in buyList )
						{
							if ( vbi.Serial == item.Serial )
								dupe = true;
						}

						if ( dupe )
							continue;

						// fucking osi and their blank scrolls
						if ( b.Id == item.ItemID.Value || ( b.Id == 0x0E34 && item.ItemID.Value == 0x0EF3 ) || ( b.Id == 0x0EF3 && item.ItemID.Value == 0x0E34 ) )
						{
							int count = World.Player.Backpack.GetCount( b.Id );
							if ( found.ContainsKey( b.Id ) )
								count += (int)found[b.Id];
							if ( count < b.Amount && b.Amount > 0 )
							{
								count = b.Amount - count;
								if ( count > item.Amount )
									count = item.Amount;
								else if ( count <= 0 )
									continue;

								if ( !found.ContainsKey( b.Id ) )
									found.Add( b.Id, (int)count );
								else
									found[b.Id] = (int)found[b.Id] + (int)count;

								buyList.Add( new VendorBuyItem( item.Serial, count, item.Price ) );
								total += count;
								cost += item.Price*count;
							}
						}
					}
				}
			}

			if ( cost > World.Player.Gold && cost < 2000 && buyList.Count > 0 )
			{
				lowGoldWarn = true;
				do
				{
					VendorBuyItem vbi = (VendorBuyItem)buyList[0];
					if ( cost - vbi.TotalCost <= World.Player.Gold )
					{
						while ( cost > World.Player.Gold && vbi.Amount > 0 )
						{
							cost -= vbi.Price;
							--vbi.Amount;
							--total;
						}

						if ( vbi.Amount <= 0 )
							buyList.RemoveAt( 0 );
					}
					else
					{
						cost -= vbi.TotalCost;
						total -= vbi.Amount;
						buyList.RemoveAt( 0 );
					}
				} while ( cost > World.Player.Gold && buyList.Count > 0 );
			}

			if ( buyList.Count > 0 )
			{
				args.Block = true;
				ClientCommunication.SendToServer( new VendorBuyResponse( serial, buyList ) );
				World.Player.SendMessage( MsgLevel.Force, LocString.BuyTotals, total, cost );
			}
			if ( lowGoldWarn )
				World.Player.SendMessage( MsgLevel.Force, LocString.BuyLowGold );
		}

		private static void ExtBuyInfo( PacketReader p, PacketHandlerEventArgs args )
		{
			Serial ser = p.ReadUInt32();
			Item pack = World.FindItem( ser );
			if ( pack == null )
				return;

			byte count = p.ReadByte();
			if ( count < pack.Contains.Count )
			{
				World.Player.SendMessage( MsgLevel.Debug, "Buy Agent Warning: Contains Count {0} does not match ExtInfo {1}.", pack.Contains.Count, count );
			}

			pack.Contains.Sort( ItemXYComparer.Instance );
	
			for ( int i = count-1; i >= 0; i-- )
			{
				if ( i < pack.Contains.Count )
				{
					Item item = (Item)pack.Contains[i];
					item.Price = p.ReadInt32();
					byte len = p.ReadByte();
					item.BuyDesc = p.ReadStringSafe( len );
				}
				else
				{
					p.ReadInt32();
					p.Position += p.ReadByte() + 1;
				}
			}
		}

		public override void Clear()
		{
			m_Items.Clear();
		}

		public override string Name{ get{ return String.Format( "{0}-{1}", Language.GetString( LocString.Buy ), m_Num ); } }

		public override void OnSelected( ListBox subList, params Button[] buttons )
		{
			m_SubList = subList;
			m_EnableBTN = buttons[4];
			
			buttons[0].Text = Language.GetString( LocString.AddTarg );
			buttons[0].Visible = true;
			buttons[1].Text = Language.GetString( LocString.Edit );
			buttons[1].Visible = true;
			buttons[2].Text = Language.GetString( LocString.Remove );
			buttons[2].Visible = true;
			buttons[3].Text = Language.GetString( LocString.ClearList );
			buttons[3].Visible = true;
			buttons[4].Text = Language.GetString( m_Enabled ? LocString.PushDisable : LocString.PushEnable );
			buttons[4].Visible = true;

			m_SubList.BeginUpdate();
			m_SubList.Items.Clear();
			for (int i=0;i<m_Items.Count;i++)
				m_SubList.Items.Add( m_Items[i] );
			m_SubList.EndUpdate();

			if ( !ClientCommunication.AllowBit( FeatureBit.BuyAgent ) && Engine.MainWindow != null )
			{
				for (int i=0;i<buttons.Length;i++)
					Engine.MainWindow.LockControl( buttons[i] );
				Engine.MainWindow.LockControl( subList );
			}
		}
		
		public override void OnButtonPress( int num )
		{
			switch ( num )
			{
				case 1:
					World.Player.SendMessage( MsgLevel.Force, LocString.TargItemAdd );
					Targeting.OneTimeTarget( new Targeting.TargetResponseCallback( OnTarget ) );
					break;
				case 2:
					if ( m_SubList == null )
						break;
					if ( m_SubList.SelectedIndex >= 0 )
					{
						BuyEntry e = (BuyEntry)m_Items[m_SubList.SelectedIndex];
						ushort amount = e.Amount;
						if ( InputBox.Show( Engine.MainWindow, Language.GetString( LocString.EnterAmount ), Language.GetString( LocString.InputReq ), amount.ToString() ) )
						{
							e.Amount = (ushort)InputBox.GetInt( 1 );
							m_SubList.BeginUpdate();
							m_SubList.Items.Clear();
							for (int i=0;i<m_Items.Count;i++)
								m_SubList.Items.Add( m_Items[i] );
							m_SubList.EndUpdate();
						}
					}
					break;
				case 3:
					if ( m_SubList.SelectedIndex >= 0 )
					{
						m_Items.RemoveAt( m_SubList.SelectedIndex );
						m_SubList.Items.RemoveAt( m_SubList.SelectedIndex );
						m_SubList.SelectedIndex = -1;
					}
					break;
				case 4:
					m_SubList.Items.Clear();
					m_Items.Clear();
					break;
				case 5:
					m_Enabled = !m_Enabled;
					m_EnableBTN.Text = Language.GetString( m_Enabled ? LocString.PushDisable : LocString.PushEnable );
					break;
			}
		}

		private void OnTarget( bool location, Serial serial, Point3D loc, ushort gfx )
		{
			Engine.MainWindow.ShowMe();

			if ( !location && !serial.IsMobile )
			{
				if ( InputBox.Show( Engine.MainWindow, Language.GetString( LocString.EnterAmount ), Language.GetString( LocString.InputReq ) ) )
				{
					ushort count = (ushort)InputBox.GetInt( 0 );
					if ( count <= 0 )
						return;
					BuyEntry be = new BuyEntry( gfx, count );
					m_Items.Add( be );
					if ( m_SubList != null )
						m_SubList.Items.Add( be );
				}
			}
		}

		public override void Save( XmlTextWriter xml )
		{
			if ( m_Items == null )
				return;

			xml.WriteAttributeString( "enabled", m_Enabled.ToString() );
			
			foreach ( BuyEntry b in m_Items )
			{
				xml.WriteStartElement( "item" );
				xml.WriteAttributeString( "id", b.Id.ToString() );
				xml.WriteAttributeString( "amount", b.Amount.ToString() );
				xml.WriteEndElement();
			}
		}
		
		public override void Load( XmlElement node )
		{
			try
			{
				m_Enabled = Boolean.Parse( node.GetAttribute( "enabled" ) );
			}
			catch
			{
				m_Enabled = false;
			}

			foreach ( XmlElement el in node.GetElementsByTagName( "item" ) )
			{
				try
				{
					ushort id, amount;
					id = Convert.ToUInt16( el.GetAttribute( "id" ) );
					amount = Convert.ToUInt16( el.GetAttribute( "amount" ) );

					m_Items.Add( new BuyEntry( id, amount ) );
				}
				catch
				{
				}
			}
		}
	}

	public class RestockAgent : Agent
	{
		public static void Initialize()	
		{ 
			for (int i=1;i<=5;i++)
				Agent.Add( new RestockAgent( i ) ); 
		}

		private ListBox m_SubList;
		private ArrayList m_Items;
		private Button m_HotBTN;
		private Serial m_HotBag;
		private int m_Num;
		
		public RestockAgent( int num )
		{
			m_Num = num;

			m_Items = new ArrayList();
			
			HotKey.Add( HKCategory.Agents, HKSubCat.None, String.Format( "{0}-{1}", Language.GetString( LocString.RestockAgent ), m_Num ), new HotKeyCallback( OnHotKey ) );
			HotKey.Add( HKCategory.Agents, HKSubCat.None, String.Format( "{0}-{1}", Language.GetString( LocString.SetRestockHB ), m_Num ), new HotKeyCallback( SetHB ) );
			PacketHandler.RegisterClientToServerViewer( 0x09, new PacketViewerCallback( OnSingleClick ) );

			Agent.OnItemCreated += new ItemCreatedEventHandler( CheckHBOPL );
		}

		public void CheckHBOPL( Item item )
		{
			if ( item.Serial == m_HotBag )
				item.ObjPropList.Add( Language.Format( LocString.RestockHBA1, m_Num ) );
		}

		private void OnSingleClick( PacketReader pvSrc, PacketHandlerEventArgs args )
		{
			Serial serial = pvSrc.ReadUInt32();
			if ( m_HotBag == serial )
			{
				ushort gfx = 0;
				Item c = World.FindItem( m_HotBag );
				if ( c != null )
					gfx = c.ItemID.Value;
				ClientCommunication.SendToClient( new UnicodeMessage( m_HotBag, gfx, Assistant.MessageType.Label, 0x3B2, 3, Language.CliLocName, "", Language.Format( LocString.RestockHBA1, m_Num ) ) );
			}
		}

		public override void Clear()
		{
			m_Items.Clear();
		}

		public override string Name{ get{ return String.Format( "{0}-{1}", Language.GetString( LocString.Restock ), m_Num ); } }

		public override void OnSelected( ListBox subList, params Button[] buttons )
		{
			buttons[0].Text = Language.GetString( LocString.AddTarg );
			buttons[0].Visible = true;
			buttons[1].Text = Language.GetString( LocString.Remove );
			buttons[1].Visible = true;
			buttons[2].Text = Language.GetString( LocString.SetAmt );
			buttons[2].Visible = true;
			buttons[3].Text = Language.GetString( LocString.ClearList );
			buttons[3].Visible = true;
			m_HotBTN = buttons[4];
			SetHBText();
			buttons[4].Visible = true;
			buttons[5].Text = Language.GetString( LocString.RestockNow );
			buttons[5].Visible = true;
			
			m_SubList = subList;
			subList.BeginUpdate();
			subList.Items.Clear();
			for(int i=0;i<m_Items.Count;i++)
				subList.Items.Add( m_Items[i] );
			subList.EndUpdate();

			if ( !ClientCommunication.AllowBit( FeatureBit.RestockAgent ) && Engine.MainWindow != null )
			{
				for (int i=0;i<buttons.Length;i++)
					Engine.MainWindow.LockControl( buttons[i] );
				Engine.MainWindow.LockControl( subList );
			}
		}

		public override void OnButtonPress( int num )
		{
			switch ( num )
			{
				case 1:
				{
					Targeting.OneTimeTarget( new Targeting.TargetResponseCallback( OnItemTarget ) );
					World.Player.SendMessage( MsgLevel.Force, LocString.TargItemAdd );
					break;
				}
				case 2:
				{
					if ( m_SubList.SelectedIndex >= 0 && m_SubList.SelectedIndex < m_Items.Count )
					{
						m_Items.RemoveAt( m_SubList.SelectedIndex );
						m_SubList.Items.RemoveAt( m_SubList.SelectedIndex );
					}
					break;
				}
				case 3:
				{
					int i = m_SubList.SelectedIndex;
					if ( i < 0 || i > m_Items.Count )
						return;

					RestockItem ri = (RestockItem)m_Items[i];
					if ( !InputBox.Show( Engine.MainWindow, Language.GetString( LocString.EnterAmount ), Language.GetString( LocString.InputReq ), ri.Amount.ToString() ) )
						return;
					
					ri.Amount = InputBox.GetInt( ri.Amount );

					m_SubList.BeginUpdate();
					m_SubList.Items.Clear();
					for(int j=0;j<m_Items.Count;j++)
						m_SubList.Items.Add( m_Items[j] );
					m_SubList.SelectedIndex = i;
					m_SubList.EndUpdate();					
					break;
				}
				case 4:
				{
					m_SubList.Items.Clear();
					m_Items.Clear();
					break;
				}
				case 5:
				{
					if ( m_HotBag == Serial.Zero )
					{
						SetHB();
					}
					else
					{
						m_HotBag = Serial.Zero;
						SetHBText();
					}
					break;
				}
				case 6:
				{
					OnHotKey();
					break;
				}
			}
		}

		private void SetHBText()
		{
			if ( m_HotBTN != null )
				m_HotBTN.Text = Language.GetString( m_HotBag == Serial.Zero ? LocString.SetHB : LocString.ClearHB );
		}

		private void SetHB()
		{
			World.Player.SendMessage( MsgLevel.Force, LocString.TargCont );
			Targeting.OneTimeTarget( new Targeting.TargetResponseCallback( OnHBTarget ) );
		}

		private void OnHBTarget( bool location, Serial serial, Point3D loc, ushort gfx )
		{
			Engine.MainWindow.ShowMe();

			Item hb = World.FindItem( m_HotBag );
			if ( hb != null )
			{
				if ( hb.ObjPropList.Remove( Language.Format( LocString.RestockHBA1, m_Num ) ) )
					hb.OPLChanged();
			}

			if ( !location && serial.IsItem )
				m_HotBag = serial;
			else
				m_HotBag = Serial.Zero;

			hb = World.FindItem( m_HotBag );
			if ( hb != null )
			{
				hb.ObjPropList.Add( Language.Format( LocString.RestockHBA1, m_Num ) );
				hb.OPLChanged();
			}

			SetHBText();
		}

		private void OnHotKey()
		{
			if ( ClientCommunication.AllowBit( FeatureBit.RestockAgent ) )
			{
				World.Player.SendMessage( MsgLevel.Force, LocString.RestockTarget );
				Targeting.OneTimeTarget( new Targeting.TargetResponseCallback( OnRestockTarget ) );
			}
		}

		Item m_Cont = null;
		private void OnRestockTarget( bool location, Serial serial, Point3D loc, ushort gfx )
		{
			if ( serial == World.Player.Serial )
			{
				m_Cont = World.Player.GetItemOnLayer( Layer.Bank ); 
			}
			else if ( serial.IsItem )
			{
				m_Cont = World.FindItem( serial );
				if ( m_Cont != null )
				{
					object root = m_Cont.RootContainer ;
					if ( root is Mobile && root != World.Player )
						m_Cont = null;
				}
			}

			if ( m_Cont == null || m_Cont.IsCorpse )
			{
				World.Player.SendMessage( MsgLevel.Force, LocString.InvalidCont );
				return;
			}

			if ( Utility.Distance( World.Player.Position, m_Cont.GetWorldPosition() ) > 3 )
			{
				World.Player.SendMessage( MsgLevel.Error, LocString.TooFar );
			}
			else
			{
				if ( m_Cont.IsContainer && m_Cont.Layer != Layer.Bank )
				{
					PlayerData.DoubleClick( m_Cont );
					Timer.DelayedCallback( TimeSpan.FromMilliseconds( Config.GetInt( "ObjectDelay" ) + 200 ), new TimerCallback( DoRestock ) ).Start();
					World.Player.SendMessage( LocString.RestockQueued );
				}
				else
				{
					DoRestock();
				}
			}
		}

		private void DoRestock()
		{
			Item bag = null;
			if ( m_HotBag != Serial.Zero )
			{
				bag = World.FindItem( m_HotBag );
				if ( bag != null && bag.RootContainer != World.Player )
					bag = null;
			}

			if ( bag == null )
			{
				bag = World.Player.Backpack;
				if ( bag == null )
				{
					World.Player.SendMessage( MsgLevel.Force, LocString.NoBackpack );
					return;
				}
			}

			int num = 0;
			for (int i=0;i<m_Items.Count;i++)
			{
				RestockItem ri = (RestockItem)m_Items[i];
				int count = World.Player.Backpack.GetCount( ri.ItemID );

				num += Recurse( bag, m_Cont.Contains, ri, ref count );
			}
			World.Player.SendMessage( MsgLevel.Force, LocString.RestockDone, num, num != 1 ? "s" : "" );
		}

		private int Recurse( Item pack, ArrayList items, RestockItem ri, ref int count )
		{
			int num = 0;
			for(int i=0;count < ri.Amount && i<items.Count;i++)
			{
				Item item = (Item)items[i];

				if ( item.ItemID == ri.ItemID )
				{
					int amt = ri.Amount - count;
					if ( amt > item.Amount )
						amt = item.Amount;
					DragDropManager.DragDrop( item, amt, pack );
					count += amt;
					num ++;
				}
				else if ( item.Contains.Count > 0 )
				{
					num += Recurse( pack, item.Contains, ri, ref count );
				}
			}

			return num;
		}

		private void OnItemTarget( bool location, Serial serial, Point3D loc, ushort gfx )
		{
			if ( location || serial.IsMobile )
				return;

			Item item = World.FindItem( serial );
			if ( item != null )
				gfx = item.ItemID;
			
			if ( gfx == 0 || gfx >= 0x4000 )
				return;

			if ( !InputBox.Show( Engine.MainWindow, Language.GetString( LocString.EnterAmount ), Language.GetString( LocString.InputReq ), "1" ) )
				return;

			RestockItem ri = new RestockItem( gfx, InputBox.GetInt( 1 ) );
			m_Items.Add( ri );
			m_SubList.Items.Add( ri );
				
			World.Player.SendMessage( MsgLevel.Force, LocString.ItemAdded );

			Engine.MainWindow.ShowMe();
		}

		public override void Save( XmlTextWriter xml )
		{
			xml.WriteAttributeString( "hotbag", m_HotBag.Value.ToString() );
			for(int i=0;i<m_Items.Count;i++)
			{
				xml.WriteStartElement( "item" );
				RestockItem ri = (RestockItem)m_Items[i];
				xml.WriteAttributeString( "id", ri.ItemID.Value.ToString() );
				xml.WriteAttributeString( "amount", ri.Amount.ToString() );
				xml.WriteEndElement();
			}
		}
		
		public override void Load( XmlElement node )
		{
			try
			{
				m_HotBag = Convert.ToUInt32( node.GetAttribute( "hotbag" ) );
			}
			catch
			{
				m_HotBag = Serial.Zero;
			}

			foreach ( XmlElement el in node.GetElementsByTagName( "item" ) )
			{
				try
				{
					string iid = el.GetAttribute( "id" );
					string amt = el.GetAttribute( "amount" );
					m_Items.Add( new RestockItem( (ItemID)Convert.ToInt32( iid ), Convert.ToInt32( amt ) ) );
				}
				catch
				{
				}
			}
		}

		class RestockItem
		{
			public ItemID ItemID;
			public int Amount;
			public RestockItem( ItemID id, int amount )
			{
				ItemID = id;
				Amount = amount;
			}

			public override string ToString()
			{
				return String.Format( "{0}\t\t{1}", ItemID, Amount );
			}
		}
	}

	public class FriendsAgent : Agent
	{
		private static FriendsAgent m_Instance = new FriendsAgent();
		public static void Initialize()	
		{ 
			Agent.Add( m_Instance ); 
		}

		public static bool IsFriend( Mobile m )
		{
			return m_Instance.IsFriend( m.Serial );
		}

		private ListBox m_SubList;
		private ArrayList m_Chars;
		private Hashtable m_Names;
		private bool m_Enabled;
		private Button m_EnableBTN;

		public FriendsAgent()
		{
			m_Chars = new ArrayList();
			m_Names = new Hashtable();

			HotKey.Add( HKCategory.Targets, LocString.AddFriend, new HotKeyCallback( AddToFriendsList ) );
			HotKey.Add( HKCategory.Targets, LocString.RemoveFriend, new HotKeyCallback( RemoveFromFriendsList ) );

			Agent.OnMobileCreated += new MobileCreatedEventHandler( OPLCheckFriend );
		}

		public override void Clear()
		{
			m_Chars.Clear();
			m_Names.Clear();
		}

		public bool IsFriend( Serial ser )
		{
			if ( m_Enabled )
				return m_Chars.Contains( ser ) || ( Config.GetBool( "AutoFriend" ) && PacketHandlers.Party.Contains( ser ) );
			else
				return false;
		}

		public override string Name{ get{ return Language.GetString( LocString.Friends ); } }

		public override void OnSelected( ListBox subList, params Button[] buttons )
		{
			m_EnableBTN = buttons[4];

			buttons[0].Text = Language.GetString( LocString.AddTarg );
			buttons[0].Visible = true;
			buttons[1].Text = Language.GetString( LocString.Remove );
			buttons[1].Visible = true;
			buttons[2].Text = Language.GetString( LocString.RemoveTarg );
			buttons[2].Visible = true;
			buttons[3].Text = Language.GetString( LocString.ClearList );
			buttons[3].Visible = true;
			buttons[4].Text = Language.GetString( m_Enabled ? LocString.PushDisable : LocString.PushEnable );
			buttons[4].Visible = true;
			
			m_SubList = subList;
			subList.BeginUpdate();
			subList.Items.Clear();
			for(int i=0;i<m_Chars.Count;i++)
				Add2List( (Serial)m_Chars[i] );
			subList.EndUpdate();
		}

		public void AddToFriendsList()
		{
			World.Player.SendMessage( MsgLevel.Force, LocString.TargFriendAdd );
			Targeting.OneTimeTarget( new Targeting.TargetResponseCallback( OnAddTarget ) );
		}

		public void RemoveFromFriendsList()
		{
			World.Player.SendMessage( MsgLevel.Force, LocString.TargFriendRem );
			Targeting.OneTimeTarget( new Targeting.TargetResponseCallback( OnRemoveTarget ) );
		}

		public override void OnButtonPress( int num )
		{
			switch ( num )
			{
				case 1:
				{
					AddToFriendsList();
					break;
				}
				case 2:
				{		
					if ( m_SubList.SelectedIndex >= 0 && m_SubList.SelectedIndex <  m_Chars.Count )
					{
						try
						{
							m_Names.Remove( m_Chars[m_SubList.SelectedIndex] );
						}
						catch
						{
						}
						m_Chars.RemoveAt( m_SubList.SelectedIndex );
						m_SubList.Items.RemoveAt( m_SubList.SelectedIndex );
					}
					break;
				}
				case 3:
				{
					RemoveFromFriendsList();
					break;
				}
				case 4:
				{
					foreach ( Serial s in m_Chars )
					{
						Mobile m = World.FindMobile( s );
						if ( m != null )
						{
							if ( m.ObjPropList.Remove( Language.GetString( LocString.RazorFriend ) ) )
								m.OPLChanged();
						}
					}
					m_Chars.Clear();
					m_SubList.Items.Clear();
					break;
				}
				case 5:
				{
					m_Enabled = !m_Enabled;
					m_EnableBTN.Text = Language.GetString( m_Enabled ? LocString.PushDisable : LocString.PushEnable );
					break;
				}
			}
		}

		private void OPLCheckFriend( Mobile m )
		{
			if ( IsFriend( m ) )
				m.ObjPropList.Add( Language.GetString( LocString.RazorFriend ) );
		}

		private void OnAddTarget( bool location, Serial serial, Point3D loc, ushort gfx )
		{
			Engine.MainWindow.ShowMe();

			if ( !location && serial.IsMobile && serial != World.Player.Serial )
			{
				World.Player.SendMessage( MsgLevel.Force, LocString.FriendAdded );
				if ( !m_Chars.Contains( serial ) )
				{
					m_Chars.Add( serial );

					Add2List( serial );

					Mobile m = World.FindMobile( serial );
					if ( m != null )
					{
						m.ObjPropList.Add( Language.GetString( LocString.RazorFriend ) );
						m.OPLChanged();
					}
				}
			}
		}

		private void Add2List( Serial s )
		{
			Mobile m = World.FindMobile( s );
			string name = null;

			if ( m_Names.ContainsKey( s ) )
				name = m_Names[s] as string;

			if ( m != null && m.Name != null && m.Name != "" )
				name = m.Name;

			if ( name == null )
				name = "(Name Unknown)";

			m_Names[s] = name;

			if ( m_SubList != null )
				m_SubList.Items.Add( String.Format( "\"{0}\" {1}", name, s ) );
		}

		private void OnRemoveTarget( bool location, Serial serial, Point3D loc, ushort gfx )
		{
			Engine.MainWindow.ShowMe();

			if ( !location && serial.IsMobile && serial != World.Player.Serial )
			{
				m_Chars.Remove( serial );
				m_Names.Remove( serial );

				World.Player.SendMessage( MsgLevel.Force, LocString.FriendRemoved );

				m_SubList.BeginUpdate();
				m_SubList.Items.Clear();
				for(int i=0;i<m_Chars.Count;i++)
					Add2List( (Serial)m_Chars[i] );
				m_SubList.EndUpdate();

				Mobile m = World.FindMobile( serial );
				if ( m != null )
				{
					if ( m.ObjPropList.Remove( Language.GetString( LocString.RazorFriend ) ) )
						m.OPLChanged();
				}
			}
		}

		public override void Save( XmlTextWriter xml )
		{
			xml.WriteAttributeString( "enabled", m_Enabled.ToString() );
			for(int i=0;i<m_Chars.Count;i++)
			{
				xml.WriteStartElement( "friend" );
				xml.WriteAttributeString( "serial", m_Chars[i].ToString() );
				try
				{
					if ( m_Names.ContainsKey( (Serial)m_Chars[i] ) )
						xml.WriteAttributeString( "name", m_Names[(Serial)m_Chars[i]].ToString() );
				}
				catch
				{
				}
				xml.WriteEndElement();
			}
		}
		
		public override void Load( XmlElement node )
		{
			try
			{
				m_Enabled = Convert.ToBoolean( node.GetAttribute( "enabled" ) );
			}
			catch
			{
			}

			foreach ( XmlElement el in node.GetElementsByTagName( "friend" ) )
			{
				try
				{
					Serial toAdd = Serial.Parse( el.GetAttribute( "serial" ) );

					if ( !m_Chars.Contains( toAdd ) )
						m_Chars.Add( toAdd );

					string name = el.GetAttribute( "name" );
					if ( name != null && name != "" )
						m_Names.Add( toAdd, name.Trim() );
				}
				catch
				{
				}
			}
		}
	}
}

