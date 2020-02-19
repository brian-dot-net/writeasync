// <copyright file="DirectoryTreeWatcher.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample
{
    using System.IO;

    public sealed class DirectoryTreeWatcher : DirectoryTreeWatcherBase
    {
        public DirectoryTreeWatcher(DirectoryInfo path)
            : base(path)
        {
        }

        protected override IDirectoryWatcher Create(DirectoryInfo path) => new DirectoryWatcher(path);
    }
}
