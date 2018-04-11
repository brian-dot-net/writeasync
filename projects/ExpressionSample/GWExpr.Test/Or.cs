// <copyright file="Or.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Or
    {
        [InlineData("1 OR 2", "Or(L(1), L(2))")]
        [InlineData("X OR 234", "Or(NumVar(X), L(234))")]
        [InlineData("X(234) OR YZ1234", "Or(Array(NumVar(X), L(234)), NumVar(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("2 OR \"1\"")]
        [InlineData("234 OR X$")]
        [InlineData("X(234) OR YZ1234$")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("(1 OR 2)", "Or(L(1), L(2))")]
        [InlineData("(X OR 234)", "Or(NumVar(X), L(234))")]
        [InlineData("(X(234) OR YZ1234)", "Or(Array(NumVar(X), L(234)), NumVar(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1 OR 2 OR 3", "Or(Or(L(1), L(2)), L(3))")]
        [InlineData("(1 OR 2 OR 3)", "Or(Or(L(1), L(2)), L(3))")]
        [InlineData("(1 OR 2) OR 3", "Or(Or(L(1), L(2)), L(3))")]
        [InlineData("1 OR (2 OR 3)", "Or(L(1), Or(L(2), L(3)))")]
        [Theory]
        public void WithThreeTerms(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(Z+1) OR (X-Y)", "Or(Add(NumVar(Z), L(1)), Subtract(NumVar(X), NumVar(Y)))")]
        [InlineData("Z+1 OR X-Y", "Or(Add(NumVar(Z), L(1)), Subtract(NumVar(X), NumVar(Y)))")]
        [InlineData("(Z*1) OR (X^Y)", "Or(Multiply(NumVar(Z), L(1)), Pow(NumVar(X), NumVar(Y)))")]
        [InlineData("Z*1 OR X^Y", "Or(Multiply(NumVar(Z), L(1)), Pow(NumVar(X), NumVar(Y)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+OR")]
        [InlineData("OR(1,X)")]
        [Theory]
        public void FailedReserved(string input)
        {
            Test.Bad(input);
        }
    }
}
