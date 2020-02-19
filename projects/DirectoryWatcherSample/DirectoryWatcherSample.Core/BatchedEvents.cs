// <copyright file="BatchedEvents.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    public sealed class BatchedEvents<T>
    {
        private readonly Func<Task> delay;
        private readonly ConcurrentDictionary<T, TimePoint> batches;

        private Action<T> callback;

        public BatchedEvents(Func<Task> delay)
        {
            this.delay = delay;
            this.batches = new ConcurrentDictionary<T, TimePoint>();
        }

        public void Subscribe(T item, Action<T> callback)
        {
            this.callback = callback;
            this.batches.TryAdd(item, default);
        }

        public void Add(T item, TimePoint timestamp)
        {
            if (this.batches.TryUpdate(item, timestamp, default))
            {
                this.OnBatchCreated(item, timestamp);
            }
        }

        private void OnBatchCreated(T item, TimePoint timestamp)
        {
            this.delay().ContinueWith(t => this.OnBatchCompleted(item, timestamp), TaskContinuationOptions.ExecuteSynchronously);
        }

        private void OnBatchCompleted(T item, TimePoint timestamp)
        {
            if (this.batches.TryUpdate(item, default, timestamp))
            {
                this.callback(item);
            }
        }
    }
}
