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

        [TestMethod]
        public void UpdateTwoAfterOneSubscriptionDispose()
        {
            List<string> updates = new List<string>();
            FakeDirectoryWatcher watcher = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));
            DirectoryWatcherBase watcherBase = watcher;
            using (watcherBase.Subscribe("file1.txt", f => updates.Add(f.FullName)))
            {
            }

            watcherBase.Subscribe("file2.txt", f => updates.Add(f.FullName));
            watcher.Update(@"X:\root\file1.txt");
            watcher.Update(@"X:\root\file2.txt");

            updates.Should().ContainSingle().Which.Should().Be(@"X:\root\file2.txt");
        }

        [TestMethod]
        public void AfterSubscriptionDisposeSubscribeAgainAndUpdate()
        {
            List<string> updates = new List<string>();
            FakeDirectoryWatcher watcher = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));
            DirectoryWatcherBase watcherBase = watcher;
            using (watcherBase.Subscribe("file1.txt", f => updates.Add(f.FullName)))
            {
            }

            watcherBase.Subscribe("file1.txt", f => updates.Add(f.FullName));
            watcherBase.Subscribe("file2.txt", f => updates.Add(f.FullName));
            watcher.Update(@"X:\root\file1.txt");
            watcher.Update(@"X:\root\file2.txt");

            updates.Should().HaveCount(2).And.ContainInOrder(
                @"X:\root\file1.txt",
                @"X:\root\file2.txt");
        }

        [TestMethod]
        public void SubscribeSameFileTwice()
        {
            DirectoryWatcherBase watcherBase = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));
            Action<FileInfo> onUpdate = f => { };
            watcherBase.Subscribe("file1.txt", onUpdate);

            Action act = () => watcherBase.Subscribe("file1.txt", onUpdate);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage(@"A subscription for 'X:\root\file1.txt' already exists.");
        }

        [TestMethod]
        public void SubscribeSameFileTwiceDifferentCase()
        {
            DirectoryWatcherBase watcherBase = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));
            Action<FileInfo> onUpdate = f => { };
            watcherBase.Subscribe("file1.txt", onUpdate);

            Action act = () => watcherBase.Subscribe("FILE1.txt", onUpdate);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage(@"A subscription for 'X:\root\FILE1.txt' already exists.");
        }

        [TestMethod]
        public void SubscribeNullFile()
        {
            DirectoryWatcherBase watcherBase = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));
            Action<FileInfo> onUpdate = f => { };
            string file = null;

            Action act = () => watcherBase.Subscribe(file, onUpdate);

            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("file");
        }

        [TestMethod]
        public void SubscribeNullCallback()
        {
            DirectoryWatcherBase watcherBase = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));
            Action<FileInfo> onUpdate = null;

            Action act = () => watcherBase.Subscribe("file1.txt", onUpdate);

            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("onUpdate");
        }

        [TestMethod]
        public void SubscribeBadFileName()
        {
            DirectoryWatcherBase watcherBase = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));
            Action<FileInfo> onUpdate = f => { };

            Action act = () => watcherBase.Subscribe("**BADNAME??", onUpdate);

            ArgumentException ae = act.Should().Throw<ArgumentException>().Which;
            ae.Message.Should().Contain("Invalid file name '**BADNAME??'");
            ae.ParamName.Should().Be("file");
        }

        [TestMethod]
        public void SubscribeEmptyFileName()
        {
            DirectoryWatcherBase watcherBase = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));
            Action<FileInfo> onUpdate = f => { };

            Action act = () => watcherBase.Subscribe(string.Empty, onUpdate);

            ArgumentException ae = act.Should().Throw<ArgumentException>().Which;
            ae.Message.Should().Contain("File name cannot be empty");
            ae.ParamName.Should().Be("file");
        }

        [TestMethod]
        public void SubscribeDotFileName()
        {
            DirectoryWatcherBase watcherBase = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));
            Action<FileInfo> onUpdate = f => { };

            Action act = () => watcherBase.Subscribe(".", onUpdate);

            ArgumentException ae = act.Should().Throw<ArgumentException>().Which;
            ae.Message.Should().Contain(@"The file '.' is not directly within directory 'X:\root'");
            ae.ParamName.Should().Be("file");
        }

        [TestMethod]
        public void SubscribeFileRelativeSubDir()
        {
            DirectoryWatcherBase watcherBase = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));
            Action<FileInfo> onUpdate = f => { };

            Action act = () => watcherBase.Subscribe(@"inner\file1.txt", onUpdate);

            ArgumentException ae = act.Should().Throw<ArgumentException>().Which;
            ae.Message.Should().Contain(@"Invalid file name 'inner\file1.txt'");
            ae.ParamName.Should().Be("file");
        }

        [TestMethod]
        public void SubscribeFileRelativeOutsideDir()
        {
            DirectoryWatcherBase watcherBase = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));
            Action<FileInfo> onUpdate = f => { };

            Action act = () => watcherBase.Subscribe(@"..\file1.txt", onUpdate);

            ArgumentException ae = act.Should().Throw<ArgumentException>().Which;
            ae.Message.Should().Contain(@"Invalid file name '..\file1.txt'");
            ae.ParamName.Should().Be("file");
        }

        [TestMethod]
        public void SubscribeFileRootedPath()
        {
            DirectoryWatcherBase watcherBase = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));
            Action<FileInfo> onUpdate = f => { };

            Action act = () => watcherBase.Subscribe(@"D:\path.txt", onUpdate);

            ArgumentException ae = act.Should().Throw<ArgumentException>().Which;
            ae.Message.Should().Contain(@"Invalid file name 'D:\path.txt'");
            ae.ParamName.Should().Be("file");
        }
    }
}
