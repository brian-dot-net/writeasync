// <copyright file="BasicLiteral.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr
{
    internal sealed class BasicLiteral : BasicExpression
    {
        private readonly BasicType type;
        private readonly object o;

        private BasicLiteral(BasicType type, object o)
        {
            this.type = type;
            this.o = o;
        }

        public static BasicLiteral Num(string n) => new BasicLiteral(BasicType.Num, int.Parse(n));

        public static BasicLiteral Str(string s) => new BasicLiteral(BasicType.Str, s);

        public override string ToString()
        {
            string s = this.o.ToString();
            if (this.type == BasicType.Str)
            {
                s = "\"" + s + "\"";
            }

            return this.type + "L(" + s + ")";
        }
    }
}