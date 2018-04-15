// <copyright file="AssignmentStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GW.Statements
{
    using System;
    using GW.Expressions;

    internal sealed class AssignmentStatement : BasicStatement
    {
        private static readonly IExpressionVisitor Visitor = new LValueVisitor();

        private readonly BasicExpression left;
        private readonly BasicExpression right;

        public AssignmentStatement(BasicExpression left, BasicExpression right)
        {
            this.left = EnsureLValue(left);
            this.right = right;
        }

        public override string ToString() => "Assign(" + this.left + ", " + this.right + ")";

        private static BasicExpression EnsureLValue(BasicExpression left)
        {
            left.Accept(Visitor);
            return left;
        }

        private sealed class LValueVisitor : IExpressionVisitor
        {
            public void Array(BasicType type, string name, BasicExpression[] subs)
            {
                // Allowed
            }

            public void Literal(BasicType type, object o)
            {
                throw new NotSupportedException("Not an L-value.");
            }

            public void Operator(string name, BasicExpression[] operands)
            {
                throw new NotSupportedException("Not an L-value.");
            }

            public void Variable(BasicType type, string name)
            {
                // Allowed
            }
        }
    }
}