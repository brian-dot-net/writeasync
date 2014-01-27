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

        public Task<T> DequeueAsync()
        {
            this.eventSource.Dequeue(this.id);
            return this.inner.DequeueAsync();
        }

        public void Enqueue(T item)
        {
            this.eventSource.Enqueue(this.id);
            this.inner.Enqueue(item);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
