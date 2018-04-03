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

        [InlineData("\"1\"", "StringLiteral(\"1\")")]
        [InlineData("\"\"", "StringLiteral(\"\")")]
        [InlineData("\"string with spaces\"", "StringLiteral(\"string with spaces\")")]
        [Theory]
        public void Strings(string input, string output)
        {
            BasicExpression.FromString(input).ToString().Should().Be(output);
        }

        [InlineData("A", "NumericVariable(A)")]
        [InlineData("AB", "NumericVariable(AB)")]
        [Theory]
        public void NumericVariables(string input, string output)
        {
            BasicExpression.FromString(input).ToString().Should().Be(output);
        }
    }
}
