//-----------------------------------------------------------------------
// <copyright file="LoopingSchedulerTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventHandlerSample.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LoopingSchedulerTest
    {
        public LoopingSchedulerTest()
        {
        }

        [TestMethod]
        public void RunThrowsOnFirstException()
        {
            Exception exception = new InvalidOperationException("Expected.");
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            tcs.SetException(exception);
            LoopingScheduler scheduler = new LoopingScheduler(() => tcs.Task);
            
            Task task = scheduler.RunAsync(TimeSpan.MaxValue);

            AssertTaskThrows(task, exception);
        }

        [TestMethod]
        public void RunExecutesTasksUntilException()
        {
            Exception exception = new InvalidOperationException("Expected.");
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            tcs.SetException(exception);
            Task[] tasks = new Task[]
            {
                Task.FromResult(false),
                Task.FromResult(true),
                tcs.Task
            };

            int invokeCount = 0;
            LoopingScheduler scheduler = new LoopingScheduler(() => tasks[invokeCount++]);

            Task task = scheduler.RunAsync(TimeSpan.MaxValue);

            AssertTaskThrows(task, exception);
            invokeCount.Should().Be(3);
        }

        [TestMethod]
        public void RunRaisesPausedAfterFinitePauseInterval()
        {
            Exception exception = new InvalidOperationException("Expected.");
            int invokeCount = 0;
            LoopingScheduler scheduler = new LoopingScheduler(() => Task.FromResult(++invokeCount));
            scheduler.GetElapsed = () => TimeSpan.FromSeconds(invokeCount);
            scheduler.Paused += delegate
            {
                throw exception;
            };

            Task task = scheduler.RunAsync(TimeSpan.FromSeconds(1.0d));

            AssertTaskThrows(task, exception);
            invokeCount.Should().Be(1);
        }

        [TestMethod]
        public void RunRaisesPausedAfterFinitePauseIntervalTwoIterationsLater()
        {
            Exception exception = new InvalidOperationException("Expected.");
            int invokeCount = 0;
            LoopingScheduler scheduler = new LoopingScheduler(() => Task.FromResult(++invokeCount));
            scheduler.GetElapsed = () => TimeSpan.FromSeconds(invokeCount);
            scheduler.Paused += delegate
            {
                throw exception;
            };

            Task task = scheduler.RunAsync(TimeSpan.FromSeconds(2.0d));

            AssertTaskThrows(task, exception);
            invokeCount.Should().Be(2);
        }

        [TestMethod]
        public void RunRaisesPausedAfterFinitePauseIntervalRelativeToNonZeroElapsedTwoIterationsLater()
        {
            Exception exception = new InvalidOperationException("Expected.");
            int invokeCount = 0;
            LoopingScheduler scheduler = new LoopingScheduler(() => Task.FromResult(++invokeCount));
            scheduler.GetElapsed = () => TimeSpan.FromSeconds(1 + invokeCount);
            scheduler.Paused += delegate
            {
                throw exception;
            };

            Task task = scheduler.RunAsync(TimeSpan.FromSeconds(2.0d));

            AssertTaskThrows(task, exception);
            invokeCount.Should().Be(2);
        }

        [TestMethod]
        public void RunRaisesPausedTwiceOnTwoElapsedPauseIntervalsOfTwoSeconds()
        {
            Exception exception = new InvalidOperationException("Expected.");
            int pauseInvokeCount = 0;
            int invokeCount = 0;
            LoopingScheduler scheduler = new LoopingScheduler(() => Task.FromResult(++invokeCount));
            scheduler.GetElapsed = () => TimeSpan.FromSeconds(invokeCount);
            scheduler.Paused += delegate
            {
                if (++pauseInvokeCount == 2)
                {
                    throw exception;
                }
            };

            Task task = scheduler.RunAsync(TimeSpan.FromSeconds(2.0d));

            AssertTaskThrows(task, exception);
            invokeCount.Should().Be(4);
            pauseInvokeCount.Should().Be(2);
        }

        private static void AssertTaskThrows(Task task, Exception expected)
        {
            task.IsCompleted.Should().BeTrue();
            task.Exception.Should().NotBeNull();
            task.Exception.InnerExceptions.Should().HaveCount(1).And.Contain(expected);
        }
    }
}
