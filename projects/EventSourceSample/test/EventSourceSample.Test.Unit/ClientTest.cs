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

                Assert.Equal(1, listener.Events.Count);
                Assert.Equal((int)ClientEventId.Add, listener.Events[0].EventId);
                Assert.Equal(EventLevel.Informational, listener.Events[0].Level);
                Assert.True(listener.Events[0].Keywords.HasFlag(ClientEventSource.Keywords.Basic));
                Assert.Equal(2, listener.Events[0].Payload.Count);
                Assert.Equal(1.0d, listener.Events[0].Payload[0]);
                Assert.Equal(2.0d, listener.Events[0].Payload[1]);
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
                VerifyResult(0.0d, client.SubtractAsync(1.0d, 2.0d));

                Assert.Equal(1, listener.Events.Count);
                Assert.Equal((int)ClientEventId.Subtract, listener.Events[0].EventId);
                Assert.Equal(EventLevel.Informational, listener.Events[0].Level);
                Assert.True(listener.Events[0].Keywords.HasFlag(ClientEventSource.Keywords.Basic));
                Assert.Equal(2, listener.Events[0].Payload.Count);
                Assert.Equal(1.0d, listener.Events[0].Payload[0]);
                Assert.Equal(2.0d, listener.Events[0].Payload[1]);
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

        private sealed class CalculatorClientStub : ICalculatorClientAsync
        {
            public CalculatorClientStub()
                : this(false)
            {
            }

            public CalculatorClientStub(bool useDefaultResult)
            {
                this.Operations = new List<Tuple<string, double, double>>();
                if (!useDefaultResult)
                {
                    this.Results = new Queue<double>();
                }
            }

            public IList<Tuple<string, double, double>> Operations { get; private set; }

            public Queue<double> Results { get; private set; }

            public Task<double> AddAsync(double x, double y)
            {
                return this.Complete("Add", x, y);
            }

            public Task<double> SubtractAsync(double x, double y)
            {
                return this.Complete("Subtract", x, y);
            }

            public Task<double> SquareRootAsync(double x)
            {
                return this.Complete("SquareRoot", x, 0.0d);
            }

            private Task<double> Complete(string name, double x, double y)
            {
                this.Operations.Add(Tuple.Create(name, x, y));
                return Task.FromResult(this.NextResult());
            }

            private double NextResult()
            {
                double result = 0.0d;
                if (this.Results != null)
                {
                    result = this.Results.Dequeue();
                }

                return result;
            }
        }
    }
}
