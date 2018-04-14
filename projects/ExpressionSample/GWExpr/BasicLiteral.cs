// <copyright file="BasicLiteral.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr
{
    internal abstract class BasicLiteral : BasicExpression
    {
        private readonly object o;
        private readonly string s;

        protected BasicLiteral(object o, string s)
        {
            this.o = o;
            this.s = s;
        }

        public static BasicLiteral Num(string n) => new NumericLiteral(int.Parse(n));

        public static BasicLiteral Str(string s) => new StringLiteral(s);

        public override string ToString() => "L(" + this.s + this.o + this.s + ")";

        private sealed class StringLiteral : BasicLiteral
        {
            public StringLiteral(string s)
                : base(s, "\"")
            {
            }
        }

        private sealed class NumericLiteral : BasicLiteral
        {
            public NumericLiteral(int n)
                    : base(n, string.Empty)
            {
            }
        }
    }
}