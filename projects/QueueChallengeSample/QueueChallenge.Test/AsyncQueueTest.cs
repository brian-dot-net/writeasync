// <copyright file="AsyncQueueTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace QueueChallenge.Test
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class AsyncQueueTest
    {
        [TestMethod]
        public void EmptyPending()
        {
            AsyncQueue<string> queue = new AsyncQueue<string>();

            Task<string> task = queue.DequeueAsync();

            task.IsCompleted.Should().BeFalse();
        }

        [TestMethod]
        public void EnqueueThenDequeue()
        {
            AsyncQueue<string> queue = new AsyncQueue<string>();

            queue.Enqueue("one");
            Task<string> task = queue.DequeueAsync();

            task.IsCompletedSuccessfully.Should().BeTrue();
            task.Result.Should().Be("one");
        }

        [TestMethod]
        public void DequeueThenEnqueue()
        {
            AsyncQueue<string> queue = new AsyncQueue<string>();

            Task<string> task = queue.DequeueAsync();
            queue.Enqueue("one");

            task.IsCompletedSuccessfully.Should().BeTrue();
            task.Result.Should().Be("one");
        }

        [TestMethod]
        public void EnqueueTwoThenDequeueTwo()
        {
            AsyncQueue<string> queue = new AsyncQueue<string>();

            queue.Enqueue("one");
            queue.Enqueue("two");

            Task<string> task = queue.DequeueAsync();
            task.IsCompletedSuccessfully.Should().BeTrue();
            task.Result.Should().Be("one");

            task = queue.DequeueAsync();
            task.IsCompletedSuccessfully.Should().BeTrue();
            task.Result.Should().Be("two");
        }

        [TestMethod]
        public void EmptyPendingDequeueAgain()
        {
            AsyncQueue<string> queue = new AsyncQueue<string>();

            Task<string> task = queue.DequeueAsync();
            Action act = () => queue.DequeueAsync();

            act.Should().Throw<InvalidOperationException>();

            task.IsCompleted.Should().BeFalse();
        }
    }
}
