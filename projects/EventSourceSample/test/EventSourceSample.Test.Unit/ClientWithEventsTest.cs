//-----------------------------------------------------------------------
// <copyright file="ClientWithEventsTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample.Test.Unit
{
    using System.Diagnostics.Tracing;
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
            CalculatorClientWithEvents client = new CalculatorClientWithEvents(new CalculatorClientStub(() => Task.FromResult(0.0d)), eventSource);

            using (ClientEventListener listener = new ClientEventListener(eventSource, EventLevel.Informational, ClientEventSource.Keywords.Basic))
            {
                VerifyResult(0.0d, client.AddAsync(1.0d, 2.0d));
                listener.VerifyEvent(ClientEventId.Add, EventLevel.Informational, ClientEventSource.Keywords.Basic, 1.0d, 2.0d);
            }
        }

        [Fact]
        public void Subtract_with_events_traces_event()
        {
            ClientEventSource eventSource = ClientEventSource.Instance;
            CalculatorClientWithEvents client = new CalculatorClientWithEvents(new CalculatorClientStub(() => Task.FromResult(0.0d)), eventSource);

            using (ClientEventListener listener = new ClientEventListener(eventSource, EventLevel.Informational, ClientEventSource.Keywords.Basic))
            {
                VerifyResult(0.0d, client.SubtractAsync(3.0d, 4.0d));
                listener.VerifyEvent(ClientEventId.Subtract, EventLevel.Informational, ClientEventSource.Keywords.Basic, 3.0d, 4.0d);
            }
        }

        [Fact]
        public void SquareRoot_with_events_traces_event()
        {
            ClientEventSource eventSource = ClientEventSource.Instance;
            CalculatorClientWithEvents client = new CalculatorClientWithEvents(new CalculatorClientStub(() => Task.FromResult(0.0d)), eventSource);

            using (ClientEventListener listener = new ClientEventListener(eventSource, EventLevel.Informational, ClientEventSource.Keywords.Advanced))
            {
                VerifyResult(0.0d, client.SquareRootAsync(5.0d));
                listener.VerifyEvent(ClientEventId.SquareRoot, EventLevel.Informational, ClientEventSource.Keywords.Advanced, 5.0d);
            }
        }

        private static void VerifyResult(double expectedResult, Task<double> task)
        {
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(expectedResult, task.Result);            
        }
    }
}
