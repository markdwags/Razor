using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UOSteam;

namespace Assistant.Macros.Scripts
{
    public static class ScriptManager
    {
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

        private static ScriptTimer _timer;

        static ScriptManager()
        {
            _timer = new ScriptTimer();
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
    }
}
