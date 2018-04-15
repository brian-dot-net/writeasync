// <copyright file="DimensionStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    using GWParse.Expressions;

    internal sealed class DimensionStatement : BasicStatement
    {
        private readonly BasicExpression a;

        public DimensionStatement(BasicExpression a)
        {
            this.a = a;
        }

        public override string ToString() => "Dim(" + this.a + ")";
    }
}