// <copyright file="WrappedWriter.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System.IO;
    using System.Text;

    internal sealed class WrappedWriter : TextWriter
    {
        private readonly TextWriter inner;
        private readonly StringBuilder text;

        public WrappedWriter(TextWriter inner, StringBuilder text)
        {
            this.inner = inner;
            this.text = text;
        }

        public override Encoding Encoding => this.inner.Encoding;

        public override void Write(char value)
        {
            this.text.Append(value);
            this.inner.Write(value);
        }

        public override void Write(string value)
        {
            this.text.Append(value);
            this.inner.Write(value);
        }

        public override void WriteLine(string value)
        {
            this.text.AppendLine(value);
            this.inner.WriteLine(value);
        }
    }
}
