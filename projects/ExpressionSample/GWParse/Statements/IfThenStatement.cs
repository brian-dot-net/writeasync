// <copyright file="IfThenStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    using GWParse.Expressions;

    internal sealed class IfThenStatement : BasicStatement
    {
        private readonly BasicExpression cond;
        private readonly int dest;

        public IfThenStatement(BasicExpression cond, int dest)
        {
            this.cond = cond;
            this.dest = dest;
        }

        public override string ToString() => "If(" + this.cond + ", Goto(" + this.dest + "))";
    }
}