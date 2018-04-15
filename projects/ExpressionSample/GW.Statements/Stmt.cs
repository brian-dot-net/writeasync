// <copyright file="Stmt.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GW.Statements
{
    using System;
    using GW.Expressions;
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

        private static readonly Parser<BasicStatement> Assign =
            from left in Parse.CharExcept('=').AtLeastOnce().Text()
            from o in Parse.Char('=')
            from right in Parse.AnyChar.AtLeastOnce().Text()
            select Asgn(left, right);

        private static readonly Parser<BasicStatement> Any =
            Rem.Or(Cls).Or(Assign);

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

        private static BasicStatement Asgn(string left, string right)
        {
            try
            {
                return new AssignmentStatement(
                    BasicExpression.FromString(left),
                    BasicExpression.FromString(right));
            }
            catch (NotSupportedException e)
            {
                throw new ParseException(e.Message);
            }
        }
    }
}