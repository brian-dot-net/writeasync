// <copyright file="Rem.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GW.Statements.Test
{
    using Xunit;

    public sealed class Rem
    {
        [InlineData("REM", "Rem(\"\")")]
        [InlineData("REM hello", "Rem(\"hello\")")]
        [InlineData("REM REM starts here", "Rem(\"REM starts here\")")]
        [Theory]
        public void Valid(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
