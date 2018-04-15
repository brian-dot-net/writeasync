// <copyright file="NextStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    using System.Collections.Generic;
    using System.Linq;
    using GWParse.Expressions;

    internal sealed class NextStatement : BasicStatement
    {
        private readonly BasicExpression[] vars;

        public NextStatement(IEnumerable<BasicExpression> vars)
        {
            this.vars = vars.ToArray();
        }

        public override string ToString()
        {
            return "Next(" + string.Join<BasicExpression>(", ", this.vars) + ")";
        }
    }
}