// <copyright file="Arrays.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Arrays
    {
        [InlineData("A(1)", "Array(NumericVariable(A), NumericLiteral(1))")]
        [InlineData("AB(1)", "Array(NumericVariable(AB), NumericLiteral(1))")]
        [InlineData("XYZ123(1)", "Array(NumericVariable(XYZ123), NumericLiteral(1))")]
        [Theory]
        public void NumericWithLiteralIndex1D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A(B)", "Array(NumericVariable(A), NumericVariable(B))")]
        [InlineData("AB(CD123)", "Array(NumericVariable(AB), NumericVariable(CD123))")]
        [InlineData("XY5(ZZ)", "Array(NumericVariable(XY5), NumericVariable(ZZ))")]
        [Theory]
        public void NumericWithVarIndex1D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A(1,0)", "Array(NumericVariable(A), NumericLiteral(1), NumericLiteral(0))")]
        [InlineData("AB(1,2)", "Array(NumericVariable(AB), NumericLiteral(1), NumericLiteral(2))")]
        [InlineData("XYZ123(250,99)", "Array(NumericVariable(XYZ123), NumericLiteral(250), NumericLiteral(99))")]
        [Theory]
        public void NumericWithLiteralIndex2D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A(1,B)", "Array(NumericVariable(A), NumericLiteral(1), NumericVariable(B))")]
        [InlineData("AB(CD123,2)", "Array(NumericVariable(AB), NumericVariable(CD123), NumericLiteral(2))")]
        [InlineData("XY5(ZZ,33)", "Array(NumericVariable(XY5), NumericVariable(ZZ), NumericLiteral(33))")]
        [Theory]
        public void NumericWithLiteralAndVarIndex2D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A$(1)", "Array(StringVariable(A), NumericLiteral(1))")]
        [InlineData("AB$(1)", "Array(StringVariable(AB), NumericLiteral(1))")]
        [InlineData("XYZ123$(1)", "Array(StringVariable(XYZ123), NumericLiteral(1))")]
        [Theory]
        public void StringWithLiteralIndex1D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A$(1,0)", "Array(StringVariable(A), NumericLiteral(1), NumericLiteral(0))")]
        [InlineData("AB$(1,2)", "Array(StringVariable(AB), NumericLiteral(1), NumericLiteral(2))")]
        [InlineData("XYZ123$(250,99)", "Array(StringVariable(XYZ123), NumericLiteral(250), NumericLiteral(99))")]
        [Theory]
        public void StringWithLiteralIndex2D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("A$(1,B)", "Array(StringVariable(A), NumericLiteral(1), NumericVariable(B))")]
        [InlineData("AB$(CD123,2)", "Array(StringVariable(AB), NumericVariable(CD123), NumericLiteral(2))")]
        [InlineData("XY5$(ZZ,33)", "Array(StringVariable(XY5), NumericVariable(ZZ), NumericLiteral(33))")]
        [Theory]
        public void StringWithLiteralAndVarIndex2D(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("a(1)", "Array(NumericVariable(A), NumericLiteral(1))")]
        [InlineData("Ab(1)", "Array(NumericVariable(AB), NumericLiteral(1))")]
        [InlineData("xYz123(1)", "Array(NumericVariable(XYZ123), NumericLiteral(1))")]
        [InlineData("a$(1)", "Array(StringVariable(A), NumericLiteral(1))")]
        [InlineData("Ab$(1)", "Array(StringVariable(AB), NumericLiteral(1))")]
        [InlineData("xYz123$(1)", "Array(StringVariable(XYZ123), NumericLiteral(1))")]
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

        [InlineData("A(B(1,2))", "Array(NumericVariable(A), Array(NumericVariable(B), NumericLiteral(1), NumericLiteral(2)))")]
        [Theory]
        public void WithArraySubscript(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
