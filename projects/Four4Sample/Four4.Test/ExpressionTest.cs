// <copyright file="ExpressionTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class ExpressionTest
    {
        [Fact]
        public void Empty()
        {
            Expression expr = default(Expression);

            expr.ToString().Should().Be(string.Empty);
        }

        [Theory]
        [InlineData("+", "+")]
        public void AppendToEmpty(string input, string result)
        {
            Expression expr = default(Expression);

            expr = expr.Append(input);

            expr.ToString().Should().Be(result);
        }
    }
}
