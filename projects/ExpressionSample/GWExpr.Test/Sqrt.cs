// <copyright file="Sqrt.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Sqrt
    {
        [InlineData("SQR(1)", "Sqrt(NumL(1))")]
        [InlineData("SQR(X)", "Sqrt(NumVar(X))")]
        [InlineData("SQR(X(234))", "Sqrt(Array(NumVar(X), NumL(234)))")]
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

        [InlineData("SQR((1))", "Sqrt(NumL(1))")]
        [InlineData("SQR(SQR(X))", "Sqrt(Sqrt(NumVar(X)))")]
        [InlineData("(SQR(X))", "Sqrt(NumVar(X))")]
        [Theory]
        public void WithParens(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+SQR(-2)", "Add(NumL(1), Sqrt(Neg(NumL(2))))")]
        [InlineData("SQR(1+2)", "Sqrt(Add(NumL(1), NumL(2)))")]
        [InlineData("1-SQR(-2)", "Sub(NumL(1), Sqrt(Neg(NumL(2))))")]
        [InlineData("SQR(1-SQR(2))", "Sqrt(Sub(NumL(1), Sqrt(NumL(2))))")]
        [InlineData("SQR(1*2)", "Sqrt(Mult(NumL(1), NumL(2)))")]
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
