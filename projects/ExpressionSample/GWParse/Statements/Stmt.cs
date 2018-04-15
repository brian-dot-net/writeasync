// <copyright file="Stmt.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GWParse.Expressions;
    using Sprache;

    internal static class Stmt
    {
        private static readonly Parser<BasicStatement> RemEmpty =
            from k in Parse.IgnoreCase("REM")
            select new RemarkStatement(string.Empty);

        private static readonly Parser<BasicStatement> RemNonEmpty =
            from k in Parse.IgnoreCase("REM ")
            from s in Parse.AnyChar.AtLeastOnce().Text()
            select new RemarkStatement(s);

        private static readonly Parser<BasicStatement> Rem = RemNonEmpty.Or(RemEmpty);

        private static readonly Parser<BasicStatement> Cls =
            from k in Parse.IgnoreCase("CLS")
            select new ClearScreenStatement();

        private static readonly Parser<IEnumerable<BasicExpression>> Arrays =
            from head in Expr.AnyArray.Once()
            from rest in Parse.Char(',').Token().Then(_ => Expr.AnyArray).Many()
            select head.Concat(rest);

        private static readonly Parser<BasicStatement> Dim =
            from k in Parse.IgnoreCase("DIM").Token()
            from list in Arrays
            select new DimensionStatement(list);

        private static readonly Parser<BasicStatement> PrintEmpty =
            from k in Parse.IgnoreCase("PRINT")
            select new PrintStatement(Enumerable.Empty<BasicExpression>());

        private static readonly Parser<IEnumerable<BasicExpression>> PrintList =
            from head in Expr.Any.Once()
            from rest in Parse.Char(';').Token().Then(_ => Expr.Any).Many()
            select head.Concat(rest);

        private static readonly Parser<BasicStatement> PrintMany =
            from k in Parse.IgnoreCase("PRINT").Token()
            from list in PrintList
            select new PrintStatement(list);

        private static readonly Parser<BasicStatement> PrintNMany =
            from k in Parse.IgnoreCase("PRINT").Token()
            from list in PrintList
            from o in Parse.Char(';').Token()
            select new PrintStatement(list, false);

        private static readonly Parser<BasicStatement> Print =
            PrintNMany.Or(PrintMany).Or(PrintEmpty);

        private static readonly Parser<BasicStatement> Gosub =
            from k in Parse.IgnoreCase("GOSUB").Token()
            from n in Parse.Number
            select new GosubStatement(int.Parse(n));

        private static readonly Parser<BasicStatement> Assign =
            from left in Expr.AnyVar
            from o in Parse.Char('=').Token()
            from right in Expr.Any
            select new AssignmentStatement(left, right);

        private static readonly Parser<BasicStatement> Any =
            Rem.Or(Cls).Or(Dim).Or(Print).Or(Gosub).Or(Assign);

        public static BasicStatement FromString(string input)
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