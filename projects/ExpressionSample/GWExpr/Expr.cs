// <copyright file="Expr.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sprache;

    internal static class Expr
    {
        private static readonly Parser<BasicExpression> Any = Parse.Ref(() => Root);

        private static readonly Parser<BasicExpression> Paren =
            from lp in Ch.LeftParen
            from x in Any
            from rp in Ch.RightParen
            select x;

        private static readonly Parser<BasicExpression> Value = Str.Any.Or(Num.Any);

        private static readonly Parser<BasicExpression> Factor = Value.Or(Paren);

        private static readonly Parser<BasicExpression> And =
            Parse.ChainOperator(Op.And, Factor, Op.Apply);

        private static readonly Parser<BasicExpression> Or =
            Parse.ChainOperator(Op.Or, And, Op.Apply);

        private static readonly Parser<BasicExpression> Root = Or;

        private interface IOperator
        {
            BasicExpression Apply(BasicExpression x, BasicExpression y);
        }

        public static BasicExpression FromString(string input)
        {
            try
            {
                return Any.End().Parse(input);
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
            public static readonly Parser<IEnumerable<char>> Exp = Parse.String("EXP");
            public static readonly Parser<IEnumerable<char>> Len = Parse.String("LEN");
            public static readonly Parser<IEnumerable<char>> Not = Parse.String("NOT");
            public static readonly Parser<IEnumerable<char>> Or = Parse.String("OR");
            public static readonly Parser<IEnumerable<char>> Sqr = Parse.String("SQR");

            public static readonly Parser<IEnumerable<char>> Left = Parse.String("LEFT");
            public static readonly Parser<IEnumerable<char>> Mid = Parse.String("MID");
            public static readonly Parser<IEnumerable<char>> Right = Parse.String("RIGHT");

            public static readonly Parser<IEnumerable<char>> Any =
                And.Or(Exp).Or(Len).Or(Not).Or(Or).Or(Sqr);

            public static readonly Parser<IEnumerable<char>> AnyStr = Left.Or(Mid).Or(Right);
        }

        private static class Lit
        {
            public static readonly Parser<BasicExpression> Num =
                from n in Parse.Number
                select BasicLiteral.Num(n);

            public static readonly Parser<BasicExpression> Str =
                from lq in Ch.Quote
                from s in Ch.NonQuote.Many().Text()
                from rq in Ch.Quote
                select BasicLiteral.Str(s);

            public static readonly Parser<BasicExpression> Any = Num.Or(Str);
        }

        private static class Var
        {
            public static readonly Parser<string> Id =
                Parse.Identifier(Parse.Letter, Parse.LetterOrDigit)
                .Except(Kw.Any);

            public static readonly Parser<BasicVariable> StrScalar =
                from v in Id.Except(Kw.AnyStr)
                from d in Ch.Dollar
                select BasicVariable.Str(v);

            public static readonly Parser<BasicVariable> NumScalar =
                from v in Id
                select BasicVariable.Num(v);

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
        }

        private static class Str
        {
            public static readonly Parser<BasicExpression> Any = Parse.Ref(() => Root);

            public static readonly Parser<BasicExpression> Paren =
                from lp in Ch.LeftParen
                from x in Any
                from rp in Ch.RightParen
                select x;

            private static readonly Parser<BasicExpression> Left =
                from f in Kw.Left
                from d in Ch.Dollar
                from lp in Ch.LeftParen
                from x in Any
                from c in Ch.Comma
                from n in Num.Any
                from rp in Ch.RightParen
                select new LeftExpression(x, n);

            private static readonly Parser<Tuple<BasicExpression, BasicExpression>> MidPrefix =
                from f in Kw.Mid
                from d in Ch.Dollar
                from lp in Ch.LeftParen
                from x in Any
                from c in Ch.Comma
                from n in Num.Any
                select Tuple.Create(x, n);

            private static readonly Parser<BasicExpression> Mid3 =
                from t in MidPrefix
                from c in Ch.Comma
                from m in Num.Any
                from rp in Ch.RightParen
                select new Mid3Expression(t.Item1, t.Item2, m);

            private static readonly Parser<BasicExpression> Mid2 =
                from t in MidPrefix
                from rp in Ch.RightParen
                select new Mid2Expression(t.Item1, t.Item2);

            private static readonly Parser<BasicExpression> Mid = Mid3.Or(Mid2);

            private static readonly Parser<BasicExpression> Right =
                from f in Kw.Right
                from d in Ch.Dollar
                from lp in Ch.LeftParen
                from x in Any
                from c in Ch.Comma
                from n in Num.Any
                from rp in Ch.RightParen
                select new RightExpression(x, n);

            private static readonly Parser<BasicExpression> Fun = Left.Or(Mid).Or(Right);

            private static readonly Parser<BasicExpression> Value = Lit.Str.Or(Fun).Or(Var.StrAny);

            private static readonly Parser<BasicExpression> Factor = Paren.Or(Value);

            private static readonly Parser<BasicExpression> Add =
                Parse.ChainOperator(Op.Add, Factor, Op.Apply);

            private static readonly Parser<BasicExpression> Relational =
                from x in Add
                from op in Op.Relational
                from y in Add
                select op.Apply(x, y);

            private static readonly Parser<BasicExpression> Root = Relational.Or(Add);

            private sealed class LeftExpression : BinaryExpression
            {
                public LeftExpression(BasicExpression x, BasicExpression n)
                    : base("Left", x, n)
                {
                }
            }

            private sealed class Mid2Expression : BinaryExpression
            {
                public Mid2Expression(BasicExpression x, BasicExpression n)
                    : base("Mid", x, n)
                {
                }
            }

            private sealed class Mid3Expression : BasicExpression
            {
                private readonly BasicExpression x;
                private readonly BasicExpression n;
                private readonly BasicExpression m;

                public Mid3Expression(BasicExpression x, BasicExpression n, BasicExpression m)
                {
                    this.x = x;
                    this.n = n;
                    this.m = m;
                }

                public override string ToString() => "Mid(" + this.x + ", " + this.n + ", " + this.m + ")";
            }

            private sealed class RightExpression : BinaryExpression
            {
                public RightExpression(BasicExpression x, BasicExpression n)
                    : base("Right", x, n)
                {
                }
            }
        }

        private static class Num
        {
            public static readonly Parser<BasicExpression> Any = Parse.Ref(() => Root);

            private static readonly Parser<BasicExpression> Unary =
                Parse.Ref(() => Neg)
                .Or(Parse.Ref(() => Not));

            private static readonly Parser<BasicExpression> Paren =
                from lp in Ch.LeftParen
                from x in Any
                from rp in Ch.RightParen
                select x;

            private static readonly Parser<BasicExpression> Exp =
                from f in Kw.Exp
                from x in Paren
                select new ExpExpression(x);

            private static readonly Parser<BasicExpression> Len =
                from f in Kw.Len
                from x in Str.Paren
                select new LenExpression(x);

            private static readonly Parser<BasicExpression> Sqr =
                from f in Kw.Sqr
                from x in Paren
                select new SqrtExpression(x);

            private static readonly Parser<BasicExpression> Fun = Exp.Or(Len).Or(Sqr);

            private static readonly Parser<BasicExpression> Value = Lit.Num.Or(Fun).Or(Var.NumAny);

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

            private static readonly Parser<BasicExpression> Root = Not.Or(Relational);

            private sealed class NotExpression : UnaryExpression
            {
                public NotExpression(BasicExpression x)
                    : base("Not", x)
                {
                }
            }

            private sealed class NegateExpression : UnaryExpression
            {
                public NegateExpression(BasicExpression x)
                    : base("Neg", x)
                {
                }
            }

            private sealed class ExpExpression : UnaryExpression
            {
                public ExpExpression(BasicExpression x)
                    : base("Exp", x)
                {
                }
            }

            private sealed class LenExpression : UnaryExpression
            {
                public LenExpression(BasicExpression x)
                    : base("Len", x)
                {
                }
            }

            private sealed class SqrtExpression : UnaryExpression
            {
                public SqrtExpression(BasicExpression x)
                    : base("Sqrt", x)
                {
                }
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

            public static readonly Parser<IOperator> Le =
                from o1 in Ch.Less
                from oe in Ch.Equal
                select LeOperator.Value;

            public static readonly Parser<IOperator> Lt =
                from o1 in Ch.Less
                select LtOperator.Value;

            public static readonly Parser<IOperator> Ge =
                from o1 in Ch.Greater
                from o2 in Ch.Equal
                select GeOperator.Value;

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

            public static readonly Parser<IOperator> Relational = Eq.Or(Ne).Or(Le).Or(Lt).Or(Ge).Or(Gt);

            public static BasicExpression Apply(IOperator op, BasicExpression x, BasicExpression y)
            {
                return op.Apply(x, y);
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

                private sealed class OrExpression : BinaryExpression
                {
                    public OrExpression(BasicExpression x, BasicExpression y)
                        : base("Or", x, y)
                    {
                    }
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

                private sealed class AndExpression : BinaryExpression
                {
                    public AndExpression(BasicExpression x, BasicExpression y)
                        : base("And", x, y)
                    {
                    }
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

                private sealed class EqExpression : BinaryExpression
                {
                    public EqExpression(BasicExpression x, BasicExpression y)
                        : base("Eq", x, y)
                    {
                    }
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

                private sealed class NeExpression : BinaryExpression
                {
                    public NeExpression(BasicExpression x, BasicExpression y)
                        : base("Ne", x, y)
                    {
                    }
                }
            }

            private sealed class LeOperator : IOperator
            {
                public static readonly IOperator Value = new LeOperator();

                private LeOperator()
                {
                }

                public BasicExpression Apply(BasicExpression x, BasicExpression y)
                {
                    return new LeExpression(x, y);
                }

                private sealed class LeExpression : BinaryExpression
                {
                    public LeExpression(BasicExpression x, BasicExpression y)
                        : base("Le", x, y)
                    {
                    }
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

                private sealed class LtExpression : BinaryExpression
                {
                    public LtExpression(BasicExpression x, BasicExpression y)
                        : base("Lt", x, y)
                    {
                    }
                }
            }

            private sealed class GeOperator : IOperator
            {
                public static readonly IOperator Value = new GeOperator();

                private GeOperator()
                {
                }

                public BasicExpression Apply(BasicExpression x, BasicExpression y)
                {
                    return new GeExpression(x, y);
                }

                private sealed class GeExpression : BinaryExpression
                {
                    public GeExpression(BasicExpression x, BasicExpression y)
                        : base("Ge", x, y)
                    {
                    }
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

                private sealed class GtExpression : BinaryExpression
                {
                    public GtExpression(BasicExpression x, BasicExpression y)
                        : base("Gt", x, y)
                    {
                    }
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
                        : base("Sub", x, y)
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
                        : base("Mult", x, y)
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
                        : base("Div", x, y)
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
    }
}
