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

            Assert.Throws<InvalidOperationException>(() => l.Release(new MyToken()));
        }

        [Fact]
        public void Release_same_token_twice_throws_InvalidOperation()
        {
            ExclusiveLock l = new ExclusiveLock();

            ExclusiveLock.Token token = AssertTaskCompleted(l.AcquireAsync());

            l.Release(token);

            Assert.Throws<InvalidOperationException>(() => l.Release(token));
        }

        [Fact]
        public void Three_acquires_first_completes_sync_next_acquires_are_pending_until_previous_owners_release()
        {
            ExclusiveLock l = new ExclusiveLock();
            ExclusiveLock.Token token1 = AssertTaskCompleted(l.AcquireAsync());

            Task<ExclusiveLock.Token> acquireTask1 = AssertTaskPending(l.AcquireAsync());
            Task<ExclusiveLock.Token> acquireTask2 = AssertTaskPending(l.AcquireAsync());

            l.Release(token1);

            ExclusiveLock.Token token2 = AssertTaskCompleted(acquireTask1);

            l.Release(token2);

            AssertTaskCompleted(acquireTask2);
        }

        [Fact]
        public void Acquire_and_release_three_times_in_a_row_completes_sync_each_time()
        {
            ExclusiveLock l = new ExclusiveLock();

            ExclusiveLock.Token token1 = AssertTaskCompleted(l.AcquireAsync());
            l.Release(token1);

            ExclusiveLock.Token token2 = AssertTaskCompleted(l.AcquireAsync());
            l.Release(token2);

            ExclusiveLock.Token token3 = AssertTaskCompleted(l.AcquireAsync());
            l.Release(token3);
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

        private sealed class MyToken : ExclusiveLock.Token
        {
            public MyToken()
            {
            }
        }
    }
}
