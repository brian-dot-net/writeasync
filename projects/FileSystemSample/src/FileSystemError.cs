// <copyright file="FileSystemError.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace FileSystemSample
{
    using System.IO;

    internal static class FileSystemError
    {
        public static FileSystemException AlreadyOpen(FullPath path, IOException innerException)
        {
            return new FileSystemException("The file '" + path.ToString() + "' is already opened.", innerException);
        }
    }
}
