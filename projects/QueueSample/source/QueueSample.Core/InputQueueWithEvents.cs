//-----------------------------------------------------------------------
// <copyright file="InputQueueWithEvents.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace QueueSample
{
    using System;
    using System.Threading.Tasks;

    public class InputQueueWithEvents<T> : IInputQueue<T>
    {
        private readonly IInputQueue<T> inner;
        private readonly Guid id;
        private readonly QueueEventSource eventSource;

        public InputQueueWithEvents(IInputQueue<T> inner, Guid id, QueueEventSource eventSource)
        {
            this.inner = inner;
            this.id = id;
            this.eventSource = eventSource;
        }

        public async Task<T> DequeueAsync()
        {
            this.eventSource.Dequeue(this.id);
            try
            {
                T item = await this.inner.DequeueAsync();
                return item;
            }
            finally
            {
                this.eventSource.DequeueCompleted(this.id);
            }
        }

        public void Enqueue(T item)
        {
            this.eventSource.Enqueue(this.id);
            try
            {
                this.inner.Enqueue(item);
            }
            finally
            {
                this.eventSource.EnqueueCompleted(this.id);
            }
        }

        public void Dispose()
        {
            this.eventSource.QueueDispose(this.id);
            this.inner.Dispose();
        }
    }
}
