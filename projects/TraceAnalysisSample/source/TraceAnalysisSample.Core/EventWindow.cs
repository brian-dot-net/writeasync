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
        private readonly Dictionary<int, int> pending;

        public EventWindow()
        {
            this.pending = new Dictionary<int, int>();
        }

        public void Add(int eventId, Guid instanceId)
        {
            if (!this.pending.ContainsKey(eventId))
            {
                this.pending[eventId] = 0;
            }

            ++this.pending[eventId];
        }

        public int GetPendingCount(int eventId)
        {
            int count;
            if (!this.pending.TryGetValue(eventId, out count))
            {
                count = 0;
            }

            return count;
        }
    }
}
