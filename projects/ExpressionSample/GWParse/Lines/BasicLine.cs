// <copyright file="BasicLine.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Lines
{
    using System.Collections.Generic;
    using System.Linq;
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

        public override string ToString()
        {
            return "Line(" + this.number + ", " + string.Join<BasicStatement>(", ", this.list) + ")";
        }
    }
}
