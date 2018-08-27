// <copyright file="RootTerm.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace RootSample
{
    using System;

    public struct RootTerm
    {
        private readonly int n;

        public RootTerm(int n)
        {
            this.n = (int)Math.Sqrt(n);
        }

        public override string ToString() => this.n.ToString();
    }
}
