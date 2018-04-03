// <copyright file="Arrays.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class Arrays
    {
        [InlineData("A(1)", "ArrayVariable(NumericVariable(A), NumericLiteral(1))")]
        [Theory]
        public void WithLiteralIndex1D(string input, string output)
        {
            Test(input, output);
        }

        private static void Test(string input, string output)
        {
            BasicExpression.FromString(input).ToString().Should().Be(output);
        }
    }
}
