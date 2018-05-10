using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Assistant.Macros
{
    public class MacroManager
    {
        private static List<Macro> m_MacroList;
        private static List<AbsoluteTarget> m_AbsoluteTargetList;
        private static Macro m_Current, m_PrevPlay;
        private static MacroTimer m_Timer;

        public static void Initialize()
        {
            HotKey.Add(HKCategory.Macros, LocString.StopCurrent, new HotKeyCallback(HotKeyStop));

            string path = Config.GetUserDirectory("Macros");
            Recurse(null, path);

        }

        static MacroManager()
        {
            m_MacroList = new List<Macro>();
            m_AbsoluteTargetList = new List<AbsoluteTarget>();
            m_Timer = new MacroTimer();
        }
        /// <summary>
        /// Saves all the macros and absolute target lists
        /// </summary>
        public static void Save()
        {
            Engine.EnsureDirectory(Config.GetUserDirectory("Macros"));

            foreach (Macro macro in m_MacroList)
            {
                macro.Save();
            }

            if (m_AbsoluteTargetList.Count > 0)
            {
                using (StreamWriter writer =
                    new StreamWriter($"{Config.GetUserDirectory("Macros")}\\AbsoluteTargets.lst"))
                {
                    foreach (AbsoluteTarget at in m_AbsoluteTargetList)
                    {
                        try
                        {
                            writer.WriteLine(at.Serialize());
                        }
                        catch
                        {
                        }
                    }
                }
            }
            else
            {
                // Empty list, just make a new empty file
                File.Create($"{Config.GetUserDirectory("Macros")}\\AbsoluteTargets.lst");
            }
        }

        public static void LoadAbsoluteTargets(ListBox list)
        {
            if (!File.Exists($"{Config.GetUserDirectory("Macros")}\\AbsoluteTargets.lst"))
            {
                File.Create($"{Config.GetUserDirectory("Macros")}\\AbsoluteTargets.lst");
            }

            string[] targets = { };

            if (new FileInfo($"{Config.GetUserDirectory("Macros")}\\AbsoluteTargets.lst").Length > 0)
            {
                targets = File.ReadAllLines($"{Config.GetUserDirectory("Macros")}\\AbsoluteTargets.lst");
            }

            //testvar|test|0|0|824181|1371|1921|0|400

            m_AbsoluteTargetList.Clear();

            foreach (string target in targets)
            {
                AbsoluteTarget t = new AbsoluteTarget(target.Split('|'));

                if (t.TargetVariableProfile.ToLower().Equals(Config.CurrentProfile.Name.ToLower()))
                {
                    m_AbsoluteTargetList.Add(t);

                    list.Items.Add(t.ToString());
                }
            }
        }

        public static List<Macro> List { get{ return m_MacroList; } }
        public static List<AbsoluteTarget> AbsoluteTargetList { get { return m_AbsoluteTargetList; } }
        public static bool Recording{ get{ return m_Current != null && m_Current.Recording; } }
		public static bool Playing{ get{ return m_Current != null && m_Current.Playing && m_Timer != null && m_Timer.Running; } }
        public static bool StepThrough { get { return m_Current != null && m_Current.StepThrough && m_Current.Playing; } }
        public static Macro Current{ get{ return m_Current; } }
		public static bool AcceptActions{ get { return Recording || ( Playing && m_Current.Waiting ); } }
		//public static bool IsWaiting{ get{ return Playing && m_Current != null && m_Current.Waiting; } }

		public static void Add( Macro m )
		{
			HotKey.Add( HKCategory.Macros, HKSubCat.None, Language.Format( LocString.PlayA1, m ), new HotKeyCallbackState( HotKeyPlay ), m );
		    m_MacroList.Add( m );
		}

		public static void Remove( Macro m )
		{
			HotKey.Remove( Language.Format( LocString.PlayA1, m ) );
		    m_MacroList.Remove( m );
		}

		public static void RecordAt( Macro m, int at )
		{
			if ( m_Current != null )
				m_Current.Stop();
			m_Current = m;
			m_Current.RecordAt( at );
		}

		public static void Record( Macro m )
		{
			if ( m_Current != null )
				m_Current.Stop();
			m_Current = m;
			m_Current.Record();
		}

		public static void PlayAt( Macro m, int at )
		{
			if ( m_Current != null )
			{
				if ( m_Current.Playing && m_Current.Loop && !m.Loop )
					m_PrevPlay = m_Current;
				else
					m_PrevPlay = null;

				m_Current.Stop();
			}
			else
			{
				m_PrevPlay = null;
			}

			LiftAction.LastLift = null;
			m_Current = m;
			m_Current.PlayAt( at );

			m_Timer.Macro = m_Current;

		    if (!Config.GetBool("StepThroughMacro"))
		    {
		        m_Timer.Start();
		    }

			if ( Engine.MainWindow.WaitDisplay != null )
				Engine.MainWindow.WaitDisplay.Text = "";
		}

		private static void HotKeyPlay( ref object state )
		{
			HotKeyPlay( state as Macro );
		}

		public static void HotKeyPlay( Macro m )
		{
			if ( m != null )
			{
				Play( m );
				World.Player.SendMessage( LocString.PlayingA1, m );
				Engine.MainWindow.PlayMacro( m );
			}
		}

		public static void Play( Macro m )
		{
			if ( m_Current != null )
			{
				if ( m_Current.Playing && m_Current.Loop && !m.Loop )
					m_PrevPlay = m_Current;
				else
					m_PrevPlay = null;

				m_Current.Stop();
			}
			else
			{
				m_PrevPlay = null;
			}

			LiftAction.LastLift = null;
			m_Current = m;
			m_Current.Play();

			m_Timer.Macro = m_Current;

		    if (!Config.GetBool("StepThroughMacro"))
		    {
		        m_Timer.Start();
            }

			if ( Engine.MainWindow.WaitDisplay != null )
				Engine.MainWindow.WaitDisplay.Text = "";
		}

        public static void PlayNext()
        {
            if (m_Current == null)
                return;

            m_Timer.PerformNextAction();


        }

        private static void HotKeyStop()
		{
			Stop();
		}

		public static void Stop()
		{
			Stop( false );
		}

		public static void Stop( bool restartPrev )
		{
			m_Timer.Stop();
			if ( m_Current != null )
			{
				m_Current.Stop();
				m_Current = null;
			}
			ClientCommunication.PostMacroStop();
			
			if ( Engine.MainWindow.WaitDisplay != null )
				Engine.MainWindow.WaitDisplay.Text = "";

			Engine.MainWindow.OnMacroStop();
			
			//if ( restartPrev )
			//	Play( m_PrevPlay );
			m_PrevPlay = null;
		}

		public static void DisplayTo( TreeView tree )
		{
			tree.BeginUpdate();
			tree.Nodes.Clear();
			Recurse( tree.Nodes, Config.GetUserDirectory( "Macros" ) );
			tree.EndUpdate();
			tree.Refresh();
			tree.Update();
		}

        public static void DisplayAbsoluteTargetsTo(ListBox list)
        {
            list.BeginUpdate();
            list.Items.Clear();
            LoadAbsoluteTargets(list);
            list.EndUpdate();
            list.Refresh();
            list.Update();
        }
        

		private static void Recurse( TreeNodeCollection nodes, string path )
		{
			try
			{
				string[] macros = Directory.GetFiles( path, "*.macro" );
				for (int i=0;i<macros.Length;i++)
				{ 
					Macro m = null;
					for(int j=0;j<m_MacroList.Count;j++)
					{
						Macro check = m_MacroList[j];

						if ( check.Filename == macros[i] )
						{
							m = check;
							break;
						}
					}

					if ( m == null )
						Add( m = new Macro( macros[i] ) );

					if ( nodes != null )
					{
						TreeNode node = new TreeNode( Path.GetFileNameWithoutExtension( m.Filename ) );
						node.Tag = m;
						nodes.Add( node );
					}
				}
			}
			catch
			{
			}

			try
			{
				string[] dirs = Directory.GetDirectories( path );
				for (int i=0;i<dirs.Length;i++)
				{
					if ( dirs[i] != "" && dirs[i] != "." && dirs[i] != ".." )
					{
						if ( nodes != null )
						{
							TreeNode node = new TreeNode( String.Format( "[{0}]", Path.GetFileName( dirs[i] ) ) );
							node.Tag = dirs[i];
							nodes.Add( node );
							Recurse( node.Nodes, dirs[i] );
						}
						else
						{
							Recurse( null, dirs[i] );
						}
					}
				}
			}
			catch
			{
			}
		}

		public static void Select( Macro m, ListBox actionList, Button play, Button rec, CheckBox loop )
		{
			if ( m == null )
				return;

			m.DisplayTo( actionList );

			if ( Recording )
			{
				play.Enabled = false;
				play.Text = "Play";
				rec.Enabled = true;
				rec.Text = "Stop";
			}
			else
			{
				play.Enabled = true;
				if ( m.Playing )
				{
					play.Text = "Stop";
					rec.Enabled = false;
				}
				else
				{
					play.Text = "Play";
					rec.Enabled = true;
				}
				rec.Text = "Record";
				loop.Checked = m.Loop;
			}
		}

		public static bool Action( MacroAction a )
		{
			if ( m_Current != null )
				return m_Current.Action( a );
			else
				return false;
		}

		private class MacroTimer : Timer
		{
			private Macro m_Macro;
			
			public MacroTimer() : base( TimeSpan.FromMilliseconds( 50 ), TimeSpan.FromMilliseconds( 50 ) )
			{
			}

			public Macro Macro { get { return m_Macro; } set { m_Macro = value; } }

		    public void PerformNextAction()
		    {
                ExecuteNextAction();
            }

			protected override void OnTick()
			{
			    ExecuteNextAction();
			}

		    private void ExecuteNextAction()
		    {
		        try
		        {
		            if (m_Macro == null || World.Player == null)
		            {
		                this.Stop();
		                MacroManager.Stop();
		            }
		            else if (!m_Macro.ExecNext())
		            {
		                this.Stop();
		                MacroManager.Stop(true);
		                World.Player.SendMessage(LocString.MacroFinished, m_Macro);
		            }
		        }
		        catch
		        {
		            this.Stop();
		            MacroManager.Stop();
		        }
            }
		}
	}
}

