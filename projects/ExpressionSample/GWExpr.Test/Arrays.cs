// <copyright file="Arrays.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Arrays
    {
        [InlineData("A(1)", "Array(NumVar(A), Literal(1))")]
        [InlineData("AB(1)", "Array(NumVar(AB), Literal(1))")]
        [InlineData("XYZ123(1)", "Array(NumVar(XYZ123), Literal(1))")]
        [Theory]
        public void NumericWithLiteralIndex1D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A(B)", "Array(NumVar(A), NumVar(B))")]
        [InlineData("AB(CD123)", "Array(NumVar(AB), NumVar(CD123))")]
        [InlineData("XY5(ZZ)", "Array(NumVar(XY5), NumVar(ZZ))")]
        [Theory]
        public void NumericWithVarIndex1D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A(1,0)", "Array(NumVar(A), Literal(1), Literal(0))")]
        [InlineData("AB(1,2)", "Array(NumVar(AB), Literal(1), Literal(2))")]
        [InlineData("XYZ123(250,99)", "Array(NumVar(XYZ123), Literal(250), Literal(99))")]
        [Theory]
        public void NumericWithLiteralIndex2D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A(1,B)", "Array(NumVar(A), Literal(1), NumVar(B))")]
        [InlineData("AB(CD123,2)", "Array(NumVar(AB), NumVar(CD123), Literal(2))")]
        [InlineData("XY5(ZZ,33)", "Array(NumVar(XY5), NumVar(ZZ), Literal(33))")]
        [Theory]
        public void NumericWithLiteralAndVarIndex2D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A$(1)", "Array(StrVar(A), Literal(1))")]
        [InlineData("AB$(1)", "Array(StrVar(AB), Literal(1))")]
        [InlineData("XYZ123$(1)", "Array(StrVar(XYZ123), Literal(1))")]
        [Theory]
        public void StringWithLiteralIndex1D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A$(1,0)", "Array(StrVar(A), Literal(1), Literal(0))")]
        [InlineData("AB$(1,2)", "Array(StrVar(AB), Literal(1), Literal(2))")]
        [InlineData("XYZ123$(250,99)", "Array(StrVar(XYZ123), Literal(250), Literal(99))")]
        [Theory]
        public void StringWithLiteralIndex2D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A$(1,B)", "Array(StrVar(A), Literal(1), NumVar(B))")]
        [InlineData("AB$(CD123,2)", "Array(StrVar(AB), NumVar(CD123), Literal(2))")]
        [InlineData("XY5$(ZZ,33)", "Array(StrVar(XY5), NumVar(ZZ), Literal(33))")]
        [Theory]
        public void StringWithLiteralAndVarIndex2D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("a(1)", "Array(NumVar(A), Literal(1))")]
        [InlineData("Ab(1)", "Array(NumVar(AB), Literal(1))")]
        [InlineData("xYz123(1)", "Array(NumVar(XYZ123), Literal(1))")]
        [InlineData("a$(1)", "Array(StrVar(A), Literal(1))")]
        [InlineData("Ab$(1)", "Array(StrVar(AB), Literal(1))")]
        [InlineData("xYz123$(1)", "Array(StrVar(XYZ123), Literal(1))")]
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

        [InlineData("A(B(1,2))", "Array(NumVar(A), Array(NumVar(B), Literal(1), Literal(2)))")]
        [Theory]
        public void WithArraySubscript(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
