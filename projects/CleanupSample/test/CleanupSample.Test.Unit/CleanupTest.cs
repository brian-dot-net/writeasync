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

        [Fact]
        public void Should_execute_cleanup_steps_on_sync_exception_in_delegate_and_complete_with_exception()
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

            InvalidTimeZoneException expectedException = new InvalidTimeZoneException("Expected.");
            Func<CleanupGuard, Task> doAsync = delegate(CleanupGuard g)
            {
                throw expectedException;
            };

            Task task = guard.RunAsync(doAsync);

            Assert.Equal(TaskStatus.Faulted, task.Status);
            Assert.NotNull(task.Exception);
            AggregateException ae = Assert.IsType<AggregateException>(task.Exception).Flatten();
            Assert.Equal(1, ae.InnerExceptions.Count);
            Assert.Same(expectedException, ae.InnerExceptions[0]);
            Assert.Equal(new int[] { 2, 1 }, steps.ToArray());
        }

        [Fact]
        public void Should_execute_cleanup_steps_on_async_exception_in_delegate_and_complete_with_exception()
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

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            Task task = guard.RunAsync(g => tcs.Task);

            Assert.False(task.IsCompleted);

            InvalidTimeZoneException expectedException = new InvalidTimeZoneException("Expected.");
            tcs.SetException(expectedException);

            Assert.Equal(TaskStatus.Faulted, task.Status);
            Assert.NotNull(task.Exception);
            AggregateException ae = Assert.IsType<AggregateException>(task.Exception).Flatten();
            Assert.Equal(1, ae.InnerExceptions.Count);
            Assert.Same(expectedException, ae.InnerExceptions[0]);
            Assert.Equal(new int[] { 2, 1 }, steps.ToArray());
        }
    }
}