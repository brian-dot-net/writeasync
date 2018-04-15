// <copyright file="Dim.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Statements
{
    using Xunit;

    public sealed class Dim
    {
        [InlineData("DIM R(1)", "Dim(NumA(R, NumL(1)))")]
        [InlineData("DIM R(1,2)", "Dim(NumA(R, NumL(1), NumL(2)))")]
        [Theory]
        public void OneItem(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("DIM R(1),S$(2)", "Dim(NumA(R, NumL(1)), StrA(S, NumL(2)))")]
        [InlineData("DIM R(1,2),S$(A)", "Dim(NumA(R, NumL(1), NumL(2)), StrA(S, NumV(A)))")]
        [Theory]
        public void TwoItems(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("DIM R(1),S$(2),A12(3)", "Dim(NumA(R, NumL(1)), StrA(S, NumL(2)), NumA(A12, NumL(3)))")]
        [InlineData("DIM ST$(1,2,3),F(5),SIX$(7)", "Dim(StrA(ST, NumL(1), NumL(2), NumL(3)), NumA(F, NumL(5)), StrA(SIX, NumL(7)))")]
        [Theory]
        public void ThreeItems(string input, string output)
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
        [InlineData("DIM R( 1 , 2 )", "Dim(NumA(R, NumL(1), NumL(2)))")]
        [InlineData("DIM R( 1 , 2 ), A(1)", "Dim(NumA(R, NumL(1), NumL(2)), NumA(A, NumL(1)))")]
        [InlineData("DIM R( 1 , 2 ) ,A(1)", "Dim(NumA(R, NumL(1), NumL(2)), NumA(A, NumL(1)))")]
        [InlineData("  DIM R( 1 , 2 )  ,  A(1)  ", "Dim(NumA(R, NumL(1), NumL(2)), NumA(A, NumL(1)))")]
        [Theory]
        public void AllowSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("DIM")]
        [InlineData("DIM R")]
        [InlineData("DIM R,")]
        [InlineData("DIM R(")]
        [InlineData("DIM R(1")]
        [InlineData("DIM R(\"1\")")]
        [InlineData("DIM R(1,")]
        [InlineData("DIM R(1,2")]
        [InlineData("DIM R(1),")]
        [Theory]
        public void Invalid(string input)
        {
            Test.Bad(input);
        }
    }
}
