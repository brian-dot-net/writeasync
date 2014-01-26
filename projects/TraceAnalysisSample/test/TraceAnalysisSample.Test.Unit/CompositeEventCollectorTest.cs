//-----------------------------------------------------------------------
// <copyright file="CompositeEventCollectorTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceAnalysisSample.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class CompositeEventCollectorTest
    {
        public CompositeEventCollectorTest()
        {
        }

        [Fact]
        public void OnStart_passes_through_to_inner_collectors()
        {
            EventCollectorStub stub1 = new EventCollectorStub();
            EventCollectorStub stub2 = new EventCollectorStub();
            CompositeEventCollector collector = new CompositeEventCollector(stub1, stub2);
            int eventId = 12;
            Guid instanceId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            DateTime startTime = new DateTime(2000, 1, 2, 3, 4, 5, 6);

            collector.OnStart(eventId, instanceId, startTime);

            Assert.Equal(eventId, stub1.StartEventId);
            Assert.Equal(instanceId, stub1.StartInstanceId);
            Assert.Equal(startTime, stub1.StartTime);
            Assert.Equal(eventId, stub2.StartEventId);
            Assert.Equal(instanceId, stub2.StartInstanceId);
            Assert.Equal(startTime, stub2.StartTime);
        }

        private sealed class EventCollectorStub : IEventCollector
        {
            public EventCollectorStub()
            {
            }

            public int StartEventId { get; private set; }
            
            public Guid StartInstanceId { get; private set; }

            public DateTime StartTime { get; private set; }

            public void OnStart(int eventId, Guid instanceId, DateTime startTime)
            {
                this.StartEventId = eventId;
                this.StartInstanceId = instanceId;
                this.StartTime = startTime;
            }

            public void OnEnd(int eventId, Guid instanceId, DateTime endTime)
            {
                throw new NotImplementedException();
            }
        }
    }
}
