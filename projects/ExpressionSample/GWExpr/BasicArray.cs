// <copyright file="BasicArray.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr
{
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class BasicArray : BasicExpression
    {
        private readonly BasicVariable v;
        private readonly BasicExpression[] i;

        public BasicArray(BasicVariable v, IEnumerable<BasicExpression> i)
        {
            this.v = v;
            this.i = i.ToArray();
        }

        public override string ToString()
        {
            var list = string.Join<BasicExpression>(", ", this.i);
            return "Array(" + this.v + ", " + list + ")";
        }
    }
}
