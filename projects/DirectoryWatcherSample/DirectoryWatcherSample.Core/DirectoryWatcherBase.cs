// <copyright file="DirectoryWatcherBase.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;

    public abstract class DirectoryWatcherBase : IDisposable
    {
        private readonly string path;
        private readonly ConcurrentDictionary<string, Subscription> subscriptions;

        protected DirectoryWatcherBase(DirectoryInfo path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            this.path = path.FullName;
            this.subscriptions = new ConcurrentDictionary<string, Subscription>(StringComparer.OrdinalIgnoreCase);
        }

        public void Subscribe(string file, Action<FileInfo> onUpdate)
        {
            FileInfo fullPath = new FileInfo(Path.Combine(this.path, file));
            this.subscriptions[fullPath.FullName] = new Subscription(fullPath, onUpdate);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        protected void OnUpdated(string fullPath)
        {
            if (this.subscriptions.TryGetValue(fullPath, out Subscription subscription))
            {
                subscription.Invoke();
            }
        }

        private struct Subscription
        {
            private readonly FileInfo fullPath;
            private readonly Action<FileInfo> callback;

            public Subscription(FileInfo fullPath, Action<FileInfo> callback)
            {
                this.fullPath = fullPath;
                this.callback = callback;
            }

            public void Invoke() => this.callback(this.fullPath);
        }
    }
}
