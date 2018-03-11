// <copyright file="ExpressionSearch.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4
{
    using System;
    using System.Collections.Generic;

    public sealed class ExpressionSearch
    {
        private readonly List<string> ops;
        private readonly List<string> binaryOps;
        private readonly List<string> unaryOps;

        public ExpressionSearch()
        {
            this.ops = new List<string>();
            this.binaryOps = new List<string>();
            this.unaryOps = new List<string>();
        }

        public void AddOperand(string op)
        {
            this.ops.Add(op);
        }

        public void AddBinary(string op)
        {
            this.binaryOps.Add(op);
        }

        public void AddUnary(string op)
        {
            this.unaryOps.Add(op);
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

            if (expr.NumeralCount < 4)
            {
                foreach (string op in this.ops)
                {
                    if (!this.Run(expr.Append(op), each))
                    {
                        return false;
                    }
                }
            }

            foreach (string bop in this.binaryOps)
            {
                if (!this.Run(expr.Append(bop), each))
                {
                    return false;
                }
            }

            foreach (string uop in this.unaryOps)
            {
                if (!this.Run(expr.Append(uop), each))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
