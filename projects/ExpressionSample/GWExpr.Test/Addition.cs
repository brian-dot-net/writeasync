// <copyright file="Addition.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Addition
    {
        [InlineData("1+2", "Add(Literal(1), Literal(2))")]
        [InlineData("X+234", "Add(NumVar(X), Literal(234))")]
        [InlineData("X(234)+YZ1234", "Add(Array(NumVar(X), Literal(234)), NumVar(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("\"1\"+\"2\"", "Add(Literal(\"1\"), Literal(\"2\"))")]
        [InlineData("X$+\"234\"", "Add(StrVar(X), Literal(\"234\"))")]
        [InlineData("X$(234)+YZ1234$", "Add(Array(StrVar(X), Literal(234)), StrVar(YZ1234))")]
        [Theory]
        public void StringExpressions(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("\"1\"+2")]
        [InlineData("X$+234")]
        [InlineData("X(234)+YZ1234$")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("(1+2)", "Add(Literal(1), Literal(2))")]
        [InlineData("(X$+\"234\")", "Add(StrVar(X), Literal(\"234\"))")]
        [InlineData("(X$(234)+YZ1234$)", "Add(Array(StrVar(X), Literal(234)), StrVar(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+2+3", "Add(Add(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1+2+3)", "Add(Add(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1+2)+3", "Add(Add(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("1+(2+3)", "Add(Literal(1), Add(Literal(2), Literal(3)))")]
        [InlineData("\"1\"+(\"2\"+\"3\")", "Add(Literal(\"1\"), Add(Literal(\"2\"), Literal(\"3\")))")]
        [Theory]
        public void WithThreeTerms(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
