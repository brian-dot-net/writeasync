//-----------------------------------------------------------------------
// <copyright file="OperationManagerTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConcurrentSample.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class OperationManagerTest
    {
        public OperationManagerTest()
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

            OperationManager manager = new OperationManager(1, doAsync);
            Task task = manager.RunAsync(1);

            Assert.False(task.IsCompleted);
            Assert.Equal(1, pending.Count);

            TaskCompletionSource<bool> current = pending.Dequeue();
            current.SetResult(false);
        }

        [Fact]
        public void Max_of_one_with_call_count_3_allows_only_one_call_at_a_time_for_3_iterations()
        {
            Queue<TaskCompletionSource<bool>> pending = new Queue<TaskCompletionSource<bool>>();
            Func<Task> doAsync = delegate
            {
                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                pending.Enqueue(tcs);
                Assert.Equal(1, pending.Count);
                return tcs.Task;
            };

            OperationManager manager = new OperationManager(1, doAsync);
            Task task = manager.RunAsync(3);

            Assert.False(task.IsCompleted);
            Assert.Equal(1, pending.Count);

            TaskCompletionSource<bool> current = pending.Dequeue();
            current.SetResult(false);

            Assert.False(task.IsCompleted);
            Assert.Equal(1, pending.Count);

            current = pending.Dequeue();
            current.SetResult(false);

            Assert.False(task.IsCompleted);
            Assert.Equal(1, pending.Count);

            current = pending.Dequeue();
            current.SetResult(false);

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(0, pending.Count);
        }

        [Fact]
        public void Max_of_2_with_call_count_3_allows_2_calls_at_a_time_for_3_total()
        {
            Queue<TaskCompletionSource<bool>> pending = new Queue<TaskCompletionSource<bool>>();
            Func<Task> doAsync = delegate
            {
                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                pending.Enqueue(tcs);
                Assert.True(pending.Count < 3, "Pending calls should be less than 3");
                return tcs.Task;
            };

            OperationManager manager = new OperationManager(2, doAsync);
            Task task = manager.RunAsync(3);

            Assert.False(task.IsCompleted);
            Assert.Equal(2, pending.Count);

            TaskCompletionSource<bool> current = pending.Dequeue();
            current.SetResult(false);

            Assert.False(task.IsCompleted);
            Assert.Equal(2, pending.Count);

            current = pending.Dequeue();
            current.SetResult(false);

            Assert.False(task.IsCompleted);
            Assert.Equal(1, pending.Count);

            current = pending.Dequeue();
            current.SetResult(false);

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(0, pending.Count);
        }

        [Fact]
        public void First_exception_fails_operation_without_executing_remaining_calls()
        {
            Queue<TaskCompletionSource<bool>> pending = new Queue<TaskCompletionSource<bool>>();
            Func<Task> doAsync = delegate
            {
                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                pending.Enqueue(tcs);
                return tcs.Task;
            };

            OperationManager manager = new OperationManager(1, doAsync);
            Task task = manager.RunAsync(3);

            Assert.False(task.IsCompleted);
            Assert.Equal(1, pending.Count);

            TaskCompletionSource<bool> current = pending.Dequeue();
            InvalidProgramException exception = new InvalidProgramException("expected");
            current.SetException(exception);

            Assert.False(task.IsCompleted);
            Assert.Equal(1, pending.Count);

            current = pending.Dequeue();
            current.SetResult(false);

            Assert.True(task.IsCompleted);
            Assert.True(task.IsFaulted);
            Assert.NotNull(task.Exception);
            AggregateException ae = Assert.IsType<AggregateException>(task.Exception).Flatten();
            Assert.Equal(1, ae.InnerExceptions.Count);
            Assert.Same(exception, ae.InnerExceptions[0]);
        }
    }
}
