// <copyright file="Calculation.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4
{
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
                        var y = operands.Pop();
                        var x = operands.Pop();
                        operands.Push(x + y);
                        break;
                    default:
                        operands.Push(Number.Parse(token));
                        break;
                }
            }

            return operands.Pop().ToString();
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
            }

            public static Number operator +(Number left, Number right)
            {
                int n = (left.num * right.denom) + (right.num * left.denom);
                int d = left.denom * right.denom;
                return new Number(n, d);
            }

            public static Number Parse(string s)
            {
                if (s == ".4_")
                {
                    return new Number(4, 9);
                }
                else if (s.StartsWith('.'))
                {
                    int n = s.Length - 1;
                    int d = 1;
                    for (int i = 0; i < n; ++i)
                    {
                        d *= 10;
                    }

                    return new Number(int.Parse(s.Substring(1)), d);
                }
                else
                {
                    return new Number(int.Parse(s), 1);
                }
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
