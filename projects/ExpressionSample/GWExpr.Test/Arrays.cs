// <copyright file="Arrays.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Arrays
    {
        [InlineData("A(1)", "NumArr(A, NumL(1))")]
        [InlineData("AB(1)", "NumArr(AB, NumL(1))")]
        [InlineData("XYZ123(1)", "NumArr(XYZ123, NumL(1))")]
        [Theory]
        public void NumericWithLiteralIndex1D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A(B)", "NumArr(A, NumV(B))")]
        [InlineData("AB(CD123)", "NumArr(AB, NumV(CD123))")]
        [InlineData("XY5(ZZ)", "NumArr(XY5, NumV(ZZ))")]
        [Theory]
        public void NumericWithVarIndex1D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A(1,0)", "NumArr(A, NumL(1), NumL(0))")]
        [InlineData("AB(1,2)", "NumArr(AB, NumL(1), NumL(2))")]
        [InlineData("XYZ123(250,99)", "NumArr(XYZ123, NumL(250), NumL(99))")]
        [Theory]
        public void NumericWithLiteralIndex2D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A(1,B)", "NumArr(A, NumL(1), NumV(B))")]
        [InlineData("AB(CD123,2)", "NumArr(AB, NumV(CD123), NumL(2))")]
        [InlineData("XY5(ZZ,33)", "NumArr(XY5, NumV(ZZ), NumL(33))")]
        [Theory]
        public void NumericWithLiteralAndVarIndex2D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A$(1)", "StrArr(A, NumL(1))")]
        [InlineData("AB$(1)", "StrArr(AB, NumL(1))")]
        [InlineData("XYZ123$(1)", "StrArr(XYZ123, NumL(1))")]
        [Theory]
        public void StringWithLiteralIndex1D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A$(1,0)", "StrArr(A, NumL(1), NumL(0))")]
        [InlineData("AB$(1,2)", "StrArr(AB, NumL(1), NumL(2))")]
        [InlineData("XYZ123$(250,99)", "StrArr(XYZ123, NumL(250), NumL(99))")]
        [Theory]
        public void StringWithLiteralIndex2D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A$(1,B)", "StrArr(A, NumL(1), NumV(B))")]
        [InlineData("AB$(CD123,2)", "StrArr(AB, NumV(CD123), NumL(2))")]
        [InlineData("XY5$(ZZ,33)", "StrArr(XY5, NumV(ZZ), NumL(33))")]
        [Theory]
        public void StringWithLiteralAndVarIndex2D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("a(1)", "NumArr(A, NumL(1))")]
        [InlineData("Ab(1)", "NumArr(AB, NumL(1))")]
        [InlineData("xYz123(1)", "NumArr(XYZ123, NumL(1))")]
        [InlineData("a$(1)", "StrArr(A, NumL(1))")]
        [InlineData("Ab$(1)", "StrArr(AB, NumL(1))")]
        [InlineData("xYz123$(1)", "StrArr(XYZ123, NumL(1))")]
        [Theory]
        public void NameToUppercase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A$(\"bad\")")]
        [InlineData("XYZ123(\"bad\")")]
        [InlineData("Z(Z$)")]
        [Theory]
        public void FailNonNumericIndex(string input)
        {
            Test.Bad(input);
        }

        [InlineData("A(B(1,2))", "NumArr(A, NumArr(B, NumL(1), NumL(2)))")]
        [Theory]
        public void WithArraySubscript(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
