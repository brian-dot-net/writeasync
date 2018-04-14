// <copyright file="Sqrt.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Sqrt
    {
        [InlineData("SQR(1)", "Sqrt(L(1))")]
        [InlineData("SQR(X)", "Sqrt(NumVar(X))")]
        [InlineData("SQR(X(234))", "Sqrt(Array(NumVar(X), L(234)))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("SQR(\"1\")")]
        [InlineData("SQR(X$)")]
        [InlineData("SQR(YZ1234$)")]
        [Theory]
        public void TypeMismatch(string input)
        {
            Test.Bad(input);
        }

        [InlineData("SQR((1))", "Sqrt(L(1))")]
        [InlineData("SQR(SQR(X))", "Sqrt(Sqrt(NumVar(X)))")]
        [InlineData("(SQR(X))", "Sqrt(NumVar(X))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+SQR(-2)", "Add(L(1), Sqrt(Neg(L(2))))")]
        [InlineData("SQR(1+2)", "Sqrt(Add(L(1), L(2)))")]
        [InlineData("1-SQR(-2)", "Sub(L(1), Sqrt(Neg(L(2))))")]
        [InlineData("SQR(1-SQR(2))", "Sqrt(Sub(L(1), Sqrt(L(2))))")]
        [InlineData("SQR(1*2)", "Sqrt(Mult(L(1), L(2)))")]
        [Theory]
        public void WithOtherOperations(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+SQR")]
        [InlineData("SQR(1,X)")]
        [InlineData("SQR$")]
        [Theory]
        public void FailedReserved(string input)
        {
            Test.Bad(input);
        }
    }
}
