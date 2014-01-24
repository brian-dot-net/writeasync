//-----------------------------------------------------------------------
// <copyright file="EventWindow.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceAnalysisSample
{
    using System;
    using System.Collections.Generic;

    public class EventWindow
    {
        private readonly Dictionary<int, HashSet<Guid>> pending;

        public EventWindow()
        {
            this.pending = new Dictionary<int, HashSet<Guid>>();
        }

        public void Add(int eventId, Guid instanceId)
        {
            if (!this.pending.ContainsKey(eventId))
            {
                this.pending[eventId] = new HashSet<Guid>();
            }

            bool added = this.pending[eventId].Add(instanceId);
            if (!added)
            {
                throw new InvalidOperationException("An event with instance ID " + instanceId + " is already present.");
            }
        }

        public int GetPendingCount(int eventId)
        {
            int count = 0;
            HashSet<Guid> events;
            if (this.pending.TryGetValue(eventId, out events))
            {
                count = events.Count;
            }

            return count;
        }
    }
}
