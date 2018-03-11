// <copyright file="ExpressionTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4.Test
{
    using System;
    using FluentAssertions;
    using Xunit;

    public sealed class ExpressionTest
    {
        [Fact]
        public void Empty()
        {
            Expression expr = default(Expression);

            expr.ToString().Should().Be(string.Empty);
            expr.Count.Should().Be(0);
        }

        [Theory]
        [InlineData("4.4", "*'4.4'*")]
        [InlineData("!!", "*'!!'*")]
        [InlineData("", "*''*")]
        [InlineData("B", "*'B'*")]
        [InlineData("B.A.D.", "*'B.A.D.'*")]
        [InlineData(" ", "*' '*")]
        public void AppendBadToken(string input, string errorPattern)
        {
            Expression expr = default(Expression);

            Action act = () => expr.Append(input);

            act.Should().Throw<ArgumentException>().WithMessage(errorPattern).Which.ParamName.Should().Be("token");
        }

        [Theory]
        [InlineData("+", "+", 0)]
        [InlineData("-", "-", 0)]
        [InlineData("!", "!", 0)]
        [InlineData("4", "4", 1)]
        public void AppendToEmpty(string input, string result, int count)
        {
            Expression expr = default(Expression);

            TestAppend(expr, input, result, count);
        }

        [Theory]
        [InlineData("+", "4 +", 1)]
        [InlineData("-", "4 -", 1)]
        [InlineData("!", "4 !", 1)]
        [InlineData("4", "4 4", 2)]
        public void AppendToOne(string input, string result, int count)
        {
            Expression expr = default(Expression).Append("4");

            TestAppend(expr, input, result, count);
        }

        [Theory]
        [InlineData("+", "44 4 +", 3)]
        [InlineData("-", "44 4 -", 3)]
        [InlineData("!", "44 4 !", 3)]
        [InlineData("4", "44 4 4", 4)]
        public void AppendToTwo(string input, string result, int count)
        {
            Expression expr = default(Expression).Append("44").Append("4");

            TestAppend(expr, input, result, count);
        }

        [Theory]
        [InlineData("4444", ".4", 5)]
        [InlineData("4444", ".4_", 5)]
        [InlineData("4444", "4", 5)]
        [InlineData("444", "44", 5)]
        [InlineData("44", "444", 5)]
        [InlineData("4", "4444", 5)]
        [InlineData("4444", "44", 6)]
        [InlineData("444", "444", 6)]
        [InlineData("444", "4444", 7)]
        [InlineData("4444", "444", 7)]
        [InlineData("4444", "4444", 8)]
        public void AppendTooManyDigits(string x, string y, int count)
        {
            Expression expr = default(Expression).Append(x);

            expr = expr.Append(y);

            expr.Count.Should().Be(count);
        }

        [Theory]
        [InlineData("+", "4", 5)]
        [InlineData("!", "4", 5)]
        [InlineData("*", "44", 6)]
        [InlineData("R", "444", 7)]
        [InlineData("-", ".4_", 5)]
        public void AppendTooManyDigitsAfterOperator(string op, string y, int count)
        {
            Expression expr = default(Expression).Append("4444").Append(op);

            expr = expr.Append(y);

            expr.Count.Should().Be(count);
        }

        private static void TestAppend(Expression expr, string input, string result, int count)
        {
            expr = expr.Append(input);

            expr.ToString().Should().Be(result);
            expr.Count.Should().Be(count);
        }
    }
}
