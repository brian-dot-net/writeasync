// <copyright file="TimePointTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample.Test
{
    using System;
    using System.Threading;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class TimePointTest
    {
        [TestMethod]
        public void NearZeroElapsed()
        {
            TimePoint start = TimePoint.Now();

            TimeSpan elapsed = start.Elapsed();

            elapsed.TotalMilliseconds.Should().BeLessThan(1.0d);
        }

        [TestMethod]
        public void ThreeMillisecondsElapsed()
        {
            TimePoint start = TimePoint.Now();

            Thread.Sleep(3);
            TimeSpan elapsed = start.Elapsed();

            elapsed.TotalMilliseconds.Should().BeApproximately(3.0d, 1.0d);
        }
    }
}
