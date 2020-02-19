// <copyright file="BatchedEventsTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample.Test
{
    using System;
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
    }
}
