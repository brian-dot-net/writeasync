// <copyright file="FileSystemContract.Directory.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace FileSystemSample.Test
{
    using System.Linq;
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
            IDirectory dir = this.CreateDir(fs, "NoFiles");

            ShouldGetNoFiles(dir);
        }

        [TestMethod]
        public void ShouldGetOneFileFromDirectoryWithOneFile()
        {
            IFileSystem fs = this.Create();
            IDirectory dir = this.CreateDir(fs, "Dir1");
            CreateFile(dir, "onefile.txt");

            ShouldGetOneFile(dir, @"*\Dir1\onefile.txt");
        }

        [TestMethod]
        public void ShouldGetTwoFilesFromDirectoryWithTwoFiles()
        {
            IFileSystem fs = this.Create();
            IDirectory dir = this.CreateDir(fs, "Dir1");
            CreateFile(dir, "onefile.txt");
            CreateFile(dir, "another.txt");

            ShouldGetTwoFiles(dir, @"*\Dir1\another.txt", @"*\Dir1\onefile.txt");
        }

        [TestMethod]
        public void ShouldGetTwoMatchingSuffixFilesFromDirectoryWithThreeFiles()
        {
            IFileSystem fs = this.Create();
            IDirectory dir = this.CreateDir(fs, "Dir1");
            CreateFile(dir, "File1.txt");
            CreateFile(dir, "File3.z");
            CreateFile(dir, "File2.txt");

            ShouldGetTwoMatchingFiles("*.txt", dir, @"*\Dir1\File1.txt", @"*\Dir1\File2.txt");
        }

        [TestMethod]
        public void ShouldGetTwoMatchingPrefixFilesFromDirectoryWithThreeFiles()
        {
            IFileSystem fs = this.Create();
            IDirectory dir = this.CreateDir(fs, "Dir1");
            CreateFile(dir, "File1.txt");
            CreateFile(dir, "XFile3.txt");
            CreateFile(dir, "File2.txt");

            ShouldGetTwoMatchingFiles("File*", dir, @"*\Dir1\File1.txt", @"*\Dir1\File2.txt");
        }

        private static void ShouldGetNoFiles(IDirectory dir)
        {
            IFile[] files = dir.GetFiles("*");

            files.Should().BeEmpty();
        }

        private static void ShouldGetOneFile(IDirectory dir, string expectedMatch)
        {
            IFile[] files = dir.GetFiles("*");

            files.Should().ContainSingle().Which.Path.ToString().Should().Match(expectedMatch);
        }

        private static void ShouldGetTwoFiles(IDirectory dir, string expectedMatch0, string expectedMatch1)
        {
            ShouldGetTwoMatchingFiles("*", dir, expectedMatch0, expectedMatch1);
        }

        private static void ShouldGetTwoMatchingFiles(string pattern, IDirectory dir, string expectedMatch0, string expectedMatch1)
        {
            IFile[] files = dir.GetFiles(pattern);

            string[] paths = files.Select(f => f.Path.ToString()).OrderBy(p => p).ToArray();
            paths.Should().HaveCount(2);
            paths[0].Should().Match(expectedMatch0);
            paths[1].Should().Match(expectedMatch1);
        }
    }
}
