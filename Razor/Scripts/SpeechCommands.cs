using Assistant.Scripts.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public static bool Say(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: say ('text') [color]");
            }

            if (args.Length == 1)
                World.Player.Say(Config.GetInt("SysColor"), args[0].AsString());
            else
                World.Player.Say(Utility.ToInt32(args[1].AsString(), 0), args[0].AsString());

            return true;
        }

        public static bool Whisper(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: whisper ('text') [color]");
            }

            MessageType type = MessageType.Whisper & ~MessageType.Encoded;

            if (args.Length == 1)
                World.Player.Whisper(args[0].AsString(), World.Player.SpeechHue);
            else
                World.Player.Whisper(args[0].AsString(), Utility.ToInt32(args[1].AsString(), 0));

            return true;
        }

        public static bool Yell(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: yell ('text') [color]");
            }

            if (args.Length == 1)
                World.Player.Yell(args[0].AsString(), World.Player.SpeechHue);
            else
                World.Player.Yell(args[0].AsString(), Utility.ToInt32(args[1].AsString(), 0));

            return true;
        }

        public static bool Emote(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: emote ('text') [color]");
            }

            if (args.Length == 1)
                World.Player.Emote(args[0].AsString(), World.Player.SpeechHue);
            else
                World.Player.Emote(args[0].AsString(), Utility.ToInt32(args[1].AsString(), 0));

            return true;
        }

        public static bool Guild(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: guild ('text')");
            }

            if (args.Length == 1)
                World.Player.Guild(args[0].AsString(), World.Player.SpeechHue);
            else
                World.Player.Guild(args[0].AsString(), Utility.ToInt32(args[1].AsString(), 0));

            return true;
        }

        public static bool Alliance(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: alliance ('text')");
            }

            if (args.Length == 1)
                World.Player.Alliance(args[0].AsString(), World.Player.SpeechHue);
            else
                World.Player.Alliance(args[0].AsString(), Utility.ToInt32(args[1].AsString(), 0));

            return true;
        }
    }
}
