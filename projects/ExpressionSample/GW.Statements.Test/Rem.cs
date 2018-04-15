// <copyright file="Rem.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GW.Statements.Test
{
    using Xunit;

    public sealed class Rem
    {
        [InlineData("REM", "Rem()")]
        [Theory]
        public void Statements(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
