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
            TestAdd(x, y, op, 0);
        }

        [Theory]
        [InlineData("44", "44", "/")]
        [InlineData("44", "44", "+")]
        public void ValidNumbers(string x, string y, string op)
        {
            TestAdd(x, y, op, 1);
        }

        [Fact]
        public void ExactDuplicates()
        {
            Results results = new Results();
            Expression expr = default(Expression).Append("44").Append("44").Append("+");

            results.Add(expr);
            results.Add(expr);

            results.Count.Should().Be(1);
        }

        private static void TestAdd(string x, string y, string op, int count)
        {
            Results results = new Results();
            Expression expr = default(Expression).Append(x).Append(y).Append(op);

            results.Add(expr);

            results.Count.Should().Be(count, "expr was '{0}'", expr.ToString());
        }
    }
}
