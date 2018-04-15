// <copyright file="Return.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Statements
{
    using Xunit;

    public sealed class Return
    {
        [InlineData("RETURN", "Return()")]
        [Theory]
        public void Valid(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("return", "Return()")]
        [InlineData("ReTURN", "Return()")]
        [InlineData("rEtUrN", "Return()")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData(" RETURN", "Return()")]
        [InlineData("RETURN ", "Return()")]
        [InlineData("  RETURN  ", "Return()")]
        [Theory]
        public void AllowSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("RETURN1")]
        [InlineData("RETURN Please")]
        [Theory]
        public void Invalid(string input)
        {
            Test.Bad(input);
        }
    }
}
