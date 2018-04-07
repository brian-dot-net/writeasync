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
            var id = Parse.Identifier(Parse.Letter, Parse.LetterOrDigit);
            var stringScalarVar =
                from v in id
                from d in Ch.Dollar
                select Ex.StrVar(v);
            var numericScalarVar =
                from v in id
                select Ex.NumVar(v);
            var scalarVar =
                stringScalarVar
                .Or(numericScalarVar);

            var numeric = Lit.Num.Or(numericScalarVar);

            var indexList =
                from head in numeric.Once()
                from rest in Ch.Comma.Then(_ => numeric).Many()
                select head.Concat(rest);
            var array =
                from v in scalarVar
                from lp in Ch.LeftParen
                from i in indexList
                from rp in Ch.RightParen
                select Ex.Arr(v, i);

            var term =
                Lit.Any
                .Or(array)
                .Or(scalarVar);

            var add =
                from x in term
                from p in Ch.Plus
                from y in term
                select Ex.Add(x, y);

            var expr =
                add
                .Or(term)
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

        private static class Ch
        {
            public static readonly Parser<char> Quote = Parse.Char('\"');
            public static readonly Parser<char> Dollar = Parse.Char('$');
            public static readonly Parser<char> LeftParen = Parse.Char('(');
            public static readonly Parser<char> RightParen = Parse.Char(')');
            public static readonly Parser<char> Comma = Parse.Char(',');
            public static readonly Parser<char> Plus = Parse.Char('+');
            public static readonly Parser<char> NonQuote = Parse.AnyChar.Except(Quote);
        }

        private static class Lit
        {
            public static readonly Parser<BasicExpression> Num =
                from n in Parse.Number
                select Ex.Num(int.Parse(n));

            public static readonly Parser<BasicExpression> Str =
                from lq in Ch.Quote
                from c in Ch.NonQuote.Many().Text()
                from rq in Ch.Quote
                select Ex.Str(c);

            public static readonly Parser<BasicExpression> Any = Num.Or(Str);
        }

        private static class Ex
        {
            public static BasicExpression Num(int n) => new NumericLiteral(n);

            public static BasicVariable NumVar(string v) => new NumericVariable(v);

            public static BasicExpression Str(string s) => new StringLiteral(s);

            public static BasicVariable StrVar(string v) => new StringVariable(v);

            public static BasicExpression Arr(BasicVariable v, IEnumerable<BasicExpression> i) => new BasicArray(v, i);

            public static BasicExpression Add(BasicExpression x, BasicExpression y) => new AddExpression(x, y);

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

            private sealed class BasicArray : BasicExpression
            {
                private readonly BasicVariable v;
                private readonly BasicExpression[] i;

                public BasicArray(BasicVariable v, IEnumerable<BasicExpression> i)
                {
                    this.v = v;
                    this.i = i.ToArray();
                }

                public override string ToString()
                {
                    var list = string.Join<BasicExpression>(", ", this.i);
                    return "Array(" + this.v + ", " + list + ")";
                }
            }

            private sealed class AddExpression : BasicExpression
            {
                private readonly BasicExpression x;
                private readonly BasicExpression y;

                public AddExpression(BasicExpression x, BasicExpression y)
                {
                    this.x = x;
                    this.y = y;
                }

                public override string ToString() => "Add(" + this.x + ", " + this.y + ")";
            }
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
    }
}
