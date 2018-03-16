// <copyright file="Number.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4
{
    using System;

    public struct Number
    {
        public const string PointFour = ".4";
        public const string PointFourRepeating = ".4_";

        private static readonly Number NaN = default(Number);

        private readonly int num;
        private readonly int denom;

        public Number(int num, int denom)
        {
            if (denom == 0)
            {
                this = NaN;
            }
            else
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
        }

        public bool IsValid => this.denom != 0;

        public bool IsWhole => (this.num > 0) && (this.denom == 1);

        public static explicit operator int(Number right)
        {
            return right.num / right.denom;
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
            switch (s)
            {
                case PointFour:
                    return new Number(2, 5);
                case PointFourRepeating:
                    return new Number(4, 9);
                default:
                    return new Number(int.Parse(s), 1);
            }
        }

        public Number Factorial()
        {
            if ((this.num < 0) || (this.denom != 1) || (this.num == 1) || (this.num == 2))
            {
                return NaN;
            }

            int n = this.num;
            if (n > 12)
            {
                return NaN;
            }

            int fact = 1;
            for (int i = 2; i <= n; ++i)
            {
                fact *= i;
            }

            return new Number(fact, 1);
        }

        public Number SquareRoot()
        {
            if ((this.num <= 0) || ((this.num == 1) && (this.denom == 1)))
            {
                return NaN;
            }

            double nr = Math.Sqrt(this.num);
            if (nr != Math.Floor(nr))
            {
                return NaN;
            }

            double dr = Math.Sqrt(this.denom);
            if (dr != Math.Floor(dr))
            {
                return NaN;
            }

            return new Number((int)nr, (int)dr);
        }

        public Number Pow(Number exp)
        {
            if (exp.denom != 1)
            {
                return NaN;
            }

            int nr = (int)Math.Pow(this.num, exp.num);
            int dr = (int)Math.Pow(this.denom, exp.num);
            return new Number(nr, dr);
        }

        public override string ToString()
        {
            if (this.denom == 0)
            {
                return "NaN";
            }

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
