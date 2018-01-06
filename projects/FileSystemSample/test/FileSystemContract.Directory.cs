// <copyright file="FileSystemContract.Directory.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace FileSystemSample.Test
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public abstract partial class FileSystemContract
    {
        [TestMethod]
        public void ShouldCreateDirectoryIfDoesNotExist()
        {
            IFileSystem fs = this.Create();

            IDirectory dir = this.CreateDir(fs, "TotallyNew");

            dir.Path.ToString().Should().Match(@"*\TotallyNew");
        }
    }
}
