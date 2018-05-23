// <copyright file="FirstAsyncTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace TaskSample.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TaskSample.Extensions;
    using Xunit;

    public class FirstAsyncTest
    {
        [Fact]
        public void OneItemMatchesSyncReturns()
        {
            IEnumerable<Func<CancellationToken, Task<string>>> funcs = new Func<CancellationToken, Task<string>>[]
            {
                t => Task.FromResult("good")
            };

            Task<string> task = funcs.FirstAsync(r => true);

            task.IsCompletedSuccessfully.Should().BeTrue();
            task.Result.Should().Be("good");
        }

        [Fact]
        public void OneItemMatchesAsyncReturns()
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            IEnumerable<Func<CancellationToken, Task<string>>> funcs = new Func<CancellationToken, Task<string>>[]
            {
                t => tcs.Task
            };

            Task<string> task = funcs.FirstAsync(r => true);

            task.IsCompleted.Should().BeFalse();

            tcs.SetResult("good");

            task.IsCompletedSuccessfully.Should().BeTrue();
            task.Result.Should().Be("good");
        }

        [Fact]
        public void OneItemThrowsSyncThrowsInvalidOperation()
        {
            IEnumerable<Func<CancellationToken, Task<string>>> funcs = new Func<CancellationToken, Task<string>>[]
            {
                t => { throw new BadImageFormatException("!!!"); }
            };

            Task<string> task = funcs.FirstAsync(r => true);

            ShouldBeFaulted(task);
        }

        [Fact]
        public void OneItemThrowsAsyncThrowsInvalidOperation()
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            IEnumerable<Func<CancellationToken, Task<string>>> funcs = new Func<CancellationToken, Task<string>>[]
            {
                t => tcs.Task
            };

            Task<string> task = funcs.FirstAsync(r => true);

            task.IsCompleted.Should().BeFalse();

            tcs.SetException(new BadImageFormatException("!!!"));

            task.IsCompleted.Should().BeTrue();
            task.Exception.Should().NotBeNull();
            task.Exception.InnerException.Should()
                .BeOfType<InvalidOperationException>().Which
                .Message.Should().Be("No matching result.");
        }

        [Fact]
        public void TwoItemsFirstMatchesSyncReturns()
        {
            IEnumerable<Func<CancellationToken, Task<string>>> funcs = new Func<CancellationToken, Task<string>>[]
            {
                t => Task.FromResult("good 1"),
                t => Task.FromResult("good 2")
            };

            Task<string> task = funcs.FirstAsync(r => true);

            task.IsCompletedSuccessfully.Should().BeTrue();
            task.Result.Should().Be("good 1");
        }

        [Fact]
        public void TwoItemsSecondMatchesSyncReturns()
        {
            IEnumerable<Func<CancellationToken, Task<string>>> funcs = new Func<CancellationToken, Task<string>>[]
            {
                t => Task.FromResult("not good"),
                t => Task.FromResult("good 2")
            };

            Task<string> task = funcs.FirstAsync(r => r.StartsWith("good", StringComparison.Ordinal));

            task.IsCompletedSuccessfully.Should().BeTrue();
            task.Result.Should().Be("good 2");
        }

        [Fact]
        public void TwoItemsSyncNoMatchesThrowsInvalidOperation()
        {
            IEnumerable<Func<CancellationToken, Task<string>>> funcs = new Func<CancellationToken, Task<string>>[]
            {
                t => Task.FromResult("bad 1"),
                t => Task.FromResult("bad 2")
            };

            Task<string> task = funcs.FirstAsync(r => r.StartsWith("good", StringComparison.Ordinal));

            ShouldBeFaulted(task);
        }

        [Fact]
        public void TwoItemsFirstHangsUntilCancelSecondMatchesSyncReturns()
        {
            IEnumerable<Func<CancellationToken, Task<string>>> funcs = new Func<CancellationToken, Task<string>>[]
            {
                t =>
                {
                    TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
                    t.Register(() => tcs.SetCanceled());
                    return tcs.Task;
                },
                t => Task.FromResult("good 2")
            };

            Task<string> task = funcs.FirstAsync(r => r.StartsWith("good", StringComparison.Ordinal));

            task.IsCompletedSuccessfully.Should().BeTrue();
            task.Result.Should().Be("good 2");
        }

        [Fact]
        public void ThreeItemsNoMatchExceptLastAsyncReturns()
        {
            TaskCompletionSource<string> tcs1 = new TaskCompletionSource<string>();
            TaskCompletionSource<string> tcs2 = new TaskCompletionSource<string>();
            TaskCompletionSource<string> tcs3 = new TaskCompletionSource<string>();
            IEnumerable<Func<CancellationToken, Task<string>>> funcs = new Func<CancellationToken, Task<string>>[]
            {
                t => tcs1.Task,
                t => tcs2.Task,
                t => tcs3.Task
            };

            Task<string> task = funcs.FirstAsync(r => r.StartsWith("good", StringComparison.Ordinal));

            task.IsCompleted.Should().BeFalse();

            tcs1.SetResult("bad 1");

            task.IsCompleted.Should().BeFalse();

            tcs2.SetResult("bad 2");

            task.IsCompleted.Should().BeFalse();

            tcs3.SetResult("good 3");

            task.IsCompletedSuccessfully.Should().BeTrue();
            task.Result.Should().Be("good 3");
        }

        private static void ShouldBeFaulted(Task task)
        {
            task.IsCompleted.Should().BeTrue();
            task.Exception.Should().NotBeNull();
            task.Exception.InnerException.Should()
                .BeOfType<InvalidOperationException>().Which
                .Message.Should().Be("No matching result.");
        }
    }
}
