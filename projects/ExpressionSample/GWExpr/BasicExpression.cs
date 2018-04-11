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
            public static readonly Parser<char> Space = Parse.Char(' ');
            public static readonly Parser<char> Equal = Parse.Char('=');
            public static readonly Parser<char> Less = Parse.Char('<');
            public static readonly Parser<char> Greater = Parse.Char('>');
            public static readonly Parser<char> NonQuote = Parse.AnyChar.Except(Quote);
        }

        private static class Kw
        {
            public static readonly Parser<IEnumerable<char>> And = Parse.String("AND");
            public static readonly Parser<IEnumerable<char>> Not = Parse.String("NOT");
            public static readonly Parser<IEnumerable<char>> Or = Parse.String("OR");

            public static readonly Parser<IEnumerable<char>> Any =
                And.Or(Not).Or(Or);
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

                public override string ToString() => "Literal(" + this.n + ")";
            }

            private sealed class StringLiteral : BasicExpression
            {
                private readonly string s;

                public StringLiteral(string s)
                {
                    this.s = s;
                }

                public override string ToString() => "Literal(\"" + this.s + "\")";
            }
        }

        private static class Var
        {
            public static readonly Parser<string> Id =
                Parse.Identifier(Parse.Letter, Parse.LetterOrDigit)
                .Except(Kw.Any);

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

                public override string ToString() => "Num" + base.ToString();
            }

            private sealed class StringVariable : BasicVariable
            {
                public StringVariable(string v)
                    : base(v)
                {
                }

                public override string ToString() => "Str" + base.ToString();
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
            public static readonly Parser<BasicExpression> Any = Str.Any.Or(Num.Any).End();
        }

        private static class Str
        {
            public static readonly Parser<BasicExpression> Any =
                Parse.ChainOperator(Op.Add, Parse.Ref(() => Term), Op.Apply);

            private static readonly Parser<BasicExpression> Value = Lit.Str.Or(Var.StrAny);

            private static readonly Parser<BasicExpression> Paren =
                from lp in Ch.LeftParen
                from x in Any
                from rp in Ch.RightParen
                select x;

            private static readonly Parser<BasicExpression> Term = Paren.Or(Value);
        }

        private static class Num
        {
            public static readonly Parser<BasicExpression> Any = Parse.Ref(() => Or);

            private static readonly Parser<BasicExpression> Unary =
                Parse.Ref(() => Neg)
                .Or(Parse.Ref(() => Not));

            private static readonly Parser<BasicExpression> Paren =
                from lp in Ch.LeftParen
                from x in Any
                from rp in Ch.RightParen
                select x;

            private static readonly Parser<BasicExpression> Value = Lit.Num.Or(Var.NumAny);

            private static readonly Parser<BasicExpression> Factor = Paren.Or(Value);

            private static readonly Parser<BasicExpression> Operand = Unary.Or(Factor);

            private static readonly Parser<BasicExpression> Pow =
                Parse.ChainOperator(Op.Exponential, Operand, Op.Apply);

            private static readonly Parser<BasicExpression> Neg =
                from m in Ch.Minus
                from x in Pow
                select new NegateExpression(x);

            private static readonly Parser<BasicExpression> Mult =
                Parse.ChainOperator(Op.Multiplicative, Neg.Or(Pow), Op.Apply);

            private static readonly Parser<BasicExpression> Add =
                Parse.ChainOperator(Op.Additive, Mult, Op.Apply);

            private static readonly Parser<BasicExpression> Relational =
                Parse.ChainOperator(Op.Relational, Add, Op.Apply);

            private static readonly Parser<BasicExpression> Not =
                from k in Kw.Not
                from s in Ch.Space
                from x in Add
                select new NotExpression(x);

            private static readonly Parser<BasicExpression> And =
                Parse.ChainOperator(Op.And, Not.Or(Relational), Op.Apply);

            private static readonly Parser<BasicExpression> Or =
                Parse.ChainOperator(Op.Or, And, Op.Apply);

            private sealed class NegateExpression : BasicExpression
            {
                private readonly BasicExpression x;

                public NegateExpression(BasicExpression x)
                {
                    this.x = x;
                }

                public override string ToString() => "Negate(" + this.x + ")";
            }

            private sealed class NotExpression : BasicExpression
            {
                private readonly BasicExpression x;

                public NotExpression(BasicExpression x)
                {
                    this.x = x;
                }

                public override string ToString() => "Not(" + this.x + ")";
            }
        }

        private static class Op
        {
            public static readonly Parser<IOperator> Or =
                from s1 in Ch.Space
                from k in Kw.Or
                from s2 in Ch.Space
                select OrOperator.Value;

            public static readonly Parser<IOperator> And =
                from s1 in Ch.Space
                from k in Kw.And
                from s2 in Ch.Space
                select AndOperator.Value;

            public static readonly Parser<IOperator> Eq =
                from o in Ch.Equal
                select EqOperator.Value;

            public static readonly Parser<IOperator> Ne =
                from o1 in Ch.Less
                from o2 in Ch.Greater
                select NeOperator.Value;

            public static readonly Parser<IOperator> Lt =
                from o1 in Ch.Less
                select LtOperator.Value;

            public static readonly Parser<IOperator> Gt =
                from o1 in Ch.Greater
                select GtOperator.Value;

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

            public static readonly Parser<IOperator> Relational = Eq.Or(Ne).Or(Lt).Or(Gt);

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

            private sealed class OrOperator : IOperator
            {
                public static readonly IOperator Value = new OrOperator();

                private OrOperator()
                {
                }

                public BasicExpression Apply(BasicExpression x, BasicExpression y)
                {
                    return new OrExpression(x, y);
                }
            }

            private sealed class AndOperator : IOperator
            {
                public static readonly IOperator Value = new AndOperator();

                private AndOperator()
                {
                }

                public BasicExpression Apply(BasicExpression x, BasicExpression y)
                {
                    return new AndExpression(x, y);
                }
            }

            private sealed class EqOperator : IOperator
            {
                public static readonly IOperator Value = new EqOperator();

                private EqOperator()
                {
                }

                public BasicExpression Apply(BasicExpression x, BasicExpression y)
                {
                    return new EqExpression(x, y);
                }
            }

            private sealed class NeOperator : IOperator
            {
                public static readonly IOperator Value = new NeOperator();

                private NeOperator()
                {
                }

                public BasicExpression Apply(BasicExpression x, BasicExpression y)
                {
                    return new NeExpression(x, y);
                }
            }

            private sealed class LtOperator : IOperator
            {
                public static readonly IOperator Value = new LtOperator();

                private LtOperator()
                {
                }

                public BasicExpression Apply(BasicExpression x, BasicExpression y)
                {
                    return new LtExpression(x, y);
                }
            }

            private sealed class GtOperator : IOperator
            {
                public static readonly IOperator Value = new GtOperator();

                private GtOperator()
                {
                }

                public BasicExpression Apply(BasicExpression x, BasicExpression y)
                {
                    return new GtExpression(x, y);
                }
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
            }

            private sealed class OrExpression : BinaryExpression
            {
                public OrExpression(BasicExpression x, BasicExpression y)
                    : base("Or", x, y)
                {
                }
            }

            private sealed class AndExpression : BinaryExpression
            {
                public AndExpression(BasicExpression x, BasicExpression y)
                    : base("And", x, y)
                {
                }
            }

            private sealed class EqExpression : BinaryExpression
            {
                public EqExpression(BasicExpression x, BasicExpression y)
                    : base("Eq", x, y)
                {
                }
            }

            private sealed class NeExpression : BinaryExpression
            {
                public NeExpression(BasicExpression x, BasicExpression y)
                    : base("Ne", x, y)
                {
                }
            }

            private sealed class LtExpression : BinaryExpression
            {
                public LtExpression(BasicExpression x, BasicExpression y)
                    : base("Lt", x, y)
                {
                }
            }

            private sealed class GtExpression : BinaryExpression
            {
                public GtExpression(BasicExpression x, BasicExpression y)
                    : base("Gt", x, y)
                {
                }
            }

            private sealed class AddExpression : BinaryExpression
            {
                public AddExpression(BasicExpression x, BasicExpression y)
                    : base("Add", x, y)
                {
                }
            }

            private sealed class SubtractExpression : BinaryExpression
            {
                public SubtractExpression(BasicExpression x, BasicExpression y)
                    : base("Subtract", x, y)
                {
                }
            }

            private sealed class MultiplyExpression : BinaryExpression
            {
                public MultiplyExpression(BasicExpression x, BasicExpression y)
                    : base("Multiply", x, y)
                {
                }
            }

            private sealed class DivideExpression : BinaryExpression
            {
                public DivideExpression(BasicExpression x, BasicExpression y)
                    : base("Divide", x, y)
                {
                }
            }

            private sealed class PowExpression : BinaryExpression
            {
                public PowExpression(BasicExpression x, BasicExpression y)
                    : base("Pow", x, y)
                {
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

            public override string ToString() => "Var(" + this.v + ")";
        }
    }
}
