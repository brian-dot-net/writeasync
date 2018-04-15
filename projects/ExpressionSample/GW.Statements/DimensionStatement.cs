// <copyright file="DimensionStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GW.Statements
{
    using GW.Expressions;

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