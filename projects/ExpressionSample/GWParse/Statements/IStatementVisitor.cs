// <copyright file="IStatementVisitor.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    using GWParse.Expressions;

    public interface IStatementVisitor
    {
        void Assign(BasicExpression left, BasicExpression right);

        void For(BasicExpression v, BasicExpression start, BasicExpression end, BasicExpression step);

        void Go(string name, int dest);

        void IfThen(BasicExpression cond, BasicStatement ifTrue);

        void Input(string prompt, BasicExpression v);

        void Many(string name, BasicExpression[] list);

        void Remark(string text);

        void Void(string name);
    }
}