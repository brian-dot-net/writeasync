// <copyright file="BasicStreamTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS.Test
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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
            ReadLines()
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void OneLine()
        {
            ReadLines("10 GOTO 10")
                .Should()
                .HaveCount(1).And
                .ContainInOrder("Line(10, Goto(10))");
        }

        [Fact]
        public void TwoLines()
        {
            ReadLines("10 CLS", "20 GOTO 10")
                .Should()
                .HaveCount(2).And
                .ContainInOrder("Line(10, Cls())", "Line(20, Goto(10))");
        }

        [Fact]
        public void ThreeLines()
        {
            ReadLines("10 CLS", "20 PRINT", "30 GOTO 10")
                .Should()
                .HaveCount(3).And
                .ContainInOrder("Line(10, Cls())", "Line(20, Print())", "Line(30, Goto(10))");
        }

        private static IEnumerable<string> ReadLines(params string[] lines)
        {
            WrappedMemoryStream s = Lines(lines);

            Task<BasicLine[]> task = BasicStream.ReadAsync(s);

            task.IsCompletedSuccessfully.Should().BeTrue();
            s.DisposeCount.Should().Be(1);
            return task.Result.Select(l => l.ToString());
        }

        private static WrappedMemoryStream Lines(string[] lines)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string line in lines)
            {
                sb.AppendLine(line);
            }

            return new WrappedMemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
        }

        private sealed class WrappedMemoryStream : MemoryStream
        {
            public WrappedMemoryStream(byte[] buffer)
                : base(buffer)
            {
            }

            public int DisposeCount { get; private set; }

            protected override void Dispose(bool disposing)
            {
                ++this.DisposeCount;
                base.Dispose(disposing);
            }
        }
    }
}
