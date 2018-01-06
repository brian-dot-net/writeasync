// <copyright file="FakeFileSystemTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace FileSystemSample.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FakeFileSystemTest : FileSystemContract
    {
        protected override FullPath Root => new FullPath(@"X:\SomeRoot");

        protected override IFileSystem Create() => new FakeFileSystem();
    }
}
