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

        [Theory]
        [InlineData(-1, 4, "2+i")]
        [InlineData(-4, 9, "3+2*i")]
        [InlineData(1, -1073741824, "1+32768*i")]
        public void PerfectSquaresImag(int a, int b, string expected) => Test(a, b, expected);

        [Theory]
        [InlineData(2, 3, "sqrt(2)+sqrt(3)")]
        [InlineData(7, 11, "sqrt(7)+sqrt(11)")]
        [InlineData(32749, 32771, "sqrt(32749)+sqrt(32771)")]
        [InlineData(1, 2147483647, "1+sqrt(2147483647)")]
        public void Primes(int a, int b, string expected) => Test(a, b, expected);

        [Theory]
        [InlineData(-2, -3, "sqrt(2)*i+sqrt(3)*i")]
        [InlineData(-7, -11, "sqrt(7)*i+sqrt(11)*i")]
        [InlineData(-32749, -32771, "sqrt(32749)*i+sqrt(32771)*i")]
        [InlineData(-1, -2147483647, "i+sqrt(2147483647)*i")]
        public void PrimesNeg(int a, int b, string expected) => Test(a, b, expected);

        [Theory]
        [InlineData(-2, 3, "sqrt(3)+sqrt(2)*i")]
        [InlineData(7, -11, "sqrt(7)+sqrt(11)*i")]
        [InlineData(32749, -32771, "sqrt(32749)+sqrt(32771)*i")]
        [InlineData(-1, 2147483647, "sqrt(2147483647)+i")]
        public void PrimesImag(int a, int b, string expected) => Test(a, b, expected);

        [Theory]
        [InlineData(3, 27, "4*sqrt(3)")]
        [InlineData(15, 15, "2*sqrt(15)")]
        [InlineData(5, 125, "6*sqrt(5)")]
        public void MultiplesAfterReduce(int a, int b, string expected) => Test(a, b, expected);

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
