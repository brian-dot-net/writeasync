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

        [Theory]
        [InlineData(8, "2*sqrt(2)")]
        [InlineData(24, "2*sqrt(6)")]
        [InlineData(262084, "2*sqrt(65521)")]
        [InlineData(2147483644, "2*sqrt(536870911)")]
        public void FactorsOfFour(int n, string expected)
        {
            new RootTerm(n).ToString().Should().Be(expected, "sqrt({0}) is {1}", n, expected);
        }

        [Theory]
        [InlineData(18, "3*sqrt(2)")]
        [InlineData(243, "9*sqrt(3)")]
        [InlineData(2147483646, "3*sqrt(238609294)")]
        public void FactorsOfNine(int n, string expected)
        {
            new RootTerm(n).ToString().Should().Be(expected, "sqrt({0}) is {1}", n, expected);
        }

        [Theory]
        [InlineData(50, "5*sqrt(2)")]
        [InlineData(1220703125, "15625*sqrt(5)")]
        [InlineData(2147483625, "5*sqrt(85899345)")]
        public void FactorsOfTwentyFive(int n, string expected)
        {
            new RootTerm(n).ToString().Should().Be(expected, "sqrt({0}) is {1}", n, expected);
        }

        [Theory]
        [InlineData(6, "sqrt(6)")]
        [InlineData(15, "sqrt(15)")]
        [InlineData(10002, "sqrt(10002)")]
        [InlineData(223092870, "sqrt(223092870)")]
        [InlineData(2147483642, "sqrt(2147483642)")]
        public void IrreducibleComposite(int n, string expected)
        {
            new RootTerm(n).ToString().Should().Be(expected, "sqrt({0}) is {1}", n, expected);
        }

        [Theory]
        [InlineData(2, "sqrt(2)")]
        [InlineData(3, "sqrt(3)")]
        [InlineData(65521, "sqrt(65521)")]
        [InlineData(2147483647, "sqrt(2147483647)")]
        public void IrreduciblePrime(int n, string expected)
        {
            new RootTerm(n).ToString().Should().Be(expected, "sqrt({0}) is {1}", n, expected);
        }

        [Theory]
        [InlineData(1536192006, "16001*sqrt(6)")]
        [InlineData(2144601507, "26737*sqrt(3)")]
        [InlineData(2144994002, "32749*sqrt(2)")]
        public void LargePrimeSquareFactors(int n, string expected)
        {
            new RootTerm(n).ToString().Should().Be(expected, "sqrt({0}) is {1}", n, expected);
        }
    }
}
