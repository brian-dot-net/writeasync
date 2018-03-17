// <copyright file="ResultsTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4.Test
{
    using System.Linq;
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
        [InlineData("44", "44", "-", "44", "-")]
        [InlineData("44", "44", "-", "4", "-")]
        public void TooManyNumerals(string x, string y, string op1, string z, string op2)
        {
            TestTooMany(x, y, op1, z, op2);
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

        [Fact]
        public void NumericDuplicates()
        {
            Results results = new Results();
            Expression expr1 = default(Expression).Append("44").Append("4").Append("4").Append("+").Append("+");
            Expression expr2 = default(Expression).Append("4").Append("4").Append("44").Append("+").Append("+");

            results.Add(expr1);
            results.Add(expr2);

            results.Count.Should().Be(1);
        }

        [Fact]
        public void SortOrder()
        {
            Results results = new Results();
            Expression expr1 = default(Expression).Append("44").Append("44").Append("/");
            Expression expr44 = default(Expression).Append("44").Append("4").Append("-").Append("4").Append("+");

            results.Add(expr44);
            results.Add(expr1);

            Expression[] all = results.ToArray();
            all.Select(e => e.ToString()).Should().ContainInOrder(
                "44 44 /",
                "44 4 - 4 +")
                .And.HaveCount(2);
        }

        [Fact]
        public void CustomNumeralCount()
        {
            Results results = new Results { NumeralCount = 2 };
            Expression exprTooFew = default(Expression).Append("4").Append("R");
            Expression exprTooMany = default(Expression).Append("44").Append("4").Append("+");
            Expression exprJustRight = default(Expression).Append("4").Append("4").Append("/");

            results.Add(exprTooFew);
            results.Add(exprTooMany);
            results.Add(exprJustRight);

            Expression[] all = results.ToArray();
            all.Select(e => e.ToString()).Should().ContainInOrder(
                "4 4 /").And.HaveCount(1);
        }

        private static void TestAdd(string x, string y, string op, int count)
        {
            Results results = new Results();
            Expression expr = default(Expression).Append(x).Append(y).Append(op);

            results.Add(expr);

            results.Count.Should().Be(count, "expr was '{0}'", expr.ToString());
        }

        private static void TestTooMany(string x, string y, string op1, string z, string op2)
        {
            Results results = new Results();
            Expression expr = default(Expression).Append(x).Append(y).Append(op1).Append(z).Append(op2);

            results.Add(expr);

            results.Count.Should().Be(0, "expr was '{0}'", expr.ToString());
        }
    }
}
