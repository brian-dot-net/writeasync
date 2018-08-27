// <copyright file="RootTermTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace RootSample.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class RootTermTest
    {
        [Theory]
        [InlineData(0, "0")]
        [InlineData(1, "1")]
        [InlineData(4, "2")]
        [InlineData(9, "3")]
        [InlineData(65536, "256")]
        [InlineData(1073741824, "32768")]
        [InlineData(2147395600, "46340")]
        public void PerfectSquares(int n, string expected)
        {
            new RootTerm(n).ToString().Should().Be(expected, "sqrt({0}) is {1}", n, expected);
        }
    }
}