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
        public static readonly Parser<BasicStatement> Any = Parse.Ref(() => Root);

        private static readonly BasicExpression One = BasicExpression.FromString("1");

        private static readonly Parser<int> LineNum =
            from n in Parse.Number.Token()
            select int.Parse(n);

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

        private static readonly Parser<BasicStatement> Return =
            from k in Kw.Return
            select new ReturnStatement();

        private static readonly Parser<BasicStatement> NextEmpty =
            from k in Kw.NextE
            select new NextStatement(Enumerable.Empty<BasicExpression>());

        private static readonly Parser<IEnumerable<BasicExpression>> NumScalars =
            from head in Expr.AnyNumScalar.Once()
            from rest in Ch.Comma.Then(_ => Expr.AnyNumScalar).Many()
            select head.Concat(rest);

        private static readonly Parser<BasicStatement> NextNonEmpty =
            from k in Kw.Next
            from vars in NumScalars
            select new NextStatement(vars);

        private static readonly Parser<BasicStatement> Next = NextNonEmpty.Or(NextEmpty);

        private static readonly Parser<BasicExpression> DataNum =
            from n in Parse.Number
            select BasicExpression.FromString(n);

        private static readonly Parser<string> DataQuoted =
            from lq in Ch.Quote
            from s in Ch.NonQuote.Many().Text()
            from rq in Ch.Quote
            select s;

        private static readonly Parser<string> DataBare =
            from s in Parse.CharExcept("\",:").AtLeastOnce().Token().Text()
            select s.Trim();

        private static readonly Parser<BasicExpression> DataStr =
            from s in DataQuoted.Or(DataBare)
            select BasicExpression.FromString("\"" + s + "\"");

        private static readonly Parser<BasicExpression> DataItem = DataNum.Or(DataStr);

        private static readonly Parser<IEnumerable<BasicExpression>> DataItems =
            from head in DataItem.Once()
            from rest in Ch.Comma.Then(_ => DataItem).Many()
            select head.Concat(rest);

        private static readonly Parser<BasicStatement> Data =
            from k in Kw.Data
            from list in DataItems
            select new DataStatement(list);

        private static readonly Parser<IEnumerable<BasicExpression>> Arrays =
            from head in Expr.AnyArray.Once()
            from rest in Ch.Comma.Then(_ => Expr.AnyArray).Many()
            select head.Concat(rest);

        private static readonly Parser<BasicStatement> Dim =
            from k in Kw.Dim
            from list in Arrays
            select new DimensionStatement(list);

        private static readonly Parser<IEnumerable<BasicExpression>> Vars =
            from head in Expr.AnyVar.Once()
            from rest in Ch.Comma.Then(_ => Expr.AnyVar).Many()
            select head.Concat(rest);

        private static readonly Parser<BasicStatement> Read =
            from k in Kw.Read
            from list in Vars
            select new ReadStatement(list);

        private static readonly Parser<BasicExpression> IfThenCond =
            from k1 in Kw.If
            from cond in Expr.Any
            from k2 in Kw.Then
            select cond;

        private static readonly Parser<BasicStatement> IfThenGoto =
            from cond in IfThenCond
            from n in LineNum
            select new IfThenStatement(cond, new GotoStatement(n));

        private static readonly Parser<BasicStatement> IfThenNonGoto =
            from cond in IfThenCond
            from ifT in Parse.Ref(() => Any)
            select new IfThenStatement(cond, ifT);

        private static readonly Parser<BasicStatement> IfThen = IfThenNonGoto.Or(IfThenGoto);

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
            from n in LineNum
            select new GosubStatement(n);

        private static readonly Parser<BasicStatement> Goto =
            from k in Kw.Goto
            from n in LineNum
            select new GotoStatement(n);

        private static readonly Parser<Tuple<BasicExpression, BasicExpression, BasicExpression>> ForPrefix =
            from k1 in Kw.For
            from v in Expr.AnyNumScalar
            from eq in Ch.Equal.Token()
            from a in Expr.Any
            from k2 in Kw.To
            from b in Expr.Any
            select Tuple.Create(v, a, b);

        private static readonly Parser<BasicStatement> ForWithoutStep =
            from t in ForPrefix
            select new ForStatement(t.Item1, t.Item2, t.Item3, One);

        private static readonly Parser<BasicStatement> ForWithStep =
            from t in ForPrefix
            from k in Kw.Step
            from s in Expr.Any
            select new ForStatement(t.Item1, t.Item2, t.Item3, s);

        private static readonly Parser<BasicStatement> For = ForWithStep.Or(ForWithoutStep);

        private static readonly Parser<BasicStatement> Assign =
            from left in Expr.AnyVar
            from o in Ch.Equal
            from right in Expr.Any
            select new AssignmentStatement(left, right);

        private static readonly Parser<BasicStatement> Root =
            Rem
            .Or(Cls)
            .Or(Data)
            .Or(Dim)
            .Or(For)
            .Or(Gosub)
            .Or(Goto)
            .Or(IfThen)
            .Or(Input)
            .Or(Next)
            .Or(Print)
            .Or(Read)
            .Or(Return)
            .Or(Assign);

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
            public static readonly Parser<IEnumerable<char>> Data = Parse.IgnoreCase("DATA ").Token();
            public static readonly Parser<IEnumerable<char>> Dim = Parse.IgnoreCase("DIM ").Token();
            public static readonly Parser<IEnumerable<char>> For = Parse.IgnoreCase("FOR ").Token();
            public static readonly Parser<IEnumerable<char>> Gosub = Parse.IgnoreCase("GOSUB ").Token();
            public static readonly Parser<IEnumerable<char>> Goto = Parse.IgnoreCase("GOTO ").Token();
            public static readonly Parser<IEnumerable<char>> If = Parse.IgnoreCase("IF ").Token();
            public static readonly Parser<IEnumerable<char>> Input = Parse.IgnoreCase("INPUT ").Token();
            public static readonly Parser<IEnumerable<char>> Next = Parse.IgnoreCase("NEXT ").Token();
            public static readonly Parser<IEnumerable<char>> NextE = Parse.IgnoreCase("NEXT");
            public static readonly Parser<IEnumerable<char>> Print = Parse.IgnoreCase("PRINT ").Token();
            public static readonly Parser<IEnumerable<char>> PrintE = Parse.IgnoreCase("PRINT");
            public static readonly Parser<IEnumerable<char>> Read = Parse.IgnoreCase("READ ").Token();
            public static readonly Parser<IEnumerable<char>> Rem = Parse.IgnoreCase("REM ");
            public static readonly Parser<IEnumerable<char>> RemE = Parse.IgnoreCase("REM");
            public static readonly Parser<IEnumerable<char>> Return = Parse.IgnoreCase("RETURN");
            public static readonly Parser<IEnumerable<char>> Step = Parse.IgnoreCase("STEP ").Token();
            public static readonly Parser<IEnumerable<char>> Then = Parse.IgnoreCase("THEN ").Token();
            public static readonly Parser<IEnumerable<char>> To = Parse.IgnoreCase("TO ").Token();
        }
    }
}