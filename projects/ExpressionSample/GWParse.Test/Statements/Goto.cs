// <copyright file="Goto.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Statements
{
    using Xunit;

    public sealed class Goto
    {
        [InlineData("GOTO 1", "Goto(1)")]
        [InlineData("GOTO 23", "Goto(23)")]
        [InlineData("GOTO 45678", "Goto(45678)")]
        [Theory]
        public void Valid(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("goto 1", "Goto(1)")]
        [InlineData("GoTo 1", "Goto(1)")]
        [InlineData("gOtO 1", "Goto(1)")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData(" GOTO 1", "Goto(1)")]
        [InlineData("GOTO  1", "Goto(1)")]
        [InlineData("  GOTO  1  ", "Goto(1)")]
        [Theory]
        public void AllowSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("GOTO")]
        [InlineData("GOTO1")]
        [InlineData("GOTO A")]
        [InlineData("GOTO 1A")]
        [InlineData("GOTO A$")]
        [InlineData("GOTO \"1\"")]
        [Theory]
        public void Invalid(string input)
        {
            Test.Bad(input);
        }
    }
}
