// <copyright file="AsyncQueue.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace QueueChallenge
{
    using System;
    using System.Threading.Tasks;

    public sealed class AsyncQueue<T>
    {
        public AsyncQueue()
        {
        }

        public Task<T> DequeueAsync() => new TaskCompletionSource<T>().Task;
    }
}
