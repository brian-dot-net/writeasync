﻿// <copyright file="Read.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Statements
{
    using Xunit;

    public sealed class Read
    {
        [InlineData("READ R", "Read(NumV(R))")]
        [InlineData("READ R$", "Read(StrV(R))")]
        [InlineData("READ R(1)", "Read(NumA(R, NumL(1)))")]
        [InlineData("READ R$(1,2)", "Read(StrA(R, NumL(1), NumL(2)))")]
        [Theory]
        public void OneItem(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("READ R,R$", "Read(NumV(R), StrV(R))")]
        [InlineData("READ R(1),R$(1,2)", "Read(NumA(R, NumL(1)), StrA(R, NumL(1), NumL(2)))")]
        [Theory]
        public void TwoItems(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("READ R,R$,R(1)", "Read(NumV(R), StrV(R), NumA(R, NumL(1)))")]
        [InlineData("READ R$(1,2),S,T$", "Read(StrA(R, NumL(1), NumL(2)), NumV(S), StrV(T))")]
        [Theory]
        public void ThreeItems(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("read r(1)", "Read(NumA(R, NumL(1)))")]
        [InlineData("ReaD r(1)", "Read(NumA(R, NumL(1)))")]
        [InlineData("rEaD R(1)", "Read(NumA(R, NumL(1)))")]
        [InlineData("rEaD x,y", "Read(NumV(X), NumV(Y))")]
        [InlineData("rEaD x,y,Z", "Read(NumV(X), NumV(Y), NumV(Z))")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData(" READ R(1)", "Read(NumA(R, NumL(1)))")]
        [InlineData("READ  R(1)", "Read(NumA(R, NumL(1)))")]
        [InlineData("  READ  R  (  1  )", "Read(NumA(R, NumL(1)))")]
        [InlineData("READ R$( 1 , 2 )", "Read(StrA(R, NumL(1), NumL(2)))")]
        [InlineData("  READ  X  ,  Y  (  1  )  ", "Read(NumV(X), NumA(Y, NumL(1)))")]
        [InlineData("  READ  X  ,  Y  (  1  ) , Z ", "Read(NumV(X), NumA(Y, NumL(1)), NumV(Z))")]
        [Theory]
        public void AllowSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("READ")]
        [InlineData("READ ")]
        [InlineData("READ 1")]
        [InlineData("READ \"x\"")]
        [InlineData("READR")]
        [InlineData("READ R,")]
        [InlineData("READ R(")]
        [InlineData("READ R(1")]
        [InlineData("READ R(\"1\")")]
        [InlineData("READ R(1,")]
        [InlineData("READ R(1,2")]
        [InlineData("READ R,S,")]
        [InlineData("READ ,")]
        [InlineData("READ R,,S")]
        [InlineData("READ R,S,T,")]
        [Theory]
        public void Invalid(string input)
        {
            Test.Bad(input);
        }
    }
}
