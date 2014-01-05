//-----------------------------------------------------------------------
// <copyright file="CleanupTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CleanupSample.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class CleanupTest
    {
        public CleanupTest()
        {
        }

        [Fact]
        public void Should_execute_delegate_and_pass_self_as_param()
        {
            CleanupGuard guard = new CleanupGuard();
            bool executed = false;
            Func<CleanupGuard, Task> doAsync = delegate(CleanupGuard g)
            {
                Assert.Same(guard, g);
                executed = true;
                return Task.FromResult(false);
            };

            Task task = guard.RunAsync(doAsync);

            Assert.True(executed);
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void Should_execute_cleanup_steps_in_opposite_order_after_delegate()
        {
            CleanupGuard guard = new CleanupGuard();
            List<int> steps = new List<int>();
            Func<int, Task> doStepAsync = delegate(int i)
            {
                steps.Add(i);
                return Task.FromResult(false);
            };

            guard.Steps.Push(() => doStepAsync(1));
            guard.Steps.Push(() => doStepAsync(2));

            bool executed = false;
            Func<CleanupGuard, Task> doAsync = delegate(CleanupGuard g)
            {
                executed = true;
                return Task.FromResult(false);
            };

            Task task = guard.RunAsync(doAsync);

            Assert.True(executed);
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(new int[] { 2, 1 }, steps.ToArray());
        }
    }
}
