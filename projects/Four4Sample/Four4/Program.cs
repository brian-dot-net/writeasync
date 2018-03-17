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

            int d = GetDigit(args);

            ExpressionSearch search = InitSearch(d);
            Results results = new Results();
            search.Run(e => OnFound(results, e));

            stopwatch.Stop();

            Console.WriteLine("Found {0} results in {1} ms.", results.Count, stopwatch.ElapsedMilliseconds);

            foreach (Expression result in results)
            {
                Console.WriteLine("{0} = {1}", result.Result, Postfix.ToInfix(result.ToString()));
            }
        }

        private static int GetDigit(string[] args)
        {
            int d = 4;
            if ((args.Length != 0) && (args[0].Length > 0))
            {
                char c = args[0][0];
                switch (c)
                {
                    case '3':
                    case '4':
                        d = c - '0';
                        break;
                }
            }

            return d;
        }

        private static ExpressionSearch InitSearch(int n)
        {
            string d = n.ToString();
            ExpressionSearch search = new ExpressionSearch() { NumeralCount = n };
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

        private static bool OnFound(Results results, Expression expr)
        {
            results.Add(expr);
            return results.Count < 100;
        }
    }
}
