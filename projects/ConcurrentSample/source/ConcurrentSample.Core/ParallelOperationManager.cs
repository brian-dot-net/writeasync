//-----------------------------------------------------------------------
// <copyright file="ParallelOperationManager.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConcurrentSample
{
    using System;
using System.Threading.Tasks;

    public class ParallelOperationManager
    {
        private readonly Func<Task> doAsync;
        private readonly int maxPendingCalls;

        public ParallelOperationManager(int maxPendingCalls, Func<Task> doAsync)
        {
            this.doAsync = doAsync;
            this.maxPendingCalls = maxPendingCalls;
        }

        public Task RunAsync(int totalCalls)
        {
            int totalCallsPerLoop = totalCalls / this.maxPendingCalls;
            Task[] loopTasks = new Task[this.maxPendingCalls];
            for (int i = 0; i < this.maxPendingCalls; ++i)
            {
                loopTasks[i] = this.RunLoopAsync(totalCallsPerLoop);
            }

            return Task.WhenAll(loopTasks);
        }

        private async Task RunLoopAsync(int totalCalls)
        {
            for (int i = 0; i < totalCalls; ++i)
            {
                await this.doAsync();
            }
        }
    }
}
