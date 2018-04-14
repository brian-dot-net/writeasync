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
                select OperatorExpression.Binary("Left", x, n);

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
                select OperatorExpression.Ternary("Mid", t.Item1, t.Item2, m);

            private static readonly Parser<BasicExpression> Mid2 =
                from t in MidPrefix
                from rp in Ch.RightParen
                select OperatorExpression.Binary("Mid", t.Item1, t.Item2);

            private static readonly Parser<BasicExpression> Mid = Mid3.Or(Mid2);

            private static readonly Parser<BasicExpression> Right =
                from f in Kw.Right
                from d in Ch.Dollar
                from lp in Ch.LeftParen
                from x in Any
                from c in Ch.Comma
                from n in Num.Any
                from rp in Ch.RightParen
                select OperatorExpression.Binary("Right", x, n);

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
                select OperatorExpression.Unary("Exp", x);

            private static readonly Parser<BasicExpression> Len =
                from f in Kw.Len
                from x in Str.Paren
                select OperatorExpression.Unary("Len", x);

            private static readonly Parser<BasicExpression> Sqr =
                from f in Kw.Sqr
                from x in Paren
                select OperatorExpression.Unary("Sqrt", x);

            private static readonly Parser<BasicExpression> Fun = Exp.Or(Len).Or(Sqr);

            private static readonly Parser<BasicExpression> Value = Lit.Num.Or(Fun).Or(Var.NumAny);

            private static readonly Parser<BasicExpression> Factor = Paren.Or(Value);

            private static readonly Parser<BasicExpression> Operand = Unary.Or(Factor);

            private static readonly Parser<BasicExpression> Pow =
                Parse.ChainOperator(Op.Exponential, Operand, Op.Apply);

            private static readonly Parser<BasicExpression> Neg =
                from m in Ch.Minus
                from x in Pow
                select OperatorExpression.Unary("Neg", x);

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
                select OperatorExpression.Unary("Not", x);

            private static readonly Parser<BasicExpression> Root = Not.Or(Relational);
        }

        private static class Op
        {
            public static readonly Parser<IOperator> Or =
                from s1 in Ch.Space
                from k in Kw.Or
                from s2 in Ch.Space
                select Binary.Or;

            public static readonly Parser<IOperator> And =
                from s1 in Ch.Space
                from k in Kw.And
                from s2 in Ch.Space
                select Binary.And;

            public static readonly Parser<IOperator> Eq =
                from o in Ch.Equal
                select Binary.Eq;

            public static readonly Parser<IOperator> Ne =
                from o1 in Ch.Less
                from o2 in Ch.Greater
                select Binary.Ne;

            public static readonly Parser<IOperator> Le =
                from o1 in Ch.Less
                from oe in Ch.Equal
                select Binary.Le;

            public static readonly Parser<IOperator> Lt =
                from o1 in Ch.Less
                select Binary.Lt;

            public static readonly Parser<IOperator> Ge =
                from o1 in Ch.Greater
                from o2 in Ch.Equal
                select Binary.Ge;

            public static readonly Parser<IOperator> Gt =
                from o1 in Ch.Greater
                select Binary.Gt;

            public static readonly Parser<IOperator> Add =
                from o in Ch.Plus
                select Binary.Add;

            public static readonly Parser<IOperator> Subtract =
                from o in Ch.Minus
                select Binary.Sub;

            public static readonly Parser<IOperator> Multiply =
                from o in Ch.Star
                select Binary.Mult;

            public static readonly Parser<IOperator> Divide =
                from o in Ch.Slash
                select Binary.Div;

            public static readonly Parser<IOperator> Additive = Add.Or(Subtract);

            public static readonly Parser<IOperator> Multiplicative = Multiply.Or(Divide);

            public static readonly Parser<IOperator> Exponential =
                from o in Ch.Caret
                select Binary.Pow;

            public static readonly Parser<IOperator> Relational = Eq.Or(Ne).Or(Le).Or(Lt).Or(Ge).Or(Gt);

            public static BasicExpression Apply(IOperator op, BasicExpression x, BasicExpression y)
            {
                return op.Apply(x, y);
            }

            private sealed class Binary : IOperator
            {
                public static readonly IOperator Or = new Binary("Or");
                public static readonly IOperator And = new Binary("And");
                public static readonly IOperator Eq = new Binary("Eq");
                public static readonly IOperator Ne = new Binary("Ne");
                public static readonly IOperator Le = new Binary("Le");
                public static readonly IOperator Lt = new Binary("Lt");
                public static readonly IOperator Ge = new Binary("Ge");
                public static readonly IOperator Gt = new Binary("Gt");
                public static readonly IOperator Add = new Binary("Add");
                public static readonly IOperator Sub = new Binary("Sub");
                public static readonly IOperator Mult = new Binary("Mult");
                public static readonly IOperator Div = new Binary("Div");
                public static readonly IOperator Pow = new Binary("Pow");

                private readonly string name;

                private Binary(string name)
                {
                    this.name = name;
                }

                public BasicExpression Apply(BasicExpression x, BasicExpression y)
                {
                    return OperatorExpression.Binary(this.name, x, y);
                }
            }
        }
    }
}
