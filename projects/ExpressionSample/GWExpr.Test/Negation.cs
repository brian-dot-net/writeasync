// <copyright file="Negation.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Negation
    {
        [InlineData("-1", "Neg(NumL(1))")]
        [InlineData("-X", "Neg(NumVar(X))")]
        [InlineData("-X(234)", "Neg(Array(NumVar(X), NumL(234)))")]
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

        [InlineData("-(1)", "Neg(NumL(1))")]
        [InlineData("-(-(X))", "Neg(Neg(NumVar(X)))")]
        [InlineData("(-X)", "Neg(NumVar(X))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+(-2)", "Add(NumL(1), Neg(NumL(2)))")]
        [InlineData("-1+2", "Add(Neg(NumL(1)), NumL(2))")]
        [InlineData("1-(-2)", "Sub(NumL(1), Neg(NumL(2)))")]
        [InlineData("-1-2", "Sub(Neg(NumL(1)), NumL(2))")]
        [InlineData("1*(-2)", "Mult(NumL(1), Neg(NumL(2)))")]
        [InlineData("-1*2", "Mult(Neg(NumL(1)), NumL(2))")]
        [InlineData("1/(-2)", "Div(NumL(1), Neg(NumL(2)))")]
        [InlineData("-1/2", "Div(Neg(NumL(1)), NumL(2))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
