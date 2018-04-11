// <copyright file="Ne.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Ne
    {
        [InlineData("1<>2", "Ne(L(1), L(2))")]
        [InlineData("X<>234", "Ne(NumVar(X), L(234))")]
        [InlineData("X(234)<>YZ1234", "Ne(Array(NumVar(X), L(234)), NumVar(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("2<>\"1\"")]
        [InlineData("234<>X$")]
        [InlineData("X(234)<>YZ1234$")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("(1<>2)", "Ne(L(1), L(2))")]
        [InlineData("(X<>234)", "Ne(NumVar(X), L(234))")]
        [InlineData("(X(234)<>YZ1234)", "Ne(Array(NumVar(X), L(234)), NumVar(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1<>2<>3", "Ne(Ne(L(1), L(2)), L(3))")]
        [InlineData("(1<>2<>3)", "Ne(Ne(L(1), L(2)), L(3))")]
        [InlineData("(1<>2)<>3", "Ne(Ne(L(1), L(2)), L(3))")]
        [InlineData("1<>(2<>3)", "Ne(L(1), Ne(L(2), L(3)))")]
        [Theory]
        public void WithThreeTerms(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(Z+1)<>(X-Y)", "Ne(Add(NumVar(Z), L(1)), Subtract(NumVar(X), NumVar(Y)))")]
        [InlineData("Z+1<>X-Y", "Ne(Add(NumVar(Z), L(1)), Subtract(NumVar(X), NumVar(Y)))")]
        [InlineData("(Z*1)<>(X^Y)", "Ne(Multiply(NumVar(Z), L(1)), Pow(NumVar(X), NumVar(Y)))")]
        [InlineData("Z*1<>X^Y", "Ne(Multiply(NumVar(Z), L(1)), Pow(NumVar(X), NumVar(Y)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
