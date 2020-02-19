// <copyright file="BatchedEvents.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    public sealed class BatchedEvents<T> : IDisposable
    {
        private readonly Func<Task> delay;
        private readonly ConcurrentDictionary<T, TimePoint> batches;
        private readonly ConcurrentDictionary<T, Action<T>> subscriptions;

        public BatchedEvents(Func<Task> delay)
        {
            this.delay = delay;
            this.batches = new ConcurrentDictionary<T, TimePoint>();
            this.subscriptions = new ConcurrentDictionary<T, Action<T>>();
        }

        public void Dispose()
        {
            this.batches.Clear();
        }

        public IDisposable Subscribe(T item, Action<T> callback)
        {
            this.subscriptions.TryAdd(item, callback);
            this.batches.TryAdd(item, default);
            return new Subscription(() => this.batches.TryRemove(item, out _));
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
                this.subscriptions[item](item);
            }
        }

        private sealed class Subscription : IDisposable
        {
            private readonly Action onDispose;

            public Subscription(Action onDispose)
            {
                this.onDispose = onDispose;
            }

            public void Dispose() => this.onDispose();
        }
    }
}
