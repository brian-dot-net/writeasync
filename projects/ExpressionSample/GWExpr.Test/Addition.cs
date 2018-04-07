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
