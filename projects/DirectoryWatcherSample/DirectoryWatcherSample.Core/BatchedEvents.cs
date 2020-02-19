// <copyright file="BatchedEvents.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample
{
    using System;
    using System.Threading.Tasks;

    public sealed class BatchedEvents<T>
    {
        private readonly Func<Task> delay;

        private Action<T> callback;

        public BatchedEvents(Func<Task> delay)
        {
            this.delay = delay;
        }

        public void Subscribe(T item, Action<T> callback)
        {
            this.callback = callback;
        }

        public void Add(T item, TimePoint timestamp)
        {
            this.OnBatchCreated(item);
        }

        private void OnBatchCreated(T item)
        {
            this.delay().ContinueWith(t => this.callback(item));
        }
    }
}
