// <copyright file="DimensionStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    using System.Collections.Generic;
    using System.Linq;
    using GWParse.Expressions;

    internal sealed class DimensionStatement : BasicStatement
    {
        private readonly BasicExpression[] list;

        public DimensionStatement(IEnumerable<BasicExpression> list)
        {
            this.list = list.ToArray();
        }

        public override string ToString()
        {
            return "Dim(" + string.Join<BasicExpression>(", ", this.list) + ")";
        }
    }
}