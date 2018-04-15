// <copyright file="ReadStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    using GWParse.Expressions;

    internal sealed class ReadStatement : BasicStatement
    {
        private readonly BasicExpression v;

        public ReadStatement(BasicExpression v)
        {
            this.v = v;
        }

        public override string ToString() => "Read(" + this.v + ")";
    }
}