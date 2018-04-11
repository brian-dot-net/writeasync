// <copyright file="Negation.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Negation
    {
        [InlineData("-1", "Neg(L(1))")]
        [InlineData("-X", "Neg(NumVar(X))")]
        [InlineData("-X(234)", "Neg(Array(NumVar(X), L(234)))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("-\"1\"")]
        [InlineData("-X$")]
        [InlineData("-YZ1234$")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("-(1)", "Neg(L(1))")]
        [InlineData("-(-(X))", "Neg(Neg(NumVar(X)))")]
        [InlineData("(-X)", "Neg(NumVar(X))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+(-2)", "Add(L(1), Neg(L(2)))")]
        [InlineData("-1+2", "Add(Neg(L(1)), L(2))")]
        [InlineData("1-(-2)", "Sub(L(1), Neg(L(2)))")]
        [InlineData("-1-2", "Sub(Neg(L(1)), L(2))")]
        [InlineData("1*(-2)", "Mult(L(1), Neg(L(2)))")]
        [InlineData("-1*2", "Mult(Neg(L(1)), L(2))")]
        [InlineData("1/(-2)", "Div(L(1), Neg(L(2)))")]
        [InlineData("-1/2", "Div(Neg(L(1)), L(2))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
