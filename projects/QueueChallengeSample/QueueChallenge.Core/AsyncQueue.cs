// <copyright file="AsyncQueue.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace QueueChallenge
{
    using System.Threading.Tasks;

    public sealed class AsyncQueue<T>
    {
        private TaskCompletionSource<T> pending;

        public AsyncQueue()
        {
        }

        public Task<T> DequeueAsync() => this.Next().Task;

        public void EnqueueAsync(T item) => this.Next().SetResult(item);

        private TaskCompletionSource<T> Next()
        {
            if (this.pending == null)
            {
                this.pending = new TaskCompletionSource<T>();
            }

            return this.pending;
        }
    }
}
