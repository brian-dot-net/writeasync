// <copyright file="InputStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    using GWParse.Expressions;

    internal sealed class InputStatement : BasicStatement
    {
        private readonly BasicExpression v;

        public InputStatement(BasicExpression v)
        {
            this.v = v;
        }

        public override string ToString() => "Input(\"\", " + this.v + ")";
    }
}