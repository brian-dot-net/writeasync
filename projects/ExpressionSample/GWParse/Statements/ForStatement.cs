// <copyright file="ForStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    using GWParse.Expressions;

    internal sealed class ForStatement : BasicStatement
    {
        private readonly BasicExpression v;
        private readonly BasicExpression start;
        private readonly BasicExpression end;

        public ForStatement(BasicExpression v, BasicExpression start, BasicExpression end)
        {
            this.v = v;
            this.start = start;
            this.end = end;
        }

        public override string ToString()
        {
            return "For(" + this.v + ", " + this.start + ", " + this.end + ")";
        }
    }
}