// <copyright file="StringLiteral.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr
{
    internal sealed class StringLiteral : BasicExpression
    {
        private readonly string s;

        public StringLiteral(string s)
        {
            this.s = s;
        }

        public override string ToString() => "L(\"" + this.s + "\")";
    }
}
