// <copyright file="SampleTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS.Test
{
    using FluentAssertions;
    using Xunit;

    public class SampleTest
    {
        [Fact]
        public void ReturnsNameForToString()
        {
            new Sample("my name").ToString().Should().Be("my name");
        }
    }
}
