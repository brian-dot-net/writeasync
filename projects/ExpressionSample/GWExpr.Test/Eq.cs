// <copyright file="Eq.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Eq
    {
        [InlineData("1=2", "Eq(NumL(1), NumL(2))")]
        [InlineData("X=234", "Eq(NumVar(X), NumL(234))")]
        [InlineData("X(234)=YZ1234", "Eq(Array(NumVar(X), NumL(234)), NumVar(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("\"one\"=\"two\"", "Eq(StrL(one), StrL(two))")]
        [InlineData("X$=\"abc\"", "Eq(StrVar(X), StrL(abc))")]
        [InlineData("X$(234)=YZ1234$", "Eq(Array(StrVar(X), NumL(234)), StrVar(YZ1234))")]
        [Theory]
        public void String(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("2=\"1\"")]
        [InlineData("234=X$")]
        [InlineData("X(234)=YZ1234$")]
        [InlineData("A$=B$=\"C\"")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("(1=2)", "Eq(NumL(1), NumL(2))")]
        [InlineData("(X=234)", "Eq(NumVar(X), NumL(234))")]
        [InlineData("(X(234)=YZ1234)", "Eq(Array(NumVar(X), NumL(234)), NumVar(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1=2=3", "Eq(Eq(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1=2=3)", "Eq(Eq(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1=2)=3", "Eq(Eq(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("1=(2=3)", "Eq(NumL(1), Eq(NumL(2), NumL(3)))")]
        [Theory]
        public void WithThreeTerms(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(Z+1)=(X-Y)", "Eq(Add(NumVar(Z), NumL(1)), Sub(NumVar(X), NumVar(Y)))")]
        [InlineData("Z+1=X-Y", "Eq(Add(NumVar(Z), NumL(1)), Sub(NumVar(X), NumVar(Y)))")]
        [InlineData("(Z*1)=(X^Y)", "Eq(Mult(NumVar(Z), NumL(1)), Pow(NumVar(X), NumVar(Y)))")]
        [InlineData("Z*1=X^Y", "Eq(Mult(NumVar(Z), NumL(1)), Pow(NumVar(X), NumVar(Y)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
