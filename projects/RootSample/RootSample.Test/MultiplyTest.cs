// <copyright file="MultiplyTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace RootSample.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class MultiplyTest
    {
        [Theory]
        [InlineData(0, 1, "0")]
        [InlineData(1, 4, "2")]
        [InlineData(4, 9, "6")]
        [InlineData(1, 1073741824, "32768")]
        public void PerfectSquares(int a, int b, string expected) => Test(a, b, expected);

        private static void Test(int a, int b, string expected)
        {
            Test(RootTerm.Sqrt(a), RootTerm.Sqrt(b), expected);
            Test(RootTerm.Sqrt(b), RootTerm.Sqrt(a), expected);
        }

        private static void Test(RootTerm a, RootTerm b, string expected)
        {
            a.Multiply(b).ToString().Should().Be(expected, "{0} * {1} = {2}", a, b, expected);
        }
    }
}
