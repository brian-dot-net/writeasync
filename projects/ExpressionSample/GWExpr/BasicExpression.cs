// <copyright file="BasicExpression.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr
{
    using Sprache;

    public abstract class BasicExpression
    {
        protected BasicExpression()
        {
        }

        public static BasicExpression FromString(string input)
        {
            var numericLiteral =
                from n in Parse.Number
                select Num(int.Parse(n));
            var quote = Parse.Char('\"');
            var nonQuote = Parse.AnyChar.Except(quote);
            var stringLiteral =
                from lq in quote
                from c in nonQuote.Many().Text()
                from rq in quote
                select Str(c);

            var expr = numericLiteral.Or(stringLiteral);

            return expr.Parse(input);
        }

        private static BasicExpression Num(int n) => new NumericLiteral(n);

        private static BasicExpression Str(string s) => new StringLiteral(s);

        private sealed class NumericLiteral : BasicExpression
        {
            private readonly int n;

            public NumericLiteral(int n)
            {
                this.n = n;
            }

            public override string ToString() => "NumericLiteral(" + this.n + ")";
        }

        private sealed class StringLiteral : BasicExpression
        {
            private readonly string s;

            public StringLiteral(string s)
            {
                this.s = s;
            }

            public override string ToString() => "StringLiteral(\"" + this.s + "\")";
        }
    }
}
