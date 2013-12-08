//-----------------------------------------------------------------------
// <copyright file="OperationManager.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConcurrentSample
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class OperationManager
    {
        private readonly Func<Task> doAsync;
        private readonly int maxPendingCalls;

        public OperationManager(int maxPendingCalls, Func<Task> doAsync)
        {
            this.doAsync = doAsync;
            this.maxPendingCalls = maxPendingCalls;
        }

        public async Task RunAsync(int totalCalls)
        {
            AsyncSemaphore semaphore = new AsyncSemaphore(this.maxPendingCalls);
            List<Task> pending = new List<Task>();
            for (int i = 0; i < totalCalls; ++i)
            {
                await semaphore.WaitAsync();
                pending.Add(this.CallAsync(semaphore, i));
            }

            await Task.WhenAll(pending);
        }

        private async Task CallAsync(AsyncSemaphore semaphore, int i)
        {
            try
            {
                await this.doAsync();
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
