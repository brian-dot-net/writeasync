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
    }
}
