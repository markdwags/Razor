using System;
using System.Collections;
using System.Windows.Forms;
using System.Xml;

namespace Assistant.Filters
{
	public abstract class Filter
	{
		private static ArrayList m_Filters = new ArrayList();
		public static ArrayList List { get{ return m_Filters; } }

		public static void Register( Filter filter )
		{
			m_Filters.Add( filter );
		}

		public static void Load( XmlElement xml )
		{
			DisableAll();

			if ( xml == null )
				return; 
 
			foreach( XmlElement el in xml.GetElementsByTagName( "filter" ) )
			{
				try
				{
					LocString name = (LocString)Convert.ToInt32( el.GetAttribute( "name" ) );
					string enable = el.GetAttribute( "enable" );

					for(int i=0;i<m_Filters.Count;i++)
					{
						Filter f = (Filter)m_Filters[i];
						if ( f.Name == name )
						{
							if ( Convert.ToBoolean( enable ) )
								f.OnEnable();
							break;
						}
					}
				}
				catch
				{
				}
			}
		}

		public static void DisableAll()
		{
			for(int i=0;i<m_Filters.Count;i++)
				((Filter)m_Filters[i]).OnDisable();
		}

		public static void Save( XmlTextWriter xml )
		{
			for (int i=0;i<m_Filters.Count;i++)
			{
				Filter f = (Filter)m_Filters[i];
				if ( f.Enabled )
				{
					xml.WriteStartElement( "filter" );
					xml.WriteAttributeString( "name", ((int)f.Name).ToString() );
					xml.WriteAttributeString( "enable", f.Enabled.ToString() );
					xml.WriteEndElement();
				}
			}
		}

		public static void Draw( CheckedListBox list )
		{
			list.BeginUpdate();
			list.Items.Clear();

			for (int i=0;i<m_Filters.Count;i++)
			{
				Filter f = (Filter)m_Filters[i];
				list.Items.Add( f );
				list.SetItemChecked( i, f.Enabled );
			}
			list.EndUpdate();
		}

		public abstract void OnFilter( PacketReader p, PacketHandlerEventArgs args );
		public abstract byte[] PacketIDs{ get; }
		public abstract LocString Name{ get; }

		public bool Enabled{get{return m_Enabled;}}
		private bool m_Enabled;
		private PacketViewerCallback m_Callback;

		protected Filter()
		{
			m_Enabled = false;
			m_Callback = new PacketViewerCallback( this.OnFilter );
		}

		public override string ToString()
		{
			return Language.GetString( this.Name );
		}

		public virtual void OnEnable()
		{
			m_Enabled = true;
			for(int i=0;i<PacketIDs.Length;i++)
				PacketHandler.RegisterServerToClientViewer( PacketIDs[i], m_Callback );
		}

		public virtual void OnDisable()
		{
			m_Enabled = false;
			for(int i=0;i<PacketIDs.Length;i++)
				PacketHandler.RemoveServerToClientViewer( PacketIDs[i], m_Callback );
		}

		public void OnCheckChanged( CheckState newValue )
		{
			if ( Enabled && newValue == CheckState.Unchecked  )
				OnDisable();
			else if ( !Enabled && newValue == CheckState.Checked )
				OnEnable();
		}
	}
}

