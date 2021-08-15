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
    }
}
