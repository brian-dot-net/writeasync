﻿//-----------------------------------------------------------------------
// <copyright file="ClientEventListener.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample.Test.Unit
{
    using System;
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

        public event EventHandler EventWritten;

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
            this.VerifyEvent(id, level, keywords, EventOpcode.Info, payloadItems);
        }
        
        public void VerifyEvent(ClientEventId id, EventLevel level, EventKeywords keywords, EventOpcode opcode, params object[] payloadItems)
        {
            Assert.Equal(1, this.Events.Count);
            Assert.Equal((int)id, this.Events[0].EventId);
            Assert.Equal(level, this.Events[0].Level);
            Assert.Equal(opcode, this.Events[0].Opcode);
            Assert.True(this.Events[0].Keywords.HasFlag(keywords));
            Assert.Equal(payloadItems, this.Events[0].Payload.ToArray());
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            this.Events.Add(eventData);
            EventHandler handler = this.EventWritten;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
