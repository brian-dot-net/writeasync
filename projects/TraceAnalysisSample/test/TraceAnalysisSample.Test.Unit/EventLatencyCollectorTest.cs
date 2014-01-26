//-----------------------------------------------------------------------
// <copyright file="EventLatencyCollectorTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceAnalysisSample.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class EventLatencyCollectorTest
    {
        public EventLatencyCollectorTest()
        {
        }

        [Fact]
        public void Start_then_end_raises_event_completed_with_calculated_latency()
        {
            EventLatencyCollector collector = new EventLatencyCollector();
            int eventId = 1;
            Guid instanceId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            DateTime startTime = new DateTime(2000, 1, 2, 3, 4, 5, 6);
            DateTime endTime = new DateTime(2000, 1, 2, 3, 4, 5, 7);
            List<TimeSpan> latencies = new List<TimeSpan>();
            List<int> eventIds = new List<int>();
            List<Guid> instanceIds = new List<Guid>();

            collector.EventCompleted += delegate(object sender, LatencyEventArgs e)
            {
                latencies.Add(e.Latency);
                eventIds.Add(e.EventId);
                instanceIds.Add(e.InstanceId);
            };

            collector.OnStart(eventId, instanceId, startTime);

            Assert.Equal(0, latencies.Count);

            collector.OnEnd(eventId, instanceId, endTime);

            Assert.Equal(1, latencies.Count);
            Assert.Equal(TimeSpan.FromMilliseconds(1.0d), latencies[0]);
            Assert.Equal(eventId, eventIds[0]);
            Assert.Equal(instanceId, instanceIds[0]);
        }

        [Fact]
        public void End_with_no_start_does_nothing()
        {
            EventLatencyCollector collector = new EventLatencyCollector();

            int eventId = 1;
            Guid instanceId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            DateTime endTime = new DateTime(2000, 1, 2, 3, 4, 5, 7);

            int count = 0;
            collector.EventCompleted += (o, e) => ++count;

            collector.OnEnd(eventId, instanceId, endTime);

            Assert.Equal(0, count);
        }

        [Fact]
        public void Start_then_end_with_unmatched_id_does_nothing()
        {
            EventLatencyCollector collector = new EventLatencyCollector();

            int eventIdA = 1;
            int eventIdB = 2;
            Guid instanceId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            DateTime startTime = new DateTime(2000, 1, 2, 3, 4, 5, 6);
            DateTime endTime = new DateTime(2000, 1, 2, 3, 4, 5, 7);

            int count = 0;
            collector.EventCompleted += (o, e) => ++count;

            collector.OnStart(eventIdA, instanceId, endTime);
            collector.OnEnd(eventIdB, instanceId, endTime);

            Assert.Equal(0, count);
        }

        [Fact]
        public void Start_then_end_with_unmatched_instance_id_does_nothing()
        {
            EventLatencyCollector collector = new EventLatencyCollector();

            int eventId = 1;
            Guid instanceIdA = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            Guid instanceIdB = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2);
            DateTime startTime = new DateTime(2000, 1, 2, 3, 4, 5, 6);
            DateTime endTime = new DateTime(2000, 1, 2, 3, 4, 5, 7);

            int count = 0;
            collector.EventCompleted += (o, e) => ++count;

            collector.OnStart(eventId, instanceIdA, startTime);
            collector.OnEnd(eventId, instanceIdB, endTime);

            Assert.Equal(0, count);
        }

        [Fact]
        public void Start_then_end_with_no_subscriber_does_nothing()
        {
            EventLatencyCollector collector = new EventLatencyCollector();

            int eventId = 1;
            Guid instanceId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            DateTime startTime = new DateTime(2000, 1, 2, 3, 4, 5, 6);
            DateTime endTime = new DateTime(2000, 1, 2, 3, 4, 5, 7);

            collector.OnStart(eventId, instanceId, startTime);
            collector.OnEnd(eventId, instanceId, endTime);
        }

        [Fact]
        public void Start_then_end_removes_entry()
        {
            EventLatencyCollector collector = new EventLatencyCollector();
            int eventId = 1;
            Guid instanceId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            DateTime startTime = new DateTime(2000, 1, 2, 3, 4, 5, 6);
            DateTime endTime = new DateTime(2000, 1, 2, 3, 4, 5, 7);
            List<TimeSpan> latencies = new List<TimeSpan>();
            List<int> eventIds = new List<int>();
            List<Guid> instanceIds = new List<Guid>();

            collector.EventCompleted += delegate(object sender, LatencyEventArgs e)
            {
                latencies.Add(e.Latency);
                eventIds.Add(e.EventId);
                instanceIds.Add(e.InstanceId);
            };

            collector.OnStart(eventId, instanceId, startTime);
            collector.OnEnd(eventId, instanceId, endTime);

            Assert.Equal(1, latencies.Count);
            Assert.Equal(TimeSpan.FromMilliseconds(1.0d), latencies[0]);
            Assert.Equal(eventId, eventIds[0]);
            Assert.Equal(instanceId, instanceIds[0]);

            collector.OnStart(eventId, instanceId, startTime);
            collector.OnEnd(eventId, instanceId, endTime);

            Assert.Equal(2, latencies.Count);
            Assert.Equal(TimeSpan.FromMilliseconds(1.0d), latencies[1]);
            Assert.Equal(eventId, eventIds[1]);
            Assert.Equal(instanceId, instanceIds[1]);
        }

        [Fact]
        public void Start_multiple_then_end_multiple_raises_event_completed_multiple()
        {
            EventLatencyCollector collector = new EventLatencyCollector();

            int eventIdA = 1;
            int eventIdB = 2;
            Guid instanceIdA = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            Guid instanceIdB = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2);
            DateTime startTimeA = new DateTime(2000, 1, 2, 3, 4, 5, 6);
            DateTime endTimeA = new DateTime(2000, 1, 2, 3, 4, 5, 7);
            DateTime startTimeB = new DateTime(2000, 1, 2, 3, 4, 6, 5);
            DateTime endTimeB = new DateTime(2000, 1, 2, 3, 4, 6, 7);
            List<TimeSpan> latencies = new List<TimeSpan>();
            List<int> eventIds = new List<int>();
            List<Guid> instanceIds = new List<Guid>();

            collector.EventCompleted += delegate(object sender, LatencyEventArgs e)
            {
                latencies.Add(e.Latency);
                eventIds.Add(e.EventId);
                instanceIds.Add(e.InstanceId);
            };

            collector.OnStart(eventIdA, instanceIdA, startTimeA);
            collector.OnStart(eventIdB, instanceIdB, startTimeB);

            collector.OnEnd(eventIdA, instanceIdA, endTimeA);

            Assert.Equal(1, latencies.Count);
            Assert.Equal(TimeSpan.FromMilliseconds(1.0d), latencies[0]);
            Assert.Equal(eventIdA, eventIds[0]);
            Assert.Equal(instanceIdA, instanceIds[0]);

            collector.OnEnd(eventIdB, instanceIdB, endTimeB);

            Assert.Equal(2, latencies.Count);
            Assert.Equal(TimeSpan.FromMilliseconds(2.0d), latencies[1]);
            Assert.Equal(eventIdB, eventIds[1]);
            Assert.Equal(instanceIdB, instanceIds[1]);
        }
    }
}
