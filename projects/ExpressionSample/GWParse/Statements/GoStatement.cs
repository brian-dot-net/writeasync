// <copyright file="GoStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    internal sealed class GoStatement : BasicStatement
    {
        private readonly string name;
        private readonly int dest;

        public GoStatement(string name, int dest)
        {
            this.name = name;
            this.dest = dest;
        }

        public override void Accept(IStatementVisitor visit)
        {
            visit.Go(this.name, this.dest);
        }
    }
}