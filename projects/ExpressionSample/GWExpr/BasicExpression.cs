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
            try
            {
                return Expr.Any.Parse(input);
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
            public static readonly Parser<char> Minus = Parse.Char('-');
            public static readonly Parser<char> Star = Parse.Char('*');
            public static readonly Parser<char> Slash = Parse.Char('/');
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

        private static class Var
        {
            public static readonly Parser<string> Id = Parse.Identifier(Parse.Letter, Parse.LetterOrDigit);

            public static readonly Parser<BasicVariable> StrScalar =
                from v in Id
                from d in Ch.Dollar
                select Ex.StrVar(v);

            public static readonly Parser<BasicVariable> NumScalar =
                from v in Id
                select Ex.NumVar(v);

            public static readonly Parser<BasicExpression> Index = Lit.Num.Or(Parse.Ref(() => NumAny));

            public static readonly Parser<IEnumerable<BasicExpression>> IndexList =
                from lp in Ch.LeftParen
                from head in Index.Once()
                from rest in Ch.Comma.Then(_ => Index).Many()
                from rp in Ch.RightParen
                select head.Concat(rest);

            public static readonly Parser<BasicExpression> NumArray =
                from v in NumScalar
                from i in IndexList
                select Ex.Arr(v, i);

            public static readonly Parser<BasicExpression> StrArray =
                from v in StrScalar
                from i in IndexList
                select Ex.Arr(v, i);

            public static readonly Parser<BasicExpression> NumAny = NumArray.Or(NumScalar);

            public static readonly Parser<BasicExpression> StrAny = StrArray.Or(StrScalar);
        }

        private static class Expr
        {
            public static readonly Parser<BasicExpression> Any =
                Parse.Ref(() => Str)
                .Or(Parse.Ref(() => Num))
                .End();

            private static readonly Parser<BasicExpression> StrParen =
                from lp in Ch.LeftParen
                from x in Parse.Ref(() => Str)
                from rp in Ch.RightParen
                select x;

            private static readonly Parser<BasicExpression> StrTerm =
                StrParen
                .Or(Lit.Str)
                .Or(Var.StrAny);

            private static readonly Parser<BasicExpression> Concat =
                from head in StrTerm.Once()
                from rest in Ch.Plus.Then(_ => StrTerm).AtLeastOnce()
                select Ex.Add(head.Concat(rest));

            private static readonly Parser<BasicExpression> Str =
                Concat
                .Or(StrTerm);

            private static readonly Parser<BasicExpression> NumParen =
                from lp in Ch.LeftParen
                from x in Parse.Ref(() => Num)
                from rp in Ch.RightParen
                select x;

            private static readonly Parser<BasicExpression> NumTerm =
                NumParen
                .Or(Lit.Num)
                .Or(Var.NumAny);

            private static readonly Parser<BasicExpression> Multiply =
                from head in NumTerm.Once()
                from rest in Ch.Star.Then(_ => NumTerm).AtLeastOnce()
                select Ex.Multiply(head.Concat(rest));

            private static readonly Parser<BasicExpression> Divide =
                from head in NumTerm.Once()
                from rest in Ch.Slash.Then(_ => NumTerm).AtLeastOnce()
                select Ex.Divide(head.Concat(rest));

            private static readonly Parser<BasicExpression> Add =
                from head in NumTerm.Once()
                from rest in Ch.Plus.Then(_ => NumTerm).AtLeastOnce()
                select Ex.Add(head.Concat(rest));

            private static readonly Parser<BasicExpression> Subtract =
                from head in NumTerm.Once()
                from rest in Ch.Minus.Then(_ => NumTerm).AtLeastOnce()
                select Ex.Subtract(head.Concat(rest));

            private static readonly Parser<BasicExpression> Num =
                Multiply
                .Or(Divide)
                .Or(Add)
                .Or(Subtract)
                .Or(NumTerm);
        }

        private static class Ex
        {
            public static BasicExpression Num(int n) => new NumericLiteral(n);

            public static BasicVariable NumVar(string v) => new NumericVariable(v);

            public static BasicExpression Str(string s) => new StringLiteral(s);

            public static BasicVariable StrVar(string v) => new StringVariable(v);

            public static BasicExpression Arr(BasicVariable v, IEnumerable<BasicExpression> i) => new BasicArray(v, i);

            public static BasicExpression Add(IEnumerable<BasicExpression> xs)
            {
                return xs.Aggregate((x, y) => new AddExpression(x, y));
            }

            public static BasicExpression Subtract(IEnumerable<BasicExpression> xs)
            {
                return xs.Aggregate((x, y) => new SubtractExpression(x, y));
            }

            public static BasicExpression Multiply(IEnumerable<BasicExpression> xs)
            {
                return xs.Aggregate((x, y) => new MultiplyExpression(x, y));
            }

            public static BasicExpression Divide(IEnumerable<BasicExpression> xs)
            {
                return xs.Aggregate((x, y) => new DivideExpression(x, y));
            }

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

            private abstract class BinaryExpression : BasicExpression
            {
                private readonly BasicExpression x;
                private readonly BasicExpression y;

                protected BinaryExpression(BasicExpression x, BasicExpression y)
                {
                    this.x = x;
                    this.y = y;
                }

                public override string ToString() => "(" + this.x + ", " + this.y + ")";
            }

            private sealed class AddExpression : BinaryExpression
            {
                public AddExpression(BasicExpression x, BasicExpression y)
                    : base(x, y)
                {
                }

                public override string ToString() => "Add" + base.ToString();
            }

            private sealed class SubtractExpression : BinaryExpression
            {
                public SubtractExpression(BasicExpression x, BasicExpression y)
                    : base(x, y)
                {
                }

                public override string ToString() => "Subtract" + base.ToString();
            }

            private sealed class MultiplyExpression : BinaryExpression
            {
                public MultiplyExpression(BasicExpression x, BasicExpression y)
                    : base(x, y)
                {
                }

                public override string ToString() => "Multiply" + base.ToString();
            }

            private sealed class DivideExpression : BinaryExpression
            {
                public DivideExpression(BasicExpression x, BasicExpression y)
                    : base(x, y)
                {
                }

                public override string ToString() => "Divide" + base.ToString();
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
