// <copyright file="BasicStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    public abstract class BasicStatement
    {
        protected BasicStatement()
        {
        }

        public static BasicStatement FromString(string input) => Stmt.FromString(input);
    }
}
