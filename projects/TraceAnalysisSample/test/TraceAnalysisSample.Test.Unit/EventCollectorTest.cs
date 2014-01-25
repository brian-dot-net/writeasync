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
        public void One_start_within_window_produces_one_after_close()
        {
            EventCollector collector = new EventCollector();
            int eventId = 1;
            Guid instanceId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            DateTime startTime = new DateTime(2000, 1, 2, 3, 4, 5, 6);
            List<EventWindow> windows = new List<EventWindow>();

            collector.WindowClosed += (o, e) => windows.Add(e.Window);

            collector.OnStart(eventId, instanceId, startTime);

            Assert.Equal(0, windows.Count);

            collector.CloseWindow();

            Assert.Equal(1, windows.Count);
            Assert.Equal(startTime, windows[0].StartTime);
            Assert.Equal(1, windows[0].GetPendingCount(eventId));
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

        [Fact]
        public void One_start_and_one_end_outside_window_closes_one_and_produces_one_after_close()
        {
            EventCollector collector = new EventCollector();
            int eventId = 1;
            Guid instanceId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            DateTime startTime = new DateTime(2000, 1, 2, 3, 4, 5, 6);
            DateTime endTime = new DateTime(2000, 1, 2, 3, 4, 6, 6);
            List<EventWindow> windows = new List<EventWindow>();

            collector.WindowClosed += (o, e) => windows.Add(e.Window);

            collector.OnStart(eventId, instanceId, startTime);
            collector.OnEnd(eventId, instanceId, endTime);

            Assert.Equal(1, windows.Count);
            Assert.Equal(startTime, windows[0].StartTime);
            Assert.Equal(1, windows[0].GetPendingCount(eventId));
            Assert.Equal(0, windows[0].GetCompletedCount(eventId));

            collector.CloseWindow();

            Assert.Equal(2, windows.Count);
            Assert.Equal(endTime, windows[1].StartTime);
            Assert.Equal(0, windows[1].GetPendingCount(eventId));
            Assert.Equal(1, windows[1].GetCompletedCount(eventId));
        }

        [Fact]
        public void One_start_and_one_end_two_intervals_outside_window_closes_one_and_produces_one_after_close()
        {
            EventCollector collector = new EventCollector();
            int eventId = 1;
            Guid instanceId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            DateTime startTime = new DateTime(2000, 1, 2, 3, 4, 5, 6);
            DateTime endTime = new DateTime(2000, 1, 2, 3, 4, 7, 8);
            List<EventWindow> windows = new List<EventWindow>();

            collector.WindowClosed += (o, e) => windows.Add(e.Window);

            collector.OnStart(eventId, instanceId, startTime);
            collector.OnEnd(eventId, instanceId, endTime);

            Assert.Equal(1, windows.Count);
            Assert.Equal(startTime, windows[0].StartTime);
            Assert.Equal(1, windows[0].GetPendingCount(eventId));
            Assert.Equal(0, windows[0].GetCompletedCount(eventId));

            collector.CloseWindow();

            Assert.Equal(2, windows.Count);
            Assert.Equal(new DateTime(2000, 1, 2, 3, 4, 7, 6), windows[1].StartTime);
            Assert.Equal(0, windows[1].GetPendingCount(eventId));
            Assert.Equal(1, windows[1].GetCompletedCount(eventId));
        }

        [Fact]
        public void Close_window_when_no_window_does_nothing()
        {
            EventCollector collector = new EventCollector();
            int count = 0;
            collector.WindowClosed += (o, e) => ++count;

            collector.CloseWindow();

            Assert.Equal(0, count);
        }
    }
}
