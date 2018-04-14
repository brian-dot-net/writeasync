// <copyright file="Right.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Right
    {
        [InlineData("RIGHT$(\"x\",1)", "Right(L(\"x\"), L(1))")]
        [InlineData("RIGHT$(X$,X)", "Right(StrVar(X), NumVar(X))")]
        [InlineData("RIGHT$(X$(234),X(123))", "Right(Array(StrVar(X), L(234)), Array(NumVar(X), L(123)))")]
        [Theory]
        public void String(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("RIGHT$(\"x\")")]
        [InlineData("RIGHT$(X$)")]
        [InlineData("RIGHT$(X$(X,Y))")]
        [Theory]
        public void TooFewArguments(string input)
        {
            Test.Bad(input);
        }

        [InlineData("RIGHT$(1,\"x\")")]
        [InlineData("RIGHT$(X,X$)")]
        [InlineData("RIGHT$(A$,B$)")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("RIGHT$((\"x\"),1)", "Right(L(\"x\"), L(1))")]
        [InlineData("RIGHT$((X$(X)),2)", "Right(Array(StrVar(X), NumVar(X)), L(2))")]
        [InlineData("(RIGHT$(X$,1))", "Right(StrVar(X), L(1))")]
        [InlineData("RIGHT$(RIGHT$(X$,1),2)", "Right(Right(StrVar(X), L(1)), L(2))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("\"x\"+RIGHT$(X$,1)", "Add(L(\"x\"), Right(StrVar(X), L(1)))")]
        [InlineData("RIGHT$(\"a\"+\"b\",1)", "Right(Add(L(\"a\"), L(\"b\")), L(1))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+RIGHT", "Add(L(1), NumVar(RIGHT))")]
        [InlineData("RIGHT*3", "Mult(NumVar(RIGHT), L(3))")]
        [Theory]
        public void AllowReservedNum(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("\"x\"+RIGHT$")]
        [InlineData("RIGHT$")]
        [Theory]
        public void FailedReservedString(string input)
        {
            Test.Bad(input);
        }
    }
}
