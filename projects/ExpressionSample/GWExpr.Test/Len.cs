// <copyright file="Len.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Len
    {
        [InlineData("LEN(\"x\")", "Len(L(\"x\"))")]
        [InlineData("LEN(X$)", "Len(StrVar(X))")]
        [InlineData("LEN(X$(234))", "Len(Array(StrVar(X), L(234)))")]
        [Theory]
        public void String(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("LEN(1)")]
        [InlineData("LEN(X)")]
        [InlineData("LEN(YZ1234)")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("LEN((\"x\"))", "Len(L(\"x\"))")]
        [InlineData("LEN((X$(X)))", "Len(Array(StrVar(X), NumVar(X)))")]
        [InlineData("(LEN(X$))", "Len(StrVar(X))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+LEN(X$)", "Add(L(1), Len(StrVar(X)))")]
        [InlineData("LEN(\"a\"+\"b\")", "Len(Add(L(\"a\"), L(\"b\")))")]
        [InlineData("1-LEN(\"a\")", "Sub(L(1), Len(L(\"a\")))")]
        [InlineData("1*LEN(X$)", "Mult(L(1), Len(StrVar(X)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+LEN")]
        [InlineData("LEN(1,X)")]
        [InlineData("LEN$")]
        [Theory]
        public void FailedReserved(string input)
        {
            Test.Bad(input);
        }
    }
}
