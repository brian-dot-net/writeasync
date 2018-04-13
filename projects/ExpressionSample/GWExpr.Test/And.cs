// <copyright file="And.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class And
    {
        [InlineData("1 AND 2", "And(L(1), L(2))")]
        [InlineData("X AND 234", "And(NumVar(X), L(234))")]
        [InlineData("X(234) AND YZ1234", "And(Array(NumVar(X), L(234)), NumVar(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("2 AND \"1\"")]
        [InlineData("234 AND X$")]
        [InlineData("X(234) AND YZ1234$")]
        [InlineData("X$+2 AND 1")]
        [InlineData("X$+2 AND \"1\"")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("(1 AND 2)", "And(L(1), L(2))")]
        [InlineData("(X AND 234)", "And(NumVar(X), L(234))")]
        [InlineData("(X(234) AND YZ1234)", "And(Array(NumVar(X), L(234)), NumVar(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1 AND 2 AND 3", "And(And(L(1), L(2)), L(3))")]
        [InlineData("(1 AND 2 AND 3)", "And(And(L(1), L(2)), L(3))")]
        [InlineData("(1 AND 2) AND 3", "And(And(L(1), L(2)), L(3))")]
        [InlineData("1 AND (2 AND 3)", "And(L(1), And(L(2), L(3)))")]
        [Theory]
        public void WithThreeTerms(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(Z+1) AND (X-Y)", "And(Add(NumVar(Z), L(1)), Sub(NumVar(X), NumVar(Y)))")]
        [InlineData("Z+1 AND X-Y", "And(Add(NumVar(Z), L(1)), Sub(NumVar(X), NumVar(Y)))")]
        [InlineData("(Z*1) AND (X^Y)", "And(Mult(NumVar(Z), L(1)), Pow(NumVar(X), NumVar(Y)))")]
        [InlineData("Z*1 AND X^Y", "And(Mult(NumVar(Z), L(1)), Pow(NumVar(X), NumVar(Y)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(Z$<=\"x\") AND (X$=Y$)", "And(Le(StrVar(Z), L(\"x\")), Eq(StrVar(X), StrVar(Y)))")]
        [InlineData("(Z$>\"x\") AND (X$<>Y$)", "And(Gt(StrVar(Z), L(\"x\")), Ne(StrVar(X), StrVar(Y)))")]
        [Theory]
        public void WithOtherStringOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+AND")]
        [InlineData("AND(1,X)")]
        [Theory]
        public void FailedReserved(string input)
        {
            Test.Bad(input);
        }
    }
}
