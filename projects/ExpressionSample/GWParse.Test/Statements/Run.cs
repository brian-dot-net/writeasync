// <copyright file="Run.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Statements
{
    using Xunit;

    public sealed class Run
    {
        [InlineData("RUN", "Run()")]
        [Theory]
        public void Valid(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("run", "Run()")]
        [InlineData("RuN", "Run()")]
        [InlineData("rUn", "Run()")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData(" RUN", "Run()")]
        [InlineData("RUN ", "Run()")]
        [InlineData("  RUN  ", "Run()")]
        [Theory]
        public void AllowSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("RUNNING")]
        [InlineData("RUN Please")]
        [Theory]
        public void Invalid(string input)
        {
            Test.Bad(input);
        }
    }
}
