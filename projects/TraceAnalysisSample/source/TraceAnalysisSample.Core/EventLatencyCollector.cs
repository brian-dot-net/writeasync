//-----------------------------------------------------------------------
// <copyright file="EventLatencyCollector.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceAnalysisSample
{
    using System;
using System.Collections.Generic;

    public class EventLatencyCollector
    {
        private readonly Dictionary<EventKey, DateTime> events;

        public EventLatencyCollector()
        {
            this.events = new Dictionary<EventKey, DateTime>();
        }

        public event EventHandler<LatencyEventArgs> EventCompleted;

        public void OnStart(int eventId, Guid instanceId, DateTime startTime)
        {
            this.events.Add(new EventKey(eventId, instanceId), startTime);
        }

        public void OnEnd(int eventId, Guid instanceId, DateTime endTime)
        {
            DateTime startTime;
            EventKey key = new EventKey(eventId, instanceId);
            if (this.events.TryGetValue(key, out startTime))
            {
                this.events.Remove(key);
                TimeSpan latency = endTime - startTime;
                EventHandler<LatencyEventArgs> handler = this.EventCompleted;
                if (handler != null)
                {
                    handler(this, new LatencyEventArgs(latency, eventId, instanceId));
                }
            }
        }

        private struct EventKey : IEquatable<EventKey>
        {
            private readonly int eventId;
            private readonly Guid instanceId;

            public EventKey(int eventId, Guid instanceId)
            {
                this.eventId = eventId;
                this.instanceId = instanceId;
            }

            public bool Equals(EventKey other)
            {
                return (this.eventId == other.eventId) && (this.instanceId == other.instanceId);
            }

            public override int GetHashCode()
            {
                return this.eventId.GetHashCode() ^ this.instanceId.GetHashCode();
            }
        }
    }
}
