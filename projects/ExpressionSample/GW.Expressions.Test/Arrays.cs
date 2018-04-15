// <copyright file="Arrays.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GW.Expressions.Test
{
    using Xunit;

    public sealed class Arrays
    {
        [InlineData("A(1)", "NumA(A, NumL(1))")]
        [InlineData("AB(1)", "NumA(AB, NumL(1))")]
        [InlineData("XYZ123(1)", "NumA(XYZ123, NumL(1))")]
        [Theory]
        public void NumericWithLiteralIndex1D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A(B)", "NumA(A, NumV(B))")]
        [InlineData("AB(CD123)", "NumA(AB, NumV(CD123))")]
        [InlineData("XY5(ZZ)", "NumA(XY5, NumV(ZZ))")]
        [Theory]
        public void NumericWithVarIndex1D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A(1,0)", "NumA(A, NumL(1), NumL(0))")]
        [InlineData("AB(1,2)", "NumA(AB, NumL(1), NumL(2))")]
        [InlineData("XYZ123(250,99)", "NumA(XYZ123, NumL(250), NumL(99))")]
        [Theory]
        public void NumericWithLiteralIndex2D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A(1,B)", "NumA(A, NumL(1), NumV(B))")]
        [InlineData("AB(CD123,2)", "NumA(AB, NumV(CD123), NumL(2))")]
        [InlineData("XY5(ZZ,33)", "NumA(XY5, NumV(ZZ), NumL(33))")]
        [Theory]
        public void NumericWithLiteralAndVarIndex2D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A$(1)", "StrA(A, NumL(1))")]
        [InlineData("AB$(1)", "StrA(AB, NumL(1))")]
        [InlineData("XYZ123$(1)", "StrA(XYZ123, NumL(1))")]
        [Theory]
        public void StringWithLiteralIndex1D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A$(1,0)", "StrA(A, NumL(1), NumL(0))")]
        [InlineData("AB$(1,2)", "StrA(AB, NumL(1), NumL(2))")]
        [InlineData("XYZ123$(250,99)", "StrA(XYZ123, NumL(250), NumL(99))")]
        [Theory]
        public void StringWithLiteralIndex2D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A$(1,B)", "StrA(A, NumL(1), NumV(B))")]
        [InlineData("AB$(CD123,2)", "StrA(AB, NumV(CD123), NumL(2))")]
        [InlineData("XY5$(ZZ,33)", "StrA(XY5, NumV(ZZ), NumL(33))")]
        [Theory]
        public void StringWithLiteralAndVarIndex2D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("a(1)", "NumA(A, NumL(1))")]
        [InlineData("Ab(1)", "NumA(AB, NumL(1))")]
        [InlineData("xYz123(1)", "NumA(XYZ123, NumL(1))")]
        [InlineData("a$(1)", "StrA(A, NumL(1))")]
        [InlineData("Ab$(1)", "StrA(AB, NumL(1))")]
        [InlineData("xYz123$(1)", "StrA(XYZ123, NumL(1))")]
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

        [InlineData("A(B(1,2))", "NumA(A, NumA(B, NumL(1), NumL(2)))")]
        [Theory]
        public void WithArraySubscript(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData(" A(1)", "NumA(A, NumL(1))")]
        [InlineData("A (1)", "NumA(A, NumL(1))")]
        [InlineData("  A  (  1  )  ", "NumA(A, NumL(1))")]
        [InlineData("  A  (  1 , 2  )  ", "NumA(A, NumL(1), NumL(2))")]
        [Theory]
        public void IgnoreSpaces(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
