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
        public void TwoItemsFirstHangsUntilCancelSecondMatchesSyncReturns()
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            IEnumerable<Func<CancellationToken, Task<string>>> funcs = new Func<CancellationToken, Task<string>>[]
            {
                t =>
                {
                    t.Register(() => tcs.SetCanceled());
                    return tcs.Task;
                },
                t => Task.FromResult("good 2")
            };

            Task<string> task = funcs.FirstAsync(r => r.StartsWith("good", StringComparison.Ordinal));

            task.IsCompletedSuccessfully.Should().BeTrue();
            task.Result.Should().Be("good 2");
        }
    }
}
