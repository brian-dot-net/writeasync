// <copyright file="BasicOperator.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Expressions
{
    internal sealed class BasicOperator : BasicExpression
    {
        private readonly string name;
        private readonly BasicExpression[] operands;

        private BasicOperator(string name, BasicType type, BasicExpression[] operands)
            : base(type)
        {
            this.name = name;
            this.operands = operands;
        }

        public static BasicOperator Unary(string name, BasicType type, BasicExpression x)
        {
            return new BasicOperator(name, type, new BasicExpression[] { x });
        }

        public static BasicOperator Binary(string name, BasicType type, BasicExpression x, BasicExpression y)
        {
            return new BasicOperator(name, type, new BasicExpression[] { x, y });
        }

        public static BasicOperator Ternary(string name, BasicType type, BasicExpression x, BasicExpression y, BasicExpression z)
        {
            return new BasicOperator(name, type, new BasicExpression[] { x, y, z });
        }

        public override void Accept(IExpressionVisitor visit)
        {
            visit.Operator(this.name, this.operands);
        }
    }
}
