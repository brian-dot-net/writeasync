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
        public void ConvertsProperly(string input, string result)
        {
            Postfix.ToInfix(input).Should().Be(result);
        }
    }
}
