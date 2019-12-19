using System;
using System.Collections;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Text;
using Assistant;
using Assistant.UI;

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

        public Macro(string path)
        {
            m_Actions = new ArrayList();
            m_Path = path;
            m_Loaded = false;
            m_IfStatus = new Stack();
        }

        public string GetName()
        {
            return Path.GetFileNameWithoutExtension(m_Path);
        }

        public string Filename
        {
            get { return m_Path; }
            set { m_Path = value; }
        }

        public ArrayList Actions
        {
            get { return m_Actions; }
        }

        public bool Recording
        {
            get { return m_Recording; }
        }

        public bool Playing
        {
            get { return m_Playing; }
        }

        public bool StepThrough { get; set; }

        public bool Waiting
        {
            get { return m_Wait != null; }
        }

        public int CurrentAction
        {
            get { return m_CurrentAction; }
        }

        public bool Loop
        {
            get { return m_Loop && Client.Instance.AllowBit(FeatureBit.LoopingMacros); }
            set { m_Loop = value; }
        }

        public void DisplayTo(ListBox list)
        {
            m_ListBox = list;

            m_ListBox.SafeAction(s => s.Items.Clear());

            if (!m_Loaded)
                Load();

            m_ListBox.SafeAction(s =>
            {
                s.BeginUpdate();
                if (m_Actions.Count > 0)
                    s.Items.AddRange((object[]) m_Actions.ToArray(typeof(object)));
                if (m_Playing && m_CurrentAction >= 0 && m_CurrentAction < m_Actions.Count)
                    s.SelectedIndex = m_CurrentAction;
                else
                    s.SelectedIndex = -1;
                s.EndUpdate();
            });
        }

        public override string ToString()
        {
            //return Path.GetFileNameWithoutExtension( m_Path );
            StringBuilder sb = new StringBuilder(Path.GetFullPath(m_Path));
            sb.Remove(sb.Length - 6, 6);
            sb.Remove(0, Config.GetUserDirectory("Macros").Length + 1);
            return sb.ToString();
        }

        public void Insert(int idx, MacroAction a)
        {
            a.Parent = this;
            if (idx < 0 || idx > m_Actions.Count)
                idx = m_Actions.Count;
            m_Actions.Insert(idx, a);
        }

        public void Record()
        {
            m_Actions.Clear();
            if (m_ListBox != null)
                m_ListBox.SafeAction(s => s.Items.Clear());
            RecordAt(0);
        }

        public void RecordAt(int at)
        {
            Stop();
            m_Recording = true;
            m_Loaded = true;
            m_CurrentAction = at;
            if (m_CurrentAction > m_Actions.Count)
                m_CurrentAction = m_Actions.Count;
        }

        public void Play()
        {
            Stop();
            if (!m_Loaded)
                Load();
            else
                Save();

            if (m_Actions.Count > 0)
            {
                m_IfStatus.Clear();
                m_Playing = true;
                m_CurrentAction = -1;
                if (m_ListBox != null)
                    m_ListBox.SafeAction(s => s.SelectedIndex = -1);
            }
        }

        public void PlayAt(int at)
        {
            Stop();
            if (!m_Loaded)
                Load();
            else
                Save();

            if (m_Actions.Count > 0)
            {
                m_IfStatus.Clear();
                m_Playing = true;
                m_CurrentAction = at - 1;
                if (m_CurrentAction >= 0)
                    m_CurrentAction--;
            }
        }

        public void Reset()
        {
            if (m_Playing && World.Player != null && DragDropManager.Holding != null &&
                DragDropManager.Holding == LiftAction.LastLift)
                Client.Instance.SendToServer(new DropRequest(DragDropManager.Holding, World.Player.Serial));

            m_Wait = null;

            m_IfStatus.Clear();

            foreach (MacroAction a in m_Actions)
            {
                if (a is ForAction)
                    ((ForAction) a).Count = 0;
            }
        }

        public void Stop()
        {
            if (m_Recording)
                Save();

            m_Recording = m_Playing = false;
            Reset();
        }

        // returns true if the were waiting for this action
        public bool Action(MacroAction action)
        {
            if (m_Recording)
            {
                action.Parent = this;
                m_Actions.Insert(m_CurrentAction, action);
                if (m_ListBox != null)
                    m_ListBox.SafeAction(s => s.Items.Insert(m_CurrentAction, action));
                m_CurrentAction++;

                return false;
            }
            else if (m_Playing && m_Wait != null && m_Wait.CheckMatch(action))
            {
                m_Wait = null;
                ExecNext();
                return true;
            }

            return false;
        }

        public void Save()
        {
            if (m_Actions.Count == 0)
                return;

            using (StreamWriter writer = new StreamWriter(m_Path))
            {
                if (m_Loop)
                    writer.WriteLine("!Loop");

                for (int i = 0; i < m_Actions.Count; i++)
                {
                    MacroAction a = (MacroAction) m_Actions[i];
                    try
                    {
                        writer.WriteLine(a.Serialize());
                    }
                    catch
                    {
                    }
                }
            }
        }

        private static Type[] ctorArgs = new Type[1] {typeof(string[])};

        public void Load()
        {
            m_Actions.Clear();
            m_Loaded = false;

            if (!File.Exists(m_Path))
                return;

            using (StreamReader reader = new StreamReader(m_Path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length <= 2)
                        continue;

                    if (line == "!Loop")
                    {
                        m_Loop = true;
                        continue;
                    }

                    if (line[0] == '#')
                    {
                        m_Actions.Add(new MacroComment(line.Substring(1)));
                        continue;
                    }
                    else if (line[0] == '/' && line[1] == '/')
                    {
                        MacroAction a = new MacroComment(line.Substring(2));
                        a.Parent = this;
                        m_Actions.Add(a);
                        continue;
                    }

                    string[] args = line.Split('|');
                    object[] invokeArgs = new object[1] {args};

                    Type at = null;
                    try
                    {
                        at = Type.GetType(args[0], false);
                    }
                    catch
                    {
                    }

                    if (at == null)
                        continue;

                    if (args.Length > 1)
                    {
                        try
                        {
                            ConstructorInfo ctor = at.GetConstructor(ctorArgs);
                            if (ctor == null)
                                continue;

                            MacroAction a = (MacroAction) ctor.Invoke(invokeArgs);
                            a.Parent = this;
                            m_Actions.Add(a);
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        try
                        {
                            ConstructorInfo ctor = at.GetConstructor(Type.EmptyTypes);
                            if (ctor == null)
                                continue;

                            MacroAction a = (MacroAction) ctor.Invoke(null);
                            a.Parent = this;
                            m_Actions.Add(a);
                        }
                        catch
                        {
                        }
                    }
                }
            }

            m_Loaded = true;
        }

        public void Convert(MacroAction old, MacroAction newAct)
        {
            for (int i = 0; i < m_Actions.Count; i++)
            {
                if (m_Actions[i] == old)
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
            if (m_ListBox != null)
            {
                int sel = m_ListBox.SelectedIndex;
                DisplayTo(m_ListBox);
                try
                {
                    m_ListBox.SafeAction(s => s.SelectedIndex = sel);
                }
                catch
                {
                }
            }
        }

        private bool NextIsInstantWait()
        {
            int nextAct = m_CurrentAction + 1;

            if (nextAct >= 0 && nextAct < m_Actions.Count)
                return m_Actions[nextAct] is WaitForTargetAction || m_Actions[nextAct] is WaitForGumpAction ||
                       m_Actions[nextAct] is
                           WaitForMenuAction; //|| m_Actions[nextAct] is ElseAction || m_Actions[nextAct] is EndIfAction;
            else
                return false;
        }

        private static MacroWaitAction PauseB4Loop = new PauseAction(TimeSpan.FromSeconds(0.1));

        //return true to continue the macro, false to stop (macro's over)
        public bool ExecNext()
        {
            try
            {
                if (!m_Playing)
                    return false;

                if (m_Wait != null)
                {
                    TimeSpan waitLen = DateTime.UtcNow - m_Wait.StartTime;
                    if (!(m_Wait is PauseAction) && waitLen >= m_Wait.Timeout)
                    {
                        if (Loop)
                        {
                            if (Engine.MainWindow.WaitDisplay != null)
                                Engine.MainWindow.WaitDisplay.SafeAction(s => s.Text = string.Empty);
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
                        if (!m_Wait.PerformWait())
                        {
                            m_Wait = null; // done waiting
                            if (Engine.MainWindow.WaitDisplay != null)
                                Engine.MainWindow.WaitDisplay.SafeAction(s => s.Text = string.Empty);
                        }
                        else
                        {
                            if (waitLen >= TimeSpan.FromSeconds(4.0) && Engine.MainWindow.WaitDisplay != null)
                            {
                                StringBuilder sb = new StringBuilder(Language.GetString(LocString.WaitingTimeout));

                                sb.AppendLine("\n");

                                int s = (int) (m_Wait.Timeout - waitLen).TotalSeconds;
                                int m = 0;

                                if (s > 60)
                                {
                                    m = s / 60;
                                    s %= 60;
                                    if (m > 60)
                                    {
                                        sb.AppendFormat("{0}:", m / 60);
                                        m %= 60;
                                    }
                                }

                                sb.AppendFormat("{0:00}:{1:00}", m, s);

                                Engine.MainWindow.WaitDisplay.SafeAction(w => w.Text = sb.ToString());
                            }

                            return true; // keep waiting
                        }
                    }
                }

                m_CurrentAction++;
                //MacroManager.ActionUpdate( this, m_CurrentAction );
                if (m_ListBox != null)
                {
                    if (m_CurrentAction < m_ListBox.Items.Count)
                        m_ListBox.SafeAction(s => s.SelectedIndex = m_CurrentAction);
                    else
                        m_ListBox.SafeAction(s => s.SelectedIndex = -1);
                }

                if (m_CurrentAction >= 0 && m_CurrentAction < m_Actions.Count)
                {
                    MacroAction action = (MacroAction) m_Actions[m_CurrentAction];

                    if (action is IfAction)
                    {
                        bool val = ((IfAction) action).Evaluate();
                        m_IfStatus.Push(val);

                        if (!val)
                        {
                            // false so skip to an else or an endif
                            int ifcount = 0;
                            while (m_CurrentAction + 1 < m_Actions.Count)
                            {
                                if (m_Actions[m_CurrentAction + 1] is IfAction)
                                {
                                    ifcount++;
                                }
                                else if (m_Actions[m_CurrentAction + 1] is ElseAction && ifcount <= 0)
                                {
                                    break;
                                }
                                else if (m_Actions[m_CurrentAction + 1] is EndIfAction)
                                {
                                    if (ifcount <= 0)
                                        break;
                                    else
                                        ifcount--;
                                }

                                m_CurrentAction++;
                            }
                        }
                    }
                    else if (action is ElseAction && m_IfStatus.Count > 0)
                    {
                        bool val = (bool) m_IfStatus.Peek();
                        if (val)
                        {
                            // the if was true, so skip to an endif
                            int ifcount = 0;
                            while (m_CurrentAction + 1 < m_Actions.Count)
                            {
                                if (m_Actions[m_CurrentAction + 1] is IfAction)
                                {
                                    ifcount++;
                                }
                                else if (m_Actions[m_CurrentAction + 1] is EndIfAction)
                                {
                                    if (ifcount <= 0)
                                        break;
                                    else
                                        ifcount--;
                                }

                                m_CurrentAction++;
                            }
                        }
                    }
                    else if (action is EndIfAction && m_IfStatus.Count > 0)
                    {
                        m_IfStatus.Pop();
                    }
                    else if (action is ForAction)
                    {
                        ForAction fa = (ForAction) action;
                        fa.Count++;

                        if (fa.Count > fa.Max)
                        {
                            fa.Count = 0;

                            int forcount = 0;
                            m_CurrentAction++;
                            while (m_CurrentAction < m_Actions.Count)
                            {
                                if (m_Actions[m_CurrentAction] is ForAction)
                                {
                                    forcount++;
                                }
                                else if (m_Actions[m_CurrentAction] is EndForAction)
                                {
                                    if (forcount <= 0)
                                        break;
                                    else
                                        forcount--;
                                }

                                m_CurrentAction++;
                            }

                            if (m_CurrentAction < m_Actions.Count)
                                action = (MacroAction) m_Actions[m_CurrentAction];
                        }
                    }
                    else if (action is EndForAction && Client.Instance.AllowBit(FeatureBit.LoopingMacros))
                    {
                        int ca = m_CurrentAction - 1;
                        int forcount = 0;

                        while (ca >= 0)
                        {
                            if (m_Actions[ca] is EndForAction)
                            {
                                forcount--;
                            }
                            else if (m_Actions[ca] is ForAction)
                            {
                                if (forcount >= 0)
                                    break;
                                else
                                    forcount++;
                            }

                            ca--;
                        }

                        if (ca >= 0 && m_Actions[ca] is ForAction)
                            m_CurrentAction = ca - 1;
                    }
                    else if (action is WhileAction && Client.Instance.AllowBit(FeatureBit.LoopingMacros))
                    {
                        bool val = ((WhileAction)action).Evaluate();

                        if (!val)
                        {
                            // false so skip to the endwhile
                            int whilecount = 0;
                            while (m_CurrentAction + 1 < m_Actions.Count)
                            {
                                if (m_Actions[m_CurrentAction + 1] is WhileAction)
                                {
                                    whilecount++;
                                }
                                else if (m_Actions[m_CurrentAction + 1] is EndWhileAction)
                                {
                                    if (whilecount <= 0)
                                    {
                                        // Skip over the end while
                                        m_CurrentAction++;
                                        break;
                                    }

                                    whilecount--;
                                }

                                m_CurrentAction++;
                            }
                        }
                    }
                    else if (action is EndWhileAction && Client.Instance.AllowBit(FeatureBit.LoopingMacros))
                    {
                        int ca = m_CurrentAction - 1;
                        int whilecount = 0;

                        while (ca >= 0)
                        {
                            if (m_Actions[ca] is EndWhileAction)
                            {
                                whilecount--;
                            }
                            else if (m_Actions[ca] is WhileAction)
                            {
                                if (whilecount >= 0)
                                    break;
                                else
                                    whilecount++;
                            }

                            ca--;
                        }

                        if (ca >= 0 && m_Actions[ca] is WhileAction)
                            m_CurrentAction = ca - 1;
                    }
                    else if (action is DoWhileAction && Client.Instance.AllowBit(FeatureBit.LoopingMacros))
                    {
                        bool val = ((DoWhileAction)action).Evaluate();

                        if (val)
                        {
                            int ca = m_CurrentAction - 1;
                            int dowhilecount = 0;

                            while (ca >= 0)
                            {
                                if (m_Actions[ca] is DoWhileAction)
                                {
                                    dowhilecount--;
                                }
                                else if (m_Actions[ca] is StartDoWhileAction)
                                {
                                    if (dowhilecount >= 0)
                                        break;
                                    else
                                        dowhilecount++;
                                }

                                ca--;
                            }

                            if (ca >= 0 && m_Actions[ca] is StartDoWhileAction)
                                m_CurrentAction = ca - 1;
                        }
                    }

                    bool isWait = action is MacroWaitAction;
                    if (!action.Perform() && isWait)
                    {
                        m_Wait = (MacroWaitAction) action;
                        m_Wait.StartTime = DateTime.UtcNow;
                    }
                    else if (NextIsInstantWait() && !isWait)
                    {
                        return ExecNext();
                    }
                }
                else
                {
                    if (Engine.MainWindow.WaitDisplay != null)
                        Engine.MainWindow.WaitDisplay.SafeAction(s => s.Text = string.Empty);

                    if (Loop)
                    {
                        m_CurrentAction = -1;

                        Reset();

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
            catch (Exception e)
            {
                new MessageDialog("Macro Exception", true, e.ToString()).Show();
                return false;
            }

            return true;
        }
    }
}