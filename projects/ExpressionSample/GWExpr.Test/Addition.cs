// <copyright file="Addition.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class Addition
    {
        [InlineData("1+2", "Add(NumericLiteral(1), NumericLiteral(2))")]
        [InlineData("X+234", "Add(NumericVariable(X), NumericLiteral(234))")]
        [InlineData("X(234)+YZ1234", "Add(Array(NumericVariable(X), NumericLiteral(234)), NumericVariable(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test(input, output);
        }

        private static void Test(string input, string output)
        {
            BasicExpression.FromString(input).ToString().Should().Be(output);
        }
    }
}
