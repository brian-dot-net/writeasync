// <copyright file="PathPart.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace FileSystemSample
{
    using System;
    using System.IO;

    public struct PathPart
    {
        private readonly string name;

        public PathPart(string name)
        {
            this.name = Validate(name);
        }

        public override string ToString()
        {
            return this.name;
        }

        private static string Validate(string name)
        {
            if ((name == null) ||
                (name.Length == 0) ||
                (name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0))
            {
                throw new ArgumentException("The name " + Str(name) + " is invalid.", nameof(name));
            }

            return name;
        }

        private static string Str(string s)
        {
            if (s == null)
            {
                return "<null>";
            }

            if (s.Length == 0)
            {
                return "<empty>";
            }

            return "'" + s + "'";
        }
    }
}