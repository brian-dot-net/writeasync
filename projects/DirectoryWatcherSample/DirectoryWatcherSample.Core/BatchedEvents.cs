// <copyright file="BatchedEvents.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample
{
    using System;
    using System.Threading.Tasks;

    public sealed class BatchedEvents<T>
    {
        public BatchedEvents(Func<Task> delay)
        {
        }

        public void Add(T item, TimePoint timestamp)
        {
        }
    }
}
