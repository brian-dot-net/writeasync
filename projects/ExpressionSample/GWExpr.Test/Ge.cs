// <copyright file="Ge.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Ge
    {
        [InlineData("1>=2", "Ge(NumL(1), NumL(2))")]
        [InlineData("X>=234", "Ge(NumV(X), NumL(234))")]
        [InlineData("X(234)>=YZ1234", "Ge(Array(NumV(X), NumL(234)), NumV(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("\"one\">=\"two\"", "Ge(StrL(one), StrL(two))")]
        [InlineData("X$>=\"abc\"", "Ge(StrV(X), StrL(abc))")]
        [InlineData("X$(234)>=YZ1234$", "Ge(Array(StrV(X), NumL(234)), StrV(YZ1234))")]
        [Theory]
        public void String(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("2>=\"1\"")]
        [InlineData("234>=X$")]
        [InlineData("X(234)>=YZ1234$")]
        [InlineData("A$>=B$>=\"C\"")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("(1>=2)", "Ge(NumL(1), NumL(2))")]
        [InlineData("(X>=234)", "Ge(NumV(X), NumL(234))")]
        [InlineData("(X(234)>=YZ1234)", "Ge(Array(NumV(X), NumL(234)), NumV(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1>=2>=3", "Ge(Ge(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1>=2>=3)", "Ge(Ge(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1>=2)>=3", "Ge(Ge(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("1>=(2>=3)", "Ge(NumL(1), Ge(NumL(2), NumL(3)))")]
        [Theory]
        public void WithThreeTerms(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(Z+1)>=(X-Y)", "Ge(Add(NumV(Z), NumL(1)), Sub(NumV(X), NumV(Y)))")]
        [InlineData("Z+1>=X-Y", "Ge(Add(NumV(Z), NumL(1)), Sub(NumV(X), NumV(Y)))")]
        [InlineData("(Z*1)>=(X^Y)", "Ge(Mult(NumV(Z), NumL(1)), Pow(NumV(X), NumV(Y)))")]
        [InlineData("Z*1>=X^Y", "Ge(Mult(NumV(Z), NumL(1)), Pow(NumV(X), NumV(Y)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
