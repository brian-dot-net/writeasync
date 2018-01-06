// <copyright file="IFile.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace FileSystemSample
{
    using System.IO;

    public interface IFile
    {
        FullPath Path { get; }

        Stream OpenRead();

        Stream OpenWrite();
    }
}
