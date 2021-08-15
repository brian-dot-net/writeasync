﻿// <copyright file="UberQueue.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace QueueChallenge
{
    using System.Threading.Tasks;

    public sealed class UberQueue<T>
    {
        private readonly AsyncQueue<T>[] queues;

        public UberQueue(AsyncQueue<T>[] queues)
        {
            this.queues = queues;
        }

        public Task<T> DequeueAsync() => new TaskCompletionSource<T>().Task;
    }
}
