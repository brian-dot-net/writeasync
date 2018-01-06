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

        [TestMethod]
        public void ShouldGetFileIfAlreadyExists()
        {
            IFileSystem fs = this.Create();
            IFile file1 = this.CreateFile(fs, "Parent", "NotNew.txt");

            IFile file2 = this.CreateFile(fs, "Parent", "NotNew.txt");

            file2.Path.Should().Be(file1.Path);
            file2.Path.ToString().Should().Match(@"*Parent\NotNew.txt");
        }

        [TestMethod]
        public void ShouldGetFileIfAlreadyExistsIgnoreCase()
        {
            IFileSystem fs = this.Create();
            IFile file1 = this.CreateFile(fs, "Parent", "NotNew.txt");

            IFile file2 = this.CreateFile(fs, "Parent", "nOTnEW.TXT");

            file2.Path.Should().Be(file1.Path);
            file2.Path.ToString().Should().MatchEquivalentOf(@"*Parent\NotNew.txt");
        }
    }
}
