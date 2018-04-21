// <copyright file="BasicStreamTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS.Test
{
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using GWParse.Lines;
    using Xunit;

    public sealed class BasicStreamTest
    {
        [Fact]
        public void Empty()
        {
            WrappedMemoryStream s = Lines();

            Task<BasicLine[]> result = BasicStream.ReadAsync(s);

            result.IsCompleted.Should().BeTrue();
            s.DisposeCount.Should().Be(1);
            result.Result.Should().BeEmpty();
        }

        [Fact]
        public void OneLine()
        {
            WrappedMemoryStream s = Lines("10 GOTO 10");

            Task<BasicLine[]> result = BasicStream.ReadAsync(s);

            result.IsCompleted.Should().BeTrue();
            s.DisposeCount.Should().Be(1);
            result.Result.Should().ContainSingle().Which.ToString().Should().Be("Line(10, Goto(10))");
        }

        private static WrappedMemoryStream Lines(params string[] lines)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string line in lines)
            {
                sb.AppendLine(line);
            }

            return new WrappedMemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
        }
    }
}
