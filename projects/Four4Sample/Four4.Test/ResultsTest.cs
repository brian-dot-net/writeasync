// <copyright file="ResultsTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class ResultsTest
    {
        [Theory]
        [InlineData("4", "444", "-")]
        [InlineData("44", "44", "-")]
        [InlineData("4", "444", "/")]
        [InlineData("44", "44", "*")]
        [InlineData("4", "4", "+")]
        public void InvalidNumbers(string x, string y, string op)
        {
            TestAddInvalid(x, y, op);
        }

        private static void TestAddInvalid(string x, string y, string op)
        {
            Results results = new Results();
            Expression expr = default(Expression).Append(x).Append(y).Append(op);

            results.Add(expr);

            results.Count.Should().Be(0);
        }
    }
}
