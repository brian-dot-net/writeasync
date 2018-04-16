// <copyright file="Line.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Lines
{
    using System;
    using GWParse.Statements;
    using Sprache;

    internal static class Line
    {
        private static readonly Parser<BasicLine> Any =
            from n in Parse.Number
            from s in Parse.Char(' ').AtLeastOnce()
            from stmt in Stmt.Any
            select new BasicLine(int.Parse(n), stmt);

        public static BasicLine FromString(string input)
        {
            try
            {
                return Any.Token().End().Parse(input);
            }
            catch (ParseException e)
            {
                throw new FormatException("Bad statement '" + input + "'.", e);
            }
        }
    }
}
