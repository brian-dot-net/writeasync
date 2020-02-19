// <copyright file="DirectoryWatcherWithLoggingTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class DirectoryWatcherWithLoggingTest
    {
        [TestMethod]
        public void LogOnSubscribeUnsubscribeDispose()
        {
            List<string> logLines = new List<string>();
            FakeDirectoryWatcher inner = new FakeDirectoryWatcher(new DirectoryInfo(@"X:\root"));
            IDirectoryWatcher watcher = new DirectoryWatcherWithLogging(inner, "[\\Path]", new FakeLogger(logLines));

            IDisposable sub1 = watcher.Subscribe("file1.txt", f => logLines.Add("(Updated1) " + f.FullName));
            watcher.Subscribe("file2.txt", f => logLines.Add("(Updated2) " + f.FullName));
            inner.Update(@"X:\root\file1.txt");
            sub1.Dispose();
            inner.Update(@"X:\root\file1.txt");
            inner.Update(@"X:\root\file2.txt");
            watcher.Dispose();
            inner.Update(@"X:\root\file2.txt");

            logLines.Should().HaveCount(6).And.ContainInOrder(
                @"Information: Subscribing '[\Path]\file1.txt' watcher",
                @"Information: Subscribing '[\Path]\file2.txt' watcher",
                @"(Updated1) X:\root\file1.txt",
                @"Information: Disposing '[\Path]\file1.txt' watcher",
                @"(Updated2) X:\root\file2.txt",
                @"Information: Disposing '[\Path]' watcher");
        }

        private sealed class FakeLogger : ILogger
        {
            private readonly IList<string> logLines;

            public FakeLogger(IList<string> logLines)
            {
                this.logLines = logLines;
            }

            public IDisposable BeginScope<TState>(TState state) => null;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                this.logLines.Add($"{logLevel}: {formatter(state, exception)}");
            }
        }
    }
}
