// <copyright file="UnitTest1.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using FluentAssertions;
    using Xunit;

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            new Class1("ok").ToString().Should().Be("ok");
        }
    }
}
