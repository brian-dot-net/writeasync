// <copyright file="Right.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Right
    {
        [InlineData("RIGHT$(\"x\",1)", "Right(StrL(\"x\"), NumL(1))")]
        [InlineData("RIGHT$(X$,X)", "Right(StrV(X), NumV(X))")]
        [InlineData("RIGHT$(X$(234),X(123))", "Right(StrArr(X, NumL(234)), NumArr(X, NumL(123)))")]
        [Theory]
        public void String(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("RIGHT$ (\"x\",1)", "Right(StrL(\"x\"), NumL(1))")]
        [InlineData("RIGHT$( X$,X)", "Right(StrV(X), NumV(X))")]
        [InlineData("RIGHT$  (  X$(234)  ,  X(123)  )", "Right(StrArr(X, NumL(234)), NumArr(X, NumL(123)))")]
        [Theory]
        public void IgnoreSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("right$(\"x\",1)", "Right(StrL(\"x\"), NumL(1))")]
        [InlineData("RiGhT$(X$,X)", "Right(StrV(X), NumV(X))")]
        [InlineData("rIgHt$(X$(234),X(123))", "Right(StrArr(X, NumL(234)), NumArr(X, NumL(123)))")]
        [Theory]
        public void IgnoreCase(string input, string output)
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

        [InlineData("RIGHT$((\"x\"),1)", "Right(StrL(\"x\"), NumL(1))")]
        [InlineData("RIGHT$((X$(X)),2)", "Right(StrArr(X, NumV(X)), NumL(2))")]
        [InlineData("(RIGHT$(X$,1))", "Right(StrV(X), NumL(1))")]
        [InlineData("RIGHT$(RIGHT$(X$,1),2)", "Right(Right(StrV(X), NumL(1)), NumL(2))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("\"x\"+RIGHT$(X$,1)", "Add(StrL(\"x\"), Right(StrV(X), NumL(1)))")]
        [InlineData("RIGHT$(\"a\"+\"b\",1)", "Right(Add(StrL(\"a\"), StrL(\"b\")), NumL(1))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+RIGHT", "Add(NumL(1), NumV(RIGHT))")]
        [InlineData("RIGHT*3", "Mult(NumV(RIGHT), NumL(3))")]
        [InlineData("right*3", "Mult(NumV(RIGHT), NumL(3))")]
        [Theory]
        public void AllowReservedNum(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("\"x\"+RIGHT$")]
        [InlineData("RIGHT$")]
        [InlineData("right$")]
        [Theory]
        public void FailedReservedString(string input)
        {
            Test.Bad(input);
        }

        [InlineData("RIGHT1", "NumV(RIGHT1)")]
        [InlineData("rightX", "NumV(RIGHTX)")]
        [InlineData("right1right$", "StrV(RIGHT1RIGHT)")]
        [Theory(Skip = "reserved prefix variables not working")]
        public void AllowedReservedPrefix(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
