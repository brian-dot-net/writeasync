// <copyright file="Class1Test.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace TaskSample.Test
{
    using FluentAssertions;
    using Xunit;

    public class Class1Test
    {
        [Fact]
        public void Test1()
        {
            Class1 c = new Class1("hi");

            c.ToString().Should().Be("hi");
        }
    }
}
