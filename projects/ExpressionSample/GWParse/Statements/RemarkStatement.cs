// <copyright file="RemarkStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    using System.Text;

    internal sealed class RemarkStatement : BasicStatement
    {
        private readonly string text;

        public RemarkStatement(string text)
        {
            this.text = text;
        }

        public override void Accept(IStatementVisitor visit)
        {
            visit.Remark(this.text);
        }
    }
}