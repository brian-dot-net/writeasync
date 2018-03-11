﻿// <copyright file="Number.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4
{
    using System;

    public struct Number
    {
        public static readonly Number NaN = default(Number);

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
            if ((this.num < 0) || (this.denom != 1))
            {
                return NaN;
            }

            int n = this.num;
            int fact = 1;
            for (int i = 2; i <= n; ++i)
            {
                fact *= i;
            }

            return new Number(fact, 1);
        }

        public Number SquareRoot()
        {
            if (this.denom == 0)
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
