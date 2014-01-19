//-----------------------------------------------------------------------
// <copyright file="ClientWithActivityTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample.Test.Unit
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Tracing;
    using System.Threading.Tasks;
    using Xunit;

    public class ClientWithActivityTest
    {
        public ClientWithActivityTest()
        {
        }

        [Fact]
        public void Add_with_activity_traces_start_and_end()
        {
            ClientEventSource eventSource = ClientEventSource.Instance;
            TaskCompletionSource<double> tcs = new TaskCompletionSource<double>();
            CalculatorClientWithActivity client = new CalculatorClientWithActivity(new CalculatorClientStub(() => tcs.Task), eventSource);

            using (ClientEventListener listener = new ClientEventListener(eventSource, EventLevel.Informational, ClientEventSource.Keywords.Request))
            {
                Task<double> task = VerifyPending(client.AddAsync(1.0d, 2.0d));

                listener.VerifyEvent(ClientEventId.Request, EventLevel.Informational, ClientEventSource.Keywords.Request, EventOpcode.Start);
                listener.Events.Clear();

                tcs.SetResult(0.0d);
                VerifyResult(0.0d, task);

                listener.VerifyEvent(ClientEventId.RequestCompleted, EventLevel.Informational, ClientEventSource.Keywords.Request, EventOpcode.Stop);
            }
        }

        [Fact]
        public void Add_with_activity_sets_and_restores_activity_id()
        {
            ClientEventSource eventSource = ClientEventSource.Instance;
            TaskCompletionSource<double> tcs = new TaskCompletionSource<double>();
            CalculatorClientWithActivity client = new CalculatorClientWithActivity(new CalculatorClientStub(() => tcs.Task), eventSource);

            using (ClientEventListener listener = new ClientEventListener(eventSource, EventLevel.Informational, ClientEventSource.Keywords.Request))
            {
                Guid originalActivityId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
                Trace.CorrelationManager.ActivityId = originalActivityId;
                listener.EventWritten += (o, e) => Assert.NotEqual(originalActivityId, Trace.CorrelationManager.ActivityId);

                Task<double> task = VerifyPending(client.AddAsync(1.0d, 2.0d));

                Assert.Equal(originalActivityId, Trace.CorrelationManager.ActivityId);

                tcs.SetResult(0.0d);
                VerifyResult(0.0d, task);

                Assert.Equal(originalActivityId, Trace.CorrelationManager.ActivityId);
            }
        }

        [Fact]
        public void Subtract_with_activity_traces_start_and_end()
        {
            ClientEventSource eventSource = ClientEventSource.Instance;
            TaskCompletionSource<double> tcs = new TaskCompletionSource<double>();
            CalculatorClientWithActivity client = new CalculatorClientWithActivity(new CalculatorClientStub(() => tcs.Task), eventSource);

            using (ClientEventListener listener = new ClientEventListener(eventSource, EventLevel.Informational, ClientEventSource.Keywords.Request))
            {
                Task<double> task = VerifyPending(client.SubtractAsync(1.0d, 2.0d));

                listener.VerifyEvent(ClientEventId.Request, EventLevel.Informational, ClientEventSource.Keywords.Request, EventOpcode.Start);
                listener.Events.Clear();

                tcs.SetResult(0.0d);
                VerifyResult(0.0d, task);

                listener.VerifyEvent(ClientEventId.RequestCompleted, EventLevel.Informational, ClientEventSource.Keywords.Request, EventOpcode.Stop);
            }
        }

        private static Task<double> VerifyPending(Task<double> task)
        {
            Assert.False(task.IsCompleted);
            return task;
        }

        private static void VerifyResult(double expectedResult, Task<double> task)
        {
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(expectedResult, task.Result);            
        }
    }
}
