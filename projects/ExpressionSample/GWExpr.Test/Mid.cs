// <copyright file="Mid.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Mid
    {
        [InlineData("MID$(\"x\",1)", "Mid(StrL(x), NumL(1))")]
        [InlineData("MID$(X$,X)", "Mid(StrVar(X), NumVar(X))")]
        [InlineData("MID$(X$(234),X(123))", "Mid(Array(StrVar(X), NumL(234)), Array(NumVar(X), NumL(123)))")]
        [InlineData("MID$(\"x\",1,2)", "Mid(StrL(x), NumL(1), NumL(2))")]
        [InlineData("MID$(X$,X,Y)", "Mid(StrVar(X), NumVar(X), NumVar(Y))")]
        [InlineData("MID$(X$(234),X(123),Y(1))", "Mid(Array(StrVar(X), NumL(234)), Array(NumVar(X), NumL(123)), Array(NumVar(Y), NumL(1)))")]
        [Theory]
        public void String(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("MID$(\"x\")")]
        [InlineData("MID$(X$)")]
        [InlineData("MID$(X$(X,Y))")]
        [Theory]
        public void TooFewArguments(string input)
        {
            Test.Bad(input);
        }

        [InlineData("MID$(1,\"x\")")]
        [InlineData("MID$(X,X$)")]
        [InlineData("MID$(A$,B$)")]
        [InlineData("MID$(A$,1,B$)")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("MID$((\"x\"),1)", "Mid(StrL(x), NumL(1))")]
        [InlineData("MID$((X$(X)),2)", "Mid(Array(StrVar(X), NumVar(X)), NumL(2))")]
        [InlineData("(MID$(X$,1))", "Mid(StrVar(X), NumL(1))")]
        [InlineData("MID$(MID$(X$,1),2)", "Mid(Mid(StrVar(X), NumL(1)), NumL(2))")]
        [InlineData("MID$(MID$(X$,1),2,(3))", "Mid(Mid(StrVar(X), NumL(1)), NumL(2), NumL(3))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("\"x\"+MID$(X$,1)", "Add(StrL(x), Mid(StrVar(X), NumL(1)))")]
        [InlineData("MID$(\"a\"+\"b\",1)", "Mid(Add(StrL(a), StrL(b)), NumL(1))")]
        [InlineData("MID$(\"a\"+\"b\",1,3+4)", "Mid(Add(StrL(a), StrL(b)), NumL(1), Add(NumL(3), NumL(4)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+MID", "Add(NumL(1), NumVar(MID))")]
        [InlineData("MID*3", "Mult(NumVar(MID), NumL(3))")]
        [Theory]
        public void AllowReservedNum(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("\"x\"+MID$")]
        [InlineData("MID$")]
        [Theory]
        public void FailedReservedString(string input)
        {
            Test.Bad(input);
        }
    }
}
