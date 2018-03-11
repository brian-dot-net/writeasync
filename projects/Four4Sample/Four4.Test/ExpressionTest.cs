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
            Expression expr = new Expression();

            expr.ToString().Should().Be(string.Empty);
        }
    }
}
