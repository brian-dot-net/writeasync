// <copyright file="IfThenStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    using GWParse.Expressions;

    internal sealed class IfThenStatement : BasicStatement
    {
        private readonly BasicExpression cond;
        private readonly BasicStatement ifTrue;

        public IfThenStatement(BasicExpression cond, BasicStatement ifTrue)
        {
            this.cond = cond;
            this.ifTrue = ifTrue;
        }

        public override void Accept(IStatementVisitor visit)
        {
            visit.IfThen(this.cond, this.ifTrue);
        }
    }
}