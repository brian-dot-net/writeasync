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

        [InlineData("INPUT \"\";A", "Input(\"\", NumV(A))")]
        [InlineData("INPUT \"p\";A$", "Input(\"p\", StrV(A))")]
        [InlineData("INPUT \"input please\";A(1)", "Input(\"input please\", NumA(A, NumL(1)))")]
        [InlineData("INPUT \" some thing \";A$(1)", "Input(\" some thing \", StrA(A, NumL(1)))")]
        [Theory]
        public void VarAndPrompt(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("input A", "Input(\"\", NumV(A))")]
        [InlineData("InPuT a", "Input(\"\", NumV(A))")]
        [InlineData("InPut \"prompt\";a", "Input(\"prompt\", NumV(A))")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("  INPUT A", "Input(\"\", NumV(A))")]
        [InlineData("INPUT A  ", "Input(\"\", NumV(A))")]
        [InlineData("  INPUT  A  ", "Input(\"\", NumV(A))")]
        [InlineData("  INPUT  \"  a  b  \"  ;  A  ", "Input(\"  a  b  \", NumV(A))")]
        [Theory]
        public void IgnoreSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("INPUTA")]
        [InlineData("INPUT1")]
        [InlineData("INPUT\"x\";A")]
        [InlineData("INPUT \"x\"")]
        [InlineData("INPUT \"x\";")]
        [Theory]
        public void Invalid(string input)
        {
            Test.Bad(input);
        }
    }
}
