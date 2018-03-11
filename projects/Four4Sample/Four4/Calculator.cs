// <copyright file="Calculator.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4
{
    using System;
    using System.Collections.Generic;

    public static class Calculator
    {
        public static Number Eval(Number num, string input) => Eval(new NumberStack(num), input);

        public static Number Eval(string input) => Eval(new NumberStack(), input);

        private static Number Eval(NumberStack operands, string input)
        {
            string[] tokens = input.Split(' ');
            foreach (string token in tokens)
            {
                switch (token)
                {
                    case "+":
                        Binary(operands, (x, y) => x + y);
                        break;
                    case "-":
                        Binary(operands, (x, y) => x - y);
                        break;
                    case "*":
                        Binary(operands, (x, y) => x * y);
                        break;
                    case "/":
                        Binary(operands, (x, y) => x / y);
                        break;
                    case "!":
                        Unary(operands, x => x.Factorial());
                        break;
                    case "R":
                        Unary(operands, x => x.SquareRoot());
                        break;
                    default:
                        operands.Push(Number.Parse(token));
                        break;
                }
            }

            return operands.Result();
        }

        private static void Binary(NumberStack operands, Func<Number, Number, Number> op)
        {
            var y = operands.Pop();
            var x = operands.Pop();
            operands.Push(op(x, y));
        }

        private static void Unary(NumberStack operands, Func<Number, Number> op)
        {
            var x = operands.Pop();
            operands.Push(op(x));
        }

        private sealed class NumberStack
        {
            private readonly Stack<Number> stack;

            public NumberStack()
            {
                this.stack = new Stack<Number>();
            }

            public NumberStack(Number n)
                : this()
            {
                this.Push(n);
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
