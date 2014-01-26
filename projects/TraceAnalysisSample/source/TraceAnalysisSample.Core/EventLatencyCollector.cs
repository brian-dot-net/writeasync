//-----------------------------------------------------------------------
// <copyright file="EventLatencyCollector.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceAnalysisSample
{
    using System;

    public class EventLatencyCollector
    {
        private DateTime startTime;

        public EventLatencyCollector()
        {
        }

        public event EventHandler<LatencyEventArgs> EventCompleted;

        public void OnStart(int eventId, Guid instanceId, DateTime startTime)
        {
            this.startTime = startTime;
        }

        public void OnEnd(int eventId, Guid instanceId, DateTime endTime)
        {
            TimeSpan latency = endTime - this.startTime;
            this.EventCompleted(this, new LatencyEventArgs(latency, eventId, instanceId));
        }
    }
}
