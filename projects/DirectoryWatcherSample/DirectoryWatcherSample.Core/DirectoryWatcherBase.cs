﻿// <copyright file="DirectoryWatcherBase.cs" company="Brian Rogers">
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

        public IDisposable Subscribe(string file, Action<FileInfo> onUpdate)
        {
            FileInfo fullPath = new FileInfo(Path.Combine(this.path, file));
            string key = fullPath.FullName;
            Action onDispose = () => this.subscriptions.TryRemove(key, out _);
            Subscription subscription = this.subscriptions.GetOrAdd(key, k => new Subscription(fullPath, onUpdate, onDispose));
            if (subscription.FullPath != fullPath)
            {
                throw new InvalidOperationException($"A subscription for '{key}' already exists.");
            }

            return subscription;
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
