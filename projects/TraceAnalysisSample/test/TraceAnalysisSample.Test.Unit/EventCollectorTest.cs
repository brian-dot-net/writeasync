//-----------------------------------------------------------------------
// <copyright file="EventCollectorTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceAnalysisSample.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class EventCollectorTest
    {
        public EventCollectorTest()
        {
        }

        [Fact]
        public void One_start_end_pair_within_window_produces_one_after_close()
        {
            EventCollector collector = new EventCollector();
            int eventId = 1;
            Guid instanceId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            DateTime startTime = new DateTime(2000, 1, 2, 3, 4, 5, 6);
            DateTime endTime = new DateTime(2000, 1, 2, 3, 4, 5, 7);
            List<EventWindow> windows = new List<EventWindow>();

            collector.WindowClosed += (o, e) => windows.Add(e.Window);

            collector.OnStart(eventId, instanceId, startTime);
            collector.OnEnd(eventId, instanceId, endTime);

            Assert.Equal(0, windows.Count);

            collector.CloseWindow();

            Assert.Equal(1, windows.Count);
            Assert.Equal(startTime, windows[0].StartTime);
            Assert.Equal(1, windows[0].GetCompletedCount(eventId));
        }
    }
}
