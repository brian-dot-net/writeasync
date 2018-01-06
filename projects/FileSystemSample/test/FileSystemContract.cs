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

        private IDirectory CreateDir(IFileSystem fs, string name) => fs.Create(this.Root.Combine(new PathPart(name)));

        private IFile CreateFile(IFileSystem fs, string dirName, string fileName)
        {
            return this.CreateDir(fs, dirName).CreateFile(new PathPart(fileName));
        }
    }
}
