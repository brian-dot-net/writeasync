// <copyright file="RealFileSystem.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace FileSystemSample
{
    using System.IO;

    public sealed class RealFileSystem : IFileSystem
    {
        public IDirectory Create(FullPath name) => new RealDirectory(name);

        private sealed class RealDirectory : IDirectory
        {
            public RealDirectory(FullPath path)
            {
                Directory.CreateDirectory(path.ToString());
                this.Path = path;
            }

            public FullPath Path { get; private set; }
        }
    }
}
