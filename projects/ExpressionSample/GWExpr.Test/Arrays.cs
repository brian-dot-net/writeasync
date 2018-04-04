// <copyright file="Arrays.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
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

        [InlineData("A(1,0)", "ArrayVariable(NumericVariable(A), NumericLiteral(1), NumericLiteral(0))")]
        [InlineData("AB(1,2)", "ArrayVariable(NumericVariable(AB), NumericLiteral(1), NumericLiteral(2))")]
        [InlineData("XYZ123(250,99)", "ArrayVariable(NumericVariable(XYZ123), NumericLiteral(250), NumericLiteral(99))")]
        [Theory]
        public void NumericWithLiteralIndex2D(string input, string output)
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

        private static void Test(string input, string output)
        {
            BasicExpression.FromString(input).ToString().Should().Be(output);
        }
    }
}
