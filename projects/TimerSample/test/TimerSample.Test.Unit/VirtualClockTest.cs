//-----------------------------------------------------------------------
// <copyright file="VirtualClockTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimerSample.Test.Unit
{
    using System;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class VirtualClockTest
    {
        [TestMethod]
        public void ShouldInvokeCreatedActionOnSleepAfterEndpointOfInterval()
        {
            VirtualClock clock = new VirtualClock();
            int invokeCount = 0;
            TimeSpan interval = TimeSpan.FromSeconds(1.0d);

            clock.CreateAction(interval, () => ++invokeCount);

            Assert.AreEqual(0, invokeCount);

            clock.Sleep(interval + TimeSpan.FromTicks(interval.Ticks / 4));

            Assert.AreEqual(1, invokeCount);

            clock.Sleep(interval);

            Assert.AreEqual(2, invokeCount);
        }

        [TestMethod]
        public void ShouldNotInvokeCreatedActionOnSleepAtEndpointOfInterval()
        {
            VirtualClock clock = new VirtualClock();
            int invokeCount = 0;
            TimeSpan interval = TimeSpan.FromSeconds(1.0d);

            clock.CreateAction(interval, () => ++invokeCount);

            Assert.AreEqual(0, invokeCount);

            clock.Sleep(interval);

            Assert.AreEqual(0, invokeCount);

            clock.Sleep(interval);

            Assert.AreEqual(1, invokeCount);
        }

        [TestMethod]
        public void ShouldNotInvokeCreatedActionOnSleepAtBeginningOfInterval()
        {
            VirtualClock clock = new VirtualClock();
            int invokeCount = 0;
            TimeSpan interval = TimeSpan.FromSeconds(1.0d);

            clock.CreateAction(interval, () => ++invokeCount);

            Assert.AreEqual(0, invokeCount);

            clock.Sleep(interval);

            Assert.AreEqual(0, invokeCount);
            
            clock.Sleep(TimeSpan.FromTicks(interval.Ticks / 4));

            Assert.AreEqual(1, invokeCount);
        }

        [TestMethod]
        public void ShouldInvokeCreatedActionOnSleepForVerySmallContainedInterval()
        {
            VirtualClock clock = new VirtualClock();
            int invokeCount = 0;
            TimeSpan interval = TimeSpan.FromSeconds(1.0d);

            clock.CreateAction(interval, () => ++invokeCount);

            Assert.AreEqual(0, invokeCount);

            clock.Sleep(interval - TimeSpan.FromTicks(interval.Ticks / 8));

            Assert.AreEqual(0, invokeCount);

            clock.Sleep(TimeSpan.FromTicks(interval.Ticks / 4));

            Assert.AreEqual(1, invokeCount);
        }

        [TestMethod]
        public void ShouldInvokeCreatedActionTwiceOnSleepForLargerContainedInterval()
        {
            VirtualClock clock = new VirtualClock();
            int invokeCount = 0;
            TimeSpan interval = TimeSpan.FromSeconds(1.0d);

            clock.CreateAction(interval, () => ++invokeCount);

            Assert.AreEqual(0, invokeCount);

            clock.Sleep(interval - TimeSpan.FromTicks(interval.Ticks / 8));

            Assert.AreEqual(0, invokeCount);

            clock.Sleep(interval + interval);

            Assert.AreEqual(2, invokeCount);
        }

        [TestMethod]
        public void ShouldNotInvokeCreatedActionIfCurrentTimeMinusCreationTimeIsLessThanInterval()
        {
            VirtualClock clock = new VirtualClock();
            int invokeCount = 0;
            TimeSpan interval = TimeSpan.FromSeconds(1.0d);

            clock.Sleep(interval + interval - TimeSpan.FromTicks(interval.Ticks / 4));
            
            clock.CreateAction(interval, () => ++invokeCount);

            Assert.AreEqual(0, invokeCount);

            clock.Sleep(TimeSpan.FromTicks(interval.Ticks / 2));

            Assert.AreEqual(0, invokeCount);
        }

        [TestMethod]
        public void ShouldNotInvokeCreatedActionIfCurrentTimeMinusCreationTimeIsEqualToInterval()
        {
            VirtualClock clock = new VirtualClock();
            int invokeCount = 0;
            TimeSpan interval = TimeSpan.FromSeconds(1.0d);

            clock.Sleep(interval + interval - TimeSpan.FromTicks(interval.Ticks / 4));

            clock.CreateAction(interval, () => ++invokeCount);

            Assert.AreEqual(0, invokeCount);

            clock.Sleep(interval);

            Assert.AreEqual(0, invokeCount);
        }

        [TestMethod]
        public void ShouldInvokeCreatedActionIfCurrentTimeMinusCreationTimeIsGreaterThanInterval()
        {
            VirtualClock clock = new VirtualClock();
            int invokeCount = 0;
            TimeSpan interval = TimeSpan.FromSeconds(1.0d);

            clock.Sleep(interval + interval - TimeSpan.FromTicks(interval.Ticks / 4));

            clock.CreateAction(interval, () => ++invokeCount);

            Assert.AreEqual(0, invokeCount);

            clock.Sleep(interval + TimeSpan.FromTicks(interval.Ticks / 4));

            Assert.AreEqual(1, invokeCount);
        }
    }
}
