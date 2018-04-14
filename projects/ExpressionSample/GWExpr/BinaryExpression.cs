// <copyright file="BinaryExpression.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr
{
    internal abstract class BinaryExpression : BasicExpression
    {
        private readonly string name;
        private readonly BasicExpression x;
        private readonly BasicExpression y;

        protected BinaryExpression(string name, BasicExpression x, BasicExpression y)
        {
            this.name = name;
            this.x = x;
            this.y = y;
        }

        public override string ToString() => this.name + "(" + this.x + ", " + this.y + ")";
    }
}
