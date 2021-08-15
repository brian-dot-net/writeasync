// <copyright file="UberQueueTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace QueueChallenge.Test
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class UberQueueTest
    {
        [TestMethod]
        public void ZeroQueues()
        {
            UberQueue<string> queue = new UberQueue<string>(Array.Empty<AsyncQueue<string>>());

            Task<string> task = queue.DequeueAsync();

            task.IsCompleted.Should().BeFalse();
        }

        [TestMethod]
        public void OneQueueDequeueEnqueue()
        {
            AsyncQueue<string> inner0 = new AsyncQueue<string>();
            UberQueue<string> queue = new UberQueue<string>(new AsyncQueue<string>[] { inner0 });

            Task<string> task = queue.DequeueAsync();

            task.IsCompleted.Should().BeFalse();

            inner0.Enqueue("one");

            task.IsCompletedSuccessfully.Should().BeTrue();
            task.Result.Should().Be("one");
        }

        [TestMethod]
        public void TwoQueuesDequeueEnqueueTwiceAlternating()
        {
            AsyncQueue<string> inner0 = new AsyncQueue<string>();
            AsyncQueue<string> inner1 = new AsyncQueue<string>();
            UberQueue<string> queue = new UberQueue<string>(new AsyncQueue<string>[] { inner0, inner1 });

            Task<string> task = queue.DequeueAsync();

            task.IsCompleted.Should().BeFalse();

            inner0.Enqueue("one");

            task.IsCompletedSuccessfully.Should().BeTrue();
            task.Result.Should().Be("one");

            task = queue.DequeueAsync();

            inner1.Enqueue("two");

            task.IsCompletedSuccessfully.Should().BeTrue();
            task.Result.Should().Be("two");
        }
    }
}
