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

        [Theory]
        [InlineData(0, -1, "0")]
        [InlineData(-1, -4, "-2")]
        [InlineData(-4, -9, "-6")]
        [InlineData(-1, -1073741824, "-32768")]
        public void PerfectSquaresNeg(int a, int b, string expected) => Test(a, b, expected);

        [Theory]
        [InlineData(-1, 4, "2*i")]
        [InlineData(-4, 9, "6*i")]
        [InlineData(1, -1073741824, "32768*i")]
        public void PerfectSquaresImag(int a, int b, string expected) => Test(a, b, expected);

        [Theory]
        [InlineData(2, 3, "sqrt(6)")]
        [InlineData(7, 11, "sqrt(77)")]
        [InlineData(32749, 32771, "sqrt(1073217479)")]
        [InlineData(1, 2147483647, "sqrt(2147483647)")]
        public void Primes(int a, int b, string expected) => Test(a, b, expected);

        [Theory]
        [InlineData(-2, -3, "-sqrt(6)")]
        [InlineData(-7, -11, "-sqrt(77)")]
        [InlineData(-32749, -32771, "-sqrt(1073217479)")]
        [InlineData(-1, -2147483647, "-sqrt(2147483647)")]
        public void PrimesNeg(int a, int b, string expected) => Test(a, b, expected);

        [Theory]
        [InlineData(-2, 3, "sqrt(6)*i")]
        [InlineData(7, -11, "sqrt(77)*i")]
        [InlineData(32749, -32771, "sqrt(1073217479)*i")]
        [InlineData(-1, 2147483647, "sqrt(2147483647)*i")]
        public void PrimesImag(int a, int b, string expected) => Test(a, b, expected);

        [Theory]
        [InlineData(3, 27, "9")]
        [InlineData(15, 15, "15")]
        [InlineData(5, 125, "25")]
        public void PerfectSquaresAfterReduce(int a, int b, string expected) => Test(a, b, expected);

        [Theory]
        [InlineData(-3, -27, "-9")]
        [InlineData(-15, -15, "-15")]
        [InlineData(-5, -125, "-25")]
        public void PerfectSquaresAfterReduceNeg(int a, int b, string expected) => Test(a, b, expected);

        [Theory]
        [InlineData(-3, 27, "9*i")]
        [InlineData(15, -15, "15*i")]
        [InlineData(-5, 125, "25*i")]
        public void PerfectSquaresAfterReduceImag(int a, int b, string expected) => Test(a, b, expected);

        [Theory]
        [InlineData(6, 15, "3*sqrt(10)")]
        [InlineData(2, 3858, "2*sqrt(1929)")]
        [InlineData(1667, 10002, "1667*sqrt(6)")]
        public void CompositeAfterReduce(int a, int b, string expected) => Test(a, b, expected);

        [Theory]
        [InlineData(-6, -15, "-3*sqrt(10)")]
        [InlineData(-2, -3858, "-2*sqrt(1929)")]
        [InlineData(-1667, -10002, "-1667*sqrt(6)")]
        public void CompositeAfterReduceNeg(int a, int b, string expected) => Test(a, b, expected);

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
