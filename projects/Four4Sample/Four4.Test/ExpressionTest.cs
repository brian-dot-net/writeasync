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
        [InlineData("+", "+")]
        [InlineData("-", "-")]
        [InlineData("!", "!")]
        [InlineData("4", "4")]
        public void AppendToEmpty(string input, string result)
        {
            Expression expr = default(Expression);

            TestAppend(expr, input, result);
        }

        [Theory]
        [InlineData("+", "4 +")]
        [InlineData("-", "4 -")]
        [InlineData("!", "4 !")]
        [InlineData("4", "4 4")]
        public void AppendToOne(string input, string result)
        {
            Expression expr = default(Expression).Append("4");

            TestAppend(expr, input, result);
        }

        [Theory]
        [InlineData("+", "44 4 +")]
        [InlineData("-", "44 4 -")]
        [InlineData("!", "44 4 !")]
        [InlineData("4", "44 4 4")]
        public void AppendToTwo(string input, string result)
        {
            Expression expr = default(Expression).Append("44").Append("4");

            TestAppend(expr, input, result);
        }

        [Theory]
        [InlineData("4444", ".4")]
        [InlineData("4444", ".4_")]
        [InlineData("4444", "4")]
        [InlineData("444", "44")]
        [InlineData("44", "444")]
        [InlineData("4", "4444")]
        [InlineData("4444", "44")]
        [InlineData("444", "444")]
        [InlineData("444", "4444")]
        [InlineData("4444", "444")]
        [InlineData("4444", "4444")]
        public void AppendTooManyDigits(string x, string y)
        {
            Expression expr = default(Expression).Append(x);

            expr = expr.Append(y);

            expr.IsInRange.Should().BeFalse();
        }

        private static void TestAppend(Expression expr, string input, string result)
        {
            expr = expr.Append(input);

            expr.ToString().Should().Be(result);
            expr.IsInRange.Should().BeTrue();
        }
    }
}
