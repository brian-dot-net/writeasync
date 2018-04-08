// <copyright file="Negation.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Negation
    {
        [InlineData("-1", "Negate(Literal(1))")]
        [InlineData("-X", "Negate(NumVar(X))")]
        [InlineData("-X(234)", "Negate(Array(NumVar(X), Literal(234)))")]
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

        [InlineData("-(1)", "Negate(Literal(1))")]
        [InlineData("-(-(X))", "Negate(Negate(NumVar(X)))")]
        [InlineData("(-X)", "Negate(NumVar(X))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+(-2)", "Add(Literal(1), Negate(Literal(2)))")]
        [InlineData("-1+2", "Add(Negate(Literal(1)), Literal(2))")]
        [InlineData("1-(-2)", "Subtract(Literal(1), Negate(Literal(2)))")]
        [InlineData("-1-2", "Subtract(Negate(Literal(1)), Literal(2))")]
        [InlineData("1*(-2)", "Multiply(Literal(1), Negate(Literal(2)))")]
        [InlineData("-1*2", "Multiply(Negate(Literal(1)), Literal(2))")]
        [InlineData("1/(-2)", "Divide(Literal(1), Negate(Literal(2)))")]
        [InlineData("-1/2", "Divide(Negate(Literal(1)), Literal(2))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
