using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UOSteam;

namespace Assistant.Macros.Scripts
{
    public static class ScriptManager
    {
        public static bool Recording { get; set; }

        private static RichTextBox _scriptEditor { get; set; }
        private static TextBox _scriptError { get; set; }

        private class ScriptTimer : Timer
        {
            // Only run scripts once every 25ms to avoid spamming.
            public ScriptTimer() : base(TimeSpan.FromMilliseconds(25), TimeSpan.FromMilliseconds(25))
            {
            }

            protected override void OnTick()
            {                
                Interpreter.ExecuteScripts();
              
            }
        }

        private static ScriptTimer _timer { get; }

        static ScriptManager()
        {
            _timer = new ScriptTimer();
        }

        public static void SetControls(RichTextBox scriptEditor, TextBox scriptError)
        {
            _scriptEditor = scriptEditor;
            _scriptError = scriptError;
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
            return Directory.GetFiles($"{Config.GetInstallDirectory()}\\Scripts", "*.razor");
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
    }
}
