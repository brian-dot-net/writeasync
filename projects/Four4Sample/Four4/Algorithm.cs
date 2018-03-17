// <copyright file="Algorithm.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4
{
    using System.Diagnostics;
    using System.IO;

    public sealed class Algorithm
    {
        private readonly int digit;
        private readonly int min;
        private readonly int max;
        private readonly ExpressionSearch search;

        public Algorithm(params string[] args)
        {
            this.digit = GetDigit(args);
            this.min = GetMin(args);
            this.max = GetMax(args);
            this.search = this.InitSearch();
        }

        public void Run(TextWriter writer)
        {
            writer.WriteLine("Solving {0} {0}s (min={1}, max={2})...", this.digit, this.min, this.max);
            Stopwatch stopwatch = Stopwatch.StartNew();

            Results results = new Results { NumeralCount = this.digit, Min = this.min, Max = this.max };
            this.search.Run(e => this.OnFound(results, e));

            stopwatch.Stop();

            writer.WriteLine("Found {0} results in {1} ms.", results.Count, stopwatch.ElapsedMilliseconds);

            foreach (Expression result in results)
            {
                writer.WriteLine("{0} = {1}", result.Result, Postfix.ToInfix(result.ToString()));
            }
        }

        private static int GetDigit(string[] args)
        {
            int d = 4;
            if ((args.Length != 0) && (args[0].Length > 0))
            {
                char c = args[0][0];
                if (c > '0' && c <= '9')
                {
                    d = c - '0';
                }
            }

            return d;
        }

        private static int GetMin(string[] args)
        {
            int m = 1;
            if (args.Length > 1)
            {
                return int.Parse(args[1]);
            }

            return m;
        }

        private static int GetMax(string[] args)
        {
            int m = 100;
            if (args.Length > 2)
            {
                return int.Parse(args[2]);
            }

            return m;
        }

        private bool OnFound(Results results, Expression expr)
        {
            results.Add(expr);
            return results.Count < (this.max - this.min + 1);
        }

        private ExpressionSearch InitSearch()
        {
            string d = this.digit.ToString();
            ExpressionSearch search = new ExpressionSearch() { NumeralCount = this.digit };
            search.AddOperator("+");
            search.AddOperator("-");
            search.AddOperator("*");
            search.AddOperator("/");
            search.AddOperator("^");
            search.AddOperator("!");
            search.AddOperator("R");
            search.AddOperand(d + d);
            search.AddOperand(d);
            search.AddOperand("." + d);
            search.AddOperand("." + d + "_");
            return search;
        }
    }
}
