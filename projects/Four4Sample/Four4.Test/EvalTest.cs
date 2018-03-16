// <copyright file="EvalTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class EvalTest
    {
        [Theory]
        [InlineData("4", "4")]
        [InlineData(".4", "2/5")]
        [InlineData(".4_", "4/9")]
        [InlineData("44", "44")]
        [InlineData("4 4", "NaN")]
        [InlineData("444", "444")]
        [InlineData("4444", "4444")]
        public void Numbers(string input, string result)
        {
            Test(input, result);
        }

        [Theory]
        [InlineData("4 4 +", "8")]
        [InlineData("4 .4 +", "22/5")]
        [InlineData(".4 44 4 + +", "242/5")]
        [InlineData("4 44 + 4 +", "52")]
        [InlineData("4 4 4 - / 4 +", "NaN")]
        [InlineData("4 +", "NaN")]
        [InlineData("4 4 4 +", "NaN")]
        public void Addition(string input, string result)
        {
            Test(input, result);
        }

        [Theory]
        [InlineData("4 4 -", "0")]
        [InlineData(".4 .4 -", "0")]
        [InlineData("4 .4 -", "18/5")]
        [InlineData("44 .4 + .4 -", "44")]
        [InlineData("44 .4 - 4 -", "198/5")]
        [InlineData(".4 4 -", "-18/5")]
        [InlineData("4 4 4 4 - - -", "0")]
        [InlineData("4 4 4 - - 4 -", "0")]
        [InlineData("4 4 4 - / 4 -", "NaN")]
        [InlineData("4 -", "NaN")]
        [InlineData("4 4 4 -", "NaN")]
        public void Subtraction(string input, string result)
        {
            Test(input, result);
        }

        [Theory]
        [InlineData("4 4 *", "16")]
        [InlineData("4 .4 *", "8/5")]
        [InlineData(".4 .4 *", "4/25")]
        [InlineData("4 4 * 4 4 * *", "256")]
        [InlineData("4 4 4 4 * * *", "256")]
        [InlineData("4 4 4 - / 4 *", "NaN")]
        [InlineData("4 *", "NaN")]
        [InlineData("4 4 4 *", "NaN")]
        public void Multiplication(string input, string result)
        {
            Test(input, result);
        }

        [Theory]
        [InlineData("4 4 /", "1")]
        [InlineData("4 .4 /", "10")]
        [InlineData("444 .4 /", "1110")]
        [InlineData(".4 4 /", "1/10")]
        [InlineData(".4 .4 .4 / / .4 /", "1")]
        [InlineData(".4 .4 .4 .4 / / /", "1")]
        [InlineData("4 .4 4 .4 / / /", "100")]
        [InlineData("4 4 4 - /", "NaN")]
        [InlineData("4 4 4 - / 4 /", "NaN")]
        [InlineData("4 /", "NaN")]
        [InlineData("4 4 4 /", "NaN")]
        public void Division(string input, string result)
        {
            Test(input, result);
        }

        [Theory]
        [InlineData("4 !", "24")]
        [InlineData("4 .4 / !", "3628800")]
        [InlineData("4 4 / 4 + !", "120")]
        [InlineData("4 4 / ! ! !", "NaN")]
        [InlineData("4 4 - ! ! !", "NaN")]
        [InlineData(".4 !", "NaN")]
        [InlineData("4 4 - 4 - !", "NaN")]
        [InlineData("4 4 - 4 - ! !", "NaN")]
        [InlineData("!", "NaN")]
        [InlineData("4 4 !", "NaN")]
        [InlineData("4 ! !", "NaN")]
        [InlineData("4 4 / !", "NaN")]
        [InlineData("4 4 + 4 / !", "NaN")]
        public void Factorial(string input, string result)
        {
            Test(input, result);
        }

        [Theory]
        [InlineData("4 R", "2")]
        [InlineData("4 4 * R", "4")]
        [InlineData("4 ! 4 ! * R", "24")]
        [InlineData(".4_ R", "2/3")]
        [InlineData("4 4 + R", "NaN")]
        [InlineData(".4 .4_ / R", "NaN")]
        [InlineData(".4 .4_ / R R", "NaN")]
        [InlineData("4 4 - R R", "NaN")]
        [InlineData("4 4 - 4 - R", "NaN")]
        [InlineData("4 4 - .4_ - R", "NaN")]
        [InlineData("R", "NaN")]
        [InlineData("4 4 R", "NaN")]
        [InlineData("4 R R", "NaN")]
        [InlineData("4 R R R", "NaN")]
        [InlineData("4 4 4 R R R R", "NaN")]
        [InlineData("4 4 - R", "NaN")]
        [InlineData("4 4 / R", "NaN")]
        public void SquareRoot(string input, string result)
        {
            Test(input, result);
        }

        [Theory]
        [InlineData("4 4 ^", "256")]
        public void Exponent(string input, string result)
        {
            Test(input, result);
        }

        private static void Test(string input, string result)
        {
            Expression.Eval(input).ToString().Should().Be(result, "input was {0}", input);
        }
    }
}
