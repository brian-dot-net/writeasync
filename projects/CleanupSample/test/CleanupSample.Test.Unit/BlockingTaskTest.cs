//-----------------------------------------------------------------------
// <copyright file="BlockingTaskTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CleanupSample.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class BlockingTaskTest
    {
        public BlockingTaskTest()
        {
        }

        [Fact]
        public void Should_return_sync_func()
        {
            int inputValue = -1;
            Func<Task> func = Blocking.Task(i => inputValue = i, 333);

            Task task = func();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(333, inputValue);
        }
    }
}