// <copyright file="Addition.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Addition
    {
        [InlineData("1+2", "Add(NumericLiteral(1), NumericLiteral(2))")]
        [InlineData("X+234", "Add(NumericVariable(X), NumericLiteral(234))")]
        [InlineData("X(234)+YZ1234", "Add(Array(NumericVariable(X), NumericLiteral(234)), NumericVariable(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("\"1\"+\"2\"", "Add(StringLiteral(\"1\"), StringLiteral(\"2\"))")]
        [InlineData("X$+\"234\"", "Add(StringVariable(X), StringLiteral(\"234\"))")]
        [InlineData("X$(234)+YZ1234$", "Add(Array(StringVariable(X), NumericLiteral(234)), StringVariable(YZ1234))")]
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

        [InlineData("(1+2)", "Add(NumericLiteral(1), NumericLiteral(2))")]
        [InlineData("(X$+\"234\")", "Add(StringVariable(X), StringLiteral(\"234\"))")]
        [InlineData("(X$(234)+YZ1234$)", "Add(Array(StringVariable(X), NumericLiteral(234)), StringVariable(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+2+3", "Add(Add(NumericLiteral(1), NumericLiteral(2)), NumericLiteral(3))")]
        [InlineData("(1+2+3)", "Add(Add(NumericLiteral(1), NumericLiteral(2)), NumericLiteral(3))")]
        [InlineData("(1+2)+3", "Add(Add(NumericLiteral(1), NumericLiteral(2)), NumericLiteral(3))")]
        [InlineData("1+(2+3)", "Add(NumericLiteral(1), Add(NumericLiteral(2), NumericLiteral(3)))")]
        [InlineData("\"1\"+(\"2\"+\"3\")", "Add(StringLiteral(\"1\"), Add(StringLiteral(\"2\"), StringLiteral(\"3\")))")]
        [Theory]
        public void WithThreeTerms(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
