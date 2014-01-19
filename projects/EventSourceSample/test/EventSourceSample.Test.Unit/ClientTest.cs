//-----------------------------------------------------------------------
// <copyright file="ClientTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class ClientTest
    {
        public ClientTest()
        {
        }

        [Fact]
        public void Add_with_events_calls_inner()
        {
            CalculatorClientStub clientStub = new CalculatorClientStub();
            clientStub.Results.Enqueue(1.0d);
            CalculatorClientWithEvents client = new CalculatorClientWithEvents(clientStub, ClientEventSource.Instance);

            VerifyResult(1.0d, client.AddAsync(2.0d, 3.0d));
            VerifyOperation("Add", 2.0d, 3.0d, clientStub);
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
        public void Subtract_with_events_calls_inner()
        {
            CalculatorClientStub clientStub = new CalculatorClientStub();
            clientStub.Results.Enqueue(4.0d);
            CalculatorClientWithEvents client = new CalculatorClientWithEvents(clientStub, ClientEventSource.Instance);

            VerifyResult(4.0d, client.SubtractAsync(5.0d, 6.0d));
            VerifyOperation("Subtract", 5.0d, 6.0d, clientStub);
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
        public void SquareRoot_with_events_calls_inner()
        {
            CalculatorClientStub clientStub = new CalculatorClientStub();
            clientStub.Results.Enqueue(8.0d);
            CalculatorClientWithEvents client = new CalculatorClientWithEvents(clientStub, ClientEventSource.Instance);

            VerifyResult(8.0d, client.SquareRootAsync(7.0d));
            VerifyOperation("SquareRoot", 7.0d, 0.0d, clientStub);
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

        private static void VerifyOperation(string expectedName, double expectedX, double expectedY, CalculatorClientStub clientStub)
        {
            Assert.Equal(1, clientStub.Operations.Count);
            Assert.Equal(expectedName, clientStub.Operations[0].Item1);
            Assert.Equal(expectedX, clientStub.Operations[0].Item2);
            Assert.Equal(expectedY, clientStub.Operations[0].Item3);
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
