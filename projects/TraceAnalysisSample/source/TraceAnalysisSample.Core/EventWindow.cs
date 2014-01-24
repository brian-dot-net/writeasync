//-----------------------------------------------------------------------
// <copyright file="EventWindow.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceAnalysisSample
{
    using System;

    public class EventWindow
    {
        private int pendingCount;

        public EventWindow()
        {
        }

        public void Add(int eventId, Guid instanceId)
        {
            ++this.pendingCount;
        }

        public int GetPendingCount(int eventId)
        {
            return this.pendingCount;
        }
    }
}
