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
        [Theory]
        public void Invalid(string input)
        {
            Action act = () => BasicLine.FromString(input);

            act.Should().Throw<FormatException>("[{0}]", input).WithMessage("*'" + input + "'*").WithInnerException<Exception>();
        }
    }
}
