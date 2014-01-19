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
            VerifyTracesStartAndEnd(c => c.AddAsync(1.0d, 2.0d));
        }

        [Fact]
        public void Add_with_activity_sets_and_restores_activity_id()
        {
            VerifySetsAndRestoresActivityId(c => c.AddAsync(1.0d, 2.0d));
        }

        [Fact]
        public void Subtract_with_activity_traces_start_and_end()
        {
            VerifyTracesStartAndEnd(c => c.SubtractAsync(3.0d, 4.0d));
        }

        [Fact]
        public void Subtract_with_activity_sets_and_restores_activity_id()
        {
            VerifySetsAndRestoresActivityId(c => c.SubtractAsync(3.0d, 4.0d));
        }

        private static void VerifyTracesStartAndEnd(Func<ICalculatorClientAsync, Task<double>> doAsync)
        {
            ClientEventSource eventSource = ClientEventSource.Instance;
            TaskCompletionSource<double> tcs = new TaskCompletionSource<double>();
            CalculatorClientWithActivity client = new CalculatorClientWithActivity(new CalculatorClientStub(() => tcs.Task), eventSource);

            using (ClientEventListener listener = new ClientEventListener(eventSource, EventLevel.Informational, ClientEventSource.Keywords.Request))
            {
                Task<double> task = VerifyPending(doAsync(client));

                listener.VerifyEvent(ClientEventId.Request, EventLevel.Informational, ClientEventSource.Keywords.Request, EventOpcode.Start);
                listener.Events.Clear();

                tcs.SetResult(0.0d);
                VerifyResult(0.0d, task);

                listener.VerifyEvent(ClientEventId.RequestCompleted, EventLevel.Informational, ClientEventSource.Keywords.Request, EventOpcode.Stop);
            }
        }

        private static void VerifySetsAndRestoresActivityId(Func<ICalculatorClientAsync, Task<double>> doAsync)
        {
            ClientEventSource eventSource = ClientEventSource.Instance;
            TaskCompletionSource<double> tcs = new TaskCompletionSource<double>();
            CalculatorClientWithActivity client = new CalculatorClientWithActivity(new CalculatorClientStub(() => tcs.Task), eventSource);

            using (ClientEventListener listener = new ClientEventListener(eventSource, EventLevel.Informational, ClientEventSource.Keywords.Request))
            {
                Guid originalActivityId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
                Trace.CorrelationManager.ActivityId = originalActivityId;
                listener.EventWritten += (o, e) => Assert.NotEqual(originalActivityId, Trace.CorrelationManager.ActivityId);

                Task<double> task = VerifyPending(doAsync(client));

                Assert.Equal(originalActivityId, Trace.CorrelationManager.ActivityId);

                tcs.SetResult(0.0d);
                VerifyResult(0.0d, task);

                Assert.Equal(originalActivityId, Trace.CorrelationManager.ActivityId);
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
