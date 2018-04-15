// <copyright file="Test.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GW.Statements.Test
{
    using FluentAssertions;

    internal static class Test
    {
        public static void Good(string input, string output)
        {
            BasicStatement.FromString(input).ToString().Should().Be(output, "[{0}]", input);
        }
    }
}
