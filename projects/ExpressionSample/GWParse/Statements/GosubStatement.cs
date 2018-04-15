// <copyright file="GosubStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    internal sealed class GosubStatement : BasicStatement
    {
        private readonly int line;

        public GosubStatement(int line)
        {
            this.line = line;
        }

        public override string ToString() => "Gosub(" + this.line + ")";
    }
}