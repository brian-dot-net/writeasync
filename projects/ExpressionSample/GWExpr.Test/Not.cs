// <copyright file="Not.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Not
    {
        [InlineData("NOT 1", "Not(Literal(1))")]
        [InlineData("NOT X", "Not(NumVar(X))")]
        [InlineData("NOT X(234)", "Not(Array(NumVar(X), Literal(234)))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("NOT \"1\"")]
        [InlineData("NOT X$")]
        [InlineData("NOT YZ1234$")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("NOT (1)", "Not(Literal(1))")]
        [InlineData("NOT (NOT (X))", "Not(Not(NumVar(X)))")]
        [InlineData("(NOT X)", "Not(NumVar(X))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+NOT -2", "Add(Literal(1), Not(Negate(Literal(2))))")]
        [InlineData("NOT 1+2", "Not(Add(Literal(1), Literal(2)))")]
        [InlineData("1-NOT -2", "Subtract(Literal(1), Not(Negate(Literal(2))))")]
        [InlineData("NOT 1-NOT 2", "Not(Subtract(Literal(1), Not(Literal(2))))")]
        [InlineData("NOT 1*2", "Not(Multiply(Literal(1), Literal(2)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+NOT")]
        [InlineData("NOT(1,X)")]
        [Theory]
        public void FailedReserved(string input)
        {
            Test.Bad(input);
        }
    }
}
