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
    }
}
