//-----------------------------------------------------------------------
// <copyright file="ExclusiveLockTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace LockSample.Test.Unit
{
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
            Task<ExclusiveLock.Token> task = l.AcquireAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            ExclusiveLock.Token token = task.Result;

            l.Release(token);
        }

        [Fact]
        public void First_acquire_completes_sync_next_acquire_is_pending_until_first_release()
        {
            ExclusiveLock l = new ExclusiveLock();
            Task<ExclusiveLock.Token> acquireTask1 = l.AcquireAsync();

            Assert.Equal(TaskStatus.RanToCompletion, acquireTask1.Status);
            ExclusiveLock.Token token = acquireTask1.Result;

            Task<ExclusiveLock.Token> acquireTask2 = l.AcquireAsync();

            Assert.False(acquireTask2.IsCompleted);
            Assert.False(acquireTask2.IsFaulted);

            l.Release(token);

            Assert.Equal(TaskStatus.RanToCompletion, acquireTask2.Status);
        }
    }
}
