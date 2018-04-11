// <copyright file="Le.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Le
    {
        [InlineData("1<=2", "Le(Literal(1), Literal(2))")]
        [InlineData("X<=234", "Le(NumVar(X), Literal(234))")]
        [InlineData("X(234)<=YZ1234", "Le(Array(NumVar(X), Literal(234)), NumVar(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("2<=\"1\"")]
        [InlineData("234<=X$")]
        [InlineData("X(234)<=YZ1234$")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("(1<=2)", "Le(Literal(1), Literal(2))")]
        [InlineData("(X<=234)", "Le(NumVar(X), Literal(234))")]
        [InlineData("(X(234)<=YZ1234)", "Le(Array(NumVar(X), Literal(234)), NumVar(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1<=2<=3", "Le(Le(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1<=2<=3)", "Le(Le(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1<=2)<=3", "Le(Le(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("1<=(2<=3)", "Le(Literal(1), Le(Literal(2), Literal(3)))")]
        [Theory]
        public void WithThreeTerms(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(Z+1)<=(X-Y)", "Le(Add(NumVar(Z), Literal(1)), Subtract(NumVar(X), NumVar(Y)))")]
        [InlineData("Z+1<=X-Y", "Le(Add(NumVar(Z), Literal(1)), Subtract(NumVar(X), NumVar(Y)))")]
        [InlineData("(Z*1)<=(X^Y)", "Le(Multiply(NumVar(Z), Literal(1)), Pow(NumVar(X), NumVar(Y)))")]
        [InlineData("Z*1<=X^Y", "Le(Multiply(NumVar(Z), Literal(1)), Pow(NumVar(X), NumVar(Y)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
