// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal sealed class Program
    {
        private readonly ExpressionSearch search;
        private readonly SortedDictionary<int, string> results;

        public Program()
        {
            this.search = InitSearch();
            this.results = new SortedDictionary<int, string>();
        }

        public void Run()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            this.search.Run(this.OnFound);
            stopwatch.Stop();

            Console.WriteLine("Found {0} results in {1} ms.", this.results.Count, stopwatch.ElapsedMilliseconds);

            foreach (KeyValuePair<int, string> result in this.results)
            {
                Console.WriteLine("{0} = {1}", result.Key, result.Value);
            }
        }

        private static void Main(string[] args)
        {
            new Program().Run();
        }

        private static ExpressionSearch InitSearch()
        {
            ExpressionSearch search = new ExpressionSearch();
            search.AddBinary("+");
            search.AddBinary("-");
            search.AddBinary("*");
            search.AddBinary("/");
            search.AddUnary("!");
            search.AddUnary("R");
            search.AddOperand("4");
            search.AddOperand("44");
            search.AddOperand("444");
            search.AddOperand("4444");
            search.AddOperand(Number.PointFour);
            search.AddOperand(Number.PointFourRepeating);
            return search;
        }

        private bool OnFound(Expression expr)
        {
            if (expr.NumeralCount != 4)
            {
                return true;
            }

            Number num = expr.Result;
            if (!num.IsWhole)
            {
                return true;
            }

            int n = (int)num;
            if (n > 100)
            {
                return true;
            }

            if (this.results.ContainsKey(n))
            {
                return true;
            }

            this.results.Add(n, expr.ToString());
            return this.results.Count < 100;
        }
    }
}
