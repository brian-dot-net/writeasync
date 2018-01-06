// <copyright file="IFileSystem.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace FileSystemSample
{
    public interface IFileSystem
    {
        IDirectory Create(FullPath name);
    }
}
