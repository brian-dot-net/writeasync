// <copyright file="Subtraction.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Subtraction
    {
        [InlineData("1-2", "Subtract(NumericLiteral(1), NumericLiteral(2))")]
        [InlineData("X-234", "Subtract(NumericVariable(X), NumericLiteral(234))")]
        [InlineData("X(234)-YZ1234", "Subtract(Array(NumericVariable(X), NumericLiteral(234)), NumericVariable(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("2-\"1\"")]
        [InlineData("234-X$")]
        [InlineData("X(234)-YZ1234$")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }
    }
}
