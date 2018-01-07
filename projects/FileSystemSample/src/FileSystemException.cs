// <copyright file="FileSystemException.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace FileSystemSample
{
    using System;

    public sealed class FileSystemException : Exception
    {
        public FileSystemException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
