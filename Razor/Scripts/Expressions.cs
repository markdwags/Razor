using System;
using Ultima;
using UOSteam;

namespace Assistant.Scripts
{
    public static class Expressions
    {
        private static int DummyExpression(string expression, Argument[] args, bool quiet)
        {
            Console.WriteLine("Executing expression {0} {1}", expression, args);

            return 0;
        }

        public static void Register()
        {
            // Expressions
            Interpreter.RegisterExpressionHandler("findalias", FindAlias);

            Interpreter.RegisterExpressionHandler("stam", Stam);
            Interpreter.RegisterExpressionHandler("maxstam", MaxStam);
            Interpreter.RegisterExpressionHandler("hp", Hp);
            Interpreter.RegisterExpressionHandler("hits", Hp);
            Interpreter.RegisterExpressionHandler("maxhp", MaxHp);
            Interpreter.RegisterExpressionHandler("maxhits", MaxHp);
            Interpreter.RegisterExpressionHandler("mana", Mana);
            Interpreter.RegisterExpressionHandler("maxmana", MaxMana);
            Interpreter.RegisterExpressionHandler("poisoned", Poisoned);

            Interpreter.RegisterExpressionHandler("mounted", Mounted);
            Interpreter.RegisterExpressionHandler("rhandempty", RHandEmpty);
            Interpreter.RegisterExpressionHandler("lhandempty", LHandEmpty);

            Interpreter.RegisterExpressionHandler("dead", Dead);

            Interpreter.RegisterExpressionHandler("str", Str);
            Interpreter.RegisterExpressionHandler("int", Int);
            Interpreter.RegisterExpressionHandler("dex", Dex);

            Interpreter.RegisterExpressionHandler("skill", SkillExpression);
            Interpreter.RegisterExpressionHandler("count", CountExpression);
            Interpreter.RegisterExpressionHandler("counter", CountExpression);

            Interpreter.RegisterExpressionHandler("insysmsg", InSysMessage);
            Interpreter.RegisterExpressionHandler("insysmessage", InSysMessage);
        }

        private static double FindAlias(string expression, Argument[] args, bool quiet)
        {
            if (args.Length < 1)
                ScriptManager.Error("Usage: findalias (string)");

            uint serial = Interpreter.GetAlias(args[0].AsString());

            if (serial == uint.MaxValue)
                return 0;

            return 1;
        }

        private static double Mounted(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && World.Player.GetItemOnLayer(Layer.Mount) != null
                ? 1
                : 0;
        }

        private static double RHandEmpty(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && World.Player.GetItemOnLayer(Layer.RightHand) != null
                ? 1
                : 0;
        }

        private static double LHandEmpty(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && World.Player.GetItemOnLayer(Layer.LeftHand) != null
                ? 1
                : 0;
        }
        private static double Dead(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && World.Player.IsGhost
                ? 1
                : 0;
        }

        private static double InSysMessage(string expression, Argument[] args, bool quiet)
        {
            if (args.Length == 0)
            {
                ScriptManager.Error("Usage: insysmsg ('text')");
                return 0;
            }

            string text = args[0].AsString();

            for (int i = PacketHandlers.SysMessages.Count - 1; i >= 0; i--)
            {
                string sys = PacketHandlers.SysMessages[i];

                if (sys.IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    PacketHandlers.SysMessages.RemoveRange(0, i + 1);
                    return 1;
                }
            }

            return 0;
        }

        private static double Mana(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Mana;
        }

        private static double MaxMana(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.ManaMax;
        }

        private static double Poisoned(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && Client.Instance.AllowBit(FeatureBit.BlockHealPoisoned) &&
                   World.Player.Poisoned
                ? 1
                : 0;
        }

        private static double Hp(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Hits;
        }

        private static double MaxHp(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.HitsMax;
        }

        private static double Stam(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Stam;
        }

        private static double MaxStam(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.StamMax;
        }

        private static double Str(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Str;
        }

        private static double Dex(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Dex;
        }

        private static double Int(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Int;
        }

        private static double SkillExpression(string expression, Argument[] args, bool quiet)
        {
            if (args.Length < 1)
                throw new ArgumentException("Usage: skill ('name of skill')");

            if (World.Player == null)
                return 0;

            foreach (SkillInfo skill in Skills.SkillEntries)
            {
                if (skill.Name.IndexOf(args[0].AsString(), StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    return World.Player.Skills[skill.Index].Value;
                }
            }

            return 0;
        }
        
        private static double CountExpression(string expression, Argument[] args, bool quiet)
        {
            if (args.Length < 1)
                throw new ArgumentException("Usage: count ('name of counter item')");

            if (World.Player == null)
                return 0;
            
            foreach (Counter c in Counter.List)
            {
                if (c.Name.Equals(args[0].AsString(), StringComparison.OrdinalIgnoreCase))
                {
                    return c.Enabled ? c.Amount : 0;
                }
            }

            return 0;
        }
    }
}
