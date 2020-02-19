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
        public void FiveMillisecondsElapsed()
        {
            TimePoint start = TimePoint.Now();

            Thread.Sleep(5);
            TimeSpan elapsed = start.Elapsed();

            elapsed.TotalMilliseconds.Should().BeApproximately(5.0d, 2.0d);
        }

        [TestMethod]
        public void ExplicitCreationAndSubtraction()
        {
            TimePoint start = new TimePoint(1);
            TimePoint end = new TimePoint(999999999);

            TimeSpan elapsed = end - start;

            elapsed.Ticks.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void Equality()
        {
            TimePoint zeroA = default;
            TimePoint zeroB = new TimePoint(0);
            TimePoint oneA = new TimePoint(1);
            TimePoint oneB = new TimePoint(1);

            zeroA.Equals(zeroB).Should().BeTrue();
            oneA.Equals(zeroB).Should().BeFalse();
            oneB.Equals(oneA).Should().BeTrue();
        }
    }
}
