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
            RetryLoop loop = new RetryLoop(r => Task.FromResult(++count));

            Task<RetryContext> task = loop.ExecuteAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(1, count);
            RetryContext context = task.Result;
            Assert.Equal(1, context.Iteration);
            Assert.NotEqual(TimeSpan.Zero, context.ElapsedTime);
        }

        [Fact]
        public void Execute_runs_func_until_should_not_retry_returns_context()
        {
            int count = 0;
            RetryLoop loop = new RetryLoop(r => Task.FromResult(++count));
            loop.ShouldRetry = r => r.Iteration < 2;
            loop.Succeeded = r => false;

            Task<RetryContext> task = loop.ExecuteAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(2, count);
            RetryContext context = task.Result;
            Assert.Equal(2, context.Iteration);
        }

        [Fact]
        public void Execute_runs_func_returns_context_with_elapsed_time()
        {
            TimeSpan funcElapsed = TimeSpan.MaxValue;
            Func<RetryContext, Task> func = delegate(RetryContext r)
            {
                funcElapsed = r.ElapsedTime;
                return Task.FromResult(false);
            };

            RetryLoop loop = new RetryLoop(func);

            TimeSpan shouldRetryElapsed = TimeSpan.MaxValue;
            loop.ShouldRetry = delegate(RetryContext r)
            {
                shouldRetryElapsed = r.ElapsedTime;
                return false;
            };

            TimeSpan succeededElapsed = TimeSpan.MaxValue;
            loop.Succeeded = delegate(RetryContext r)
            {
                succeededElapsed = r.ElapsedTime;
                return false;
            };

            ElapsedTimerStub timer = new ElapsedTimerStub();
            loop.Timer = timer;

            timer.ElapsedTimes.Enqueue(TimeSpan.FromSeconds(1.0d));
            timer.ElapsedTimes.Enqueue(TimeSpan.FromSeconds(2.5d));
            timer.ElapsedTimes.Enqueue(TimeSpan.FromSeconds(4.0d));
            Task<RetryContext> task = loop.ExecuteAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(TimeSpan.FromSeconds(1.5d), funcElapsed);
            RetryContext context = task.Result;
            Assert.Equal(1, context.Iteration);
            Assert.Equal(TimeSpan.FromSeconds(3.0d), succeededElapsed);
            Assert.Equal(TimeSpan.FromSeconds(3.0d), shouldRetryElapsed);
            Assert.Equal(TimeSpan.FromSeconds(3.0d), context.ElapsedTime);
        }

        [Fact]
        public void Execute_runs_func_until_succeeded()
        {
            int count = 0;
            RetryLoop loop = new RetryLoop(r => Task.FromResult(++count));
            loop.Succeeded = r => r.Iteration == 1;
            loop.ShouldRetry = r => true;

            Task<RetryContext> task = loop.ExecuteAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            RetryContext context = task.Result;
            Assert.Equal(2, context.Iteration);
            Assert.Equal(2, count);
            Assert.True(context.Succeeded);
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
