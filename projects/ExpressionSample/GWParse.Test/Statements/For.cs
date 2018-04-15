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

        [InlineData("for i=1 to 10", "For(NumV(I), NumL(1), NumL(10), NumL(1))")]
        [InlineData("FoR i=1 To 10", "For(NumV(I), NumL(1), NumL(10), NumL(1))")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData(" FOR I=1 TO 10", "For(NumV(I), NumL(1), NumL(10), NumL(1))")]
        [InlineData("FOR I=1 TO 10 ", "For(NumV(I), NumL(1), NumL(10), NumL(1))")]
        [InlineData("  FOR  I  =  1  TO  10  ", "For(NumV(I), NumL(1), NumL(10), NumL(1))")]
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
        [Theory]
        public void Invalid(string input)
        {
            Test.Bad(input);
        }
    }
}
