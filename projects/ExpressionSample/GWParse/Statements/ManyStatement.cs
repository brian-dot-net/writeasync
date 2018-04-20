// <copyright file="ManyStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    using System.Collections.Generic;
    using System.Linq;
    using GWParse.Expressions;

    internal sealed class ManyStatement : BasicStatement
    {
        private readonly string name;
        private readonly BasicExpression[] list;

        public ManyStatement(string name, IEnumerable<BasicExpression> list)
        {
            this.name = name;
            this.list = list.ToArray();
        }

        public override void Accept(IStatementVisitor visit)
        {
            visit.Many(this.name, this.list);
        }
    }
}