// <copyright file="Addition.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Addition
    {
        [InlineData("1+2", "Add(L(1), L(2))")]
        [InlineData("X+234", "Add(NumVar(X), L(234))")]
        [InlineData("X(234)+YZ1234", "Add(Array(NumVar(X), L(234)), NumVar(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("\"1\"+\"2\"", "Add(L(\"1\"), L(\"2\"))")]
        [InlineData("X$+\"234\"", "Add(StrVar(X), L(\"234\"))")]
        [InlineData("X$(234)+YZ1234$", "Add(Array(StrVar(X), L(234)), StrVar(YZ1234))")]
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

        [InlineData("(1+2)", "Add(L(1), L(2))")]
        [InlineData("(X$+\"234\")", "Add(StrVar(X), L(\"234\"))")]
        [InlineData("(X$(234)+YZ1234$)", "Add(Array(StrVar(X), L(234)), StrVar(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+2+3", "Add(Add(L(1), L(2)), L(3))")]
        [InlineData("(1+2+3)", "Add(Add(L(1), L(2)), L(3))")]
        [InlineData("(1+2)+3", "Add(Add(L(1), L(2)), L(3))")]
        [InlineData("1+(2+3)", "Add(L(1), Add(L(2), L(3)))")]
        [InlineData("\"1\"+(\"2\"+\"3\")", "Add(L(\"1\"), Add(L(\"2\"), L(\"3\")))")]
        [Theory]
        public void WithThreeTerms(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
