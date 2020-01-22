using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using UOSteam;

namespace Assistant.Scripts
{
    public static class ScriptManager
    {
        public static bool Recording { get; set; }

        public static string ScriptPath
        {
            get { return $"{Config.GetInstallDirectory()}\\Scripts"; }
        }

        private static FastColoredTextBox _scriptEditor { get; set; }
        private class ScriptTimer : Timer
        {
            // Only run scripts once every 25ms to avoid spamming.
            public ScriptTimer() : base(TimeSpan.FromMilliseconds(25), TimeSpan.FromMilliseconds(25))
            {
            }

            protected override void OnTick()
            {                
                Interpreter.ExecuteScripts();

                /*if (Interpreter.ScriptCount > 0)
                {
                    int highlightLine = _scriptEditor.LinesCount - (Interpreter.ScriptCount - 1);

                    if (highlightLine < _scriptEditor.LinesCount - 1)
                        SetHighlightLine(highlightLine, Color.Yellow);
                }*/
            }
        }

        private static ScriptTimer _timer { get; }

        static ScriptManager()
        {
            _timer = new ScriptTimer();
        }

        public static void SetControls(FastColoredTextBox scriptEditor)
        {
            _scriptEditor = scriptEditor;
        }

        public static void OnLogin()
        {
            Commands.Register();
            Aliases.Register();
            Expressions.Register();

            _timer.Start();
        }

        public static void OnLogout()
        {
            _timer.Stop();
        }

        public static void StartEngine()
        {
            _timer.Start();
        }

        public static void StopEngine()
        {
            _timer.Stop();
        }

        public static string[] GetScripts()
        {
            return Directory.GetFiles(ScriptPath, "*.razor");
        }

        public static bool AddToScript(string command)
        {
            if (Recording)
            {
                _scriptEditor?.AppendText(command + Environment.NewLine);

                return true;
            }

            return false;
        }

        public static void Error(string message, string scriptname = "")
        {
            World.Player?.SendMessage(MsgLevel.Error, $"Script '{scriptname}' error => {message}");
        }

        private delegate void SetHighlightLineDelegate(int iline, Color color);

        private static void SetHighlightLine(int iline, Color background)
        {
            for (int i = 0; i < _scriptEditor.LinesCount; i++)
            { 
                _scriptEditor[i].BackgroundBrush = _scriptEditor.BackBrush;
            }

            _scriptEditor[iline].BackgroundBrush = new SolidBrush(background);
            _scriptEditor.Invalidate();
        }

        /*private static Timer _waitTimer = new ScriptTimer();

        public static void StartWait(int ms)
        {
            if (_waitTimer.Running)
            {
                _waitTimer.Stop();
            }

            _waitTimer.Interval = TimeSpan.FromMilliseconds(ms);
            _waitTimer.Start();
        }

        public static bool IsWaiting()
        {
            return _waitTimer.Running;
        }*/

        private static TimeSpan _pauseDuration;
        private static DateTime _startPause = DateTime.MaxValue;
        
        public static bool PauseComplete(int ms = 30000)
        {
            if (_startPause == DateTime.MaxValue) // no timer set
            {
                _startPause = DateTime.UtcNow;
                _pauseDuration = TimeSpan.FromMilliseconds(ms);

                return false; // we want to start pausing
            }
            
            if (_startPause + _pauseDuration < DateTime.UtcNow) // timer is set, has it elapsed?
            {
                _startPause = DateTime.MaxValue;
                _pauseDuration = TimeSpan.FromMilliseconds(ms);
                return true; //pause limit succeeded
            }

            return false; // keep on pausing
        }
    }
}
