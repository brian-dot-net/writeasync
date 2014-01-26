//-----------------------------------------------------------------------
// <copyright file="InputQueue.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace QueueSample
{
    using System;
    using System.Threading.Tasks;

    public class InputQueue<T>
    {
        private TaskCompletionSource<T> pending;

        public InputQueue()
        {
        }

        public Task<T> DequeueAsync()
        {
            if (this.pending == null)
            {
                this.pending = new TaskCompletionSource<T>();
            }

            return this.pending.Task;
        }

        public void Enqueue(T item)
        {
            if (this.pending == null)
            {
                this.pending = new TaskCompletionSource<T>();
            }

            this.pending.SetResult(item);
        }
    }
}
