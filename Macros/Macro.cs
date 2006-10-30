using System;
using System.Collections;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Text;
using Assistant;

namespace Assistant.Macros
{
	public class Macro
	{
		private string m_Path;
		private ArrayList m_Actions;
		private bool m_Recording, m_Playing;
		private MacroWaitAction m_Wait;
		private int m_CurrentAction;
		private bool m_Loop;
		private bool m_Loaded;
		private ListBox m_ListBox;
		private Stack m_IfStatus;

		public Macro( string path )
		{
			m_Actions = new ArrayList();
			m_Path = path;
			m_Loaded = false;
			m_IfStatus = new Stack();
		}

		public string Filename { get{ return m_Path; } set { m_Path = value; } }
		public ArrayList Actions { get{ return m_Actions; } }
		public bool Recording { get{ return m_Recording; } }
		public bool Playing { get{ return m_Playing; } }
		public bool Waiting{ get{ return m_Wait != null; } }
		public int CurrentAction { get{ return m_CurrentAction; } }

		public bool Loop
		{
			get { return m_Loop && ClientCommunication.AllowBit( FeatureBit.LoopingMacros ); }
			set { m_Loop = value; }
		}

		public void DisplayTo( ListBox list )
		{
			m_ListBox = list;

			m_ListBox.Items.Clear();
			
			if ( !m_Loaded )
				Load();

			m_ListBox.BeginUpdate();
			if ( m_Actions.Count > 0 )
				m_ListBox.Items.AddRange( (object[])m_Actions.ToArray( typeof( object ) ) );
			if ( m_Playing && m_CurrentAction >= 0 && m_CurrentAction < m_Actions.Count )
				m_ListBox.SelectedIndex = m_CurrentAction;
			else
				m_ListBox.SelectedIndex = -1;
			m_ListBox.EndUpdate();
		}

		public override string ToString()
		{
			//return Path.GetFileNameWithoutExtension( m_Path );
			StringBuilder sb = new StringBuilder( Path.GetFullPath( m_Path ) );
			sb.Remove( sb.Length-6, 6 );
			sb.Remove( 0, Path.Combine( Engine.BaseDirectory, "Macros" ).Length + 1 );
			return sb.ToString();
		}

		public void Insert( int idx, MacroAction a )
		{
			a.Parent = this;
			if ( idx < 0 || idx > m_Actions.Count )
				idx = m_Actions.Count;
			m_Actions.Insert( idx, a );
		}

		public void Record()
		{
			m_Actions.Clear();
			if ( m_ListBox != null )
				m_ListBox.Items.Clear();
			RecordAt( 0 );
		}

		public void RecordAt( int at )
		{
			Stop();
			m_Recording = true;
			m_Loaded = true;
			m_CurrentAction = at;
			if ( m_CurrentAction > m_Actions.Count )
				m_CurrentAction = m_Actions.Count;
		}
		
		public void Play()
		{
			Stop();
			if ( !m_Loaded )
				Load();
			else
				Save();

			if ( m_Actions.Count > 0 )
			{
				m_IfStatus.Clear();
				m_Playing = true;
				m_CurrentAction = -1;
				if ( m_ListBox != null )
					m_ListBox.SelectedIndex = -1;
			}
		}

		public void Stop()
		{
			if ( m_Recording )
				Save();
			if ( m_Playing && World.Player != null && DragDropManager.Holding != null && DragDropManager.Holding == LiftAction.LastLift )
				ClientCommunication.SendToServer( new DropRequest( DragDropManager.Holding, World.Player.Serial ) );

			m_Recording = m_Playing = false;
			m_Wait = null;
			m_IfStatus.Clear();
			
			bool resync = false;
			foreach ( MacroAction a in m_Actions )
			{
				if ( World.Player != null && a is WalkAction )
				{
					resync = true;
				}
				else if ( a is ForAction )
				{
					((ForAction)a).Count = 0;
				}
			}

			if ( resync )
			{
				// resync if the macro walked for us
				//ClientCommunication.SendToClient( new MoveReject( World.Player.WalkSequence, World.Player ) );
				ClientCommunication.SendToClient( new MobileIncoming( World.Player ) );
				ClientCommunication.SendToClient( new MobileIncoming( World.Player ) );
				ClientCommunication.SendToServer( new ResyncReq() );
				World.Player.Resync();
			}
		}

		// returns true if the were waiting for this action
		public bool Action( MacroAction action )
		{
			if ( m_Recording )
			{
				action.Parent = this;
				m_Actions.Insert( m_CurrentAction, action );
				if ( m_ListBox != null )
					m_ListBox.Items.Insert( m_CurrentAction, action );
				m_CurrentAction++;

				return false;
			}
			else if ( m_Playing && m_Wait != null && m_Wait.CheckMatch( action ) )
			{
				m_Wait = null;
				ExecNext();
				return true;
			}

			return false;
		}

		public void Save()
		{
			if ( m_Actions.Count == 0 )
				return;

			using ( StreamWriter writer = new StreamWriter( m_Path ) )
			{
				if ( m_Loop )
					writer.WriteLine( "!Loop" );

				for (int i=0;i<m_Actions.Count;i++)
				{
					MacroAction a = (MacroAction)m_Actions[i];
					try
					{
						writer.WriteLine( a.Serialize() );
					}
					catch
					{
					}
				}
			}
		}

		private static Type[] ctorArgs = new Type[1]{ typeof( string[] ) };
		public void Load()
		{
			m_Actions.Clear();
			m_Loaded = false;

			if ( !File.Exists( m_Path ) )
				return;

			using ( StreamReader reader = new StreamReader( m_Path ) )
			{
				string line;
				while ( (line=reader.ReadLine()) != null )
				{
					if ( line.Length <= 2 )
						continue;

					if ( line == "!Loop" )
					{
						m_Loop = true;
						continue;
					}

					if ( line[0] == '#' )
					{
						m_Actions.Add( new MacroComment( line.Substring( 1 ) ) );
						continue;
					}
					else if ( line[0] == '/' && line[1] == '/' )
					{
						MacroAction a = new MacroComment( line.Substring( 2 ) ); 
						a.Parent = this;
						m_Actions.Add( a );
						continue;
					}

					string[] args = line.Split( '|' );
					object[] invokeArgs = new object[1]{ args };
					
					Type at = null;
					try{ at = Type.GetType( args[0], false ); } catch {}
					if ( at == null )
						continue;

					if ( args.Length > 1 )
					{
						try
						{
							ConstructorInfo ctor = at.GetConstructor( ctorArgs );
							if ( ctor == null )
								continue;

							MacroAction a = (MacroAction)ctor.Invoke( invokeArgs );
							m_Actions.Add( a );
							a.Parent = this;
						}
						catch ( Exception e )
						{
							string blah = e.ToString();
							blah = blah;
						}
					}
					else
					{
						try
						{
							ConstructorInfo ctor = at.GetConstructor( Type.EmptyTypes );
							if ( ctor == null )
								continue;

							MacroAction a = (MacroAction)ctor.Invoke( null );
							m_Actions.Add( a );
							a.Parent = this;
						}
						catch
						{
						}
					}
				}
			}
			m_Loaded = true;
		}

		public void Convert( MacroAction old, MacroAction newAct )
		{
			for (int i=0;i<m_Actions.Count;i++)
			{
				if ( m_Actions[i] == old )
				{
					m_Actions[i] = newAct;
					newAct.Parent = this;
					Update();
					break;
				}
			}
		}

		public void Update()
		{
			if ( m_ListBox != null )
			{
				int sel = m_ListBox.SelectedIndex;
				DisplayTo( m_ListBox );
				try
				{
					m_ListBox.SelectedIndex = sel;
				}
				catch
				{
				}
			}
		}

		private bool NextIsInstantWait()
		{
			int nextAct = m_CurrentAction + 1;
			
			if ( nextAct >= 0 && nextAct < m_Actions.Count )
				return m_Actions[nextAct] is WaitForTargetAction || m_Actions[nextAct] is WaitForGumpAction || m_Actions[nextAct] is WaitForMenuAction ;//|| m_Actions[nextAct] is ElseAction || m_Actions[nextAct] is EndIfAction;
			else
				return false;
		}

		private static MacroWaitAction PauseB4Loop = new PauseAction( TimeSpan.FromSeconds( 0.1 ) );
		//return true to continue the macro, false to stop (macro's over)
		public bool ExecNext()
		{
			try
			{
				if ( !m_Playing )
					return false;

				if ( m_Wait != null )
				{
					TimeSpan waitLen = DateTime.Now - m_Wait.StartTime;
					if ( !( m_Wait is PauseAction ) && waitLen >= m_Wait.Timeout )
					{
						if ( Loop )
						{
							if ( Engine.MainWindow.WaitDisplay != null )
								Engine.MainWindow.WaitDisplay.Text = "";
							m_CurrentAction = -1;
							m_IfStatus.Clear();
							PauseB4Loop.Perform();
							PauseB4Loop.Parent = this;
							m_Wait = PauseB4Loop;
							return true;
						}
						else
						{
							Stop();
							return false;
						}
					}
					else 
					{
						if ( !m_Wait.PerformWait() )
						{
							m_Wait = null; // done waiting
							if ( Engine.MainWindow.WaitDisplay != null )
								Engine.MainWindow.WaitDisplay.Text = "";
						}
						else
						{
							if ( waitLen >= TimeSpan.FromSeconds( 4.0 ) && Engine.MainWindow.WaitDisplay != null )
							{
								StringBuilder sb = new StringBuilder( "Waiting... Timeout: " );
								int s = (int)(m_Wait.Timeout - waitLen).TotalSeconds;
								int m = 0;

								if ( s > 60 )
								{
									m = s/60;
									s %= 60;
									if ( m > 60 )
									{
										sb.AppendFormat( "{0}:", m/60 );
										m %= 60;
									}
								}

								sb.AppendFormat( "{0:00}:{1:00}", m, s );
								Engine.MainWindow.WaitDisplay.Text = sb.ToString();
							}
							return true; // keep waiting
						}
					}
				}

				m_CurrentAction ++;
				//MacroManager.ActionUpdate( this, m_CurrentAction );
				if ( m_ListBox != null )
				{
					if ( m_CurrentAction < m_ListBox.Items.Count )
						m_ListBox.SelectedIndex = m_CurrentAction;
					else
						m_ListBox.SelectedIndex = -1;
				}

				if ( m_CurrentAction >= 0 && m_CurrentAction < m_Actions.Count )
				{
					MacroAction action = (MacroAction)m_Actions[m_CurrentAction];

					if ( action is IfAction )
					{
						bool val = ((IfAction)action).Evaluate();
						m_IfStatus.Push( val );

						if ( !val )
						{
							// false so skip to an else or an endif
							int ifcount = 0;
							while ( m_CurrentAction+1 < m_Actions.Count )
							{
								if ( m_Actions[m_CurrentAction+1] is IfAction )
								{
									ifcount++;
								}
								else if ( m_Actions[m_CurrentAction+1] is ElseAction && ifcount <= 0 )
								{
									break;
								}
								else if ( m_Actions[m_CurrentAction+1] is EndIfAction )
								{
									if ( ifcount <= 0 )
										break;
									else
										ifcount--;
								}
								
								m_CurrentAction++;
							}
						}
					}
					else if ( action is ElseAction && m_IfStatus.Count > 0 )
					{
						bool val = (bool)m_IfStatus.Peek();
						if ( val )
						{
							// the if was true, so skip to an endif
							int ifcount = 0;
							while ( m_CurrentAction+1 < m_Actions.Count )
							{
								if ( m_Actions[m_CurrentAction+1] is IfAction )
								{
									ifcount++;
								}
								else if ( m_Actions[m_CurrentAction+1] is EndIfAction )
								{
									if ( ifcount <= 0 )
										break;
									else
										ifcount--;
								}
								
								m_CurrentAction++;
							}
						}
					}
					else if ( action is EndIfAction && m_IfStatus.Count > 0 )
					{
						m_IfStatus.Pop();
					}
					else if ( action is ForAction )
					{
						ForAction fa = (ForAction)action;
						fa.Count++;

						if ( fa.Count > fa.Max )
						{
							fa.Count = 0;

							int forcount = 0;
							m_CurrentAction++;
							while ( m_CurrentAction < m_Actions.Count )
							{
								if ( m_Actions[m_CurrentAction] is ForAction )
								{
									forcount++;
								}
								else if ( m_Actions[m_CurrentAction] is EndForAction )
								{
									if ( forcount <= 0 )
										break;
									else
										forcount--;
								}
								
								m_CurrentAction++;
							}

							if ( m_CurrentAction < m_Actions.Count )
								action = (MacroAction)m_Actions[m_CurrentAction];
						}
					}
					else if ( action is EndForAction && ClientCommunication.AllowBit( FeatureBit.LoopingMacros ) )
					{
						int ca = m_CurrentAction - 1;
						int forcount = 0;

						while ( ca >= 0 )
						{
							if ( m_Actions[ca] is EndForAction )
							{
								forcount--;
							}
							else if ( m_Actions[ca] is ForAction )
							{
								if ( forcount >= 0 )
									break;
								else
									forcount++;
							}

							ca--;
						}

						if ( ca >= 0 && m_Actions[ca] is ForAction )
							m_CurrentAction = ca - 1;
					}
					
					bool isWait = action is MacroWaitAction;
					if ( !action.Perform() && isWait )
					{
						m_Wait = (MacroWaitAction)action;
						m_Wait.StartTime = DateTime.Now;
					}
					else if ( NextIsInstantWait() && !isWait )
					{
						return ExecNext();
					}
				}
				else
				{
					if ( Engine.MainWindow.WaitDisplay != null )
						Engine.MainWindow.WaitDisplay.Text = "";

					if ( Loop )
					{
						m_CurrentAction = -1;
						m_IfStatus.Clear();

						PauseB4Loop.Perform();
						PauseB4Loop.Parent = this;
						m_Wait = PauseB4Loop;
					}
					else
					{
						Stop();
						return false;
					}
				}
			}
			catch ( Exception e )
			{
				new MessageDialog( "Macro Exception", true, e.ToString() ).Show();
				return false;
			}
			return true;
		}
	}
}

