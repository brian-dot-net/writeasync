// <copyright file="NumberTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class NumberTest
    {
        [Theory]
        [InlineData("4")]
        [InlineData("44")]
        [InlineData("444")]
        [InlineData("4444")]
        public void WholeNumbers(string input)
        {
            Parse(input).IsInteger.Should().BeTrue();
        }

        [Theory]
        [InlineData(".4")]
        [InlineData(".4_")]
        public void Fractions(string input)
        {
            Parse(input).IsInteger.Should().BeFalse();
        }

        [Theory]
        [InlineData("4 4 - 4 -")]
        [InlineData("4 4 -")]
        [InlineData("4 4 - 4 - 4 -")]
        public void NonPositiveNumbersAreIntegers(string input)
        {
            Expression.Eval(input).IsInteger.Should().BeTrue();
        }

        [Theory]
        [InlineData("4", 4)]
        [InlineData("44", 44)]
        [InlineData("444", 444)]
        [InlineData("4444", 4444)]
        public void CastToInt32(string input, int result)
        {
            int value = (int)Parse(input);

            value.Should().Be(result);
        }

        [Theory]
        [InlineData("4.4")]
        [InlineData(".44_")]
        [InlineData("4_4")]
        [InlineData("4_")]
        public void InvalidNumbers(string input)
        {
            Number.TryParse(input, out Number number, out int d).Should().BeFalse();
        }

        private static Number Parse(string input)
        {
            Number number;
            bool valid = Number.TryParse(input, out number, out int d);
            valid.Should().BeTrue();
            return number;
        }
    }
}
