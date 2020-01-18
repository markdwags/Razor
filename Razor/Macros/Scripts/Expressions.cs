using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UOSteam;

namespace Assistant.Macros.Scripts
{
    public class Expressions
    {
        private static int DummyExpression(ref ASTNode node, bool quiet)
        {
            Console.WriteLine("Executing expression {0} {1}", node.Type, node.Lexeme);

            while (node != null)
            {
                switch (node.Type)
                {
                    case ASTNodeType.AND:
                    case ASTNodeType.OR:
                    case ASTNodeType.EQUAL:
                    case ASTNodeType.NOT_EQUAL:
                    case ASTNodeType.LESS_THAN:
                    case ASTNodeType.LESS_THAN_OR_EQUAL:
                    case ASTNodeType.GREATER_THAN:
                    case ASTNodeType.GREATER_THAN_OR_EQUAL:
                        return 0;
                }

                node = node.Next();
            }

            return 0;
        }

        public static void Register()
        {
            // Expressions
            Interpreter.RegisterExpressionHandler("findalias", FindAlias);
            Interpreter.RegisterExpressionHandler("contents", DummyExpression);
            Interpreter.RegisterExpressionHandler("inregion", DummyExpression);
            Interpreter.RegisterExpressionHandler("skill", SkillExpression);
            Interpreter.RegisterExpressionHandler("findobject", DummyExpression);
            Interpreter.RegisterExpressionHandler("distance", DummyExpression);
            Interpreter.RegisterExpressionHandler("inrange", DummyExpression);
            Interpreter.RegisterExpressionHandler("buffexists", DummyExpression);
            Interpreter.RegisterExpressionHandler("property", DummyExpression);
            Interpreter.RegisterExpressionHandler("findtype", DummyExpression);
            Interpreter.RegisterExpressionHandler("findlayer", DummyExpression);
            Interpreter.RegisterExpressionHandler("skillstate", DummyExpression);
            Interpreter.RegisterExpressionHandler("counttype", DummyExpression);
            Interpreter.RegisterExpressionHandler("counttypeground", DummyExpression);
            Interpreter.RegisterExpressionHandler("findwand", DummyExpression);
            Interpreter.RegisterExpressionHandler("inparty", DummyExpression);
            Interpreter.RegisterExpressionHandler("infriendslist", DummyExpression);
            Interpreter.RegisterExpressionHandler("war", DummyExpression);
            Interpreter.RegisterExpressionHandler("ingump", DummyExpression);
            Interpreter.RegisterExpressionHandler("gumpexists", DummyExpression);
            Interpreter.RegisterExpressionHandler("injournal", DummyExpression);
            Interpreter.RegisterExpressionHandler("listexists", DummyExpression);
            Interpreter.RegisterExpressionHandler("list", DummyExpression);
            Interpreter.RegisterExpressionHandler("inlist", DummyExpression);

            // Player Attributes
            Interpreter.RegisterExpressionHandler("mana", Mana);
            Interpreter.RegisterExpressionHandler("x", X);
            Interpreter.RegisterExpressionHandler("y", Y);
            Interpreter.RegisterExpressionHandler("z", Z);
        }
        private static int FindAlias(ref ASTNode node, bool quiet)
        {
            node.Next();

            ASTNode alias = node.Next();

            if (alias == null)
                throw new ArgumentException("Usage: findalias (string)");

            return Interpreter.GetAlias(ref alias);
        }

        private static int Mana(ref ASTNode node, bool quiet)
        {
            node.Next();

            if (World.Player == null)
                return 0;

            return World.Player.Mana;
        }
        private static int X(ref ASTNode node, bool quiet)
        {
            node.Next();

            if (World.Player == null)
                return 0;

            return World.Player.Position.X;
        }
        private static int Y(ref ASTNode node, bool quiet)
        {
            node.Next();

            if (World.Player == null)
                return 0;

            return World.Player.Position.Y;
        }
        private static int Z(ref ASTNode node, bool quiet)
        {
            node.Next();

            if (World.Player == null)
                return 0;

            return World.Player.Position.Z;
        }

        // WIP
        private static int SkillExpression(ref ASTNode node, bool quiet)
        {
            node.Next();

            ASTNode skillName = node.Next();

            if (skillName == null)
                throw new ArgumentException("Usage: skill (name)");

            if (World.Player == null)
                return 0;

            return 0;
        }
    }
}
