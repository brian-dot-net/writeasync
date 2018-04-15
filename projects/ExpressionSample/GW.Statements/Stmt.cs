// <copyright file="Stmt.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GW.Statements
{
    using Sprache;

    internal static class Stmt
    {
        private static readonly Parser<BasicStatement> RemEmpty =
            from k in Parse.String("REM")
            select new RemarkStatement(string.Empty);

        private static readonly Parser<BasicStatement> RemNonEmpty =
            from k in Parse.String("REM ")
            from s in Parse.AnyChar.AtLeastOnce().Text()
            select new RemarkStatement(s);

        private static readonly Parser<BasicStatement> Rem = RemNonEmpty.Or(RemEmpty);
        private static readonly Parser<BasicStatement> Any = Rem;

        public static BasicStatement FromString(string input)
        {
            return Any.Parse(input);
        }
    }
}