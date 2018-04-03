// <copyright file="BasicExpression.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr
{
    public sealed class BasicExpression
    {
        private readonly string input;

        private BasicExpression(string input)
        {
            this.input = input;
        }

        public static BasicExpression Parse(string input) => new BasicExpression(input);

        public override string ToString() => "NumericLiteral(" + this.input + ")";
    }
}
