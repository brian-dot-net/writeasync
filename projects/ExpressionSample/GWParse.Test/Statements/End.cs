// <copyright file="End.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Statements
{
    using Xunit;

    public sealed class End
    {
        [InlineData("END", "End()")]
        [Theory]
        public void Valid(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("end", "End()")]
        [InlineData("EnD", "End()")]
        [InlineData("eNd", "End()")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData(" END", "End()")]
        [InlineData("END ", "End()")]
        [InlineData("  END  ", "End()")]
        [Theory]
        public void AllowSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("ENDING")]
        [InlineData("END Please")]
        [Theory]
        public void Invalid(string input)
        {
            Test.Bad(input);
        }
    }
}
