// <copyright file="PrintStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    using System.Collections.Generic;
    using System.Linq;
    using GWParse.Expressions;

    internal sealed class PrintStatement : BasicStatement
    {
        private readonly BasicExpression[] list;

        public PrintStatement(IEnumerable<BasicExpression> list)
        {
            this.list = list.ToArray();
        }

        public override string ToString()
        {
            return "Print(" + string.Join<BasicExpression>(", ", this.list) + ")";
        }
    }
}