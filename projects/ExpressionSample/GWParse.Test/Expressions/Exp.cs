// <copyright file="Exp.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Expressions
{
    using Xunit;

    public sealed class Exp
    {
        [InlineData("EXP(1)", "Exp(NumL(1))")]
        [InlineData("EXP(X)", "Exp(NumV(X))")]
        [InlineData("EXP(X(234))", "Exp(NumA(X, NumL(234)))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("EXP (1)", "Exp(NumL(1))")]
        [InlineData("EXP( X)", "Exp(NumV(X))")]
        [InlineData("EXP(  X(234)  )", "Exp(NumA(X, NumL(234)))")]
        [Theory]
        public void IgnoreSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("exp(1)", "Exp(NumL(1))")]
        [InlineData("ExP(X)", "Exp(NumV(X))")]
        [InlineData("eXp(X(234))", "Exp(NumA(X, NumL(234)))")]
        [Theory]
        public void IgnoreCase(string input, string output)
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

        [InlineData("EXP((1))", "Exp(NumL(1))")]
        [InlineData("EXP(EXP(X))", "Exp(Exp(NumV(X)))")]
        [InlineData("(EXP(X))", "Exp(NumV(X))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+EXP(-2)", "Add(NumL(1), Exp(Neg(NumL(2))))")]
        [InlineData("EXP(1+2)", "Exp(Add(NumL(1), NumL(2)))")]
        [InlineData("1-EXP(-2)", "Sub(NumL(1), Exp(Neg(NumL(2))))")]
        [InlineData("EXP(1-EXP(2))", "Exp(Sub(NumL(1), Exp(NumL(2))))")]
        [InlineData("EXP(1*2)", "Exp(Mult(NumL(1), NumL(2)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+EXP")]
        [InlineData("EXP(1,X)")]
        [InlineData("EXP$")]
        [InlineData("exp$")]
        [InlineData("1+exp")]
        [Theory]
        public void FailedReserved(string input)
        {
            Test.Bad(input);
        }

        [InlineData("EXP1", "NumV(EXP1)")]
        [InlineData("expX", "NumV(EXPX)")]
        [InlineData("exp1exp$", "StrV(EXP1EXP)")]
        [Theory]
        public void AllowedReservedPrefix(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
