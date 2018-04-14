// <copyright file="Atoms.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Atoms
    {
        [InlineData("1", "NumL(1)")]
        [InlineData("22", "NumL(22)")]
        [InlineData("32000", "NumL(32000)")]
        [Theory]
        public void Integers(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("\"1\"", "StrL(1)")]
        [InlineData("\"\"", "StrL()")]
        [InlineData("\"string with spaces\"", "StrL(string with spaces)")]
        [Theory]
        public void Strings(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A", "NumVar(A)")]
        [InlineData("AB", "NumVar(AB)")]
        [InlineData("XYZ123", "NumVar(XYZ123)")]
        [Theory]
        public void NumVars(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A$", "StrVar(A)")]
        [InlineData("AB$", "StrVar(AB)")]
        [InlineData("XYZ123$", "StrVar(XYZ123)")]
        [Theory]
        public void StrVars(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("a", "NumVar(A)")]
        [InlineData("ab", "NumVar(AB)")]
        [InlineData("xyZ123", "NumVar(XYZ123)")]
        [InlineData("a$", "StrVar(A)")]
        [InlineData("Ab$", "StrVar(AB)")]
        [InlineData("XyZ123$", "StrVar(XYZ123)")]
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

        [InlineData("(1)", "NumL(1)")]
        [InlineData("(\"x\")", "StrL(x)")]
        [InlineData("(A)", "NumVar(A)")]
        [InlineData("(A$)", "StrVar(A)")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
