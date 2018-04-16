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

        [InlineData("DATA \"\"", "Data(StrL(\"\"))")]
        [InlineData("DATA \"x\"", "Data(StrL(\"x\"))")]
        [InlineData("DATA \"X123\"", "Data(StrL(\"X123\"))")]
        [InlineData("DATA \" A B C \"", "Data(StrL(\" A B C \"))")]
        [Theory]
        public void OneQuotedString(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("DATA 1", "Data(NumL(1))")]
        [InlineData("DATA 123", "Data(NumL(123))")]
        [InlineData("DATA 999999", "Data(NumL(999999))")]
        [Theory]
        public void OneNumber(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("data x", "Data(StrL(\"x\"))")]
        [InlineData("DaTa X", "Data(StrL(\"X\"))")]
        [InlineData("daTA 1", "Data(NumL(1))")]
        [InlineData("daTA \"x\"", "Data(StrL(\"x\"))")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData(" DATA x", "Data(StrL(\"x\"))")]
        [InlineData("DATA x ", "Data(StrL(\"x\"))")]
        [InlineData("  DATA  x  ", "Data(StrL(\"x\"))")]
        [InlineData("  DATA  x  y  ", "Data(StrL(\"x  y\"))")]
        [InlineData("  DATA  1  ", "Data(NumL(1))")]
        [InlineData("  DATA  \"x\"  ", "Data(StrL(\"x\"))")]
        [Theory]
        public void AllowSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("DATA")]
        [InlineData("DATA ")]
        [InlineData("DATA 1x")]
        [InlineData("DATA \"")]
        [InlineData("DATA \"x")]
        [InlineData("DATA x\"")]
        [Theory]
        public void Invalid(string input)
        {
            Test.Bad(input);
        }
    }
}
