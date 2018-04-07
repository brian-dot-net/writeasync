// <copyright file="Multiplication.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Multiplication
    {
        [InlineData("1*2", "Multiply(NumericLiteral(1), NumericLiteral(2))")]
        [InlineData("X*234", "Multiply(NumericVariable(X), NumericLiteral(234))")]
        [InlineData("X(234)*YZ1234", "Multiply(Array(NumericVariable(X), NumericLiteral(234)), NumericVariable(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("2*\"1\"")]
        [InlineData("234*X$")]
        [InlineData("X(234)*YZ1234$")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("(1*2)", "Multiply(NumericLiteral(1), NumericLiteral(2))")]
        [InlineData("(X*234)", "Multiply(NumericVariable(X), NumericLiteral(234))")]
        [InlineData("(X(234)*YZ1234)", "Multiply(Array(NumericVariable(X), NumericLiteral(234)), NumericVariable(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
