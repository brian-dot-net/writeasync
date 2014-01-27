//-----------------------------------------------------------------------
// <copyright file="InputQueueWithEventsTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace QueueSample.Test.Unit
{
    using System;
    using System.Diagnostics.Tracing;
    using System.Threading.Tasks;
    using Xunit;

    public class InputQueueWithEventsTest
    {
        public InputQueueWithEventsTest()
        {
        }

        [Fact]
        public void Enqueue_with_events_traces_event()
        {
            InputQueueStub<string> inner = new InputQueueStub<string>();
            QueueEventSource eventSource = QueueEventSource.Instance;
            Guid id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            InputQueueWithEvents<string> queue = new InputQueueWithEvents<string>(inner, id, eventSource);

            using (QueueEventListener listener = new QueueEventListener(eventSource, EventLevel.Informational, EventKeywords.None))
            {
                inner.Enqueued += delegate(object sender, EventArgs e)
                {
                    listener.VerifyEvent(QueueEventId.Enqueue, EventLevel.Informational, EventKeywords.None, id);
                    listener.Events.Clear();
                };

                queue.Enqueue("a");

                Assert.Equal("a", inner.Item);
                listener.VerifyEvent(QueueEventId.EnqueueCompleted, EventLevel.Informational, EventKeywords.None, id);
            }
        }

        [Fact]
        public void Dequeue_with_events_traces_event()
        {
            InputQueueStub<string> inner = new InputQueueStub<string>();
            QueueEventSource eventSource = QueueEventSource.Instance;
            Guid id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            InputQueueWithEvents<string> queue = new InputQueueWithEvents<string>(inner, id, eventSource);

            using (QueueEventListener listener = new QueueEventListener(eventSource, EventLevel.Informational, EventKeywords.None))
            {
                Task<string> task = queue.DequeueAsync();

                Assert.False(task.IsCompleted);

                listener.VerifyEvent(QueueEventId.Dequeue, EventLevel.Informational, EventKeywords.None, id);
                listener.Events.Clear();

                inner.PendingDequeue.SetResult("a");

                Assert.Equal(TaskStatus.RanToCompletion, task.Status);
                Assert.Equal("a", task.Result);
                listener.VerifyEvent(QueueEventId.DequeueCompleted, EventLevel.Informational, EventKeywords.None, id);
            }
        }

        [Fact]
        public void Dequeue_with_events_traces_event_on_error()
        {
            InputQueueStub<string> inner = new InputQueueStub<string>();
            QueueEventSource eventSource = QueueEventSource.Instance;
            Guid id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            InputQueueWithEvents<string> queue = new InputQueueWithEvents<string>(inner, id, eventSource);

            using (QueueEventListener listener = new QueueEventListener(eventSource, EventLevel.Informational, EventKeywords.None))
            {
                Task<string> task = queue.DequeueAsync();

                Assert.False(task.IsCompleted);

                listener.VerifyEvent(QueueEventId.Dequeue, EventLevel.Informational, EventKeywords.None, id);
                listener.Events.Clear();

                InvalidTimeZoneException expectedException = new InvalidTimeZoneException("Expected.");
                inner.PendingDequeue.SetException(expectedException);

                Assert.Equal(TaskStatus.Faulted, task.Status);
                Assert.NotNull(task.Exception);
                AggregateException ae = task.Exception;
                Assert.Equal(1, ae.InnerExceptions.Count);
                Assert.Same(expectedException, ae.InnerExceptions[0]);

                listener.VerifyEvent(QueueEventId.DequeueCompleted, EventLevel.Informational, EventKeywords.None, id);
            }
        }

        [Fact]
        public void Dispose_with_events_traces_event()
        {
            InputQueueStub<string> inner = new InputQueueStub<string>();
            QueueEventSource eventSource = QueueEventSource.Instance;
            Guid id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            InputQueueWithEvents<string> queue = new InputQueueWithEvents<string>(inner, id, eventSource);

            using (QueueEventListener listener = new QueueEventListener(eventSource, EventLevel.Informational, EventKeywords.None))
            {
                queue.Dispose();

                Assert.True(inner.Disposed);
                listener.VerifyEvent(QueueEventId.QueueDispose, EventLevel.Informational, EventKeywords.None, id);
            }
        }

        private sealed class InputQueueStub<T> : IInputQueue<T>
        {
            public InputQueueStub()
            {
            }

            public event EventHandler Enqueued;

            public T Item { get; private set; }

            public TaskCompletionSource<T> PendingDequeue { get; private set; }

            public bool Disposed { get; private set; }

            public Task<T> DequeueAsync()
            {
                this.PendingDequeue = new TaskCompletionSource<T>();
                return this.PendingDequeue.Task;
            }

            public void Enqueue(T item)
            {
                this.Item = item;
                this.Enqueued(this, EventArgs.Empty);
            }

            public void Dispose()
            {
                this.Disposed = true;
            }
        }
    }
}
