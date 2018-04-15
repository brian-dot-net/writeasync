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
            from k in Kw.RemE
            select new RemarkStatement(string.Empty);

        private static readonly Parser<BasicStatement> RemNonEmpty =
            from k in Kw.Rem
            from s in Parse.AnyChar.AtLeastOnce().Text()
            select new RemarkStatement(s);

        private static readonly Parser<BasicStatement> Rem = RemNonEmpty.Or(RemEmpty);

        private static readonly Parser<BasicStatement> Cls =
            from k in Kw.Cls
            select new ClearScreenStatement();

        private static readonly Parser<IEnumerable<BasicExpression>> Arrays =
            from head in Expr.AnyArray.Once()
            from rest in Ch.Comma.Then(_ => Expr.AnyArray).Many()
            select head.Concat(rest);

        private static readonly Parser<BasicStatement> Dim =
            from k in Kw.Dim
            from list in Arrays
            select new DimensionStatement(list);

        private static readonly Parser<BasicStatement> PrintEmpty =
            from k in Kw.PrintE
            select new PrintStatement(Enumerable.Empty<BasicExpression>());

        private static readonly Parser<IEnumerable<BasicExpression>> PrintList =
            from k in Kw.Print
            from head in Expr.Any.Once()
            from rest in Ch.Semicolon.Then(_ => Expr.Any).Many()
            select head.Concat(rest);

        private static readonly Parser<BasicStatement> PrintMany =
            from list in PrintList
            select new PrintStatement(list);

        private static readonly Parser<BasicStatement> PrintNMany =
            from list in PrintList
            from o in Ch.Semicolon
            select new PrintStatement(list, false);

        private static readonly Parser<BasicStatement> Print =
            PrintNMany.Or(PrintMany).Or(PrintEmpty);

        private static readonly Parser<BasicStatement> InputP =
            from k in Kw.Input
            from lq in Ch.Quote
            from p in Ch.NonQuote.Many().Text()
            from rq in Ch.Quote
            from s in Ch.Semicolon
            from v in Expr.AnyVar
            select new InputStatement(p, v);

        private static readonly Parser<BasicStatement> InputV =
            from k in Kw.Input
            from v in Expr.AnyVar
            select new InputStatement(string.Empty, v);

        private static readonly Parser<BasicStatement> Input = InputP.Or(InputV);

        private static readonly Parser<BasicStatement> Gosub =
            from k in Kw.Gosub
            from n in Parse.Number
            select new GosubStatement(int.Parse(n));

        private static readonly Parser<BasicStatement> Assign =
            from left in Expr.AnyVar
            from o in Ch.Equal
            from right in Expr.Any
            select new AssignmentStatement(left, right);

        private static readonly Parser<BasicStatement> Any =
            Rem.Or(Cls).Or(Dim).Or(Print).Or(Input).Or(Gosub).Or(Assign);

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

        private static class Ch
        {
            public static readonly Parser<char> Equal = Parse.Char('=').Token();
            public static readonly Parser<char> Comma = Parse.Char(',').Token();
            public static readonly Parser<char> Semicolon = Parse.Char(';').Token();
            public static readonly Parser<char> Quote = Parse.Char('"');
            public static readonly Parser<char> NonQuote = Parse.AnyChar.Except(Quote);
        }

        private static class Kw
        {
            public static readonly Parser<IEnumerable<char>> Cls = Parse.IgnoreCase("CLS");
            public static readonly Parser<IEnumerable<char>> Dim = Parse.IgnoreCase("DIM").Token();
            public static readonly Parser<IEnumerable<char>> Gosub = Parse.IgnoreCase("GOSUB ").Token();
            public static readonly Parser<IEnumerable<char>> Input = Parse.IgnoreCase("INPUT ").Token();
            public static readonly Parser<IEnumerable<char>> Print = Parse.IgnoreCase("PRINT ").Token();
            public static readonly Parser<IEnumerable<char>> PrintE = Parse.IgnoreCase("PRINT");
            public static readonly Parser<IEnumerable<char>> Rem = Parse.IgnoreCase("REM ");
            public static readonly Parser<IEnumerable<char>> RemE = Parse.IgnoreCase("REM");
        }
    }
}