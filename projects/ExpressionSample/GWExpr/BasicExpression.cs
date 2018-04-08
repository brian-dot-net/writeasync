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

        private interface IOperator
        {
            BasicExpression Apply(BasicExpression x, BasicExpression y);
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
            public static readonly Parser<char> Caret = Parse.Char('^');
            public static readonly Parser<char> NonQuote = Parse.AnyChar.Except(Quote);
        }

        private static class Lit
        {
            public static readonly Parser<BasicExpression> Num =
                from n in Parse.Number
                select new NumericLiteral(int.Parse(n));

            public static readonly Parser<BasicExpression> Str =
                from lq in Ch.Quote
                from s in Ch.NonQuote.Many().Text()
                from rq in Ch.Quote
                select new StringLiteral(s);

            public static readonly Parser<BasicExpression> Any = Num.Or(Str);

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

        private static class Var
        {
            public static readonly Parser<string> Id = Parse.Identifier(Parse.Letter, Parse.LetterOrDigit);

            public static readonly Parser<BasicVariable> StrScalar =
                from v in Id
                from d in Ch.Dollar
                select new StringVariable(v);

            public static readonly Parser<BasicVariable> NumScalar =
                from v in Id
                select new NumericVariable(v);

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
                select new BasicArray(v, i);

            public static readonly Parser<BasicExpression> StrArray =
                from v in StrScalar
                from i in IndexList
                select new BasicArray(v, i);

            public static readonly Parser<BasicExpression> NumAny = NumArray.Or(NumScalar);

            public static readonly Parser<BasicExpression> StrAny = StrArray.Or(StrScalar);

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
        }

        private static class Expr
        {
            public static readonly Parser<BasicExpression> Any =
                Parse.Ref(() => Str)
                .Or(Parse.Ref(() => Num))
                .End();

            private static readonly Parser<BasicExpression> StrValue = Lit.Str.Or(Var.StrAny);

            private static readonly Parser<BasicExpression> StrParen =
                from lp in Ch.LeftParen
                from x in Parse.Ref(() => Str)
                from rp in Ch.RightParen
                select x;

            private static readonly Parser<BasicExpression> StrTerm = StrParen.Or(StrValue);

            private static readonly Parser<BasicExpression> Str =
                Parse.ChainOperator(Op.Add, StrTerm, Op.Apply);

            private static readonly Parser<BasicExpression> NumParen =
                from lp in Ch.LeftParen
                from x in Parse.Ref(() => Num)
                from rp in Ch.RightParen
                select x;

            private static readonly Parser<BasicExpression> NumValue = Lit.Num.Or(Var.NumAny);

            private static readonly Parser<BasicExpression> NumFactor = NumParen.Or(NumValue);

            private static readonly Parser<BasicExpression> NumNeg =
                from m in Ch.Minus
                from x in NumFactor
                select new NegateExpression(x);

            private static readonly Parser<BasicExpression> NumOperand = NumNeg.Or(NumFactor);

            private static readonly Parser<BasicExpression> NumPow =
                Parse.ChainRightOperator(Op.Exponential, NumOperand, Op.Apply);

            private static readonly Parser<BasicExpression> NumMult =
                Parse.ChainOperator(Op.Multiplicative, NumPow, Op.Apply);

            private static readonly Parser<BasicExpression> Num =
                Parse.ChainOperator(Op.Additive, NumMult, Op.Apply);

            private sealed class NegateExpression : BasicExpression
            {
                private readonly BasicExpression x;

                public NegateExpression(BasicExpression x)
                {
                    this.x = x;
                }

                public override string ToString() => "Negate(" + this.x + ")";
            }
        }

        private static class Op
        {
            public static readonly Parser<IOperator> Add =
                from o in Ch.Plus
                select AddOperator.Value;

            public static readonly Parser<IOperator> Subtract =
                from o in Ch.Minus
                select SubtractOperator.Value;

            public static readonly Parser<IOperator> Multiply =
                from o in Ch.Star
                select MultiplyOperator.Value;

            public static readonly Parser<IOperator> Divide =
                from o in Ch.Slash
                select DivideOperator.Value;

            public static readonly Parser<IOperator> Additive = Add.Or(Subtract);

            public static readonly Parser<IOperator> Multiplicative = Multiply.Or(Divide);

            public static readonly Parser<IOperator> Exponential =
                from o in Ch.Caret
                select PowOperator.Value;

            public static BasicExpression Apply(IOperator op, BasicExpression x, BasicExpression y)
            {
                return op.Apply(x, y);
            }

            private abstract class BinaryExpression : BasicExpression
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

            private sealed class AddOperator : IOperator
            {
                public static readonly IOperator Value = new AddOperator();

                private AddOperator()
                {
                }

                public BasicExpression Apply(BasicExpression x, BasicExpression y)
                {
                    return new AddExpression(x, y);
                }

                private sealed class AddExpression : BinaryExpression
                {
                    public AddExpression(BasicExpression x, BasicExpression y)
                        : base("Add", x, y)
                    {
                    }
                }
            }

            private sealed class SubtractOperator : IOperator
            {
                public static readonly IOperator Value = new SubtractOperator();

                private SubtractOperator()
                {
                }

                public BasicExpression Apply(BasicExpression x, BasicExpression y)
                {
                    return new SubtractExpression(x, y);
                }

                private sealed class SubtractExpression : BinaryExpression
                {
                    public SubtractExpression(BasicExpression x, BasicExpression y)
                        : base("Subtract", x, y)
                    {
                    }
                }
            }

            private sealed class MultiplyOperator : IOperator
            {
                public static readonly IOperator Value = new MultiplyOperator();

                private MultiplyOperator()
                {
                }

                public BasicExpression Apply(BasicExpression x, BasicExpression y)
                {
                    return new MultiplyExpression(x, y);
                }

                private sealed class MultiplyExpression : BinaryExpression
                {
                    public MultiplyExpression(BasicExpression x, BasicExpression y)
                        : base("Multiply", x, y)
                    {
                    }
                }
            }

            private sealed class DivideOperator : IOperator
            {
                public static readonly IOperator Value = new DivideOperator();

                private DivideOperator()
                {
                }

                public BasicExpression Apply(BasicExpression x, BasicExpression y)
                {
                    return new DivideExpression(x, y);
                }

                private sealed class DivideExpression : BinaryExpression
                {
                    public DivideExpression(BasicExpression x, BasicExpression y)
                        : base("Divide", x, y)
                    {
                    }
                }
            }

            private sealed class PowOperator : IOperator
            {
                public static readonly IOperator Value = new PowOperator();

                private PowOperator()
                {
                }

                public BasicExpression Apply(BasicExpression x, BasicExpression y)
                {
                    return new PowExpression(x, y);
                }

                private sealed class PowExpression : BinaryExpression
                {
                    public PowExpression(BasicExpression x, BasicExpression y)
                        : base("Pow", x, y)
                    {
                    }
                }
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
