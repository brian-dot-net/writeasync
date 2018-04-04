// <copyright file="Arrays.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using System;
    using FluentAssertions;
    using Xunit;

    public sealed class Arrays
    {
        [InlineData("A(1)", "ArrayVariable(NumericVariable(A), NumericLiteral(1))")]
        [InlineData("AB(1)", "ArrayVariable(NumericVariable(AB), NumericLiteral(1))")]
        [InlineData("XYZ123(1)", "ArrayVariable(NumericVariable(XYZ123), NumericLiteral(1))")]
        [Theory]
        public void NumericWithLiteralIndex1D(string input, string output)
        {
            Test(input, output);
        }

        [InlineData("A(B)", "ArrayVariable(NumericVariable(A), NumericVariable(B))")]
        [InlineData("AB(CD123)", "ArrayVariable(NumericVariable(AB), NumericVariable(CD123))")]
        [InlineData("XY5(ZZ)", "ArrayVariable(NumericVariable(XY5), NumericVariable(ZZ))")]
        [Theory]
        public void NumericWithVarIndex1D(string input, string output)
        {
            Test(input, output);
        }

        [InlineData("A(1,0)", "ArrayVariable(NumericVariable(A), NumericLiteral(1), NumericLiteral(0))")]
        [InlineData("AB(1,2)", "ArrayVariable(NumericVariable(AB), NumericLiteral(1), NumericLiteral(2))")]
        [InlineData("XYZ123(250,99)", "ArrayVariable(NumericVariable(XYZ123), NumericLiteral(250), NumericLiteral(99))")]
        [Theory]
        public void NumericWithLiteralIndex2D(string input, string output)
        {
            Test(input, output);
        }

        [InlineData("A(1,B)", "ArrayVariable(NumericVariable(A), NumericLiteral(1), NumericVariable(B))")]
        [InlineData("AB(CD123,2)", "ArrayVariable(NumericVariable(AB), NumericVariable(CD123), NumericLiteral(2))")]
        [InlineData("XY5(ZZ,33)", "ArrayVariable(NumericVariable(XY5), NumericVariable(ZZ), NumericLiteral(33))")]
        [Theory]
        public void NumericWithLiteralAndVarIndex2D(string input, string output)
        {
            Test(input, output);
        }

        [InlineData("A$(1)", "ArrayVariable(StringVariable(A), NumericLiteral(1))")]
        [InlineData("AB$(1)", "ArrayVariable(StringVariable(AB), NumericLiteral(1))")]
        [InlineData("XYZ123$(1)", "ArrayVariable(StringVariable(XYZ123), NumericLiteral(1))")]
        [Theory]
        public void StringWithLiteralIndex1D(string input, string output)
        {
            Test(input, output);
        }

        [InlineData("A$(1,0)", "ArrayVariable(StringVariable(A), NumericLiteral(1), NumericLiteral(0))")]
        [InlineData("AB$(1,2)", "ArrayVariable(StringVariable(AB), NumericLiteral(1), NumericLiteral(2))")]
        [InlineData("XYZ123$(250,99)", "ArrayVariable(StringVariable(XYZ123), NumericLiteral(250), NumericLiteral(99))")]
        [Theory]
        public void StringWithLiteralIndex2D(string input, string output)
        {
            Test(input, output);
        }

        [InlineData("A$(1,B)", "ArrayVariable(StringVariable(A), NumericLiteral(1), NumericVariable(B))")]
        [InlineData("AB$(CD123,2)", "ArrayVariable(StringVariable(AB), NumericVariable(CD123), NumericLiteral(2))")]
        [InlineData("XY5$(ZZ,33)", "ArrayVariable(StringVariable(XY5), NumericVariable(ZZ), NumericLiteral(33))")]
        [Theory]
        public void StringWithLiteralAndVarIndex2D(string input, string output)
        {
            Test(input, output);
        }

        [InlineData("A$(\"bad\")")]
        [InlineData("XYZ123(\"bad\")")]
        [Theory]
        public void FailNonNumericIndex(string input)
        {
            Action act = () => BasicExpression.FromString(input);

            act.Should().Throw<FormatException>().WithMessage("*'" + input + "'*").WithInnerException<Exception>();
        }

        private static void Test(string input, string output)
        {
            BasicExpression.FromString(input).ToString().Should().Be(output);
        }
    }
}
