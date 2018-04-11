// <copyright file="Negation.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Negation
    {
        [InlineData("-1", "Negate(L(1))")]
        [InlineData("-X", "Negate(NumVar(X))")]
        [InlineData("-X(234)", "Negate(Array(NumVar(X), L(234)))")]
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

        [InlineData("-(1)", "Negate(L(1))")]
        [InlineData("-(-(X))", "Negate(Negate(NumVar(X)))")]
        [InlineData("(-X)", "Negate(NumVar(X))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+(-2)", "Add(L(1), Negate(L(2)))")]
        [InlineData("-1+2", "Add(Negate(L(1)), L(2))")]
        [InlineData("1-(-2)", "Subtract(L(1), Negate(L(2)))")]
        [InlineData("-1-2", "Subtract(Negate(L(1)), L(2))")]
        [InlineData("1*(-2)", "Multiply(L(1), Negate(L(2)))")]
        [InlineData("-1*2", "Multiply(Negate(L(1)), L(2))")]
        [InlineData("1/(-2)", "Divide(L(1), Negate(L(2)))")]
        [InlineData("-1/2", "Divide(Negate(L(1)), L(2))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
