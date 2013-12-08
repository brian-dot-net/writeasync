//-----------------------------------------------------------------------
// <copyright file="AsyncSemaphore.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConcurrentSample
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AsyncSemaphore
    {
        private static readonly Task CompletedTask = Task.FromResult(true);

        private readonly Queue<TaskCompletionSource<bool>> waiters;

        private int currentCount;

        public AsyncSemaphore(int initialCount)
        {
            if (initialCount < 0)
            {
                throw new ArgumentOutOfRangeException("initialCount");
            }

            this.currentCount = initialCount;
            this.waiters = new Queue<TaskCompletionSource<bool>>();
        }

        public Task WaitAsync()
        {
            lock (this.waiters)
            {
                if (this.currentCount > 0)
                {
                    --this.currentCount;
                    return CompletedTask;
                }
                else
                {
                    TaskCompletionSource<bool> waiter = new TaskCompletionSource<bool>();
                    this.waiters.Enqueue(waiter);
                    return waiter.Task;
                }
            }
        }

        public void Release()
        {
            TaskCompletionSource<bool> toRelease = null;
            lock (this.waiters)
            {
                if (this.waiters.Count > 0)
                {
                    toRelease = this.waiters.Dequeue();
                }
                else
                {
                    ++this.currentCount;
                }
            }
            
            if (toRelease != null)            
            {
                toRelease.SetResult(true);
            }
        }
    }
}
