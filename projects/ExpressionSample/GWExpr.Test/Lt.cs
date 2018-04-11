// <copyright file="Lt.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Lt
    {
        [InlineData("1<2", "Lt(Literal(1), Literal(2))")]
        [InlineData("X<234", "Lt(NumVar(X), Literal(234))")]
        [InlineData("X(234)<YZ1234", "Lt(Array(NumVar(X), Literal(234)), NumVar(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("2<\"1\"")]
        [InlineData("234<X$")]
        [InlineData("X(234)<YZ1234$")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("(1<2)", "Lt(Literal(1), Literal(2))")]
        [InlineData("(X<234)", "Lt(NumVar(X), Literal(234))")]
        [InlineData("(X(234)<YZ1234)", "Lt(Array(NumVar(X), Literal(234)), NumVar(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1<2<3", "Lt(Lt(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1<2<3)", "Lt(Lt(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1<2)<3", "Lt(Lt(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("1<(2<3)", "Lt(Literal(1), Lt(Literal(2), Literal(3)))")]
        [Theory]
        public void WithThreeTerms(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(Z+1)<(X-Y)", "Lt(Add(NumVar(Z), Literal(1)), Subtract(NumVar(X), NumVar(Y)))")]
        [InlineData("Z+1<X-Y", "Lt(Add(NumVar(Z), Literal(1)), Subtract(NumVar(X), NumVar(Y)))")]
        [InlineData("(Z*1)<(X^Y)", "Lt(Multiply(NumVar(Z), Literal(1)), Pow(NumVar(X), NumVar(Y)))")]
        [InlineData("Z*1<X^Y", "Lt(Multiply(NumVar(Z), Literal(1)), Pow(NumVar(X), NumVar(Y)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
