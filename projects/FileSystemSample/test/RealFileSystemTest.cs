// <copyright file="RealFileSystemTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace FileSystemSample.Test
{
    using System;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RealFileSystemTest : FileSystemContract
    {
        private FullPath root;

        protected override bool MustBlock => true;

        protected override FullPath Root => this.root;

        [TestInitialize]
        public void TestInitialize()
        {
            // Create a new root folder every time to avoid mixing state between tests.
            this.root = new FullPath(@".\fstest\" + Guid.NewGuid().ToString("n"));
            Directory.CreateDirectory(this.root.ToString());
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Directory.Delete(this.root.ToString(), true);
        }

        protected override IFileSystem Create() => new RealFileSystem();
    }
}
