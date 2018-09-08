// <copyright file="RootSum.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace RootSample
{
    using System.Text;

    public struct RootSum
    {
        private readonly RootTerm a;
        private readonly RootTerm b;

        private RootSum(RootTerm a, RootTerm b)
        {
            this.a = a;
            this.b = b;
        }

        public static RootSum Add(RootTerm x, RootTerm y)
        {
            if (x.IsZero)
            {
                return new RootSum(y, RootTerm.Zero);
            }

            if (y.IsZero)
            {
                return new RootSum(x, RootTerm.Zero);
            }

            if (x.X == y.X)
            {
                return new RootSum(new RootTerm(x.C + y.C, x.X), RootTerm.Zero);
            }

            if (x.X > y.X)
            {
                return new RootSum(x, y);
            }

            return new RootSum(y, x);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.a.ToString());
            if (!this.b.IsZero)
            {
                sb.Append('+');
                sb.Append(this.b.ToString());
            }

            return sb.ToString();
        }
    }
}
