// <copyright file="UnitTest1.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace RootSample.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            new Class1("x").ToString().Should().Be("[x]");
        }
    }
}
