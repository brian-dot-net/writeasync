// <copyright file="Precedence.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Precedence
    {
        [InlineData("1+2*3", "Add(Literal(1), Multiply(Literal(2), Literal(3)))")]
        [InlineData("1+2/3", "Add(Literal(1), Divide(Literal(2), Literal(3)))")]
        [InlineData("1-2*3", "Subtract(Literal(1), Multiply(Literal(2), Literal(3)))")]
        [InlineData("1-2/3", "Subtract(Literal(1), Divide(Literal(2), Literal(3)))")]
        [InlineData("(1+2)/3", "Divide(Add(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1-2)/3", "Divide(Subtract(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1+2)*3", "Multiply(Add(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1-2)*3", "Multiply(Subtract(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1*2)/3", "Divide(Multiply(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1*2)+3", "Add(Multiply(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1/2)-3", "Subtract(Divide(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("1*2^3", "Multiply(Literal(1), Pow(Literal(2), Literal(3)))")]
        [InlineData("1^2/3", "Divide(Pow(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("1^2+3", "Add(Pow(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("1^2-3", "Subtract(Pow(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("1+2^3", "Add(Literal(1), Pow(Literal(2), Literal(3)))")]
        [InlineData("1-2^3", "Subtract(Literal(1), Pow(Literal(2), Literal(3)))")]
        [InlineData("(-1)^2+3", "Add(Pow(Negate(Literal(1)), Literal(2)), Literal(3))")]
        [InlineData("1^-2+3", "Add(Pow(Literal(1), Negate(Literal(2))), Literal(3))")]
        [InlineData("1^(-2)+3", "Add(Pow(Literal(1), Negate(Literal(2))), Literal(3))")]
        [InlineData("-1^2+3", "Add(Negate(Pow(Literal(1), Literal(2)), Literal(3))", Skip = "Unary minus bug :(")]
        [Theory]
        public void Arithmetic(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
