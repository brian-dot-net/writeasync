// <copyright file="Results.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4
{
    using System.Collections.Generic;

    public sealed class Results
    {
        private readonly List<Expression> results;

        public Results()
        {
            this.results = new List<Expression>();
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

            this.results.Add(expr);
        }
    }
}
