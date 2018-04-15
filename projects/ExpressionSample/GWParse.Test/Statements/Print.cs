// <copyright file="Print.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Statements
{
    using Xunit;

    public sealed class Print
    {
        [InlineData("PRINT", "Print()")]
        [Theory]
        public void Empty(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("PRINT 1", "Print(NumL(1))")]
        [InlineData("PRINT \"one\"", "Print(StrL(\"one\"))")]
        [InlineData("PRINT A", "Print(NumV(A))")]
        [InlineData("PRINT A$", "Print(StrV(A))")]
        [InlineData("PRINT A(1)", "Print(NumA(A, NumL(1)))")]
        [InlineData("PRINT A$(1)", "Print(StrA(A, NumL(1)))")]
        [Theory]
        public void OneItem(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("PRINT 1;\"two\"", "Print(NumL(1), StrL(\"two\"))")]
        [InlineData("PRINT \"one\";A", "Print(StrL(\"one\"), NumV(A))")]
        [InlineData("PRINT A;A$", "Print(NumV(A), StrV(A))")]
        [InlineData("PRINT A(1);A$(X)", "Print(NumA(A, NumL(1)), StrA(A, NumV(X)))")]
        [Theory]
        public void TwoItems(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("PRINT 1;\"two\";A3", "Print(NumL(1), StrL(\"two\"), NumV(A3))")]
        [InlineData("PRINT A;A$;A(X)", "Print(NumV(A), StrV(A), NumA(A, NumV(X)))")]
        [Theory]
        public void ThreeItems(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("print", "Print()")]
        [InlineData("PrInT", "Print()")]
        [InlineData("PrInT 1", "Print(NumL(1))")]
        [InlineData("PrInT a", "Print(NumV(A))")]
        [InlineData("PrInT a;b", "Print(NumV(A), NumV(B))")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData(" PRINT", "Print()")]
        [InlineData("PRINT ", "Print()")]
        [InlineData(" PRINT  ", "Print()")]
        [InlineData(" PRINT  1  ", "Print(NumL(1))")]
        [InlineData(" PRINT  1  ; 2", "Print(NumL(1), NumL(2))")]
        [Theory]
        public void IgnoreSpaces(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
