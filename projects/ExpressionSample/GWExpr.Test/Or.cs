// <copyright file="Or.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Or
    {
        [InlineData("1 OR 2", "Or(NumL(1), NumL(2))")]
        [InlineData("X OR 234", "Or(NumVar(X), NumL(234))")]
        [InlineData("X(234) OR YZ1234", "Or(Array(NumVar(X), NumL(234)), NumVar(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(1 OR 2)", "Or(NumL(1), NumL(2))")]
        [InlineData("(X OR 234)", "Or(NumVar(X), NumL(234))")]
        [InlineData("(X(234) OR YZ1234)", "Or(Array(NumVar(X), NumL(234)), NumVar(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1 OR 2 OR 3", "Or(Or(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1 OR 2 OR 3)", "Or(Or(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1 OR 2) OR 3", "Or(Or(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("1 OR (2 OR 3)", "Or(NumL(1), Or(NumL(2), NumL(3)))")]
        [Theory]
        public void WithThreeTerms(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(Z+1) OR (X-Y)", "Or(Add(NumVar(Z), NumL(1)), Sub(NumVar(X), NumVar(Y)))")]
        [InlineData("Z+1 OR X-Y", "Or(Add(NumVar(Z), NumL(1)), Sub(NumVar(X), NumVar(Y)))")]
        [InlineData("(Z*1) OR (X^Y)", "Or(Mult(NumVar(Z), NumL(1)), Pow(NumVar(X), NumVar(Y)))")]
        [InlineData("Z*1 OR X^Y", "Or(Mult(NumVar(Z), NumL(1)), Pow(NumVar(X), NumVar(Y)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(Z$<=\"x\") OR (X$=Y$)", "Or(Le(StrVar(Z), StrL(x)), Eq(StrVar(X), StrVar(Y)))")]
        [InlineData("(Z$>\"x\") OR (X$<>Y$)", "Or(Gt(StrVar(Z), StrL(x)), Ne(StrVar(X), StrVar(Y)))")]
        [Theory]
        public void WithOtherStringOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(Z$<=\"x\") OR (X=Y)", "Or(Le(StrVar(Z), StrL(x)), Eq(NumVar(X), NumVar(Y)))")]
        [InlineData("(Z>1) OR (X$<>Y$)", "Or(Gt(NumVar(Z), NumL(1)), Ne(StrVar(X), StrVar(Y)))")]
        [Theory]
        public void WithNumAndString(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+OR")]
        [InlineData("OR(1,X)")]
        [InlineData("OR$")]
        [Theory]
        public void FailedReserved(string input)
        {
            Test.Bad(input);
        }
    }
}
