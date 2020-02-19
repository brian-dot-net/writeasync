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

        [TestMethod]
        public void AddOneAndCompleteTwice()
        {
            Queue<TaskCompletionSource<bool>> pending = new Queue<TaskCompletionSource<bool>>();
            BatchedEvents<string> events = new BatchedEvents<string>(() => pending.Dequeue().Task);
            List<string> batches = new List<string>();
            TaskCompletionSource<bool> pending1 = new TaskCompletionSource<bool>();
            TaskCompletionSource<bool> pending2 = new TaskCompletionSource<bool>();
            pending.Enqueue(pending1);
            pending.Enqueue(pending2);

            events.Subscribe("item1", i => batches.Add(i));

            events.Add("item1", new TimePoint(1));

            batches.Should().BeEmpty();

            pending1.SetResult(true);

            batches.Should().ContainSingle().Which.Should().Be("item1");
            batches.Clear();

            events.Add("item1", new TimePoint(2));

            batches.Should().BeEmpty();

            pending2.SetResult(true);

            batches.Should().ContainSingle().Which.Should().Be("item1");
        }

        [TestMethod]
        public void AddTwoDifferentBatches()
        {
            Queue<TaskCompletionSource<bool>> pending = new Queue<TaskCompletionSource<bool>>();
            BatchedEvents<string> events = new BatchedEvents<string>(() => pending.Dequeue().Task);
            List<string> batches = new List<string>();
            TaskCompletionSource<bool> pending1 = new TaskCompletionSource<bool>();
            TaskCompletionSource<bool> pending2 = new TaskCompletionSource<bool>();
            pending.Enqueue(pending1);
            pending.Enqueue(pending2);

            events.Subscribe("item1", i => batches.Add("A:" + i));
            events.Subscribe("item2", i => batches.Add("B:" + i));

            events.Add("item1", new TimePoint(1));
            events.Add("item2", new TimePoint(1));

            batches.Should().BeEmpty();

            pending1.SetResult(true);

            batches.Should().ContainSingle().Which.Should().Be("A:item1");
            batches.Clear();

            pending2.SetResult(true);

            batches.Should().ContainSingle().Which.Should().Be("B:item2");
        }
    }
}
