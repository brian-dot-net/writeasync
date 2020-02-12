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

        [TestMethod]
        public void UpdateTwoFilesTwoSubscriptionsSameDir()
        {
            List<string> updates = new List<string>();
            FakeDirectoryTreeWatcher watcher = new FakeDirectoryTreeWatcher(new DirectoryInfo(@"X:\root"));
            DirectoryTreeWatcherBase watcherBase = watcher;
            watcherBase.Subscribe("file1.txt", f => updates.Add(f.FullName));
            watcherBase.Subscribe("file2.txt", f => updates.Add(f.FullName));

            FakeDirectoryWatcher innerWatcher = watcher.Watchers.Should().ContainSingle().Which;
            innerWatcher.Update(@"X:\root\file2.txt");
            innerWatcher.Update(@"X:\root\file1.txt");

            updates.Should().HaveCount(2).And.ContainInOrder(
                @"X:\root\file2.txt",
                @"X:\root\file1.txt");
        }

        [TestMethod]
        public void UpdateTwoFilesTwoSubscriptionsDifferentDir()
        {
            List<string> updates = new List<string>();
            FakeDirectoryTreeWatcher watcher = new FakeDirectoryTreeWatcher(new DirectoryInfo(@"X:\root"));
            DirectoryTreeWatcherBase watcherBase = watcher;
            watcherBase.Subscribe(@"inner1\file1.txt", f => updates.Add(f.FullName));
            watcherBase.Subscribe(@"inner2\file2.txt", f => updates.Add(f.FullName));

            watcher.Watchers.Should().HaveCount(2);
            watcher.Watchers[1].Update(@"X:\root\inner2\file2.txt");
            watcher.Watchers[0].Update(@"X:\root\inner1\file1.txt");

            updates.Should().HaveCount(2).And.ContainInOrder(
                @"X:\root\inner2\file2.txt",
                @"X:\root\inner1\file1.txt");
        }

        [TestMethod]
        public void UpdateAfterDispose()
        {
            List<string> updates = new List<string>();
            FakeDirectoryTreeWatcher watcher = new FakeDirectoryTreeWatcher(new DirectoryInfo(@"X:\root"));
            using (DirectoryTreeWatcherBase watcherBase = watcher)
            {
                watcherBase.Subscribe("file1.txt", f => updates.Add(f.FullName));
            }

            FakeDirectoryWatcher innerWatcher = watcher.Watchers.Should().ContainSingle().Which;
            innerWatcher.Update(@"X:\root\file1.txt");

            updates.Should().BeEmpty();
        }

        [TestMethod]
        public void UpdateTwoAfterOneSubscriptionDispose()
        {
            List<string> updates = new List<string>();
            FakeDirectoryTreeWatcher watcher = new FakeDirectoryTreeWatcher(new DirectoryInfo(@"X:\root"));
            DirectoryTreeWatcherBase watcherBase = watcher;
            using (watcherBase.Subscribe("file1.txt", f => updates.Add(f.FullName)))
            {
            }

            watcherBase.Subscribe("file2.txt", f => updates.Add(f.FullName));
            FakeDirectoryWatcher innerWatcher = watcher.Watchers.Should().ContainSingle().Which;
            innerWatcher.Update(@"X:\root\file1.txt");
            innerWatcher.Update(@"X:\root\file2.txt");

            updates.Should().ContainSingle().Which.Should().Be(@"X:\root\file2.txt");
        }

        [TestMethod]
        public void AfterSubscriptionDisposeSubscribeAgainAndUpdate()
        {
            List<string> updates = new List<string>();
            FakeDirectoryTreeWatcher watcher = new FakeDirectoryTreeWatcher(new DirectoryInfo(@"X:\root"));
            DirectoryTreeWatcherBase watcherBase = watcher;
            using (watcherBase.Subscribe("file1.txt", f => updates.Add(f.FullName)))
            {
            }

            watcherBase.Subscribe("file1.txt", f => updates.Add(f.FullName));
            watcherBase.Subscribe("file2.txt", f => updates.Add(f.FullName));
            FakeDirectoryWatcher innerWatcher = watcher.Watchers.Should().ContainSingle().Which;
            innerWatcher.Update(@"X:\root\file1.txt");
            innerWatcher.Update(@"X:\root\file2.txt");

            updates.Should().HaveCount(2).And.ContainInOrder(
                @"X:\root\file1.txt",
                @"X:\root\file2.txt");
        }

        [TestMethod]
        public void SubscribeSameFileTwice()
        {
            DirectoryTreeWatcherBase watcherBase = new FakeDirectoryTreeWatcher(new DirectoryInfo(@"X:\root"));
            Action<FileInfo> onUpdate = f => { };
            watcherBase.Subscribe("file1.txt", onUpdate);

            Action act = () => watcherBase.Subscribe("file1.txt", onUpdate);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage(@"A subscription for 'X:\root\file1.txt' already exists.");
        }

        [TestMethod]
        public void SubscribeSameFileTwiceDifferentCase()
        {
            DirectoryTreeWatcherBase watcherBase = new FakeDirectoryTreeWatcher(new DirectoryInfo(@"X:\root"));
            Action<FileInfo> onUpdate = f => { };
            watcherBase.Subscribe("file1.txt", onUpdate);

            Action act = () => watcherBase.Subscribe("FILE1.txt", onUpdate);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage(@"A subscription for 'X:\root\FILE1.txt' already exists.");
        }

        [TestMethod]
        public void SubscribeNullFile()
        {
            DirectoryTreeWatcherBase watcherBase = new FakeDirectoryTreeWatcher(new DirectoryInfo(@"X:\root"));
            Action<FileInfo> onUpdate = f => { };
            string file = null;

            Action act = () => watcherBase.Subscribe(file, onUpdate);

            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("file");
        }

        [TestMethod]
        public void SubscribeNullCallback()
        {
            DirectoryTreeWatcherBase watcherBase = new FakeDirectoryTreeWatcher(new DirectoryInfo(@"X:\root"));
            Action<FileInfo> onUpdate = null;

            Action act = () => watcherBase.Subscribe("file1.txt", onUpdate);

            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("onUpdate");
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
