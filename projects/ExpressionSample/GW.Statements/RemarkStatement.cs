// <copyright file="RemarkStatement.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GW.Statements
{
    using System.Text;

    internal sealed class RemarkStatement : BasicStatement
    {
        private readonly string text;

        public RemarkStatement(string text)
        {
            this.text = text;
        }

        public override string ToString() => "Rem(\"" + this.Escape() + "\")";

        private string Escape()
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in this.text)
            {
                switch (c)
                {
                    case '"':
                    case '\\':
                        sb.Append('\\');
                        break;
                }

                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}