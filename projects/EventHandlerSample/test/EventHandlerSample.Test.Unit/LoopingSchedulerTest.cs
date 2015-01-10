//-----------------------------------------------------------------------
// <copyright file="LoopingSchedulerTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventHandlerSample.Test.Unit
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LoopingSchedulerTest
    {
        public LoopingSchedulerTest()
        {
        }

        [TestMethod]
        public void RunThrowsOnFirstException()
        {
            Exception exception = new InvalidOperationException("Expected.");
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            tcs.SetException(exception);
            LoopingScheduler scheduler = new LoopingScheduler(() => tcs.Task);
            
            Task task = scheduler.RunAsync();

            task.IsCompleted.Should().BeTrue();
            task.Exception.InnerExceptions.Should().HaveCount(1).And.Contain(exception);
        }
    }
}
