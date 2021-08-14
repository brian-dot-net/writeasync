// <copyright file="AsyncQueueTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace QueueChallenge.Test
{
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

            queue.EnqueueAsync("one");
            Task<string> task = queue.DequeueAsync();

            task.IsCompletedSuccessfully.Should().BeTrue();
            task.Result.Should().Be("one");
        }

        [TestMethod]
        public void DequeueThenEnqueue()
        {
            AsyncQueue<string> queue = new AsyncQueue<string>();

            Task<string> task = queue.DequeueAsync();
            queue.EnqueueAsync("one");

            task.IsCompletedSuccessfully.Should().BeTrue();
            task.Result.Should().Be("one");
        }

        [TestMethod]
        public void EnqueueTwoThenDequeueTwo()
        {
            AsyncQueue<string> queue = new AsyncQueue<string>();

            queue.EnqueueAsync("one");
            queue.EnqueueAsync("two");

            Task<string> task = queue.DequeueAsync();
            task.IsCompletedSuccessfully.Should().BeTrue();
            task.Result.Should().Be("one");

            task = queue.DequeueAsync();
            task.IsCompletedSuccessfully.Should().BeTrue();
            task.Result.Should().Be("two");
        }
    }
}
