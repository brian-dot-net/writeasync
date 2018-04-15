// <copyright file="Next.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Statements
{
    using Xunit;

    public sealed class Next
    {
        [InlineData("NEXT", "Next()")]
        [Theory]
        public void Empty(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("next", "Next()")]
        [InlineData("NeXT", "Next()")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData(" NEXT", "Next()")]
        [InlineData("NEXT ", "Next()")]
        [InlineData(" NEXT  ", "Next()")]
        [Theory]
        public void IgnoreSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("NEXTA")]
        [InlineData("NEXT1")]
        [Theory]
        public void Invalid(string input)
        {
            Test.Bad(input);
        }
    }
}
