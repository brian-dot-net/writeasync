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
                queue.Enqueue("a");

                Assert.Equal("a", inner.Item);
                listener.VerifyEvent(QueueEventId.Enqueue, EventLevel.Informational, EventKeywords.None, id);
            }
        }

        private sealed class InputQueueStub<T> : IInputQueue<T>
        {
            public InputQueueStub()
            {
            }

            public T Item { get; set; }

            public Task<T> DequeueAsync()
            {
                throw new NotImplementedException();
            }

            public void Enqueue(T item)
            {
                this.Item = item;
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}
