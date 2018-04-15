// <copyright file="Len.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Expressions
{
    using Xunit;

    public sealed class Len
    {
        [InlineData("LEN(\"x\")", "Len(StrL(\"x\"))")]
        [InlineData("LEN(X$)", "Len(StrV(X))")]
        [InlineData("LEN(X$(234))", "Len(StrA(X, NumL(234)))")]
        [Theory]
        public void String(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("LEN (\"x\")", "Len(StrL(\"x\"))")]
        [InlineData("LEN( X$)", "Len(StrV(X))")]
        [InlineData("LEN  (  X$(234)  )", "Len(StrA(X, NumL(234)))")]
        [Theory]
        public void IgnoreSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("len(\"x\")", "Len(StrL(\"x\"))")]
        [InlineData("Len(X$)", "Len(StrV(X))")]
        [InlineData("lEN(X$(234))", "Len(StrA(X, NumL(234)))")]
        [Theory]
        public void IgnoreCase(string input, string output)
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

        [InlineData("LEN((\"x\"))", "Len(StrL(\"x\"))")]
        [InlineData("LEN((X$(X)))", "Len(StrA(X, NumV(X)))")]
        [InlineData("(LEN(X$))", "Len(StrV(X))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+LEN(X$)", "Add(NumL(1), Len(StrV(X)))")]
        [InlineData("LEN(\"a\"+\"b\")", "Len(Add(StrL(\"a\"), StrL(\"b\")))")]
        [InlineData("1-LEN(\"a\")", "Sub(NumL(1), Len(StrL(\"a\")))")]
        [InlineData("1*LEN(X$)", "Mult(NumL(1), Len(StrV(X)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+LEN")]
        [InlineData("LEN(1,X)")]
        [InlineData("LEN$")]
        [InlineData("len$")]
        [InlineData("1+len")]
        [Theory]
        public void FailedReserved(string input)
        {
            Test.Bad(input);
        }

        [InlineData("LEN1", "NumV(LEN1)")]
        [InlineData("lenX", "NumV(LENX)")]
        [InlineData("len1len$", "StrV(LEN1LEN)")]
        [Theory(Skip = "reserved prefix variables not working")]
        public void AllowedReservedPrefix(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
