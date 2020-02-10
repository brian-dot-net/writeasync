// <copyright file="DirectoryWatcher.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;

    public sealed class DirectoryWatcher : IDisposable
    {
        private readonly string path;
        private readonly ConcurrentDictionary<string, Subscription> subscriptions;
        private readonly FileSystemWatcher watcher;

        public DirectoryWatcher(DirectoryInfo path)
        {
            this.path = path.FullName;
            this.subscriptions = new ConcurrentDictionary<string, Subscription>(StringComparer.OrdinalIgnoreCase);
            this.watcher = new FileSystemWatcher(this.path);
            this.watcher.NotifyFilter =
                NotifyFilters.FileName |
                NotifyFilters.LastWrite |
                NotifyFilters.CreationTime;
            this.watcher.Changed += (o, e) => this.OnUpdated(e.FullPath);
            this.watcher.Created += (o, e) => this.OnUpdated(e.FullPath);
            this.watcher.Renamed += (o, e) => this.OnUpdated(e.FullPath);
            this.watcher.EnableRaisingEvents = true;
        }

        public void Dispose() => this.watcher.Dispose();

        public IDisposable Subscribe(string file, Action<FileInfo> onUpdate)
        {
            FileInfo fullPath = new FileInfo(Path.Combine(this.path, file));
            if (!string.Equals(fullPath.DirectoryName, this.path, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "The file '{file}' is not directly within directory '{path}'.",
                    nameof(file));
            }

            string key = fullPath.FullName;
            Subscription subscription = this.subscriptions.GetOrAdd(
                key,
                k => new Subscription(fullPath, onUpdate, () => this.subscriptions.TryRemove(k, out _)));
            if (!object.ReferenceEquals(subscription.FullPath, fullPath))
            {
                throw new InvalidOperationException($"A subscription for '{key}' already exists.");
            }

            return subscription;
        }

        private void OnUpdated(string fullPath)
        {
            if (this.subscriptions.TryGetValue(fullPath, out Subscription subscription))
            {
                subscription.Invoke();
            }
        }

        private sealed class Subscription : IDisposable
        {
            private readonly Action<FileInfo> callback;
            private readonly Action onDispose;

            public Subscription(FileInfo fullPath, Action<FileInfo> callback, Action onDispose)
            {
                this.FullPath = fullPath;
                this.callback = callback;
                this.onDispose = onDispose;
            }

            public FileInfo FullPath { get; }

            public void Invoke() => this.callback(this.FullPath);

            public void Dispose() => this.onDispose();
        }
    }
}
