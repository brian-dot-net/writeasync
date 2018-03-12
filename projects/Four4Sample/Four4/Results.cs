// <copyright file="Results.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4
{
    using System.Collections.Generic;

    public sealed class Results
    {
        private readonly Dictionary<int, Expression> results;

        public Results()
        {
            this.results = new Dictionary<int, Expression>();
        }

        public int Count => this.results.Count;

        public void Add(Expression expr)
        {
            if (expr.NumeralCount != 4)
            {
                return;
            }

            Number num = expr.Result;
            if (!num.IsWhole)
            {
                return;
            }

            int n = (int)num;
            if (n > 100)
            {
                return;
            }

            if (!this.results.ContainsKey(n))
            {
                this.results.Add(n, expr);
            }
        }
    }
}
