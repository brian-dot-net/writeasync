// <copyright file="OperatorExpression.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr
{
    using System.Linq;

    internal sealed class OperatorExpression : BasicExpression
    {
        private readonly string name;
        private readonly BasicExpression[] operands;

        public OperatorExpression(string name, BasicExpression x, params BasicExpression[] xs)
        {
            this.name = name;
            this.operands = new BasicExpression[] { x }.Concat(xs).ToArray();
        }

        public override string ToString()
        {
            return this.name + "(" + string.Join<BasicExpression>(", ", this.operands) + ")";
        }
    }
}
