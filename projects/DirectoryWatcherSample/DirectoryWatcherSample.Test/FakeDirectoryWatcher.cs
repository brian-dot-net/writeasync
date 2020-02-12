// <copyright file="FakeDirectoryWatcher.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample.Test
{
    using System.IO;

    internal sealed class FakeDirectoryWatcher : DirectoryWatcherBase
    {
        public FakeDirectoryWatcher(DirectoryInfo path)
            : base(path)
        {
        }

        public bool Disposed { get; private set; }

        public void Update(string fullPath)
        {
            if (!this.Disposed)
            {
                this.OnUpdated(fullPath);
            }
        }

        protected override void Dispose(bool disposing) => this.Disposed = true;
    }
}
