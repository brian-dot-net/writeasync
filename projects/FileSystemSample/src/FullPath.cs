// <copyright file="FullPath.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace FileSystemSample
{
    using System.IO;

    public struct FullPath
    {
        private readonly string path;

        public FullPath(string path)
        {
            this.path = new DirectoryInfo(path).FullName;
        }

        public FullPath Combine(string name)
        {
            return new FullPath(Path.Combine(this.path, name));
        }

        public override string ToString()
        {
            return this.path;
        }
    }
}