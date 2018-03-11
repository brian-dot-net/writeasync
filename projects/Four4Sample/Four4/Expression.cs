// <copyright file="Expression.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4
{
    using System;

    public struct Expression
    {
        private readonly NumberStack operands;

        private Expression(NumberStack operands)
        {
            this.operands = operands;
        }

        public static Number Eval(string input)
        {
            Expression expr = default(Expression);
            string[] tokens = input.Split(' ');
            foreach (string token in tokens)
            {
                expr = expr.Append(token);
            }

            return expr.Result();
        }

        public Number Result() => this.operands.Result();

        public override string ToString()
        {
            return string.Empty;
        }

        private Expression Append(string token)
        {
            switch (token)
            {
                case "+":
                    return this.Binary((x, y) => x + y);
                case "-":
                    return this.Binary((x, y) => x - y);
                case "*":
                    return this.Binary((x, y) => x * y);
                case "/":
                    return this.Binary((x, y) => x / y);
                case "!":
                    return this.Unary(x => x.Factorial());
                case "R":
                    return this.Unary(x => x.SquareRoot());
                default:
                    return this.Push(Number.Parse(token));
            }
        }

        private Expression Push(Number number)
        {
            return new Expression(this.operands.Push(number));
        }

        private Expression Binary(Func<Number, Number, Number> op)
        {
            return new Expression(this.operands.Apply2(op));
        }

        private Expression Unary(Func<Number, Number> op)
        {
            return new Expression(this.operands.Apply1(op));
        }

        private struct NumberStack
        {
            private readonly Number n1;
            private readonly Number n2;
            private readonly Number n3;
            private readonly Number n4;
            private readonly int size;

            private NumberStack(Number n1, Number n2 = default(Number), Number n3 = default(Number), Number n4 = default(Number))
            {
                this.n1 = n1;
                this.n2 = n2;
                this.n3 = n3;
                this.n4 = n4;
                if (this.n4.IsValid)
                {
                    this.size = 4;
                }
                else if (this.n3.IsValid)
                {
                    this.size = 3;
                }
                else if (this.n2.IsValid)
                {
                    this.size = 2;
                }
                else
                {
                    this.size = 1;
                }
            }

            public Number Result()
            {
                switch (this.size)
                {
                    case 1:
                        return this.n1;
                    default:
                        return default(Number);
                }
            }

            public NumberStack Push(Number n)
            {
                switch (this.size)
                {
                    case 0:
                        return new NumberStack(n);
                    case 1:
                        return new NumberStack(this.n1, n);
                    case 2:
                        return new NumberStack(this.n1, this.n2, n);
                    case 3:
                        return new NumberStack(this.n1, this.n2, this.n3, n);
                    default:
                        throw new InvalidOperationException("Stack full!");
                }
            }

            public NumberStack Apply1(Func<Number, Number> op)
            {
                switch (this.size)
                {
                    case 0:
                        return default(NumberStack);
                    case 1:
                        return new NumberStack(op(this.n1));
                    case 2:
                        return new NumberStack(this.n1, op(this.n2));
                    case 3:
                        return new NumberStack(this.n1, this.n2, op(this.n3));
                    default:
                        return new NumberStack(this.n1, this.n2, this.n3, op(this.n4));
                }
            }

            public NumberStack Apply2(Func<Number, Number, Number> op)
            {
                switch (this.size)
                {
                    case 0:
                    case 1:
                        return default(NumberStack);
                    case 2:
                        return new NumberStack(op(this.n1, this.n2));
                    case 3:
                        return new NumberStack(this.n1, op(this.n2, this.n3));
                    default:
                        return new NumberStack(this.n1, this.n2, op(this.n3, this.n4));
                }
            }
        }
    }
}
