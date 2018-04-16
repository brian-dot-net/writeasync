// <copyright file="Test.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Lines
{
    using System;
    using FluentAssertions;
    using GWParse.Lines;
    using Xunit;

    public sealed class Test
    {
        [InlineData("1 REM", "Line(1, Rem(\"\"))")]
        [InlineData("10 REM", "Line(10, Rem(\"\"))")]
        [InlineData("  10  REM r", "Line(10, Rem(\"r\"))")]
        [InlineData("10 PRINT  ", "Line(10, Print())")]
        [InlineData("55559 REM", "Line(55559, Rem(\"\"))")]
        [InlineData("1 REM : REM", "Line(1, Rem(\": REM\"))")]
        [InlineData("2 PRINT : REM many", "Line(2, Print(), Rem(\"many\"))")]
        [InlineData("3 PRINT : A=1 : B=2", "Line(3, Print(), Assign(NumV(A), NumL(1)), Assign(NumV(B), NumL(2)))")]
        [InlineData("3 DATA 1:PRINT", "Line(3, Data(NumL(1)), Print())")]
        [InlineData("3 DATA x:PRINT", "Line(3, Data(StrL(\"x\")), Print())")]
        [InlineData("3 DATA \"x:PRINT\"", "Line(3, Data(StrL(\"x:PRINT\")))")]
        [Theory]
        public void Valid(string input, string output)
        {
            BasicLine.FromString(input).ToString().Should().Be(output, "[{0}]", input);
        }

        [InlineData("10")]
        [InlineData("10 ")]
        [InlineData(" 10 ")]
        [InlineData("-10 REM")]
        [InlineData("A1 REM")]
        [InlineData("1 :")]
        [InlineData("1 X=1 :")]
        [Theory]
        public void Invalid(string input)
        {
            Action act = () => BasicLine.FromString(input);

            act.Should().Throw<FormatException>("[{0}]", input).WithMessage("*'" + input + "'*").WithInnerException<Exception>();
        }
    }
}
