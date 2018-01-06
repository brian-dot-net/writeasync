// <copyright file="RealFileSystem.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace FileSystemSample
{
    public sealed class RealFileSystem : IFileSystem
    {
        public IDirectory Create(FullPath name) => new RealDirectory(name);

        private sealed class RealDirectory : IDirectory
        {
            public RealDirectory(FullPath path)
            {
                this.Path = path;
            }

            public FullPath Path { get; private set; }

            public IFile CreateFile(PathPart name)
            {
                return new RealFile(this.Path.Combine(name));
            }

            private sealed class RealFile : IFile
            {
                public RealFile(FullPath path)
                {
                    this.Path = path;
                }

                public FullPath Path { get; private set; }
            }
        }
    }
}
