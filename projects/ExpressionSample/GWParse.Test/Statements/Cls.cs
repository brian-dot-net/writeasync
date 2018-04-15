// <copyright file="Cls.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Statements
{
    using Xunit;

    public sealed class Cls
    {
        [InlineData("CLS", "Cls()")]
        [Theory]
        public void Valid(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("cls", "Cls()")]
        [InlineData("ClS", "Cls()")]
        [InlineData("cLs", "Cls()")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData(" CLS", "Cls()")]
        [InlineData("CLS ", "Cls()")]
        [InlineData("  CLS  ", "Cls()")]
        [Theory]
        public void AllowSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("CLSCRN")]
        [InlineData("CLS Please")]
        [Theory]
        public void Invalid(string input)
        {
            Test.Bad(input);
        }
    }
}
