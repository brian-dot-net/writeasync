// <copyright file="DirectoryWatcherWithLogging.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Logging;

    public sealed class DirectoryWatcherWithLogging : IDirectoryWatcher
    {
        private readonly IDirectoryWatcher inner;
        private readonly string path;
        private readonly ILogger log;

        public DirectoryWatcherWithLogging(IDirectoryWatcher inner, string path, ILogger log)
        {
            this.inner = inner;
            this.path = path;
            this.log = log;
        }

        public void Dispose()
        {
            this.log.LogInformation("Disposing '{0}' watcher", this.path);
            this.inner.Dispose();
        }

        public IDisposable Subscribe(string file, Action<FileInfo> onUpdate)
        {
            string newPath = Path.Combine(this.path, file);
            this.log.LogInformation("Subscribing '{0}' watcher", newPath);
            return new DisposableWithLogging(this.inner.Subscribe(file, onUpdate), newPath, this.log);
        }

        private sealed class DisposableWithLogging : IDisposable
        {
            private readonly IDisposable inner;
            private readonly string path;
            private readonly ILogger log;

            public DisposableWithLogging(IDisposable inner, string path, ILogger log)
            {
                this.inner = inner;
                this.path = path;
                this.log = log;
            }

            public void Dispose()
            {
                this.log.LogInformation("Disposing '{0}' watcher", this.path);
                this.inner.Dispose();
            }
        }
    }
}
