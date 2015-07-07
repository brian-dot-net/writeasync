//-----------------------------------------------------------------------
// <copyright file="TimerGroupTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimerSample.Test.Unit
{
    using System;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TimerGroupTest
    {
        [TestMethod]
        public void ShouldScheduleActionOnIntervalAfterAdd()
        {
            TimerGroup timers = new TimerGroup();

            int invokeCount = 0;
            timers.Add(TimeSpan.FromSeconds(1.0d), () => ++invokeCount);

            Assert.AreEqual(0, invokeCount);

            Thread.Sleep(TimeSpan.FromSeconds(1.1d));

            Assert.AreEqual(1, invokeCount);
        }

        [TestMethod]
        public void ShouldScheduleActionPeriodicallyOnIntervalsAfterAdd()
        {
            TimerGroup timers = new TimerGroup();

            int invokeCount = 0;
            timers.Add(TimeSpan.FromSeconds(1.0d), () => ++invokeCount);

            Assert.AreEqual(0, invokeCount);

            Thread.Sleep(TimeSpan.FromSeconds(1.1d));

            Assert.AreEqual(1, invokeCount);

            Thread.Sleep(TimeSpan.FromSeconds(1.0d));

            Assert.AreEqual(2, invokeCount);
        }

        [TestMethod]
        public void ShouldScheduleMultipleActionsPeriodicallyOnSeparateIntervalsAfterAdd()
        {
            TimerGroup timers = new TimerGroup();

            int invokeCount1 = 0;
            int invokeCount2 = 0;
            timers.Add(TimeSpan.FromSeconds(1.0d), () => ++invokeCount1);
            timers.Add(TimeSpan.FromSeconds(1.5d), () => ++invokeCount2);

            Assert.AreEqual(0, invokeCount1);
            Assert.AreEqual(0, invokeCount2);

            Thread.Sleep(TimeSpan.FromSeconds(1.1d));

            Assert.AreEqual(1, invokeCount1);
            Assert.AreEqual(0, invokeCount2);

            Thread.Sleep(TimeSpan.FromSeconds(1.0d));

            Assert.AreEqual(2, invokeCount1);
            Assert.AreEqual(1, invokeCount2);

            Thread.Sleep(TimeSpan.FromSeconds(1.0d));

            Assert.AreEqual(3, invokeCount1);
            Assert.AreEqual(2, invokeCount2);
        }

        [TestMethod]
        public void ShouldCancelActionByIdAfterRemove()
        {
            TimerGroup timers = new TimerGroup();

            int invokeCount1 = 0;
            int invokeCount2 = 0;
            Guid id1 = timers.Add(TimeSpan.FromSeconds(1.0d), () => ++invokeCount1);
            Guid id2 = timers.Add(TimeSpan.FromSeconds(1.5d), () => ++invokeCount2);

            Assert.AreNotEqual(id1, id2);
            Assert.AreEqual(0, invokeCount1);
            Assert.AreEqual(0, invokeCount2);

            Thread.Sleep(TimeSpan.FromSeconds(1.1d));

            Assert.AreEqual(1, invokeCount1);
            Assert.AreEqual(0, invokeCount2);

            Thread.Sleep(TimeSpan.FromSeconds(1.0d));

            Assert.AreEqual(2, invokeCount1);
            Assert.AreEqual(1, invokeCount2);

            timers.Remove(id1);

            Thread.Sleep(TimeSpan.FromSeconds(1.0d));

            Assert.AreEqual(2, invokeCount1);
            Assert.AreEqual(2, invokeCount2);
        }

        [TestMethod]
        public void ShouldCancelAllActionsOnDispose()
        {
            TimerGroup timers = new TimerGroup();

            int invokeCount1 = 0;
            int invokeCount2 = 0;
            Guid id1 = timers.Add(TimeSpan.FromSeconds(1.0d), () => ++invokeCount1);
            Guid id2 = timers.Add(TimeSpan.FromSeconds(1.5d), () => ++invokeCount2);

            Assert.AreNotEqual(id1, id2);
            Assert.AreEqual(0, invokeCount1);
            Assert.AreEqual(0, invokeCount2);

            Thread.Sleep(TimeSpan.FromSeconds(1.1d));

            Assert.AreEqual(1, invokeCount1);
            Assert.AreEqual(0, invokeCount2);

            timers.Dispose();

            Thread.Sleep(TimeSpan.FromSeconds(1.0d));

            Assert.AreEqual(1, invokeCount1);
            Assert.AreEqual(0, invokeCount2);
        }

        [TestMethod]
        public void ShouldIgnoreInvalidIdOnRemove()
        {
            TimerGroup timers = new TimerGroup();

            timers.Remove(Guid.Empty);
        }
    }
}
