// <copyright file="BasicExpression.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Expressions
{
    using System.Text;

    public abstract class BasicExpression
    {
        protected BasicExpression(BasicType type)
        {
            this.Type = type;
        }

        public BasicType Type { get; private set; }

        public static BasicExpression FromString(string input) => Expr.FromString(input);

        public abstract void Accept(IExpressionVisitor visit);

        public override string ToString()
        {
            ExpressionString str = new ExpressionString();
            this.Accept(str);
            return str.ToString();
        }

        private sealed class ExpressionString : IExpressionVisitor
        {
            private readonly StringBuilder sb;

            public ExpressionString()
            {
                this.sb = new StringBuilder();
            }

            public void Literal(BasicType type, object o)
            {
                this.sb.Append(type.ToString()).Append("L(");

                string s = o.ToString();
                if (type == BasicType.Str)
                {
                    s = "\"" + s + "\"";
                }

                this.sb.Append(s);

                this.sb.Append(")");
            }

            public void Variable(BasicType type, string name)
            {
                this.sb.Append(type.ToString()).Append("V(");

                this.sb.Append(name);

                this.sb.Append(")");
            }

            public void Array(BasicType type, string name, BasicExpression[] subs)
            {
                this.sb.Append(type.ToString()).Append("A(");

                this.sb.Append(name);

                foreach (BasicExpression sub in subs)
                {
                    this.sb.Append(", ");
                    sub.Accept(this);
                }

                this.sb.Append(")");
            }

            public void Operator(string name, BasicExpression[] operands)
            {
                this.sb.Append(name).Append("(");

                foreach (BasicExpression op in operands)
                {
                    op.Accept(this);
                    this.sb.Append(", ");
                }

                this.sb.Remove(this.sb.Length - 2, 2);

                this.sb.Append(")");
            }

            public override string ToString()
            {
                return this.sb.ToString();
            }
        }
    }
}
