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
        [Theory]
        public void Integers(string input, string output)
        {
            BasicExpression.Parse(input).ToString().Should().Be(output);
        }
    }
}
