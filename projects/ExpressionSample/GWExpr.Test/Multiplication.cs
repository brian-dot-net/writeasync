// <copyright file="Multiplication.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Multiplication
    {
        [InlineData("1*2", "Multiply(Literal(1), Literal(2))")]
        [InlineData("X*234", "Multiply(NumVar(X), Literal(234))")]
        [InlineData("X(234)*YZ1234", "Multiply(Array(NumVar(X), Literal(234)), NumVar(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("2*\"1\"")]
        [InlineData("234*X$")]
        [InlineData("X(234)*YZ1234$")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("(1*2)", "Multiply(Literal(1), Literal(2))")]
        [InlineData("(X*234)", "Multiply(NumVar(X), Literal(234))")]
        [InlineData("(X(234)*YZ1234)", "Multiply(Array(NumVar(X), Literal(234)), NumVar(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1*2*3", "Multiply(Multiply(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1*2*3)", "Multiply(Multiply(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1*2)*3", "Multiply(Multiply(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("1*(2*3)", "Multiply(Literal(1), Multiply(Literal(2), Literal(3)))")]
        [Theory]
        public void WithThreeTerms(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
