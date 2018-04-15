// <copyright file="Input.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Statements
{
    using Xunit;

    public sealed class Input
    {
        [InlineData("INPUT A", "Input(\"\", NumV(A))")]
        [InlineData("INPUT A$", "Input(\"\", StrV(A))")]
        [InlineData("INPUT A(1)", "Input(\"\", NumA(A, NumL(1)))")]
        [InlineData("INPUT A$(1)", "Input(\"\", StrA(A, NumL(1)))")]
        [Theory]
        public void VarOnly(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("input A", "Input(\"\", NumV(A))")]
        [InlineData("InPuT a", "Input(\"\", NumV(A))")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("  INPUT A", "Input(\"\", NumV(A))")]
        [InlineData("INPUT A  ", "Input(\"\", NumV(A))")]
        [InlineData("  INPUT  A  ", "Input(\"\", NumV(A))")]
        [Theory]
        public void IgnoreSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("INPUTA")]
        [InlineData("INPUT1")]
        [Theory]
        public void Invalid(string input)
        {
            Test.Bad(input);
        }
    }
}
