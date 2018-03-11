// <copyright file="PostfixToInfixTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class PostfixToInfixTest
    {
        [Theory]
        [InlineData("4", "4")]
        [InlineData("44", "44")]
        [InlineData(".4_", ".4_")]
        [InlineData("4 !", "(4)!")]
        [InlineData(".4_ R", "sqrt(.4_)")]
        [InlineData("4 4 +", "(4+4)")]
        [InlineData("4 4 + 4 +", "((4+4)+4)")]
        [InlineData("4 4 4 + +", "(4+(4+4))")]
        [InlineData("4 4 -", "(4-4)")]
        [InlineData("4 4 - 4 -", "((4-4)-4)")]
        [InlineData("4 4 4 - -", "(4-(4-4))")]
        [InlineData("4 4 *", "(4*4)")]
        [InlineData("4 4 * 4 *", "((4*4)*4)")]
        [InlineData("4 4 4 * *", "(4*(4*4))")]
        [InlineData("4 4 /", "(4/4)")]
        [InlineData("4 4 / 4 /", "((4/4)/4)")]
        [InlineData("4 4 4 / /", "(4/(4/4))")]
        [InlineData("4 R 4 ! * 4 / 4 -", "(((sqrt(4)*(4)!)/4)-4)")]
        public void ConvertsProperly(string input, string result)
        {
            Postfix.ToInfix(input).Should().Be(result);
        }
    }
}
