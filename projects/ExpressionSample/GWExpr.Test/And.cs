// <copyright file="And.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class And
    {
        [InlineData("1 AND 2", "And(NumL(1), NumL(2))")]
        [InlineData("X AND 234", "And(NumV(X), NumL(234))")]
        [InlineData("X(234) AND YZ1234", "And(NumArr(X, NumL(234)), NumV(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1  AND 2", "And(NumL(1), NumL(2))")]
        [InlineData("X  AND  234", "And(NumV(X), NumL(234))")]
        [InlineData("X(234)AND  YZ1234", "And(NumArr(X, NumL(234)), NumV(YZ1234))")]
        [Theory]
        public void IgnoreSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1 and 2", "And(NumL(1), NumL(2))")]
        [InlineData("X AnD 234", "And(NumV(X), NumL(234))")]
        [InlineData("X(234) aNd YZ1234", "And(NumArr(X, NumL(234)), NumV(YZ1234))")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(1 AND 2)", "And(NumL(1), NumL(2))")]
        [InlineData("(X AND 234)", "And(NumV(X), NumL(234))")]
        [InlineData("(X(234) AND YZ1234)", "And(NumArr(X, NumL(234)), NumV(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1 AND 2 AND 3", "And(And(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1 AND 2 AND 3)", "And(And(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1 AND 2) AND 3", "And(And(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1 AND 2)AND 3", "And(And(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("1 AND (2 AND 3)", "And(NumL(1), And(NumL(2), NumL(3)))")]
        [InlineData("1 AND(2 AND 3)", "And(NumL(1), And(NumL(2), NumL(3)))")]
        [Theory]
        public void WithThreeTerms(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(Z+1) AND (X-Y)", "And(Add(NumV(Z), NumL(1)), Sub(NumV(X), NumV(Y)))")]
        [InlineData("Z+1 AND X-Y", "And(Add(NumV(Z), NumL(1)), Sub(NumV(X), NumV(Y)))")]
        [InlineData("(Z*1) AND (X^Y)", "And(Mult(NumV(Z), NumL(1)), Pow(NumV(X), NumV(Y)))")]
        [InlineData("Z*1 AND X^Y", "And(Mult(NumV(Z), NumL(1)), Pow(NumV(X), NumV(Y)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(Z$<=\"x\") AND (X$=Y$)", "And(Le(StrV(Z), StrL(x)), Eq(StrV(X), StrV(Y)))")]
        [InlineData("(Z$>\"x\") AND (X$<>Y$)", "And(Gt(StrV(Z), StrL(x)), Ne(StrV(X), StrV(Y)))")]
        [Theory]
        public void WithOtherStringOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(Z$<=\"x\") AND (X=Y)", "And(Le(StrV(Z), StrL(x)), Eq(NumV(X), NumV(Y)))")]
        [InlineData("(Z>1) AND (X$<>Y$)", "And(Gt(NumV(Z), NumL(1)), Ne(StrV(X), StrV(Y)))")]
        [Theory]
        public void WithNumAndString(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+AND")]
        [InlineData("AND(1,X)")]
        [InlineData("AND$")]
        [InlineData("and$")]
        [InlineData("1+and")]
        [Theory]
        public void FailedReserved(string input)
        {
            Test.Bad(input);
        }

        [InlineData("AND1", "NumV(AND1)")]
        [InlineData("andX", "NumV(ANDX)")]
        [InlineData("and1and$", "StrV(AND1AND)")]
        [Theory(Skip = "reserved prefix variables not working")]
        public void AllowedReservedPrefix(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
