// <copyright file="AssignmentStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GW.Statements
{
    using GW.Expressions;

    internal sealed class AssignmentStatement : BasicStatement
    {
        private readonly BasicExpression left;
        private readonly BasicExpression right;

        public AssignmentStatement(BasicExpression left, BasicExpression right)
        {
            this.left = left;
            this.right = right;
        }

        public override string ToString() => "Assign(" + this.left + ", " + this.right + ")";
    }
}