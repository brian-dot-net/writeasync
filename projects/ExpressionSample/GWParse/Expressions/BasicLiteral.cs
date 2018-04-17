// <copyright file="BasicLiteral.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Expressions
{
    internal sealed class BasicLiteral : BasicExpression
    {
        private readonly object o;

        private BasicLiteral(BasicType type, object o)
            : base(type)
        {
            this.o = o;
        }

        public static BasicLiteral Num(string n) => new BasicLiteral(BasicType.Num, int.Parse(n));

        public static BasicLiteral Str(string s) => new BasicLiteral(BasicType.Str, s);

        public override void Accept(IExpressionVisitor visit)
        {
            visit.Literal(this.Type, this.o);
        }
    }
}