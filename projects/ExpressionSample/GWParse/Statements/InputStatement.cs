// <copyright file="InputStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    using GWParse.Expressions;

    internal sealed class InputStatement : BasicStatement
    {
        private readonly string prompt;
        private readonly BasicExpression v;

        public InputStatement(string prompt, BasicExpression v)
        {
            this.prompt = prompt;
            this.v = v;
        }

        public override void Accept(IStatementVisitor visit)
        {
            visit.Input(this.prompt, this.v);
        }
    }
}