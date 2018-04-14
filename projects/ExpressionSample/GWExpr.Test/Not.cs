// <copyright file="Not.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Not
    {
        [InlineData("NOT 1", "Not(NumL(1))")]
        [InlineData("NOT X", "Not(NumV(X))")]
        [InlineData("NOT X(234)", "Not(NumArr(X, NumL(234)))")]
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

        [InlineData("NOT (1)", "Not(NumL(1))")]
        [InlineData("NOT (NOT (X))", "Not(Not(NumV(X)))")]
        [InlineData("(NOT X)", "Not(NumV(X))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+NOT -2", "Add(NumL(1), Not(Neg(NumL(2))))")]
        [InlineData("NOT 1+2", "Not(Add(NumL(1), NumL(2)))")]
        [InlineData("1-NOT -2", "Sub(NumL(1), Not(Neg(NumL(2))))")]
        [InlineData("NOT 1-NOT 2", "Not(Sub(NumL(1), Not(NumL(2))))")]
        [InlineData("NOT 1*2", "Not(Mult(NumL(1), NumL(2)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+NOT")]
        [InlineData("NOT(1,X)")]
        [InlineData("NOT$")]
        [Theory]
        public void FailedReserved(string input)
        {
            Test.Bad(input);
        }
    }
}
