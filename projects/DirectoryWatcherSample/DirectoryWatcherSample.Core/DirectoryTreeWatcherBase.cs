// <copyright file="DirectoryTreeWatcherBase.cs" company="Brian Rogers">
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
        private readonly ConcurrentDictionary<string, Watcher> watchers;

        protected DirectoryTreeWatcherBase(DirectoryInfo path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            this.path = path.FullName + Path.DirectorySeparatorChar;
            this.watchers = new ConcurrentDictionary<string, Watcher>(StringComparer.OrdinalIgnoreCase);
        }

        public IDisposable Subscribe(string file, Action<FileInfo> onUpdate)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (file.Length == 0)
            {
                throw new ArgumentException("File name cannot be empty.", nameof(file));
            }

            FileInfo fullPath = new FileInfo(Path.Combine(this.path, file));
            DirectoryInfo dir = fullPath.Directory;
            string key = dir.FullName + Path.DirectorySeparatorChar;
            if (!key.StartsWith(this.path, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    $"The file '{file}' is not within directory '{this.path}'.",
                    nameof(file));
            }

            Watcher watcher = this.watchers.GetOrAdd(key, k => new Watcher(() => this.Create(dir)));
            return watcher.Subscribe(fullPath.Name, onUpdate);
        }

        public void Dispose()
        {
            this.Dispose(true);
            foreach (Watcher watcher in this.watchers.Values)
            {
                watcher.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        protected abstract DirectoryWatcherBase Create(DirectoryInfo path);

        protected virtual void Dispose(bool disposing)
        {
        }

        private readonly struct Watcher : IDisposable
        {
            private readonly Lazy<DirectoryWatcherBase> inner;

            public Watcher(Func<DirectoryWatcherBase> create)
            {
                this.inner = new Lazy<DirectoryWatcherBase>(create);
            }

            public void Dispose() => this.inner.Value.Dispose();

            public IDisposable Subscribe(string name, Action<FileInfo> onUpdate)
            {
                return this.inner.Value.Subscribe(name, onUpdate);
            }
        }
    }
}
