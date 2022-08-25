#region license
// Razor: An Ultima Online Assistant
// Copyright (c) 2022 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion

using Assistant.Scripts.Engine;

namespace Assistant.Scripts
{
    public static class SpeechCommands
    {
        public static void Register()
        {
            // Messages
            Interpreter.RegisterCommandHandler("say", Say);
            Interpreter.RegisterCommandHandler("msg", Say);
            Interpreter.RegisterCommandHandler("yell", Yell);
            Interpreter.RegisterCommandHandler("whisper", Whisper);
            Interpreter.RegisterCommandHandler("emote", Emote);
            Interpreter.RegisterCommandHandler("guild", Guild);
            Interpreter.RegisterCommandHandler("alliance", Alliance);
        }

        private static bool Say(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length == 0)
            {
                throw new RunTimeError("Usage: say ('text') [color]");
            }

            if (vars.Length == 1)
                World.Player.Say(Config.GetInt("SysColor"), vars[0].AsString());
            else
                World.Player.Say(Utility.ToInt32(vars[1].AsString(), 0), vars[0].AsString());

            return true;
        }

        private static bool Whisper(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length == 0)
            {
                throw new RunTimeError("Usage: whisper ('text') [color]");
            }

            MessageType type = MessageType.Whisper & ~MessageType.Encoded;

            if (vars.Length == 1)
                World.Player.Whisper(vars[0].AsString(), World.Player.SpeechHue);
            else
                World.Player.Whisper(vars[0].AsString(), Utility.ToInt32(vars[1].AsString(), 0));

            return true;
        }

        private static bool Yell(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length == 0)
            {
                throw new RunTimeError("Usage: yell ('text') [color]");
            }

            if (vars.Length == 1)
                World.Player.Yell(vars[0].AsString(), World.Player.SpeechHue);
            else
                World.Player.Yell(vars[0].AsString(), Utility.ToInt32(vars[1].AsString(), 0));

            return true;
        }

        private static bool Emote(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length == 0)
            {
                throw new RunTimeError("Usage: emote ('text') [color]");
            }

            if (vars.Length == 1)
                World.Player.Emote(vars[0].AsString(), World.Player.SpeechHue);
            else
                World.Player.Emote(vars[0].AsString(), Utility.ToInt32(vars[1].AsString(), 0));

            return true;
        }

        private static bool Guild(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length == 0)
            {
                throw new RunTimeError("Usage: guild ('text')");
            }

            if (vars.Length == 1)
                World.Player.Guild(vars[0].AsString(), World.Player.SpeechHue);
            else
                World.Player.Guild(vars[0].AsString(), Utility.ToInt32(vars[1].AsString(), 0));

            return true;
        }

        private static bool Alliance(string command, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length == 0)
            {
                throw new RunTimeError("Usage: alliance ('text')");
            }

            if (vars.Length == 1)
                World.Player.Alliance(vars[0].AsString(), World.Player.SpeechHue);
            else
                World.Player.Alliance(vars[0].AsString(), Utility.ToInt32(vars[1].AsString(), 0));

            return true;
        }
    }
}
