// <copyright file="IExpressionVisitor.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Expressions
{
    public interface IExpressionVisitor
    {
        void Literal(BasicType type, object o);

        void Variable(BasicType type, string name);

        void Array(BasicType type, string name, BasicExpression[] subs);

        void Operator(string name, BasicExpression[] operands);
    }
}