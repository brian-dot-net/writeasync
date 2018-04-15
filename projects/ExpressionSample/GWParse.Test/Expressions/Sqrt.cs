// <copyright file="Sqrt.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Expressions
{
    using Xunit;

    public sealed class Sqrt
    {
        [InlineData("SQR(1)", "Sqrt(NumL(1))")]
        [InlineData("SQR(X)", "Sqrt(NumV(X))")]
        [InlineData("SQR(X(234))", "Sqrt(NumA(X, NumL(234)))")]
        [Theory]
        public void Numeric(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("SQR (1)", "Sqrt(NumL(1))")]
        [InlineData("SQR( X)", "Sqrt(NumV(X))")]
        [InlineData("SQR  (  X(234)  )", "Sqrt(NumA(X, NumL(234)))")]
        [Theory]
        public void IgnoreSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("sqr(1)", "Sqrt(NumL(1))")]
        [InlineData("SqR(X)", "Sqrt(NumV(X))")]
        [InlineData("sQr(X(234))", "Sqrt(NumA(X, NumL(234)))")]
        [Theory]
        public void IgnoreCase(string input, string output)
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
        [InlineData("SQR(SQR(X))", "Sqrt(Sqrt(NumV(X)))")]
        [InlineData("(SQR(X))", "Sqrt(NumV(X))")]
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
        [InlineData("sqr$")]
        [InlineData("1+sqr")]
        [Theory]
        public void FailedReserved(string input)
        {
            Test.Bad(input);
        }

        [InlineData("SQR1", "NumV(SQR1)")]
        [InlineData("sqrX", "NumV(SQRX)")]
        [InlineData("sqr1sqr$", "StrV(SQR1SQR)")]
        [Theory(Skip = "reserved prefix variables not working")]
        public void AllowedReservedPrefix(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
