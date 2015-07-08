//-----------------------------------------------------------------------
// <copyright file="VirtualClockTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimerSample.Test.Unit
{
    using System;
    using System.Collections.Generic;
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

        [TestMethod]
        public void ShouldInvokeMultipleActionsInOrderOfAscendingDeadlines()
        {
            VirtualClock clock = new VirtualClock();
            TimeSpan interval1 = TimeSpan.FromSeconds(2.0d);
            TimeSpan interval2 = TimeSpan.FromSeconds(4.0d);
            List<string> invocations = new List<string>();

            clock.Sleep(TimeSpan.FromTicks(interval1.Ticks / 2));

            clock.CreateAction(interval1, () => invocations.Add("A"));

            clock.Sleep(TimeSpan.FromTicks(interval1.Ticks / 2));

            clock.CreateAction(interval2, () => invocations.Add("B"));

            CollectionAssert.AreEqual(new string[0], invocations.ToArray());

            clock.Sleep(interval1 + interval2);

            CollectionAssert.AreEqual(new string[] { "A", "A", "B", "A" }, invocations.ToArray());
        }

        [TestMethod]
        public void ShouldNotInvokeRemovedAction()
        {
            VirtualClock clock = new VirtualClock();
            TimeSpan interval1 = TimeSpan.FromSeconds(2.0d);
            TimeSpan interval2 = TimeSpan.FromSeconds(4.0d);
            List<string> invocations = new List<string>();

            clock.Sleep(TimeSpan.FromTicks(interval1.Ticks / 2));

            PeriodicAction action1 = clock.CreateAction(interval1, () => invocations.Add("A"));

            clock.Sleep(TimeSpan.FromTicks(interval1.Ticks / 2));

            PeriodicAction action2 = clock.CreateAction(interval2, () => invocations.Add("B"));

            CollectionAssert.AreEqual(new string[0], invocations.ToArray());

            action1.Dispose();

            clock.Sleep(interval1 + interval2);

            CollectionAssert.AreEqual(new string[] { "B" }, invocations.ToArray());
        }
    }
}
