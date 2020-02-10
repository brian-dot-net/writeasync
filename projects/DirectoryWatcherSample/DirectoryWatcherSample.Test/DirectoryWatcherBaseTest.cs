// <copyright file="DirectoryWatcherBaseTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample.Test
{
    using System;
    using System.IO;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class DirectoryWatcherBaseTest
    {
        [TestMethod]
        public void CreateNullPath()
        {
            DirectoryInfo path = null;

            Action act = () => new FakeDirectoryWatcher(path);

            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("path");
        }

        [TestMethod]
        public void CreateAndDispose()
        {
            DirectoryWatcherBase watcher = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));

            Action act = () => watcher.Dispose();

            act.Should().NotThrow();
        }

        [TestMethod]
        public void UpdateZeroSubscriptions()
        {
            FakeDirectoryWatcher watcher = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));

            Action act = () => watcher.Update(@"X:\root\file1.txt");

            act.Should().NotThrow();
        }

        private sealed class FakeDirectoryWatcher : DirectoryWatcherBase
        {
            public FakeDirectoryWatcher(DirectoryInfo path)
                : base(path)
            {
            }

            public void Update(string fullPath)
            {
            }
        }
    }
}
