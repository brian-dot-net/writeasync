// <copyright file="DataStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    using System.Collections.Generic;
    using System.Linq;
    using GWParse.Expressions;

    internal sealed class DataStatement : BasicStatement
    {
        private readonly BasicExpression[] list;

        public DataStatement(IEnumerable<BasicExpression> list)
        {
            this.list = list.ToArray();
        }

        public override string ToString()
        {
            return "Data(" + string.Join<BasicExpression>(", ", this.list) + ")";
        }
    }
}