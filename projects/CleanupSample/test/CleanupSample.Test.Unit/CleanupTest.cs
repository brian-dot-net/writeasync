//-----------------------------------------------------------------------
// <copyright file="CleanupTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CleanupSample.Test.Unit
{
    using System;
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
    }
}
