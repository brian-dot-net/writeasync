// <copyright file="Division.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Division
    {
        [InlineData("1/2", "Divide(Literal(1), Literal(2))")]
        [InlineData("X/234", "Divide(NumericVariable(X), Literal(234))")]
        [InlineData("X(234)/YZ1234", "Divide(Array(NumericVariable(X), Literal(234)), NumericVariable(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("2/\"1\"")]
        [InlineData("234/X$")]
        [InlineData("X(234)/YZ1234$")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("(1/2)", "Divide(Literal(1), Literal(2))")]
        [InlineData("(X/234)", "Divide(NumericVariable(X), Literal(234))")]
        [InlineData("(X(234)/YZ1234)", "Divide(Array(NumericVariable(X), Literal(234)), NumericVariable(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1/2/3", "Divide(Divide(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1/2/3)", "Divide(Divide(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1/2)/3", "Divide(Divide(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("1/(2/3)", "Divide(Literal(1), Divide(Literal(2), Literal(3)))")]
        [Theory]
        public void WithThreeTerms(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
