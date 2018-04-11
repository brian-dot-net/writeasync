// <copyright file="Gt.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Gt
    {
        [InlineData("1>2", "Gt(Literal(1), Literal(2))")]
        [InlineData("X>234", "Gt(NumVar(X), Literal(234))")]
        [InlineData("X(234)>YZ1234", "Gt(Array(NumVar(X), Literal(234)), NumVar(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("2>\"1\"")]
        [InlineData("234>X$")]
        [InlineData("X(234)>YZ1234$")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("(1>2)", "Gt(Literal(1), Literal(2))")]
        [InlineData("(X>234)", "Gt(NumVar(X), Literal(234))")]
        [InlineData("(X(234)>YZ1234)", "Gt(Array(NumVar(X), Literal(234)), NumVar(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1>2>3", "Gt(Gt(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1>2>3)", "Gt(Gt(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1>2)>3", "Gt(Gt(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("1>(2>3)", "Gt(Literal(1), Gt(Literal(2), Literal(3)))")]
        [Theory]
        public void WithThreeTerms(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(Z+1)>(X-Y)", "Gt(Add(NumVar(Z), Literal(1)), Subtract(NumVar(X), NumVar(Y)))")]
        [InlineData("Z+1>X-Y", "Gt(Add(NumVar(Z), Literal(1)), Subtract(NumVar(X), NumVar(Y)))")]
        [InlineData("(Z*1)>(X^Y)", "Gt(Multiply(NumVar(Z), Literal(1)), Pow(NumVar(X), NumVar(Y)))")]
        [InlineData("Z*1>X^Y", "Gt(Multiply(NumVar(Z), Literal(1)), Pow(NumVar(X), NumVar(Y)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
