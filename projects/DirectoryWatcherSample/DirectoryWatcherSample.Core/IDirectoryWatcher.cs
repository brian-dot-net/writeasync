// <copyright file="IDirectoryWatcher.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample
{
    using System;
    using System.IO;

    public interface IDirectoryWatcher : IDisposable
    {
        IDisposable Subscribe(string file, Action<FileInfo> onUpdate);
    }
}
