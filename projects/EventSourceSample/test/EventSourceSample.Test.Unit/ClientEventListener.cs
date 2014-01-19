//-----------------------------------------------------------------------
// <copyright file="ClientEventListener.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample.Test.Unit
{
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;

    internal sealed class ClientEventListener : EventListener
    {
        private readonly ClientEventSource eventSource;

        public ClientEventListener(ClientEventSource eventSource, EventLevel level, EventKeywords keywords)
        {
            this.Events = new List<EventWrittenEventArgs>();
            this.eventSource = eventSource;
            this.EnableEvents(this.eventSource, level, keywords);
        }

        public IList<EventWrittenEventArgs> Events { get; private set; }

        public override void Dispose()
        {
            try
            {
                this.DisableEvents(this.eventSource);
            }
            finally
            {
                base.Dispose();
            }
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            this.Events.Add(eventData);
        }
    }
}
