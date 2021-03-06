﻿// <copyright file="Results.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4
{
    using System.Collections;
    using System.Collections.Generic;

    public sealed class Results : IEnumerable<Expression>
    {
        private readonly IDictionary<int, Expression> results;

        public Results()
        {
            this.results = new SortedDictionary<int, Expression>();
            this.NumeralCount = 4;
            this.Min = 1;
            this.Max = 100;
        }

        public int Count => this.results.Count;

        public int NumeralCount { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }

        public void Add(Expression expr)
        {
            if (expr.NumeralCount != this.NumeralCount)
            {
                return;
            }

            Number num = expr.Result;
            if (!num.IsInteger)
            {
                return;
            }

            int n = (int)num;
            if ((n < this.Min) || (n > this.Max))
            {
                return;
            }

            if (!this.results.ContainsKey(n))
            {
                this.results.Add(n, expr);
            }
        }

        public IEnumerator<Expression> GetEnumerator() => this.results.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
