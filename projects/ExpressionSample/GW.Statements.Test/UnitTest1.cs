// <copyright file="UnitTest1.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GW.Statements.Test
{
    using FluentAssertions;
    using Xunit;

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            new Class1("c1").ToString().Should().Be("c1");
        }
    }
}
