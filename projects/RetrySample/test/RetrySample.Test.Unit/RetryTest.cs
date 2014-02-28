//-----------------------------------------------------------------------
// <copyright file="RetryTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace RetrySample.Test.Unit
{
    using System.Threading.Tasks;
    using Xunit;

    public class RetryTest
    {
        public RetryTest()
        {
        }

        [Fact]
        public void Execute_runs_func_once()
        {
            int count = 0;
            RetryLoop loop = new RetryLoop(() => Task.FromResult(++count));

            Task task = loop.ExecuteAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(1, count);
        }
    }
}
