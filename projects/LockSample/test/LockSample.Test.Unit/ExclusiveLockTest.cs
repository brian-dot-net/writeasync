//-----------------------------------------------------------------------
// <copyright file="ExclusiveLockTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace LockSample.Test.Unit
{
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class ExclusiveLockTest
    {
        public ExclusiveLockTest()
        {
        }

        [Fact]
        public void Acquire_completes_sync_then_release_succeeds()
        {
            ExclusiveLock l = new ExclusiveLock();

            ExclusiveLock.Token token = AssertTaskCompleted(l.AcquireAsync());

            l.Release(token);
        }

        [Fact]
        public void First_acquire_completes_sync_next_acquire_is_pending_until_first_release()
        {
            ExclusiveLock l = new ExclusiveLock();
            ExclusiveLock.Token token = AssertTaskCompleted(l.AcquireAsync());

            Task<ExclusiveLock.Token> nextAcquireTask = AssertTaskPending(l.AcquireAsync());

            l.Release(token);

            AssertTaskCompleted(nextAcquireTask);
        }

        [Fact]
        public void Release_invalid_token_throws_InvalidOperation()
        {
            ExclusiveLock l = new ExclusiveLock();

            Assert.Throws<InvalidOperationException>(() => l.Release(new ExclusiveLock.Token()));
        }

        private static TResult AssertTaskCompleted<TResult>(Task<TResult> task)
        {
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            return task.Result;
        }

        private static Task<TResult> AssertTaskPending<TResult>(Task<TResult> task)
        {
            Assert.False(task.IsCompleted, "Task should not be completed.");
            Assert.False(task.IsFaulted, "Task should not be faulted: " + task.Exception);
            return task;
        }
    }
}
