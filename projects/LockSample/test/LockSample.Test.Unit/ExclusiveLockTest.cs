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
    }
}
