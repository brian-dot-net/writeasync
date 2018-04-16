// <copyright file="BasicLine.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Lines
{
    using GWParse.Statements;

    public sealed class BasicLine
    {
        private readonly int number;
        private readonly BasicStatement stmt;

        public BasicLine(int number, BasicStatement stmt)
        {
            this.number = number;
            this.stmt = stmt;
        }

        public static BasicLine FromString(string input) => Line.FromString(input);

        public override string ToString() => "Line(" + this.number + ", " + this.stmt + ")";
    }
}
