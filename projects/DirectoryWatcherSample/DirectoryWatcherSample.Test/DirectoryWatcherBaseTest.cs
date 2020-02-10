// <copyright file="DirectoryWatcherBaseTest.cs" company="Brian Rogers">
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

        [TestMethod]
        public void UpdateIrrelevantFileOneSubscription()
        {
            List<string> updates = new List<string>();
            FakeDirectoryWatcher watcher = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));
            DirectoryWatcherBase watcherBase = watcher;
            watcherBase.Subscribe("file1.txt", f => updates.Add(f.FullName));

            watcher.Update(@"X:\root\not-relevant.txt");

            updates.Should().BeEmpty();
        }

        [TestMethod]
        public void UpdateRelevantFileOneSubscription()
        {
            List<string> updates = new List<string>();
            FakeDirectoryWatcher watcher = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));
            DirectoryWatcherBase watcherBase = watcher;
            watcherBase.Subscribe("file1.txt", f => updates.Add(f.FullName));

            watcher.Update(@"X:\root\file1.txt");

            updates.Should().ContainSingle().Which.Should().Be(@"X:\root\file1.txt");
        }

        [TestMethod]
        public void UpdateRelevantFileOneSubscriptionDifferentCase()
        {
            List<string> updates = new List<string>();
            FakeDirectoryWatcher watcher = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));
            DirectoryWatcherBase watcherBase = watcher;
            watcherBase.Subscribe("file1.txt", f => updates.Add(f.FullName));

            watcher.Update(@"X:\root\FILE1.txt");

            updates.Should().ContainSingle().Which.Should().Be(@"X:\root\file1.txt");
        }

        [TestMethod]
        public void UpdateTwoFilesTwoSubscriptions()
        {
            List<string> updates = new List<string>();
            FakeDirectoryWatcher watcher = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));
            DirectoryWatcherBase watcherBase = watcher;
            watcherBase.Subscribe("file1.txt", f => updates.Add(f.FullName));
            watcherBase.Subscribe("file2.txt", f => updates.Add(f.FullName));

            watcher.Update(@"X:\root\file2.txt");
            watcher.Update(@"X:\root\file1.txt");

            updates.Should().HaveCount(2).And.ContainInOrder(
                @"X:\root\file2.txt",
                @"X:\root\file1.txt");
        }

        [TestMethod]
        public void UpdateAfterDispose()
        {
            List<string> updates = new List<string>();
            FakeDirectoryWatcher watcher = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));
            using (DirectoryWatcherBase watcherBase = watcher)
            {
                watcherBase.Subscribe("file1.txt", f => updates.Add(f.FullName));
            }

            watcher.Update(@"X:\root\file1.txt");

            updates.Should().BeEmpty();
        }

        private sealed class FakeDirectoryWatcher : DirectoryWatcherBase
        {
            public FakeDirectoryWatcher(DirectoryInfo path)
                : base(path)
            {
            }

            public bool Disposed { get; private set; }

            public void Update(string fullPath)
            {
                if (!this.Disposed)
                {
                    this.OnUpdated(fullPath);
                }
            }

            protected override void Dispose(bool disposing) => this.Disposed = true;
        }
    }
}
