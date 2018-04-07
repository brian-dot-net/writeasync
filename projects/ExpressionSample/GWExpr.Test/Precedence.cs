// <copyright file="Precedence.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Precedence
    {
        [InlineData("1+2*3", "Add(NumericLiteral(1), Multiply(NumericLiteral(2), NumericLiteral(3)))")]
        [InlineData("1+2/3", "Add(NumericLiteral(1), Divide(NumericLiteral(2), NumericLiteral(3)))")]
        [InlineData("1-2*3", "Subtract(NumericLiteral(1), Multiply(NumericLiteral(2), NumericLiteral(3)))")]
        [InlineData("1-2/3", "Subtract(NumericLiteral(1), Divide(NumericLiteral(2), NumericLiteral(3)))")]
        [Theory]
        public void Arithmetic(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
