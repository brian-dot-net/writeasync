﻿// <copyright file="DirectoryTreeWatcherBase.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;

    public abstract class DirectoryTreeWatcherBase : IDisposable
    {
        private readonly string path;
        private readonly ConcurrentDictionary<string, DirectoryWatcherBase> watchers;

        protected DirectoryTreeWatcherBase(DirectoryInfo path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            this.path = path.FullName;
            this.watchers = new ConcurrentDictionary<string, DirectoryWatcherBase>();
        }

        public IDisposable Subscribe(string file, Action<FileInfo> onUpdate)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            FileInfo fullPath = new FileInfo(Path.Combine(this.path, file));
            DirectoryInfo dir = fullPath.Directory;
            DirectoryWatcherBase watcher = this.watchers.GetOrAdd(dir.FullName, k => this.Create(dir));
            return watcher.Subscribe(fullPath.Name, onUpdate);
        }

        public void Dispose()
        {
            this.Dispose(true);
            foreach (DirectoryWatcherBase watcher in this.watchers.Values)
            {
                watcher.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        protected abstract DirectoryWatcherBase Create(DirectoryInfo path);

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
