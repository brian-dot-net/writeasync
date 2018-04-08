// <copyright file="Atoms.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Atoms
    {
        [InlineData("1", "Literal(1)")]
        [InlineData("22", "Literal(22)")]
        [InlineData("32000", "Literal(32000)")]
        [Theory]
        public void Integers(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("\"1\"", "Literal(\"1\")")]
        [InlineData("\"\"", "Literal(\"\")")]
        [InlineData("\"string with spaces\"", "Literal(\"string with spaces\")")]
        [Theory]
        public void Strings(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A", "NumericVariable(A)")]
        [InlineData("AB", "NumericVariable(AB)")]
        [InlineData("XYZ123", "NumericVariable(XYZ123)")]
        [Theory]
        public void NumericVariables(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A$", "StringVariable(A)")]
        [InlineData("AB$", "StringVariable(AB)")]
        [InlineData("XYZ123$", "StringVariable(XYZ123)")]
        [Theory]
        public void StringVariables(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("a", "NumericVariable(A)")]
        [InlineData("ab", "NumericVariable(AB)")]
        [InlineData("xyZ123", "NumericVariable(XYZ123)")]
        [InlineData("a$", "StringVariable(A)")]
        [InlineData("Ab$", "StringVariable(AB)")]
        [InlineData("XyZ123$", "StringVariable(XYZ123)")]
        [Theory]
        public void VariablesToUppercase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("123x")]
        [Theory]
        public void InvalidNumber(string input)
        {
            Test.Bad(input);
        }

        [InlineData("\"just the beginning")]
        [Theory]
        public void InvalidString(string input)
        {
            Test.Bad(input);
        }

        [InlineData("(1)", "Literal(1)")]
        [InlineData("(\"x\")", "Literal(\"x\")")]
        [InlineData("(A)", "NumericVariable(A)")]
        [InlineData("(A$)", "StringVariable(A)")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
