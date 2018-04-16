// <copyright file="ReadStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    using System.Collections.Generic;
    using System.Linq;
    using GWParse.Expressions;

    internal sealed class ReadStatement : BasicStatement
    {
        private readonly BasicExpression[] list;

        public ReadStatement(IEnumerable<BasicExpression> list)
        {
            this.list = list.ToArray();
        }

        public override string ToString()
        {
            return "Read(" + string.Join<BasicExpression>(", ", this.list) + ")";
        }
    }
}