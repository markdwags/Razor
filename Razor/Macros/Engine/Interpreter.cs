using System;
using System.Collections.Generic;
using System.Globalization;

namespace UOSteam
{
    public class RunTimeError : Exception
    {
        public ASTNode Node;

        public RunTimeError(ASTNode node, string error) : base(error)
        {
            Node = node;
        }
    }

    internal class Scope
    {
        public Dictionary<string, object> Namespace = new Dictionary<string, object>();

        public readonly ASTNode StartNode;
        public readonly Scope Parent;

        public Scope(Scope parent, ASTNode start)
        {
            Parent = parent;
            StartNode = start;
        }
    }

    public class Argument
    {
        private ASTNode _node;

        public Argument(ASTNode node)
        {
            _node = node;
        }

        // Treat the argument as an integer
        public int AsInt()
        {
            if (_node.Lexeme == null)
                throw new RunTimeError(_node, "Cannot convert argument to int");

            int val;

            if (_node.Lexeme.StartsWith("0x"))
            {
                if (int.TryParse(_node.Lexeme.Substring(2), NumberStyles.HexNumber, Interpreter.Culture, out val))
                    return val;
            }
            else if (int.TryParse(_node.Lexeme, out val))
                return val;

            throw new RunTimeError(_node, "Cannot convert argument to int");
        }

        // Treat the argument as an unsigned integer
        public uint AsUInt()
        {
            if (_node.Lexeme == null)
                throw new RunTimeError(_node, "Cannot convert argument to uint");

            uint val;

            if (_node.Lexeme.StartsWith("0x"))
            {
                if (uint.TryParse(_node.Lexeme.Substring(2), NumberStyles.HexNumber, Interpreter.Culture, out val))
                    return val;
            }
            else if (uint.TryParse(_node.Lexeme, out val))
                return val;

            throw new RunTimeError(_node, "Cannot convert argument to uint");
        }

        // Treat the argument as a serial or an alias. Aliases will
        // be automatically resolved to serial numbers.
        public uint AsSerial()
        {
            if (_node.Lexeme == null)
                throw new RunTimeError(_node, "Cannot convert argument to serial");

            // Resolving aliases takes precedence
            uint serial = Interpreter.GetAlias(_node.Lexeme);

            if (serial != uint.MaxValue)
                return serial;

            return AsUInt();
        }

        // Treat the argument as a string
        public string AsString()
        {
            if (_node.Lexeme == null)
                throw new RunTimeError(_node, "Cannot convert argument to string");

            return _node.Lexeme;
        }
    }

    public class Script
    {
        private ASTNode _statement;

        private Scope _scope;

        private object Lookup(string name)
        {
            var scope = _scope;
            object result = null;

            while (scope != null)
            {
                if (scope.Namespace.TryGetValue(name, out result))
                    return result;
            }

            return result;
        }

        private void PushScope(ASTNode node)
        {
            _scope = new Scope(_scope, node);
        }

        private void PopScope()
        {
            _scope = _scope.Parent;
        }

        private Argument[] ConstructArguments(ref ASTNode node)
        {
            List<Argument> args = new List<Argument>();

            node = node.Next();

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
                        return args.ToArray();
                }

                args.Add(new Argument(node));

                node = node.Next();
            }

            return args.ToArray();
        }

        // For now, the scripts execute directly from the
        // abstract syntax tree. This is relatively simple.
        // A more robust approach would be to "compile" the
        // scripts to a bytecode. That would allow more errors
        // to be caught with better error messages, as well as
        // make the scripts execute more quickly.
        public Script(ASTNode root)
        {
            // Set current to the first statement
            _statement = root.FirstChild();

            // Create a default scope
            _scope = new Scope(null, _statement);
        }

        public bool ExecuteNext()
        {
            if (_statement == null)
                return false;

            if (_statement.Type != ASTNodeType.STATEMENT)
                throw new RunTimeError(_statement, "Invalid script");

            var node = _statement.FirstChild();

            if (node == null)
                throw new RunTimeError(_statement, "Invalid statement");

            int depth = 0;

            switch (node.Type)
            {
                case ASTNodeType.IF:
                    {
                        PushScope(node);

                        var expr = node.FirstChild();
                        var result = EvaluateExpression(ref expr);

                        // Advance to next statement
                        _statement = _statement.Next();

                        // Evaluated true. Jump right into execution.
                        if (result)
                            break;

                        // The expression evaluated false, so keep advancing until
                        // we hit an elseif, else, or, endif statement that matches
                        // and try again.
                        depth = 0;

                        while (_statement != null)
                        {
                            node = _statement.FirstChild();

                            if (node.Type == ASTNodeType.IF)
                            {
                                depth++;
                            }
                            else if (node.Type == ASTNodeType.ELSEIF)
                            {
                                if (depth > 0)
                                {
                                    continue;
                                }

                                expr = node.FirstChild();
                                result = EvaluateExpression(ref expr);

                                // Evaluated true. Jump right into execution
                                if (result)
                                {
                                    _statement = _statement.Next();
                                    break;
                                }
                            }
                            else if (node.Type == ASTNodeType.ELSE)
                            {
                                if (depth > 0)
                                {
                                    continue;
                                }

                                // Jump into the else clause
                                _statement = _statement.Next();
                                break;
                            }
                            else if (node.Type == ASTNodeType.ENDIF)
                            {
                                if (depth > 0)
                                {
                                    depth--;
                                    continue;
                                }

                                break;
                            }

                            _statement = _statement.Next();
                        }

                        if (_statement == null)
                            throw new RunTimeError(node, "If with no matching endif");

                        break;
                    }
                case ASTNodeType.ELSEIF:
                    // If we hit the elseif statement during normal advancing, skip over it. The only way
                    // to execute an elseif clause is to jump directly in from an if statement.
                    depth = 0;

                    while (_statement != null)
                    {
                        node = _statement.FirstChild();

                        if (node.Type == ASTNodeType.IF)
                        {
                            depth++;
                        }
                        else if (node.Type == ASTNodeType.ENDIF)
                        {
                            if (depth == 0)
                                break;

                            depth--;
                        }

                        _statement = _statement.Next();
                    }

                    if (_statement == null)
                        throw new RunTimeError(node, "If with no matching endif");

                    break;
                case ASTNodeType.ENDIF:
                    PopScope();
                    _statement = _statement.Next();
                    break;
                case ASTNodeType.ELSE:
                    // If we hit the else statement during normal advancing, skip over it. The only way
                    // to execute an else clause is to jump directly in from an if statement.
                    depth = 0;

                    while (_statement != null)
                    {
                        node = _statement.FirstChild();

                        if (node.Type == ASTNodeType.IF)
                        {
                            depth++;
                        }
                        else if (node.Type == ASTNodeType.ENDIF)
                        {
                            if (depth == 0)
                                break;

                            depth--;
                        }

                        _statement = _statement.Next();
                    }

                    if (_statement == null)
                        throw new RunTimeError(node, "If with no matching endif");

                    break;
                case ASTNodeType.WHILE:
                    {
                        PushScope(node);

                        var expr = node.FirstChild();
                        var result = EvaluateExpression(ref expr);

                        // Advance to next statement
                        _statement = _statement.Next();

                        // The expression evaluated false, so keep advancing until
                        // we hit an endwhile statement.
                        if (!result)
                        {
                            depth = 0;

                            while (_statement != null)
                            {
                                node = _statement.FirstChild();

                                if (node.Type == ASTNodeType.WHILE)
                                {
                                    depth++;
                                }
                                else if (node.Type == ASTNodeType.ENDWHILE)
                                {
                                    if (depth == 0)
                                    {
                                        PopScope();
                                        // Go one past the endwhile so the loop doesn't repeat
                                        _statement = _statement.Next();
                                        break;
                                    }

                                    depth--;
                                }

                                _statement = _statement.Next();
                            }
                        }
                        break;
                    }
                case ASTNodeType.ENDWHILE:
                    // Walk backward to the while statement
                    _statement = _statement.Prev();

                    depth = 0;

                    while (_statement != null)
                    {
                        node = _statement.FirstChild();

                        if (node.Type == ASTNodeType.ENDWHILE)
                        {
                            depth++;
                        }
                        else if (node.Type == ASTNodeType.WHILE)
                        {
                            if (depth == 0)
                                break;

                            depth--;
                        }

                        _statement = _statement.Prev();
                    }

                    if (_statement == null)
                        throw new RunTimeError(node, "Unexpected endwhile");

                    PopScope();

                    break;
                case ASTNodeType.FOR:
                case ASTNodeType.FOREACH:
                    PushScope(node);
                    throw new RunTimeError(node, "For loops are not supported yet");
                case ASTNodeType.ENDFOR:
                    // Walk backward to the for statement
                    _statement = _statement.Prev();

                    while (_statement != null)
                    {
                        node = _statement.FirstChild();

                        if (node.Type == ASTNodeType.FOR ||
                            node.Type == ASTNodeType.FOREACH)
                        {
                            break;
                        }

                        _statement = _statement.Prev();
                    }

                    if (_statement == null)
                        throw new RunTimeError(node, "Unexpected endfor");

                    PopScope();
                    break;
                case ASTNodeType.BREAK:
                    // Walk until the end of the loop
                    _statement = _statement.Next();

                    depth = 0;

                    while (_statement != null)
                    {
                        node = _statement.FirstChild();

                        if (node.Type == ASTNodeType.WHILE ||
                            node.Type == ASTNodeType.FOR ||
                            node.Type == ASTNodeType.FOREACH)
                        {
                            depth++;
                        }
                        else if (node.Type == ASTNodeType.ENDWHILE ||
                            node.Type == ASTNodeType.ENDFOR)
                        {
                            if (depth == 0)
                            {
                                PopScope();

                                // Go one past the end so the loop doesn't repeat
                                _statement = _statement.Next();
                                break;
                            }

                            depth--;
                        }

                        _statement = _statement.Next();
                    }

                    PopScope();
                    break;
                case ASTNodeType.CONTINUE:
                    // Walk backward to the loop statement
                    _statement = _statement.Prev();

                    depth = 0;

                    while (_statement != null)
                    {
                        node = _statement.FirstChild();

                        if (node.Type == ASTNodeType.ENDWHILE ||
                            node.Type == ASTNodeType.ENDFOR)
                        {
                            depth++;
                        }
                        else if (node.Type == ASTNodeType.WHILE ||
                                 node.Type == ASTNodeType.FOR ||
                                 node.Type == ASTNodeType.FOREACH)
                        {
                            if (depth == 0)
                                break;

                            depth--;
                        }

                        _statement = _statement.Prev();
                    }

                    if (_statement == null)
                        throw new RunTimeError(node, "Unexpected continue");
                    break;
                case ASTNodeType.STOP:
                    _statement = null;
                    break;
                case ASTNodeType.REPLAY:
                    _statement = _statement.Parent.FirstChild();
                    break;
                case ASTNodeType.QUIET:
                case ASTNodeType.FORCE:
                case ASTNodeType.COMMAND:
                    if (ExecuteCommand(node))
                        _statement = _statement.Next();
                    break;
            }

            return (_statement != null) ? true : false;
        }

        private ASTNode EvaluateModifiers(ASTNode node, out bool quiet, out bool force, out bool not)
        {
            quiet = false;
            force = false;
            not = false;

            while (true)
            {
                switch (node.Type)
                {
                    case ASTNodeType.QUIET:
                        quiet = true;
                        break;
                    case ASTNodeType.FORCE:
                        force = true;
                        break;
                    case ASTNodeType.NOT:
                        not = true;
                        break;
                    default:
                        return node;
                }

                node = node.Next();
            }
        }

        private bool ExecuteCommand(ASTNode node)
        {
            node = EvaluateModifiers(node, out bool quiet, out bool force, out _);

            var handler = Interpreter.GetCommandHandler(node.Lexeme);

            if (handler == null)
                throw new RunTimeError(node, "Unknown command");

            var cont = handler(node.Lexeme, ConstructArguments(ref node), quiet, force);

            if (node != null)
                throw new RunTimeError(node, "Command did not consume all available arguments");

            return cont;
        }

        private bool EvaluateExpression(ref ASTNode expr)
        {
            if (expr == null || (expr.Type != ASTNodeType.UNARY_EXPRESSION && expr.Type != ASTNodeType.BINARY_EXPRESSION && expr.Type != ASTNodeType.LOGICAL_EXPRESSION))
                throw new RunTimeError(expr, "No expression following control statement");

            var node = expr.FirstChild();

            if (node == null)
                throw new RunTimeError(expr, "Empty expression following control statement");

            switch (expr.Type)
            {
                case ASTNodeType.UNARY_EXPRESSION:
                    return EvaluateUnaryExpression(ref node);
                case ASTNodeType.BINARY_EXPRESSION:
                    return EvaluateBinaryExpression(ref node);
            }

            bool lhs = EvaluateExpression(ref node);

            node = node.Next();

            while (node != null)
            {
                // Capture the operator
                var op = node.Type;
                node = node.Next();

                if (node == null)
                    throw new RunTimeError(node, "Invalid logical expression");

                bool rhs;

                var e = node.FirstChild();

                switch (node.Type)
                {
                    case ASTNodeType.UNARY_EXPRESSION:
                        rhs = EvaluateUnaryExpression(ref e);
                        break;
                    case ASTNodeType.BINARY_EXPRESSION:
                        rhs = EvaluateBinaryExpression(ref e);
                        break;
                    default:
                        throw new RunTimeError(node, "Nested logical expressions are not possible");
                }

                switch (op)
                {
                    case ASTNodeType.AND:
                        lhs = lhs && rhs;
                        break;
                    case ASTNodeType.OR:
                        lhs = lhs || rhs;
                        break;
                    default:
                        throw new RunTimeError(node, "Invalid logical operator");
                }

                node = node.Next();
            }

            return lhs;
        }

        private bool EvaluateUnaryExpression(ref ASTNode node)
        {
            node = EvaluateModifiers(node, out bool quiet, out _, out bool not);

            // Unary expressions are converted to bool.
            var result = ExecuteExpression(ref node, quiet) != 0;

            if (not)
                return !result;
            else
                return result;
        }

        private bool EvaluateBinaryExpression(ref ASTNode node)
        {
            int lhs;
            int rhs;

            // Evaluate the left hand side
            node = EvaluateModifiers(node, out bool quiet, out _, out _);
            if (node.Type == ASTNodeType.INTEGER)
            {
                lhs = int.Parse(node.Lexeme);
                node = node.Next();
            }
            else
                lhs = ExecuteExpression(ref node, quiet);

            // Capture the operator
            var op = node.Type;
            node = node.Next();

            // Evaluate the right hand side
            node = EvaluateModifiers(node, out quiet, out _, out _);
            if (node.Type == ASTNodeType.INTEGER)
            {
                rhs = int.Parse(node.Lexeme);
                node = node.Next();
            }
            else
                rhs = ExecuteExpression(ref node, quiet);

            switch (op)
            {
                case ASTNodeType.EQUAL:
                    return lhs == rhs;
                case ASTNodeType.NOT_EQUAL:
                    return lhs != rhs;
                case ASTNodeType.LESS_THAN:
                    return lhs < rhs;
                case ASTNodeType.LESS_THAN_OR_EQUAL:
                    return lhs <= rhs;
                case ASTNodeType.GREATER_THAN:
                    return lhs > rhs;
                case ASTNodeType.GREATER_THAN_OR_EQUAL:
                    return lhs >= rhs;
            }

            throw new RunTimeError(node, "Invalid operator type in expression");
        }

        private int ExecuteExpression(ref ASTNode node, bool quiet)
        {
            var handler = Interpreter.GetExpressionHandler(node.Lexeme);

            if (handler == null)
                throw new RunTimeError(node, "Unknown expression");

            var result = handler(node.Lexeme, ConstructArguments(ref node), quiet);

            return result;
        }
    }

    public static class Interpreter
    {
        // Aliases only hold serial numbers
        private static Dictionary<string, uint> _aliases = new Dictionary<string, uint>();

        // Lists
        private static Dictionary<string, object[]> _lists = new Dictionary<string, object[]>();

        public delegate int ExpressionHandler(string expression, Argument[] args, bool quiet);

        private static Dictionary<string, ExpressionHandler> _exprHandlers = new Dictionary<string, ExpressionHandler>();

        public delegate bool CommandHandler(string command, Argument[] args, bool quiet, bool force);

        private static Dictionary<string, CommandHandler> _commandHandlers = new Dictionary<string, CommandHandler>();

        public delegate uint AliasHandler(string alias);

        private static Dictionary<string, AliasHandler> _aliasHandlers = new Dictionary<string, AliasHandler>();

        private static LinkedList<Script> _scripts = new LinkedList<Script>();

        public static CultureInfo Culture;

        static Interpreter()
        {
            Culture = new CultureInfo("en-EN", false);
            Culture.NumberFormat.NumberDecimalSeparator = ".";
            Culture.NumberFormat.NumberGroupSeparator = ",";
        }

        public static void RegisterExpressionHandler(string keyword, ExpressionHandler handler)
        {
            _exprHandlers[keyword] = handler;
        }

        public static ExpressionHandler GetExpressionHandler(string keyword)
        {
            _exprHandlers.TryGetValue(keyword, out ExpressionHandler handler);

            return handler;
        }

        public static void RegisterCommandHandler(string keyword, CommandHandler handler)
        {
            _commandHandlers[keyword] = handler;
        }

        public static CommandHandler GetCommandHandler(string keyword)
        {
            _commandHandlers.TryGetValue(keyword, out CommandHandler handler);

            return handler;
        }

        public static void RegisterAliasHandler(string keyword, AliasHandler handler)
        {
            _aliasHandlers[keyword] = handler;
        }

        public static uint GetAlias(string alias)
        {
            // If a handler is explicitly registered, call that.
            if (_aliasHandlers.TryGetValue(alias, out AliasHandler handler))
                return handler(alias);

            uint value;
            if (_aliases.TryGetValue(alias, out value))
                return value;

            return uint.MaxValue;
        }

        public static void SetAlias(string alias, uint serial)
        {
            _aliases[alias] = serial;
        }

        public static void StartScript(Script script)
        {
            _scripts.AddLast(script);
        }

        public static void StopScript(Script script)
        {
            _scripts.Remove(script);
        }

        public static bool ExecuteScripts()
        {
            var node = _scripts.Last;

            while (node != null)
            {
                var prev = node.Previous;

                if (!node.Value.ExecuteNext())
                    _scripts.Remove(node);

                node = prev;
            }

            return _scripts.Count > 0;
        }
    }
}