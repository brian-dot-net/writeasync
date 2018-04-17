// <copyright file="BasicVariable.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Expressions
{
    internal sealed class BasicVariable : BasicExpression
    {
        private readonly string name;

        private BasicVariable(BasicType type, string name)
            : base(type)
        {
            this.name = name;
        }

        public static BasicVariable Num(string name) => new BasicVariable(BasicType.Num, name);

        public static BasicVariable Str(string name) => new BasicVariable(BasicType.Str, name);

        public override void Accept(IExpressionVisitor visit)
        {
            visit.Variable(this.Type, this.name);
        }
    }
}
