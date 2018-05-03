// <copyright file="WrappedReader.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System.Collections.Generic;
    using System.IO;

    internal sealed class WrappedReader : TextReader
    {
        private readonly TextReader inner;
        private readonly IList<string> lines;

        public WrappedReader(TextReader inner, IList<string> lines)
        {
            this.inner = inner;
            this.lines = lines;
        }

        public override string ReadLine()
        {
            string line = this.inner.ReadLine();
            this.lines.Add(line);
            return line;
        }
    }
}
