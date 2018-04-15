// <copyright file="DimensionStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GW.Statements
{
    using System;
    using GWParse.Expressions;

    internal sealed class DimensionStatement : BasicStatement
    {
        private static readonly IExpressionVisitor Visitor = new ArrayVisitor();

        private readonly BasicExpression a;

        public DimensionStatement(BasicExpression a)
        {
            this.a = EnsureArray(a);
        }

        public override string ToString() => "Dim(" + this.a + ")";

        private static BasicExpression EnsureArray(BasicExpression a)
        {
            a.Accept(Visitor);
            return a;
        }

        private sealed class ArrayVisitor : IExpressionVisitor
        {
            public void Array(BasicType type, string name, BasicExpression[] subs)
            {
                // Allowed
            }

            public void Literal(BasicType type, object o)
            {
                throw new NotSupportedException("Not an array.");
            }

            public void Operator(string name, BasicExpression[] operands)
            {
                throw new NotSupportedException("Not an array.");
            }

            public void Variable(BasicType type, string name)
            {
                throw new NotSupportedException("Not an array.");
            }
        }
    }
}