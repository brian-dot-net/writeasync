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
        private readonly bool lineBreak;

        public PrintStatement(IEnumerable<BasicExpression> list, bool lineBreak = true)
        {
            this.list = list.ToArray();
            this.lineBreak = lineBreak;
        }

        public override string ToString()
        {
            return
                "Print" + (this.lineBreak ? string.Empty : "N") + "(" +
                string.Join<BasicExpression>(", ", this.list) + ")";
        }
    }
}