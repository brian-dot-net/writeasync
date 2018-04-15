// <copyright file="Print.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Statements
{
    using Xunit;

    public sealed class Print
    {
        [InlineData("PRINT", "Print()")]
        [Theory]
        public void Empty(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
