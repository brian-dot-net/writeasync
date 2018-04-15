// <copyright file="Dim.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GW.Statements.Test
{
    using Xunit;

    public sealed class Dim
    {
        [InlineData("DIM R(1)", "Dim(NumA(R, NumL(1)))")]
        [Theory]
        public void Valid(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("dim R(1)", "Dim(NumA(R, NumL(1)))")]
        [InlineData("DiM r(1)", "Dim(NumA(R, NumL(1)))")]
        [InlineData("dIm R(1)", "Dim(NumA(R, NumL(1)))")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData(" DIM R(1)", "Dim(NumA(R, NumL(1)))")]
        [InlineData("DIM  R(1)", "Dim(NumA(R, NumL(1)))")]
        [InlineData("  DIM  R  (  1  )", "Dim(NumA(R, NumL(1)))")]
        [Theory]
        public void AllowSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("DIM")]
        [InlineData("DIM R(")]
        [InlineData("DIM R(1")]
        [InlineData("DIM R(\"1\")")]
        [InlineData("DIM R(1),")]
        [Theory]
        public void Invalid(string input)
        {
            Test.Bad(input);
        }
    }
}
