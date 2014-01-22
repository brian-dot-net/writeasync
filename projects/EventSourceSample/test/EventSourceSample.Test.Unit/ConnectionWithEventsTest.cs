//-----------------------------------------------------------------------
// <copyright file="ConnectionWithEventsTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample.Test.Unit
{
    using System;
    using System.Diagnostics.Tracing;
    using System.Threading.Tasks;
    using Xunit;

    public class ConnectionWithEventsTest
    {
        public ConnectionWithEventsTest()
        {
        }

        private interface IMyProxy
        {
            void Stub();
        }

        [Fact]
        public void Open_with_events_calls_inner()
        {
            ConnectionStub<IMyProxy> inner = new ConnectionStub<IMyProxy>();
            ConnectionWithEvents<IMyProxy> outer = new ConnectionWithEvents<IMyProxy>(inner, ClientEventSource.Instance, Guid.Empty);

            Task task = outer.OpenAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(1, inner.OpenCount);
        }

        [Fact]
        public void Abort_with_events_calls_inner()
        {
            ConnectionStub<IMyProxy> inner = new ConnectionStub<IMyProxy>();
            ConnectionWithEvents<IMyProxy> outer = new ConnectionWithEvents<IMyProxy>(inner, ClientEventSource.Instance, Guid.Empty);

            outer.Abort();

            Assert.Equal(1, inner.AbortCount);
        }

        [Fact]
        public void Get_instance_with_events_calls_inner()
        {
            ConnectionStub<IMyProxy> inner = new ConnectionStub<IMyProxy>();
            MyProxyStub proxyStub = new MyProxyStub();
            inner.Instance = proxyStub;
            ConnectionWithEvents<IMyProxy> outer = new ConnectionWithEvents<IMyProxy>(inner, ClientEventSource.Instance, Guid.Empty);

            Assert.Same(proxyStub, outer.Instance);
        }

        [Fact]
        public void Open_with_events_traces_start_and_end()
        {
            ConnectionStub<IMyProxy> inner = new ConnectionStub<IMyProxy>(true);
            ClientEventSource eventSource = ClientEventSource.Instance;
            Guid id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4);
            ConnectionWithEvents<IMyProxy> outer = new ConnectionWithEvents<IMyProxy>(inner, eventSource, id);

            using (ClientEventListener listener = new ClientEventListener(eventSource, EventLevel.Informational, ClientEventSource.Keywords.Connection))
            {
                Task task = outer.OpenAsync();

                Assert.False(task.IsCompleted);
                listener.VerifyEvent(ClientEventId.ConnectionOpening, EventLevel.Informational, ClientEventSource.Keywords.Connection, EventOpcode.Start, id);
                listener.Events.Clear();

                inner.OpenCall.SetResult(false);
                Assert.Equal(TaskStatus.RanToCompletion, task.Status);
                listener.VerifyEvent(ClientEventId.ConnectionOpened, EventLevel.Informational, ClientEventSource.Keywords.Connection, EventOpcode.Stop, id);
                listener.Events.Clear();
            }
        }

        [Fact]
        public void Open_with_events_traces_start_and_end_error()
        {
            ConnectionStub<IMyProxy> inner = new ConnectionStub<IMyProxy>(true);
            ClientEventSource eventSource = ClientEventSource.Instance;
            Guid id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5);
            ConnectionWithEvents<IMyProxy> outer = new ConnectionWithEvents<IMyProxy>(inner, eventSource, id);

            using (ClientEventListener listener = new ClientEventListener(eventSource, EventLevel.Informational, ClientEventSource.Keywords.Connection))
            {
                Task task = outer.OpenAsync();

                Assert.False(task.IsCompleted);
                listener.VerifyEvent(ClientEventId.ConnectionOpening, EventLevel.Informational, ClientEventSource.Keywords.Connection, EventOpcode.Start, id);
                listener.Events.Clear();

                InvalidTimeZoneException expectedException = new InvalidTimeZoneException("Expected.");
                inner.OpenCall.SetException(expectedException);
                Assert.True(task.IsFaulted);
                AggregateException ae = Assert.IsType<AggregateException>(task.Exception);
                Assert.Equal(1, ae.InnerExceptions.Count);
                Assert.Same(expectedException, ae.InnerException);

                listener.VerifyEvent(ClientEventId.ConnectionError, EventLevel.Informational, ClientEventSource.Keywords.Connection, EventOpcode.Stop, id, "System.InvalidTimeZoneException", "Expected.");
                listener.Events.Clear();
            }
        }

        [Fact]
        public void Abort_with_events_traces_event()
        {
            ConnectionStub<IMyProxy> inner = new ConnectionStub<IMyProxy>();
            ClientEventSource eventSource = ClientEventSource.Instance;
            Guid id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6);
            ConnectionWithEvents<IMyProxy> outer = new ConnectionWithEvents<IMyProxy>(inner, eventSource, id);

            using (ClientEventListener listener = new ClientEventListener(eventSource, EventLevel.Informational, ClientEventSource.Keywords.Connection))
            {
                outer.Abort();
                listener.VerifyEvent(ClientEventId.ConnectionAborting, EventLevel.Informational, ClientEventSource.Keywords.Connection, id);
            }
        }

        private sealed class MyProxyStub : IMyProxy
        {
            public MyProxyStub()
            {
            }

            public void Stub()
            {
                throw new NotImplementedException();
            }
        }
    }
}
