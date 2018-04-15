// <copyright file="For.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Statements
{
    using Xunit;

    public sealed class For
    {
        [InlineData("FOR I=1 TO 10", "For(NumV(I), NumL(1), NumL(10), NumL(1))")]
        [InlineData("FOR A1=B1 TO C1", "For(NumV(A1), NumV(B1), NumV(C1), NumL(1))")]
        [InlineData("FOR A1=B(0) TO C(D)", "For(NumV(A1), NumA(B, NumL(0)), NumA(C, NumV(D)), NumL(1))")]
        [Theory]
        public void Valid(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("FOR I=1 TO 10 STEP 2", "For(NumV(I), NumL(1), NumL(10), NumL(2))")]
        [InlineData("FOR A1=B1 TO C1 STEP D1", "For(NumV(A1), NumV(B1), NumV(C1), NumV(D1))")]
        [InlineData("FOR A1=B(0) TO C(D) STEP E(1)", "For(NumV(A1), NumA(B, NumL(0)), NumA(C, NumV(D)), NumA(E, NumL(1)))")]
        [Theory]
        public void ValidWithStep(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("for i=1 to 10", "For(NumV(I), NumL(1), NumL(10), NumL(1))")]
        [InlineData("FoR i=1 To 10", "For(NumV(I), NumL(1), NumL(10), NumL(1))")]
        [InlineData("FoR i=1 To 10 sTeP 2", "For(NumV(I), NumL(1), NumL(10), NumL(2))")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData(" FOR I=1 TO 10", "For(NumV(I), NumL(1), NumL(10), NumL(1))")]
        [InlineData("FOR I=1 TO 10 ", "For(NumV(I), NumL(1), NumL(10), NumL(1))")]
        [InlineData("  FOR  I  =  1  TO  10  ", "For(NumV(I), NumL(1), NumL(10), NumL(1))")]
        [InlineData("  FOR  I  =  1  TO  10  STEP  3  ", "For(NumV(I), NumL(1), NumL(10), NumL(3))")]
        [Theory]
        public void AllowSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("FOR I")]
        [InlineData("FOR I=")]
        [InlineData("FOR I=1")]
        [InlineData("FOR I=1 TO")]
        [InlineData("FOR I=1TO10")]
        [InlineData("FOR I$=1 TO 10")]
        [InlineData("FOR I(0)=1 TO 10")]
        [InlineData("FOR I=1 TO 10STEP")]
        [InlineData("FOR I=1 TO 10STEP1")]
        [InlineData("FOR I=1 TO 10 STEP1")]
        [Theory]
        public void Invalid(string input)
        {
            Test.Bad(input);
        }
    }
}
