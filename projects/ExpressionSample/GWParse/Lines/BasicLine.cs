// <copyright file="BasicLine.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Lines
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using GWParse.Statements;

    public sealed class BasicLine
    {
        private readonly int number;
        private readonly BasicStatement[] list;

        public BasicLine(int number, IEnumerable<BasicStatement> list)
        {
            this.number = number;
            this.list = list.ToArray();
        }

        public static BasicLine FromString(string input) => Line.FromString(input);

        public void Accept(ILineVisitor visit)
        {
            visit.Line(this.number, this.list);
        }

        public override string ToString()
        {
            LineString str = new LineString();
            this.Accept(str);
            return str.ToString();
        }

        private sealed class LineString : ILineVisitor
        {
            private readonly StringBuilder sb;

            public LineString()
            {
                this.sb = new StringBuilder();
            }

            public void Line(int number, BasicStatement[] list)
            {
                this.sb.Append("Line(").Append(number);
                foreach (BasicStatement stmt in list)
                {
                    this.sb.Append(", ");
                    this.sb.Append(stmt.ToString());
                }

                this.sb.Append(")");
            }

            public override string ToString() => this.sb.ToString();
        }
    }
}
