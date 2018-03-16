// <copyright file="ExpressionSearch.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4
{
    using System;
    using System.Collections.Generic;

    public sealed class ExpressionSearch
    {
        private readonly List<string> operands;
        private readonly List<string> operators;

        public ExpressionSearch()
        {
            this.operands = new List<string>();
            this.operators = new List<string>();
        }

        public void AddOperand(string op)
        {
            this.operands.Add(op);
        }

        public void AddOperator(string op)
        {
            this.operators.Add(op);
        }

        public void Run(Func<Expression, bool> each)
        {
            this.Run(default(Expression), each);
        }

        private bool Run(Expression expr, Func<Expression, bool> each)
        {
            if (!expr.IsValid)
            {
                return true;
            }

            if (expr.Result.IsValid)
            {
                if (!each(expr))
                {
                    return false;
                }
            }

            foreach (string op in this.operators)
            {
                if (!this.Run(expr.Append(op), each))
                {
                    return false;
                }
            }

            if (expr.NumeralCount < 4)
            {
                foreach (string op in this.operands)
                {
                    if (!this.Run(expr.Append(op), each))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
