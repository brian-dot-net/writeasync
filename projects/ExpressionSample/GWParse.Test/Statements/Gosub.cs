// <copyright file="Gosub.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Statements
{
    using Xunit;

    public sealed class Gosub
    {
        [InlineData("GOSUB 1", "Gosub(1)")]
        [InlineData("GOSUB 23", "Gosub(23)")]
        [InlineData("GOSUB 45678", "Gosub(45678)")]
        [Theory]
        public void Valid(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("gosub 1", "Gosub(1)")]
        [InlineData("GoSuB 1", "Gosub(1)")]
        [InlineData("gOsUb 1", "Gosub(1)")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData(" GOSUB 1", "Gosub(1)")]
        [InlineData("GOSUB  1", "Gosub(1)")]
        [InlineData("  GOSUB  1  ", "Gosub(1)")]
        [Theory]
        public void AllowSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("GOSUB")]
        [InlineData("GOSUB A")]
        [InlineData("GOSUB 1A")]
        [InlineData("GOSUB A$")]
        [InlineData("GOSUB \"1\"")]
        [Theory]
        public void Invalid(string input)
        {
            Test.Bad(input);
        }
    }
}
