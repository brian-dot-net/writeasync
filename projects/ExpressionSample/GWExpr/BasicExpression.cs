// <copyright file="BasicExpression.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            var dollar = Parse.Char('$');
            var leftParen = Parse.Char('(');
            var rightParen = Parse.Char(')');
            var comma = Parse.Char(',');
            var nonQuote = Parse.AnyChar.Except(quote);
            var stringLiteral =
                from lq in quote
                from c in nonQuote.Many().Text()
                from rq in quote
                select Str(c);

            var id = Parse.Identifier(Parse.Letter, Parse.LetterOrDigit);
            var stringVar =
                from v in id
                from d in dollar
                select StrVar(v);
            var numericVar =
                from v in id
                select NumVar(v);
            var scalar =
                stringVar
                .Or(numericVar);

            var numeric = numericLiteral.Or(numericVar);

            var indexList =
                from head in numeric.Once()
                from rest in comma.Then(_ => numeric).Many()
                select head.Concat(rest);
            var array =
                from v in scalar
                from lp in leftParen
                from i in indexList
                from rp in rightParen
                select Arr(v, i);

            var expr =
                numericLiteral
                .Or(stringLiteral)
                .Or(array)
                .Or(scalar)
                .End();

            try
            {
                return expr.Parse(input);
            }
            catch (ParseException e)
            {
                throw new FormatException("Bad expression '" + input + "'.", e);
            }
        }

        private static BasicExpression Num(int n) => new NumericLiteral(n);

        private static BasicExpression NumVar(string v) => new NumericVariable(v);

        private static BasicExpression Str(string s) => new StringLiteral(s);

        private static BasicExpression StrVar(string v) => new StringVariable(v);

        private static BasicExpression Arr(BasicExpression v, IEnumerable<BasicExpression> i) => new ArrayVariable(v, i);

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

        private abstract class BasicVariable : BasicExpression
        {
            private readonly string v;

            protected BasicVariable(string v)
            {
                this.v = v.ToUpperInvariant();
            }

            public override string ToString() => "Variable(" + this.v + ")";
        }

        private sealed class NumericVariable : BasicVariable
        {
            public NumericVariable(string v)
                : base(v)
            {
            }

            public override string ToString() => "Numeric" + base.ToString();
        }

        private sealed class StringVariable : BasicVariable
        {
            public StringVariable(string v)
                : base(v)
            {
            }

            public override string ToString() => "String" + base.ToString();
        }

        private sealed class ArrayVariable : BasicExpression
        {
            private readonly BasicExpression v;
            private readonly BasicExpression[] i;

            public ArrayVariable(BasicExpression v, IEnumerable<BasicExpression> i)
            {
                this.v = v;
                this.i = i.ToArray();
            }

            public override string ToString()
            {
                var list = string.Join<BasicExpression>(", ", this.i);
                return "ArrayVariable(" + this.v + ", " + list + ")";
            }
        }
    }
}
