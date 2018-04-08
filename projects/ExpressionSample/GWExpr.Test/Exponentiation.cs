// <copyright file="Exponentiation.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Exponentiation
    {
        [InlineData("1^2", "Pow(Literal(1), Literal(2))")]
        [InlineData("X^234", "Pow(NumVar(X), Literal(234))")]
        [InlineData("X(234)^YZ1234", "Pow(Array(NumVar(X), Literal(234)), NumVar(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("2^\"1\"")]
        [InlineData("234^X$")]
        [InlineData("X(234)^YZ1234$")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("(1^2)", "Pow(Literal(1), Literal(2))")]
        [InlineData("(X^234)", "Pow(NumVar(X), Literal(234))")]
        [InlineData("(X(234)^YZ1234)", "Pow(Array(NumVar(X), Literal(234)), NumVar(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1^2^3", "Pow(Pow(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1^2^3)", "Pow(Pow(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1^2)^3", "Pow(Pow(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("1^(2^3)", "Pow(Literal(1), Pow(Literal(2), Literal(3)))")]
        [Theory]
        public void WithThreeTerms(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
