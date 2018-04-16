// <copyright file="Data.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Statements
{
    using Xunit;

    public sealed class Data
    {
        [InlineData("DATA x", "Data(StrL(\"x\"))")]
        [InlineData("DATA X123", "Data(StrL(\"X123\"))")]
        [InlineData("DATA A B C", "Data(StrL(\"A B C\"))")]
        [Theory]
        public void OneString(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("data x", "Data(StrL(\"x\"))")]
        [InlineData("DaTa X", "Data(StrL(\"X\"))")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData(" DATA x", "Data(StrL(\"x\"))")]
        [InlineData("DATA x ", "Data(StrL(\"x\"))")]
        [InlineData("  DATA  x  ", "Data(StrL(\"x\"))")]
        [InlineData("  DATA  x  y  ", "Data(StrL(\"x  y\"))")]
        [Theory]
        public void AllowSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("DATA")]
        [InlineData("DATA ")]
        [Theory]
        public void Invalid(string input)
        {
            Test.Bad(input);
        }
    }
}
