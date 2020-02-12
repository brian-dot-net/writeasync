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
        public void UpdateIrrelevantFileOneSubscription()
        {
            List<string> updates = new List<string>();
            FakeDirectoryTreeWatcher watcher = new FakeDirectoryTreeWatcher(new DirectoryInfo(@"X:\root"));
            DirectoryTreeWatcherBase watcherBase = watcher;
            watcherBase.Subscribe("file1.txt", f => updates.Add(f.FullName));

            FakeDirectoryWatcher innerWatcher = watcher.Watchers.Should().ContainSingle().Which;
            innerWatcher.Update(@"X:\root\not-relevant.txt");

            updates.Should().BeEmpty();
        }

        [TestMethod]
        public void UpdateRelevantFileOneSubscription()
        {
            List<string> updates = new List<string>();
            FakeDirectoryTreeWatcher watcher = new FakeDirectoryTreeWatcher(new DirectoryInfo(@"X:\root"));
            DirectoryTreeWatcherBase watcherBase = watcher;
            watcherBase.Subscribe("file1.txt", f => updates.Add(f.FullName));

            FakeDirectoryWatcher innerWatcher = watcher.Watchers.Should().ContainSingle().Which;
            innerWatcher.Update(@"X:\root\file1.txt");

            updates.Should().ContainSingle().Which.Should().Be(@"X:\root\file1.txt");
        }

        [TestMethod]
        public void UpdateRelevantFileOneSubscriptionDifferentCase()
        {
            List<string> updates = new List<string>();
            FakeDirectoryTreeWatcher watcher = new FakeDirectoryTreeWatcher(new DirectoryInfo(@"X:\root"));
            DirectoryTreeWatcherBase watcherBase = watcher;
            watcherBase.Subscribe("file1.txt", f => updates.Add(f.FullName));

            FakeDirectoryWatcher innerWatcher = watcher.Watchers.Should().ContainSingle().Which;
            innerWatcher.Update(@"X:\root\FILE1.txt");

            updates.Should().ContainSingle().Which.Should().Be(@"X:\root\file1.txt");
        }

        private sealed class FakeDirectoryTreeWatcher : DirectoryTreeWatcherBase
        {
            public FakeDirectoryTreeWatcher(DirectoryInfo path)
                : base(path)
            {
                this.Watchers = new List<FakeDirectoryWatcher>();
            }

            public IList<FakeDirectoryWatcher> Watchers { get; }

            protected override DirectoryWatcherBase Create(DirectoryInfo path)
            {
                FakeDirectoryWatcher watcher = new FakeDirectoryWatcher(path);
                this.Watchers.Add(watcher);
                return watcher;
            }

            protected override void Dispose(bool disposing)
            {
            }
        }
    }
}
