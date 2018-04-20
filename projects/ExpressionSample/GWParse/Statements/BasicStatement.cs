// <copyright file="BasicStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    using System.Text;
    using GWParse.Expressions;

    public abstract class BasicStatement
    {
        protected BasicStatement()
        {
        }

        public static BasicStatement FromString(string input) => Stmt.FromString(input);

        public abstract void Accept(IStatementVisitor visit);

        public override string ToString()
        {
            StatementString str = new StatementString();
            this.Accept(str);
            return str.ToString();
        }

        private sealed class StatementString : IStatementVisitor
        {
            private readonly StringBuilder sb;

            public StatementString()
            {
                this.sb = new StringBuilder();
            }

            public void Assign(BasicExpression left, BasicExpression right)
            {
                this.Write("Assign", left, right);
            }

            public void For(BasicExpression v, BasicExpression start, BasicExpression end, BasicExpression step)
            {
                this.Write("For", v, start, end, step);
            }

            public void Go(string name, int dest)
            {
                this.sb.Append(name).Append("(").Append(dest).Append(")");
            }

            public void IfThen(BasicExpression cond, BasicStatement ifTrue)
            {
                this.sb.Append("If(").Append(cond).Append(", ").Append(ifTrue).Append(")");
            }

            public void Input(string prompt, BasicExpression v)
            {
                this.sb.Append("Input(\"").Append(prompt).Append("\", ").Append(v).Append(")");
            }

            public void Many(string name, BasicExpression[] list) => this.Write(name, list);

            public void Remark(string text)
            {
                this.sb.Append("Rem(\"");

                foreach (char c in text)
                {
                    switch (c)
                    {
                        case '"':
                        case '\\':
                            this.sb.Append('\\');
                            break;
                    }

                    this.sb.Append(c);
                }

                this.sb.Append("\")");
            }

            public void Void(string name) => this.Write(name);

            public override string ToString() => this.sb.ToString();

            private void Write(string name, params BasicExpression[] list)
            {
                this.sb.Append(name).Append("(");

                foreach (BasicExpression expr in list)
                {
                    this.sb.Append(expr.ToString());
                    this.sb.Append(", ");
                }

                if (list.Length > 0)
                {
                    this.sb.Remove(this.sb.Length - 2, 2);
                }

                this.sb.Append(")");
            }
        }
    }
}
