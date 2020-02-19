// <copyright file="BatchedEventsTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class BatchedEventsTest
    {
        [TestMethod]
        public void AddBeforeSubscribe()
        {
            BatchedEvents<string> events = new BatchedEvents<string>(() => Task.CompletedTask);

            Action act = () => events.Add("item1", new TimePoint(1));

            act.Should().NotThrow();
        }

        [TestMethod]
        public void AddOneAndComplete()
        {
            TaskCompletionSource<bool> pending = new TaskCompletionSource<bool>();
            BatchedEvents<string> events = new BatchedEvents<string>(() => pending.Task);
            List<string> batches = new List<string>();

            events.Subscribe("item1", i => batches.Add(i));
            events.Add("item1", new TimePoint(1));

            batches.Should().BeEmpty();

            pending.SetResult(true);

            batches.Should().ContainSingle().Which.Should().Be("item1");
        }

        [TestMethod]
        public void AddTwoAndComplete()
        {
            TaskCompletionSource<bool> pending = new TaskCompletionSource<bool>();
            BatchedEvents<string> events = new BatchedEvents<string>(() => pending.Task);
            List<string> batches = new List<string>();

            events.Subscribe("item1", i => batches.Add(i));
            events.Add("item1", new TimePoint(1));
            events.Add("item1", new TimePoint(2));

            batches.Should().BeEmpty();

            pending.SetResult(true);

            batches.Should().ContainSingle().Which.Should().Be("item1");
        }
    }
}
