// <copyright file="CalculationTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class CalculationTest
    {
        [Theory]
        [InlineData("4", "4")]
        [InlineData(".4", "2/5")]
        [InlineData(".4_", "4/9")]
        public void NumbersWithOne4(string input, string result)
        {
            Calc(input, result);
        }

        [Theory]
        [InlineData(".44", "11/25")]
        [InlineData("4.4", "22/5")]
        [InlineData("44", "44")]
        public void NumbersWithTwo4s(string input, string result)
        {
            Calc(input, result);
        }

        [Theory]
        [InlineData(".444", "111/250")]
        [InlineData("4.44", "111/25")]
        [InlineData("44.4", "222/5")]
        [InlineData("444", "444")]
        public void NumbersWithThree4s(string input, string result)
        {
            Calc(input, result);
        }

        [Theory]
        [InlineData(".4444", "1111/2500")]
        [InlineData("4.444", "1111/250")]
        [InlineData("44.44", "1111/25")]
        [InlineData("444.4", "2222/5")]
        [InlineData("4444", "4444")]
        public void NumbersWithFour4s(string input, string result)
        {
            Calc(input, result);
        }

        [Theory]
        [InlineData("4 4 +", "8")]
        [InlineData("4 .4 +", "22/5")]
        [InlineData(".4 .44 4 + +", "121/25")]
        [InlineData("4 44 + 4 +", "52")]
        [InlineData("4 4 4 - / 4 +", "NaN")]
        public void Addition(string input, string result)
        {
            Calc(input, result);
        }

        [Theory]
        [InlineData("4 4 -", "0")]
        [InlineData(".4 .4 -", "0")]
        [InlineData("4 .4 -", "18/5")]
        [InlineData("44.4 .4 -", "44")]
        [InlineData("44 .4 - 4 -", "198/5")]
        [InlineData(".4 4 -", "-18/5")]
        [InlineData("4 4 4 4 - - -", "0")]
        [InlineData("4 4 4 - - 4 -", "0")]
        [InlineData("4 4 4 - / 4 -", "NaN")]
        public void Subtraction(string input, string result)
        {
            Calc(input, result);
        }

        [Theory]
        [InlineData("4 4 *", "16")]
        [InlineData("4 .4 *", "8/5")]
        [InlineData(".4 .4 *", "4/25")]
        [InlineData("44 .44 *", "484/25")]
        [InlineData("4 4 * 4 4 * *", "256")]
        [InlineData("4 4 4 4 * * *", "256")]
        [InlineData("4 4 4 - / 4 *", "NaN")]
        public void Multiplication(string input, string result)
        {
            Calc(input, result);
        }

        [Theory]
        [InlineData("4 4 /", "1")]
        [InlineData("4 .4 /", "10")]
        [InlineData("44 .44 /", "100")]
        [InlineData(".4 4 /", "1/10")]
        [InlineData(".4 .4 .4 / / .4 /", "1")]
        [InlineData(".4 .4 .4 .4 / / /", "1")]
        [InlineData("4 .4 4 .4 / / /", "100")]
        [InlineData("4 4 4 - /", "NaN")]
        [InlineData("4 4 4 - / 4 /", "NaN")]
        public void Division(string input, string result)
        {
            Calc(input, result);
        }

        [Theory]
        [InlineData("4 !", "24")]
        [InlineData("4 .4 / !", "3628800")]
        [InlineData("4 4 / 4 + !", "120")]
        [InlineData("4 4 / ! ! !", "1")]
        [InlineData("4 4 - ! ! !", "1")]
        [InlineData(".4 !", "NaN")]
        [InlineData("4 4 - 4 - !", "NaN")]
        [InlineData("4 4 - 4 - ! !", "NaN")]
        public void Factorial(string input, string result)
        {
            Calc(input, result);
        }

        [Theory]
        [InlineData("4 R", "2")]
        [InlineData("4 4 * R", "4")]
        [InlineData("4 ! 4 ! * R", "24")]
        [InlineData(".4_ R", "2/3")]
        [InlineData("4 4 + R", "NaN")]
        [InlineData(".4 .4_ / R", "NaN")]
        public void SquareRoot(string input, string result)
        {
            Calc(input, result);
        }

        private static void Calc(string input, string result)
        {
            Calculation.FromString(input).Should().Be(result, "input was {0}", input);
        }
    }
}
