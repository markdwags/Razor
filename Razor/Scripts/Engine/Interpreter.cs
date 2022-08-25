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

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Assistant.Scripts.Engine
{
    public class RunTimeError : Exception
    {
        public RunTimeError(string error) : base(error)
        {
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

            throw new RunTimeError("Cannot convert argument to int");
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

            throw new RunTimeError("Cannot convert argument to uint");
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

            throw new RunTimeError("Cannot convert argument to ushort");
        }

        public static double ToDouble(string token)
        {
            double val;

            if (double.TryParse(token, out val))
                return val;

            throw new RunTimeError("Cannot convert argument to double");
        }

        public static bool ToBool(string token)
        {
            bool val;

            if (bool.TryParse(token, out val))
                return val;

            throw new RunTimeError("Cannot convert argument to bool");
        }
    }

    internal class Scope
    {
        private Dictionary<string, Variable> _namespace = new Dictionary<string, Variable>();
        private readonly HashSet<Serial> _ignoreList = new HashSet<Serial>();

        public readonly ASTNode StartNode;
        public readonly Scope Parent;

        public Scope(Scope parent, ASTNode start)
        {
            Parent = parent;
            StartNode = start;
        }

        public Variable GetVariable(string name)
        {
            Variable arg;

            if (_namespace.TryGetValue(name, out arg))
                return arg;

            return null;
        }

        public void SetVariable(string name, Variable val)
        {
            _namespace[name] = val;
        }

        public void ClearVariable(string name)
        {
            _namespace.Remove(name);
        }

        public bool ExistVariable(string name)
        {
            return _namespace.ContainsKey(name);
        }

        public void AddIgnore(Serial serial)
        {
            _ignoreList.Add(serial);
        }

        public void ClearIgnore()
        {
            _ignoreList.Clear();
        }

        public bool CheckIgnored(Serial serial)
        {
            return _ignoreList.Contains(serial);
        }

    }

    public class Variable
    {
        private string _value;

        public Variable(string value)
        {
            _value = value;
        }

        // Treat the argument as an integer
        public int AsInt()
        {
            if (_value == null)
                throw new RunTimeError("Cannot convert argument to int");

            // Try to resolve it as a scoped variable first
            var arg = Interpreter.GetVariable(_value);
            if (arg != null)
                return arg.AsInt();

            return TypeConverter.ToInt(_value);
        }

        // Treat the argument as an unsigned integer
        public uint AsUInt()
        {
            if (_value == null)
                throw new RunTimeError("Cannot convert argument to uint");

            // Try to resolve it as a scoped variable first
            var arg = Interpreter.GetVariable(_value);
            if (arg != null)
                return arg.AsUInt();

            return TypeConverter.ToUInt(_value);
        }

        public ushort AsUShort()
        {
            if (_value == null)
                throw new RunTimeError("Cannot convert argument to ushort");

            // Try to resolve it as a scoped variable first
            var arg = Interpreter.GetVariable(_value);
            if (arg != null)
                return arg.AsUShort();

            return TypeConverter.ToUShort(_value);
        }

        // Treat the argument as a serial or an alias. Aliases will
        // be automatically resolved to serial numbers.
        public uint AsSerial()
        {
            if (_value == null)
                throw new RunTimeError("Cannot convert argument to serial");

            // Try to resolve it as a scoped variable first
            var arg = Interpreter.GetVariable(_value);
            if (arg != null)
                return arg.AsSerial();

            // Resolve it as a global alias next
            uint serial = Interpreter.GetAlias(_value);
            if (serial != uint.MaxValue)
                return serial;

            try
            {
                return AsUInt();
            }
            catch (RunTimeError)
            { }

            return Serial.MinusOne;
        }

        // Treat the argument as a string
        public string AsString(bool resolve = true)
        {
            if (_value == null)
                throw new RunTimeError("Cannot convert argument to string");

            if (resolve)
            {
                // Try to resolve it as a scoped variable first
                var arg = Interpreter.GetVariable(_value);
                if (arg != null)
                    return arg.AsString();
            }

            return _value;
        }

        public bool AsBool()
        {
            if (_value == null)
                throw new RunTimeError("Cannot convert argument to bool");

            return TypeConverter.ToBool(_value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Variable arg = obj as Variable;

            if (arg == null)
                return false;

            return Equals(arg);
        }

        public bool Equals(Variable other)
        {
            if (other == null)
                return false;

            return (other._value == _value);
        }
    }

    public class Script
    {
        private ASTNode _statement;

        public int CurrentLine
        {
            get
            {
                return _statement == null ? 0 : _statement.LineNumber;
            }
        }

        private Variable[] ConstructArguments(ref ASTNode node)
        {
            List<Variable> args = new List<Variable>();

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
                    case ASTNodeType.AS:
                        return args.ToArray();
                }

                args.Add(new Variable(node.Lexeme));

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
        }

        public void Initialize()
        {
            Interpreter.PushScope(_statement);
        }

        public bool ExecuteNext()
        {
            if (_statement == null)
                return false;

            if (_statement.Type != ASTNodeType.STATEMENT)
                throw new RunTimeError("Invalid script");

            var node = _statement.FirstChild();

            if (node == null)
                throw new RunTimeError("Invalid statement");

            int depth = 0;

            switch (node.Type)
            {
                case ASTNodeType.IF:
                    {
                        Interpreter.PushScope(node);

                        var expr = node.FirstChild();
                        var result = EvaluateExpression(ref expr);

                        // Advance to next statement
                        Advance();

                        // Evaluated true. Jump right into execution.
                        if (result)
                            break;

                        // The expression evaluated false, so keep advancing until
                        // we hit an elseif, else, or endif statement that matches
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
                                if (depth == 0)
                                {
                                    expr = node.FirstChild();
                                    result = EvaluateExpression(ref expr);

                                    // Evaluated true. Jump right into execution
                                    if (result)
                                    {
                                        Advance();
                                        break;
                                    }
                                }
                            }
                            else if (node.Type == ASTNodeType.ELSE)
                            {
                                if (depth == 0)
                                {
                                    // Jump into the else clause
                                    Advance();
                                    break;
                                }
                            }
                            else if (node.Type == ASTNodeType.ENDIF)
                            {
                                if (depth == 0)
                                    break;

                                depth--;
                            }

                            Advance();
                        }

                        if (_statement == null)
                            throw new RunTimeError("If with no matching endif");

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

                        Advance();
                    }

                    if (_statement == null)
                        throw new RunTimeError("If with no matching endif");

                    break;
                case ASTNodeType.ENDIF:
                    Interpreter.PopScope();
                    Advance();
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

                        Advance();
                    }

                    if (_statement == null)
                        throw new RunTimeError("If with no matching endif");

                    break;
                case ASTNodeType.WHILE:
                    {
                        // The iterator variable's name is the hash code of the for loop's ASTNode.
                        var iterName = node.GetHashCode().ToString();

                        // When we first enter the loop, push a new scope
                        if (Interpreter.CurrentScope.StartNode != node)
                        {
                            Interpreter.PushScope(node);
                            Interpreter.SetVariable(iterName, "0");
                            Interpreter.SetVariable("index", "0");
                        }
                        else
                        {
                            // Increment the iterator argument
                            var arg = Interpreter.GetVariable(iterName);
                            var index = arg.AsUInt() + 1;
                            Interpreter.SetVariable(iterName, index.ToString());
                            Interpreter.SetVariable("index", index.ToString());
                        }

                        var expr = node.FirstChild();
                        var result = EvaluateExpression(ref expr);

                        // Advance to next statement
                        Advance();

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
                                        Interpreter.PopScope();
                                        // Go one past the endwhile so the loop doesn't repeat
                                        Advance();
                                        break;
                                    }

                                    depth--;
                                }

                                Advance();
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
                        throw new RunTimeError("Unexpected endwhile");

                    break;
                case ASTNodeType.FOR:
                    {
                        // The iterator variable's name is the hash code of the for loop's ASTNode.
                        var iterName = node.GetHashCode().ToString();

                        // When we first enter the loop, push a new scope
                        if (Interpreter.CurrentScope.StartNode != node)
                        {
                            Interpreter.PushScope(node);

                            // Grab the arguments
                            var max = node.FirstChild();

                            if (max.Type != ASTNodeType.INTEGER)
                                throw new RunTimeError("Invalid for loop syntax");

                            // Create a dummy argument that acts as our loop variable
                            Interpreter.SetVariable(iterName, "0");
                            Interpreter.SetVariable("index", "0");
                        }
                        else
                        {
                            // Increment the iterator argument
                            var arg = Interpreter.GetVariable(iterName);
                            var index = arg.AsUInt() + 1;
                            Interpreter.SetVariable(iterName, index.ToString());
                            Interpreter.SetVariable("index", index.ToString());
                        }

                        // Check loop condition
                        var i = Interpreter.GetVariable(iterName);

                        // Grab the max value to iterate to
                        node = node.FirstChild();
                        var end = new Variable(node.Lexeme);

                        if (i.AsUInt() < end.AsUInt())
                        {
                            // enter the loop
                            Advance();
                        }
                        else
                        {
                            // Walk until the end of the loop
                            Advance();

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
                                        Interpreter.PopScope();
                                        // Go one past the end so the loop doesn't repeat
                                        Advance();
                                        break;
                                    }

                                    depth--;
                                }

                                Advance();
                            }
                        }
                    }
                    break;
                case ASTNodeType.ENDFOR:
                    // Walk backward to the for statement
                    _statement = _statement.Prev();

                    // track depth in case there is a nested for
                    depth = 0;

                    while (_statement != null)
                    {
                        node = _statement.FirstChild();

                        if (node.Type == ASTNodeType.ENDFOR)
                        {
                            depth++;
                        }
                        else if (node.Type == ASTNodeType.FOR)
                        {
                            if (depth == 0)
                            {
                                break;
                            }
                            depth--;
                        }

                        _statement = _statement.Prev();
                    }

                    if (_statement == null)
                        throw new RunTimeError("Unexpected endfor");

                    break;
                case ASTNodeType.BREAK:
                    // Walk until the end of the loop
                    Advance();

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
                                // Go one past the end so the loop doesn't repeat
                                Advance();
                                break;
                            }

                            depth--;
                        }

                        Advance();
                    }

                    Interpreter.PopScope();
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
                        throw new RunTimeError("Unexpected continue");
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
                        Advance();

                    break;
            }

            return (_statement != null) ? true : false;
        }

        public void Advance()
        {
            Interpreter.ClearTimeout();
            _statement = _statement.Next();
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
                throw new RunTimeError("Unknown command");

            var cont = handler(node.Lexeme, ConstructArguments(ref node), quiet, force);

            if (node != null)
                throw new RunTimeError("Command did not consume all available arguments");

            return cont;
        }

        private bool EvaluateExpression(ref ASTNode expr)
        {
            if (expr == null || (expr.Type != ASTNodeType.UNARY_EXPRESSION && expr.Type != ASTNodeType.BINARY_EXPRESSION && expr.Type != ASTNodeType.LOGICAL_EXPRESSION))
                throw new RunTimeError("No expression following control statement");

            var node = expr.FirstChild();

            if (node == null)
                throw new RunTimeError("Empty expression following control statement");

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
                    throw new RunTimeError("Invalid logical expression");

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
                        throw new RunTimeError("Nested logical expressions are not possible");
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
                        throw new RunTimeError("Invalid logical operator");
                }

                node = node.Next();
            }

            return lhs;
        }

        private bool CompareOperands(ASTNodeType op, IComparable lhs, IComparable rhs)
        {
            if (op == ASTNodeType.AS)
            {
                if (lhs.GetType() != typeof(uint))
                {
                    throw new RunTimeError("The left hand side of an 'as' expression must evaluate to a serial");
                }

                if (rhs.GetType() != typeof(string))
                {
                    throw new RunTimeError("The right hand side of an 'as' expression must evaluate to a string");
                }
            }
            else if (lhs.GetType() != rhs.GetType())
            {
                // Different types. Try to convert one to match the other.

                if (rhs is double)
                {
                    // Special case for rhs doubles because we don't want to lose precision.
                    lhs = (double)lhs;
                }
                else if (rhs is bool)
                {
                    // Special case for rhs bools because we want to down-convert the lhs.
                    var tmp = Convert.ChangeType(lhs, typeof(bool));
                    lhs = (IComparable)tmp;
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
                    case ASTNodeType.AS:
                        Interpreter.SetVariable(rhs.ToString(), lhs.ToString());
                        return CompareOperands(ASTNodeType.EQUAL, lhs, true);
                }
            }
            catch (ArgumentException e)
            {
                throw new RunTimeError(e.Message);
            }

            throw new RunTimeError("Unknown operator in expression");

        }

        private bool EvaluateUnaryExpression(ref ASTNode node)
        {
            node = EvaluateModifiers(node, out bool quiet, out bool force, out bool not);

            var handler = Interpreter.GetExpressionHandler(node.Lexeme);

            if (handler == null)
                throw new RunTimeError("Unknown expression");

            var result = handler(node.Lexeme, ConstructArguments(ref node), quiet, force);

            if (not)
                return CompareOperands(ASTNodeType.EQUAL, result, false);
            else
                return CompareOperands(ASTNodeType.EQUAL, result, true);
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

            return CompareOperands(op, lhs, rhs);
        }

        private IComparable EvaluateBinaryOperand(ref ASTNode node)
        {
            IComparable val;

            node = EvaluateModifiers(node, out bool quiet, out bool force, out _);
            switch (node.Type)
            {
                case ASTNodeType.INTEGER:
                    val = TypeConverter.ToInt(node.Lexeme);
                    node = node.Next();
                    break;
                case ASTNodeType.SERIAL:
                    val = TypeConverter.ToUInt(node.Lexeme);
                    node = node.Next();
                    break;
                case ASTNodeType.STRING:
                    val = node.Lexeme;
                    node = node.Next();
                    break;
                case ASTNodeType.DOUBLE:
                    val = TypeConverter.ToDouble(node.Lexeme);
                    node = node.Next();
                    break;
                case ASTNodeType.OPERAND:
                    {
                        // This might be a registered keyword, so do a lookup
                        var handler = Interpreter.GetExpressionHandler(node.Lexeme);

                        if (handler != null)
                        {
                            val = handler(node.Lexeme, ConstructArguments(ref node), quiet, force);
                            break;
                        }

                        // It could be a variable
                        var arg = Interpreter.GetVariable(node.Lexeme);
                        if (arg != null)
                        {
                            // TODO: Should really look at the type of arg here
                            val = arg.AsString();
                            node = node.Next();
                            break;
                        }

                        // It's just a string
                        val = node.Lexeme;
                        node = node.Next();
                        break;
                    }
                default:
                    throw new RunTimeError("Invalid type found in expression");
            }

            return val;
        }
    }

    public static class Interpreter
    {
        // The "global" scope
        private readonly static Scope _scope = new Scope(null, null);

        // The current scope
        private static Scope _currentScope = _scope;

        // Expressions
        public delegate IComparable ExpressionHandler(string expression, Variable[] args, bool quiet, bool force);
        public delegate T ExpressionHandler<T>(string expression, Variable[] args, bool quiet, bool force) where T : IComparable;

        private static Dictionary<string, ExpressionHandler> _exprHandlers = new Dictionary<string, ExpressionHandler>();

        public delegate bool CommandHandler(string command, Variable[] args, bool quiet, bool force);

        private static Dictionary<string, CommandHandler> _commandHandlers = new Dictionary<string, CommandHandler>();

        public delegate uint AliasHandler(string alias);

        private static Dictionary<string, AliasHandler> _aliasHandlers = new Dictionary<string, AliasHandler>();

        private static Script _activeScript = null;

        public static int CurrentLine
        {
            get
            {
                return _activeScript == null ? 0 : _activeScript.CurrentLine;
            }
        }

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

        internal static CultureInfo Culture;

        static Interpreter()
        {
            Culture = new CultureInfo(CultureInfo.CurrentCulture.LCID, false);
            Culture.NumberFormat.NumberDecimalSeparator = ".";
            Culture.NumberFormat.NumberGroupSeparator = ",";
        }

        public static void RegisterExpressionHandler<T>(string keyword, ExpressionHandler<T> handler) where T : IComparable
        {
            _exprHandlers[keyword] = (expression, args, quiet, force) => handler(expression, args, quiet, force);
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

        public static bool AliasHandlerExist(string alias)
        {
            return _aliasHandlers.TryGetValue(alias, out _);
        }

        public static void PushScope(ASTNode node)
        {
            _currentScope = new Scope(_currentScope, node);
        }

        public static void PopScope()
        {
            if (_currentScope == _scope)
                throw new RunTimeError("Attempted to remove global scope");

            _currentScope = _currentScope.Parent;
        }

        internal static Scope CurrentScope => _currentScope;

        public static void SetVariable(string name, string value, bool global = false)
        {
            Scope scope = global ? _scope : _currentScope;

            scope.SetVariable(name, new Variable(value));
        }

        public static void AddIgnore(Serial serial, bool global = true)
        {
            Scope scope = global ? _scope : _currentScope;
            scope.AddIgnore(serial);
        }

        public static void ClearIgnore(bool global = true)
        {
            Scope scope = global ? _scope : _currentScope;
            scope.ClearIgnore();
        }


        public static bool CheckIgnored(Serial serial, bool global = true)
        {
            Scope scope = global ? _scope : _currentScope;
            return scope.CheckIgnored(serial);
        }

        public static Variable GetVariable(string name)
        {
            var scope = _currentScope;
            Variable result = null;

            while (scope != null)
            {
                result = scope.GetVariable(name);
                if (result != null)
                    return result;

                scope = scope.Parent;
            }

            return result;
        }

        public static void ClearVariable(string name)
        {
            _currentScope.ClearVariable(name);
        }

        public static bool ExistVariable(string name)
        {
            return _currentScope.ExistVariable(name);
        }

        public static uint GetAlias(string alias)
        {
            // If a handler is explicitly registered, call that.
            if (_aliasHandlers.TryGetValue(alias, out AliasHandler handler))
                return handler(alias);

            var arg = GetVariable(alias);

            return arg?.AsUInt() ?? uint.MaxValue;
        }

        public static void SetAlias(string alias, uint serial)
        {
            SetVariable(alias, serial.ToString(), true);
        }

        public static void ClearAlias(string alias)
        {
            _scope.ClearVariable(alias);
        }

        public static bool ExistAlias(string alias)
        {
            return _scope.ExistVariable(alias);
        }

        public static bool StartScript(Script script)
        {
            if (_activeScript != null)
                return false;

            _currentScope = _scope;
            _activeScript = script;
            _activeScript.Initialize();
            _executionState = ExecutionState.RUNNING;

            ExecuteScript();

            return true;
        }

        public static void StopScript()
        {
            _activeScript = null;
            _currentScope = _scope;
            _executionState = ExecutionState.RUNNING;
        }

        public static void PauseScript()
        {
            _pauseTimeout = DateTime.MaxValue.Ticks;
            _executionState = ExecutionState.PAUSED;
        }
        public static void ResumeScript()
        {
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
            if (_executionState != ExecutionState.TIMING_OUT)
                return;

            _pauseTimeout = 0;
            _executionState = ExecutionState.RUNNING;
        }
    }
}