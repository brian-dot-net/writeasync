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
        public void Numbers(string input, string result)
        {
            Calculation.FromString(input).Should().Be(result);
        }

        [Theory]
        [InlineData("4 4 +", "8")]
        public void Addition(string input, string result)
        {
            Calculation.FromString(input).Should().Be(result);
        }
    }
}
