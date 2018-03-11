// <copyright file="Calculation.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4
{
    using System;
    using System.Collections.Generic;

    public static class Calculation
    {
        public static string FromString(string input)
        {
            Stack<Number> operands = new Stack<Number>();
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
                    default:
                        operands.Push(Number.Parse(token));
                        break;
                }
            }

            return operands.Pop().ToString();
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

        private struct Number
        {
            private readonly int num;
            private readonly int denom;

            private Number(int num, int denom)
            {
                int gcd = Gcd(num, denom);
                this.num = num / gcd;
                this.denom = denom / gcd;

                if (this.denom < 0)
                {
                    this.num *= -1;
                    this.denom *= -1;
                }
            }

            public static Number operator +(Number left, Number right)
            {
                int n = (left.num * right.denom) + (right.num * left.denom);
                int d = left.denom * right.denom;
                return new Number(n, d);
            }

            public static Number operator -(Number left, Number right)
            {
                return left + (-right);
            }

            public static Number operator -(Number right)
            {
                return new Number(-right.num, right.denom);
            }

            public static Number operator *(Number left, Number right)
            {
                int n = left.num * right.num;
                int d = left.denom * right.denom;
                return new Number(n, d);
            }

            public static Number operator /(Number left, Number right)
            {
                return left * new Number(right.denom, right.num);
            }

            public static Number Parse(string s)
            {
                if (s == ".4_")
                {
                    return new Number(4, 9);
                }

                int p = s.IndexOf('.');
                if (p < 0)
                {
                    return new Number(int.Parse(s), 1);
                }

                Number whole = new Number(0, 1);
                Number frac = new Number(0, 1);
                if (p > 0)
                {
                    whole = new Number(int.Parse(s.Substring(0, p)), 1);
                }

                int n = s.Length - p - 1;
                int d = 1;
                for (int i = 0; i < n; ++i)
                {
                    d *= 10;
                }

                frac = new Number(int.Parse(s.Substring(p + 1)), d);

                return whole + frac;
            }

            public Number Factorial()
            {
                int n = this.num;
                int fact = 1;
                for (int i = 2; i <= n; ++i)
                {
                    fact *= i;
                }

                return new Number(fact, 1);
            }

            public override string ToString()
            {
                string value = this.num.ToString();
                if (this.denom != 1)
                {
                    value += "/" + this.denom;
                }

                return value;
            }

            private static int Gcd(int a, int b)
            {
                while (b != 0)
                {
                    int t = b;
                    b = a % b;
                    a = t;
                }

                return a;
            }
        }
    }
}
