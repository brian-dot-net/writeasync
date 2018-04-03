// <copyright file="Atoms.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using FluentAssertions;
    using Xunit;

    public class Atoms
    {
        [InlineData("1", "NumericLiteral(1)")]
        [InlineData("22", "NumericLiteral(22)")]
        [InlineData("32000", "NumericLiteral(32000)")]
        [Theory]
        public void Integers(string input, string output)
        {
            BasicExpression.FromString(input).ToString().Should().Be(output);
        }
    }
}
