// <copyright file="Next.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Statements
{
    using Xunit;

    public sealed class Next
    {
        [InlineData("NEXT", "Next()")]
        [Theory]
        public void Empty(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("NEXT I", "Next(NumV(I))")]
        [InlineData("NEXT AB123", "Next(NumV(AB123))")]
        [Theory]
        public void OneVar(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("NEXT I,J", "Next(NumV(I), NumV(J))")]
        [InlineData("NEXT AB123,X", "Next(NumV(AB123), NumV(X))")]
        [Theory]
        public void TwoVars(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("NEXT I,J,K", "Next(NumV(I), NumV(J), NumV(K))")]
        [InlineData("NEXT A1,A2,A3", "Next(NumV(A1), NumV(A2), NumV(A3))")]
        [Theory]
        public void ThreeVars(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("next", "Next()")]
        [InlineData("NeXT", "Next()")]
        [InlineData("next i", "Next(NumV(I))")]
        [InlineData("next i,j", "Next(NumV(I), NumV(J))")]
        [InlineData("next i,j,k", "Next(NumV(I), NumV(J), NumV(K))")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData(" NEXT", "Next()")]
        [InlineData("NEXT ", "Next()")]
        [InlineData(" NEXT  ", "Next()")]
        [InlineData(" NEXT  I  ", "Next(NumV(I))")]
        [InlineData(" NEXT  I  ,  J  ", "Next(NumV(I), NumV(J))")]
        [InlineData(" NEXT  I  ,  J,K  ", "Next(NumV(I), NumV(J), NumV(K))")]
        [Theory]
        public void IgnoreSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("NEXTA")]
        [InlineData("NEXT1")]
        [InlineData("NEXT A$")]
        [InlineData("NEXT A(1)")]
        [InlineData("NEXT 1")]
        [InlineData("NEXT \"x\"")]
        [InlineData("NEXT I,")]
        [InlineData("NEXT I,J$")]
        [InlineData("NEXT I,J,")]
        [Theory]
        public void Invalid(string input)
        {
            Test.Bad(input);
        }
    }
}
