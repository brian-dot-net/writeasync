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
                private readonly FakeFiles fakeFiles;

                public FakeDirectory(FullPath path)
                {
                    this.Path = path;
                    this.fakeFiles = new FakeFiles(this.Path);
                }

                public FullPath Path { get; private set; }

                public IFile CreateFile(PathPart name) => this.fakeFiles.Create(name);

                private sealed class FakeFiles
                {
                    private readonly Dictionary<PathPart, FakeFile> fakeFiles;
                    private readonly FullPath root;

                    public FakeFiles(FullPath root)
                    {
                        this.fakeFiles = new Dictionary<PathPart, FakeFile>();
                        this.root = root;
                    }

                    public IFile Create(PathPart name)
                    {
                        FakeFile file;
                        if (!this.fakeFiles.TryGetValue(name, out file))
                        {
                            file = new FakeFile(this.root.Combine(name));
                            this.fakeFiles.Add(name, file);
                        }

                        return file;
                    }

                    private sealed class FakeFile : IFile
                    {
                        public FakeFile(FullPath path)
                        {
                            this.Path = path;
                        }

                        public FullPath Path { get; private set; }
                    }
                }
            }
        }
    }
}
