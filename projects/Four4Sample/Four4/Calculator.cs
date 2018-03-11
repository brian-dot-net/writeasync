// <copyright file="Calculator.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4
{
    using System;
    using System.Collections.Generic;

    public sealed class Calculator
    {
        public Number Eval(Number num, string input)
        {
            Stack<Number> operands = new Stack<Number>();
            operands.Push(num);
            return Eval(operands, input);
        }

        public Number Eval(string input) => Eval(new Stack<Number>(), input);

        private static Number Eval(Stack<Number> operands, string input)
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

            return operands.Pop();
        }

        private static void Binary(Stack<Number> operands, Func<Number, Number, Number> op)
        {
            var y = operands.Pop();
            var x = operands.Pop();
            operands.Push(op(x, y));
        }

        private static void Unary(Stack<Number> operands, Func<Number, Number> op)
        {
            var x = operands.Pop();
            operands.Push(op(x));
        }
    }
}
