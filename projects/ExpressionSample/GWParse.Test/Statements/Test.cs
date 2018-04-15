// <copyright file="Test.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Test.Statements
{
    using System;
    using FluentAssertions;
    using GWParse.Statements;

    internal static class Test
    {
        public static void Good(string input, string output)
        {
            BasicStatement.FromString(input).ToString().Should().Be(output, "[{0}]", input);
        }

        public static void Bad(string input)
        {
            Action act = () => BasicStatement.FromString(input);

            act.Should().Throw<FormatException>("[{0}]", input).WithMessage("*'" + input + "'*").WithInnerException<Exception>();
        }
    }
}
