// <copyright file="BasicVariable.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr
{
    internal abstract class BasicVariable : BasicExpression
    {
        private readonly string v;

        protected BasicVariable(string v)
        {
            this.v = v.ToUpperInvariant();
        }

        public static BasicVariable Str(string v) => new StringVariable(v);

        public static BasicVariable Num(string v) => new NumericVariable(v);

        public override string ToString() => "Var(" + this.v + ")";

        private sealed class NumericVariable : BasicVariable
        {
            public NumericVariable(string v)
                : base(v)
            {
            }

            public override string ToString() => "Num" + base.ToString();
        }

        private sealed class StringVariable : BasicVariable
        {
            public StringVariable(string v)
                : base(v)
            {
            }

            public override string ToString() => "Str" + base.ToString();
        }
    }
}
