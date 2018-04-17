// <copyright file="Exponentiation.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Expressions
{
    using Xunit;

    public sealed class Exponentiation
    {
        [InlineData("1^2", "Pow(NumL(1), NumL(2))")]
        [InlineData("X^234", "Pow(NumV(X), NumL(234))")]
        [InlineData("X(234)^YZ1234", "Pow(NumA(X, NumL(234)), NumV(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1 ^2", "Pow(NumL(1), NumL(2))")]
        [InlineData("X^ 234", "Pow(NumV(X), NumL(234))")]
        [InlineData("X(234)  ^  YZ1234", "Pow(NumA(X, NumL(234)), NumV(YZ1234))")]
        [Theory]
        public void IgnoreSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1^\"x\"")]
        [InlineData("\"x\"^1")]
        [InlineData("X^X$")]
        [InlineData("X$^X")]
        [InlineData("X^\"x\"")]
        [InlineData("\"x\"^X")]
        [InlineData("1^X$")]
        [InlineData("X$^1")]
        [InlineData("X$(1)^X(1)")]
        [InlineData("X(1)^X$(1)")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("(1^2)", "Pow(NumL(1), NumL(2))")]
        [InlineData("(X^234)", "Pow(NumV(X), NumL(234))")]
        [InlineData("(X(234)^YZ1234)", "Pow(NumA(X, NumL(234)), NumV(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1^2^3", "Pow(Pow(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1^2^3)", "Pow(Pow(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1^2)^3", "Pow(Pow(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("1^(2^3)", "Pow(NumL(1), Pow(NumL(2), NumL(3)))")]
        [Theory]
        public void WithThreeTerms(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
