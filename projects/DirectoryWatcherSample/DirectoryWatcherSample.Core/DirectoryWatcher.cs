// <copyright file="DirectoryWatcher.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample
{
    using System.IO;

    public sealed class DirectoryWatcher : DirectoryWatcherBase
    {
        private readonly FileSystemWatcher watcher;

        public DirectoryWatcher(DirectoryInfo path)
            : base(path)
        {
            this.watcher = new FileSystemWatcher(path.FullName);
            this.watcher.NotifyFilter =
                NotifyFilters.FileName |
                NotifyFilters.LastWrite |
                NotifyFilters.CreationTime;
            this.watcher.Changed += (o, e) => this.OnUpdated(e.FullPath);
            this.watcher.Created += (o, e) => this.OnUpdated(e.FullPath);
            this.watcher.Renamed += (o, e) => this.OnUpdated(e.FullPath);
            this.watcher.EnableRaisingEvents = true;
        }

        protected override void Dispose(bool disposing) => this.watcher.Dispose();
    }
}
