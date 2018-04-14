// <copyright file="UnaryExpression.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr
{
    internal abstract class UnaryExpression : BasicExpression
    {
        private readonly string name;
        private readonly BasicExpression x;

        protected UnaryExpression(string name, BasicExpression x)
        {
            this.name = name;
            this.x = x;
        }

        public override string ToString() => this.name + "(" + this.x + ")";
    }
}
