//-----------------------------------------------------------------------
// <copyright file="ParallelOperationManagerTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConcurrentSample.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class ParallelOperationManagerTest
    {
        public ParallelOperationManagerTest()
        {
        }

        [Fact]
        public void Max_of_one_allows_only_one_call_at_a_time()
        {
            Queue<TaskCompletionSource<bool>> pending = new Queue<TaskCompletionSource<bool>>();
            Func<Task> doAsync = delegate
            {
                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                pending.Enqueue(tcs);
                Assert.Equal(1, pending.Count);
                return tcs.Task;
            };

            ParallelOperationManager manager = new ParallelOperationManager(1, doAsync);
            Task task = manager.RunAsync(1);

            Assert.False(task.IsCompleted);
            Assert.Equal(1, pending.Count);

            TaskCompletionSource<bool> current = pending.Dequeue();
            current.SetResult(false);
        }
    }
}
