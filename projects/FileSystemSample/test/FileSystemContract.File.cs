// <copyright file="FileSystemContract.File.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace FileSystemSample.Test
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public abstract partial class FileSystemContract
    {
        [TestMethod]
        public void ShouldCreateFileIfDoesNotExist()
        {
            IFileSystem fs = this.Create();

            IFile file = this.CreateFile(fs, "Parent", "new.txt");

            file.Path.ToString().Should().Match(@"*\Parent\new.txt");
        }
    }
}
