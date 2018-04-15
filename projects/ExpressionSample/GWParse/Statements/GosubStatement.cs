// <copyright file="GosubStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    internal sealed class GosubStatement : BasicStatement
    {
        private readonly int dest;

        public GosubStatement(int dest)
        {
            this.dest = dest;
        }

        public override string ToString() => "Gosub(" + this.dest + ")";
    }
}