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

        [TestMethod]
        public void ShouldGetDirectoryIfAlreadyExists()
        {
            IFileSystem fs = this.Create();
            IDirectory dir1 = this.CreateDir(fs, "StillThere");

            IDirectory dir2 = this.CreateDir(fs, "StillThere");

            dir2.Path.Should().Be(dir1.Path);
            dir2.Path.ToString().Should().Match(@"*\StillThere");
        }

        [TestMethod]
        public void ShouldGetDirectoryIfAlreadyExistsIgnoreCase()
        {
            IFileSystem fs = this.Create();
            IDirectory dir1 = this.CreateDir(fs, "StillThere");

            IDirectory dir2 = this.CreateDir(fs, "sTILLtHERE");

            dir2.Path.Should().Be(dir1.Path);
            dir2.Path.ToString().Should().MatchEquivalentOf(@"*\StillThere");
        }

        [TestMethod]
        public void ShouldGetNoFilesFromEmptyDirectory()
        {
            IFileSystem fs = this.Create();
            IDirectory dir1 = this.CreateDir(fs, "NoFiles");

            this.ShouldGetNoFiles(dir1);
        }

        private void ShouldGetNoFiles(IDirectory dir)
        {
            IFile[] files = dir.GetFiles("*");

            files.Should().BeEmpty();
        }
    }
}
