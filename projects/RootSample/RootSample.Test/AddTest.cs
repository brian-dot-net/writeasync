// <copyright file="AddTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace RootSample.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class AddTest
    {
        [Theory]
        [InlineData(0, 1, "1")]
        [InlineData(1, 4, "3")]
        [InlineData(4, 9, "5")]
        [InlineData(1, 1073741824, "32769")]
        public void PerfectSquares(int a, int b, string expected) => Test(a, b, expected);

        [Theory]
        [InlineData(0, -1, "i")]
        [InlineData(-1, -4, "3*i")]
        [InlineData(-4, -9, "5*i")]
        [InlineData(-1, -1073741824, "32769*i")]
        public void PerfectSquaresNeg(int a, int b, string expected) => Test(a, b, expected);

        private static void Test(int a, int b, string expected)
        {
            Test(RootTerm.Sqrt(a), RootTerm.Sqrt(b), expected);
            Test(RootTerm.Sqrt(b), RootTerm.Sqrt(a), expected);
        }

        private static void Test(RootTerm a, RootTerm b, string expected)
        {
            a.Add(b).ToString().Should().Be(expected, "{0} + {1} = {2}", a, b, expected);
        }
    }
}
