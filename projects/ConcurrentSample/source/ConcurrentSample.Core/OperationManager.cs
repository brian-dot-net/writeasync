//-----------------------------------------------------------------------
// <copyright file="OperationManager.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConcurrentSample
{
    using System;
    using System.Collections.Generic;
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
            Queue<Task> pending = new Queue<Task>();
            List<Exception> exceptions = new List<Exception>();
            for (int i = 0; i < totalCalls; ++i)
            {
                await semaphore.WaitAsync();
                pending.Enqueue(this.CallAsync(semaphore, i));
                HandleCompletedCalls(pending, exceptions);
                if (exceptions.Count > 0)
                {
                    throw new AggregateException(exceptions).Flatten();
                }
            }

            await Task.WhenAll(pending);
        }

        private static void HandleCompletedCalls(Queue<Task> pending, IList<Exception> exceptions)
        {
            while (pending.Count > 0)
            {
                Task task = pending.Peek();
                if (task.IsCompleted)
                {
                    pending.Dequeue();
                    if (task.Exception != null)
                    {
                        exceptions.Add(task.Exception);
                    }
                }
                else
                {
                    return;
                }
            }
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
