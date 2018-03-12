// <copyright file="ResultsTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class ResultsTest
    {
        [Fact]
        public void NegativeNumbers()
        {
            Results results = new Results();
            Expression expr = default(Expression).Append("4").Append("444").Append("-");

            results.Add(expr);

            results.Count.Should().Be(0);
        }
    }
}
