// <copyright file="DirectoryWatcherBase.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample
{
    using System;
    using System.IO;

    public abstract class DirectoryWatcherBase : IDisposable
    {
        private readonly string path;

        private string file;
        private Action<FileInfo> onUpdate;

        protected DirectoryWatcherBase(DirectoryInfo path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            this.path = path.FullName;
        }

        public void Subscribe(string file, Action<FileInfo> onUpdate)
        {
            this.file = Path.Combine(this.path, file);
            this.onUpdate = onUpdate;
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
            if (string.Equals(this.file, fullPath, StringComparison.OrdinalIgnoreCase))
            {
                this.onUpdate(new FileInfo(this.file));
            }
        }
    }
}
