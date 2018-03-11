// <copyright file="ExpressionSearchTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4.Test
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Xunit;

    public sealed class ExpressionSearchTest
    {
        [Fact]
        public void OneOperandAndOneBinaryOperator()
        {
            List<string> expressions = new List<string>();
            ExpressionSearch search = new ExpressionSearch();
            search.AddOperand("4");
            search.AddBinary("+");

            search.Run(e => expressions.Add(e.ToString()));

            expressions.Sort();
            expressions.Should().ContainInOrder(
                "4",
                "4 4 +",
                "4 4 + 4 +",
                "4 4 + 4 + 4 +",
                "4 4 + 4 4 + +",
                "4 4 4 + +",
                "4 4 4 + + 4 +",
                "4 4 4 + 4 + +",
                "4 4 4 4 + + +");
        }
    }
}
