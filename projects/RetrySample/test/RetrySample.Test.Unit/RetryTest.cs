//-----------------------------------------------------------------------
// <copyright file="RetryTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace RetrySample.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class RetryTest
    {
        public RetryTest()
        {
        }

        [Fact]
        public void Execute_runs_func_once_returns_context()
        {
            int count = 0;
            RetryLoop loop = new RetryLoop(() => Task.FromResult(++count));

            Task<RetryContext> task = loop.ExecuteAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(1, count);
            RetryContext context = task.Result;
            Assert.Equal(1, context.Iteration);
            Assert.NotEqual(TimeSpan.Zero, context.ElapsedTime);
        }

        [Fact]
        public void Execute_runs_func_until_condition_false_returns_context()
        {
            int count = 0;
            RetryLoop loop = new RetryLoop(() => Task.FromResult(++count));
            loop.Condition = r => r.Iteration < 2;

            Task<RetryContext> task = loop.ExecuteAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(2, count);
            RetryContext context = task.Result;
            Assert.Equal(2, context.Iteration);
        }

        [Fact]
        public void Execute_runs_func_returns_context_with_elapsed_time()
        {
            int count = 0;
            RetryLoop loop = new RetryLoop(() => Task.FromResult(++count));
            ElapsedTimerStub timer = new ElapsedTimerStub();
            loop.Timer = timer;

            timer.ElapsedTimes.Enqueue(TimeSpan.FromSeconds(5.0d));
            Task<RetryContext> task = loop.ExecuteAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(1, count);
            RetryContext context = task.Result;
            Assert.Equal(1, context.Iteration);
            timer.ElapsedTimes.Enqueue(TimeSpan.FromSeconds(13.0d));
            Assert.Equal(TimeSpan.FromSeconds(8.0d), context.ElapsedTime);
        }

        private sealed class ElapsedTimerStub : IElapsedTimer
        {
            public ElapsedTimerStub()
            {
                this.ElapsedTimes = new Queue<TimeSpan>();
            }

            public Queue<TimeSpan> ElapsedTimes { get; private set; }

            public TimeSpan Elapsed
            {
                get { return this.ElapsedTimes.Dequeue(); }
            }
        }
    }
}
