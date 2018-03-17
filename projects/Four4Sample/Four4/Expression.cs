// <copyright file="Expression.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4
{
    using System;
    using System.Collections.Immutable;

    public struct Expression
    {
        private readonly string text;
        private readonly NumberStack operands;
        private readonly int count;

        private Expression(int count, string text, NumberStack operands)
        {
            this.count = count;
            this.text = text;
            this.operands = operands;
        }

        public bool IsValid => this.operands.IsValid;

        public int NumeralCount => this.count;

        public Number Result => this.operands.Result;

        public static Number Eval(string input)
        {
            Expression expr = default(Expression);
            string[] tokens = input.Split(' ');
            foreach (string token in tokens)
            {
                expr = expr.Append(token);
            }

            return expr.Result;
        }

        public override string ToString()
        {
            return this.text ?? string.Empty;
        }

        public Expression Append(string token)
        {
            switch (token)
            {
                case "+":
                    return this.Binary(token, (x, y) => x + y);
                case "-":
                    return this.Binary(token, (x, y) => x - y);
                case "*":
                    return this.Binary(token, (x, y) => x * y);
                case "/":
                    return this.Binary(token, (x, y) => x / y);
                case "^":
                    return this.Binary(token, (x, y) => x.Pow(y));
                case "!":
                    return this.Unary(token, x => x.Factorial());
                case "R":
                    return this.Unary(token, x => x.SquareRoot());
                default:
                    int add;
                    if (IsNumeric(token, out add))
                    {
                        Number number = Number.Parse(token);
                        return this.Push(add, token, Number.Parse(token));
                    }
                    else
                    {
                        throw new ArgumentException("Bad token '" + token + "'", nameof(token));
                    }
            }
        }

        private static bool IsNumeric(string token, out int add)
        {
            add = 0;
            int n = token.Length;
            for (int i = 0; i < n; ++i)
            {
                char d = token[i];
                switch (d)
                {
                    case '4':
                        ++add;
                        break;
                    case '.':
                        if (i > 0)
                        {
                            return false;
                        }

                        break;
                    case '_':
                        if (i < (n - 1))
                        {
                            return false;
                        }

                        break;

                    default:
                        return false;
                }
            }

            return add > 0;
        }

        private Expression Push(int add, string token, Number number)
        {
            return new Expression(this.count + add, this.Join(token), this.operands.Push(number));
        }

        private Expression Binary(string token, Func<Number, Number, Number> op)
        {
            return new Expression(this.count, this.Join(token), this.operands.Apply2(op));
        }

        private Expression Unary(string token, Func<Number, Number> op)
        {
            return new Expression(this.count, this.Join(token), this.operands.Apply1(op));
        }

        private string Join(string token)
        {
            if (this.text == null)
            {
                return token;
            }

            return this.text + " " + token;
        }

        private struct NumberStack
        {
            private static readonly NumberStack Invalid = new NumberStack(1, ImmutableStack.Create(default(Number)));

            private readonly int size;
            private readonly ImmutableStack<Number> stack;

            private NumberStack(int size, ImmutableStack<Number> stack)
            {
                this.size = size;
                this.stack = stack;
            }

            public bool IsValid
            {
                get
                {
                    if (this.size == 0)
                    {
                        return true;
                    }

                    return this.stack.Peek().IsValid;
                }
            }

            public Number Result
            {
                get
                {
                    if (this.size == 1)
                    {
                        return this.stack.Peek();
                    }

                    return default(Number);
                }
            }

            public NumberStack Push(Number n)
            {
                if (this.size == 0)
                {
                    return new NumberStack(1, ImmutableStack.Create(n));
                }

                return new NumberStack(this.size + 1, this.stack.Push(n));
            }

            public NumberStack Apply1(Func<Number, Number> op)
            {
                if (this.size < 1)
                {
                    return Invalid;
                }

                Number n1;
                var newStack = this.stack.Pop(out n1);
                Number n = op(n1);
                return new NumberStack(this.size, newStack.Push(n));
            }

            public NumberStack Apply2(Func<Number, Number, Number> op)
            {
                if (this.size < 2)
                {
                    return Invalid;
                }

                var newStack = this.stack;
                Number n2;
                newStack = newStack.Pop(out n2);
                Number n1;
                newStack = newStack.Pop(out n1);
                Number n = op(n1, n2);
                return new NumberStack(this.size - 1, newStack.Push(n));
            }
        }
    }
}