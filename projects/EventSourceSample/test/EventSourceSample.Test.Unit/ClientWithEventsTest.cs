//-----------------------------------------------------------------------
// <copyright file="ClientWithEventsTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample.Test.Unit
{
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class ClientWithEventsTest
    {
        public ClientWithEventsTest()
        {
        }

        [Fact]
        public void Add_with_events_traces_event()
        {
            ClientEventSource eventSource = ClientEventSource.Instance;
            CalculatorClientWithEvents client = new CalculatorClientWithEvents(new CalculatorClientStub(true), eventSource);

            using (ClientEventListener listener = new ClientEventListener(eventSource, EventLevel.Informational, ClientEventSource.Keywords.Basic))
            {
                VerifyResult(0.0d, client.AddAsync(1.0d, 2.0d));
                VerifyEvent(listener, ClientEventId.Add, EventLevel.Informational, ClientEventSource.Keywords.Basic, 1.0d, 2.0d);
            }
        }

        [Fact]
        public void Subtract_with_events_traces_event()
        {
            ClientEventSource eventSource = ClientEventSource.Instance;
            CalculatorClientWithEvents client = new CalculatorClientWithEvents(new CalculatorClientStub(true), eventSource);

            using (ClientEventListener listener = new ClientEventListener(eventSource, EventLevel.Informational, ClientEventSource.Keywords.Basic))
            {
                VerifyResult(0.0d, client.SubtractAsync(3.0d, 4.0d));
                VerifyEvent(listener, ClientEventId.Subtract, EventLevel.Informational, ClientEventSource.Keywords.Basic, 3.0d, 4.0d);
            }
        }

        [Fact]
        public void SquareRoot_with_events_traces_event()
        {
            ClientEventSource eventSource = ClientEventSource.Instance;
            CalculatorClientWithEvents client = new CalculatorClientWithEvents(new CalculatorClientStub(true), eventSource);

            using (ClientEventListener listener = new ClientEventListener(eventSource, EventLevel.Informational, ClientEventSource.Keywords.Advanced))
            {
                VerifyResult(0.0d, client.SquareRootAsync(5.0d));
                VerifyEvent(listener, ClientEventId.SquareRoot, EventLevel.Informational, ClientEventSource.Keywords.Advanced, 5.0d);
            }
        }

        private static void VerifyResult(double expectedResult, Task<double> task)
        {
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(expectedResult, task.Result);            
        }

        private static void VerifyEvent(ClientEventListener listener, ClientEventId id, EventLevel level, EventKeywords keywords, params object[] payloadItems)
        {
            Assert.Equal(1, listener.Events.Count);
            Assert.Equal((int)id, listener.Events[0].EventId);
            Assert.Equal(level, listener.Events[0].Level);
            Assert.True(listener.Events[0].Keywords.HasFlag(keywords));
            Assert.Equal(payloadItems, listener.Events[0].Payload.ToArray());
        }

        private sealed class ClientEventListener : EventListener
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
}
