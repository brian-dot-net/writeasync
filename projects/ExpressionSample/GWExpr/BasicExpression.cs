// <copyright file="BasicExpression.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr
{
    using Sprache;

    public sealed class BasicExpression
    {
        private readonly string input;

        private BasicExpression(string input)
        {
            this.input = input;
        }

        public static BasicExpression FromString(string input)
        {
            var expr =
                from n in Parse.Number
                select new BasicExpression(n);

            return expr.Parse(input);
        }

        public override string ToString() => "NumericLiteral(" + this.input + ")";
    }
}
