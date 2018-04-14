// <copyright file="Len.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Len
    {
        [InlineData("LEN(\"x\")", "Len(StrL(x))")]
        [InlineData("LEN(X$)", "Len(StrV(X))")]
        [InlineData("LEN(X$(234))", "Len(StrArr(X, NumL(234)))")]
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

        [InlineData("LEN((\"x\"))", "Len(StrL(x))")]
        [InlineData("LEN((X$(X)))", "Len(StrArr(X, NumV(X)))")]
        [InlineData("(LEN(X$))", "Len(StrV(X))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+LEN(X$)", "Add(NumL(1), Len(StrV(X)))")]
        [InlineData("LEN(\"a\"+\"b\")", "Len(Add(StrL(a), StrL(b)))")]
        [InlineData("1-LEN(\"a\")", "Sub(NumL(1), Len(StrL(a)))")]
        [InlineData("1*LEN(X$)", "Mult(NumL(1), Len(StrV(X)))")]
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
