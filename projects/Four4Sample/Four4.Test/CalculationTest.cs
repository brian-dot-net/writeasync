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
        [InlineData(".44", "11/25")]
        [InlineData(".444", "111/250")]
        [InlineData(".4444", "1111/2500")]
        public void Numbers(string input, string result)
        {
            Calculation.FromString(input).Should().Be(result, "input was {0}", input);
        }

        [Theory]
        [InlineData("4 4 +", "8")]
        [InlineData("4 .4 +", "22/5")]
        public void Addition(string input, string result)
        {
            Calculation.FromString(input).Should().Be(result);
        }
    }
}
