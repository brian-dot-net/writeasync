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
            Number.Parse(input).IsWhole.Should().BeTrue();
        }

        [Theory]
        [InlineData(".4")]
        [InlineData(".4_")]
        public void Fractions(string input)
        {
            Number.Parse(input).IsWhole.Should().BeFalse();
        }
    }
}
