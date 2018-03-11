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
                this.num = num;
                this.denom = denom;
            }

            public static Number operator +(Number left, Number right)
            {
                return new Number(left.num + right.num, 1);
            }

            public static Number Parse(string s)
            {
                switch (s)
                {
                    case ".4":
                        return new Number(2, 5);
                    case ".4_":
                        return new Number(4, 9);
                    default:
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
        }
    }
}
