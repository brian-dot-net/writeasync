// <copyright file="FakeFileSystem.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace FileSystemSample
{
    using System.Collections.Generic;

    public sealed class FakeFileSystem : IFileSystem
    {
        private readonly FakeDirectories fakeDirs;

        public FakeFileSystem()
        {
            this.fakeDirs = new FakeDirectories();
        }

        public IDirectory Create(FullPath name)
        {
            return this.fakeDirs.Create(name);
        }

        private sealed class FakeDirectories
        {
            private readonly Dictionary<FullPath, FakeDirectory> fakeDirs;

            public FakeDirectories()
            {
                this.fakeDirs = new Dictionary<FullPath, FakeDirectory>();
            }

            public IDirectory Create(FullPath name)
            {
                FakeDirectory dir;
                if (!this.fakeDirs.TryGetValue(name, out dir))
                {
                    dir = new FakeDirectory(name);
                    this.fakeDirs.Add(name, dir);
                }

                return dir;
            }

            private sealed class FakeDirectory : IDirectory
            {
                public FakeDirectory(FullPath path)
                {
                    this.Path = path;
                }

                public FullPath Path { get; private set; }
            }
        }
    }
}
