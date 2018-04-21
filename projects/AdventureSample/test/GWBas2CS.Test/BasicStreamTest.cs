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
            result.Result.Should().BeEmpty();
            s.DisposeCount.Should().Be(1);
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
