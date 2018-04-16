// <copyright file="DataStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    using GWParse.Expressions;

    internal sealed class DataStatement : BasicStatement
    {
        private readonly BasicExpression c;

        public DataStatement(BasicExpression c)
        {
            this.c = c;
        }

        public override string ToString() => "Data(" + this.c + ")";
    }
}