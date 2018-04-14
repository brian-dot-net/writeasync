// <copyright file="Left.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Left
    {
        [InlineData("LEFT$(\"x\",1)", "Left(L(\"x\"), L(1))")]
        [InlineData("LEFT$(X$,X)", "Left(StrVar(X), NumVar(X))")]
        [InlineData("LEFT$(X$(234),X(123))", "Left(Array(StrVar(X), L(234)), Array(NumVar(X), L(123)))")]
        [Theory]
        public void String(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("LEFT$(\"x\")")]
        [InlineData("LEFT$(X$)")]
        [InlineData("LEFT$(X$(X,Y))")]
        [Theory]
        public void TooFewArguments(string input)
        {
            Test.Bad(input);
        }

        [InlineData("LEFT$(1,\"x\")")]
        [InlineData("LEFT$(X,X$)")]
        [InlineData("LEFT$(A$,B$)")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("LEFT$((\"x\"),1)", "Left(L(\"x\"), L(1))")]
        [InlineData("LEFT$((X$(X)),2)", "Left(Array(StrVar(X), NumVar(X)), L(2))")]
        [InlineData("(LEFT$(X$,1))", "Left(StrVar(X), L(1))")]
        [InlineData("LEFT$(LEFT$(X$,1),2)", "Left(Left(StrVar(X), L(1)), L(2))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("\"x\"+LEFT$(X$,1)", "Add(L(\"x\"), Left(StrVar(X), L(1)))")]
        [InlineData("LEFT$(\"a\"+\"b\",1)", "Left(Add(L(\"a\"), L(\"b\")), L(1))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+LEFT", "Add(L(1), NumVar(LEFT))")]
        [InlineData("LEFT*3", "Mult(NumVar(LEFT), L(3))")]
        [Theory]
        public void AllowReservedNum(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("\"x\"+LEFT$")]
        [InlineData("LEFT$")]
        [Theory]
        public void FailedReservedString(string input)
        {
            Test.Bad(input);
        }
    }
}
