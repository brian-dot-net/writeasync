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
        public void ShouldInvokeCreatedActionOnSleepEndpointOfInterval()
        {
            VirtualClock clock = new VirtualClock();
            int invokeCount = 0;
            TimeSpan interval = TimeSpan.FromSeconds(1.0d);

            clock.CreateAction(interval, () => ++invokeCount);

            Assert.AreEqual(0, invokeCount);

            clock.Sleep(interval);

            Assert.AreEqual(1, invokeCount);

            clock.Sleep(interval);

            Assert.AreEqual(2, invokeCount);
        }
    }
}
