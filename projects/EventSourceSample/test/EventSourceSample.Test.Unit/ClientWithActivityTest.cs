//-----------------------------------------------------------------------
// <copyright file="ClientWithActivityTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample.Test.Unit
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Eventing;
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
        public void Add_with_activity_traces_start_and_end_with_error()
        {
            VerifyTracesStartAndEndError(c => c.AddAsync(1.0d, 2.0d));
        }

        [Fact]
        public void Add_with_activity_traces_start_and_end_with_sync_error()
        {
            VerifyTracesStartAndEndSyncError(c => c.AddAsync(1.0d, 2.0d));
        }

        [Fact]
        public void Add_with_activity_sets_and_restores_activity_id()
        {
            VerifySetsAndRestoresActivityId(c => c.AddAsync(1.0d, 2.0d));
        }

        [Fact]
        public void Add_with_activity_sets_and_restores_activity_id_on_error()
        {
            VerifySetsAndRestoresActivityIdOnError(c => c.AddAsync(1.0d, 2.0d));
        }

        [Fact]
        public void Add_with_activity_sets_and_restores_activity_id_on_sync_error()
        {
            VerifySetsAndRestoresActivityIdOnSyncError(c => c.AddAsync(1.0d, 2.0d));
        }

        [Fact]
        public void Subtract_with_activity_traces_start_and_end()
        {
            VerifyTracesStartAndEnd(c => c.SubtractAsync(3.0d, 4.0d));
        }

        [Fact]
        public void Subtract_with_activity_traces_start_and_end_with_error()
        {
            VerifyTracesStartAndEndError(c => c.SubtractAsync(3.0d, 4.0d));
        }

        [Fact]
        public void Subtract_with_activity_traces_start_and_end_with_sync_error()
        {
            VerifyTracesStartAndEndSyncError(c => c.SubtractAsync(3.0d, 4.0d));
        }

        [Fact]
        public void Subtract_with_activity_sets_and_restores_activity_id()
        {
            VerifySetsAndRestoresActivityId(c => c.SubtractAsync(3.0d, 4.0d));
        }

        [Fact]
        public void Subtract_with_activity_sets_and_restores_activity_id_on_error()
        {
            VerifySetsAndRestoresActivityIdOnError(c => c.SubtractAsync(3.0d, 4.0d));
        }

        [Fact]
        public void Subtract_with_activity_sets_and_restores_activity_id_on_sync_error()
        {
            VerifySetsAndRestoresActivityIdOnSyncError(c => c.SubtractAsync(3.0d, 4.0d));
        }

        [Fact]
        public void SquareRoot_with_activity_traces_start_and_end()
        {
            VerifyTracesStartAndEnd(c => c.SquareRootAsync(5.0d));
        }

        [Fact]
        public void SquareRoot_with_activity_traces_start_and_end_with_error()
        {
            VerifyTracesStartAndEndError(c => c.SquareRootAsync(5.0d));
        }

        [Fact]
        public void SquareRoot_with_activity_sets_and_restores_activity_id()
        {
            VerifySetsAndRestoresActivityId(c => c.SquareRootAsync(5.0d));
        }

        [Fact]
        public void SquareRoot_with_activity_sets_and_restores_activity_id_on_error()
        {
            VerifySetsAndRestoresActivityIdOnError(c => c.SquareRootAsync(5.0d));
        }

        [Fact]
        public void SquareRoot_with_activity_sets_and_restores_activity_id_on_sync_error()
        {
            VerifySetsAndRestoresActivityIdOnSyncError(c => c.SquareRootAsync(5.0d));
        }

        private static void VerifyTracesStartAndEnd(Func<ICalculatorClientAsync, Task<double>> doAsync)
        {
            ClientEventSource eventSource = ClientEventSource.Instance;
            TaskCompletionSource<double> tcs = new TaskCompletionSource<double>();
            Guid clientId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xC);
            CalculatorClientWithActivity client = new CalculatorClientWithActivity(new CalculatorClientStub(() => tcs.Task), eventSource, clientId);

            using (ClientEventListener listener = new ClientEventListener(eventSource, EventLevel.Informational, ClientEventSource.Keywords.Request))
            {
                Task<double> task = VerifyPending(doAsync(client));

                listener.VerifyEvent(ClientEventId.Request, EventLevel.Informational, ClientEventSource.Keywords.Request, EventOpcode.Start, clientId);
                listener.Events.Clear();

                tcs.SetResult(0.0d);
                VerifyResult(0.0d, task);

                listener.VerifyEvent(ClientEventId.RequestCompleted, EventLevel.Informational, ClientEventSource.Keywords.Request, EventOpcode.Stop, clientId);
            }
        }

        private static void VerifyTracesStartAndEndError(Func<ICalculatorClientAsync, Task<double>> doAsync)
        {
            ClientEventSource eventSource = ClientEventSource.Instance;
            TaskCompletionSource<double> tcs = new TaskCompletionSource<double>();
            Guid clientId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xC);
            CalculatorClientWithActivity client = new CalculatorClientWithActivity(new CalculatorClientStub(() => tcs.Task), eventSource, clientId);

            using (ClientEventListener listener = new ClientEventListener(eventSource, EventLevel.Informational, ClientEventSource.Keywords.Request))
            {
                Task<double> task = VerifyPending(doAsync(client));

                listener.VerifyEvent(ClientEventId.Request, EventLevel.Informational, ClientEventSource.Keywords.Request, EventOpcode.Start, clientId);
                listener.Events.Clear();

                InvalidCastException expectedException = new InvalidCastException("Expected.");
                tcs.SetException(expectedException);
                VerifyResultError(expectedException, task);

                listener.VerifyEvent(ClientEventId.RequestError, EventLevel.Warning, ClientEventSource.Keywords.Request, EventOpcode.Stop, clientId, "System.InvalidCastException", "Expected.");
            }
        }

        private static void VerifyTracesStartAndEndSyncError(Func<ICalculatorClientAsync, Task<double>> doAsync)
        {
            ClientEventSource eventSource = ClientEventSource.Instance;
            Guid clientId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xC);
            InvalidTimeZoneException expectedException = new InvalidTimeZoneException("Expected.");

            using (ClientEventListener listener = new ClientEventListener(eventSource, EventLevel.Informational, ClientEventSource.Keywords.Request))
            {
                Func<Task<double>> verifyStartEventAndThrow = delegate
                {
                    listener.VerifyEvent(ClientEventId.Request, EventLevel.Informational, ClientEventSource.Keywords.Request, EventOpcode.Start, clientId);
                    listener.Events.Clear();
                    throw expectedException;
                };

                CalculatorClientWithActivity client = new CalculatorClientWithActivity(new CalculatorClientStub(verifyStartEventAndThrow), eventSource, clientId);

                InvalidTimeZoneException ite = Assert.Throws<InvalidTimeZoneException>(() => doAsync(client));
                Assert.Same(expectedException, ite);

                listener.VerifyEvent(ClientEventId.RequestError, EventLevel.Warning, ClientEventSource.Keywords.Request, EventOpcode.Stop, clientId, "System.InvalidTimeZoneException", "Expected.");
            }
        }

        private static void VerifySetsAndRestoresActivityId(Func<ICalculatorClientAsync, Task<double>> doAsync)
        {
            ClientEventSource eventSource = ClientEventSource.Instance;
            TaskCompletionSource<double> tcs = new TaskCompletionSource<double>();
            CalculatorClientWithActivity client = new CalculatorClientWithActivity(new CalculatorClientStub(() => tcs.Task), eventSource, Guid.Empty);

            using (ClientEventListener listener = new ClientEventListener(eventSource, EventLevel.Informational, ClientEventSource.Keywords.Request))
            {
                Guid originalActivityId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
                EventProvider.SetActivityId(ref originalActivityId);
                listener.EventWritten += (o, e) => Assert.NotEqual(originalActivityId, Trace.CorrelationManager.ActivityId);

                Task<double> task = VerifyPending(doAsync(client));

                Assert.Equal(originalActivityId, Trace.CorrelationManager.ActivityId);

                tcs.SetResult(0.0d);
                VerifyResult(0.0d, task);

                Assert.Equal(originalActivityId, Trace.CorrelationManager.ActivityId);
            }
        }

        private static void VerifySetsAndRestoresActivityIdOnError(Func<ICalculatorClientAsync, Task<double>> doAsync)
        {
            ClientEventSource eventSource = ClientEventSource.Instance;
            TaskCompletionSource<double> tcs = new TaskCompletionSource<double>();
            CalculatorClientWithActivity client = new CalculatorClientWithActivity(new CalculatorClientStub(() => tcs.Task), eventSource, Guid.Empty);

            using (ClientEventListener listener = new ClientEventListener(eventSource, EventLevel.Informational, ClientEventSource.Keywords.Request))
            {
                Guid originalActivityId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
                EventProvider.SetActivityId(ref originalActivityId);
                listener.EventWritten += (o, e) => Assert.NotEqual(originalActivityId, Trace.CorrelationManager.ActivityId);

                Task<double> task = VerifyPending(doAsync(client));

                Assert.Equal(originalActivityId, Trace.CorrelationManager.ActivityId);

                InvalidTimeZoneException expectedException = new InvalidTimeZoneException("Expected.");
                tcs.SetException(expectedException);
                VerifyResultError(expectedException, task);

                Assert.Equal(originalActivityId, Trace.CorrelationManager.ActivityId);
            }
        }

        private static void VerifySetsAndRestoresActivityIdOnSyncError(Func<ICalculatorClientAsync, Task<double>> doAsync)
        {
            ClientEventSource eventSource = ClientEventSource.Instance;
            InvalidProgramException expectedException = new InvalidProgramException("Expected.");
            CalculatorClientWithActivity client = new CalculatorClientWithActivity(new CalculatorClientStub(() => Throw(expectedException)), eventSource, Guid.Empty);

            using (ClientEventListener listener = new ClientEventListener(eventSource, EventLevel.Informational, ClientEventSource.Keywords.Request))
            {
                Guid originalActivityId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
                EventProvider.SetActivityId(ref originalActivityId);
                listener.EventWritten += (o, e) => Assert.NotEqual(originalActivityId, Trace.CorrelationManager.ActivityId);

                InvalidProgramException ipe = Assert.Throws<InvalidProgramException>(() => doAsync(client));
                Assert.Same(expectedException, ipe);

                Assert.Equal(originalActivityId, Trace.CorrelationManager.ActivityId);
            }
        }

        private static Task<double> Throw(Exception e)
        {
            throw e;
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

        private static void VerifyResultError<TException>(TException expectedException, Task<double> task)
        {
            Assert.Equal(TaskStatus.Faulted, task.Status);
            AggregateException ae = Assert.IsType<AggregateException>(task.Exception).Flatten();
            Assert.Equal(1, ae.InnerExceptions.Count);
            Assert.Same(expectedException, ae.InnerExceptions[0]);
        }
    }
}
