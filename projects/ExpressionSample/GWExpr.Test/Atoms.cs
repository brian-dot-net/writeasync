// <copyright file="Atoms.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class Atoms
    {
        [InlineData("1", "NumericLiteral(1)")]
        [InlineData("22", "NumericLiteral(22)")]
        [InlineData("32000", "NumericLiteral(32000)")]
        [Theory]
        public void Integers(string input, string output)
        {
            Test(input, output);
        }

        [InlineData("\"1\"", "StringLiteral(\"1\")")]
        [InlineData("\"\"", "StringLiteral(\"\")")]
        [InlineData("\"string with spaces\"", "StringLiteral(\"string with spaces\")")]
        [Theory]
        public void Strings(string input, string output)
        {
            Test(input, output);
        }

        [InlineData("A", "NumericVariable(A)")]
        [InlineData("AB", "NumericVariable(AB)")]
        [InlineData("XYZ123", "NumericVariable(XYZ123)")]
        [Theory]
        public void NumericVariables(string input, string output)
        {
            Test(input, output);
        }

        [InlineData("A$", "StringVariable(A)")]
        [InlineData("AB$", "StringVariable(AB)")]
        [InlineData("XYZ123$", "StringVariable(XYZ123)")]
        [Theory]
        public void StringVariables(string input, string output)
        {
            Test(input, output);
        }

        private static void Test(string input, string output)
        {
            BasicExpression.FromString(input).ToString().Should().Be(output);
        }
    }
}
