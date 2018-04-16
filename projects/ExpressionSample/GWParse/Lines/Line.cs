// <copyright file="Line.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Lines
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GWParse.Statements;
    using Sprache;

    internal static class Line
    {
        private static readonly Parser<IEnumerable<BasicStatement>> StmtList =
            from head in Stmt.Any.Once()
            from rest in Parse.Char(':').Token().Then(_ => Stmt.Any).Many()
            select head.Concat(rest);

        private static readonly Parser<BasicLine> Any =
            from n in Parse.Number
            from s in Parse.Char(' ').AtLeastOnce()
            from list in StmtList
            select new BasicLine(int.Parse(n), list);

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
