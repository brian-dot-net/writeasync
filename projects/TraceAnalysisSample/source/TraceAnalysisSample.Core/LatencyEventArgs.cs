//-----------------------------------------------------------------------
// <copyright file="LatencyEventArgs.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceAnalysisSample
{
    using System;

    public class LatencyEventArgs : EventArgs
    {
        public LatencyEventArgs(TimeSpan latency, int eventId, Guid instanceId)
        {
            this.Latency = latency;
            this.EventId = eventId;
            this.InstanceId = instanceId;
        }

        public TimeSpan Latency { get; private set; }

        public int EventId { get; private set; }

        public Guid InstanceId { get; private set; }
    }
}
