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
        private readonly BasicExpression step;

        public ForStatement(BasicExpression v, BasicExpression start, BasicExpression end, BasicExpression step)
        {
            this.v = v;
            this.start = start;
            this.end = end;
            this.step = step;
        }

        public override string ToString()
        {
            return "For(" + this.v + ", " + this.start + ", " + this.end + ", " + this.step + ")";
        }
    }
}