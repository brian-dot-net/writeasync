// <copyright file="PathPart.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace FileSystemSample
{
    using System;
    using System.IO;

    public struct PathPart : IEquatable<PathPart>
    {
        private readonly string name;

        public PathPart(string name)
        {
            this.name = Validate(name);
        }

        public override string ToString() => this.name;

        public override bool Equals(object obj)
        {
            if (!(obj is PathPart))
            {
                return false;
            }

            return this.Equals((PathPart)obj);
        }

        public override int GetHashCode() => this.name.ToUpperInvariant().GetHashCode();

        public bool Equals(PathPart other) => string.Equals(this.name, other.name, StringComparison.OrdinalIgnoreCase);

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