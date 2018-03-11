// <copyright file="Expression.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4
{
    using System;
    using System.Collections.Generic;

    public sealed class Expression
    {
        private readonly NumberStack operands;

        public Expression()
        {
            this.operands = new NumberStack();
        }

        public static Number Eval(string input)
        {
            Expression expr = new Expression();
            string[] tokens = input.Split(' ');
            foreach (string token in tokens)
            {
                expr.Append(token);
            }

            return expr.Result();
        }

        public Number Result() => this.operands.Result();

        public override string ToString()
        {
            return string.Empty;
        }

        private void Append(string token)
        {
            switch (token)
            {
                case "+":
                    this.Binary((x, y) => x + y);
                    break;
                case "-":
                    this.Binary((x, y) => x - y);
                    break;
                case "*":
                    this.Binary((x, y) => x * y);
                    break;
                case "/":
                    this.Binary((x, y) => x / y);
                    break;
                case "!":
                    this.Unary(x => x.Factorial());
                    break;
                case "R":
                    this.Unary(x => x.SquareRoot());
                    break;
                default:
                    this.operands.Push(Number.Parse(token));
                    break;
            }
        }

        private void Binary(Func<Number, Number, Number> op)
        {
            var y = this.operands.Pop();
            var x = this.operands.Pop();
            this.operands.Push(op(x, y));
        }

        private void Unary(Func<Number, Number> op)
        {
            var x = this.operands.Pop();
            this.operands.Push(op(x));
        }

        private sealed class NumberStack
        {
            private readonly Stack<Number> stack;

            public NumberStack()
            {
                this.stack = new Stack<Number>();
            }

            public Number Pop()
            {
                if (this.stack.Count == 0)
                {
                    return Number.NaN;
                }

                return this.stack.Pop();
            }

            public void Push(Number n) => this.stack.Push(n);

            public Number Result()
            {
                if (this.stack.Count > 1)
                {
                    return Number.NaN;
                }

                return this.Pop();
            }
        }
    }
}
