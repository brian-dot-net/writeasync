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

        private IDirectory CreateDir(IFileSystem fs, string name)
        {
            return fs.Create(this.Root.Combine(name));
        }
    }
}
