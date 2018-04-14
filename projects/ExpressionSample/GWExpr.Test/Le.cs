// <copyright file="Le.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Le
    {
        [InlineData("1<=2", "Le(NumL(1), NumL(2))")]
        [InlineData("X<=234", "Le(NumVar(X), NumL(234))")]
        [InlineData("X(234)<=YZ1234", "Le(Array(NumVar(X), NumL(234)), NumVar(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("\"one\"<=\"two\"", "Le(StrL(one), StrL(two))")]
        [InlineData("X$<=\"abc\"", "Le(StrVar(X), StrL(abc))")]
        [InlineData("X$(234)<=YZ1234$", "Le(Array(StrVar(X), NumL(234)), StrVar(YZ1234))")]
        [Theory]
        public void String(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("2<=\"1\"")]
        [InlineData("234<=X$")]
        [InlineData("X(234)<=YZ1234$")]
        [InlineData("A$<=B$<=\"C\"")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("(1<=2)", "Le(NumL(1), NumL(2))")]
        [InlineData("(X<=234)", "Le(NumVar(X), NumL(234))")]
        [InlineData("(X(234)<=YZ1234)", "Le(Array(NumVar(X), NumL(234)), NumVar(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1<=2<=3", "Le(Le(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1<=2<=3)", "Le(Le(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1<=2)<=3", "Le(Le(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("1<=(2<=3)", "Le(NumL(1), Le(NumL(2), NumL(3)))")]
        [Theory]
        public void WithThreeTerms(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(Z+1)<=(X-Y)", "Le(Add(NumVar(Z), NumL(1)), Sub(NumVar(X), NumVar(Y)))")]
        [InlineData("Z+1<=X-Y", "Le(Add(NumVar(Z), NumL(1)), Sub(NumVar(X), NumVar(Y)))")]
        [InlineData("(Z*1)<=(X^Y)", "Le(Mult(NumVar(Z), NumL(1)), Pow(NumVar(X), NumVar(Y)))")]
        [InlineData("Z*1<=X^Y", "Le(Mult(NumVar(Z), NumL(1)), Pow(NumVar(X), NumVar(Y)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
