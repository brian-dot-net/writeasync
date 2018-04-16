// <copyright file="IfThen.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Statements
{
    using Xunit;

    public sealed class IfThen
    {
        [InlineData("IF 1 THEN 2", "If(NumL(1), Goto(2))")]
        [InlineData("IF X THEN 34", "If(NumV(X), Goto(34))")]
        [InlineData("IF X=1 THEN 56789", "If(Eq(NumV(X), NumL(1)), Goto(56789))")]
        [InlineData("IF A$>\"x\" AND B=1 THEN 56789", "If(And(Gt(StrV(A), StrL(\"x\")), Eq(NumV(B), NumL(1))), Goto(56789))")]
        [InlineData("IF C>LEN(CM$) THEN 150", "If(Gt(NumV(C), Len(StrV(CM))), Goto(150))")]
        [Theory]
        public void ValidGoto(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("IF 1 THEN X$=\"ok\"", "If(NumL(1), Assign(StrV(X), StrL(\"ok\")))")]
        [InlineData("IF X THEN GOTO 1", "If(NumV(X), Goto(1))")]
        [InlineData("IF X=1 THEN PRINT \"ok\"", "If(Eq(NumV(X), NumL(1)), Print(StrL(\"ok\")))")]
        [InlineData("IF (OB(I) AND 127)=R THEN PRINT \" \";OB$(I)", "If(Eq(And(NumA(OB, NumV(I)), NumL(127)), NumV(R)), Print(StrL(\" \"), StrA(OB, NumV(I))))")]
        [Theory]
        public void ValidNonGoto(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("if 1 then 2", "If(NumL(1), Goto(2))")]
        [InlineData("If 1 TheN 2", "If(NumL(1), Goto(2))")]
        [InlineData("If 1 TheN pRINT \"ok\"", "If(NumL(1), Print(StrL(\"ok\")))")]
        [Theory]
        public void IgnoreCase(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData(" IF 1 THEN 2", "If(NumL(1), Goto(2))")]
        [InlineData("IF 1 THEN 2 ", "If(NumL(1), Goto(2))")]
        [InlineData("  IF  1 THEN  2  ", "If(NumL(1), Goto(2))")]
        [InlineData("  IF  1 THEN  PRINT  \"ok\"  ", "If(NumL(1), Print(StrL(\"ok\")))")]
        [Theory]
        public void AllowSpaces(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("IFA")]
        [InlineData("IF1")]
        [InlineData("IF 1")]
        [InlineData("IF 1 THEN")]
        [InlineData("IF THEN")]
        [InlineData("IF 1THEN")]
        [InlineData("IF 1THEN2")]
        [InlineData("IF 1 THEN2")]
        [InlineData("IF 1 THENPRINT")]
        [Theory]
        public void Invalid(string input)
        {
            Test.Bad(input);
        }
    }
}
