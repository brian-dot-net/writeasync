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
            Calculation.FromString(input).Should().Be(result, "input was {0}", input);
        }

        [Theory]
        [InlineData(".44", "11/25")]
        [InlineData("4.4", "22/5")]
        [InlineData("44", "44")]
        public void NumbersWithTwo4s(string input, string result)
        {
            Calculation.FromString(input).Should().Be(result, "input was {0}", input);
        }

        [Theory]
        [InlineData(".444", "111/250")]
        [InlineData("4.44", "111/25")]
        [InlineData("44.4", "222/5")]
        [InlineData("444", "444")]
        public void NumbersWithThree4s(string input, string result)
        {
            Calculation.FromString(input).Should().Be(result, "input was {0}", input);
        }

        [Theory]
        [InlineData(".4444", "1111/2500")]
        [InlineData("4.444", "1111/250")]
        [InlineData("44.44", "1111/25")]
        [InlineData("444.4", "2222/5")]
        [InlineData("4444", "4444")]
        public void NumbersWithFour4s(string input, string result)
        {
            Calculation.FromString(input).Should().Be(result, "input was {0}", input);
        }

        [Theory]
        [InlineData("4 4 +", "8")]
        [InlineData("4 .4 +", "22/5")]
        [InlineData(".4 .44 4 + +", "121/25")]
        [InlineData("4 44 + 4 +", "52")]
        public void Addition(string input, string result)
        {
            Calculation.FromString(input).Should().Be(result);
        }
    }
}
