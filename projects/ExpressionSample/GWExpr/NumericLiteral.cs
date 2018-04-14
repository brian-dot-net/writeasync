// <copyright file="NumericLiteral.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr
{
    internal sealed class NumericLiteral : BasicExpression
    {
        private readonly int n;

        public NumericLiteral(int n)
        {
            this.n = n;
        }

        public override string ToString() => "L(" + this.n + ")";
    }
}