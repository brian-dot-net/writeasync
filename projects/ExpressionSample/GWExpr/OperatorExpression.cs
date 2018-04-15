// <copyright file="OperatorExpression.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr
{
    internal sealed class OperatorExpression : BasicExpression
    {
        private readonly string name;
        private readonly BasicExpression[] operands;

        private OperatorExpression(string name, BasicExpression[] operands)
        {
            this.name = name;
            this.operands = operands;
        }

        public static OperatorExpression Unary(string name, BasicExpression x)
        {
            return new OperatorExpression(name, new BasicExpression[] { x });
        }

        public static OperatorExpression Binary(string name, BasicExpression x, BasicExpression y)
        {
            return new OperatorExpression(name, new BasicExpression[] { x, y });
        }

        public static OperatorExpression Ternary(string name, BasicExpression x, BasicExpression y, BasicExpression z)
        {
            return new OperatorExpression(name, new BasicExpression[] { x, y, z });
        }

        public override void Accept(IVisitor visit)
        {
            visit.Operator(this.name, this.operands);
        }
    }
}
