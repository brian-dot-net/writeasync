// <copyright file="FullPath.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace FileSystemSample
{
    using System;
    using System.IO;

    public struct FullPath : IEquatable<FullPath>
    {
        private readonly string path;

        public FullPath(string path)
        {
            this.path = new DirectoryInfo(path).FullName;
        }

        public FullPath Combine(PathPart name) => new FullPath(Path.Combine(this.path, name.ToString()));

        public override int GetHashCode() => this.path.ToUpperInvariant().GetHashCode();

        public override bool Equals(object obj)
        {
            if (!(obj is FullPath))
            {
                return false;
            }

            return this.Equals((FullPath)obj);
        }

        public bool Equals(FullPath other) => string.Equals(this.path, other.path, StringComparison.OrdinalIgnoreCase);

        public override string ToString() => this.path;
    }
}