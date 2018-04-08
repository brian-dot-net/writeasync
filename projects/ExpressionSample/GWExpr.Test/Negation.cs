// <copyright file="Negation.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Negation
    {
        [InlineData("-1", "Negate(NumericLiteral(1))")]
        [InlineData("-X", "Negate(NumericVariable(X))")]
        [InlineData("-X(234)", "Negate(Array(NumericVariable(X), NumericLiteral(234)))")]
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

        [InlineData("-(1)", "Negate(NumericLiteral(1))")]
        [InlineData("-(-(X))", "Negate(Negate(NumericVariable(X)))")]
        [InlineData("(-X)", "Negate(NumericVariable(X))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+(-2)", "Add(NumericLiteral(1), Negate(NumericLiteral(2)))")]
        [InlineData("-1+2", "Add(Negate(NumericLiteral(1)), NumericLiteral(2))")]
        [InlineData("1-(-2)", "Subtract(NumericLiteral(1), Negate(NumericLiteral(2)))")]
        [InlineData("-1-2", "Subtract(Negate(NumericLiteral(1)), NumericLiteral(2))")]
        [InlineData("1*(-2)", "Multiply(NumericLiteral(1), Negate(NumericLiteral(2)))")]
        [InlineData("-1*2", "Multiply(Negate(NumericLiteral(1)), NumericLiteral(2))")]
        [InlineData("1/(-2)", "Divide(NumericLiteral(1), Negate(NumericLiteral(2)))")]
        [InlineData("-1/2", "Divide(Negate(NumericLiteral(1)), NumericLiteral(2))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
