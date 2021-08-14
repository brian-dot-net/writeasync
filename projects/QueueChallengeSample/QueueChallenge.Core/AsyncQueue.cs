// <copyright file="AsyncQueue.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace QueueChallenge
{
    using System;
    using System.Threading.Tasks;

    public sealed class AsyncQueue<T>
    {
        private T item;

        public AsyncQueue()
        {
        }

        public Task<T> DequeueAsync()
        {
            if (this.item == null)
            {
                return new TaskCompletionSource<T>().Task;
            }

            return Task.FromResult(this.item);
        }

        public void EnqueueAsync(T item) => this.item = item;
    }
}
