// <copyright file="IVisitor.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr
{
    public interface IVisitor
    {
        void Literal(BasicType type, object o);

        void Variable(BasicType type, string name);

        void Array(BasicType type, string name, BasicExpression[] subs);

        void Operator(string name, BasicExpression[] operands);
    }
}