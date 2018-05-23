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
    }
}
