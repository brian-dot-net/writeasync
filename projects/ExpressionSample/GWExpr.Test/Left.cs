// <copyright file="Left.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Left
    {
        [InlineData("LEFT$(\"x\",1)", "Left(StrL(\"x\"), NumL(1))")]
        [InlineData("LEFT$(X$,X)", "Left(StrV(X), NumV(X))")]
        [InlineData("LEFT$(X$(234),X(123))", "Left(StrA(X, NumL(234)), NumA(X, NumL(123)))")]
        [Theory]
        public void String(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("LEFT$ (\"x\",1)", "Left(StrL(\"x\"), NumL(1))")]
        [InlineData("LEFT$( X$,X)", "Left(StrV(X), NumV(X))")]
        [InlineData("LEFT$  (  X$(234)  ,  X(123)  )", "Left(StrA(X, NumL(234)), NumA(X, NumL(123)))")]
        [Theory]
        public void IgnoreSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("left$(\"x\",1)", "Left(StrL(\"x\"), NumL(1))")]
        [InlineData("Left$(X$,X)", "Left(StrV(X), NumV(X))")]
        [InlineData("LeFT$(X$(234),X(123))", "Left(StrA(X, NumL(234)), NumA(X, NumL(123)))")]
        [Theory]
        public void IgnoreCase(string input, string output)
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

        [InlineData("LEFT$((\"x\"),1)", "Left(StrL(\"x\"), NumL(1))")]
        [InlineData("LEFT$((X$(X)),2)", "Left(StrA(X, NumV(X)), NumL(2))")]
        [InlineData("(LEFT$(X$,1))", "Left(StrV(X), NumL(1))")]
        [InlineData("LEFT$(LEFT$(X$,1),2)", "Left(Left(StrV(X), NumL(1)), NumL(2))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("\"x\"+LEFT$(X$,1)", "Add(StrL(\"x\"), Left(StrV(X), NumL(1)))")]
        [InlineData("LEFT$(\"a\"+\"b\",1)", "Left(Add(StrL(\"a\"), StrL(\"b\")), NumL(1))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+LEFT", "Add(NumL(1), NumV(LEFT))")]
        [InlineData("LEFT*3", "Mult(NumV(LEFT), NumL(3))")]
        [InlineData("left*3", "Mult(NumV(LEFT), NumL(3))")]
        [Theory]
        public void AllowReservedNum(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("\"x\"+LEFT$")]
        [InlineData("LEFT$")]
        [InlineData("left$")]
        [Theory]
        public void FailedReservedString(string input)
        {
            Test.Bad(input);
        }

        [InlineData("LEFT1", "NumV(LEFT1)")]
        [InlineData("leftX", "NumV(LEFTX)")]
        [InlineData("left1left$", "StrV(LEFT1LEFT)")]
        [Theory(Skip = "reserved prefix variables not working")]
        public void AllowedReservedPrefix(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
