// <copyright file="DirectoryTreeWatcherBase.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample
{
    using System;
    using System.IO;

    public abstract class DirectoryTreeWatcherBase : IDisposable
    {
        private readonly string path;

        protected DirectoryTreeWatcherBase(DirectoryInfo path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            this.path = path.FullName;
        }

        public void Subscribe(string file, Action<FileInfo> onUpdate)
        {
            FileInfo fullPath = new FileInfo(Path.Combine(this.path, file));
            this.Create(fullPath.Directory).Subscribe(fullPath.Name, onUpdate);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract DirectoryWatcherBase Create(DirectoryInfo path);

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
