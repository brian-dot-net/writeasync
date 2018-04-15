// <copyright file="BasicArray.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Expressions
{
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class BasicArray : BasicExpression
    {
        private readonly string name;
        private readonly BasicType type;
        private readonly BasicExpression[] subs;

        private BasicArray(BasicType type, string name, IEnumerable<BasicExpression> subs)
        {
            this.type = type;
            this.name = name;
            this.subs = subs.ToArray();
        }

        public static BasicArray Num(string name, IEnumerable<BasicExpression> subs)
        {
            return new BasicArray(BasicType.Num, name, subs);
        }

        public static BasicArray Str(string name, IEnumerable<BasicExpression> subs)
        {
            return new BasicArray(BasicType.Str, name, subs);
        }

        public override void Accept(IExpressionVisitor visit)
        {
            visit.Array(this.type, this.name, this.subs);
        }
    }
}
