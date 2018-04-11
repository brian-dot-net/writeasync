// <copyright file="Not.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Not
    {
        [InlineData("NOT 1", "Not(L(1))")]
        [InlineData("NOT X", "Not(NumVar(X))")]
        [InlineData("NOT X(234)", "Not(Array(NumVar(X), L(234)))")]
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

        [InlineData("NOT (1)", "Not(L(1))")]
        [InlineData("NOT (NOT (X))", "Not(Not(NumVar(X)))")]
        [InlineData("(NOT X)", "Not(NumVar(X))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+NOT -2", "Add(L(1), Not(Neg(L(2))))")]
        [InlineData("NOT 1+2", "Not(Add(L(1), L(2)))")]
        [InlineData("1-NOT -2", "Sub(L(1), Not(Neg(L(2))))")]
        [InlineData("NOT 1-NOT 2", "Not(Sub(L(1), Not(L(2))))")]
        [InlineData("NOT 1*2", "Not(Mult(L(1), L(2)))")]
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
