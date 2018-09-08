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

        public RootSum(RootTerm a, RootTerm b)
        {
            this.a = a;
            this.b = b;
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
