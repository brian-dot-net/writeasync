// <copyright file="VoidStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    internal sealed class VoidStatement : BasicStatement
    {
        private readonly string name;

        public VoidStatement(string name)
        {
            this.name = name;
        }

        public override string ToString() => this.name + "()";
    }
}