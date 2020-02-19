// <copyright file="DirectoryWatcherWithBatching.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample
{
    using System;
    using System.IO;

    public sealed class DirectoryWatcherWithBatching : IDirectoryWatcher
    {
        private readonly IDirectoryWatcher inner;
        private readonly string path;
        private readonly BatchedEvents<FileInfo> batchedEvents;

        public DirectoryWatcherWithBatching(IDirectoryWatcher inner, string path, BatchedEvents<FileInfo> batchedEvents)
        {
            this.inner = inner;
            this.path = path;
            this.batchedEvents = batchedEvents;
        }

        public void Dispose()
        {
            using (this.inner)
            using (this.batchedEvents)
            {
            }
        }

        public IDisposable Subscribe(string file, Action<FileInfo> onUpdate)
        {
            FileInfo item = new FileInfo(Path.Combine(this.path, file));
            return new CompositeDisposable(
                this.batchedEvents.Subscribe(item, onUpdate),
                this.inner.Subscribe(file, f => this.batchedEvents.Add(item, TimePoint.Now())));
        }

        private sealed class CompositeDisposable : IDisposable
        {
            private readonly IDisposable first;
            private readonly IDisposable second;

            public CompositeDisposable(IDisposable first, IDisposable second)
            {
                this.first = first;
                this.second = second;
            }

            public void Dispose()
            {
                using (this.first)
                using (this.second)
                {
                }
            }
        }
    }
}
