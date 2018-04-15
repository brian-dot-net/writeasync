// <copyright file="Or.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Or
    {
        [InlineData("1 OR 2", "Or(NumL(1), NumL(2))")]
        [InlineData("X OR 234", "Or(NumV(X), NumL(234))")]
        [InlineData("X(234) OR YZ1234", "Or(NumArr(X, NumL(234)), NumV(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1  OR 2", "Or(NumL(1), NumL(2))")]
        [InlineData("X OR  234", "Or(NumV(X), NumL(234))")]
        [InlineData("X(234)OR YZ1234", "Or(NumArr(X, NumL(234)), NumV(YZ1234))")]
        [Theory]
        public void IgnoreSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1 or 2", "Or(NumL(1), NumL(2))")]
        [InlineData("X oR 234", "Or(NumV(X), NumL(234))")]
        [InlineData("X(234) Or YZ1234", "Or(NumArr(X, NumL(234)), NumV(YZ1234))")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(1 OR 2)", "Or(NumL(1), NumL(2))")]
        [InlineData("(X OR 234)", "Or(NumV(X), NumL(234))")]
        [InlineData("(X(234) OR YZ1234)", "Or(NumArr(X, NumL(234)), NumV(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1 OR 2 OR 3", "Or(Or(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1 OR 2 OR 3)", "Or(Or(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1 OR 2) OR 3", "Or(Or(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1 OR 2)OR 3", "Or(Or(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("1 OR (2 OR 3)", "Or(NumL(1), Or(NumL(2), NumL(3)))")]
        [InlineData("1 OR(2 OR 3)", "Or(NumL(1), Or(NumL(2), NumL(3)))")]
        [Theory]
        public void WithThreeTerms(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(Z+1) OR (X-Y)", "Or(Add(NumV(Z), NumL(1)), Sub(NumV(X), NumV(Y)))")]
        [InlineData("Z+1 OR X-Y", "Or(Add(NumV(Z), NumL(1)), Sub(NumV(X), NumV(Y)))")]
        [InlineData("(Z*1) OR (X^Y)", "Or(Mult(NumV(Z), NumL(1)), Pow(NumV(X), NumV(Y)))")]
        [InlineData("Z*1 OR X^Y", "Or(Mult(NumV(Z), NumL(1)), Pow(NumV(X), NumV(Y)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(Z$<=\"x\") OR (X$=Y$)", "Or(Le(StrV(Z), StrL(x)), Eq(StrV(X), StrV(Y)))")]
        [InlineData("(Z$>\"x\") OR (X$<>Y$)", "Or(Gt(StrV(Z), StrL(x)), Ne(StrV(X), StrV(Y)))")]
        [Theory]
        public void WithOtherStringOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(Z$<=\"x\") OR (X=Y)", "Or(Le(StrV(Z), StrL(x)), Eq(NumV(X), NumV(Y)))")]
        [InlineData("(Z>1) OR (X$<>Y$)", "Or(Gt(NumV(Z), NumL(1)), Ne(StrV(X), StrV(Y)))")]
        [Theory]
        public void WithNumAndString(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+OR")]
        [InlineData("OR(1,X)")]
        [InlineData("OR$")]
        [InlineData("or$")]
        [InlineData("1+or")]
        [Theory]
        public void FailedReserved(string input)
        {
            Test.Bad(input);
        }

        [InlineData("OR1", "NumV(OR1)")]
        [InlineData("orX", "NumV(ORX)")]
        [InlineData("or1or$", "StrV(OR1OR)")]
        [Theory(Skip = "reserved prefix variables not working")]
        public void AllowedReservedPrefix(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
