// <copyright file="GoldenMaster.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using FluentAssertions;
    using Xunit;

    public sealed class GoldenMaster
    {
        [Fact]
        public void Test()
        {
            string[] inputLines = File.ReadAllLines("adventure.in");
            string expectedOutput = File.ReadAllText("adventure.out");
            StringBuilder outputText = new StringBuilder();
            CannedInput input = new CannedInput(inputLines);

            Game.Run(input, outputText);

            input.IsEmpty.Should().BeTrue();
            outputText.ToString().Should().Be(expectedOutput);
        }

        private sealed class CannedInput : TextReader
        {
            private readonly Queue<string> input;

            public CannedInput(IEnumerable<string> input)
            {
                this.input = new Queue<string>(input);
            }

            public bool IsEmpty => this.input.Count == 0;

            public override string ReadLine() => this.input.Dequeue();
        }
    }
}
