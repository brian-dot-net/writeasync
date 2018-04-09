// <copyright file="And.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class And
    {
        [InlineData("1 AND 2", "And(Literal(1), Literal(2))")]
        [InlineData("X AND 234", "And(NumVar(X), Literal(234))")]
        [InlineData("X(234) AND YZ1234", "And(Array(NumVar(X), Literal(234)), NumVar(YZ1234))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("2 AND \"1\"")]
        [InlineData("234 AND X$")]
        [InlineData("X(234) AND YZ1234$")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("(1 AND 2)", "And(Literal(1), Literal(2))")]
        [InlineData("(X AND 234)", "And(NumVar(X), Literal(234))")]
        [InlineData("(X(234) AND YZ1234)", "And(Array(NumVar(X), Literal(234)), NumVar(YZ1234))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1 AND 2 AND 3", "And(And(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1 AND 2 AND 3)", "And(And(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("(1 AND 2) AND 3", "And(And(Literal(1), Literal(2)), Literal(3))")]
        [InlineData("1 AND (2 AND 3)", "And(Literal(1), And(Literal(2), Literal(3)))")]
        [Theory]
        public void WithThreeTerms(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("(Z+1) AND (X-Y)", "And(Add(NumVar(Z), Literal(1)), Subtract(NumVar(X), NumVar(Y)))")]
        [InlineData("Z+1 AND X-Y", "And(Add(NumVar(Z), Literal(1)), Subtract(NumVar(X), NumVar(Y)))")]
        [InlineData("(Z*1) AND (X^Y)", "And(Multiply(NumVar(Z), Literal(1)), Pow(NumVar(X), NumVar(Y)))")]
        [InlineData("Z*1 AND X^Y", "And(Multiply(NumVar(Z), Literal(1)), Pow(NumVar(X), NumVar(Y)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
