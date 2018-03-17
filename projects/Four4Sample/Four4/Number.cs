// <copyright file="Number.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4
{
    using System;

    public struct Number
    {
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

        public static bool TryParse(string s, out Number number, out int digits)
        {
            number = NaN;
            digits = 0;

            if (s == null)
            {
                return false;
            }

            int a = 0;
            int b = s.Length;
            int nr = 0;
            int dr = 0;
            int f = 1;

            if (b == 0)
            {
                return false;
            }

            if (s[0] == '.')
            {
                a = 1;
                f = 10;
            }

            if (s[b - 1] == '_')
            {
                --b;
                f = 9;
            }

            for (int i = a; i < b; ++i)
            {
                int d = s[i] - '0';
                if (d != 4)
                {
                    return false;
                }

                nr = (10 * nr) + d;
                ++digits;
                if (f != 1)
                {
                    dr = (dr * 10) + f;
                }
                else
                {
                    dr = 1;
                }
            }

            number = new Number(nr, dr);
            return true;
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

            if ((this.num == 0) && (exp.num == 0))
            {
                return NaN;
            }

            double nr = Math.Pow(this.num, exp.num);
            if (nr > int.MaxValue)
            {
                return NaN;
            }

            double dr = Math.Pow(this.denom, exp.num);
            if (dr > int.MaxValue)
            {
                return NaN;
            }

            return new Number((int)nr, (int)dr);
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
