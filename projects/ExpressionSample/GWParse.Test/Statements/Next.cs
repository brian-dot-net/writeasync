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

        [InlineData("NEXT I", "Next(NumV(I))")]
        [Theory]
        public void OneVar(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("next", "Next()")]
        [InlineData("NeXT", "Next()")]
        [InlineData("next i", "Next(NumV(I))")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData(" NEXT", "Next()")]
        [InlineData("NEXT ", "Next()")]
        [InlineData(" NEXT  ", "Next()")]
        [InlineData(" NEXT  I  ", "Next(NumV(I))")]
        [Theory]
        public void IgnoreSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("NEXTA")]
        [InlineData("NEXT1")]
        [InlineData("NEXT A$")]
        [InlineData("NEXT 1")]
        [InlineData("NEXT \"x\"")]
        [Theory]
        public void Invalid(string input)
        {
            Test.Bad(input);
        }
    }
}
