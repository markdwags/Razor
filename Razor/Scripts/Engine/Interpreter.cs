#region license

// Razor: An Ultima Online Assistant
// Copyright (C) 2020 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Assistant.Scripts.Engine
{
    public class RunTimeError : Exception
    {
        public ASTNode Node;

        public RunTimeError(ASTNode node, string error) : base(error)
        {
            Node = node;
        }
    }

    internal static class TypeConverter
    {
        public static int ToInt(string token)
        {
            int val;

            if (token.StartsWith("0x"))
            {
                if (int.TryParse(token.Substring(2), NumberStyles.HexNumber, Interpreter.Culture, out val))
                    return val;
            }
            else if (int.TryParse(token, out val))
                return val;

            throw new RunTimeError(null, "Cannot convert argument to int");
        }

        public static uint ToUInt(string token)
        {
            uint val;

            if (token.StartsWith("0x"))
            {
                if (uint.TryParse(token.Substring(2), NumberStyles.HexNumber, Interpreter.Culture, out val))
                    return val;
            }
            else if (uint.TryParse(token, out val))
                return val;

            throw new RunTimeError(null, "Cannot convert argument to uint");
        }

        public static ushort ToUShort(string token)
        {
            ushort val;

            if (token.StartsWith("0x"))
            {
                if (ushort.TryParse(token.Substring(2), NumberStyles.HexNumber, Interpreter.Culture, out val))
                    return val;
            }
            else if (ushort.TryParse(token, out val))
                return val;

            throw new RunTimeError(null, "Cannot convert argument to ushort");
        }

        public static double ToDouble(string token)
        {
            double val;

            if (double.TryParse(token, out val))
                return val;

            throw new RunTimeError(null, "Cannot convert argument to double");
        }

        public static bool ToBool(string token)
        {
            bool val;

            if (bool.TryParse(token, out val))
                return val;

            throw new RunTimeError(null, "Cannot convert argument to bool");
        }
    }

    internal class Scope
    {
        private Dictionary<string, Argument> _namespace = new Dictionary<string, Argument>();

        public readonly ASTNode StartNode;
        public readonly Scope Parent;

        public Scope(Scope parent, ASTNode start)
        {
            Parent = parent;
            StartNode = start;
        }

        public Argument GetVar(string name)
        {
            Argument arg;

            if (_namespace.TryGetValue(name, out arg))
                return arg;

            return null;
        }

        public void SetVar(string name, Argument val)
        {
            _namespace[name] = val;
        }

        public void ClearVar(string name)
        {
            _namespace.Remove(name);
        }
    }

    public class Argument
    {
        private ASTNode _node;
        private Script _script;

        public Argument(Script script, ASTNode node)
        {
            _node = node;
            _script = script;
        }

        // Treat the argument as an integer
        public int AsInt()
        {
            if (_node.Lexeme == null)
                throw new RunTimeError(_node, "Cannot convert argument to int");

            // Try to resolve it as a scoped variable first
            var arg = _script.Lookup(_node.Lexeme);
            if (arg != null)
                return arg.AsInt();

            return TypeConverter.ToInt(_node.Lexeme);
        }

        // Treat the argument as an unsigned integer
        public uint AsUInt()
        {
            if (_node.Lexeme == null)
                throw new RunTimeError(_node, "Cannot convert argument to uint");

            // Try to resolve it as a scoped variable first
            var arg = _script.Lookup(_node.Lexeme);
            if (arg != null)
                return arg.AsUInt();

            return TypeConverter.ToUInt(_node.Lexeme);
        }

        public ushort AsUShort()
        {
            if (_node.Lexeme == null)
                throw new RunTimeError(_node, "Cannot convert argument to ushort");

            // Try to resolve it as a scoped variable first
            var arg = _script.Lookup(_node.Lexeme);
            if (arg != null)
                return arg.AsUShort();

            return TypeConverter.ToUShort(_node.Lexeme);
        }

        // Treat the argument as a serial or an alias. Aliases will
        // be automatically resolved to serial numbers.
        public uint AsSerial()
        {
            if (_node.Lexeme == null)
                throw new RunTimeError(_node, "Cannot convert argument to serial");

            // Try to resolve it as a scoped variable first
            var arg = _script.Lookup(_node.Lexeme);
            if (arg != null)
                return arg.AsSerial();

            // Resolve it as a global alias next
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

            // Try to resolve it as a scoped variable first
            var arg = _script.Lookup(_node.Lexeme);
            if (arg != null)
                return arg.AsString();

            return _node.Lexeme;
        }

        public bool AsBool()
        {
            if (_node.Lexeme == null)
                throw new RunTimeError(_node, "Cannot convert argument to bool");

            return TypeConverter.ToBool(_node.Lexeme);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Argument arg = obj as Argument;

            if (arg == null)
                return false;

            return Equals(arg);
        }

        public bool Equals(Argument other)
        {
            if (other == null)
                return false;

            return (other._node.Lexeme == _node.Lexeme);
        }
    }

    public class Script
    {
        private ASTNode _statement;

        private Scope _scope;

        public Argument Lookup(string name)
        {
            var scope = _scope;
            Argument result = null;

            while (scope != null)
            {
                result = scope.GetVar(name);
                if (result != null)
                    return result;

                scope = scope.Parent;
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

                args.Add(new Argument(this, node));

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
                        // When we first enter the loop, push a new scope
                        if (_scope.StartNode != node)
                        {
                            PushScope(node);
                        }

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

                    break;
                case ASTNodeType.FOR:
                    {
                        // The iterator variable's name is the hash code of the for loop's ASTNode.
                        var iterName = node.GetHashCode().ToString();

                        // When we first enter the loop, push a new scope
                        if (_scope.StartNode != node)
                        {
                            PushScope(node);

                            // Grab the arguments
                            var max = node.FirstChild();

                            if (max.Type != ASTNodeType.INTEGER)
                                throw new RunTimeError(max, "Invalid for loop syntax");

                            // Create a dummy argument that acts as our loop variable
                            var iter = new ASTNode(ASTNodeType.INTEGER, "0", node, 0);

                            _scope.SetVar(iterName, new Argument(this, iter));
                        }
                        else
                        {
                            // Increment the iterator argument
                            var arg = _scope.GetVar(iterName);

                            var iter = new ASTNode(ASTNodeType.INTEGER, (arg.AsUInt() + 1).ToString(), node, 0);

                            _scope.SetVar(iterName, new Argument(this, iter));
                        }

                        // Check loop condition
                        var i = _scope.GetVar(iterName);

                        // Grab the max value to iterate to
                        node = node.FirstChild();
                        var end = new Argument(this, node);

                        if (i.AsUInt() < end.AsUInt())
                        {
                            // enter the loop
                            _statement = _statement.Next();
                        }
                        else
                        {
                            // Walk until the end of the loop
                            _statement = _statement.Next();

                            depth = 0;

                            while (_statement != null)
                            {
                                node = _statement.FirstChild();

                                if (node.Type == ASTNodeType.FOR)
                                {
                                    depth++;
                                }
                                else if (node.Type == ASTNodeType.ENDFOR)
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
                        }
                    }
                    break;
                case ASTNodeType.ENDFOR:
                    // Walk backward to the for statement
                    _statement = _statement.Prev();

                    while (_statement != null)
                    {
                        node = _statement.FirstChild();

                        if (node.Type == ASTNodeType.FOR)
                        {
                            break;
                        }

                        _statement = _statement.Prev();
                    }

                    if (_statement == null)
                        throw new RunTimeError(node, "Unexpected endfor");

                    break;
                case ASTNodeType.BREAK:
                    // Walk until the end of the loop
                    _statement = _statement.Next();

                    depth = 0;

                    while (_statement != null)
                    {
                        node = _statement.FirstChild();

                        if (node.Type == ASTNodeType.WHILE ||
                            node.Type == ASTNodeType.FOR)
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
                                 node.Type == ASTNodeType.FOR)
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

        public void Advance() { _statement = _statement.Next(); }

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

            var handler = Interpreter.GetExpressionHandler(node.Lexeme);

            if (handler == null)
                throw new RunTimeError(node, "Unknown expression");

            var result = handler(node.Lexeme, ConstructArguments(ref node), quiet);

            if (not)
                return result.CompareTo(true) != 0;
            else
                return result.CompareTo(true) == 0;
        }

        private bool EvaluateBinaryExpression(ref ASTNode node)
        {
            // Evaluate the left hand side
            var lhs = EvaluateBinaryOperand(ref node);

            // Capture the operator
            var op = node.Type;
            node = node.Next();

            // Evaluate the right hand side
            var rhs = EvaluateBinaryOperand(ref node);

            if (lhs.GetType() != rhs.GetType())
            {
                // Different types. Try to convert one to match the other.

                // Special case for rhs doubles because we don't want to lose precision.
                if (rhs is double)
                {
                    double tmp = (double)lhs;
                    lhs = tmp;
                }
                else
                {
                    var tmp = Convert.ChangeType(rhs, lhs.GetType());
                    rhs = (IComparable)tmp;
                }
            }

            try
            {
                // Evaluate the whole expression
                switch (op)
                {
                    case ASTNodeType.EQUAL:
                        return lhs.CompareTo(rhs) == 0;
                    case ASTNodeType.NOT_EQUAL:
                        return lhs.CompareTo(rhs) != 0;
                    case ASTNodeType.LESS_THAN:
                        return lhs.CompareTo(rhs) < 0;
                    case ASTNodeType.LESS_THAN_OR_EQUAL:
                        return lhs.CompareTo(rhs) <= 0;
                    case ASTNodeType.GREATER_THAN:
                        return lhs.CompareTo(rhs) > 0;
                    case ASTNodeType.GREATER_THAN_OR_EQUAL:
                        return lhs.CompareTo(rhs) >= 0;
                }
            }
            catch (ArgumentException e)
            {
                throw new RunTimeError(node, e.Message);
            }

            throw new RunTimeError(node, "Unknown operator in expression");
        }

        private IComparable EvaluateBinaryOperand(ref ASTNode node)
        {
            IComparable val;

            node = EvaluateModifiers(node, out bool quiet, out _, out _);
            switch (node.Type)
            {
                case ASTNodeType.INTEGER:
                    val = TypeConverter.ToInt(node.Lexeme);
                    break;
                case ASTNodeType.SERIAL:
                    val = TypeConverter.ToUInt(node.Lexeme);
                    break;
                case ASTNodeType.STRING:
                    val = node.Lexeme;
                    break;
                case ASTNodeType.DOUBLE:
                    val = TypeConverter.ToDouble(node.Lexeme);
                    break;
                case ASTNodeType.OPERAND:
                    {
                        // This might be a registered keyword, so do a lookup
                        var handler = Interpreter.GetExpressionHandler(node.Lexeme);

                        if (handler == null)
                        {
                            // It's just a string
                            val = node.Lexeme;
                        }
                        else
                        {
                            val = handler(node.Lexeme, ConstructArguments(ref node), quiet);
                        }
                        break;
                    }
                default:
                    throw new RunTimeError(node, "Invalid type found in expression");
            }

            return val;
        }
    }

    public static class Interpreter
    {
        // Aliases only hold serial numbers
        private static Dictionary<string, uint> _aliases = new Dictionary<string, uint>();

        // Lists
        private static Dictionary<string, List<Argument>> _lists = new Dictionary<string, List<Argument>>();

        // Timers
        private static Dictionary<string, DateTime> _timers = new Dictionary<string, DateTime>();

        // Expressions
        public delegate IComparable ExpressionHandler(string expression, Argument[] args, bool quiet);
        public delegate T ExpressionHandler<T>(string expression, Argument[] args, bool quiet) where T : IComparable;

        private static Dictionary<string, ExpressionHandler> _exprHandlers = new Dictionary<string, ExpressionHandler>();

        public delegate bool CommandHandler(string command, Argument[] args, bool quiet, bool force);

        private static Dictionary<string, CommandHandler> _commandHandlers = new Dictionary<string, CommandHandler>();

        public delegate uint AliasHandler(string alias);

        private static Dictionary<string, AliasHandler> _aliasHandlers = new Dictionary<string, AliasHandler>();

        private static Script _activeScript = null;

        private enum ExecutionState
        {
            RUNNING,
            PAUSED,
            TIMING_OUT
        };

        public delegate bool TimeoutCallback();

        private static ExecutionState _executionState = ExecutionState.RUNNING;
        private static long _pauseTimeout = long.MaxValue;
        private static TimeoutCallback _timeoutCallback = null;

        public static CultureInfo Culture;

        static Interpreter()
        {
            Culture = new CultureInfo(CultureInfo.CurrentCulture.LCID, false);
            Culture.NumberFormat.NumberDecimalSeparator = ".";
            Culture.NumberFormat.NumberGroupSeparator = ",";
        }

        public static void RegisterExpressionHandler<T>(string keyword, ExpressionHandler<T> handler) where T : IComparable
        {
            _exprHandlers[keyword] = (expression, args, quiet) => handler(expression, args, quiet);
        }

        public static ExpressionHandler GetExpressionHandler(string keyword)
        {
            _exprHandlers.TryGetValue(keyword, out var expression);

            return expression;
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

        public static void UnregisterAliasHandler(string keyword)
        {
            _aliasHandlers.Remove(keyword);
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
        public static bool StartScript(Script script)
        {
            if (_activeScript != null)
                return false;

            _activeScript = script;
            _executionState = ExecutionState.RUNNING;

            ExecuteScript();

            return true;
        }

        public static void StopScript()
        {
            _activeScript = null;
            _executionState = ExecutionState.RUNNING;
        }

        public static bool ExecuteScript()
        {
            if (_activeScript == null)
                return false;

            if (_executionState == ExecutionState.PAUSED)
            {
                if (_pauseTimeout < DateTime.UtcNow.Ticks)
                    _executionState = ExecutionState.RUNNING;
                else
                    return true;
            }
            else if (_executionState == ExecutionState.TIMING_OUT)
            {
                if (_pauseTimeout < DateTime.UtcNow.Ticks)
                {
                    if (_timeoutCallback != null)
                    {
                        if (_timeoutCallback())
                        {
                            _activeScript.Advance();
                            ClearTimeout();
                        }

                        _timeoutCallback = null;
                    }

                    /* If the callback changed the state to running, continue
                     * on. Otherwise, exit.
                     */
                    if (_executionState != ExecutionState.RUNNING)
                    {
                        _activeScript = null;
                        return false;
                    }
                }
            }

            if (!_activeScript.ExecuteNext())
            {
                _activeScript = null;
                return false;
            }

            return true;
        }

        // Pause execution for the given number of milliseconds
        public static void Pause(long duration)
        {
            // Already paused or timing out
            if (_executionState != ExecutionState.RUNNING)
                return;

            _pauseTimeout = DateTime.UtcNow.Ticks + (duration * 10000);
            _executionState = ExecutionState.PAUSED;
        }

        // Unpause execution
        public static void Unpause()
        {
            if (_executionState != ExecutionState.PAUSED)
                return;

            _pauseTimeout = 0;
            _executionState = ExecutionState.RUNNING;
        }

        // If forward progress on the script isn't made within this
        // amount of time (milliseconds), bail
        public static void Timeout(long duration, TimeoutCallback callback)
        {
            // Don't change an existing timeout
            if (_executionState != ExecutionState.RUNNING)
                return;

            _pauseTimeout = DateTime.UtcNow.Ticks + (duration * 10000);
            _executionState = ExecutionState.TIMING_OUT;
            _timeoutCallback = callback;
        }

        // Clears any previously set timeout. Automatically
        // called any time the script advances a statement.
        public static void ClearTimeout()
        {
            _pauseTimeout = 0;
            _executionState = ExecutionState.RUNNING;
        }
    }
}