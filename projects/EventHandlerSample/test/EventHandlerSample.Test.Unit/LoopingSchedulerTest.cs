//-----------------------------------------------------------------------
// <copyright file="LoopingSchedulerTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventHandlerSample.Test.Unit
{
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
        public void ShouldReturnNameAfterCompletingSync()
        {
            LoopingScheduler c = new LoopingScheduler("MyName");
            
            Task<string> task = c.DoAsync();

            task.IsCompleted.Should().BeTrue();
            task.Result.Should().Be("MyName");
        }
    }
}
