// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4
{
    using System;
    using System.Diagnostics;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            ExpressionSearch search = InitSearch();
            Results results = new Results();
            search.Run(e => OnFound(results, e));

            stopwatch.Stop();

            Console.WriteLine("Found {0} results in {1} ms.", results.Count, stopwatch.ElapsedMilliseconds);

            foreach (Expression result in results)
            {
                Console.WriteLine("{0} = {1}", result.Result, Postfix.ToInfix(result.ToString()));
            }
        }

        private static ExpressionSearch InitSearch()
        {
            ExpressionSearch search = new ExpressionSearch();
            search.AddOperator("+");
            search.AddOperator("-");
            search.AddOperator("*");
            search.AddOperator("/");
            search.AddOperator("^");
            search.AddOperator("!");
            search.AddOperator("R");
            search.AddOperand("44");
            search.AddOperand("4");
            search.AddOperand(Number.PointFour);
            search.AddOperand(Number.PointFourRepeating);
            return search;
        }

        private static bool OnFound(Results results, Expression expr)
        {
            results.Add(expr);
            return results.Count < 100;
        }
    }
}
