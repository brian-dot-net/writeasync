// <copyright file="FileSystemContract.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace FileSystemSample.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public abstract partial class FileSystemContract
    {
        protected FileSystemContract()
        {
        }

        protected abstract FullPath Root { get; }

        protected abstract IFileSystem Create();

        private static IFile CreateFile(IDirectory dir, string fileName) => dir.CreateFile(new PathPart(fileName));

        private IDirectory CreateDir(IFileSystem fs, string name) => fs.Create(this.Root.Combine(new PathPart(name)));

        private IFile CreateFile(IFileSystem fs, string dirName, string fileName)
        {
            IDirectory dir = this.CreateDir(fs, dirName);
            return CreateFile(dir, fileName);
        }
    }
}
