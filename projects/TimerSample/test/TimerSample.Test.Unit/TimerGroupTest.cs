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
            VirtualClock clock = new VirtualClock();
            using (TimerGroup timers = new TimerGroup() { Create = clock.CreateAction })
            {
                int invokeCount = 0;
                timers.Add(TimeSpan.FromSeconds(1.0d), () => ++invokeCount);

                Assert.AreEqual(0, invokeCount);

                clock.Sleep(TimeSpan.FromSeconds(1.1d));

                Assert.AreEqual(1, invokeCount);
            }
        }

        [TestMethod]
        public void ShouldScheduleActionPeriodicallyOnIntervalsAfterAdd()
        {
            VirtualClock clock = new VirtualClock();
            using (TimerGroup timers = new TimerGroup() { Create = clock.CreateAction })
            {
                int invokeCount = 0;
                timers.Add(TimeSpan.FromSeconds(1.0d), () => ++invokeCount);

                Assert.AreEqual(0, invokeCount);

                clock.Sleep(TimeSpan.FromSeconds(1.1d));

                Assert.AreEqual(1, invokeCount);

                clock.Sleep(TimeSpan.FromSeconds(1.0d));

                Assert.AreEqual(2, invokeCount);
            }
        }

        [TestMethod]
        public void ShouldScheduleMultipleActionsPeriodicallyOnSeparateIntervalsAfterAdd()
        {
            VirtualClock clock = new VirtualClock();
            using (TimerGroup timers = new TimerGroup() { Create = clock.CreateAction })
            {
                int invokeCount1 = 0;
                int invokeCount2 = 0;
                timers.Add(TimeSpan.FromSeconds(1.0d), () => ++invokeCount1);
                timers.Add(TimeSpan.FromSeconds(1.5d), () => ++invokeCount2);

                Assert.AreEqual(0, invokeCount1);
                Assert.AreEqual(0, invokeCount2);

                clock.Sleep(TimeSpan.FromSeconds(1.1d));

                Assert.AreEqual(1, invokeCount1);
                Assert.AreEqual(0, invokeCount2);

                clock.Sleep(TimeSpan.FromSeconds(1.0d));

                Assert.AreEqual(2, invokeCount1);
                Assert.AreEqual(1, invokeCount2);

                clock.Sleep(TimeSpan.FromSeconds(1.0d));

                Assert.AreEqual(3, invokeCount1);
                Assert.AreEqual(2, invokeCount2);
            }
        }

        [TestMethod]
        public void ShouldCancelActionByIdAfterRemove()
        {
            VirtualClock clock = new VirtualClock();
            using (TimerGroup timers = new TimerGroup() { Create = clock.CreateAction })
            {
                int invokeCount1 = 0;
                int invokeCount2 = 0;
                Guid id1 = timers.Add(TimeSpan.FromSeconds(1.0d), () => ++invokeCount1);
                Guid id2 = timers.Add(TimeSpan.FromSeconds(1.5d), () => ++invokeCount2);

                Assert.AreNotEqual(id1, id2);
                Assert.AreEqual(0, invokeCount1);
                Assert.AreEqual(0, invokeCount2);

                clock.Sleep(TimeSpan.FromSeconds(1.1d));

                Assert.AreEqual(1, invokeCount1);
                Assert.AreEqual(0, invokeCount2);

                clock.Sleep(TimeSpan.FromSeconds(1.0d));

                Assert.AreEqual(2, invokeCount1);
                Assert.AreEqual(1, invokeCount2);

                timers.Remove(id1);

                clock.Sleep(TimeSpan.FromSeconds(1.0d));

                Assert.AreEqual(2, invokeCount1);
                Assert.AreEqual(2, invokeCount2);
            }
        }

        [TestMethod]
        public void ShouldCancelAllActionsOnDispose()
        {
            VirtualClock clock = new VirtualClock();
            TimerGroup timers = new TimerGroup() { Create = clock.CreateAction };

            int invokeCount1 = 0;
            int invokeCount2 = 0;
            Guid id1 = timers.Add(TimeSpan.FromSeconds(1.0d), () => ++invokeCount1);
            Guid id2 = timers.Add(TimeSpan.FromSeconds(1.5d), () => ++invokeCount2);

            Assert.AreNotEqual(id1, id2);
            Assert.AreEqual(0, invokeCount1);
            Assert.AreEqual(0, invokeCount2);

            clock.Sleep(TimeSpan.FromSeconds(1.1d));

            Assert.AreEqual(1, invokeCount1);
            Assert.AreEqual(0, invokeCount2);

            timers.Dispose();

            clock.Sleep(TimeSpan.FromSeconds(1.0d));

            Assert.AreEqual(1, invokeCount1);
            Assert.AreEqual(0, invokeCount2);
        }

        [TestMethod]
        public void ShouldIgnoreInvalidIdOnRemove()
        {
            using (TimerGroup timers = new TimerGroup())
            {
                timers.Remove(Guid.Empty);
            }
        }

        [TestMethod]
        public void ShouldIgnoreMultipleRemove()
        {
            using (TimerGroup timers = new TimerGroup())
            {
                Guid id = timers.Add(TimeSpan.FromHours(1.0d), () => { });

                timers.Remove(id);
                timers.Remove(id);
            }
        }
    }
}
