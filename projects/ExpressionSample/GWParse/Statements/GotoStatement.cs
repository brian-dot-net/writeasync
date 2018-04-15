// <copyright file="GotoStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    internal sealed class GotoStatement : BasicStatement
    {
        private readonly int dest;

        public GotoStatement(int dest)
        {
            this.dest = dest;
        }

        public override string ToString() => "Goto(" + this.dest + ")";
    }
}