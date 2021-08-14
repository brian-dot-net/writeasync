// <copyright file="AsyncQueue.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace QueueChallenge
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed class AsyncQueue<T>
    {
        private readonly Queue<T> queue;

        private TaskCompletionSource<T> pending;

        public AsyncQueue()
        {
            this.queue = new Queue<T>();
        }

        public Task<T> DequeueAsync()
        {
            if (this.queue.Count == 0)
            {
                if (this.pending != null)
                {
                    throw new InvalidOperationException("A dequeue operation is already pending.");
                }

                this.pending = new TaskCompletionSource<T>();
                return this.pending.Task;
            }

            return Task.FromResult(this.queue.Dequeue());
        }

        public void EnqueueAsync(T item)
        {
            if (this.pending != null)
            {
                this.pending.SetResult(item);
            }
            else
            {
                this.queue.Enqueue(item);
            }
        }
    }
}
