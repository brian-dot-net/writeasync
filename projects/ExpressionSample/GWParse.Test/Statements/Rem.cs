// <copyright file="Rem.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Statements
{
    using Xunit;

    public sealed class Rem
    {
        [InlineData("REM", "Rem(\"\")")]
        [InlineData("REM hello", "Rem(\"hello\")")]
        [InlineData("REM REM starts here", "Rem(\"REM starts here\")")]
        [InlineData("REM : REM starts here", "Rem(\": REM starts here\")")]
        [InlineData(@"REM ""C:\""", @"Rem(""\""C:\\\"""")")]
        [Theory]
        public void Valid(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("rem", "Rem(\"\")")]
        [InlineData("ReM hello", "Rem(\"hello\")")]
        [InlineData("rEm REM starts here", "Rem(\"REM starts here\")")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("  REM", "Rem(\"\")")]
        [InlineData("REM  ", "Rem(\" \")")]
        [InlineData("REM  hello ", "Rem(\" hello \")")]
        [InlineData(" REM  REM starts here", "Rem(\" REM starts here\")")]
        [Theory]
        public void AllowSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("REMARK")]
        [InlineData("REMnospaces")]
        [Theory]
        public void Invalid(string input)
        {
            Test.Bad(input);
        }
    }
}
