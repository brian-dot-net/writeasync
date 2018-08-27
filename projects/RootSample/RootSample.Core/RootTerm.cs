// <copyright file="RootTerm.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace RootSample
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public struct RootTerm
    {
        private static readonly List<ushort> Primes = new List<ushort>() { 2, 3, 5 };

        private readonly int c;
        private readonly int x;

        public RootTerm(int n)
        {
            if (n == 0)
            {
                this.c = 0;
                this.x = 0;
                return;
            }

            int s = (int)Math.Sqrt(n);
            if (s * s == n)
            {
                this.x = 1;
                this.c = s;
                return;
            }

            this.c = 1;
            foreach (ushort p in Primes)
            {
                int r;
                while (true)
                {
                    int m = Math.DivRem(n, p * p, out r);
                    if (r != 0)
                    {
                        break;
                    }

                    n = m;
                    this.c *= p;
                }
            }

            this.x = n;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (this.c != 1)
            {
                sb.Append(this.c);
            }

            if (this.x > 1)
            {
                sb.Append('*');
                sb.Append("sqrt(");
                sb.Append(this.x);
                sb.Append(')');
            }

            if (sb.Length == 0)
            {
                return "1";
            }

            return sb.ToString();
        }
    }
}
