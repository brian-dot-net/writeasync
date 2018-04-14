// <copyright file="Exp.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Exp
    {
        [InlineData("EXP(1)", "Exp(L(1))")]
        [InlineData("EXP(X)", "Exp(NumVar(X))")]
        [InlineData("EXP(X(234))", "Exp(Array(NumVar(X), L(234)))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("EXP(\"1\")")]
        [InlineData("EXP(X$)")]
        [InlineData("EXP(YZ1234$)")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("EXP((1))", "Exp(L(1))")]
        [InlineData("EXP(EXP(X))", "Exp(Exp(NumVar(X)))")]
        [InlineData("(EXP(X))", "Exp(NumVar(X))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+EXP(-2)", "Add(L(1), Exp(Neg(L(2))))")]
        [InlineData("EXP(1+2)", "Exp(Add(L(1), L(2)))")]
        [InlineData("1-EXP(-2)", "Sub(L(1), Exp(Neg(L(2))))")]
        [InlineData("EXP(1-EXP(2))", "Exp(Sub(L(1), Exp(L(2))))")]
        [InlineData("EXP(1*2)", "Exp(Mult(L(1), L(2)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+EXP")]
        [InlineData("EXP(1,X)")]
        [InlineData("EXP$")]
        [Theory]
        public void FailedReserved(string input)
        {
            Test.Bad(input);
        }
    }
}
