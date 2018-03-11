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

        public void Run(Action<Expression> each)
        {
            this.Run(default(Expression), each);
        }

        private void Run(Expression expr, Action<Expression> each)
        {
            if (!expr.IsValid)
            {
                return;
            }

            if (expr.Result.IsValid)
            {
                each(expr);
            }

            if (expr.NumeralCount < 4)
            {
                foreach (string op in this.ops)
                {
                    this.Run(expr.Append(op), each);
                }
            }

            foreach (string bop in this.binaryOps)
            {
                this.Run(expr.Append(bop), each);
            }

            foreach (string uop in this.unaryOps)
            {
                this.Run(expr.Append(uop), each);
            }
        }
    }
}
