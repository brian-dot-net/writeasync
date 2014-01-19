//-----------------------------------------------------------------------
// <copyright file="ClientEventListener.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample.Test.Unit
{
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using Xunit;

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

        public void VerifyEvent(ClientEventId id, EventLevel level, EventKeywords keywords, params object[] payloadItems)
        {
            Assert.Equal(1, this.Events.Count);
            Assert.Equal((int)id, this.Events[0].EventId);
            Assert.Equal(level, this.Events[0].Level);
            Assert.True(this.Events[0].Keywords.HasFlag(keywords));
            Assert.Equal(payloadItems, this.Events[0].Payload.ToArray());
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            this.Events.Add(eventData);
        }
    }
}
