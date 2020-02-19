// <copyright file="DirectoryWatcherWithBatchingTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class DirectoryWatcherWithBatchingTest
    {
        [TestMethod]
        public void BatchMultipleEvents()
        {
            List<string> events = new List<string>();
            FakeDirectoryWatcher inner = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));
            Queue<TaskCompletionSource<bool>> pending = new Queue<TaskCompletionSource<bool>>();
            IDirectoryWatcher watcher = new DirectoryWatcherWithBatching(
                inner,
                @"X:\root",
                new BatchedEvents<FileInfo>(() => pending.Dequeue().Task));
            TaskCompletionSource<bool> pending1 = new TaskCompletionSource<bool>();
            TaskCompletionSource<bool> pending2 = new TaskCompletionSource<bool>();
            pending.Enqueue(pending1);
            pending.Enqueue(pending2);

            IDisposable sub1 = watcher.Subscribe("file1.txt", f => events.Add("(Updated1) " + f.FullName));
            watcher.Subscribe("file2.txt", f => events.Add("(Updated2) " + f.FullName));
            inner.Update(@"X:\root\file1.txt");
            inner.Update(@"X:\root\file2.txt");
            sub1.Dispose();
            inner.Update(@"X:\root\file1.txt");
            pending1.SetResult(true);

            events.Should().BeEmpty();

            inner.Update(@"X:\root\file2.txt");
            pending2.SetResult(true);
            inner.Update(@"X:\root\file1.txt");
            watcher.Dispose();
            inner.Update(@"X:\root\file2.txt");

            events.Should().ContainSingle().Which.Should().Be(@"(Updated2) X:\root\file2.txt");
        }
    }
}
