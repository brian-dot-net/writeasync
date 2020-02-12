// <copyright file="DirectoryTreeWatcherBaseTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class DirectoryTreeWatcherBaseTest
    {
        [TestMethod]
        public void CreateNullPath()
        {
            DirectoryInfo path = null;

            Action act = () => new FakeDirectoryTreeWatcher(path);

            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("path");
        }

        [TestMethod]
        public void CreateAndDispose()
        {
            DirectoryTreeWatcherBase watcher = new FakeDirectoryTreeWatcher(new DirectoryInfo(@"X:\root"));

            Action act = () => watcher.Dispose();

            act.Should().NotThrow();
        }

        [TestMethod]
        public void UpdateZeroSubscriptions()
        {
            FakeDirectoryTreeWatcher watcher = new FakeDirectoryTreeWatcher(new DirectoryInfo(@"X:\root"));

            Action act = () => watcher.Update(@"X:\root\file1.txt");

            act.Should().NotThrow();
        }

        [TestMethod]
        public void UpdateIrrelevantFileOneSubscription()
        {
            List<string> updates = new List<string>();
            FakeDirectoryTreeWatcher watcher = new FakeDirectoryTreeWatcher(new DirectoryInfo(@"X:\root"));
            DirectoryTreeWatcherBase watcherBase = watcher;
            watcherBase.Subscribe("file1.txt", f => updates.Add(f.FullName));

            watcher.Update(@"X:\root\not-relevant.txt");

            updates.Should().BeEmpty();
        }

        private sealed class FakeDirectoryTreeWatcher : DirectoryTreeWatcherBase
        {
            public FakeDirectoryTreeWatcher(DirectoryInfo path)
                : base(path)
            {
            }

            public void Update(string fullPath)
            {
            }

            protected override void Dispose(bool disposing)
            {
            }
        }
    }
}
