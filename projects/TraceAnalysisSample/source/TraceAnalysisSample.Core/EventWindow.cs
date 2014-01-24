//-----------------------------------------------------------------------
// <copyright file="EventWindow.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceAnalysisSample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class EventWindow
    {
        private readonly Dictionary<int, HashSet<Guid>> pending;
        private readonly Dictionary<int, int> completed;

        public EventWindow(DateTime startTime)
        {
            this.pending = new Dictionary<int, HashSet<Guid>>();
            this.completed = new Dictionary<int, int>();
            this.StartTime = startTime;
        }

        public EventWindow(EventWindow other)
        {
            this.pending = new Dictionary<int, HashSet<Guid>>(other.pending);
            this.completed = new Dictionary<int, int>(other.completed);
            this.StartTime = other.StartTime;
        }

        public DateTime StartTime { get; private set; }

        public IEnumerable<int> KnownEventIds
        {
            get { return this.pending.Keys.Concat(this.completed.Keys).Distinct(); }
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

        public void Complete(int eventId, Guid instanceId)
        {
            bool wasPending = false;
            HashSet<Guid> events;
            if (this.pending.TryGetValue(eventId, out events))
            {
                wasPending = events.Remove(instanceId);
            }

            if (wasPending)
            {
                if (!this.completed.ContainsKey(eventId))
                {
                    this.completed[eventId] = 0;
                }

                ++this.completed[eventId];
            }
        }

        public void ClearCompleted()
        {
            this.completed.Clear();
        }

        public int GetCompletedCount(int eventId)
        {
            int count;
            if (!this.completed.TryGetValue(eventId, out count))
            {
                count = 0;
            }

            return count;
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
