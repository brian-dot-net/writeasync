﻿// <copyright file="RealFileSystem.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace FileSystemSample
{
    using System.IO;
    using System.Linq;

    public sealed class RealFileSystem : IFileSystem
    {
        public IDirectory Create(FullPath name) => new RealDirectory(name);

        private sealed class RealDirectory : IDirectory
        {
            private readonly DirectoryInfo dir;

            public RealDirectory(FullPath path)
            {
                this.Path = path;
                this.dir = Directory.CreateDirectory(this.Path.ToString());
            }

            public FullPath Path { get; private set; }

            public IFile CreateFile(PathPart name) => RealFile.Create(this.Path.Combine(name));

            public IFile[] GetFiles(string pattern) => this.dir.GetFiles(pattern).Select(ToFile).ToArray();

            private static IFile ToFile(FileInfo f) => new RealFile(new FullPath(f.FullName));

            private sealed class RealFile : IFile
            {
                public RealFile(FullPath path)
                {
                    this.Path = path;
                }

                public FullPath Path { get; private set; }

                public static RealFile Create(FullPath path)
                {
                    using (File.Create(path.ToString()))
                    {
                        return new RealFile(path);
                    }
                }
            }
        }
    }
}
