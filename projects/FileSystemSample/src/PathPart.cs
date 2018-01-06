// <copyright file="PathPart.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace FileSystemSample
{
    public struct PathPart
    {
        private readonly string name;

        public PathPart(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}