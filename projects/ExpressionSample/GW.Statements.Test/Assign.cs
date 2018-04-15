// <copyright file="Assign.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GW.Statements.Test
{
    using Xunit;

    public sealed class Assign
    {
        [InlineData("R=1", "Assign(NumV(R), NumL(1))")]
        [InlineData("S1$=\"x\"", "Assign(StrV(S1), StrL(\"x\"))")]
        [InlineData("AR(X)=Z2(1)", "Assign(NumA(AR, NumV(X)), NumA(Z2, NumL(1)))")]
        [Theory]
        public void Valid(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("X=\"s\"", "Assign(NumV(X), StrL(\"s\"))")]
        [InlineData("X$=1", "Assign(StrV(X), NumL(1))")]
        [InlineData("X(1)=X$", "Assign(NumA(X, NumL(1)), StrV(X))")]
        [InlineData("X$(1)=X", "Assign(StrA(X, NumL(1)), NumV(X))")]
        [Theory]
        public void ValidButWrongType(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("r=1", "Assign(NumV(R), NumL(1))")]
        [InlineData("s1$=\"x\"", "Assign(StrV(S1), StrL(\"x\"))")]
        [InlineData("Ar(X)=z2(1)", "Assign(NumA(AR, NumV(X)), NumA(Z2, NumL(1)))")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData(" r=1", "Assign(NumV(R), NumL(1))")]
        [InlineData("r =1", "Assign(NumV(R), NumL(1))")]
        [InlineData("  r  =  1  ", "Assign(NumV(R), NumL(1))")]
        [Theory]
        public void AllowSpaces(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
